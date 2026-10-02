# Stage 14.3 — Startup Hotfix

- Removed EF-tracked OrganizationSettings startup insertion.
- Organization settings are now seeded by a direct idempotent SQL statement after schema reconciliation.
- Legacy OrganizationSettings rows/column types are normalized before startup continues.
- Existing database data is preserved; no database deletion is required.
- No new feature stage was added while startup stabilization is in progress.
