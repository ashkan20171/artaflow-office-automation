# Ashkan Office Automation — Stage 5

Added:
- Task management with assignee, priority, due date and status
- Organizational meetings with attendees, location and agenda
- Notifications for assigned tasks and meeting invitations
- Persian date formatting service for the new operational screens
- SQL row-version concurrency token on letters
- Additional security response headers
- dotnet-ef local tool manifest
- Migration creation/update PowerShell scripts
- New task/meeting permissions
- Stage 4 CS0119 fix retained

## Database migration transition
The running prototype still defaults to EnsureCreated so a fresh clone can start without a hand-written migration.
For the migration-based path:
1. Delete the old development database once.
2. From the solution root run `dotnet tool restore`.
3. Run `scripts/create-migration.ps1`.
4. Review the generated migration.
5. Replace `EnsureCreatedAsync()` in SeedData with `MigrateAsync()`.
6. Run `scripts/update-database.ps1`.

This avoids shipping a fake/empty migration that would mark the database migrated without actually creating the Identity and application tables.

## Next hardening
2FA, recovery codes, session/device management, workflow transition policies, delegated-access enforcement, SignalR notifications, malware scanning, content-signature validation, automated tests and full FA/EN localization.
