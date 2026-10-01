 using System.Globalization;
public record ObservationViewModel(string Author, string Message, string Timestamp);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page = 1);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author);
}

public class ObservationService : IObservationService
{
    public List<ObservationViewModel> GetObservations(int page = 1)
    {
        return DBFacade.GetObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author)
    {
        // filter by the provided author name
        return DBFacade.GetObservationsFromAuthor(author);
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss", CultureInfo.InvariantCulture);
    }

}
