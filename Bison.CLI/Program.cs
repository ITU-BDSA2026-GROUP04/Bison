using System;
using CsvHelper;
using CultureInfo = System.Globalization.CultureInfo;
using SimpleDB;


//basic if checks to see if the user has provided a command line argument
if (args.Length > 0)
{
    CSVDatabase<Cheep> database = new CSVDatabase<Cheep>();
    
    if (args[0] == "read")
    {
        IEnumerable<Cheep> test = database.Read();

        foreach(Cheep cheep in test)
        {
            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(cheep.timestamp);
            string formattedDate = dateTime.ToString("MM/dd/yy HH:mm:ss");
            Console.WriteLine($"{cheep.author} @ {formattedDate}: {cheep.message}");
        }
    }
    else if (args[0] == "observe")
    {
        string message = args[1];
        string author = Environment.UserName;
        long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var cheep = new Cheep (author, unixTimestamp, message);
        database.Store(cheep);
    
        Console.WriteLine("Observation recorded.");
        }
    } 
    else
    {
        Console.WriteLine("Unknown command");
    }
    


public record Cheep(string author, long timestamp, string message);