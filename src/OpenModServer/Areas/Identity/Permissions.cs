using System.ComponentModel;

namespace OpenModServer.Areas.Identity;

public enum Permissions
{
    [Description("A person with this permission can approve or deny new listing releases.")]
    ApproveReleases,
    
    [Description("A person with this permission can delete listings, including all releases.")]
    ManageListings,
    
    [Description("A person with this permission can invite and delete site users.")]
    ManageUsers,
    
    [Description("A person with this permission can change the roles and permissions of other users.")]
    Administrator
}