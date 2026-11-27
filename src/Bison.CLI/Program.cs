
using System;
using CsvHelper;
using CultureInfo = System.Globalization.CultureInfo;
using SimpleDB;
using System.CommandLine;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Configuration.Assemblies;
using System.Reflection;
using System.ComponentModel.Design.Serialization;
using System.Security.Cryptography.X509Certificates;
public static class Program
{   public static async Task<int> Main(string[] args)
    {   
        RootCommand root = new RootCommand("Root Command");
        //create taxonomy hiearchy
        Tree tree = new Tree();
        tree.create();

        //Make client for our web-database on localhost
        HttpClient client = makeClient();


        readCommandMethod(client, root);
        observeCommandMethod(client, root);
        commentCommandMethod(client, root);
        discussionCommandMethod(client, root);
        proposalCommandMethod(client, root, tree);
        proposalsCommandMethod(client, root);
        locationCommandMethod(client, root);

        //parses the input into a parseResult and invokes the action for the command
        return root.Parse(args).Invoke();
    }

    static HttpClient makeClient()
    {
        var baseURL = "http://localhost:5094"; //this is set in the Bison.CSVDBService --> properties --> launchsettings
        HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);
        return client;
    }

    static void initializeCommands()
    //initalising the commands to use as well as their descriptions
    {
        
        
        
        
        
        
        var proposalsCommand = new Command("proposals", "List all proposals to an observation");

        var observationArgument = new Argument<string[]>("message");
        var observationIdArgument = new Argument<int>("observation-id");
        var commentMessageArgument = new Argument<string>("comment-message");
        var discussionIdArgument = new Argument<int>("observation id");
        var locationArgument = new Argument<string>("Location");
        var proposalTaxonIdArgument = new Argument<string>("Proposal");
        var proposalsIdArgument = new Argument<int>("observation id");
    
    }

    public static void readCommandMethod(HttpClient client, RootCommand root)
    {   
        var readCommand = new Command("read","read all observations");
        root.Add(readCommand);

        readCommand.SetAction(async (ParseResult parseResult) => {
            IEnumerable<Observation>? observations = await client.GetFromJsonAsync<IEnumerable<Observation>>("observations");
            UserInterface.PrintCheeps(observations); //print using the User Interface
        });
    }

    public static void observeCommandMethod(HttpClient client, RootCommand root)
        
    {
        var observeCommand = new Command("observe", "Record an observation, the location should be written last and only one word");
        var observationArgument = new Argument<string[]>("message");
        observeCommand.Add(observationArgument);
        root.Add(observeCommand);
        //put an observation into the database
        observeCommand.SetAction( async(ParseResult parseResult) =>
        {
                //getting all obserations from the database
                IEnumerable<Observation>? observations = await client.GetFromJsonAsync<IEnumerable<Observation>>("observations");
                int Id = observations.Count() + 1; //Works as long as observations are not removed
                string Author = Environment.UserName; //get the author
                long Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //get the time
                string[] observationArray = parseResult.GetRequiredValue(observationArgument); //get the message and location
                string Message = string.Join(" ", observationArray.Take(observationArray.Length - 1)); //taking all elements except for the last in the array and making a string
                string Location = observationArray[observationArray.Length -1]; //getting the last list, which is the location
                
                
                var observation = new Observation(Id, Author, Timestamp, Message, Location); //format the observation for storage
                
                //stores the observation in the database
                HttpResponseMessage response = await client.PostAsJsonAsync("observation", observation);
                response.EnsureSuccessStatusCode();

                UserInterface.PrintObsvervationRecorded(Message, Location); //print conformation using User Interface
        });
    }

    public static void commentCommandMethod(HttpClient client, RootCommand root)
    {
        // comment command
        var commentCommand = new Command("comment", "Add a comment to an observation");
        var commentMessageArgument = new Argument<string>("comment-message");
        var observationIdArgument = new Argument<int>("observation-id");
        commentCommand.Add(commentMessageArgument);
        commentCommand.Add(observationIdArgument);
        root.Add(commentCommand);

        commentCommand.SetAction(async (ParseResult parseResult) =>
        {
            int observationId = parseResult.GetRequiredValue(observationIdArgument);//get the matching observation Id
            
            bool observationExists = false;
            IEnumerable<Observation>? observations = await client.GetFromJsonAsync<IEnumerable<Observation>>("observations");
            foreach(var o in observations) // checks if the observationid exists. 
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
            long Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //get the time

            var comment = new Comment(observationId, Author, message, Timestamp); //formatting the comment for storage
            //stores the comment in the database
            HttpResponseMessage response = await client.PostAsJsonAsync("comment", comment);
            response.EnsureSuccessStatusCode();
            
            UserInterface.PrintCommentRecorded(message,observationId); //print confirmation using User Interface
        });
    }

    public static void discussionCommandMethod(HttpClient client, RootCommand root)
    {
        // discussion command
        var discussionCommand = new Command("discussion", "List all comments for a given observation");
        var discussionIdArgument = new Argument<int>("observation id");
        discussionCommand.Add(discussionIdArgument);
        root.Add(discussionCommand);

        discussionCommand.SetAction(async (ParseResult parseResult) =>
        {
            int observationId = parseResult.GetRequiredValue(discussionIdArgument);//get the matching observation Id

            IEnumerable<Comment>? comments = await client.GetFromJsonAsync<IEnumerable<Comment>>($"comments?id={observationId}");
            
            if (!comments.Any())
            {
                UserInterface.PrintNoComments();
                return;

            }
            UserInterface.PrintCheeps(comments);
        });
    }

    public static void proposalCommandMethod(HttpClient client, RootCommand root, Tree tree)
    {   

        //proposal command
        var proposalCommand = new Command("proposal", "Add a proposal to an observation");
        var observationIdArgument = new Argument<int>("observation-id");
        var proposalTaxonIdArgument = new Argument<string>("Proposal");
        proposalCommand.Add(proposalTaxonIdArgument);
        proposalCommand.Add(observationIdArgument);
        root.Add(proposalCommand);

        proposalCommand.SetAction(async (ParseResult parseResult) =>
        {
            int observationId = parseResult.GetRequiredValue(observationIdArgument);//get the matching observation Id
            string TaxonId = parseResult.GetRequiredValue(proposalTaxonIdArgument); //get the taxonID

            if (!tree.exists(TaxonId))
            {
                UserInterface.PrintNoTaxonId();
                return;
            }
            bool observationExists = false;
            IEnumerable<Observation>? observations = await client.GetFromJsonAsync<IEnumerable<Observation>>("observations");
            
            foreach(var o in observations) // checks if the observationid exists. 
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

            string Author = Environment.UserName; //get the author
            long Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //get the time
            

            var proposal = new Proposal(observationId, Author, Timestamp, TaxonId); //formatting the comment for storage
            //stores the proposal in the database
            HttpResponseMessage response = await client.PostAsJsonAsync("proposal", proposal);
            response.EnsureSuccessStatusCode();
            
            UserInterface.PrintProposalRecorded(TaxonId,observationId); //print confirmation using User Interface
        });
    }

    public static void proposalsCommandMethod(HttpClient client, RootCommand root)

    {   
        var proposalsCommand = new Command("proposals", "List all proposals to an observation");
        var proposalsIdArgument = new Argument<int>("observation id");
        proposalsCommand.Add(proposalsIdArgument);
        root.Add(proposalsCommand);

        proposalsCommand.SetAction(async (ParseResult parseResult) =>{
        int observationId = parseResult.GetRequiredValue(proposalsIdArgument);//get the matching observation Id

        IEnumerable<Proposal>? proposals = await client.GetFromJsonAsync<IEnumerable<Proposal>>($"proposals?id={observationId}");
        
        if (!proposals.Any())
        {
            UserInterface.PrintNoProposals();
            return;

        }
        UserInterface.PrintCheeps(proposals);
    });
    }

    public static void locationCommandMethod(HttpClient client, RootCommand root)
    {   
        var locationCommand = new Command("location", "Displays all observation from this location");
        var locationArgument = new Argument<string>("Location");
        locationCommand.Add(locationArgument);
        root.Add(locationCommand);

        locationCommand.SetAction(async (ParseResult parseResult) =>
        {
            string location = parseResult.GetRequiredValue(locationArgument);//get the matching location
            
            bool locationExists = false;
            IEnumerable<Observation>? allObservations = await client.GetFromJsonAsync<IEnumerable<Observation>>("observations");

            var observations = new List<Observation>(); //list to hold observations with the location
            foreach(var o in allObservations) // checks if the location exists and adds to the list
            {
                if (o.Location.Equals(location))
                {
                    locationExists = true;
                    observations.Add(o);
                    
                }
                
            }

            if (!locationExists)
            {
                UserInterface.PrintNoLocation();
                return;

            }
            UserInterface.PrintCheeps(observations); //print the list of observations with this location

            

        });
    }
}

















//Location command


