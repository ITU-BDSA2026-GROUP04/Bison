 using System.Globalization;
public record ObservationViewModel(string Author, string Message, string Timestamp);

public record Comment(string Author, string Message, string Timestamp);
public record ObservationDetailViewModel(string Author, string Message, string Timestamp, List<Comment> Comment);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page = 1);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1);
    public ObservationDetailViewModel GetObservationDetailsById(int id, int page = 1);
}

public class ObservationService : IObservationService
{
    public List<ObservationViewModel> GetObservations(int page = 1)
    {
        return DBFacade.GetObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1)
    {
        // filter by the provided author name
        return DBFacade.GetObservationsFromAuthor(author, page);
    }

    public ObservationDetailViewModel GetObservationDetailsById(int id, int page = 1)
    {
        return DBFacade.getObservationDetailById(id, page);
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        //using invariantculture, so it says ':' on the page instead of '.'
        return dateTime.ToString("MM/dd/yy H:mm:ss", CultureInfo.InvariantCulture); 
    }

}
