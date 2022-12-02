using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Core.Games.Supported;
using OpenModServer.Core.Structures;
using OpenModServer.Web.Data;

namespace OpenModServer.Web.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    internal List<IGrouping<string, ModListing>>? ModListings;
    private readonly ApplicationDbContext _database;

    public PrivacyModel(ILogger<PrivacyModel> logger, ApplicationDbContext database)
    {
        _database = database;
        _logger = logger;
    }

    public async Task OnGet()
    {
        ModListings = await _database.ModListings.Include(e => e.Creator).GroupBy(d => d.GameIdentifier).ToListAsync();
    }
}