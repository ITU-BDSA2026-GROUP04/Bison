using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Models;

public class BisonDBContext : IdentityDbContext<Author, IdentityRole<int>, int> // We are not actually using roles, but im not allowed to not add a role:// It should not be messing with anything else though, so maybe we can leave it??
{
    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options) { }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Taxon> Taxons { get; set; }
    public DbSet<Observation> Observations { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Proposal> Proposals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Post>().UseTptMappingStrategy();
    }
    
}