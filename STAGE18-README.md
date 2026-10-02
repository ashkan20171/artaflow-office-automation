# Stage 18 — Bilingual Culture + Persian/Gregorian Calendar
Persian is default. Persian mode uses RTL and Jalali date display; English uses LTR and Gregorian dates.
Culture persists with ASP.NET Core's culture cookie. Date presentation is centralized in ILocalizedDateService.
Common existing Razor date renderings were migrated. No database schema changes.
Full translation of every legacy hard-coded Persian label is a separate resource-localization step.
