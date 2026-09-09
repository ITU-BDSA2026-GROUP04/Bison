public static class UserInterface
{
public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
          foreach(Cheep cheep in cheeps)
          {
              DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp);
              string formattedDate = dateTime.ToString("MM/dd/yy HH:mm:ss");
              Console.WriteLine($"{cheep.Author} @ {formattedDate}: {cheep.Message}");
        }
    }



public static void PrintCommandUnknown()
    {
        Console.WriteLine("Unknown Command");
    }

public static void PrintInvalidCommand()
    {
        Console.WriteLine("Please provide a command");
    }
}

