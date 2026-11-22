# Implementation Thinking

## Approach
The implementation preserves the existing provider null contract, adds a distinct upstream status in the lookup service, and maps that status in the weather endpoint.

## Root Cause and Fix
The catch block in `WeatherLookupService.LookupAsync` discarded provider exceptions, and the endpoint fallback was not a 502 mapping. The fix preserves caller cancellation, returns an upstream status for provider exceptions, and returns 404 only for null readings.

## Changes by File
| File | Change |
|---|---|
| `src/WeatherDesk.Core/Services/WeatherLookupService.cs` | Map provider exceptions to `LookupStatus.UpstreamError` while preserving cancellation. |
| `src/WeatherDesk.Web/Program.cs` | Map `/api/weather` upstream failures to 502 and unknown-city results to 404. |
| `src/WeatherDesk.Web/WeatherEndpoint.cs` | Removed the unnecessary separate endpoint file. |
| `tests/WeatherDesk.Tests/WeatherDesk.Tests.csproj` | Reference the web project so endpoint-level tests can invoke the handler. |
| `tests/WeatherDesk.Tests/ProviderAndServiceTests.cs` | Add failing-provider and empty-answer endpoint tests plus upstream service coverage. |
| `CHANGELOG.md` | Add the user-visible outage behavior under Unreleased. |
| `docs/ai-reports/CLAUDE-12.md` | Record the solution report. |
| `docs/ai-reports/CLAUDE-12-pr.md` | Record the PR description. |
| `docs/ai-reports/CLAUDE-12-jira.md` | Record the Jira comment with the PR placeholder. |
| `docs/ai-reports/CLAUDE-12-solution-thinking.md` | Record the solution reasoning. |
| `docs/ai-reports/CLAUDE-12-implementation-thinking.md` | Record the implementation reasoning and changed-file table. |

## Test Strategy
The regression tests invoke `WeatherEndpoint.LookupAsync` and inspect the resulting HTTP status for a thrown provider exception and a null provider answer.

## Validation
The pipeline must run the solution build and test suite. Local build and test execution were not available in this task environment.

## Risks
The change affects only lookup outage handling. Caller cancellation continues to propagate rather than becoming a 502 response.
