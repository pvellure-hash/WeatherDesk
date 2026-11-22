# PR Description

## Summary
This PR distinguishes weather-service outages from cities that do not exist.

## Changes
- Return `LookupStatus.UpstreamError` when the weather provider throws, except for caller cancellation.
- Map upstream failures to HTTP 502 with a friendly error and keep unknown cities at HTTP 404.
- Add endpoint-level regression coverage for both outcomes.

## Testing
- A failing provider test invokes the lookup endpoint and verifies HTTP 502.
- A null provider answer test invokes the lookup endpoint and verifies HTTP 404.
- Automated build and test execution is pending pipeline validation.

## Jira
Fixes CLAUDE-12.
