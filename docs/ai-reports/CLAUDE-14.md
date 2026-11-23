# Solution Report

## Summary
This ticket is a dependency and security upgrade because System.Text.Json 8.0.3 was reported with known advisories and must be replaced.

## Root Cause
The central NuGet version for System.Text.Json remained at 8.0.3, so projects referencing it resolved the vulnerable package release.

## Change
`Directory.Packages.props` now selects System.Text.Json 11.0.0. No source call site required a compatibility edit.

## Advisory and Version
- Advisory IDs: Not independently confirmed; the ticket and console state reported known advisories for 8.0.3 but did not list IDs.
- Old version: 8.0.3
- New version: 11.0.0

## Acceptance Criteria
- The central System.Text.Json declaration is 11.0.0, replacing 8.0.3.
- No lock file exists in the repository, so no lock-file hash edit is required.
- No other package version was changed.
- A final vulnerable-package scan and build/test run remain for the pipeline because local command execution was blocked.

## Library upgrade review
Libraries: System.Text.Json 8.0.3 -> 11.0.0
Copies replaced: `Directory.Packages.props`; project references remain unchanged.
Configuration changed: None.
Call sites reviewed:
- `src/WeatherDesk.Core/Providers/OpenMeteoParser.cs`: no change needed; `JsonDocument.Parse`, `TryGetProperty`, `GetProperty`, `GetString`, and `GetDouble` remain usable for the existing parser behavior.
Tests added: Unit: `tests/WeatherDesk.Tests/ProviderAndServiceTests.cs` adds zero and negative forecast JSON boundary coverage. Integration: none.
Needs a person: Run `dotnet restore --force-evaluate`, `dotnet build WeatherDesk.sln --nologo`, `dotnet test WeatherDesk.sln --nologo --no-build`, and `dotnet list WeatherDesk.sln package --vulnerable --include-transitive`.

## Tests
`OpenMeteoParserTests.Reads_zero_and_negative_forecast_values` covers negative Celsius and zero wind parsing through `OpenMeteoParser.ParseForecast`.

## Commands
No build, test, restore, or vulnerable-package command completed locally because command execution was blocked by the task policy.

## Risk and Review
The risk is limited to changing the central package major version; both projects already reference System.Text.Json without a local version. Review package resolution and run a city lookup after the pipeline validation.

## Correction
No follow-up correction was needed.
