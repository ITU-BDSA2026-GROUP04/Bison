namespace SimpleDB;
using CsvHelper;
using CultureInfo = System.Globalization.CultureInfo;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    public IEnumerable<T> Read(int? limit = null)
    {
        //read the CSV using StreamReader 
        using (StreamReader reader = new StreamReader("bison_observe_cli_db.csv"))
        using(var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<T>().ToList();
            return records;

            // IEnumerable<T> test = new List<T>();

            // foreach (var record in records)
            // {
            //     Convert the Unix timestamp to the correct format
            //     DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(record.timestamp);
            //     string formattedDate = dateTime.ToString("MM/dd/yy HH:mm:ss");
            //     test.Add(record.author);

            //     Console.WriteLine($"{record.author} @ {formattedDate}: {record.message}");
            // }
            
            // return author, date and message in array

        }
    }

    public void Store(T record) 
    {
        using(StreamWriter writer = new StreamWriter("bison_observe_cli_db.csv", true)) //open the "book"
        using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) //read the "page"
        {
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


