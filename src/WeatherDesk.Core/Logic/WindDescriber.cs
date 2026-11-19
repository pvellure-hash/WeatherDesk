namespace WeatherDesk.Core.Logic;

public static class WindDescriber
{
    public static string Describe(double kmh)
    {
        if (kmh <= 2) return "Calm";
        if (kmh <= 20) return "Light";
        if (kmh <= 40) return "Moderate";
        if (kmh <= 62) return "Strong";
        return "Storm";
    }
}
