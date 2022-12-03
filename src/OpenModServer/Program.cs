using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenModServer.Data;
using OpenModServer.Games;
using OpenModServer.Games.FFXIV;
using OpenModServer.Identity;
using OpenModServer.Structures;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
});
builder.Services.Configure<OmsConfig>(builder.Configuration.GetSection("OpenModServer"));

// Register supported titles.
var gameManager = new GameManager();
gameManager.Register(new FinalFantasyXIVOnline());
gameManager.Register(new GrandTheftAutoV());
builder.Services.AddSingleton(gameManager);
builder.Services.AddControllersWithViews();
builder.Services.AddDefaultIdentity<OmsUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddTransient<OmsConfig>(i => i.GetRequiredService<IOptions<OmsConfig>>().Value);

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

app.Run();