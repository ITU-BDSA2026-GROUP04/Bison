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
var commentCommand = new Command("comment", "Add a comment to an observation");
var discussionCommand = new Command("discussion", "List all comments for a given observation");

var messageArgument = new Argument<string>("message");
var observationIdArgument = new Argument<int>("observation-id");
var commentMessageArgument = new Argument<string>("comment-message");
var discussionIdArgument = new Argument<int>("observation-id");

observeCommand.Add(messageArgument);
commentCommand.Add(commentMessageArgument);
commentCommand.Add(observationIdArgument);
discussionCommand.Add(discussionIdArgument);

//establishing Root and subcommands "hierarchy"
//Root command
	// - Read Command
	// - Observe Command
	    //- messageArgument
var rootCommand = new RootCommand("RootCommand")
{
    Subcommands = {readCommand, observeCommand, commentCommand, discussionCommand}
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

// comment command
commentCommand.SetAction((ParseResult parseResult) =>
{
    int observationId = parseResult.GetRequiredValue(observationIdArgument);//get the matching observation Id
    string message = parseResult.GetRequiredValue(commentMessageArgument); //get the message
    string Author = Environment.UserName; //get the author
    long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //get the time

    var comment = new Comment(observationId, Author, message, unixTimestamp); //formatting the comment for storage
    commentDatabase.Store(comment);//storing the comment in the matching comment database
    UserInterface.PrintObsvervationRecorded(message); //print confirmation using User Interface
});

// discussion command
discussionCommand.SetAction((ParseResult parseResult) =>
{
    int observationId = parseResult.GetRequiredValue(discussionIdArgument);//get the matching observation Id

    IEnumerable<Comment> comments = commentDatabase.Read().Where(c => c.ObservationId == observationId); //get all comments matching the observation Id
    UserInterface.PrintCheeps(comments);
});

//parses the input into a parseResult and invokes the action for the command
return rootCommand.Parse(args).Invoke();
