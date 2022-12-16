using Microsoft.AspNetCore.Mvc;

namespace OpenModServer.Games.Capabilities;

public interface ISupportedGame
{
    public string Identifier { get;  }
    public string Name { get; }
    
    public string Description { get; }
}