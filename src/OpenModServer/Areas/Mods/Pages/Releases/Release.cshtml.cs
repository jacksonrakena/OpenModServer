using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data;
using OpenModServer.Data.Releases;

namespace OpenModServer.Areas.Mods.Pages.Releases;

public class ReleaseModel : PageModel
{
    public ModRelease Release { get; set; }
    private readonly ApplicationDbContext _database;
    private readonly ILogger<ReleaseModel> _logger;
    public ReleaseModel(ILogger<ReleaseModel> logger, ApplicationDbContext database)
    {
        _database = database;
        _logger = logger;
    }
    public async Task OnGet()
    {
        Release = await _database.ModReleases
            .Include(d => d.ApprovalHistory)
            .FirstOrDefaultAsync(d => d.Id == Guid.Parse(RouteData.Values["release_id"].ToString()));
    }
}