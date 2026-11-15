# Roadmap

Realistic for 5 hours/week and ~0 kr. Dates are targets, not promises.

## Phase 1 – Launch (weeks 1–4)  ← we are here
- [x] Tracker MVP: follow games, tick off daily, per-game + perfect-day streaks, stats, custom games, export/import
- [x] CI + automatic deploy to GitHub Pages
- [ ] Enable GitHub Pages (Settings → Pages → Source: GitHub Actions) and check the live site on a phone
- [ ] Owner adds Ko-fi link and contact e-mail (TODOs in About.razor)
- [ ] Verify every link in games.json (content-seo)
- [ ] Google Search Console set up + sitemap
- [ ] First launch post (growth) – one community, e.g. r/SideProject

## Phase 2 – Make people come back (weeks 5–12)
- [ ] PWA: installable on phone home screen, works offline
- [ ] Paste a share result (e.g. Wordle "3/6") → auto-tick + store score
- [ ] Optional daily reminder (browser notification)
- [ ] Shareable streak card
- [ ] 3–5 high-quality guide pages (e.g. "games like Wordle") – prerendered for SEO
- [ ] Cookieless analytics (Cloudflare Web Analytics)
- Goal: 1,000 visits/week, 30% returning

## Phase 3 – Money + accounts (from ~month 4, only if traffic is there)
- [ ] Custom domain (~80–120 kr/year – the first real cost, CEO decides)
- [ ] Ads via AdSense with Google's free certified consent banner (CMP) – see legal-checklist
- [ ] ASP.NET Core API + Azure SQL free tier (db/schema.sql) for accounts and sync between devices
- [ ] Friends/leaderboards
- [ ] CVR registration when income becomes regular

## Kill/pivot criteria
If after 12 weeks there are < 100 weekly visitors despite 4+ launch posts, the CEO agent proposes a pivot
(e.g. Danish-language daily games, or a niche such as geography games only).
