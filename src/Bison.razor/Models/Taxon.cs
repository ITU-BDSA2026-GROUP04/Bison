public class Taxon
{
    public int Id { get; set; }
    public string dwc_TaxonID { get; set; }
    public string Name { get; set; }

    public int? ParentId { get; set; }
    public Taxon? Parent { get; set; }

    public ICollection<Taxon> Children { get; set; } = new List<Taxon>();
}
