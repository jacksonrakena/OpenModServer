using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenModServer.Data.Identity;

namespace OpenModServer.Data.Comments;

public class ModComment
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("listing_id")]
    public Guid ModListingId { get; set; }
    public ModListing Listing { get; set; }
    
    [Column("content"), MaxLength(560)]
    public string Content { get; set; }
    
    [Column("author_id")]
    public Guid AuthorId { get; set; }
    public OmsUser Author { get; set; }
    
    [Column("parent_comment_id")]
    public Guid? ParentCommentId { get; set; }
    public ModComment? ParentComment { get; set; }
}