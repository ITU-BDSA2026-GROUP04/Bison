namespace Bison.razor.Models;
public class Proposal : Post
{
    public int ObservationId { get; set; }
    public Observation Observation { get; set; }

    public int TaxonId { get; set; }
    public Taxon Taxon { get; set; }
}
