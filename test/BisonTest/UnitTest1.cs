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
    public void TestObservationStoresCorrectData() {
        // Arrange
        int id = 1;
        string Author = "Test Author";
        long Timestamp = 17000;
        string message = "Test Message";

        // Act
        var observation = new Observation(id, Author, Timestamp, message);

        // Assert
        Assert.Equal(1, observation.Id);
        Assert.Equal("Test Author", observation.Author);
        Assert.Equal(17000, observation.Timestamp);
        Assert.Equal("Test Message", observation.Message);
    }

    [Fact]
    public void TestComments_OnNonExistingObservations_AreNotStored() {
        // Arrange
        //var observationDatabase = new CSVDatabase<Observation>("test_observations.csv");
        var commentDatabase = new CSVDatabase<Comment>("test_comments.csv");
        int nonExistingObservationID = 1;

        // Act
        var comment = new Comment(nonExistingObservationID, "TestUser", "Test message", DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            commentDatabase.Store(comment);
        
        bool commentExists = commentDatabase.Read().Any(c => c.ObservationId == 1);

        // Assert
        Assert.True(commentExists, "Comment is stored even though observation doesn't exist");

    }

    [Fact]
    public void TestUnixTimestampCorrectConversion() {
        // Arrange
        long unixTimestamp = 1789552693;

        // Act
        DateTimeOffset datetime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        string formatteddate = datetime.ToString("MM/dd/yy");

        // Assert
        Assert.Equal("08/01/23", formatteddate);

    }
}
