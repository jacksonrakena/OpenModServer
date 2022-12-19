using Microsoft.AspNetCore.Mvc;
using OpenModServer.Data;

namespace OpenModServer.Pages.Shared.Components.ListingCard;

public class ListingCard : ViewComponent
{
    public IViewComponentResult Invoke(ModListing listing)
    {
        return View("ListingCard", listing);
    }
}