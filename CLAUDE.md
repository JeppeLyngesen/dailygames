# DailyGames – company handbook for Claude

## Mission
A free, fast, friendly tracker for daily web games (Wordle, Connections, Strands, …). Users tick off what
they've played, keep streaks and see stats. Revenue: donations first, privacy-respecting ads once traffic exists.

## Hard constraints (never break these)
- **Budget ≈ 0 kr.** Free tiers only (GitHub Pages, GitHub Actions, Buy Me a Coffee, Google Search Console). Propose
  anything paid to the owner with cost + expected return, never add it yourself.
- **Owner time: 5 hours/week.** Anything needing the owner must be batched into the weekly review (docs/weekly-routine.md).
- **The owner approves** anything that publishes content, spends money, contacts people, or has legal effect.
  Agents draft; the owner ships.
- **No mass-produced AI content.** Google penalises "scaled content abuse". Every public page must be useful
  and owner-reviewed. Quality > quantity.
- **No spoilers / answer pages** for games (legal and ethical risk, and it annoys the game makers).
- **Respect trademarks:** use game names only to describe/link to them. No logos, no copied artwork,
  no "official" wording. Always show the "not affiliated" notice.
- **Privacy by default:** no tracking cookies or third-party scripts without consent (GDPR / Danish cookie rules).
- Never commit secrets. Keys go in GitHub Secrets.

## Tech
- .NET 8, C#. `src/DailyGames.Core` = models + rules (StreakCalculator, Tracker) – **all logic goes here, with
  xUnit tests in `tests/`**. `src/DailyGames.Web` = Blazor WebAssembly UI, thin pages.
- Phase 1: data in the browser's localStorage (`TrackerService`). Phase 2: ASP.NET Core API + SQL
  (`db/schema.sql`) for accounts and sync – only when users ask for it.
- Game catalog: `src/DailyGames.Web/wwwroot/data/games.json`.
- Hosting: GitHub Pages via `.github/workflows/deploy.yml` (every push to main deploys).
- Before committing: `dotnet build -warnaserror && dotnet test`.
- The owner is a 3rd-semester datamatiker student: explain non-obvious code briefly, prefer clear C# over clever C#.

## Departments (subagents in .claude/agents/)
| Agent | Use it for |
|---|---|
| `ceo` | Weekly review, priorities, the one-page plan for next week |
| `product-manager` | Turning ideas/feedback into small GitHub issues with acceptance criteria |
| `qa-reviewer` | Reviewing changes: bugs, tests, accessibility, performance |
| `content-seo` | Game guide pages, meta tags, catalog updates, Search Console insights |
| `growth` | Launch posts, community outreach drafts, share features, newsletter |
| `legal-compliance` | Privacy, cookies/consent, trademark checks, ad-network policies, Danish tax notes |
| `finance-analytics` | KPI tracking in docs/kpis.md, revenue vs. costs, monetisation decisions |

## Key docs
- docs/roadmap.md – phases and milestones
- docs/weekly-routine.md – the owner's 5-hour week
- docs/kpis.md – numbers, updated weekly
- docs/legal-checklist.md – what must be true before ads/accounts
