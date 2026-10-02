# Stage 18.4 — Localization Cleanup

- Preserves the Stage 18.3 definitive RTL/LTR shell.
- Makes Command Center, AI Copilot and SLA sidebar labels culture-aware.
- Expands English UI translation coverage for legacy Razor views.
- Removes a duplicated `asp-append-version` attribute.
- Runtime translation is enabled only for English and targets static UI text/attributes; persisted user/database values are not intentionally translated.
- No database schema changes.
