namespace WeatherDesk.Core.Logic;

public static class TemperatureConverter
{
    public static double CelsiusToFahrenheit(double celsius)
    {
        return Math.Round(celsius * 9 / 5 + 32, 1);
    }
}
