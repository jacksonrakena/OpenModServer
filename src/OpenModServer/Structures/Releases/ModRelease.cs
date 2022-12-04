using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using OpenModServer.Structures.Releases.Approvals;

namespace OpenModServer.Structures.Releases;

[Table("mod_releases")]
public class ModRelease
{
    [Column("id"), Key]
    public Guid Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    public ModListing ModListing { get; set; }
    [Column("listing_id")]
    public Guid ModListingId { get; set; }

    [Column("download_count")]
    public int DownloadCount { get; set; } = 0;
    
    [MaxLength(2048), Column("changelog")]
    public string? Changelog { get; set; }
    
    [Column("release_type")]
    public ModReleaseType ReleaseType { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    public List<ModReleaseApprovalChange> ApprovalHistory { get; set; }

    public ModReleaseApprovalStatus CurrentStatus { get; set; } = ModReleaseApprovalStatus.Unapproved;
    
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public ulong FileSizeKilobytes { get; set; }

    public void UpdateStatus(ModReleaseApprovalChange change)
    {
        ApprovalHistory.Add(change);
        CurrentStatus = change.CurrentState;
    }
}