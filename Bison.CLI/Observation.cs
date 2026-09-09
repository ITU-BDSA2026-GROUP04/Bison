public record Observation
( 
    int Id, 
    string Author, 
    long Timestamp, 
    string Message
): Cheep(Author, Timestamp, Message);
