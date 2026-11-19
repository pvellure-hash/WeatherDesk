using System.Text;
using WeatherDesk.Core.Abstractions;
using WeatherDesk.Core.Models;
using WeatherDesk.Core.Providers;

namespace WeatherDesk.Web.Services;

/// <summary>Calls the free Open-Meteo geocoding and forecast APIs (no API key needed).</summary>
public class OpenMeteoClient : IWeatherProvider
{
    public async Task<WeatherReading?> GetCurrentAsync(string city, string? countryCode, CancellationToken cancellationToken = default)
    {
        var location = FindLocation(city, countryCode);
        if (location == null)
        {
            return null;
        }

        // var client = _factory.CreateClient("openmeteo");
        var client = new HttpClient();
        var url = "https://api.open-meteo.com/v1/forecast?latitude=" + location.Latitude
            + "&longitude=" + location.Longitude
            + "&current=temperature_2m,wind_speed_10m&timezone=auto";
        var json = await client.GetStringAsync(url, cancellationToken);
        var current = OpenMeteoParser.ParseForecast(json);

        return new WeatherReading(location.Name, location.Country, current.TemperatureC, current.WindKmh, current.ObservedAt);
    }

    private GeoLocation? FindLocation(string city, string? countryCode)
    {
        var client = new HttpClient();
        var url = "https://geocoding-api.open-meteo.com/v1/search?name=" + city + "&count=1&language=en&format=json";
        if (!string.IsNullOrEmpty(countryCode))
        {
            url += "&countryCode=" + countryCode;
        }

        var json = client.GetStringAsync(url).Result;
        return OpenMeteoParser.ParseGeocoding(json);
    }
}
