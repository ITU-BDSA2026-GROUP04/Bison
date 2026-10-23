//BisonService calls methods from here, to fetch data from the database. 
//This is the repository layer, which is responsible for interacting with the database and returning data to the service layer.
//This is the only class where we should use to update the database.


public class PostRepository<T> : IPostRepository<T> where T : class //IAggregateRoot
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public PostRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();

    }

    /*
    public async Task<T> GetObservations(int page = 1) => await ... ; //Consider using Observation instead of T 
    public async Task<T> GetObservationsFromAuthor(string author, int page = 1) => await ... ;
    public async Task<T> GetObservationDetailsById(int id, int page = 1) => await ... ;

    public async Task<T> UnixTimeStampToDateTimeString(double unixTimeStamp) => await ... ;
    */

}


