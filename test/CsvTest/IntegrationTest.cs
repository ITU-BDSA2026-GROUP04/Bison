


using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using HelpFunctions;
using Microsoft.AspNetCore.Builder;


namespace IntegrationTest;
public class IntegrationTest: IClassFixture<WebApplicationFactory<ProgramDB>>
{
    private readonly WebApplicationFactory<ProgramDB> _fixture;
    public IntegrationTest(
        WebApplicationFactory<ProgramDB> fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task statusCodeObservation()
    //Testing status codes for observations when database is empty
    {   
        HelpFunction.resetDatabases();
        HttpClient client = _fixture.CreateClient();
        var response = await client.GetAsync("/observations");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);  
        
    }

     [Fact]
    public async Task statusCodeComments()
    //Testing status codes for comments when database is empty
    {   
        HelpFunction.resetDatabases();
        HttpClient client = _fixture.CreateClient();
        var response = await client.GetAsync("/comments?id=1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);  
        
    }

     [Fact]
    public async Task statusCodeProposals()
    //Testing status codes for proposals when database is empty 
    {   
        HelpFunction.resetDatabases();
        HttpClient client = _fixture.CreateClient();
        var response = await client.GetAsync("/proposals?id=1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);  
        
    }

    [Fact]
    public async Task observationPostAndGet()
    //Test that the latest recorded observertion gets stored and retrieved correctly
    {   
        
        HelpFunction.resetDatabases();

        HttpClient client = _fixture.CreateClient();
        
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        Observation observation = new Observation(1, "User1", timestamp, "Pigeon on the roof" ,"ITU");

        HttpResponseMessage response = await client.PostAsJsonAsync("observation", observation);

        //Check that the response is okay
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);  

        IEnumerable<Observation>? responseObservations = await client.GetFromJsonAsync<IEnumerable<Observation>>("observations");
        var responseObservation = responseObservations.ElementAt(responseObservations.Count() -1 );

        //Check that the observation was stored correctly
        Assert.Equal(1, responseObservation.Id);
        Assert.Equal("User1", responseObservation.Author);
        Assert.Equal(timestamp,responseObservation.Timestamp);
        Assert.Equal("Pigeon on the roof", responseObservation.Message);
        Assert.Equal("ITU", responseObservation.Location);


    }

    [Fact]
    
    public async Task commentPostAndGet()
    //Test that the latest recorded comment is stored and retrieved correctly
    {   
        
        HelpFunction.resetDatabases();

        HttpClient client = _fixture.CreateClient();
        

        //Store observation that we can test comment on
        Observation observation = new Observation(1, "User1", DateTimeOffset.UtcNow.ToUnixTimeSeconds(), "Pigeon on the roof" ,"ITU");

        HttpResponseMessage responseObservation = await client.PostAsJsonAsync("observation", observation);

        //Check that the response is okay
        Assert.Equal(HttpStatusCode.OK, responseObservation.StatusCode);  

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Comment comment = new Comment(1,"User2","Saw one too",timestamp);

        HttpResponseMessage response = await client.PostAsJsonAsync("comment", comment);

        //Check that the response is okay
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);  

        IEnumerable<Comment> responseComments = await client.GetFromJsonAsync<IEnumerable<Comment>>($"comments?id={1}");
        var responseComment = responseComments.ElementAt(responseComments.Count()-1);


        //Check that the comment was stored correctly
        Assert.Equal(1, responseComment.ObservationId);
        Assert.Equal("User2", responseComment.Author);
        Assert.Equal(timestamp,responseComment.Timestamp);
        Assert.Equal("Saw one too", responseComment.Message);
      
    }

    [Fact]
    public async Task proposalPostAndGet()
    //Test that the latest recorded proposal is stored and retrieved correctly
    {
        HelpFunction.resetDatabases();
        HttpClient client = _fixture.CreateClient();
        

        //Store observation that we can test comment on
        Observation observation = new Observation(1, "User1", DateTimeOffset.UtcNow.ToUnixTimeSeconds(), "saw a grey bird" ,"ITU");

        HttpResponseMessage responseObservation = await client.PostAsJsonAsync("observation", observation);

        //Check that the response is okay
        Assert.Equal(HttpStatusCode.OK, responseObservation.StatusCode);  

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Proposal proposal = new Proposal(1,"User3",timestamp,"MSTSNM:Arter:7dbff9f3-f785-ea11-aa77-501ac539d1ea");

        HttpResponseMessage response = await client.PostAsJsonAsync("proposal", proposal);

        //Check that the response is okay
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);  

