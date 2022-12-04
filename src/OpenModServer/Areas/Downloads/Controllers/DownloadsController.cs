using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data;
using OpenModServer.Structures.Releases.Approvals;

namespace OpenModServer.Areas.Downloads.Controllers;

[Controller, Route("/downloads")]
[Area("Downloads")]
public class DownloadController : Controller
{
    private readonly ILogger<DownloadController> _logger;
    private readonly ApplicationDbContext _database;

    public DownloadController(ILogger<DownloadController> logger, ApplicationDbContext database)
    {
        _database = database;
        _logger = logger;
    }
    
    [HttpGet("releases/{id}")]
    public async Task<IActionResult> DownloadReleaseById([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var guid)) return BadRequest();
        var release = await _database.ModReleases.FirstOrDefaultAsync(r => r.Id == guid);
        if (release is not { CurrentStatus: ModReleaseApprovalStatus.Approved }) return NotFound();
        release.DownloadCount++;
        _ = _database.SaveChangesAsync();
        var stream = System.IO.File.OpenRead(release.FilePath);
        return new FileStreamResult(stream, "application/octet-stream")
        { 
            LastModified = release.CreatedAt,
            FileDownloadName = release.FileName
        };
    }
}