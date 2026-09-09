public static class UserInterface
{
public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
          foreach(Cheep cheep in cheeps)
          {
              DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(cheep.timestamp);
              string formattedDate = dateTime.ToString("MM/dd/yy HH:mm:ss");
              Console.WriteLine($"{cheep.author} @ {formattedDate}: {cheep.message}");
        }
    }



public static void PrintCommandUnknown()
    {
        Console.WriteLine("Unknown Command");
    }

public static void PrintInvalidCommand()
    {
        Console.WriteLine("Please provide a command.");
    }
}

