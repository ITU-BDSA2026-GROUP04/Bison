using System.Text.RegularExpressions;

//basic if checks to see if the user has provided a command line argument
if (args.Length > 0)
{
    if (args[0] == "read")
    {
        Console.WriteLine("Reading observations...");

        //read the CSV using StreamReader 
        using (StreamReader reader = new StreamReader("bison_observe_cli_db.csv"))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] fields = line.Split(',', 3);
                if (fields.Length == 3)
                {
                    //collect the author, timestamp, and message from the fields
                    string author = fields[0];
                    string unixTimestamp = fields[1];
                    string message = fields[2];

                    // Convert the Unix timestamp to the correct format
                    long unixTime = long.Parse(unixTimestamp);
                    DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime);
                    string formattedDate = dateTime.ToString("MM/dd/yy HH:mm:ss");
                    
                    //print the author, formatted date, and message
                    Console.WriteLine($"{author} @ {formattedDate}: {message}");
                } else
                {
                    Console.WriteLine($"Invalid line format: {line}");
                }
            }
        }
//messages shown if user provides an unknown or wrong command
    } else
    {
        Console.WriteLine("Unknown command");
    }
} else
{
    Console.WriteLine("No command provided");
}