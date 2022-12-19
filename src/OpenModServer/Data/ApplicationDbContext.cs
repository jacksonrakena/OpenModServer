using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenModServer.Data.Comments;
using OpenModServer.Data.Identity;
using OpenModServer.Data.Releases;
using OpenModServer.Data.Releases.Approvals;
using OpenModServer.Structures;

namespace OpenModServer.Data;

public class ApplicationDbContext : IdentityDbContext<OmsUser, IdentityRole<Guid>, Guid>
{
    public DbSet<ModListing> ModListings { get; set; }
    public DbSet<ModReleaseApprovalChange> ApprovalChanges { get; set; }
    public DbSet<ModRelease> ModReleases { get; set; }
    
    public DbSet<ModComment> Comments { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ModComment>().Property(d => d.CreatedAt).HasDefaultValueSql("now()");

        builder.Entity<ModListing>().Property(d => d.Tags).HasDefaultValue(new List<string>());
        builder.Entity<ModListing>().HasIndex(b => b.Tags).HasMethod("gin");
        builder.Entity<ModListing>().Property(b => b.CreatedAt).HasDefaultValueSql("now()");
        builder.Entity<ModRelease>().Property(b => b.CreatedAt).HasDefaultValueSql("now()");
        builder.Entity<ModListing>().HasOne(d => d.Creator);
        builder.Entity<ModRelease>()
            .HasOne(b => b.ModListing)
            .WithMany(d => d.Releases)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<ModReleaseApprovalChange>().Property(b => b.CreatedAt).HasDefaultValueSql("now()");
        
        // rename ASP.NET Identity tables
        builder.Entity<OmsRole>().ToTable("roles");
        builder.Entity<IdentityRole<Guid>>().ToTable("roles");
        builder.Entity<OmsUser>().ToTable("users");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
    }
}