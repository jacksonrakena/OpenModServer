using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data;
using OpenModServer.Data.Identity;

namespace OpenModServer.Areas.Mods.Pages.Tags;

public class ViewTag : PageModel
{
    private readonly ILogger<ViewAllModsModel> _logger;
    internal List<IGrouping<string, ModListing>>? ModListings;
    private readonly ApplicationDbContext _database;

    public string Name { get; set; }

    public ViewTag(ILogger<ViewAllModsModel> logger, ApplicationDbContext database)
    {
        _database = database;
        _logger = logger;
    }
    public async Task<IActionResult> OnGetAsync()
    {
        var name= RouteData.Values["id"]?.ToString();
        if (string.IsNullOrWhiteSpace(name)) return NotFound();

        Name = name;
        ModListings = await _database.ModListings.Where(m => m.Tags.Contains(Name)).Take(20)
            .GroupBy(d => d.GameIdentifier).ToListAsync();

        return Page();
    }
}