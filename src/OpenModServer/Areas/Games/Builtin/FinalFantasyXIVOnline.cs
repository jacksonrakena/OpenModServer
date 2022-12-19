using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Areas.Games.Capabilities;
using OpenModServer.Data;
using OpenModServer.Data.Releases;
using OpenModServer.Data.Releases.Approvals;

namespace OpenModServer.Areas.Games.Builtin;

public class FinalFantasyXIVOnline : ISupportedGame, IPublishable
{
    public string Identifier => "ff14d";
    public string Name => "Final Fantasy XIV Online (Dalamud)";

    public string Description =>
        "Final Fantasy XIV is a massively multiplayer online role-playing game (MMORPG) developed and published by Square Enix.";

    public async Task<IActionResult> GetPublisherAsync(HttpContext context)
    {
        var linkGenerator = context.RequestServices.GetRequiredService<LinkGenerator>();
        var db = context.RequestServices.GetRequiredService<ApplicationDbContext>();
        var mods = await db.ModListings
            .Include(d => d.Creator)
            .Include(d => d.Releases)
            .Where(e => e.GameIdentifier == Identifier)
            .ToListAsync();

        var output = new List<object>();
        foreach (var d in mods)
        {
            var orderedReleases = d.Releases
                .OrderByDescending(d => d.CreatedAt).Where(d => d.CurrentStatus == ModReleaseApprovalStatus.Approved).ToList(); 
            var latestProduction = orderedReleases.FirstOrDefault(d => d.ReleaseType == ModReleaseType.Production);
            if (latestProduction == null) continue;
            var latestTesting = orderedReleases.FirstOrDefault(d => d.ReleaseType == ModReleaseType.Testing); 
            var downloads = d.Releases.Sum(e => e.DownloadCount);

            var productionLink = linkGenerator.GetUriByAction(context, "DownloadReleaseById", "Download",
                new { id = latestProduction.Id, Area="Downloads" });
            output.Add(new
            {
                Author = d.Creator.UserName,
                Name = d.Name,
                Description = d.Description,
                Punchline = d.Tagline,
                InternalName = d.Name,
                AssemblyVersion = latestProduction.Name,
                RepoUrl = linkGenerator.GetUriByPage(context, "/Index", null, new{id=d.Id,Area="Mods"}),
                Changelog = latestProduction.Changelog,
                Tags = new List<string>(),
                IsHide = "False",
                IsTestingExclusive = "False",
                LastUpdated = ((DateTimeOffset)latestProduction.CreatedAt.ToUniversalTime()).ToUnixTimeSeconds(),
                DownloadCount = downloads,
                DownloadLinkInstall = productionLink,
                DownloadLinkTesting = latestTesting != null ? linkGenerator.GetUriByAction(context, "DownloadReleaseById", "Download",
                    new { id = latestTesting.Id, Area="Downloads" }) : productionLink,
                DownloadLinkUpdate = productionLink
            });
        }

        return new JsonResult(output) { SerializerSettings = new JsonSerializerOptions() };
    }
}