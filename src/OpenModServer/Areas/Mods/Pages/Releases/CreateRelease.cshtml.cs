using System.ComponentModel.DataAnnotations;
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

namespace OpenModServer.Areas.Mods.Pages.Releases;

[Authorize]
public class CreateRelease : PageModel
{
    [BindProperty]
    [MaxLength(64), Required]
    public string Name { get; set; }
    
    [BindProperty]
    [Required]
    public ModReleaseType Type { get; set; }
    
    [BindProperty]
    [MaxLength(2048)]
    [Required]
    public string Changelog { get; set; }
    
    [BindProperty]
    [Required]
    public IFormFile UploadedFile { get; set; }

    private readonly FileManagerService _fileManager;
    private readonly ApplicationDbContext _database;
    private readonly UserManager<OmsUser> _userManager;

    public CreateRelease(FileManagerService fileManager, ApplicationDbContext context, UserManager<OmsUser> manager)
    {
        _database = context;
        _fileManager = fileManager;
        _userManager = manager;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = Guid.Parse(_userManager.GetUserId(User));
        if (ModelState.IsValid)
        {
            if (Guid.TryParse(RouteData.Values["mod_id"]?.ToString(), out var modId))
            {
                var mod = await _database.ModListings.FirstOrDefaultAsync(d => d.Id == modId && d.CreatorId == user);
                if (mod == null)
                {
                    ModelState.AddModelError("Authorization", "That mod doesn't exist.");
                    return Page();
                }
                
                var file = await _fileManager.AcceptIncomingReleaseUploadAsync(UploadedFile);

                if (file == null) return BadRequest();

                var release = new ModRelease
                {
                    ModListingId = modId,
                    Name = Name,
                    Changelog = Changelog,
                    DownloadCount = 0,
                    FileName = file.Name,
                    FilePath = file.Path,
                    FileSizeKilobytes = file.SizeKilobytes,
                    ReleaseType = Type,
                    ApprovalHistory = new List<ModReleaseApprovalChange>
                    {
                        new()
                        {
                            PreviousStatus = ModReleaseApprovalStatus.Unapproved,
                            CurrentState = ModReleaseApprovalStatus.Unapproved,
                            Reason = $"This upload was received automatically at {DateTime.Now.ToUniversalTime():O} with a file size of {file.SizeKilobytes}KB."
                        }
                    }
                };
                _database.ModReleases.Add(release);
                await _database.SaveChangesAsync();
                return RedirectToPage("/Index", new { id = modId, area="Mods"});
            } else return Page();
        }
        else return Page();
    }
}