using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenModServer.Data;

namespace OpenModServer.Pages.Components;

public class ListingCard : ViewComponent
{
    public IViewComponentResult Invoke(ModListing listing)
    {
        return View("ListingCard", listing);
    }
}