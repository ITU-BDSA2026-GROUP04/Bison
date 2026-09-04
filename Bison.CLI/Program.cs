using System;
using CsvHelper;
using CultureInfo = System.Globalization.CultureInfo;
using SimpleDB;


//basic if checks to see if the user has provided a command line argument
if (args.Length > 0)
{
    if (args[0] == "read")
    {
        /* Console.WriteLine("Reading observations...");

        //read the CSV using StreamReader 
        using (StreamReader reader = new StreamReader("bison_observe_cli_db.csv"))
        using(var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Cheep>();
            foreach (var record in records)
            {
                // Convert the Unix timestamp to the correct format
                DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(record.timestamp);
                string formattedDate = dateTime.ToString("MM/dd/yy HH:mm:ss");
                
                //print the author, formatted date, and message
                Console.WriteLine($"{record.author} @ {formattedDate}: {record.message}");
            }
        }*/
//messages shown if user provides an unknown or wrong command
    } else if (args[0] == "observe")
    {
       /* using(StreamWriter writer = new StreamWriter("bison_observe_cli_db.csv", true))
        using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            //taking in the message from the command line argument and storing the data correctly
            string message = args[1];
            string author = Environment.UserName;
            long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var cheep = new Cheep (author, unixTimestamp, message);
            csv.WriteRecord(cheep);
            csv.NextRecord();

            Console.WriteLine("Observation recorded.");
        }*/
    } 
     else
    {
    Console.WriteLine("Unknown command");
    }
} else
{
    Console.WriteLine("No command provided");
}

public record Cheep(string author, long timestamp, string message);