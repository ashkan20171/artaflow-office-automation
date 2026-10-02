# Stage 11 — MVC Architecture Refactor

This stage explicitly standardizes Ashkan Office Automation around ASP.NET Core MVC.

Added:
- Centralized `LetterAccessService`
- Centralized `WorkflowService` with legal transition rules
- Strongly typed `DashboardViewModel`
- `DashboardService`
- `ExportService`
- Architecture documentation and in-app architecture page
- DI registrations for new application services
- Responsive architecture UI

All Stage 1-10 features are retained.

Important:
The current execution environment used to package this artifact does not provide the .NET SDK, so no successful `dotnet build` is claimed. Open the solution in Visual Studio 2022, restore NuGet packages, and build it. Any compiler/runtime error should be fixed before the next feature stage.
