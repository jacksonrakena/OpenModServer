using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Core.Structures;
using OpenModServer.Core.Structures.Releases;
using OpenModServer.Core.Structures.Releases.Approvals;
using OpenModServer.Web.Data;

namespace OpenModServer.Web.Pages;

public class ReleaseModel : PageModel
{
    public ModRelease Release { get; set; }
    private readonly ApplicationDbContext _database;
    private readonly ILogger<Mod> _logger;
    public ReleaseModel(ILogger<Mod> logger, ApplicationDbContext database)
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