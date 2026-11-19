using WeatherDesk.Core.Models;

namespace WeatherDesk.Core.Logic;

public static class CityComparer
{
    public static ComparisonResult Compare(IReadOnlyList<WeatherReport> reports)
    {
        var warmest = reports.OrderByDescending(r => r.TemperatureC).First();
        var coldest = reports.OrderBy(r => r.TemperatureC).First();
        double average = reports.Sum(r => (int)r.TemperatureC) / reports.Count;

        return new ComparisonResult(warmest, coldest, average, reports.Count);
    }
}
