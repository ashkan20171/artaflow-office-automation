# Ashkan Office Automation — Stage 3

Added:
- Organizational departments
- Role/Permission administration
- Server-side permission enforcement
- Advanced authorized letter search
- Secure attachment storage outside wwwroot
- Attachment type/size allow-list
- Notification center
- Referral notifications
- Improved letter details and inbox workflow
- Default permissions for Manager / Secretariat / Employee / Auditor
- Audit/security foundation retained

Security note: file extension/size checks are baseline controls. Production deployment should add malware scanning, content-signature validation, encryption-at-rest, storage quotas and retention rules.

Database note:
This development build still uses EnsureCreated. If you already created the Stage 1/2 database, delete the local development database once before first Stage 3 run so the new tables/columns are created. For production, replace EnsureCreated with EF Core migrations.
