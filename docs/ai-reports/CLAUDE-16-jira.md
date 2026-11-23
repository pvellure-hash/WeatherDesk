# Jira Comment

Updated the vendoring script to download jquery-validation 1.22.1, preventing it from restoring vulnerable version 1.19.5. The console had already replaced the checked-in library file. Application call sites were reviewed and did not need changes.

Build, tests, and the vulnerable-package scan remain for pipeline validation. A browser check should verify empty-form validation and a city lookup.

PR: <PR link>
