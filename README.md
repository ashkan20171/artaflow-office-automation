# ArtaFlow — Intelligent Office Automation Platform

> A bilingual, role-aware office automation and correspondence platform built with ASP.NET Core MVC, SQL Server and ASP.NET Core Identity.

**ArtaFlow** is a portfolio-grade enterprise workspace for managing organizational correspondence, referrals, tasks, meetings, notifications, knowledge, secretariat operations and workflow activity from one responsive interface. It combines a practical MVC architecture with Persian/English localization, true RTL/LTR layouts, Jalali/Gregorian presentation, granular authorization and productivity-focused UX.

## Why ArtaFlow?

Many office-automation demos stop at CRUD screens. ArtaFlow is designed around the daily flow of an organization: register correspondence, route it to the right people, track work, surface overdue actions, preserve auditability and give each user a focused workspace.

## Highlights

- **Correspondence management** — incoming, outgoing and internal letters, attachments, tags, archive data and secretariat registration.
- **Referral & workflow** — user referrals, completion state, workflow actions and a visual activity rail.
- **Smart Operations Dashboard** — access-scoped KPIs, today's focus, overdue work, unread alerts and recent correspondence.
- **Command Center** — a consolidated operational view of inbox, tasks, meetings and attention items.
- **Global Search & Command Palette** — keyboard-first navigation with `Ctrl/Cmd + K`.
- **Personal Workspace** — browser-persisted favorites, recently visited pages, focus mode and compact density.
- **Tasks & Meetings** — personal work tracking and upcoming meeting visibility.
- **Notification Center** — unread filtering, mark-as-read and mark-all-read flows.
- **Knowledge Base & Templates** — reusable organizational knowledge and letter templates.
- **Security** — ASP.NET Core Identity, roles, permission claims, account-security foundation, security headers and audit events.
- **Bilingual UX** — Persian and English with a deterministic RTL/LTR shell.
- **Localized dates** — Jalali presentation in Persian and Gregorian presentation in English while keeping normal `DateTime` values in persistence.
- **Seasonal login experience** — spring, summer, autumn and winter imagery selected automatically by month.
- **Responsive & installable** — mobile bottom navigation, PWA manifest/service-worker foundation and responsive layouts.
- **Accessibility** — keyboard navigation, focus-visible states and reduced-motion support.
- **Legacy integration boundary** — a separate `.NET Framework 4.8` contracts project for legacy interoperability without pretending ASP.NET Core itself targets .NET Framework.

## Tech Stack

| Area | Technology |
|---|---|
| Web | ASP.NET Core MVC / .NET 8 |
| Authentication | ASP.NET Core Identity |
| Data | Entity Framework Core + SQL Server |
| Realtime foundation | SignalR |
| UI | Razor Views, CSS, vanilla JavaScript |
| Legacy boundary | .NET Framework 4.8 contracts project |
| Architecture | MVC + application services + authorization policies |

## Architecture

The web project keeps controllers thin where practical and moves reusable business concerns into services such as dashboard, workflow, letter access, SLA, search, audit, database health and AI-assistant abstractions. Permission policies complement roles, while access-sensitive queries scope information to the current user unless administrative access applies.

The application stores dates normally in the database. Jalali/Gregorian behavior is a **presentation concern**, which avoids corrupting sorting, querying and EF Core persistence semantics.

## UX Details

ArtaFlow is deliberately designed as a working product rather than a collection of disconnected pages. The desktop experience uses a persistent navigation shell and personal workspace; mobile users receive a dedicated bottom navigation bar. Persian mode places the sidebar physically on the right and applies RTL inside content. English mode places it on the left and uses LTR.

The login background changes automatically by season and all four images are bundled locally, so the experience does not depend on a CDN.

## Getting Started

### Prerequisites
- Visual Studio 2022
- .NET 8 SDK
- SQL Server / SQL Server Express / LocalDB

### Run
1. Open `AshkanOfficeAutomation.sln`.
2. Review the `DefaultConnection` connection string in `src/AshkanOfficeAutomation.Web/appsettings.json`.
3. Restore NuGet packages.
4. Set `AshkanOfficeAutomation.Web` as the startup project.
5. Run with HTTPS from Visual Studio.

The development seed includes an administrator account for local evaluation. **Change seeded credentials before any shared or production-like deployment.**

## Security Notes

This repository is a portfolio/reference implementation, not a claim of production certification. Before real deployment, review secret management, Data Protection key persistence, upload malware scanning/storage, backup policy, HTTPS/reverse-proxy configuration, rate limiting, logging/monitoring, privacy requirements and your organization's authorization model.

See [`SECURITY.md`](SECURITY.md).

## Keyboard Productivity

- `Ctrl/Cmd + K` — open the global command palette
- `Ctrl/Cmd + Shift + B` — open Personal Workspace
- `Ctrl/Cmd + Shift + F` — favorite/unfavorite the current page
- `Esc` — close transient workspace/palette UI

## Portfolio Focus

This project demonstrates more than framework syntax: authorization design, data-access scoping, bilingual UX, RTL/LTR engineering, workflow-oriented product thinking, progressive enhancement, responsive design, legacy integration boundaries and maintainable MVC structure.

## Roadmap

Planned production-oriented extensions include a proper EF Core migration baseline, pluggable object storage and malware scanning, complete two-factor sign-in enforcement, provider-based AI integration, richer workflow modeling, persistent notification preferences, browser SignalR notifications, integration tests and deployment automation.

## Screenshots

Add repository screenshots or a short product GIF here after running the application with representative demo data. Recommended captures: seasonal login, Smart Operations dashboard, correspondence details/workflow, Command Center and mobile navigation.

## License

Choose and add the license that matches how you want others to use this repository before publishing it publicly.

---

Built as an enterprise software engineering portfolio project with a focus on clean architecture, practical workflows and polished user experience.
"# artaflow-office-automation" 
