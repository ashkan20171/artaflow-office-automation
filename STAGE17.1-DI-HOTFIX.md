# Stage 17.1 — Dependency Injection Hotfix

Runtime failure fixed: `AiCopilotController` required `IAiCopilotService`, but Stage 17 `Program.cs` did not register it.

A complete service-registration audit found multiple application services missing from DI. `Program.cs` now explicitly registers all service interfaces currently declared in the Services layer, including AI Copilot, Global Search, SLA, Database Health, Activity Feed, Letter Access, Workflow, Dashboard and Export services. `SlaMonitoringService` is restored as a hosted service.

No database schema changes are included in this hotfix.
