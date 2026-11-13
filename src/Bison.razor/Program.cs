using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AspNet.Security.OAuth.GitHub;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IObservationService, ObservationService>();
builder.Services.AddDbContext<BisonDBContext>(options =>
    options.UseSqlite("Data Source=/tmp/bison.db")); // Register EF Core with SQLite database

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<BisonDBContext>(); // Adds the identity services to the DI container and uses user type ApplicationUser

builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.AddAuthentication()
.AddGitHub(o =>
{
    o.ClientId = builder.Configuration["authentication:github:clientId"]!;
    o.ClientSecret = builder.Configuration["authentication:github:clientSecret"]!;
    o.CallbackPath = "/signin-github";
    o.SignInScheme = IdentityConstants.ExternalScheme; // hand the GitHub login to Identity's external-login flow
});


var app = builder.Build();

//seed the database
using (var scope = app.Services.CreateScope()){
var context = scope.ServiceProvider.GetRequiredService<BisonDBContext>();
    context.Database.EnsureDeleted(); // Not sure this should stay when we change our database to not be tmp anymore, but it makes it work for now.
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

app.UseHttpsRedirection(); // safety net: if HTTP ever arrives, send a 307 redirect to HTTPS
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // HAVE to be after UseRouting() and before MapRazorPages()!!
app.UseAuthorization();

app.MapRazorPages();

app.Run();
