 using System.Globalization;
 using Models;
public record ObservationViewModel(string Author, string Message, string Timestamp, Taxon Taxon, int? TaxonId, ImageURL = null);

//public record Comment(string Author, string Message, string Timestamp); Obsolete, now using the Comment class from the Post.cs model instead of a separate record.
public record ObservationDetailViewModel(string Author, string Message, string Timestamp, List<Comment> Comment, List<Proposal> Proposals);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page = 1);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1);
    public ObservationDetailViewModel GetObservationDetailsById(int id, int page = 1);

    public List<ObservationViewModel> GetFilteredObservations(int taxonId, int page = 1);

    //For creating observations, comments and proposals
    public void createObservation(string text, string authorName, int taxonId, string? imageURL = null);
    public void createComment(int postId, string text, string authorName); //Or observationId instead of postId?
    public void createProposal(int postId, string text, string authorName); //Or observationId instead of postId?

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
        return _repository.GetAllObservations(page)
            .Select(p => new ObservationViewModel(p.Author.Name, p.Text, p.TimeStamp.ToString(), p.Taxon, p.TaxonId)).ToList();
    }


    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1)
    {
        return _repository.GetByAuthor(author, page)
            .Select(p => new ObservationViewModel(p.Author.Name, p.Text, p.TimeStamp.ToString(), p.Taxon, p.TaxonId)).ToList();
    }

    public ObservationDetailViewModel GetObservationDetailsById(int id, int page = 1)
    {
        var post = _repository.GetById(id);
        if (post == null) return null;
        
        var comments = (post as Observation)?.Comments?.ToList() ?? new List<Comment>(); //if this post is an Observation, get its comments, otherwise return an empty list.
        var proposals = (post as Observation)?.Proposals?.ToList() ?? new List<Proposal>(); 
            return new ObservationDetailViewModel(post.Author.Name, post.Text, post.TimeStamp.ToString(), comments, proposals);
    }

    public List<ObservationViewModel> GetFilteredObservations(int taxonId, int page = 1)
    {
        var root = _repository.GetTaxonById(taxonId);
        
        if(root == null)
        {
            return new List<ObservationViewModel>();
            //return empty list, so the page will recognize and show "no observations"
        }

        var allObservations = _repository.GetAllObservations(page);

        var observationsArray = allObservations.ToArray();

        var observationSequence = Dafny.Sequence<Observation>.FromArray(observationsArray);


        var filteredObservations = FilterWrapper.__default.getFilteredObservations(root, observationSequence);


       var result = new List<ObservationViewModel> ();
       foreach(var ob in filteredObservations)
        {
            result.Add(new ObservationViewModel(ob.Author.Name,ob.Text,ob.TimeStamp.ToString(), ob.Taxon, ob.TaxonId));
        }
        return result;
    }

    //For creating observations, comments and proposals
    public void createObservation(string text, string authorName, int taxonId, string? imageURL = null)
    {
        _repository.createObservation(text, authorName, taxonId, imageURL);
    }

    public void createComment(int postId, string text, string authorName)
    {
        _repository.createComment(postId, text, authorName);
    }

    public void createProposal(int postId, string text, string authorName)
    {
        _repository.createProposal(postId, text, authorName);
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
    

