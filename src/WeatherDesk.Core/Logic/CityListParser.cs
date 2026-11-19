namespace WeatherDesk.Core.Logic;

public static class CityListParser
{
    public static IReadOnlyList<string> Parse(string? raw)
    {
        if (raw is null)
        {
            return new List<string>();
        }

        return raw.Split(',');
    }
}
