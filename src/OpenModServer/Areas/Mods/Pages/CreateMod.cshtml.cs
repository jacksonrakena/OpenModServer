using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenModServer.Data;
using OpenModServer.Identity;
using OpenModServer.Structures;

namespace OpenModServer.Areas.Mods.Pages;

public class TransitiveModListing
{
     
    [MaxLength(128)]
    [DisplayName("Name")]
    [BindProperty] public string Name { get; set; }
    
    [MaxLength(2048)]
    [DisplayName("Description")]
    [DataType(DataType.MultilineText)]
    [BindProperty] public string Description { get; set; }
    
    [DisplayName("Game")]
    [DataType("game_selector")]
    [BindProperty] public string GameIdentifier { get; set; }   
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
        };
        _database.ModListings.Add(listing);
        await _database.SaveChangesAsync();
        return RedirectToPage("/Mods/Mods");
    }
}