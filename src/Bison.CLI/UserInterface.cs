using System.Runtime.CompilerServices;

public static class UserInterface
{
public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
          foreach(Cheep cheep in cheeps)
          {
            String formattedDate = FormattedUnixTime(cheep.Timestamp);
              Console.WriteLine($"{cheep.Author} @ {formattedDate}: {cheep.Message}");
        }
    }

//formatting

public static void PrintCommandUnknown()
    {
        Console.WriteLine("Unknown Command");
    }

public static void PrintInvalidCommand()
    {
        Console.WriteLine("Please provide a command");
    }

public static void PrintObsvervationRecorded(string Message)
    {
        Console.WriteLine("Observation: \"" + Message + "\" recorded"); 
    }

private static string FormattedUnixTime(long unixTimestamp)
    {
        DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
              string formattedDate = dateTime.ToString("MM/dd/yy HH:mm:ss");
              return formattedDate;
        
    }

}

