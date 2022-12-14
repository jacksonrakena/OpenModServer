using Microsoft.AspNetCore.Mvc;

namespace OpenModServer.Areas.Games.Capabilities;

public interface IPublishable
{
    public Task<IActionResult> GetPublisherAsync(HttpContext context);
}