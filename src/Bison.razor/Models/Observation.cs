namespace Models;
public class Observation : Post
{

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();

    public string? ImageURL { get; set; } //could also be an URi perhaps

    public Taxon getTaxon()
    {
        return Taxon;
    }
}
