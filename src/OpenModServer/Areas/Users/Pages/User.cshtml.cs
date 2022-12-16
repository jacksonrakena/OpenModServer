using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Areas.Mods.Pages;
using OpenModServer.Data;
using OpenModServer.Data.Comments;
using OpenModServer.Data.Identity;
using OpenModServer.Structures;

namespace OpenModServer.Areas.Users.Pages;

public class UserModel : PageModel
{
    public OmsUser User { get; set; }
    public List<ModListing> Mods { get; set; }
    public List<ModComment> Comments { get; set; }
    private readonly ApplicationDbContext _database;
    private readonly ILogger<UserModel> _logger;
    public UserModel(ILogger<UserModel> logger, ApplicationDbContext database)
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
        User = user;
        Mods = await _database.ModListings.Where(d => d.CreatorId == guid).ToListAsync();
        Comments = await _database.Comments
            .OrderByDescending(d => d.CreatedAt)
            .Include(d => d.Listing)
            .Where(d => d.AuthorId == guid)
            .Take(20)
            .ToListAsync();
        return Page();
    }
}