using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Areas.Account;
using OpenModServer.Data.Identity;
using OpenModServer.Structures;

namespace OpenModServer.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly SignInManager<OmsUser> _signInManager;
    private readonly UserManager<OmsUser> _userManager;
    private readonly RoleManager<OmsRole> _roleManager;

    public IndexModel(ILogger<IndexModel> logger, 
        UserManager<OmsUser> userManager,
        SignInManager<OmsUser> signInManager, 
        RoleManager<OmsRole> roleManager)
    {
        _logger = logger;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostInitSetupAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        var isAwaitingSetup = (await _roleManager.Roles.ToListAsync()).Count == 0;
        if (!isAwaitingSetup || user == null) return Forbid();

        var role = new OmsRole { Name = "Administrator" };
        var roleCreationResult = await _roleManager.CreateAsync(role);
        if (!roleCreationResult.Succeeded)
        {
            this.SetAlert(new TemporaryAlert(TemporaryAlertType.Danger, "Failed to setup.", roleCreationResult.Errors.FirstOrDefault()?.Description));
            return Page();
        }

        foreach (var permission in typeof(Permissions).GetEnumNames())
        {
            await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        }

        await _userManager.AddToRoleAsync(user, role.Name);
        await _signInManager.RefreshSignInAsync(user);
        
        return RedirectToPage();
    }
}