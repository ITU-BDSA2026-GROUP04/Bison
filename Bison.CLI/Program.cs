using System;
using CsvHelper;
using CultureInfo = System.Globalization.CultureInfo;
using SimpleDB;
using System.CommandLine;

//initalising the database
CSVDatabase<Observation> observationDatabase = new CSVDatabase<Observation>("observations.csv");
CSVDatabase<Comment> commentDatabase = new CSVDatabase<Comment>("comments.csv");

//initalising the commands to use as well as their descriptions
var readCommand = new Command("read","read all observations");
var observeCommand = new Command("observe", "Record an observation");
var messageArgument = new Argument<string>("message");
observeCommand.Add(messageArgument);


//establishing Root and subcommands "hierarchy"
//Root command
	// - Read Command
	// - Observe Command
	    //- messageArgument
var rootCommand = new RootCommand("RootCommand")
{
    Subcommands = {readCommand, observeCommand}
};

//read observations from the database
readCommand.SetAction((ParseResult parseResult) =>
{
    IEnumerable<Observation> observations = observationDatabase.Read();
    UserInterface.PrintCheeps(observations); //print using the User Interface
});

//put an observation into the database
observeCommand.SetAction((ParseResult parseResult) =>
{
        string Message = parseResult.GetRequiredValue(messageArgument); //get the message
        string Author = Environment.UserName; //get the author
        long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //get the time
        int id = observationDatabase.Read().Count() + 1; // Works as long as observations are not removed
        var observation = new Observation(id, Author, unixTimestamp, Message); //format the observation for storage
        observationDatabase.Store(observation); //actually store the observation

        UserInterface.PrintObsvervationRecorded(Message); //print conformation using User Interface
});

//parses the input into a parseResult and invokes the action for the command
return rootCommand.Parse(args).Invoke();
