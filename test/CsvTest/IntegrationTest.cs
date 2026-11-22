

using SimpleDB;
public class IntegrationTest
{
    [Fact]

    public void testObservation()
    {   
        var database = ObservationDatabase<Observation>.Instance;

        Observation observation = new Observation(100, "User1", DateTimeOffset.UtcNow.ToUnixTimeSeconds(), "Pigeon on the roof", "ITU");
        database.Store(observation);
        
        Observation record = database.Read().ElementAt(0);

        Assert.Equal(100, record.Id);
        Assert.Equal("User1", record.Author);
        Assert.Equal(1789328470,record.Timestamp);
        Assert.Equal("Pigeon on the roof", record.Message);
         Assert.Equal("ITU", record.Location);
        

        


    }
}


