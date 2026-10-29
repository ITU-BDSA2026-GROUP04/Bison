using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


public class BisonDBContext : IdentityDbContext<ApplicationUser>
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
        modelBuilder.Entity<Post>().UseTptMappingStrategy();
    }
    
}