# PR Description

## Summary
This PR fixes freezing city temperatures being shown with incorrect comfort labels.

## Changes
- Compare Celsius temperatures using their signed values in `ComfortRater`.
- Add regression coverage for negative temperatures and every temperature-band boundary.
- Record the bug fix in the changelog.

## Testing
- Focused `ComfortRaterTests` coverage includes negative values and boundary temperatures.
- Automated build and test execution is pending pipeline validation.

## Jira
Fixes CLAUDE-11.
