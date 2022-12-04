using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data;
using OpenModServer.Data.Identity;
using OpenModServer.Data.Releases.Approvals;
using OpenModServer.Services;

namespace OpenModServer.Areas.Admin.Pages;

[Authorize(Roles = "Administrator,ApprovalModerator")]
public class ModerationActions : PageModel
{
    public List<ModReleaseApprovalChange> Changes { get; set; }
    
    private readonly UserManager<OmsUser> _userManager;
    private readonly FileManagerService _fileManagerService;

    public ModerationActions(ApplicationDbContext context, FileManagerService fileManagerService, UserManager<OmsUser> userManager)
    {
        _database = context;
        _userManager = userManager;
        _fileManagerService = fileManagerService;
    }

    private readonly ApplicationDbContext _database;
    public async Task OnGetAsync()
    {
        Changes = await _database.ApprovalChanges
            .Include(c => c.ModeratorResponsible)
            .OrderByDescending(d => d.CreatedAt)
            .Take(100)
            .Include(c => c.ModRelease)
            .ToListAsync();
    }
}