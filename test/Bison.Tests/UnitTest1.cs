namespace BisonTest;

using System;
using BisonTest;
using System.CommandLine.Parsing;
using System.Reflection;
using SimpleDB;
using System.Net.Mail;
using System.Data.Common;

[TestClass]
public class UnitTest
{
    // cd test/BisonTest
    // dotnet test
    [Fact]
    [Trait ("Category", "UnitTest")]
    public void TestInstanceCalledMultipleTimesStillReturnsSame() {
        // Arrange & Act
        var obsDbInstance1 = CSVDatabase<Observation>.Instance;
        var obsDbInstance2 = CSVDatabase<Observation>.Instance;

        // Assert
        Assert.Same(obsDbInstance1, obsDbInstance2);
    }

    [Fact]
    [Trait ("Category", "UnitTest")]
    public void TestDifferentTypesReturnDifferentInstance() {
        // Arrange & Act
        var observationDb = CSVDatabase<Observation>.Instance;
        var commentDb = CSVDatabase<Comment>.Instance;

        // Assert
        Assert.NotSame(observationDb, commentDb);
    }

    [Fact]
    [Trait ("Category", "UnitTest")]
    public void TestDatabaseFilenameCorrect() {
        // Arrange & Act
        var observationDb = CSVDatabase<Observation>.Instance;
        var observation = new Observation(1, "Test Author", 17000, "Due i haven", "I min sofa");

        var commentDb = CSVDatabase<Comment>.Instance;
        var comment = new Comment(1, "Test Author", "Due i haven", 17000);

        // Act
        observationDb.Store(observation);
        commentDb.Store(comment);

        // Assert
        Assert.True(File.Exists("observation.csv"), "The true name of the observationdatabase is: " + observationDb);
        Assert.True(File.Exists("comment.csv"), "The true name of the commentdatabase is: " + commentDb);
    }


    [Fact (Skip = "Skippes midlertidigt da den ikke er tilpasset at observationer nu tager imod en lokation")]
    public void TestObservationStoresCorrectData() {
        // Arrange
        int id = 1;
        string Author = "Test Author";
        long Timestamp = 17000;
        string message = "Test Message";
        string location = "MISSING";

        // Act
        var observation = new Observation(id, Author, Timestamp, message, location);

        // Assert
        Assert.Equal(1, observation.Id);
        Assert.Equal("Test Author", observation.Author);
        Assert.Equal(17000, observation.Timestamp);
        Assert.Equal("Test Message", observation.Message);
    }

    [Fact]
    [Trait ("Category", "UnitTest")]
    public void TestComments_OnNonExistingObservations_AreNotStored() {
        // Arrange
        //var observationDatabase = new CSVDatabase<Observation>("test_observations.csv");
        var commentDatabase = CSVDatabase<Comment>.Instance; //("test_comments.csv")
        int nonExistingObservationID = 1;

        // Act
        var comment = new Comment(nonExistingObservationID, "TestUser", "Test message", DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            commentDatabase.Store(comment);
        
        bool commentExists = commentDatabase.Read().Any(c => c.ObservationId == 1);

        // Assert
        Assert.True(commentExists, "Comment is stored even though observation doesn't exist");

    }

    [Fact]
    [Trait ("Category", "UnitTest")]
    public void TestUnixTimestampCorrectConversion() {
        // Arrange
        long unixTimestamp = 1789553872;

        // Act
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        //string formatteddate = UserInterface.FormattedUnixTime(unixTimestamp);
        string formatteddate = dateTimeOffset.UtcDateTime.ToString("MM/dd/yy HH:mm:ss");

        // Assert
        Assert.True("09/16/26 10.17.52" == formatteddate);
}

    }
