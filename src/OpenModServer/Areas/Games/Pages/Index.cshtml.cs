using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Areas.Games.Capabilities;
using OpenModServer.Data;

namespace OpenModServer.Areas.Games.Pages;

public class Index : PageModel
{
    private readonly GameManager _gameManager;
    private readonly ApplicationDbContext _dbContext;

    public Index(GameManager gameManager, ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _gameManager = gameManager;
    }
    
    public ISupportedGame? Game { get; set; }
    public IPublishable? GamePublisher { get; set; }
    public List<ModListing> Mods { get; set; }
    public async Task OnGetAsync()
    {
        if (RouteData.Values.TryGetValue("id", out var id))
        {
            Game = _gameManager.Resolve(id?.ToString());
            Mods = await _dbContext.ModListings.Where(d => d.GameIdentifier == id.ToString()).ToListAsync();
        }
    }
}