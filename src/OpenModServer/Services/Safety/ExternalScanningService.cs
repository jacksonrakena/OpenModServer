using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
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
        
        // Because Google is possibly the worst technology company on Earth, we have to hardcode Content Disposition:
        streamContent.Headers.Add("Content-Disposition", "form-data;name=\"file\";filename=\""+release.FileName+"\"");

        // If Google had any competence, we could just do this instead, follow the standard:
        // streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = release.FileName };
        
        content.Add(streamContent);
        request.Content = content;

        var response = await _vtHttpClient.SendAsync(request);
        
        // TODO: also not this
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadFromJsonAsync<JsonDocument>();
        var analysisId = responseJson?.RootElement.GetProperty("data").GetProperty("id").GetString();
        
        if (string.IsNullOrWhiteSpace(analysisId))
            throw new Exception("VirusTotal didn't accept the upload.");

        var entry = database.ModReleases.Update(release);
        entry.Entity.VT_AnalysisId = analysisId;
        entry.Entity.VT_SubmittedAt = DateTime.Now.ToUniversalTime();
        entry.Entity.VT_LastUpdatedAt = DateTime.Now.ToUniversalTime();
        entry.Entity.VT_ScanResult = VirusTotalScanStatus.WaitingOnVirusTotalToComplete;
        await database.SaveChangesAsync();
    }
    
    public async Task ExecuteIterationAsync(CancellationToken cancellationToken)
    {
        using var operationScope = _provider.CreateScope();
        var services = operationScope.ServiceProvider;
        await using var database = services.GetRequiredService<ApplicationDbContext>();
        
        // Fetch all stored releases where we're still waiting on a VT result
        var releases = await database.ModReleases
            .Where(c => c.VT_AnalysisId != null
                        && c.VT_ScanResult == VirusTotalScanStatus.WaitingOnVirusTotalToComplete).ToListAsync(cancellationToken: cancellationToken);

        if (releases.Count == 0) return;
        _logger.LogInformation("Beginning VirusTotal update round for {Count} releases.", releases.Count);
        
        foreach (var release in releases)
        {
            try
            {
                _logger.LogInformation("Beginning update check for release {Id}", release.Id);
                var response =
                    await _vtHttpClient.GetAsync(_getAnalysisUrl(release.VT_AnalysisId), cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed update check for {Id}: {ReasonPhrase}", release.Id,
                        response.ReasonPhrase);
                    continue;
                }

                var responseJson =
                    await response.Content.ReadFromJsonAsync<JsonDocument>(cancellationToken: cancellationToken);
                var attributes = responseJson.RootElement.GetProperty("data").GetProperty("attributes");
                var status = attributes.GetProperty("status").GetString();
                if (status != "completed")
                {
                    release.VT_LastUpdatedAt = DateTime.Now.ToUniversalTime();
                    _logger.LogInformation("Ending update check for {Id}, status is {status}", release.Id, status);
                    continue;
                }

                var stats = attributes.GetProperty("stats");
                release.VT_NumberOfHarmlessScans = stats.GetProperty("harmless").GetInt32() +
                                                   stats.GetProperty("undetected").GetInt32();
                release.VT_NumberOfFailedScans = stats.GetProperty("failure").GetInt32();
                release.VT_NumberOfMaliciousScans = stats.GetProperty("malicious").GetInt32();
                release.VT_NumberOfSuspiciousScans = stats.GetProperty("suspicious").GetInt32();
                release.VT_LastUpdatedAt = DateTime.Now.ToUniversalTime();
                release.VT_ScanResult =
                    (release.VT_NumberOfMaliciousScans > 0 || release.VT_NumberOfSuspiciousScans > 0)
                        ? VirusTotalScanStatus.Dirty
                        : VirusTotalScanStatus.Clean;
                
                _logger.LogInformation("Ending successful update check for {Id}, new scan status is {VT_ScanResult}", release.Id, release.VT_ScanResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception during update check for {Id}", release.Id);
            }
            
        }

        var written = await database.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Ending VirusTotal update round - {written} state entries written.", written);
    }
}