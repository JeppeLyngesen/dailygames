# DailyGames 🧩

A free tracker for daily web games: tick off Wordle, Connections, Strands and more, keep your streaks,
and see your stats. Built with C# / Blazor WebAssembly. Hosted for free on GitHub Pages.

## Run locally
```bash
dotnet run --project src/DailyGames.Web
```

## Test
```bash
dotnet test
```

## Structure
- `src/DailyGames.Core` – models and rules (streaks, tracker) – unit tested
- `src/DailyGames.Web` – Blazor WebAssembly UI; data stored in the browser
- `tests/` – xUnit tests
- `db/schema.sql` – SQL schema for phase 2 (accounts + sync)
- `docs/` – roadmap, weekly routine, KPIs, legal checklist
- `CLAUDE.md` + `.claude/agents/` – the AI "company": CEO, product, QA, content/SEO, growth, legal, finance

## Deploy
Every push to `main` deploys via GitHub Actions. One-time: Settings → Pages → Source: **GitHub Actions**.

DailyGames is an independent fan project and is not affiliated with any of the games it links to.
