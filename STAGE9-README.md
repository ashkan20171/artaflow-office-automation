# Ashkan Office Automation — Stage 9

New capabilities:
- Letter comments / collaboration notes
- Per-user letter bookmarks
- Letter tags
- Personal Productivity Center for bookmarks, near deadlines and open tasks
- System Health / Diagnostics dashboard
- CSV export endpoint for up to 5000 letters
- User activation/deactivation action
- New diagnostics permission
- Schema upgrader support for comments/bookmarks
- Existing Stage 8 analytics and Ctrl+K command palette retained

Quality notes:
- Razor directive scan and known RolesController permission-name collision checks were run before packaging.
- The packaging environment does not currently expose the .NET SDK, so compilation was not claimed. Build the solution in Visual Studio 2022 and send any compiler/runtime error verbatim for a fixed package.
- Existing EnsureCreated-era databases continue to use the compatibility upgrader. A reviewed EF Core migration baseline remains recommended before production deployment.
