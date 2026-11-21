# Solution Report

## Summary
This ticket is a bug fix because the app could mislabel temperatures below zero.

## Root Cause
`ComfortRater.Rate` used `Math.Abs(celsius)`, which removed the negative sign, and then compared the magnitude with negative thresholds that could never match.

## Change
The method now compares the original Celsius value against the signed temperature boundaries in `src/WeatherDesk.Core/Logic/ComfortRater.cs`.

## Acceptance Criteria
- `-15` C is met as Bitterly cold and `-8` C is met as Freezing.
- The exact boundaries `-10`, `0`, `10`, `18`, `25`, and `32` C match the README and page legend.

## Tests
The `ComfortRaterTests` theory was expanded with negative temperatures and every band boundary.

## Commands
The focused test and required build/test commands could not be executed because command execution was denied without interactive approval.

## Risk and Review
The change is limited to comfort-band classification. A reviewer should check `Feels` output for -15 C, -8 C, -10 C, and 32 C.
