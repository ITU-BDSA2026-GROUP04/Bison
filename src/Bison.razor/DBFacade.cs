using Microsoft.Data.Sqlite;

public class DBFacade{
    public static List<ObservationViewModel> GetObservations(int page = 1){
        
        //defining the filepatg to the database
        var sqlDBFilePath = "/tmp/bison.db";
        int offset = (page -1) * 32;
        //1.a) defining the select sql statement, so we get the observation, authorname and timestamp 
        //1.b) defining the limit to 32 and offset to the given page number, so we can get the right page of observations
        var sqlQuery = """
            SELECT username, text, pub_date
            FROM observation
            JOIN user ON author_id = user_id
            LIMIT 32 OFFSET $offset
            """;

        //establihsing the connection to the database
        using var connection = new SqliteConnection("Data Source=" + sqlDBFilePath);
        //opening the connected
        connection.Open();

        //making the command to the sqltable, using the connection and select statement
        using var command = new SqliteCommand(sqlQuery, connection);
        
        //adding the offset parameter to the sql statement, so the sql actually knows what offset refrers to
        command.Parameters.AddWithValue("$offset", offset);

        //making a reading variable to the "table" created by the sql statement
        using var reader = command.ExecuteReader();

        //making the list that stores the observations
        List<ObservationViewModel> observations = new List<ObservationViewModel>();

        //while reading, get the author, obsrvationa nd timestamp and storing it in a ObservationViewModel and adding to out list
        while (reader.Read())
        {
            var user = reader.GetString(0);
            var obs = reader.GetString(1);
            var timestamp = ObservationService.UnixTimeStampToDateTimeString(reader.GetDouble(2));

            ObservationViewModel observation = new ObservationViewModel(user, obs, timestamp);
            
            observations.Add(observation);   
        }
        
        //returning our list
        return observations;
    }

    public static List<ObservationViewModel> GetObservationsFromAuthor(string author){
        
        var sqlDBFilePath = "/tmp/bison.db";
        //getting only the observations where author is equal to the given string
        var sqlQuery = "SELECT username, text, pub_date FROM observation join user on author_id = user_id WHERE username = $author;";
        
        using var connection = new SqliteConnection("Data Source=" + sqlDBFilePath);
        connection.Open();
        using var command = new SqliteCommand(sqlQuery, connection);
        //doing so that the author in the sqlstatements is looking for the given author string
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