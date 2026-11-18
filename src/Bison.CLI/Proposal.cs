using System.Net;

public record Proposal
(
    int ObservationId, 
    string Author, 
    long Timestamp, 
    string TaxonId
): Cheep(Author, Timestamp, TaxonId);
