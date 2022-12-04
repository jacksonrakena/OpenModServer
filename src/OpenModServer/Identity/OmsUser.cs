using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace OpenModServer.Identity;

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
}