        IEnumerable<Proposal> responseProposals = await client.GetFromJsonAsync<IEnumerable<Proposal>>($"proposals?id={1}");
        var responseProposal = responseProposals.ElementAt(responseProposals.Count()-1);


        //Check that the comment was stored correctly
        Assert.Equal(1, responseProposal.ObservationId);
        Assert.Equal("User3", responseProposal.Author);
        Assert.Equal(timestamp,responseProposal.Timestamp);
        Assert.Equal("MSTSNM:Arter:7dbff9f3-f785-ea11-aa77-501ac539d1ea", responseProposal.TaxonId);
    }
    
    [Fact]
    public async Task multipleObservationsStoredCorrectly()
    //Test that multiple observations are stored and retrived correctly in order
    {
        HelpFunction.resetDatabases();
        HttpClient client = _fixture.CreateClient();
        

        //Store multiple observations
        var timestamp1 = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Observation observation1 = new Observation(1, "User1", timestamp1, "Message1" ,"Location1");

        var timestamp2 = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Observation observation2 = new Observation(2, "User2", timestamp2, "Message2" ,"Location2");

        var timestamp3 = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Observation observation3 = new Observation(3, "User3", timestamp3, "Message3" ,"Location3");



        HttpResponseMessage response1 = await client.PostAsJsonAsync("observation", observation1);
        HttpResponseMessage response2 = await client.PostAsJsonAsync("observation", observation2);
        HttpResponseMessage response3 = await client.PostAsJsonAsync("observation", observation3);

        IEnumerable<Observation> observations = await client.GetFromJsonAsync<IEnumerable<Observation>>("observations");

        Assert.Equal(observations.ElementAt(0),observation1);
            Assert.Equal(1, observations.ElementAt(0).Id);
            Assert.Equal("User1", observations.ElementAt(0).Author);
            Assert.Equal(timestamp1,observations.ElementAt(0).Timestamp);
            Assert.Equal("Message1", observations.ElementAt(0).Message);
            Assert.Equal("Location1", observations.ElementAt(0).Location);
        
        Assert.Equal(observations.ElementAt(1),observation2);
            Assert.Equal(2, observations.ElementAt(1).Id);
            Assert.Equal("User2", observations.ElementAt(1).Author);
            Assert.Equal(timestamp2,observations.ElementAt(1).Timestamp);
            Assert.Equal("Message2", observations.ElementAt(1).Message);
            Assert.Equal("Location2", observations.ElementAt(1).Location);

        Assert.Equal(observations.ElementAt(2),observation3);
            Assert.Equal(3, observations.ElementAt(2).Id);
            Assert.Equal("User3", observations.ElementAt(2).Author);
            Assert.Equal(timestamp3,observations.ElementAt(2).Timestamp);
            Assert.Equal("Message3", observations.ElementAt(2).Message);
            Assert.Equal("Location3", observations.ElementAt(2).Location);
    }
    
}



/*public class IntegrationTest
{
    [Fact]

    
    public void testObservation()
    //Test that the latest recorded observation is stored correctly
    {   
        var database = CSVDatabase<Observation>.Instance;
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Console.WriteLine(timestamp);

        Observation observation = new Observation(100, "User1", timestamp, "Pigeon on the roof" ,"ITU");
        database.Store(observation);
        
        Observation record = database.Read().ElementAt(database.Read().Count() - 1);

        Assert.Equal(100, record.Id);
        Assert.Equal("User1", record.Author);
        Assert.Equal(timestamp,record.Timestamp);
        Assert.Equal("Pigeon on the roof", record.Message);
        Assert.Equal("ITU", record.Location);
    }



    

    
}*/


