using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data;
using OpenModServer.Structures;

namespace OpenModServer.Areas.Mods.Pages;

public class ViewAllModsModel : PageModel
{
    private readonly ILogger<ViewAllModsModel> _logger;
    internal List<IGrouping<string, ModListing>>? ModListings;
    private readonly ApplicationDbContext _database;
    
    public ModListing? Listing { get; set; }

    public ViewAllModsModel(ILogger<ViewAllModsModel> logger, ApplicationDbContext database)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        if (Guid.TryParse(RouteData.Values["id"]?.ToString(), out var modId))
        {
            var query = await _database.ModListings
                .Include(c => c.Creator)
                .Include(d => d.Releases)
                .FirstOrDefaultAsync(d => d.Id == modId);
            if (query == null) return NotFound();
            Listing = query;
            return Page();
        }
        
        ModListings = await _database.ModListings
            .Include(e => e.Creator)
            .OrderByDescending(t => t.DownloadCount)
            .Take(50)
            .GroupBy(d => d.GameIdentifier)
            .ToListAsync();
        return Page();
    }
}