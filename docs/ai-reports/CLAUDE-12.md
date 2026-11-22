# Solution Report

## Summary
This ticket is a bug fix because provider failures and unknown cities are currently collapsed into the same not-found result.

## Root Cause
`WeatherLookupService.LookupAsync` catches every provider exception and then returns `LookupResult.NotFound`, while `/api/weather` has no 502 mapping for `LookupStatus.UpstreamError`.

## Change
The service now treats provider exceptions as an upstream failure except when the caller cancels the request, and `/api/weather` returns a friendly 502 for that status. A provider null result remains the city-not-found 404 path.

## Acceptance Criteria
- `-` is met: provider network, timeout, and server exceptions produce `LookupStatus.UpstreamError` and the endpoint responds with 502 and the friendly message.
- `-` is met: a provider null result produces 404 for a city that does not exist.
- `-` is met: focused tests invoke `WeatherEndpoint.LookupAsync` for both a failing provider and an empty provider answer.
- `-` is met: existing Victoria behavior is unchanged in the successful provider test.
- `-` is met: existing tests remain in place and the solution test suite must pass under pipeline validation.

## Tests
The new endpoint tests cover 502 for a failing provider and 404 for a null provider answer. The existing service test covers a successful Victoria lookup.

## Commands
No build or test command was run locally; this environment's pipeline is responsible for solution build and test validation.

## Risk and Review
The change is limited to upstream failure classification and HTTP status mapping. A reviewer should verify a real outage returns 502 and a real made-up city returns 404.

## Correction
This follow-up replaces the separate endpoint and unnecessary client/project changes from the earlier implementation. The 502/404 behavior remains in the lookup service and `/api/weather` handler, reducing the code change to the files required for the fix and endpoint tests.

The pipeline build identified misspelled ASP.NET status constants. This correction uses `StatusCodes.Status502BadGateway` in both the handler and endpoint test.
