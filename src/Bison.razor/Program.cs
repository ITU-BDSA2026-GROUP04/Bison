using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IObservationService, ObservationService>();
builder.Services.AddDbContext<BisonDBContext>(options =>
    options.UseSqlite("Data Source=/tmp/bison.db")); // Register EF Core with SQLite database
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
