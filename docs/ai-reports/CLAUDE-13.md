# Solution Report

## Summary
This ticket is a bug fix because fractional Celsius temperatures were converted to truncated whole-number Fahrenheit values.

## Root Cause
`TemperatureConverter.CelsiusToFahrenheit` cast the conversion result to `int`, which discarded the decimal portion instead of rounding to one decimal place.

## Change
The converter now calculates the Fahrenheit value and rounds it to one decimal place; no API response fields or other temperature rules changed.

## Acceptance Criteria
- Positive decimal Celsius values are converted and rounded to one decimal place.
- Negative decimal Celsius values are converted and rounded to one decimal place.
- Zero Celsius remains 32.0 Fahrenheit.
- A focused xUnit theory test covers positive, negative, and zero values.

## Tests
`TemperatureConverterTests.Converts_temperatures_to_one_decimal` covers 21.7 C, 0 C, -40 C, and -10.5 C. The test suite could not be run locally because command execution was blocked by the task sandbox.

## Commands
No build or test command was completed locally; the pipeline is responsible for solution build and test validation. The attempted targeted test command was blocked before execution.

## Risk and Review
The change is limited to Fahrenheit conversion. A reviewer should verify 21.7 C displays as 71.1 F and negative or zero values remain correct.

## Correction
No follow-up correction was needed.
