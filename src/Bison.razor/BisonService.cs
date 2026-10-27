 using System.Globalization;
public record ObservationViewModel(string Author, string Message, string Timestamp);

//public record Comment(string Author, string Message, string Timestamp); Obsolete, now using the Comment class from the Post.cs model instead of a separate record.
public record ObservationDetailViewModel(string Author, string Message, string Timestamp, List<Comment> Comment);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page = 1);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1);
    public ObservationDetailViewModel GetObservationDetailsById(int id, int page = 1);
}

public class ObservationService : IObservationService
{
    private readonly IPostRepository _repository;

    public ObservationService(IPostRepository postRepository)
    {
        _repository = postRepository;
    }
    public List<ObservationViewModel> GetObservations(int page = 1)
    {
        return _repository.GetAll(page)
            .Select(p => new ObservationViewModel(p.Author.Name, p.Text, p.TimeStamp.ToString())).ToList();
    }


    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1)
    {
        return _repository.GetByAuthor(author, page)
            .Select(p => new ObservationViewModel(p.Author.Name, p.Text, p.TimeStamp.ToString())).ToList();
    }

    public ObservationDetailViewModel GetObservationDetailsById(int id, int page = 1)
    {
        var post = _repository.GetById(id);
        if (post == null) return null;
        
        var comments = (post as Observation)?.Comments?.ToList() ?? new List<Comment>(); //if this post is an Observation, get its comments, otherwise return an empty list.
            return new ObservationDetailViewModel(post.Author.Name, post.Text, post.TimeStamp.ToString(), comments);
    }
}

    //Obsolete, now using DateTime.ToString() instead of converting to string manually.

    // public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    // {
    //     // Unix timestamp is seconds past epoch
    //     DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    //     dateTime = dateTime.AddSeconds(unixTimeStamp);
    //     //using invariantculture, so it says ':' on the page instead of '.'
    //     return dateTime.ToString("MM/dd/yy H:mm:ss", CultureInfo.InvariantCulture); 
    // } 
    

