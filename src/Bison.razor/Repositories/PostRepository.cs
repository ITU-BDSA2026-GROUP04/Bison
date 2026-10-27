//BisonService calls methods from here, to fetch data from the database. 
//This is the repository layer, which is responsible for interacting with the database and returning data to the service layer.
//This is the only class where we should use to update the database.
using Microsoft.EntityFrameworkCore;

public class PostRepository : IPostRepository //IAggregateRoot
{
    private readonly AppDbContext _context;
    //private readonly DbSet<T> _dbSet;

    public PostRepository(AppDbContext context)
    {
        _context = context;
        //_dbSet = context.Set<T>();
    }



public IEnumerable<Post> GetAll(int page = 1)
    //New getObservations method from DBFacade.cs, but now using the DbContext and DbSet instead of raw SQL queries.
    {
        int pagesize = 32;
        int offset = (page - 1) * pagesize;

        return _context.Posts
            .OrderBy(p => p.Id)
            .Skip(offset)
            .Take(pagesize)
            .ToList();
    }

public IEnumerable<Post> GetByAuthor(string author, int page = 1)
    //New getObservationsFromAuthor method from DBFacade.cs, but now using the DbContext and DbSet instead of raw SQL queries.
    {
        int pagesize = 32;
        int offset = (page - 1) * pagesize;

        return _context.Posts
            .Include(p => p.Author)
            .Where(p => p.Author.Name == author)
            .Skip(offset)
            .Take(32)
            .ToList();
    }

public Post? GetById(int id)
    //New getObservationDetailsById method from DBFacade.cs blah blah blah same thing as above.
{
    return _context.Posts
        .Include(p => p.Author)
        .FirstOrDefault(p => p.Id == id);
}

    /*
    public async Task<T> GetObservations(int page = 1) => await ... ; //Consider using Observation instead of T 
    public async Task<T> GetObservationsFromAuthor(string author, int page = 1) => await ... ;
    public async Task<T> GetObservationDetailsById(int id, int page = 1) => await ... ;

    public async Task<T> UnixTimeStampToDateTimeString(double unixTimeStamp) => await ... ;
    */

}


