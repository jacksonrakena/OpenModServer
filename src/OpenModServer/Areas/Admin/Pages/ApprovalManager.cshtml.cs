using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Areas.Identity;
using OpenModServer.Data;
using OpenModServer.Data.Identity;
using OpenModServer.Data.Releases;
using OpenModServer.Services;

namespace OpenModServer.Areas.Admin.Pages;

[Authorize(Policy = nameof(Permissions.ApproveReleases))]
public class ApprovalManager : PageModel
{
    private readonly ApplicationDbContext _database;
    private readonly UserManager<OmsUser> _userManager;
    private readonly FileManagerService _fileManagerService;

    public ModRelease Release { get; set; }
    
    public ApprovalManager(ApplicationDbContext context, FileManagerService fileManagerService, UserManager<OmsUser> userManager)
    {
        _database = context;
        _userManager = userManager;
        _fileManagerService = fileManagerService;
    }
    public async Task<IActionResult> OnGetAsync()
    {
        IActionResult ErrorRedirect() => RedirectToPage("/Mods", new { Area = "Admin" });
        
        if (Guid.TryParse(RouteData.Values["id"]?.ToString(), out var releaseId))
        {
            var release = await _database
                .ModReleases
                .Include(t => t.ApprovalHistory)
                .ThenInclude(t => t.ModeratorResponsible)
                .Include(t => t.ModListing)
                .ThenInclude(c => c.Creator)
                .FirstOrDefaultAsync(d => d.Id == releaseId);
            if (release == null) return ErrorRedirect();
            
            Release = release;
            
            return Page();
        }

        return ErrorRedirect();
    }
}