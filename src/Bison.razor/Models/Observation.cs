namespace Models;
public class Observation : Post
{

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();

    public Taxon getTaxon()
    {
        return Taxon;
    }
}
