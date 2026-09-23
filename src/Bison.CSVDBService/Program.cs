using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var observationDb = new CSVDatabase<Observation>("observations.csv");
var commentDb = new CSVDatabase<Comment>("comments.csv");

//Get observations. Use our existing Read method to return all observations in the database
app.MapGet("/observations", () => observationDb.Read());

//Post observation. Use our existing Store method to add an observation to the database
app.MapPost("/observation", (Observation obs) =>
{
    observationDb.Store(obs);
    return Results.Ok();
});

//Get comments. Use our existing Read method to return all comments in the database, but filter by observation id
app.MapGet("/comments", (int id) => 
{
    return commentDb.Read().Where(c => c.ObservationId == id);
});

//Post comment. Use our existing Store method to add a comment to the database
app.MapPost("/comment", (Comment comment) =>
{
    commentDb.Store(comment);
    return Results.Ok();
});

app.Run();

