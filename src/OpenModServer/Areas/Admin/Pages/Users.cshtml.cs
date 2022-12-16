using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenModServer.Areas.Identity;
using OpenModServer.Data;
using OpenModServer.Data.Identity;
using OpenModServer.Data.Releases;
using OpenModServer.Data.Releases.Approvals;
using OpenModServer.Services;
using OpenModServer.Structures;

namespace OpenModServer.Areas.Admin.Pages;

[Authorize(Policy = nameof(Permissions.Administrator))]
public class Users : PageModel
{
    [Display(Name="Role name")]
    [BindProperty]
    public string RoleName { get; set; }
    
    [Display(Name = "Permissions")]
    [BindProperty]
    public Dictionary<string, bool> RolePermissions { get; set; }
    
    public List<OmsUser> UserAccounts { get; set; }
    public List<IdentityRole<Guid>> Roles { get; set; }
    public List<IdentityUserRole<Guid>> UserRoles { get; set; }
    public List<IdentityRoleClaim<Guid>> RoleClaims { get; set; }

    private readonly UserManager<OmsUser> _userManager;
    private readonly FileManagerService _fileManagerService;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public Users(ApplicationDbContext context, FileManagerService fileManagerService, UserManager<OmsUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _database = context;
        _userManager = userManager;
        _fileManagerService = fileManagerService;
        _roleManager = roleManager;
    }

    private readonly ApplicationDbContext _database;
    public async Task OnGetAsync()
    {
        UserAccounts = await _database.Users.ToListAsync();
        RoleClaims = await _database.RoleClaims.ToListAsync();
        Roles = await _database.Roles.ToListAsync();
        UserRoles = await _database.UserRoles.ToListAsync();
    }

    public async Task<IActionResult> OnPostHandleRoleCreateAsync()
    {
        var role = new IdentityRole<Guid>
        {
            Name = RoleName,
        };
        var createRoleResult = await _roleManager.CreateAsync(role);
        if (!createRoleResult.Succeeded)
        {
            TempData.PutJson("Alert", new TemporaryAlert(
                TemporaryAlertType.Danger,
                "Failed to create role.",
                string.Join(", ", createRoleResult.Errors.Select(d => d.Description))
            ));
            await OnGetAsync();
            return Page();
        }
        
        foreach (var p in RolePermissions.Where(p => p.Value))
        {
            await _roleManager.AddClaimAsync(role, new Claim("Permission", p.Key));
        }

        return RedirectToPage();
    }
}