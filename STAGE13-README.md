# Stage 13 — Audit Compile Fix + SLA Engine

## Critical compile fix
`WorkflowService` called `IAuditService.WriteAsync` with 3 arguments while the current interface requires:
`userId, action, entity, id, details`.
The workflow call now passes all five arguments.

## New
- MVC SLA dashboard
- `ISlaService` / `SlaService`
- Strongly typed SLA ViewModels
- Default deadline assignment
- Overdue / due-soon / healthy metrics
- Background SLA monitoring every 15 minutes
- In-app notifications for overdue or near-due correspondence
- Permission-protected SLA management

All previous MVC modules are retained.
