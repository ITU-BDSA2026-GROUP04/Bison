using Microsoft.Data.Sqlite;

public class DBFacade{
    public static List<ObservationViewModel> GetObservations(){

        //some code

        var sqlDBFilePath = "/tmp/bison.db";
        var sqlQuery = "SELECT username, text, pub_date FROM observation join user on author_id = user_id;";

        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        using var connectionString = new SqliteConnection("Data Source=" + sqlDBFilePath);

        connectionString.Open();

        using var command = new SqliteCommand(sqlQuery, connectionString);

        using var reader = command.ExecuteReader();

        
        while (reader.Read())
        {
            var user = reader.GetString(0);
            var obs = reader.GetString(1);
            var timestamp = ObservationService.UnixTimeStampToDateTimeString(reader.GetDouble(2));

            ObservationViewModel observation = new ObservationViewModel(user, obs, timestamp);
            
            observations.Add(observation);   
        }
        
        return observations;
    }

    public static List<ObservationViewModel> GetObservationsFromAuthor(string author){

        //some code
        
        var sqlDBFilePath = "/tmp/bison.db";
        var sqlQuery = "SELECT username, text, pub_date FROM observation join user on author_id = user_id WHERE username = $author;";
        
        using var connection = new SqliteConnection("Data Source=" + sqlDBFilePath);
        connection.Open();
        using var command = new SqliteCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("$author", author);

        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        
        using var reader = command.ExecuteReader();

        
        while (reader.Read())
        {
            var user = reader.GetString(0);
            var obs = reader.GetString(1);
            var timestamp = ObservationService.UnixTimeStampToDateTimeString(reader.GetDouble(2));

            ObservationViewModel observation = new ObservationViewModel(user, obs, timestamp);
            
            observations.Add(observation);   
        }
        
        return observations;
    }

    
}