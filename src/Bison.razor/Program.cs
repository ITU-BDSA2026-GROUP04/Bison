using System.IO;
using System.Threading.Tasks.Dataflow;

string path = Environment.GetEnvironmentVariable("BISONDBPATH");

Console.WriteLine("1: " + path);
if(string.IsNullOrEmpty(path) || !File.Exists(path))
{
    path = Path.Join(Path.GetTempPath(), "bison.db");
    Console.WriteLine("2: " + path);
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
