# Stage 18.2 Localization Refactor
- Definitive Flex-based shell direction: FA sidebar right, EN sidebar left.
- Culture cookie remains authoritative.
- Global English translation runtime covers legacy hard-coded Razor text and attributes without changing stored user/business data.
- Login is culture-aware even with Layout=null.
- Jalali/Gregorian presentation continues through ILocalizedDateService.
- No database schema changes.
