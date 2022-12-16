using System.ComponentModel.DataAnnotations.Schema;
using OpenModServer.Data.Identity;

namespace OpenModServer.Data.Releases.Approvals;

[Table("mod_release_approvals")]
public class ModReleaseApprovalChange
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("mod_release_id")]
    public Guid ModReleaseId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public ModRelease ModRelease { get; set; }
    
    public ModReleaseApprovalStatus PreviousStatus { get; set; }
    public ModReleaseApprovalStatus CurrentState { get; set; }

    [Column("is_private_note")]
    public bool IsPrivateNote { get; set; } = false;

    public string Reason { get; set; }
    
    /// <summary>
    ///     If null, this was an automated system action.
    /// </summary>
    public Guid? ModeratorResponsibleId { get; set; }
    public OmsUser? ModeratorResponsible { get; set; }
}