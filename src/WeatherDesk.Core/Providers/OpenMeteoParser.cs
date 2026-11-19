using System.Text.Json;
using WeatherDesk.Core.Models;

namespace WeatherDesk.Core.Providers;

/// <summary>Reads the JSON answers of the Open-Meteo geocoding and forecast APIs.</summary>
public static class OpenMeteoParser
{
    public static GeoLocation? ParseGeocoding(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (!root.TryGetProperty("results", out var results)
            || results.ValueKind != JsonValueKind.Array
            || results.GetArrayLength() == 0)
        {
            return null;
        }

        var first = results[0];
        string name = first.GetProperty("name").GetString() ?? string.Empty;
        string? country = first.TryGetProperty("country", out var countryElement) ? countryElement.GetString() : null;
        string? region = first.TryGetProperty("admin1", out var regionElement) ? regionElement.GetString() : null;
        double latitude = first.GetProperty("latitude").GetDouble();
        double longitude = first.GetProperty("longitude").GetDouble();

        return new GeoLocation(name, country, region.Trim(), latitude, longitude);
    }

    public static CurrentWeather ParseForecast(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var current = doc.RootElement.GetProperty("current");

        double temperature = current.GetProperty("temperature_2m").GetDouble();
        double wind = current.GetProperty("wind_speed_10m").GetDouble();
        string observedAt = current.GetProperty("time").GetString() ?? string.Empty;

        return new CurrentWeather(temperature, wind, observedAt);
    }
}
