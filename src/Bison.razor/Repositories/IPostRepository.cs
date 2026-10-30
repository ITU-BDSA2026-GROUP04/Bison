//Generic Interface for the Post Repository

public interface  IPostRepository
{
    IEnumerable<Post> GetAll(int page = 1);
    IEnumerable<Post> GetByAuthor(string author, int page = 1);
    
    // Task GetObservations(int page = 1); //Consider using Observation instead of T 
    // Task GetObservationsFromAuthor(string author, int page = 1);
    // Task GetObservationDetailsById(int id, int page = 1);
    // Task<T> UnixTimeStampToDateTimeString(double unixTimeStamp);

    Post GetById(int id);
}
