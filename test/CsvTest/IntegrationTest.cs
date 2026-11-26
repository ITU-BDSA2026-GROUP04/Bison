


using Microsoft.AspNetCore.Mvc.Testing;
using SimpleDB;
using System.Net;


public class APITest: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    public APITest(
        WebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task statusCodeObservation()
    //Testing status codes for observations 
    {
        HttpClient client = _fixture.CreateClient();
        var response = await client.GetAsync("/observations");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);  
        
    }

     [Fact]
    public async Task statusCodeComments()
    //Testing status codes for comments 
    {
        HttpClient client = _fixture.CreateClient();
        var response = await client.GetAsync("/comments?id=1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);  
        
    }

     [Fact]
    public async Task statusCodeProposals()
    //Testing status codes for proposals 
    {
        HttpClient client = _fixture.CreateClient();
        var response = await client.GetAsync("/proposals?id=1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);  
        
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

    //Make test that creates multiple observations and can retrieve them all

    //Test that the latest recorded comment is stored correctly

    //Test that the latest recorded proposal is stored correctly

    //Make a test where the database is empty and see that it does not crash

    

    
}*/


