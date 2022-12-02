using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenModServer.Core.Structures.Releases.Approvals;

namespace OpenModServer.Core.Structures.Releases;

[Table("mod_releases")]
public class ModRelease
{
    [Column("id"), Key]
    public Guid Id { get; set; }
    
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
}