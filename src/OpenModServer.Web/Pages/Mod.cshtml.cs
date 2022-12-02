using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Core.Structures;
using OpenModServer.Core.Structures.Releases;
using OpenModServer.Core.Structures.Releases.Approvals;
using OpenModServer.Web.Data;

namespace OpenModServer.Web.Pages;

public class Mod : PageModel
{
    public ModListing Listing { get; set; }
    private readonly ApplicationDbContext _database;
    private readonly ILogger<Mod> _logger;
    public Mod(ILogger<Mod> logger, ApplicationDbContext database)
    {
        _database = database;
        _logger = logger;
    }
    public async Task OnGet()
    {
        Listing = await _database.ModListings
            .Include(d => d.Releases)
            .ThenInclude(d => d.ApprovalHistory)
            .FirstOrDefaultAsync(d => d.Id == Guid.Parse(RouteData.Values["id"].ToString()));
    }
}