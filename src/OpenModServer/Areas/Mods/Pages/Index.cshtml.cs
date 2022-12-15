using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data;
using OpenModServer.Data.Comments;
using OpenModServer.Data.Identity;
using OpenModServer.Structures;

namespace OpenModServer.Areas.Mods.Pages;

public class ViewAllModsModel : PageModel
{
    private readonly ILogger<ViewAllModsModel> _logger;
    internal List<IGrouping<string, ModListing>>? ModListings;
    private readonly ApplicationDbContext _database;
    private readonly UserManager<OmsUser> _userManager;

    public ModListing? Listing { get; set; }

    public ViewAllModsModel(ILogger<ViewAllModsModel> logger, ApplicationDbContext database, UserManager<OmsUser> user)
    {
        _database = database;
        _userManager = user;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        if (Guid.TryParse(RouteData.Values["id"]?.ToString(), out var modId))
        {
            var query = await _database.ModListings
                .Include(c => c.Creator)
                .Include(d => d.Releases)
                .Include(c => c.Comments)
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
    
    public async Task<IActionResult> OnPostCreateCommentAsync(string modId, string comment)
    {
        var user = await _userManager.GetUserAsync(User);
        if (string.IsNullOrWhiteSpace(modId) || string.IsNullOrWhiteSpace(comment) || user == null)
        {
            return await OnGet();
        }

        _database.Comments.Add(new ModComment
            { AuthorId = user.Id, ModListingId = Guid.Parse(modId), Content = comment });
        await _database.SaveChangesAsync();

        return await OnGet();
    }
}