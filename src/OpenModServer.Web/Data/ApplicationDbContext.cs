using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Core.Structures;
using OpenModServer.Core.Structures.Releases;
using OpenModServer.Core.Structures.Releases.Approvals;
using OpenModServer.Web.Identity;

namespace OpenModServer.Web.Data;

public class ApplicationDbContext : IdentityDbContext<OmsUser, IdentityRole<Guid>, Guid>
{
    public DbSet<ModListing> ModListings { get; set; }
    public DbSet<ModReleaseApprovalChange> ApprovalChanges { get; set; }
    public DbSet<ModRelease> ModReleases { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ModListing>().Property(b => b.CreatedAt).HasDefaultValueSql("now()");
        builder.Entity<ModListing>().HasOne(d => d.Creator);
        builder.Entity<ModRelease>().HasOne(b => b.ModListing).WithMany(d => d.Releases);
        builder.Entity<ModReleaseApprovalChange>().Property(b => b.CreatedAt).HasDefaultValueSql("now()");
    }
}