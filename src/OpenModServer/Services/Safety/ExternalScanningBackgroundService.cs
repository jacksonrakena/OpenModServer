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
    private readonly TimeSpan _runDelay = TimeSpan.FromMinutes(2);
    
    
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
            await _externalScanningService.ExecuteIterationAsync(stoppingToken);
            await Task.Delay(_runDelay, stoppingToken);
        }
    }
}