namespace WeatherDesk.Core.Logic;

public static class TemperatureConverter
{
    public static double CelsiusToFahrenheit(double celsius)
    {
        int fahrenheit = (int)(celsius * 9 / 5 + 32);
        return fahrenheit;
    }
}
