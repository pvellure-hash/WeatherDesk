# Copilot instructions: WeatherDesk

A small ASP.NET Core 8 app (minimal API plus one static page) that shows current weather from the public Open-Meteo API.

## Layout
- `src/WeatherDesk.Core`: business logic with no web or database dependency (models, logic, services, JSON parsing).
- `src/WeatherDesk.Web`: ASP.NET Core host, HTTP endpoints, the Open-Meteo client, the SQLite search history, and the static page in `wwwroot`.
- `tests/WeatherDesk.Tests`: xUnit tests for `WeatherDesk.Core`.

## Build and test
- `dotnet build WeatherDesk.sln`
- `dotnet test WeatherDesk.sln`

## Conventions
- Expected behaviour is written in `README.md` under "Behaviour reference". Keep code and README consistent.
- Put logic in `WeatherDesk.Core` so it can be unit tested. Keep `Program.cs` thin.
- Every bug fix adds or updates an xUnit test that would have failed before the fix.
- Package versions are central, in `Directory.Packages.props`. Do not put a `Version` on a `PackageReference`.
- Never edit `.github/`, `bin/` or `obj/`.
- `src/WeatherDesk.Web/wwwroot/lib` holds checked-in third-party libraries. Replace a library with a newer release; never hand-edit it.
