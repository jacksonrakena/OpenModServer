using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Identity;
using OpenModServer.Structures.Releases;

namespace OpenModServer.Structures;

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
    
    [MaxLength(2048)]
    [DisplayName("Description")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public List<ModRelease> Releases { get; set; }
}