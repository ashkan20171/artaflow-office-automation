# Ashkan Office Automation — Stage 7

## Critical database fix
The "Invalid column ArchiveCode / ArchivedAt / DueAt / RowVersion" exception was caused by old databases created with EnsureCreated.
EnsureCreated does not update an existing schema.

Stage 7 adds `DevelopmentSchemaUpgrader`, an idempotent SQL Server compatibility upgrader. At startup, after EnsureCreated, it:
- adds the missing Letter columns,
- creates Stage 3-6 tables if absent,
- preserves existing data.

A small emergency SQL file is also included at `scripts/repair-stage7-schema.sql`.

## Stage 7 features
- Real session/device telemetry middleware
- Revoked sessions are blocked on subsequent requests
- TOTP authenticator verification
- 2FA enable/disable
- Recovery-code generation
- Delegated-access support in advanced search
- Stage 6 SignalR server foundation retained
- Schema compatibility upgrade for old prototype databases

## Recommended run
Back up your development database, then run Stage 7 normally. The compatibility upgrader should patch the existing SQL Server schema automatically.

## Production direction
After stabilizing the prototype schema, generate a real EF Core baseline migration and stop using EnsureCreated/compatibility SQL for new production installations.
