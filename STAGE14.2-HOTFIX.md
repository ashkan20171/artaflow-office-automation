# Stage 14.2 Hotfix — OrganizationSettings Seed / Schema Reconciliation

The application now reaches runtime successfully, but failed while inserting the first OrganizationSetting.

Fixes:
- DevelopmentSchemaUpgrader now reconciles every OrganizationSettings column individually.
- Safe defaults are applied when legacy databases are missing required non-null columns.
- SeedData explicitly seeds all current OrganizationSetting fields.
- Seed failure now throws a focused diagnostic exception while preserving the original SQL exception as InnerException.
- Added an MVC Database Health service/page for connection and critical OrganizationSettings schema checks.

No Stage 15 feature expansion was started; this remains a stabilization hotfix.
