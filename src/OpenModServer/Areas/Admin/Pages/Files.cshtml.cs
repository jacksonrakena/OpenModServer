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
public class Files : PageModel
{
    public List<ModRelease> Releases { get; set; }
    public List<ModRelease> ModerationQueue { get; set; }

    private readonly UserManager<OmsUser> _userManager;
    private readonly FileManagerService _fileManagerService;

    public Files(ApplicationDbContext context, FileManagerService fileManagerService, UserManager<OmsUser> userManager)
    {
        _database = context;
        _userManager = userManager;
        _fileManagerService = fileManagerService;
    }

    private readonly ApplicationDbContext _database;
    public async Task OnGetAsync()
    {
        ModerationQueue = await _database.ModReleases.Where(d => d.CurrentStatus == ModReleaseApprovalStatus.Unapproved)
            .ToListAsync();
        
        Releases = await _database.ModReleases.Include(c => c.ModListing).ToListAsync();
    }

    public async Task<IActionResult> OnPostHandleDeletionAsync(string id)
    {
        if (!Guid.TryParse(id, out var releaseId)) return RedirectToPage("/Files", new { Area = "Admin" });
        var release = await _database.ModReleases.FirstOrDefaultAsync(d => d.Id == releaseId);
        if (release == null) return RedirectToPage("/Files", new { Area = "Admin" });
        _database.ModReleases.Remove(release);

        await _fileManagerService.DeleteModReleaseAsync(release);

        await _database.SaveChangesAsync();
        
        ModerationQueue = await _database.ModReleases
            .Where(d => d.CurrentStatus == ModReleaseApprovalStatus.Unapproved)
            .ToListAsync();
        
        Releases = await _database.ModReleases.Include(c => c.ModListing).ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostHandleApprovalAsync(string id)
    {
        var userId = Guid.Parse(await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User)));
        if (!Guid.TryParse(id, out var releaseId)) return RedirectToPage("/Files", new { Area = "Admin" });
        var release = await _database.ModReleases.Include(c =>c.ApprovalHistory).FirstOrDefaultAsync(d => d.Id == releaseId);
        if (release == null) return RedirectToPage("/Files", new { Area = "Admin" });

        var originalState = release.CurrentStatus;
        release.CurrentStatus = ModReleaseApprovalStatus.Approved;
        release.ApprovalHistory.Add(new ModReleaseApprovalChange
        {
            ModeratorResponsibleId = userId,
            Reason = "Approved in admin console",
            CurrentState = ModReleaseApprovalStatus.Approved,
            PreviousStatus = originalState
        });

        await _database.SaveChangesAsync();
        
        ModerationQueue = await _database.ModReleases.Where(d => d.CurrentStatus == ModReleaseApprovalStatus.Unapproved)
            .ToListAsync();
        
        Releases = await _database.ModReleases.Include(c => c.ModListing).ToListAsync();
        return Page();
    }
}