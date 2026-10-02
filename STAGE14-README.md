# Stage 14 — Database Upgrader Fix + Notification Preferences

## Runtime fix
The large compatibility SQL command in `DevelopmentSchemaUpgrader` was replaced by a sequence of small idempotent SQL commands.
This avoids the runtime failure where a ~6000 character command/value was bound to a SQL parameter with a 4000-character limit.

The upgrader still preserves existing data and remains a development compatibility bridge.

## MVC additions
- User notification preferences
- SLA/referral/task/meeting/security notification switches
- Daily digest preference foundation
- Strong user-scoped controller
- Responsive preferences UI
- Idempotent NotificationPreferences schema upgrade

All previous MVC, SLA, workflow, audit, security, analytics and collaboration modules remain.
