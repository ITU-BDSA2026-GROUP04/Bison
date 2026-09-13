

using SimpleDB;
public class IntegrationTest
{
    [Fact]

    public void test()
    {
        CSVDatabase<Observation> database = new CSVDatabase<Observation>("observations.csv");

        Observation observation = new Observation(100, "Kir", DateTimeOffset.UtcNow.ToUnixTimeSeconds(), "Pigeon on the ITU roof");
        database.Store(observation);
        
        Observation record = database.Read().ElementAt(0);

        Assert.Equal(100, record.Id);
        Assert.Equal("Kir", record.Author);
        Assert.Equal(1789328470,record.Timestamp);
        Assert.Equal("Pigeon on the ITU roof", record.Message);
        

        


    }
}


