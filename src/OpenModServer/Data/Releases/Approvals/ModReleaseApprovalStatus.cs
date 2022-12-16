namespace OpenModServer.Data.Releases.Approvals;

public enum ModReleaseApprovalStatus
{
    Unapproved = 0,
    DeniedByModerator = 1,
    DeniedBySystem = 2,
    Approved = 3,
    Removed = 4,
    NeedMoreInformation = 5,
    Pending = 6
}