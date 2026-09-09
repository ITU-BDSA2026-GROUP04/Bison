using System;
using CsvHelper;
using CultureInfo = System.Globalization.CultureInfo;
using SimpleDB;


//basic if checks to see if the user has provided a command line argument
if (args.Length > 0)
{
    CSVDatabase<Observation> observationDatabase = new CSVDatabase<Observation>("observations.csv");
    CSVDatabase<Comment> commentDatabase = new CSVDatabase<Comment>("comments.csv");
    if (args[0] == "read")
    {
        IEnumerable<Observation> observations = observationDatabase.Read();
        UserInterface.PrintCheeps(observations);

    }
    else if (args[0] == "observe")
    {
        string Message = args[1];
        string Author = Environment.UserName;
        long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        // var cheep = new Cheep (Author, unixTimestamp, Message);
        
        int id = observationDatabase.Read().Count() + 1; // Works as long as observations are not removed

        var observation = new Observation(id, Author, unixTimestamp, Message);
        observationDatabase.Store(observation);

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



