using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenModServer.Data.Releases.Approvals;
using OpenModServer.Data.Releases.Safety;

namespace OpenModServer.Data.Releases;

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
    
    [Column("vt_analysis_id")]
    public string? VT_AnalysisId { get; set; }
    
    [Column("vt_submitted_at")]
    public DateTime? VT_SubmittedAt { get; set; }
    
    [Column("vt_last_updated")]
    public DateTime? VT_LastUpdatedAt { get; set; }
    
    [Column("vt_num_fails")]
    public int? VT_NumberOfFailedScans { get; set; }
    
    [Column("vt_num_harmless")]
    public int? VT_NumberOfHarmlessScans { get; set; }
    
    [Column("vt_num_malicious")]
    public int? VT_NumberOfMaliciousScans { get; set; }
    
    [Column("vt_num_sus")]
    public int? VT_NumberOfSuspiciousScans { get; set; }
    
    [Column("vt_scan_result")]
    public VirusTotalScanStatus? VT_ScanResult { get; set; }
}