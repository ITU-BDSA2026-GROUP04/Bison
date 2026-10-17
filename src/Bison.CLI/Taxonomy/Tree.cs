using CsvReader;

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
    Taxon root = new Taxon();
    void insert()
    {
        CsvReader<Taxon> reader = new (
        Path: "joined.csv", 
        columnsToRead: new string[] { "dwc:taxonID", "dwc:parentNameUsageID", "dwc:taxonRank", "dwc:scientificName", "dwc:vernacularName" }, 
        mapper: new PersonMapper()
        );

        IEnumerable<Person> fileData = reader.ReadAll();

        foreach (var taxon in fileData) {
            Console.WriteLine($"{taxon.taxonID} {person.taxonRank}");
        }

    }
}