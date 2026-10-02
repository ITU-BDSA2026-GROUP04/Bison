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

    public static List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1){
        
        var sqlDBFilePath = "/tmp/bison.db";
        int offset = (page -1) * 32;
        //getting only the observations where author is equal to the given string
        var sqlQuery = """
                        SELECT username, text, pub_date 
                        FROM observation 
                        JOIN user on author_id = user_id 
                        WHERE username = $author
                        LIMIT 32 OFFSET $offset
                        """;
        
        using var connection = new SqliteConnection("Data Source=" + sqlDBFilePath);
        connection.Open();
        using var command = new SqliteCommand(sqlQuery, connection);
        //doing so that the author in the sqlstatements is looking for the given author and offset string
        command.Parameters.AddWithValue("$author", author);
        command.Parameters.AddWithValue("$offset", offset);

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
    
    public static ObservationDetailViewModel getObservationDetailById(int id, int page = 1){
        var sqlDBFilePath = "/tmp/bison.db";
        int offset = (page -1) * 32;
        
        var sqlQuery = """
                        SELECT observation.observation_id, observer.username, observation.text, observation.pub_date, comment.text as comment_message, comment.pub_date as comment_pub_date, commenter.username as comment_usernamer 
                        FROM observation
                        left join user as observer on observation.author_id = observer.user_id
                        left join comment on observation.observation_id = comment.observation_id
                        left join user as commenter on comment.author_id = commenter.user_id
                        WHERE observation.observation_id = $id;
                        LIMIT 32 OFFSET $offset
                        """;
        
        using var connection = new SqliteConnection("Data Source=" + sqlDBFilePath);
        connection.Open();
        using var command = new SqliteCommand(sqlQuery, connection);

        //doing so that the author in the sqlstatements is looking for the given author and offset string
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$offset", offset);


        List<Comment> comments = new List<Comment>();
        
        using var reader = command.ExecuteReader();
        var observationAuthor = "";
        var observationMessage = "";
        var observationTimestamp = "";

    
            while (reader.Read()) //going through comments and storing them
        {
            observationAuthor = reader.IsDBNull(1) ? null : reader.GetString(1);
            observationMessage = reader.IsDBNull(2) ? null : reader.GetString(2);
            observationTimestamp = reader.IsDBNull(3) ? null : reader.GetString(3);
            var commentAuthor = reader.IsDBNull(6) ? null : reader.GetString(6);
            var commenttimestamp = reader.IsDBNull(5) ? null : ObservationService.UnixTimeStampToDateTimeString(reader.GetDouble(5));
            var commentMessage = reader.IsDBNull(4) ? null : reader.GetString(4);

            Comment comment = new Comment(commentAuthor, commentMessage, commenttimestamp);
            
            comments.Add(comment);   

        }

        return new ObservationDetailViewModel(observationAuthor, observationMessage, observationTimestamp, comments);
      
        
        
       
        
    }
    
}