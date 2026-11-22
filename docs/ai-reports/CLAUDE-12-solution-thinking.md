# Solution Thinking

## Restated Requirement
This is a bug fix because an upstream outage is incorrectly reported as a missing city.

## Acceptance Criteria
The implementation must distinguish provider failures from null city results, return 502 for failures, and return 404 for unknown cities.

## Evidence
`WeatherLookupService.LookupAsync` catches provider exceptions and falls through to the null-reading 404 branch, and `/api/weather` does not map `LookupStatus.UpstreamError` to 502.

## Alternatives Considered
Handling each outage only in `OpenMeteoClient` would leave the service and endpoint status contract inconsistent. Returning a generic null from every provider failure would retain the wrong 404 behavior.

## Chosen Approach
The provider contract remains unchanged: null means unknown city, exceptions mean upstream failure. The service maps exceptions to `UpstreamError`, and the endpoint maps that status to 502.

## Assumptions
Network, timeout, and HTTP server failures are surfaced by the provider as exceptions; a normal geocoding answer with no results is returned as null.

## Correction
The follow-up removes the unnecessary extracted endpoint file and client changes. The handler remains in `Program.cs`, with only the lookup service and endpoint-level tests changed for the defect.

The build correction replaces the unavailable `StatusCodes.StatusNotFound` test constant with `StatusCodes.Status404NotFound`.

The follow-up also says to ignore the failing 404 endpoint test, so this correction leaves that test unchanged.
