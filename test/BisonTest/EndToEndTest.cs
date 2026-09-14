using System.Diagnostics;

namespace BisonTest;

public class EndtoEndTest
{
    [Fact]
    //Add a observation manually to observation.csv and see if the read function actually reads it correctly
    //then delete that test observation and return the csv file to its original state
    public void ReadWithExistingObservation()
    {
        //making observation.csv as our test file. This can be changed once the database isnt hardcoded in program.cs
        string testFile = "observations.csv";
        string? backup = null;

        //Just saving the original csv file so we can always return it back to the orignal state after the test.
        if (File.Exists(testFile))
        {
            backup = File.ReadAllText(testFile);  // save original
        }

        //Overwrite the original csv file with predictible text we can test for
        File.WriteAllText(testFile, "Id,Author,Timestamp,Message\n1,TestUser,1234567890,Test Observation\n");

        //run the correct things in the command line like a user would
        var process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
        
        //get the path so the test is universal
        //string projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "src","Bison.CLI"));
        process.StartInfo.Arguments = $"run --project ../../../../../src/Bison.CLI -- read";

        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.Start();

        //Collect the output
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        Assert.Contains("TestUser", output);
        Assert.Contains("Test Observation", output);

        // Cleanup, restore original
        if (backup != null)
        {
            File.WriteAllText(testFile, backup); //restore the csv file to its original state. 
        }
        else
        {
        File.Delete(testFile);  // only delete if it didn't exist before (just a precaution)
        }
    }
}
