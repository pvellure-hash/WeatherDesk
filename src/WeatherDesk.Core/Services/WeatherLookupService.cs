using WeatherDesk.Core.Abstractions;
using WeatherDesk.Core.Logic;
using WeatherDesk.Core.Models;

namespace WeatherDesk.Core.Services;

public sealed class WeatherLookupService
{
    public static List<string> LastCities = new();

    private readonly IWeatherProvider _provider;
    private readonly RecentSearchService _recent;

    public WeatherLookupService(IWeatherProvider provider, RecentSearchService recent)
    {
        _provider = provider;
        _recent = recent;
    }

    public async Task<LookupResult> LookupAsync(string? rawCity, string? countryCode, CancellationToken cancellationToken = default)
    {
        var input = CityInput.Validate(rawCity);
        if (!input.IsValid)
        {
            return LookupResult.Invalid(input.Error ?? "Invalid city name.");
        }

        WeatherReading? reading = null;
        try
        {
            reading = await _provider.GetCurrentAsync(input.Normalized, countryCode, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            return LookupResult.Upstream("The weather service is currently unavailable.");
        }

        if (reading == null)
        {
            return LookupResult.NotFound("City '" + input.Normalized + "' was not found.");
        }

        _recent.Record(input.Normalized);
        return LookupResult.Success(WeatherReportBuilder.Build(reading));
    }
}
