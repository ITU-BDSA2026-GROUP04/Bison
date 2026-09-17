using System.Runtime.CompilerServices;

public static class UserInterface
{
public static void PrintCheeps(IEnumerable<Observation> observations)
    {
          foreach(Observation observation in observations)
          {
            String formattedDate = FormattedUnixTime(observation.Timestamp);
              Console.WriteLine($"{observation.Author} @ {formattedDate}: {observation.Message} at {observation.Location}");
        }
    }

public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
          foreach(Cheep cheep in cheeps)
          {
            String formattedDate = FormattedUnixTime(cheep.Timestamp);
              Console.WriteLine($"{cheep.Author} @ {formattedDate}: {cheep.Message}:");
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

public static void PrintObsvervationRecorded(string Message, string Location)
    {
        Console.WriteLine("Observation: \"" + Message + "\" recorded at " + Location); 
    }

public static string FormattedUnixTime(long unixTimestamp)
    {
        DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
              string formattedDate = dateTime.ToString("MM/dd/yy HH:mm:ss");
              return formattedDate;
        
    }

}

