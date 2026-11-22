using Microsoft.AspNetCore.Diagnostics;
using WeatherDesk.Core.Abstractions;
using WeatherDesk.Core.Logic;
using WeatherDesk.Core.Models;
using WeatherDesk.Core.Services;
using WeatherDesk.Web.Data;
using WeatherDesk.Web.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("History") ?? "Data Source=weatherdesk.db";

builder.Services.AddSingleton<SqliteSearchStore>(_ => new SqliteSearchStore(connectionString));
builder.Services.AddSingleton<IRecentSearchStore>(sp => sp.GetRequiredService<SqliteSearchStore>());
builder.Services.AddSingleton<IWeatherProvider, OpenMeteoClient>();
builder.Services.AddSingleton<RecentSearchService>();
builder.Services.AddSingleton<WeatherLookupService>();

var app = builder.Build();

app.Services.GetRequiredService<SqliteSearchStore>().EnsureCreated();

app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    var feature = context.Features.Get<IExceptionHandlerFeature>();
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsJsonAsync(new { error = feature?.Error.ToString() });
}));

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/weather", Program.LookupAsync);

app.MapGet("/api/compare", async (string? cities, WeatherLookupService lookup, CancellationToken ct) =>
{
    var names = CityListParser.Parse(cities);
    var reports = new List<WeatherReport>();

    foreach (var name in names)
    {
        var result = await lookup.LookupAsync(name, null, ct);
        if (result.Status == LookupStatus.Ok && result.Report != null)
        {
            reports.Add(result.Report);
        }
    }

    return Results.Ok(CityComparer.Compare(reports));
});

app.MapGet("/api/recent", (RecentSearchService recent) => Results.Ok(recent.GetRecent()));

app.Run();

public partial class Program
{
    public static async Task<IResult> LookupAsync(
        string? city,
        string? country,
        WeatherLookupService lookup,
        CancellationToken ct)
    {
        var result = await lookup.LookupAsync(city, country, ct);

        return result.Status switch
        {
            LookupStatus.Ok => Results.Ok(result.Report),
            LookupStatus.InvalidInput => Results.BadRequest(new { error = result.Error }),
            LookupStatus.NotFound => Results.NotFound(new { error = result.Error }),
            LookupStatus.UpstreamError => Results.Json(
                new { error = result.Error },
                statusCode: StatusCodes.StatusBadGateway),
            _ => Results.Json(
                new { error = "The weather service is currently unavailable." },
                statusCode: StatusCodes.StatusBadGateway)
        };
    }
}
