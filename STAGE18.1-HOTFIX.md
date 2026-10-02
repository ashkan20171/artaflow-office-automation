# Stage 18.1 — RTL/LTR + Calendar Hotfix

Root causes fixed:
1. `.app` was permanently `grid-template-columns: 270px 1fr`, so changing HTML `dir` did not physically move the sidebar.
2. Several legacy views bypassed the culture-aware date service by using `IPersianDateService`, `ToString("yyyy/MM/dd")`, or raw `ToLocalTime()`.
3. Direction-sensitive CSS used physical right/left values.

Behavior:
- fa: sidebar right, RTL content/forms/tables, Jalali date presentation.
- en: sidebar left, LTR content/forms/tables, Gregorian date presentation.
- culture cookie is the first request-culture provider and persists after navigation/refresh.
- no database schema change.

The hotfix intentionally keeps database DateTime values Gregorian/normal internally. Only presentation changes by culture, which avoids corrupting sorting, queries, EF mappings and persisted data.
