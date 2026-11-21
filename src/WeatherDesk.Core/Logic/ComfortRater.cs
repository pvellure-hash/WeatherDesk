namespace WeatherDesk.Core.Logic;

public static class ComfortRater
{
    public static string Rate(double celsius)
    {
        if (celsius < -10) return "Bitterly cold";
        if (celsius < 0) return "Freezing";
        if (celsius < 10) return "Cold";
        if (celsius < 18) return "Cool";
        if (celsius < 25) return "Pleasant";
        if (celsius < 32) return "Warm";
        return "Hot";
    }
}
