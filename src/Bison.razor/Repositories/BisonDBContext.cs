public class BisonDBContext : DbContext
{
    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options) { }

    public DbSet<Post> Posts { get; set; }
    
}