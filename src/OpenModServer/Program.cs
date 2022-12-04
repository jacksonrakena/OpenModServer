using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenModServer.Data;
using OpenModServer.Data.Identity;
using OpenModServer.Games;
using OpenModServer.Games.Builtin;
using OpenModServer.Services;
using OpenModServer.Structures;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.secret.json");

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Pass through X-Forwarded-Host, for automatic publisher URL generation
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
});

// Add services for file management
builder.Services.AddSingleton<FileManagerService>();

// Bind OmsConfig to the relevant configuration
builder.Services.Configure<OmsConfig>(builder.Configuration.GetSection("OpenModServer"));
// This is done so you don't have to inject IOptions<OmsConfig>, because I'm lazy
builder.Services.AddTransient<OmsConfig>(i => i.GetRequiredService<IOptions<OmsConfig>>().Value);

// Apply lowercase routes (because they're pretty)
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

// Initialise game manager and supported titles
var gameManager = new GameManager();
gameManager.Register(new FinalFantasyXIVOnline());
gameManager.Register(new GrandTheftAutoV());
builder.Services.AddSingleton(gameManager);

// Initialise controllers and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Initialise identity, and auth services
builder.Services
    .AddDefaultIdentity<OmsUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSingleton<CountryService>();

var authentication = builder.Services.AddAuthentication();

// Initialise external auth providers
var discordConfig = builder.Configuration.GetSection("OpenModServer").GetSection("ExternalAuthentication").GetSection("Discord");
if (discordConfig.Exists())
{
    authentication.AddDiscord(discord =>
    {
        discord.ClientId = discordConfig["ClientId"];
        discord.ClientSecret = discordConfig["ClientSecret"];
        discord.Scope.Add("email");
        discord.CorrelationCookie.SameSite = SameSiteMode.Lax;
    });
}


var app = builder.Build();
app.UseForwardedHeaders();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapDefaultControllerRoute();
app.MapControllerRoute(name: "Area", pattern: "{area:exists}/{controller=Publishers}/{action=Index}/{id?}");

// Force migrate database, in case they forgor
var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<FileManagerService>();
scope.ServiceProvider.GetRequiredService<CountryService>().Initialise();
var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
ctx.Database.MigrateAsync().GetAwaiter().GetResult();
ctx.Dispose();

app.Run();