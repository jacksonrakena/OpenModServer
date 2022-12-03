using Microsoft.AspNetCore.Mvc;

namespace OpenModServer.Games.Capabilities;

public interface IPublishable
{
    public Task<IActionResult> GetPublisherAsync(HttpContext context);
}