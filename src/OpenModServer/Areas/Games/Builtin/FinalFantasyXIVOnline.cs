using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Areas.Games.Capabilities;
using OpenModServer.Data;
using OpenModServer.Data.Releases;
using OpenModServer.Data.Releases.Approvals;

namespace OpenModServer.Areas.Games.Builtin;

public class FinalFantasyXIVOnline : ISupportedGame, IPublishable, IMetadataCarrier
{
    public string Identifier => "ff14d";
    public string Name => "Final Fantasy XIV Online";

    public string Description =>
        "Final Fantasy XIV is a massively multiplayer online role-playing game (MMORPG) developed and published by Square Enix.";

    public void BuildMetadataFields(MetadataFieldCollectionBuilder builder)
    {
        builder.AddField("assembly-name", MetadataFieldType.SingleLineText, assemblyName =>
        {
            assemblyName.Name = "Assembly name";
            assemblyName.Placeholder = "The name of your .NET assembly.";
        });
        builder.AddField("is-r18", MetadataFieldType.Boolean, r18 =>
        {
            r18.Name = "Does this mod contain materials considered explicit or pornographic in nature?";
        });
        builder.AddField("is-texture", MetadataFieldType.Boolean, texture =>
        {
            texture.Name = "Is this mod a texture or resource mod? (TexTools, Penumbra, etc..)";
        });
    }

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
                InternalName = d.GameMetadata?["assembly-name"]?.GetValue<string>() ?? d.Name,
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