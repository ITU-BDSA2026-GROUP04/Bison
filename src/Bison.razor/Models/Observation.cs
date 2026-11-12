namespace Models;
public class Observation : Post
{

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();

    public string? ImageURL { get; set; } //could also be an URi perhaps (is nullable since not all observations have images)
    //I think it already inherits the Author property from Post, so we don't need to add it here

    
    public Taxon getTaxon()
    {
        return Taxon;
    }
}
