# Solution Thinking

## Restated Requirement
This is a bug fix for negative Celsius values being assigned comfort labels that do not match the temperature legend.

## Acceptance Criteria
The implementation must label negative values correctly and handle every temperature boundary exactly as described in the table.

## Evidence
`ComfortRater.Rate` used an absolute value and impossible negative comparisons, while the README and page legend define the bands using signed Celsius values.

## Alternatives Considered
Changing each caller or formatter would not fix the shared classification logic and could leave other weather reports incorrect.

## Chosen Approach
The smallest fix is to compare the original Celsius value directly against the signed thresholds in `ComfortRater.Rate`.

## Assumptions
The table in the README and page is authoritative, so `-10 C` is Freezing and `32 C` is Hot.
