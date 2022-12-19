using Microsoft.AspNetCore.Mvc;
using OpenModServer.Areas.Games;
using OpenModServer.Areas.Games.Capabilities;

namespace OpenModServer.Areas.Publishers.Controllers;

[Controller, Route("/publishers")]
public class PublishersController: Controller
{
    private readonly GameManager _manager;
    public PublishersController(GameManager manager)
    {
        _manager = manager;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPublisherAsync([FromRoute] string id)
    {
        var game = _manager.Resolve(id);
        if (game is not IPublishable publisher) return NotFound();
        return await publisher.GetPublisherAsync(HttpContext);
    }
}