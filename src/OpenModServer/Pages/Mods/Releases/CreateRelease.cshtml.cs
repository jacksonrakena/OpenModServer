using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenModServer.Structures.Releases;

namespace OpenModServer.Pages.Releases;

public class CreateRelease : PageModel
{
    [MaxLength(64)]
    public string Name { get; set; }
    public ModReleaseType Type { get; set; }
    
    [MaxLength(2048)]
    public string Changelog { get; set; }
    
    public void OnGet()
    {
        
    }
}