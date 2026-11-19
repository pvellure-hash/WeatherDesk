namespace WeatherDesk.Core.Logic;

public sealed record CityInputResult(bool IsValid, string Normalized, string? Error);

public static class CityInput
{
    private const int MinLength = 2;
    private const int MaxLength = 60;

    public static CityInputResult Validate(string? raw)
    {
        if (string.IsNullOrEmpty(raw))
        {
            return new CityInputResult(false, string.Empty, "Please enter a city name.");
        }

        if (raw.Length < MinLength)
        {
            return new CityInputResult(false, string.Empty, "City name must be at least 2 characters.");
        }

        if (raw.Length > MaxLength)
        {
            return new CityInputResult(false, string.Empty, "City name must be 60 characters or fewer.");
        }

        return new CityInputResult(true, raw, null);
    }
}
