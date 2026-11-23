# PR Description

## Summary
Upgrade the jquery-validation vendoring URL to 1.22.1 so rerunning the download script does not restore vulnerable version 1.19.5.

## Changes
- The console-provided checked-in library is already 1.22.1 and was left untouched.
- Update only the jquery-validation URL in `scripts/Add-VendoredLibs.ps1`.
- Record the security upgrade in the changelog and document the review.

## Testing
- No JavaScript test was added because the changed code is only a download URL and the repository has no JavaScript test setup.
- Build, xUnit tests, and vulnerability scan are pending pipeline validation.
- A person should check empty-form validation, city lookup, and city comparison in a browser.

## Jira
Fixes CLAUDE-16.
