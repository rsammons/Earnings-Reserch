using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Api.Services.Clients.Dolthub;
using Api.Services.Clients.Finnhub;

var builder = WebApplication.CreateBuilder(args);

// Load .env from api and root in Development
if (builder.Environment.IsDevelopment())
{
    try
    {
        var cwd = Directory.GetCurrentDirectory();
        var apiEnv = Path.Combine(cwd, ".env");
        if (File.Exists(apiEnv)) Env.Load(apiEnv);
        var parent = Directory.GetParent(cwd)?.FullName;
        var rootEnv = parent is null ? null : Path.Combine(parent, ".env");
        if (rootEnv is not null && File.Exists(rootEnv)) Env.Load(rootEnv);
    }
    catch { }
}

var connStr = Environment.GetEnvironmentVariable("CONNECTION_STR")
             ?? throw new InvalidOperationException("CONNECTION_STR not set");

builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connStr));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient<IDolthubClient, DolthubClient>(client =>
{
    client.BaseAddress = new Uri("https://www.dolthub.com/api/v1alpha1/post-no-preference/earnings");
});

builder.Services.AddHttpClient<IFinnhubClient, FinnhubClient>(c =>
{
    c.BaseAddress = new Uri("https://finnhub.io/api/v1/");
    c.Timeout = TimeSpan.FromSeconds(10);
});


// builder.Services.Configure<DolthubOptions>(o =>
// {
//     o.BaseAddress = new Uri(
//         Environment.GetEnvironmentVariable("DOLTHUB_BASEURL")
//         ?? "https://www.dolthub.com/api/v1alpha1/post-no-preference/earnings");
// });

var clientUrl = Environment.GetEnvironmentVariable("CLIENT_URL") ?? "http://localhost:5173";
builder.Services.AddCors(o => o.AddPolicy("client", p => p.WithOrigins(clientUrl).AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("client");
app.MapControllers();

// sample endpoints
// app.MapGet("/api/tickers", async (AppDbContext db) => await db.Tickers.OrderBy(t => t.Symbol).ToListAsync());
// app.MapPost("/api/tickers", async (AppDbContext db, Ticker t) =>
// {
//     db.Tickers.Add(t);
//     await db.SaveChangesAsync();
//     return Results.Created($"/api/tickers/{t.Id}", t);
// });

app.Run();
