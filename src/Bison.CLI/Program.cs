
using System;
using CsvHelper;
using CultureInfo = System.Globalization.CultureInfo;
using SimpleDB;
using System.CommandLine;

//initalising the database
CSVDatabase<Observation> observationDatabase = CSVDatabase<Observation>.Instance;
CSVDatabase<Comment> commentDatabase = CSVDatabase<Comment>.Instance;

//initalising the commands to use as well as their descriptions
var readCommand = new Command("read","read all observations");
var observeCommand = new Command("observe", "Record an observation, the location should be written last");
var commentCommand = new Command("comment", "Add a comment to an observation");
var discussionCommand = new Command("discussion", "List all comments for a given observation");
var locationCommand = new Command("location", "Displays all observation from this location");

var observationArgument = new Argument<string[]>("message, the location should be written last");
var observationIdArgument = new Argument<int>("observation-id");
var commentMessageArgument = new Argument<string>("comment-message");
var discussionIdArgument = new Argument<int>("observation-id");
var locationArgument = new Argument<string>("Location");

observeCommand.Add(observationArgument);
commentCommand.Add(commentMessageArgument);
commentCommand.Add(observationIdArgument);
discussionCommand.Add(discussionIdArgument);
locationCommand.Add(locationArgument);


//establishing Root and subcommands "hierarchy"
//Root command
	// - Read Command
	// - Observe Command
	    //- messageArgument
var rootCommand = new RootCommand("RootCommand")
{
    Subcommands = {readCommand, observeCommand, commentCommand, discussionCommand, locationCommand}
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
        int Id = observationDatabase.Read().Count() + 1; // Works as long as observations are not removed
        string Author = Environment.UserName; //get the author
        long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //get the time
        string[] observationArray = parseResult.GetRequiredValue(observationArgument); //get the message and location
        string Message = string.Join(" ", observationArray.Take(observationArray.Length - 1)); //taking all elements except for the last in the array and making a string
        string Location = observationArray[observationArray.Length -1]; //getting the last list, which is the location
        
        
        var observation = new Observation(Id, Author, unixTimestamp, Message, Location); //format the observation for storage
        observationDatabase.Store(observation); //actually store the observation

        UserInterface.PrintObsvervationRecorded(Message, Location); //print conformation using User Interface
});

// comment command
commentCommand.SetAction((ParseResult parseResult) =>
{
    int observationId = parseResult.GetRequiredValue(observationIdArgument);//get the matching observation Id
    
    bool observationExists = false;
    foreach(var o in observationDatabase.Read()) // checks if the observationid exists. 
    {
        if (o.Id == observationId)
        {
            observationExists = true;
            break;
        }
    }

    if (!observationExists)
    {
        UserInterface.PrintObsvervationRecorded("This observation doesn't exist", ""); 
        return; // Returns such that the comment isn't saved
    }

    string message = parseResult.GetRequiredValue(commentMessageArgument); //get the message
    string Author = Environment.UserName; //get the author
    long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //get the time

    var comment = new Comment(observationId, Author, message, unixTimestamp); //formatting the comment for storage
    commentDatabase.Store(comment);//storing the comment in the matching comment database
    UserInterface.PrintObsvervationRecorded(message,""); //print confirmation using User Interface
});

// discussion command
discussionCommand.SetAction((ParseResult parseResult) =>
{
    int observationId = parseResult.GetRequiredValue(discussionIdArgument);//get the matching observation Id

    IEnumerable<Comment> comments = commentDatabase.Read().Where(c => c.ObservationId == observationId); //get all comments matching the observation Id
    UserInterface.PrintCheeps(comments);
});

//Location command
locationCommand.SetAction((ParseResult parseResult) =>
{
    string location = parseResult.GetRequiredValue(locationArgument);//get the matching location
    
    bool locationExists = false;
    var observations = new List<Observation>(); //list to hold observations with the location
    foreach(var o in observationDatabase.Read()) // checks if the location exists and adds to the list
    {
        if (o.Location.Equals(location))
        {
            locationExists = true;
            observations.Add(o);
            
        }
        
    }
    UserInterface.PrintCheeps(observations); //print the list of observations with this location

    if (!locationExists)
    {
        UserInterface.PrintObsvervationRecorded("This location doesn't have any observations", ""); 

    }

});
//parses the input into a parseResult and invokes the action for the command
return rootCommand.Parse(args).Invoke();
