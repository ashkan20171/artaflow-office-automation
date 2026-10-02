# Stage 18.5 — Seasonal Login Experience

- Uses the four user-provided Spring, Summer, Autumn and Winter images locally (no CDN).
- Automatically selects the seasonal background from the server's current month.
- March–May: Spring; June–August: Summer; September–November: Autumn; December–February: Winter.
- Keeps the Stage 18.3 RTL/LTR shell and Stage 18.4 localization work intact.
- Adds a bilingual glassmorphism login experience with season-specific accent colors.
- Responsive behavior for desktop/tablet/mobile and reduced-motion accessibility support.
- Images live under `wwwroot/img/seasons`.
- No database/schema changes.
