//BisonService calls methods from here, to fetch data from the database. 
//This is the repository layer, which is responsible for interacting with the database and returning data to the service layer.
//This is the only class where we should use to update the database.
using Microsoft.EntityFrameworkCore;
using Models;
public class PostRepository : IPostRepository //IAggregateRoot
{
    private readonly BisonDBContext _context;
    //private readonly DbSet<T> _dbSet;

    public PostRepository(BisonDBContext context)
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
            .Include(p => p.Author)
            .Include(p => p.Taxon)
            .OrderBy(p => p.PostId)
            .Skip(offset)
            .Take(pagesize)
            .ToList();
    }

public IEnumerable<Observation> GetByAuthor(string author, int page = 1)
    //New getObservationsFromAuthor method from DBFacade.cs, but now using the DbContext and DbSet instead of raw SQL queries.
    {
        int pagesize = 32;
        int offset = (page - 1) * pagesize;

        return _context.Observations
            .Include(p => p.Author)
            .Include(p => p.Taxon)
            .Where(p => p.Author.Name == author)
            .Skip(offset)
            .Take(32)
            .ToList();
    }

    //New observation creation
    public void CreateObservation(string text, string authorName, int taxonId, string? imageURL = null)
    {
        //Find author by name in the database, if not found, throw an exception
        var author = _context.Authors.FirstOrDefault(a => a.Name == authorName);
        if (author == null)
        {
            throw new Exception($"Author with name '{authorName}' not found.");
        }
        
        //Create new observation and add it to the database
        var observation = new Observation
        {
            Text = text,
            Author = author,
            //Timestamp = DateTime.UtcNow, //Timestamp is already set in the Post constructor, so we don't need to set it here
            Taxon = _context.Taxons.Find(taxonId),
            ImageURL = imageURL
        };

        //Add the observation to the database and save changes
        _context.Observations.Add(observation);
        _context.SaveChanges();
    }

public Post? GetById(int id)
    //New getObservationDetailsById method from DBFacade.cs blah blah blah same thing as above.
{
    return _context.Observations
        .Include(p => p.Author)
        .Include(p => p.Comments)
            .ThenInclude(c => c.Author)
        .Include(p => p.Proposals)
            .ThenInclude(pr => pr.Author)
        .FirstOrDefault(p => p.PostId == id);
}

public Taxon? GetTaxonById(int id)
    //New getObservationDetailsById method from DBFacade.cs blah blah blah same thing as above.
{
    return _context.Taxons.Find(id);
}

public IEnumerable<Observation> GetAllObservations(int page = 1){
        int pagesize = 32;
        int offset = (page - 1) * pagesize;

        return _context.Observations
            .Include(p => p.Author)
            .Include(p => p.Taxon)
            .OrderBy(p => p.PostId)
            .Skip(offset)
            .Take(pagesize)
            .ToList();
    }
}

    /*
    public async Task<T> GetObservations(int page = 1) => await ... ; //Consider using Observation instead of T 
    public async Task<T> GetObservationsFromAuthor(string author, int page = 1) => await ... ;
    public async Task<T> GetObservationDetailsById(int id, int page = 1) => await ... ;

    public async Task<T> UnixTimeStampToDateTimeString(double unixTimeStamp) => await ... ;
    */




