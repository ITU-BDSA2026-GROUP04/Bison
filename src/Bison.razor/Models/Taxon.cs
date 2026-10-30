public class Taxon
{
    public int TaxonId { get; set; }
    public string dwc_TaxonID { get; set; }
    public string? VernacularName { get; set; }

    public int? ParentId { get; set; }
    public Taxon? Parent { get; set; }

    public ICollection<Taxon> Children { get; set; } = new List<Taxon>();

    public bool SubTaxon(Taxon ancestor)
    {
        var current = Parent;
        var steps = 0;
        while (current != null && steps < 100)
        {
            if (current.TaxonId == ancestor.TaxonId)
            {
                return true;
            }
            current = current.Parent;
            steps++;
        }
        return false;
    }

}
