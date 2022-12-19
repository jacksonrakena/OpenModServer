using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenModServer.Areas.Games;
using OpenModServer.Areas.Games.Capabilities;
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
    public MetadataFieldCollectionBuilder? MetadataFields { get; set; }
    public ISupportedGame Game { get; set; }
    
    private readonly ApplicationDbContext _database;
    private readonly UserManager<OmsUser> _userManager;
    private readonly GameManager _gameManager;

    public CreateMod(ApplicationDbContext context, UserManager<OmsUser> userManager, GameManager gameManager)
    {
        _gameManager = gameManager;
        _userManager = userManager;
        _database = context;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery(Name="game")] string gameIdentifier)
    {
        var game = _gameManager.Resolve(gameIdentifier);
        if (game == null)
        {
            return NotFound();
        }

        Game = game;
        if (Game is IMetadataCarrier carrier)
        {
            MetadataFields = new MetadataFieldCollectionBuilder();
            carrier.BuildMetadataFields(MetadataFields);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync([FromQuery(Name="game")] string gameIdentifier)
    {
        if (!ModelState.IsValid || DataModel == null) return Page();

        var game = _gameManager.Resolve(gameIdentifier);
        if (game == null)
        {
            ModelState.AddModelError("Model", "Unknown game.");
            return await OnGetAsync(gameIdentifier);
        }
        var jsonNode = new JsonObject();
        if (game is IMetadataCarrier metadataCarrier)
        {
            var builder = new MetadataFieldCollectionBuilder();
            metadataCarrier.BuildMetadataFields(builder);
            var fields = builder.Fields;
            foreach (var field in fields)
            {
                if (!Request.Form.TryGetValue(field.Key, out var fieldValue) || string.IsNullOrWhiteSpace(fieldValue.ToString()))
                {
                    if (field.Value.FieldType != MetadataFieldType.Boolean)
                    {
                        ModelState.AddModelError(field.Key, $"A response is required for {field.Value.Name}.");
                        continue;
                    }
                    else fieldValue = "off";
                }

                jsonNode[field.Key] = fieldValue.ToString();
            }

            if (!ModelState.IsValid) return await OnGetAsync(gameIdentifier);
        }

        var listing = new ModListing
        {
            CreatorId = Guid.Parse(_userManager.GetUserId(User)!), 
            Name = DataModel.Name,
            Description = DataModel.Description, 
            GameIdentifier = game.Identifier,
            Tagline = DataModel.Tagline,
            GameMetadata = jsonNode
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
        return RedirectToPage("/Index", new {Area ="Mods",id=listing.Id});
    }
}