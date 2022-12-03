using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data;
using OpenModServer.Identity;
using OpenModServer.Structures;

namespace OpenModServer.Pages;

public class UserModel : PageModel
{
    public OmsUser User { get; set; }
    public List<ModListing> Mods { get; set; }
    private readonly ApplicationDbContext _database;
    private readonly ILogger<Mod> _logger;
    public UserModel(ILogger<Mod> logger, ApplicationDbContext database)
    {
        _database = database;
        _logger = logger;
    }
    public async Task<IActionResult> OnGet()
    {
        if (!Guid.TryParse(RouteData.Values["id"]?.ToString(), out var guid)) return NotFound();
        var user = await _database.Users
            .FirstOrDefaultAsync(d => d.Id == guid);
        if (user == null) return NotFound();
        Mods = await _database.ModListings.Where(d => d.CreatorId == guid).ToListAsync();
        return Page();
    }
}