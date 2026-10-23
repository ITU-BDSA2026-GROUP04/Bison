//Generic Interface for the Post Repository

public interface  IPostRepository<T> where T : class
{
    //IEnumerable<Post> GetAll(); ?
    Task<T> GetObservations(int page = 1); //Consider using Observation instead of T 
    Task<T> GetObservationsFromAuthor(string author, int page = 1);
    Task<T> GetObservationDetailsById(int id, int page = 1);

    Task<T> UnixTimeStampToDateTimeString(double unixTimeStamp);



}
