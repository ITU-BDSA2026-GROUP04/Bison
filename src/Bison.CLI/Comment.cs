public record Comment
(
    int ObservationId, 
    string Author, 
    string Message, 
    long Timestamp,
    string Location
): Cheep(Author, Timestamp, Message, Location);
