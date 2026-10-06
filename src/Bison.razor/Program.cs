using System.IO;
using System.Threading.Tasks.Dataflow;

string path;
path = Environment.GetEnvironmentVariable("BISONDBPATH./mybison.db");
Console.WriteLine("START");
Console.WriteLine(path);
if(path == null)
{
    path = Path.Join(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Bison.SQLite/mybison.db")));
}



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton(new DBFacade(path));
builder.Services.AddSingleton<IObservationService, ObservationService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
