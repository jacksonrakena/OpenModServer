using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenModServer.Areas.Identity;

namespace OpenModServer.Areas.Admin.Pages;

[Authorize(nameof(Permissions.Administrator))]
public class Status : PageModel
{
    public void OnGet()
    {
        
    }
}