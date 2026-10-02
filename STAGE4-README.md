# Ashkan Office Automation — Stage 4

## Fixed before Stage 4
CS0119 in RolesController was caused by the `Permissions` action shadowing the `Permissions` static class.
The controller now uses `AppPermissions` alias.

## Stage 4 additions
- Transactional registry numbering by letter type/year
- Incoming / outgoing / internal serials
- Workflow actions and timeline
- Submit / approve / reject / archive / reopen backend support
- Archive code and archive timestamp
- Delegation / temporary substitute records
- Expanded dashboard KPIs
- New permissions for workflow, delegation and registry foundations
- Existing role/permission, attachment, notification, audit and search features retained

## Important database note
Stage 4 introduces new tables/columns. This development branch still uses EnsureCreated.
Delete the old LocalDB database once before first run of Stage 4, or move the project to EF Core Migrations.
The next stage should switch fully to migrations.

## Production hardening still recommended
Workflow transition rules, delegated-access enforcement, malware scanning, file signature validation, rate limiting, 2FA, recovery codes, encryption/key management, database migrations, concurrency tokens, automated tests, SignalR, Persian calendar and localization.
