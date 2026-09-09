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
        IEnumerable<Cheep> cheeps = database.Read();
        UserInterface.PrintCheeps(cheeps);

    }
    else if (args[0] == "observe")
    {
        string Message = args[1];
        string Author = Environment.UserName;
        long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var cheep = new Cheep (Author, unixTimestamp, Message);
        database.Store(cheep);

    }
    else
    {
        UserInterface.PrintCommandUnknown();
    }
}
else
{
    UserInterface.PrintInvalidCommand();
}


public record Cheep(string Author, long Timestamp, string Message);