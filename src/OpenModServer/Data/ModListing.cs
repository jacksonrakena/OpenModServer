using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data.Comments;
using OpenModServer.Data.Identity;
using OpenModServer.Data.Releases;

namespace OpenModServer.Data;

[Table("mod_listings"), Index(nameof(GameIdentifier))]
public class ModListing
{
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public Guid Id { get; set; }
    
    [Column("creator_id")]
    public Guid CreatorId { get; set; }
    
    public OmsUser Creator { get; set; }
    
    [Column("game_identifier")]
    [DisplayName("Game")]
    [DataType("game_selector")]
    public string GameIdentifier { get; set; }
    
    [MaxLength(128)]
    [DisplayName("Name")]
    public string Name { get; set; }
    
    [MaxLength(128)]
    [DisplayName("Tagline")]
    public string Tagline { get; set; }
    
    [MaxLength(2048)]
    [DisplayName("Description")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public bool IsVisibleToPublic { get; set; } = false;

    [Column("download_count")] public int DownloadCount { get; set; } = 0;
    
    public List<ModRelease> Releases { get; set; }
    
    public List<ModComment> Comments { get; set; }

    [Column("tags")]
    public List<string> Tags { get; set; } = new List<string>();

    [Column(TypeName="jsonb")]
    public JsonNode GameMetadata { get; set; } = new JsonObject();
}