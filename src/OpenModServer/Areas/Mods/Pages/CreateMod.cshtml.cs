using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenModServer.Data;
using OpenModServer.Data.Identity;
using OpenModServer.Structures;

namespace OpenModServer.Areas.Mods.Pages;

public class TransitiveModListing
{
     
    [MaxLength(128)]
    [DisplayName("Name")]
    [Display(Prompt = "My new mod")]
    [Required]
    [BindProperty] public string Name { get; set; }
    
    [MaxLength(2048)]
    [DisplayName("Description")]
    [Display(Prompt = "Markdown formatting is supported.")]
    [DataType(DataType.MultilineText)]
    [BindProperty] public string Description { get; set; }
    
    [DisplayName("Game")]
    [Required]
    [DataType("game_selector")]
    [BindProperty] public string GameIdentifier { get; set; }   
    
    [DisplayName("Tags")]
    [MaxLength(64)]
    [Display(Prompt = "Separate by commas, i.e. 'fun, difficult, challenge'")]
    [BindProperty]
    public string Tags { get; set; }
    
        
    [MaxLength(128)]
    [DisplayName("Tagline")]
    [Display(Prompt = "A short sentence used in the mod list.")]
    [Required]
    [BindProperty]
    public string Tagline { get; set; }
}
[Authorize]
public class CreateMod : PageModel
{
    [BindProperty] public TransitiveModListing? DataModel { get; set; }
    
    private readonly ApplicationDbContext _database;
    private readonly UserManager<OmsUser> _userManager;

    public CreateMod(ApplicationDbContext context, UserManager<OmsUser> userManager)
    {
        _userManager = userManager;
        _database = context;
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || DataModel == null) return Page();
        var listing = new ModListing
        {
            CreatorId = Guid.Parse(_userManager.GetUserId(User)!), 
            Name = DataModel.Name,
            Description = DataModel.Description, 
            GameIdentifier = DataModel.GameIdentifier,
            Tagline = DataModel.Tagline
        };
        if (!string.IsNullOrWhiteSpace(DataModel.Tags))
        {
            listing.Tags = DataModel.Tags
                .Split(",")
                .Select(e=>e.Trim())
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .Distinct()
                .ToList();
        }
        _database.ModListings.Add(listing);
        await _database.SaveChangesAsync();
        return RedirectToPage("/Mods/Mods");
    }
}