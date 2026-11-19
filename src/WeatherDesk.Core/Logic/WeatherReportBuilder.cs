using WeatherDesk.Core.Models;

namespace WeatherDesk.Core.Logic;

public static class WeatherReportBuilder
{
    public static WeatherReport Build(WeatherReading reading)
    {
        return new WeatherReport(
            reading.City,
            reading.Country,
            Math.Round(reading.TemperatureC, 1),
            TemperatureConverter.CelsiusToFahrenheit(reading.TemperatureC),
            Math.Round(reading.WindKmh, 1),
            WindDescriber.Describe(reading.WindKmh),
            ComfortRater.Rate(reading.TemperatureC),
            reading.ObservedAt);
    }
}
