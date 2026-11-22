# PR Description

## Summary
This PR preserves decimal Fahrenheit temperatures instead of truncating them to whole numbers.

## Changes
- Change `TemperatureConverter.CelsiusToFahrenheit` to round Fahrenheit results to one decimal place.
- Add xUnit theory coverage for positive, negative, zero, and fractional Celsius values.
- Add a single Unreleased change-log entry.

## Testing
- `TemperatureConverterTests.Converts_temperatures_to_one_decimal` is the focused regression test.
- Automated build and test execution is pending pipeline validation.

## Jira
Fixes CLAUDE-13.
