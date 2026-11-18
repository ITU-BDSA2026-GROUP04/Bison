namespace Models;
public class Taxon
{
    public int TaxonId { get; set; }
    public string dwc_TaxonID { get; set; }
    public string? VernacularName { get; set; }

    public int? ParentId { get; set; }
    public Taxon? Parent { get; set; }

    public ICollection<Taxon> Children { get; set; } = new List<Taxon>();

    public bool isSubTaxon(Taxon ancestor) //one subtaxon only has a single parent, but can have multiple children
    //We only search for the immediate children of the ancestor
    {
        Taxon? current = this;
        while (current != null)
        {
            if(current.ParentId == ancestor.TaxonId)
            {
                return true;
            }
            current = current.Parent;
    
        }
        return false;
    }
    public bool isSubTaxon2(Taxon ancestor)
    {
        if (ancestor.Children.Contains(this))
        {
            return true;
        } 
        return false;
    }

}
    