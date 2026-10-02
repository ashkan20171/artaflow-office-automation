# Ashkan Office Automation — MVC Architecture

Stage 11 formalizes the application as ASP.NET Core MVC.

## Presentation / MVC
- Controllers: HTTP orchestration only
- Views: Razor `.cshtml`
- ViewModels: strongly typed page/input models

## Application Services
- `ILetterAccessService`: centralized correspondence authorization
- `IWorkflowService`: workflow transition rules and auditing
- `IDashboardService`: dashboard query composition
- `IExportService`: export generation
- Existing registry, Persian date, audit and realtime services remain application services.

## Data
- `AppDbContext` / EF Core / SQL Server
- Identity for users and roles
- `DevelopmentSchemaUpgrader` remains a compatibility bridge for prototype databases.

## Rules
Controllers should not contain reusable business rules or direct formatting/export logic.
Authorization is enforced both at controller/policy level and in application services for resource-level access.
Production deployment should replace prototype `EnsureCreated` compatibility with reviewed EF Core migrations.
