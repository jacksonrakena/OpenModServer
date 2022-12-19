using System.Net.Http.Headers;
using System.Text.Json;
using OpenModServer.Data;
using OpenModServer.Data.Releases;
using OpenModServer.Data.Releases.Safety;
using OpenModServer.Structures;

namespace OpenModServer.Services.Safety;

public partial class ExternalScanningService
{
    private readonly IServiceProvider _provider;
    private readonly string _vtKey;
    private readonly HttpClient _vtHttpClient;
    private readonly ILogger<ExternalScanningService> _logger;
    
    public bool IsScanningDisabled { get; }
    
    public ExternalScanningService(
        IServiceProvider serviceProvider, 
        OmsConfig config,
        ILogger<ExternalScanningService> logger,
        IHttpClientFactory clientFactory)
    {
        _vtHttpClient = clientFactory.CreateClient("VirusTotal");
        _provider = serviceProvider;
        _logger = logger;
        _vtKey = config.Keys.VirusTotal;
        _vtHttpClient.DefaultRequestHeaders.Add("x-apikey", _vtKey);

        IsScanningDisabled = string.IsNullOrWhiteSpace(_vtKey);
        
        if (IsScanningDisabled)
            _logger.LogError("An invalid VirusTotal key has been provided. Scanning will be disabled.");
        else _logger.LogInformation("Found valid VirusTotal key.");
    }
    
    /// <summary>
    ///     This method will upload a mod release file to VirusTotal for scanning.
    ///     This method will block during the upload process, so it is recommended
    ///     not to await this method during sensitive code.
    /// </summary>
    public async Task EnqueueReleaseForScanningAsync(ModRelease release)
    {
        using var operationScope = _provider.CreateScope();
        var services = operationScope.ServiceProvider;
        await using var database = services.GetRequiredService<ApplicationDbContext>();

        var url = await _vtHttpClient.GetAsync(_getLargeUploadUrl);
        // TODO: not this
        url.EnsureSuccessStatusCode();

        var uploadUrl = (await url.Content.ReadFromJsonAsync<JsonDocument>())?.RootElement.GetProperty("data")
            .GetString();

        if (string.IsNullOrWhiteSpace(uploadUrl))
            throw new Exception("VirusTotal didn't give us a URL to upload our file.");
        
        // Upload file
        var request = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
        
        // Open file and create form data
        await using var stream = File.OpenRead(release.FilePath);
        using var content = new MultipartFormDataContent();
        using var streamContent = new StreamContent(stream);
        var disposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = release.FileName,
            Size = stream.Length,
            FileNameStar = null
        };  
        streamContent.Headers.ContentDisposition = disposition;
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(streamContent);
        request.Content = content;

        var response = await _vtHttpClient.SendAsync(request);
        _logger.LogInformation(await response.Content.ReadAsStringAsync());
        
        // TODO: also not this
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadFromJsonAsync<JsonDocument>();
        var analysisId = responseJson?.RootElement.GetProperty("data").GetProperty("id").GetString();
        
        if (string.IsNullOrWhiteSpace(analysisId))
            throw new Exception("VirusTotal didn't accept the upload.");

        var entry = database.ModReleases.Update(release);
        entry.Entity.VT_AnalysisId = analysisId;
        entry.Entity.VT_SubmittedAt = DateTime.Now;
        entry.Entity.VT_ScanResult = VirusTotalScanStatus.WaitingOnVirusTotalToComplete;
        await database.SaveChangesAsync();
    }
}