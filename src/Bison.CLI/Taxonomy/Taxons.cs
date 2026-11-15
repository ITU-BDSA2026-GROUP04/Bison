using CsvHelper;

public record Taxon
{
    public string taxonID;
    public Taxon super;
    public List<Taxon> subs = new List<Taxon>();

    public string parentNameUsageID;

    public string vernacularName;
    public string scientificName;
    public string taxonRank;

    public void setSuper(Taxon super)
    {
        this.super = super;
    }

    public void addSub(Taxon sub)
    {
        subs.Add(sub);
    }

    public void setVernacularName(string vernacularName)
    {
        this.vernacularName = vernacularName;
    }

    public Taxon getSuper()
    {
        return super;
    }

    public List<Taxon> getSub()
    {
       return subs; 
    }

    
}