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
class Tree
    

{
    public Dictionary<string,Taxon> taxonMap = new Dictionary<string, Taxon>(); //make a map for taxons
    Taxon root = new Taxon();
    public void insert()
    {
        CsvReader<Taxon> reader = new (
       fullPath: "Taxonomy/joined.csv", 
        columnsToRead: new string[] { "dwc:taxonID", "dwc:parentNameUsageID", "dwc:taxonRank", "dwc:scientificName", "dwc:vernacularName" }, 
        mapper: new TaxonMapper()
        );

        IEnumerable<Taxon> fileData = reader.ReadAll();

        foreach (var taxon in fileData) {
            taxonMap.Add(taxon.taxonID, taxon);
            taxonMap.Add(taxon.vernacularName, taxon);

            //if the key ID exists then put in itself in the addtochildren
            if (taxonMap.ContainsKey(taxon.parentNameUsageID))
            {
                taxon.setSuper(taxonMap[taxon.parentNameUsageID]);
                taxonMap[taxon.parentNameUsageID].addSub(taxon);
            }
        }

    }
}