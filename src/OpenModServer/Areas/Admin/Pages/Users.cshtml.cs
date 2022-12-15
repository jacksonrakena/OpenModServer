using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data;
using OpenModServer.Data.Identity;
using OpenModServer.Data.Releases;
using OpenModServer.Data.Releases.Approvals;
using OpenModServer.Services;

namespace OpenModServer.Areas.Admin.Pages;

[Authorize(Roles = "Administrator,ApprovalModerator")]
public class Users : PageModel
{
    public List<OmsUser> UserAccounts { get; set; }

    private readonly UserManager<OmsUser> _userManager;
    private readonly FileManagerService _fileManagerService;

    public Users(ApplicationDbContext context, FileManagerService fileManagerService, UserManager<OmsUser> userManager)
    {
        _database = context;
        _userManager = userManager;
        _fileManagerService = fileManagerService;
    }

    private readonly ApplicationDbContext _database;
    public async Task OnGetAsync()
    {
        UserAccounts = await _database.Users.ToListAsync();
    }
    
    // public async Task<IActionResult> OnPostHandleDeletionAsync(string id)
    // {
    //     if (!Guid.TryParse(id, out var listing)) return RedirectToPage("/Files", new { Area = "Admin" });
    //     var release = await _database.ModListings.FirstOrDefaultAsync(d => d.Id == listing);
    //     if (release == null) return RedirectToPage("/Files", new { Area = "Admin" });
    //     _database.ModListings.Remove(release);
    //     
    //     await _database.SaveChangesAsync();
    //     
    //     Users = await _database.ModListings.ToListAsync();
    //     return Page();
    // }
}