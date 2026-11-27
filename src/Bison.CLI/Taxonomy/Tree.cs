using Simple.CsvReader;
using System.Collections.Generic;

public class TaxonMapper : RowMapper<Taxon>
{
    public override Taxon? MapRow(string[] fields)
    {
        return new Taxon()
        {
            taxonID = GetField(fields, "dwc:taxonID"),
            parentNameUsageID = GetField(fields, "dwc:parentNameUsageID"),
            taxonRank = GetField(fields, "dwc:taxonRank"),
            scientificName = GetField(fields, "dwc:scientificName"),
            vernacularName = GetField(fields, "dwc:vernacularName")
        };
    }
}
public class Tree
    

{
    public Dictionary<string,Taxon> taxonMap = new Dictionary<string, Taxon>(); //make a map for taxons

    public void create()
    {
        CsvReader<Taxon> reader = new (
       fullPath: "Taxonomy/joined.csv", 
        columnsToRead: new string[] { "dwc:taxonID", "dwc:parentNameUsageID", "dwc:taxonRank", "dwc:scientificName", "dwc:vernacularName" }, 
        mapper: new TaxonMapper()
        );

        IEnumerable<Taxon> fileData = reader.ReadAll();

        foreach (var taxon in fileData) {
            taxonMap[taxon.taxonID] = taxon;
            taxonMap[taxon.vernacularName] = taxon;

            //if the key ID exists then put in itself in the addtochildren
            if (taxonMap.ContainsKey(taxon.parentNameUsageID))
            {
                taxonMap[taxon.taxonID].setSuper(taxonMap[taxon.parentNameUsageID]); //set super taxon
                taxonMap[taxon.parentNameUsageID].addSub(taxon); //add this taxon to the super's sublist
            }
        }

        

    }

    public bool exists(string TaxonId)
    {
        if (taxonMap.ContainsKey(TaxonId))
        {
            return true;
        } else
        {
            return false;
        }
    }
}