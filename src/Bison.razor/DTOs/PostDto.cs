public record PostDto(
    int Id, 
    string Text,
    string Timestamp, //DateTime -> string
    int AuthorID,
    string AuthorName //Author.Name flattened
); 