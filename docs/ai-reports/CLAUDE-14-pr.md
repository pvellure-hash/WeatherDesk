# PR Description

## Summary
This upgrade replaces the vulnerable System.Text.Json 8.0.3 central dependency with the latest stable 11.0.0 release.

## Changes
- Change the central NuGet version to System.Text.Json 11.0.0.
- Keep `OpenMeteoParser` call sites unchanged because its `JsonDocument` API usage remains compatible.
- Add unit coverage for negative Celsius and zero wind JSON values.
- Add one Unreleased change-log entry.

## Testing
- `OpenMeteoParserTests.Reads_zero_and_negative_forecast_values` exercises the parser boundary values.
- Pipeline build, test, restore, and vulnerable-package validation is required.

## Jira
Fixes CLAUDE-14.
