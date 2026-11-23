# Solution Thinking

## Restated Requirement
This is a dependency and security upgrade: replace System.Text.Json 8.0.3 with 11.0.0 and prove that no other package or behavior changes unexpectedly.

## Acceptance Criteria
The central declaration must use 11.0.0, the vulnerable 8.0.3 package must disappear from project resolution, no unrelated package may change, and build/tests must pass.

## Evidence
`Directory.Packages.props` declared 8.0.3 and both projects referenced it centrally. `OpenMeteoParser.cs` used only stable `JsonDocument` parsing APIs.

## Alternatives Considered
A source-only change could not remove the vulnerable package; adding a direct project pin could duplicate central package management and was unnecessary.

## Chosen Approach
Change the central package version only and add parser boundary coverage without changing source call sites.

## Assumptions
The NuGet 11.0.0 release is available and compatible with the target framework, and the final scan will confirm transitive resolution.

## Correction
No follow-up correction was needed.
