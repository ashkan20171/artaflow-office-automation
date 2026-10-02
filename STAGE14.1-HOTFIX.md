# Stage 14.1 Hotfix — DevelopmentSchemaUpgrader Build Repair

This hotfix intentionally adds no new feature stage.

The Stage 14 generated `DevelopmentSchemaUpgrader.cs` was replaced completely with straightforward compiler-safe C#:
- no generated `string[]` raw-string array,
- no fragile comma insertion,
- one small `ExecuteSqlRawAsync` command per idempotent schema operation,
- `BodyTemplate` uses `nvarchar(max)`,
- Stage 3-14 compatibility tables/columns remain covered.

The hundreds of compiler errors reported against DevelopmentSchemaUpgrader were cascade errors from malformed C# syntax in that generated file.

Static checks performed:
- raw-string delimiters balanced,
- C# braces balanced,
- old generated batch-array removed,
- Razor model-directive regression scan,
- SeedData brace check,
- Workflow audit fix retained.

A real `dotnet build` is not claimed because the packaging environment does not provide the .NET SDK.
