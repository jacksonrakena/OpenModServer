using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenModServer.Areas.Account;

namespace OpenModServer.Structures;

public static class PageUtilities
{
    public static void SetAlert(this PageModel model, TemporaryAlert alert)
    {
        model.TempData.PutJson("Alert", alert);
    }

    public static TemporaryAlert? GetAlert(this PageModel model)
    {
        return model.TempData.GetJson<TemporaryAlert>("Alert");
    }

    public static TemporaryAlert? GetAlert(this RazorPageBase page)
    {
        return page.TempData.GetJson<TemporaryAlert>("Alert");
    }

    public static bool HasPermission(this ClaimsPrincipal principal, Permissions permission)
    {
        return principal.Claims.HasPermission(permission);
        //return principal.HasClaim("Permission",permission.ToString("G"))
        //      || principal.HasClaim("Permission", Permissions.Administrator.ToString("G"));
    }

    public static bool HasPermission(this IEnumerable<Claim> principal, Permissions permissions)
    {
        return principal.Any(p => p.Type == "Permission" 
                                  && p.Value == permissions.ToString("G") ||
                                  p.Value == Permissions.Administrator.ToString("G"));
    }
}