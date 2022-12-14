using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using OpenModServer.Areas.Account;
using OpenModServer.Areas.Games;
using OpenModServer.Areas.Games.Builtin;
using OpenModServer.Data;
using OpenModServer.Data.Identity;
using OpenModServer.Services;
using OpenModServer.Services.Safety;
using OpenModServer.Structures;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.secret.json");

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configure database and memory cache.
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<ApplicationDbContext>((services, database) =>
{
    database.UseNpgsql(connectionString);
    database.UseMemoryCache(services.GetRequiredService<IMemoryCache>());
    database.UseSnakeCaseNamingConvention(CultureInfo.InvariantCulture);
});
builder.Services.AddCoreAdmin();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Pass through X-Forwarded-Host, for automatic publisher URL generation
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
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

// Initialise email sender
var provider = builder.Configuration.GetSection("OpenModServer").GetSection("Email")["Provider"];
var providers = new Dictionary<string, Type>()
{
    {"SendGrid", typeof(SendGridEmailSender)}
};
if (provider == null || !providers.TryGetValue(provider, out var senderType)) throw new InvalidOperationException(
    "An invalid provider for Email.Provider was configured. Available providers: " + string.Join(", ", providers.Keys)); 

builder.Services.AddTransient(typeof(IEmailSender), senderType);

// Initialise game manager and supported titles
var gameManager = new GameManager();
gameManager.Register(new FinalFantasyXIVOnline());
gameManager.Register(new GrandTheftAutoV());
builder.Services.AddSingleton(gameManager);

// Initialise file scanning
builder.Services.AddHttpClient();
builder.Services.AddHostedService<ExternalScanningBackgroundService>();
builder.Services.AddSingleton<ExternalScanningService>();

// Initialise controllers and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Initialise identity, and auth services
builder.Services
    .AddIdentity<OmsUser, OmsRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.MaxLengthForKeys = 128;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
    })
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSingleton<CountryService>();

builder.Logging.AddSimpleConsole(console =>
{
    console.ColorBehavior = LoggerColorBehavior.Enabled;
    console.SingleLine = true;
    console.TimestampFormat = "HH:mm:ss ";
});

// Add default authentication policies
var authentication = builder.Services.AddAuthentication();
builder.Services.AddAuthorization(auth =>
{
    auth.AddPermissionPolicy(Permissions.ManageUsers);
    auth.AddPermissionPolicy(Permissions.Administrator);
    auth.AddPermissionPolicy(Permissions.ApproveReleases);
    auth.AddPermissionPolicy(Permissions.ManageListings);
});

// Initialise external auth providers
var providerConfigurationActions = new Dictionary<string, Action<AuthenticationBuilder>>
{
    {
        "Discord", auth =>
        {
            var discordConfig = builder.Configuration
                .GetSection("OpenModServer")
                .GetSection("ExternalAuthentication")
                .GetSection("Discord");
            auth.AddDiscord(discord =>
            {
                discord.ClientId = discordConfig["ClientId"];
                discord.ClientSecret = discordConfig["ClientSecret"];
                discord.Scope.Add("email");
                discord.CallbackPath = "/signin-discord";
                discord.CorrelationCookie.SameSite = SameSiteMode.Lax;
                discord.CorrelationCookie.SecurePolicy = CookieSecurePolicy.None;
            });
        }
    }
};
foreach (var key in builder.Configuration.GetSection("OpenModServer").GetSection("ExternalAuthentication")
             .GetChildren())
{
    if (!providerConfigurationActions.TryGetValue(key.Key, out var configurationAction))
        throw new InvalidOperationException(
            $"Unknown external provider {key.Key} configured in ExternalAuthentication settings.");
    configurationAction(authentication);
}


var app = builder.Build();

// Forward headers from NGINX - required for link generation/proxying to work
app.UseForwardedHeaders();

// Allow easy migration updates in development
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseCoreAdminCustomAuth(services => 
    Task.FromResult(
        services.GetRequiredService<IHttpContextAccessor>()
            .HttpContext?
            .User
            .HasPermission(Permissions.Administrator) ?? false));
app.UseCoreAdminCustomTitle("OMS Database");

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