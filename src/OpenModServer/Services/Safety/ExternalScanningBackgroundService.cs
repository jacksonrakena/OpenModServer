using System.Net.Http.Headers;
using System.Text.Json;
using OpenModServer.Data;
using OpenModServer.Data.Releases;
using OpenModServer.Data.Releases.Safety;
using OpenModServer.Structures;

namespace OpenModServer.Services.Safety;

/// <summary>
///     A background service that continuously checks releases that need to be posted to
///     VirusTotal for detection.
/// </summary>
public partial class ExternalScanningBackgroundService : BackgroundService
{
    private readonly TimeSpan _runDelay = TimeSpan.FromMinutes(5);
    
    
    private readonly IServiceProvider _provider;
    private readonly ExternalScanningService _externalScanningService;
    private readonly ILogger<ExternalScanningBackgroundService> _logger;
    public ExternalScanningBackgroundService(
        IServiceProvider serviceProvider, 
        OmsConfig config,
        ILogger<ExternalScanningBackgroundService> logger,
        ExternalScanningService parent)
    {
        _externalScanningService = parent;
        _provider = serviceProvider;
        _logger = logger;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ExecuteIterationAsync(stoppingToken);
            await Task.Delay(_runDelay, stoppingToken);
        }
    }

    private async Task ExecuteIterationAsync(CancellationToken cancellationToken)
    {
        using var operationScope = _provider.CreateScope();
        var services = operationScope.ServiceProvider;
        await using var database = services.GetRequiredService<ApplicationDbContext>();
        
        // Fetch all stored releases where we're still waiting on a VT result
        var releases = database.ModReleases
            .Where(c => c.VT_AnalysisId != null
                        && c.VT_ScanResult == VirusTotalScanStatus.WaitingOnVirusTotalToComplete);

        foreach (var release in releases)
        {
            // Ask VirusTotal for an update
        }

        await database.SaveChangesAsync(cancellationToken);
    }
}