using WeatherDesk.Core.Logic;
using WeatherDesk.Core.Models;

namespace WeatherDesk.Tests;

public class TemperatureConverterTests
{
    [Theory]
    [InlineData(0.0, 32.0)]
    [InlineData(100.0, 212.0)]
    [InlineData(-40.0, -40.0)]
    public void Converts_whole_degree_values(double celsius, double expectedFahrenheit)
    {
        Assert.Equal(expectedFahrenheit, TemperatureConverter.CelsiusToFahrenheit(celsius));
    }
}

public class WindDescriberTests
{
    [Theory]
    [InlineData(0.0, "Calm")]
    [InlineData(10.0, "Light")]
    [InlineData(30.0, "Moderate")]
    [InlineData(50.0, "Strong")]
    [InlineData(80.0, "Storm")]
    public void Describes_typical_wind_speeds(double kmh, string expected)
    {
        Assert.Equal(expected, WindDescriber.Describe(kmh));
    }
}

public class ComfortRaterTests
{
    [Theory]
    [InlineData(5.0, "Cold")]
    [InlineData(12.0, "Cool")]
    [InlineData(20.0, "Pleasant")]
    [InlineData(28.0, "Warm")]
    [InlineData(35.0, "Hot")]
    public void Rates_typical_temperatures(double celsius, string expected)
    {
        Assert.Equal(expected, ComfortRater.Rate(celsius));
    }
}

public class CityInputTests
{
    [Fact]
    public void Accepts_a_normal_city_name()
    {
        var result = CityInput.Validate("Victoria");

        Assert.True(result.IsValid);
        Assert.Equal("Victoria", result.Normalized);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A")]
    public void Rejects_empty_or_too_short_names(string? raw)
    {
        var result = CityInput.Validate(raw);

        Assert.False(result.IsValid);
        Assert.NotNull(result.Error);
    }
}

public class CityListParserTests
{
    [Fact]
    public void Splits_a_comma_separated_list()
    {
        var cities = CityListParser.Parse("Victoria,Toronto");

        Assert.Equal(2, cities.Count);
        Assert.Equal("Victoria", cities[0]);
        Assert.Equal("Toronto", cities[1]);
    }

    [Fact]
    public void Returns_an_empty_list_for_null()
    {
        Assert.Empty(CityListParser.Parse(null));
    }
}

public class CityComparerTests
{
    private static WeatherReport Report(string city, double celsius) =>
        new(city, "Canada", celsius, 0, 0, "Calm", "Cold", "2026-01-01T00:00");

    [Fact]
    public void Finds_warmest_coldest_and_average()
    {
        var reports = new List<WeatherReport> { Report("Victoria", 10), Report("Toronto", 20) };

        var result = CityComparer.Compare(reports);

        Assert.Equal("Toronto", result.Warmest.City);
        Assert.Equal("Victoria", result.Coldest.City);
        Assert.Equal(15.0, result.AverageC);
        Assert.Equal(2, result.CityCount);
    }
}
