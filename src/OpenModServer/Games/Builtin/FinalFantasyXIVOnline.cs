using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data;
using OpenModServer.Games.Capabilities;
using OpenModServer.Structures.Releases;
using OpenModServer.Structures.Releases.Approvals;

namespace OpenModServer.Games.Builtin;

public class FinalFantasyXIVOnline : ISupportedGame, IPublishable
{
    public string Identifier => "ff14d";
    public string Name => "Final Fantasy XIV Online (Dalamud)";

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
                Punchline = d.Description,
                InternalName = d.Name,
                AssemblyVersion = "",
                RepoUrl = "",
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