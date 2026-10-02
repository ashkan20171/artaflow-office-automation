# Ashkan Office Automation — Stage 6
Added:
- SignalR server-side notification hub and realtime notifier service
- Personal Command Center with inbox/tasks/overdue/notifications/meetings KPIs
- Secretariat registration records and UI
- Account security center
- User session/device data model and revoke foundation
- Authenticator/2FA setup foundation
- Additional enterprise UI refinements
- Existing workflow, delegation, tasks, meetings, attachments, RBAC and audit retained

Important:
- The SignalR server foundation is active; a bundled browser SignalR client should be added before showing toast notifications live in the UI.
- 2FA setup is intentionally not presented as complete until TOTP verification and recovery codes are implemented.
- Session telemetry model is present, but login middleware still needs to populate/update session records before device management is fully operational.
- Database schema changed again. For this prototype branch use a fresh development DB, or generate/review EF Core migrations using the scripts introduced in Stage 5.

Stage 7 recommended:
Complete TOTP verification + recovery codes, session tracking middleware, bundled SignalR JS client, realtime toast/inbox counters, workflow transition rules, delegated access enforcement, reports/charts, full Persian calendar inputs, tests and production migrations.
