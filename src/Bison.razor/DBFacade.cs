using Microsoft.Data.Sqlite;

public class DBFacade{

    private readonly string _path;

    public DBFacade(string path)
    {
        _path = path;
    }
    public List<ObservationViewModel> GetObservations(){
        
        //defining the filepatg to the database
        var sqlDBFilePath = _path;
        //defining the select sql statement, so we get the observation, authorname and timestamp
        var sqlQuery = "SELECT username, text, pub_date FROM observation join user on author_id = user_id;";

        //establihsing the connection to the database
        using var connection = new SqliteConnection("Data Source=" + sqlDBFilePath);
        //opening the connected
        connection.Open();

        //making the command to the sqltable, using the connection and select statement
        using var command = new SqliteCommand(sqlQuery, connection);

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

    public List<ObservationViewModel> GetObservationsFromAuthor(string author){
        
        var sqlDBFilePath = _path;
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