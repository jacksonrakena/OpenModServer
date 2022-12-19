using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Areas.Account;
using OpenModServer.Data;
using OpenModServer.Data.Identity;
using OpenModServer.Data.Releases;
using OpenModServer.Data.Releases.Approvals;
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

    public async Task<IActionResult> HandleStateChangeAsync(string id, ModReleaseApprovalStatus newStatus)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !Guid.TryParse(id, out var releaseId)) return RedirectToPage();
        var release = await _database.ModReleases
            .Include(c =>c.ApprovalHistory)
            .Include(d => d.ModListing)
            .FirstOrDefaultAsync(d => d.Id == releaseId);
        if (release == null) return RedirectToPage();

        if (newStatus == ModReleaseApprovalStatus.Approved) release.ModListing.IsVisibleToPublic = true;
        var originalState = release.CurrentStatus;
        release.CurrentStatus = newStatus;
        release.ApprovalHistory.Add(new ModReleaseApprovalChange
        {
            ModeratorResponsibleId = user.Id,
            Reason = "Actioned in Case Manager",
            CurrentState = newStatus,
            PreviousStatus = originalState
        });

        await _database.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostHandleApprovalAsync(string id)
    {
        return await HandleStateChangeAsync(id, ModReleaseApprovalStatus.Approved);
    }
    
    public async Task<IActionResult> OnPostHandleRejectionAsync(string id)
    {
        return await HandleStateChangeAsync(id, ModReleaseApprovalStatus.DeniedByModerator);
    }

    public async Task<IActionResult> OnPostHandleRequestInformationAsync(string id)
    {
        return await HandleStateChangeAsync(id, ModReleaseApprovalStatus.NeedMoreInformation);
    }

    public async Task<IActionResult> OnPostHandleDeletionAsync(string id)
    {
        if (!Guid.TryParse(id, out var releaseId)) return RedirectToPage();
        var release = await _database.ModReleases.FirstOrDefaultAsync(d => d.Id == releaseId);
        if (release == null) return RedirectToPage();
        _database.ModReleases.Remove(release);

        await _fileManagerService.DeleteModReleaseAsync(release);

        await _database.SaveChangesAsync();
        
        return RedirectToPage();
    }
}