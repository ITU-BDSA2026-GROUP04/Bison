using Microsoft.EntityFrameworkCore;

public class BisonDBContext : DbContext
{
    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options) { }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Author> Authors { get; set; }

    public DbSet<Taxon> Taxons { get; set; }
    
}