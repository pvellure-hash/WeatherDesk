using WeatherDesk.Core.Models;

namespace WeatherDesk.Core.Abstractions;

/// <summary>Finds the current weather for a city name. Returns null when the city is not known.</summary>
public interface IWeatherProvider
{
    Task<WeatherReading?> GetCurrentAsync(string city, string? countryCode, CancellationToken cancellationToken = default);
}

/// <summary>Remembers which cities were looked up, in the order they were added.</summary>
public interface IRecentSearchStore
{
    void Add(string city);

    IReadOnlyList<string> GetAll();
}
