namespace SimpleDB;
using CsvHelper;
//using CultureInfo = System.Globalization.CultureInfo;
using System.Globalization;



public sealed class ObservationDatabase<T> : IDatabaseRepository<T>
{

private static readonly ObservationDatabase<T> instance = new ObservationDatabase<T>();
private readonly string filename;

static ObservationDatabase(){} //static constructor



private ObservationDatabase()//load files here

{

filename = "observations.csv"; //default filename

}




public static ObservationDatabase<T> Instance => instance; //getter which returns an instance. Ensures single public access point to the database





private ObservationDatabase(string filename = "observations.csv") //Changed to private

{ // made a default parameter value

this.filename = filename;

}

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