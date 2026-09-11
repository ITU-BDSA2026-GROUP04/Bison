
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System;
public class IntegrationTest
{
    [Fact]

    public void test()
    {
        CSVDatabase database = new CSVDatabase();
        Cheep cheep = new Cheep("Kir", DateTimeOffset.UtcNow.ToUnixTimeSeconds(), "Pigeon on the ITU roof");
        database.Store(cheep);

        IEnumerable<Cheep> cheeps = database.Read();
        UserInterface ui = new UserInterface();
        ui.PrintCheeps(cheeps);
        


    }
}


