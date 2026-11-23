# Solution Thinking

## Restated Requirement
This is a dependency and security bug fix: remove every project use of vulnerable System.Text.Json 8.0.3 and preserve build and test behavior.

## Acceptance Criteria
The package state must contain 11.0.0, no 8.0.3 package declaration may remain, no unrelated package may change, parser behavior must remain compatible, and build/tests must pass.

## Evidence
`Directory.Packages.props` already declares System.Text.Json 11.0.0. Both project files reference the centrally managed package, and `OpenMeteoParser.cs` uses stable `JsonDocument` APIs.

## Alternatives Considered
Changing source or adding a direct project pin would duplicate central package management and would not alter package resolution because the package is already fixed.

## Chosen Approach
Leave the package declarations and parser source unchanged, verify repository-wide package references, retain existing parser boundary tests, and record the validation commands for the pipeline.

## Assumptions
The console-selected old-to-new package move is represented by the already-applied 11.0.0 state, and the package scan will confirm no vulnerable transitive copy is resolved.

## Correction
No follow-up correction was needed.
