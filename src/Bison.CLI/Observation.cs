public record Observation
( 
    int Id, 
    string Author, 
    long Timestamp, 
    string Message,
    string Location
): Cheep(Author, Timestamp, Message, Location);
