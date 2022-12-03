using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data;
using OpenModServer.Structures;

namespace OpenModServer.Pages;

public class Mod : PageModel
{
    [NotNull]
    public ModListing? Listing { get; set; }
    private readonly ApplicationDbContext _database;
    private readonly ILogger<Mod> _logger;
    public Mod(ILogger<Mod> logger, ApplicationDbContext database)
    {
        _database = database;
        _logger = logger;
    }
    public async Task<IActionResult> OnGet()
    {
        if (!Guid.TryParse(RouteData.Values["id"]?.ToString(), out var guid)) return NotFound();
        var query = await _database.ModListings
            .Include(c => c.Creator)
            .Include(d => d.Releases)
            .ThenInclude(d => d.ApprovalHistory)
            .FirstOrDefaultAsync(d => d.Id == guid);
        if (query == null) return NotFound();
        Listing = query;
        return Page();
    }
}