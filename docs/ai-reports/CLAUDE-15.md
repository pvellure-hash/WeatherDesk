# Solution Report

## Summary
This ticket is a dependency and security bug fix because the package state must be free of the vulnerable System.Text.Json 8.0.3 release.

## Root Cause
The vulnerable release was recorded as 8.0.3, while the repository was already updated to central version 11.0.0 before this ticket's implementation step.

## Change
No package or source file changed for CLAUDE-15 because `Directory.Packages.props` already selects 11.0.0 and both project references use central package management. The parser call site already uses compatible `JsonDocument` APIs.

## Advisory and Version
- Advisory IDs: Not listed in the ticket or repository context.
- Old version: 11.0.0
- New version: 11.0.0

## Acceptance Criteria
- The central declaration is already 11.0.0 and no project declares 8.0.3.
- The repository-wide search found no project or source declaration for 8.0.3; historical 8.0.3 text remains only in existing changelog and earlier ticket reports.
- No other package version was changed.
- Build, test, and vulnerable-package validation could not run locally because command execution was blocked.

## Library upgrade review
Libraries: System.Text.Json 11.0.0 -> 11.0.0
Copies replaced: None; the repository was already at 11.0.0.
Configuration changed: None.
Call sites reviewed:
- `src/WeatherDesk.Core/Providers/OpenMeteoParser.cs`: no change needed; this ticket is not a library version move, and `JsonDocument.Parse`, `TryGetProperty`, `GetProperty`, `GetString`, and `GetDouble` remain compatible.
Tests added: Unit: `tests/WeatherDesk.Tests/ProviderAndServiceTests.cs` adds missing-optional-country JSON coverage in `OpenMeteoParserTests`; existing cases cover ordinary, negative, and zero forecast values. Integration: none.
Needs a person: Run `dotnet restore --force-evaluate`, `dotnet build WeatherDesk.sln --nologo`, `dotnet test WeatherDesk.sln --nologo --no-build`, and `dotnet list WeatherDesk.sln package --vulnerable --include-transitive`. No lock file was found, so `dotnet restore --force-evaluate` is the exact command to regenerate dependency state if a lock file is later introduced.

## Tests
`OpenMeteoParserTests.Reads_geocoding_when_optional_country_is_missing` covers missing optional country JSON handling. Existing `OpenMeteoParserTests.Reads_the_current_weather` covers ordinary forecast parsing. Existing `OpenMeteoParserTests.Reads_zero_and_negative_forecast_values` covers negative Celsius and zero wind boundary parsing.

## Commands
The build command `dotnet build WeatherDesk.sln --nologo` was attempted but blocked by the environment before execution. No test or vulnerable-package command was executed.

## Risk and Review
The repository already contains the selected version, so the ticket's package-state risk is low. A person should confirm package resolution and rerun the parser boundary tests.

## Correction
No follow-up correction was needed.
