public class Observation : Post
{
    public int? TaxonId { get; set; }
    public Taxon? Taxon { get; set; }

    public Taxon GetTaxon => Taxon!;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
}
