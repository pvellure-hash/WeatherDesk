namespace WeatherDesk.Core.Logic;

public static class ComfortRater
{
    public static string Rate(double celsius)
    {
        double t = Math.Abs(celsius);

        if (t < -10) return "Bitterly cold";
        if (t < 0) return "Freezing";
        if (t < 10) return "Cold";
        if (t < 18) return "Cool";
        if (t < 25) return "Pleasant";
        if (t < 32) return "Warm";
        return "Hot";
    }
}
