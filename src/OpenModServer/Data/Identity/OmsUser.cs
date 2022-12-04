using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using OpenModServer.Data.Comments;
using OpenModServer.Data.Releases;

namespace OpenModServer.Data.Identity;

public class OmsUser : IdentityUser<Guid>
{
    public string GenerateMd5EmailHash()
    {
        if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(Email.Trim()))
            throw new Exception("The email is empty.");

        var encoder = new UTF8Encoding();
        var hashedBytes = MD5.HashData(encoder.GetBytes(Email.ToLower()));
        var sb = new StringBuilder(hashedBytes.Length * 2);

        foreach (var t in hashedBytes) sb.Append(t.ToString("X2"));
        return sb.ToString().ToLower();
    }
    
    public List<ModRelease> Releases { get; set; }
    public List<ModListing> Listings { get; set; }
    public List<ModComment> Comments { get; set; }
    
    // LOCATION
    public string? CountryIsoCode { get; set; }
    public string? City { get; set; }
    
    // SOCIAL INFORMATION
    public string? Website { get; set; }
    public string? FacebookPageName { get; set; }
    public string? TwitterUsername { get; set; }
    public string? SteamCommunityName { get; set; }
    public string? GitHubName { get; set; }
    public string? DiscordInviteCode { get; set; }
}