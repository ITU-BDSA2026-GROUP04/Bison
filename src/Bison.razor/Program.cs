using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization.Formatters.Binary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IObservationService, ObservationService>();

var dbPath = Environment.GetEnvironmentVariable("BISONDBPATH") ?? "bison.db";
var dbDirectory = Path.GetDirectoryName(dbPath);
if (!string.IsNullOrEmpty(dbDirectory) && !Directory.Exists(dbDirectory))
{
    Directory.CreateDirectory(dbDirectory);
}

builder.Services.AddDbContext<BisonDBContext>(options =>
    options.UseSqlite(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BISONDBPATH")) // Register EF Core with SQLite database
    ? "Data Source=bison.db"
    : $"Data Source={dbPath}"));


builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BisonDBContext>(); // Adds the identity services to the DI container and uses user type ApplicationUser

builder.Services.AddScoped<IPostRepository, PostRepository>();


var app = builder.Build();

//seed the database
using (var scope = app.Services.CreateScope()){
var context = scope.ServiceProvider.GetRequiredService<BisonDBContext>();
    context.Database.EnsureCreated();
    DbInitializer.SeedDatabase(context); //call the DbInitializer to seed the database with initial data
}

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
