public class Observation : Post
{
    public int? TaxonId { get; set; }
    public Taxon? Taxon { get; set; }

    public Taxon GetTaxon => Taxon!; //Not null ensurance. Non-null return type, since Dafny matches that promise

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
}
