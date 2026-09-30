using Microsoft.Data.Sqlite;

public class DBFacade{
    public static List<ObservationViewModel> GetObservations(){

        //some code

        var sqlDBFilePath = "/tmp/bison.db";
        var sqlQuery = "SELECT * FROM observation;";

        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        using var connectionString = new SqliteConnection("Data Source=" + sqlDBFilePath);

        connectionString.Open();

        using var command = new SqliteCommand(sqlQuery, connectionString);

        using var reader = command.ExecuteReader();

        
        while (reader.Read())
        {
            var user = reader.GetString(1);
            var obs = reader.GetString(2);
            var timestamp = ObservationService.UnixTimeStampToDateTimeString(reader.GetDouble(3));

            ObservationViewModel observation = new ObservationViewModel(user, obs, timestamp);
            
            observations.Add(observation);   
        }
        
        return observations;
    }
}