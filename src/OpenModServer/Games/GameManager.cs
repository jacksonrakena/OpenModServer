using OpenModServer.Games.Capabilities;

namespace OpenModServer.Games;

public class GameManager
{
    private readonly Dictionary<string, ISupportedGame> _games = new Dictionary<string, ISupportedGame>();

    public void Register(ISupportedGame value)
    {
        _games.Add(value.Identifier, value);
    }

    public int Size() => _games.Count;

    public ISupportedGame? Resolve(string name) => _games.GetValueOrDefault(name);
    public T? Resolve<T>(string name) where T : class, ISupportedGame => _games.GetValueOrDefault(name) as T;

    public string GeneratePublisherUrl(ISupportedGame game, HttpContext context)
    {
        return context.RequestServices.GetRequiredService<LinkGenerator>()
            .GetUriByAction(context, "GetPublisher", "Publishers", new { id = game.Identifier });
    }
}