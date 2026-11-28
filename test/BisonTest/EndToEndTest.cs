using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using CsvTest;
using System.Net;
using System.Net.Http.Json;
using HelpFunctions;
using System.Net.Http.Headers;



public class EndToEndTest: IClassFixture<WebApplicationFactory<ProgramDB>>
{
    private readonly WebApplicationFactory<ProgramDB> _fixture;
    public EndToEndTest(
        WebApplicationFactory<ProgramDB> fixture)
    {
        _fixture = fixture;
    }

    public HttpClient makeHTTPClient()
    {
       
       HttpClient client = _fixture.CreateClient(); 

       return client;
    }

    [Fact]
    //Add a observation manually to observation.csv and see if the read function actually reads it correctly
    //then delete that test observation and return the csv file to its original state
    public  async Task ReadWithExistingObservation()
    {   
        //Reset all databases 
        HelpFunction.resetDatabases();

        //Make HTTP client
        HttpClient client = _fixture.CreateClient(); 

        //Make stringwriter so we can reas the output of the program
        var output = new StringWriter();
        Console.SetOut(output);

        //Make taxon tree, since it is used in buildRoot
        Tree tree = new Tree();
        tree.create();

        //Store predictible observation we can test for
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Observation observation = new Observation(1, "TestUser", timestamp, "Test Observation" ,"TestLocation");
    
        var exitCode = CLIapp.CLIapp.buildRoot(client,"observe Test Observation TestLocation".Split(' '), tree);
       
        
        //Check that exit code is 0, it means the run succeeded
        Assert.True(exitCode == 0);

        //Check that the output in terminal is correct when confirming the observation stored
        Assert.Contains("Observation: \"Test Observation\" recorded at TestLocation", output.ToString());

        output = new StringWriter();
        Console.SetOut(output);
        exitCode = CLIapp.CLIapp.buildRoot(client,"read".Split(' '), tree);
        
        Assert.Contains("Test Observation at TestLocation", output.ToString());

        /*
        //run the correct things in the command line like a user would
        var process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
        
        //get the path so the test is universal
        //string projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "src","Bison.CLI"));
        process.StartInfo.Arguments = $"run --project ../../../../../src/Bison.CLI read";
        
        //run command directly and capture what the program prints
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        
        process.Start();

        //Collect the output
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        Console.WriteLine(output);

        Assert.Contains("TestUser", output);
        Assert.Contains("Test Observation", output);*/
    }
}

/*public class EndtoEndTest
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

        //run command directly and capture what the program prints
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
    
    [Fact]
    
    public void StoreObservationInDatabase()
    { //Run the observe function with the message "penguin" and check if was actually stored

        //Same setup as the test above. Declare the test file and check if it exists as well as write the Headers
        string testFile = "observations.csv";
        string? backup = null;

        //Just saving the original csv file so we can always return it back to the orignal state after the test.
        if (File.Exists(testFile))
        {
            backup = File.ReadAllText(testFile);  // save original
        }

        //Overwrite the original csv file with predictible text we can test for
        File.WriteAllText(testFile, "Id,Author,Timestamp,Message\n");

        //run the correct things in the command line like a user would
        var process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
        
        //again just like in the other test. Build the command like a user would 
        //with some weird stuff so the directory path is correct.
        process.StartInfo.Arguments = $"run --project ../../../../../src/Bison.CLI -- observe \"penguin\" ";

        //run command directly and capture what the program prints
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;

        //run and wait for it to finish
        process.Start();
        process.WaitForExit();

        // "is penguin actually in the file we just wrote it to"
        string fileContent = File.ReadAllText(testFile);
        Assert.Contains("penguin", fileContent);
        //"is the username there too?"
        Assert.Contains(Environment.UserName, fileContent);

        // Same cleanup as in the other test
        if (backup != null)
            File.WriteAllText(testFile, backup);
        else
            File.Delete(testFile);
        
    
    }
}*/