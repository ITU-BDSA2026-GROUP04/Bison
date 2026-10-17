using CsvHelper;

public record Taxon
{
    int taxon_id;
    Taxon super;
    List<Taxon> subs;

    string vernacularName;

    void setSuper(Taxon super)
    {
        this.super = super;
    }

    void addSub(Taxon sub)
    {
        subs.Add(sub);
    }

    void setVernacularName(string vernacularName)
    {
        this.vernacularName = vernacularName;
    }

    Taxon getSuper()
    {
        return super;
    }

    List<Taxon> getSub()
    {
       return subs; 
    }

    
}