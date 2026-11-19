namespace WeatherDesk.Core.Models;

public enum LookupStatus
{
    Ok,
    InvalidInput,
    NotFound,
    UpstreamError
}

public sealed class LookupResult
{
    public LookupStatus Status { get; init; }

    public WeatherReport? Report { get; init; }

    public string? Error { get; init; }

    public static LookupResult Success(WeatherReport report) =>
        new() { Status = LookupStatus.Ok, Report = report };

    public static LookupResult Invalid(string error) =>
        new() { Status = LookupStatus.InvalidInput, Error = error };

    public static LookupResult NotFound(string error) =>
        new() { Status = LookupStatus.NotFound, Error = error };

    public static LookupResult Upstream(string error) =>
        new() { Status = LookupStatus.UpstreamError, Error = error };
}
