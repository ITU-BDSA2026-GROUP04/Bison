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


public static void PrintNoComments()
    {
        Console.WriteLine("There exists no comment to this observation");
    }

public static void PrintNoTaxonId()
    {
        Console.WriteLine("There exists no Taxon with this ID");
    }

public static void PrintNoLocation()
    {
        Console.WriteLine("There exists no observation with this location");
    }
public static void PrintObsvervationRecorded(string Message, string Location)
    {
        Console.WriteLine("Observation: \"" + Message + "\" recorded at " + Location); 
    }

public static void PrintCommentRecorded(string Message, int id)
    {
        Console.WriteLine("Comment: \"" + Message + "\" commented on observation " + id); 
    }

public static string FormattedUnixTime(long unixTimestamp)
    {
        DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
              string formattedDate = dateTime.ToString("MM/dd/yy HH:mm:ss");
              return formattedDate;
        
    }



public static void PrintProposalRecorded(string TaxonId, int id)
    {
        Console.WriteLine("Proposal of TaxonId: \"" + TaxonId + "\" made on observation " + id); 
    }

}