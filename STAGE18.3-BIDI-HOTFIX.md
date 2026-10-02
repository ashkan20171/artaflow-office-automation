# Stage 18.3 — Definitive RTL/LTR Shell Fix

Root cause:
CSS Flexbox `row` follows the element's `direction`. In Stage 18.2 the shell inherited RTL,
so flex order that looked correct numerically produced the opposite physical placement.

Fix:
- `.app` shell axis is permanently `direction:ltr` only for physical layout.
- Persian child content remains RTL; English child content remains LTR.
- Persian: main order 1, sidebar order 2 => sidebar physically RIGHT.
- English: sidebar order 1, main order 2 => sidebar physically LEFT.
- Old Stage 18.1/18.2 competing shell overrides were removed.
- CSS/JS use ASP.NET Core `asp-append-version` cache busting.
- No database schema changes.
