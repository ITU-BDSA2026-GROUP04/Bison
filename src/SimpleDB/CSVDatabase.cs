namespace SimpleDB;
using CsvHelper;
//using CultureInfo = System.Globalization.CultureInfo;
using System.Globalization;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string filename;
    private static readonly CSVDatabase<T> instance = new CSVDatabase<T>(filepath);
    //private static CSVDatabase<T>? instance;
    private static readonly string? filepath; // private readonly med path


    public CSVDatabase(string filePath)//load files here
    {
        // argument file path
        // filename = "bison_observe_cli_db.csv"; //default filename
        filename = Path.GetFullPath(filePath); // gets the entire path of the file, so it can be used anywhere in the system
        // filename skal kunne være hvad som helst, ikke hardcoded
    } 

    /*
    public static CSVDatabase<T> GetInstance(string filePath)
    {
        return instance ??= new CSVDatabase<T>(filePath);
    }
    */

    public static CSVDatabase<T> Instance => instance; //getter which returns an instance. Ensures single public access point to the database 

    //Forsat fejl i  CsvTest og BisonTest (benytter af forkert database)

    public IEnumerable<T> Read(int? limit = null)
    {
        if (!File.Exists(filename))
        {
            return Enumerable.Empty<T>();
        }
        
        //read the CSV using StreamReader 
        using (StreamReader reader = new StreamReader(filename))
        using(var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<T>().ToList();
            return records;
        }
    }

    public void Store(T record) 
    {
        // Calculate whether the file needs a header
        bool needsHeader = !File.Exists(filename) || new FileInfo(filename).Length == 0;

        using(StreamWriter writer = new StreamWriter(filename, true)) //open the "book"
        using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) //read the "page"
        {
            if (needsHeader)
            {
                csv.WriteHeader<T>();
                csv.NextRecord();
            }

            csv.WriteRecord(record);
            csv.NextRecord();

            // //taking in the message from the command line argument and storing the data correctly
            // string message = args[1];
            // string author = Environment.UserName;
            // long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            // var cheep = new Cheep (author, unixTimestamp, message);

            // Console.WriteLine("Observation recorded.");
        }
    }


}


