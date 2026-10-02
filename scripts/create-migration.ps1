$ErrorActionPreference = "Stop"
dotnet tool restore
dotnet ef migrations add InitialEnterpriseSchema --project src/AshkanOfficeAutomation.Web --startup-project src/AshkanOfficeAutomation.Web --output-dir Data/Migrations
Write-Host "Migration created. Review it, then run scripts/update-database.ps1"
