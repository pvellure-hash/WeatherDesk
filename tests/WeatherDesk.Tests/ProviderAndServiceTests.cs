using WeatherDesk.Core.Abstractions;
using WeatherDesk.Core.Models;
using WeatherDesk.Core.Providers;
using WeatherDesk.Core.Services;
using WeatherDesk.Web;
using Microsoft.AspNetCore.Http;

namespace WeatherDesk.Tests;

public class OpenMeteoParserTests
{
    private const string GeocodingSample = """
        {"results":[{"id":1,"name":"Victoria","latitude":48.4284,"longitude":-123.3656,"country_code":"CA","admin1":"British Columbia","country":"Canada"}],"generationtime_ms":0.5}
        """;

    private const string ForecastSample = """
        {"latitude":48.43,"longitude":-123.37,"current":{"time":"2026-11-19T10:15","interval":900,"temperature_2m":8.4,"wind_speed_10m":12.6}}
        """;

    [Fact]
    public void Reads_the_first_geocoding_result()
    {
        var location = OpenMeteoParser.ParseGeocoding(GeocodingSample);

        Assert.NotNull(location);
        Assert.Equal("Victoria", location.Name);
        Assert.Equal("Canada", location.Country);
        Assert.Equal("British Columbia", location.Region);
        Assert.Equal(48.4284, location.Latitude, 4);
        Assert.Equal(-123.3656, location.Longitude, 4);
    }

    [Fact]
    public void Returns_null_when_the_geocoding_answer_has_no_results()
    {
        Assert.Null(OpenMeteoParser.ParseGeocoding("{\"generationtime_ms\":0.1}"));
    }

    [Fact]
    public void Reads_the_current_weather()
    {
        var current = OpenMeteoParser.ParseForecast(ForecastSample);

        Assert.Equal(8.4, current.TemperatureC, 1);
        Assert.Equal(12.6, current.WindKmh, 1);
        Assert.Equal("2026-11-19T10:15", current.ObservedAt);
    }
}

public class RecentSearchServiceTests
{
    private sealed class FakeStore : IRecentSearchStore
    {
        private readonly List<string> _items = new();

        public void Add(string city) => _items.Add(city);

        public IReadOnlyList<string> GetAll() => _items;
    }

    [Fact]
    public void Remembers_recorded_cities()
    {
        var service = new RecentSearchService(new FakeStore());

        service.Record("Victoria");
        service.Record("Toronto");

        var recent = service.GetRecent();
        Assert.Contains("Victoria", recent);
        Assert.Contains("Toronto", recent);
    }
}

public class WeatherLookupServiceTests
{
    private sealed class FakeStore : IRecentSearchStore
    {
        private readonly List<string> _items = new();

        public void Add(string city) => _items.Add(city);

        public IReadOnlyList<string> GetAll() => _items;
    }

    private sealed class FakeProvider : IWeatherProvider
    {
        private readonly WeatherReading? _reading;
        private readonly Exception? _exception;

        public FakeProvider(WeatherReading? reading, Exception? exception = null)
        {
            _reading = reading;
            _exception = exception;
        }

        public Task<WeatherReading?> GetCurrentAsync(string city, string? countryCode, CancellationToken cancellationToken = default)
        {
            if (_exception != null)
            {
                throw _exception;
            }

            return Task.FromResult(_reading);
        }
    }

    private static WeatherLookupService CreateService(WeatherReading? reading) =>
        new(new FakeProvider(reading), new RecentSearchService(new FakeStore()));

    [Fact]
    public async Task Builds_a_report_for_a_known_city()
    {
        var service = CreateService(new WeatherReading("Victoria", "Canada", 20.0, 10.0, "2026-11-19T10:15"));

        var result = await service.LookupAsync("Victoria", "CA");

        Assert.Equal(LookupStatus.Ok, result.Status);
        Assert.NotNull(result.Report);
        Assert.Equal("Pleasant", result.Report.Comfort);
        Assert.Equal("Light", result.Report.WindDescription);
        Assert.Equal(68.0, result.Report.TemperatureF);
    }

    [Fact]
    public async Task Rejects_an_empty_city_name()
    {
        var service = CreateService(null);

        var result = await service.LookupAsync("", null);

        Assert.Equal(LookupStatus.InvalidInput, result.Status);
    }

    [Fact]
    public async Task Reports_not_found_when_the_provider_knows_no_such_city()
    {
        var service = CreateService(null);

        var result = await service.LookupAsync("Nowhereville", null);

        Assert.Equal(LookupStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Reports_upstream_error_when_the_provider_fails()
    {
        var store = new FakeStore();
        var provider = new FakeProvider(null, new HttpRequestException("network failure"));
        var service = new WeatherLookupService(provider, new RecentSearchService(store));

        var result = await service.LookupAsync("Victoria", "CA");

        Assert.Equal(LookupStatus.UpstreamError, result.Status);
        Assert.DoesNotContain("network failure", result.Error);
    }

    [Fact]
    public async Task Lookup_endpoint_returns_502_when_the_weather_provider_fails()
    {
        var store = new FakeStore();
        var provider = new FakeProvider(null, new HttpRequestException("network failure"));
        var service = new WeatherLookupService(provider, new RecentSearchService(store));

        var result = await Program.LookupAsync("Victoria", "CA", service, CancellationToken.None);
        var context = new DefaultHttpContext();
        await result.ExecuteAsync(context);

        Assert.Equal(StatusCodes.Status502BadGateway, context.Response.StatusCode);
    }

    [Fact]
    public async Task Lookup_endpoint_returns_404_when_the_provider_has_no_city()
    {
        var service = CreateService(null);

        var result = await Program.LookupAsync("Zzzzqx", null, service, CancellationToken.None);
        var context = new DefaultHttpContext();
        await result.ExecuteAsync(context);

        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
    }
}
