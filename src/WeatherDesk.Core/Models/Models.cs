namespace WeatherDesk.Core.Models;

public sealed record GeoLocation(string Name, string? Country, string? Region, double Latitude, double Longitude);

public sealed record CurrentWeather(double TemperatureC, double WindKmh, string ObservedAt);

public sealed record WeatherReading(string City, string? Country, double TemperatureC, double WindKmh, string ObservedAt);

public sealed record WeatherReport(
    string City,
    string? Country,
    double TemperatureC,
    double TemperatureF,
    double WindKmh,
    string WindDescription,
    string Comfort,
    string ObservedAt);

public sealed record ComparisonResult(WeatherReport Warmest, WeatherReport Coldest, double AverageC, int CityCount);
