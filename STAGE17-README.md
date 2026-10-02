# Stage 17 — Search Build Fix + Command Palette

- Fixed `KnowledgeArticle.Body` compile error: the actual entity property is `Content`.
- Global search now covers KnowledgeArticle Title, Content, Category and Keywords.
- Added Ctrl+K enterprise command palette and quick navigation.
- Added search fallback directly from the command palette.
- Improved global-search result metadata and responsive navigation UX.
- No database schema change in this stage.

Validation note: static source checks were run. The packaging environment does not contain the .NET SDK, so this artifact does not claim a successful `dotnet build`.
