# CompareHare — Rust Backend Rewrite Plan

Status: **planning** (D1 and D2 decided) · Assessed against `develop` on 2026-10-17; audited and corrected 2026-10-19 · Scope: backend only (the frontend is a separate, later project)

This document does three things: (1) assesses the existing .NET backend, (2) proposes the shape of the Rust replacement, and (3) breaks the work into broad **workstreams**, each written so it can be split into ordered GitHub issues and later handed to an agent to implement and to you to verify. Four decisions (auth, database, workers, scraping crates) are deliberately **carved out** in §3 so they can be settled separately without blocking everything else. Two are now decided (D1 auth, D2 database); D3 and D4 remain open.

## TL;DR

- The .NET code is small in *logic* (a few hundred lines of real behavior) wrapped in a lot of ceremony (MediatR, AutoMapper, Autofac, FluentValidation). The **design** is worth keeping; the **code** is not worth a line-by-line port.
- The design worth keeping: a hash short-circuit pipeline (`load → hash → persist → assess → notify`), a shared `StateUtilityIndex` so scrapes scale with distinct (state, utility, zip/distributor) rather than users, and one strategy seam per state/utility and per retailer.
- A meaningful share of the work is **finishing features that were never built**, not porting: the notification pipeline, registration/verification/reset, product price-history graphs, and a working PA Power source. The PA Power loader is mid-rewrite and the repo does not build as committed (checked in a scratch copy, §1.4).
- The docs, the code and the TODOs disagree in several places (§1.5), and the code has shortcuts and bugs that shouldn't be copied (§1.6) — including the login password check, which was deliberately bypassed for demos.
- Recommended approach: work **strictly in sequence**, with a human verification gate after each step that sets a contract (domain types/traits, schema, HTTP contract). Build a **vertical slice** (PA Power electric: alert → refresh → assess → email to a local mail sink) before product scraping.
- **Two stages.** Utility prices are built, secured and shipped first. Product tracking is a separate later stage, because retailer anti-bot defences make it the riskier half; it starts with its own go/no-go spike.
- **Greenfield.** No existing data or deployment needs to be preserved, so there is no importer, no data migration and no cutover — the schema is designed fresh and the 25 EF migrations are history only.

---

## 1. Assessment of the existing backend

### 1.1 Shape

| Project | Role |
|---|---|
| [CompareHare.Domain](src/CompareHare.Domain) | Entities, offer loaders, price scrapers, alert assessor, hashing, HTML parsing/requester helpers, Dapper SQL helpers, SMTP wrapper |
| [CompareHare.Api](src/CompareHare.Api) | ASP.NET Core MVC + MediatR handlers per feature (Alerts, Authentication, CurrentUser, Offers, PaPower, Prices, Products), Hangfire jobs, Identity cookie auth, EF Core (Pomelo/MySQL, 25 migrations 2019–2024) |
| [CompareHare.Tests](src/CompareHare.Tests) | xUnit; domain-only (hasher, parser helper, product helper, PA loader, price scraper) with saved HTML fixtures. **No tests** for the assessor, persisters, handlers or API |
| [CompareHare.React](src/CompareHare.React) | Out of scope. It is React/Redux; the README's "Angular Full-Stack Generator" line is stale boilerplate |

About 9.3k lines of hand-written C# (counted 2026-10-19: 9,330 incl. 883 of tests, excl. migrations). Tech: .NET Core 2.2 (end-of-life), EF Core + Dapper, Autofac module scanning via a marker interface (`IFeatureService`), AutoMapper 8, FluentValidation 7, Hangfire 1.7 on MySQL, AngleSharp (+ `AngleSharp.Js`) for scraping, MySqlConnector.

### 1.2 What the system does

```text
UTILITY ALERTS                                         PRODUCT TRACKING
──────────────                                         ────────────────
StateUtilityIndex (state, type, id1..3, last_hash)     TrackedProduct ─< TrackedProductRetailer
   │  OfferLoader  (per state+type)                        (retailer enum, url, css selector)
   ▼  hash unchanged → stop                                   │  PriceScraper (selector + per-retailer config)
UtilityPrice (current) + UtilityPriceHistory                  ▼  price unchanged → stop
   │  AlertAssessor (per alert)                           ProductRetailerPrice (current)
   ▼  index-hash unchanged → stop                         + ProductRetailerPriceHistory (append-only)
AlertMatch ─< AlertMatchUtilityPriceHistory               + ProductPriceScrapingException (last failure)
   │  match-hash unchanged → stop                         (alerting for products = FUTURE)
   ▼
PendingAlertNotification ──► email        ← NOT IMPLEMENTED

SHARED: users/roles (ASP.NET Identity, cookie auth) · recurring jobs (daily 00:00 / 01:00 / 02:00 ET) · SMTP · throttling
```

### 1.3 What's worth carrying over

1. **Two-level hash short-circuit** (index-level `LastUpdatedHash`, match-level `AlertOfferHash`). Cheap and the heart of the system.
2. **`StateUtilityIndex` indirection**: one upstream fetch per distinct (state, type, zip, distributor, rate) serves every user's alerts.
3. **A strategy seam per state/utility and per retailer** (`OfferLoaderPicker`, `PriceScraperPicker`). Right seam; the implementation is `if` chains duplicated in four places (§1.6 #6).
4. **History rows keyed by id, linked from alert matches**, so "current" tables can be refreshed without breaking what the user sees.
5. **Real captured HTML as test fixtures** (PA Power, Lowe's, Home Depot, Best Buy, Appliances Connection) — reusable as-is in Rust tests.
6. **Feature-folder layout** — maps cleanly onto Rust modules.

### 1.4 Completeness

| Capability | State | Notes |
|---|---|---|
| Alert create / list / get / delete | Implemented | Edit endpoint is commented out |
| PA Power offer loader | **Partial / broken** | Mid-rewrite (last commits: "WIP: Getting PA Power working again"). The `URL` constant is commented out, so [PAPowerOfferLoader.cs](src/CompareHare.Domain/Features/OfferLoaders/PAPowerOfferLoader.cs) can't compile. Targets the old HTML site; its own TODO says move to the `umbraco/Api/ShopApi` JSON/CSV endpoints. Tests call the old signature |
| Gas, other states | Enum values only | `UtilityTypes.Gas` exists; no loader, no assessor query |
| Alert assessor | PA Power only | Doesn't emit notifications |
| Notification pipeline | **Not implemented** | `PendingAlertNotification` entity is never written or read. `EmailSender` is never called. No `.hbs` email templates exist anywhere in the repo |
| Registration, email verification, forgot/reset/change password | **Not implemented** | Controller actions are commented out; only a seeded user exists. Login requires a confirmed email |
| Login | Implemented; password check deliberately bypassed | Demo shortcut — see §1.6 #1 |
| Product tracking CRUD (create, list, current, enable/disable) | Implemented | |
| Product price scraping | Implemented | Generic CSS-selector scrape + hardcoded selectors/cookies for 4 retailers; JS execution via `AngleSharp.Js` |
| Manual price entry | Implemented | Recomputes neighbouring price diffs |
| Price-history data for graphs | **Stub** | `GetSummaryHistoryHandler` returns empty OK; the intended algorithm is only sketched in comments |
| Product alerting (`TrackedProductCriteria`) | Not started | Documented as FUTURE |
| Background jobs | Implemented | 3 recurring Hangfire jobs, 1 worker, fixed clock offsets; dashboard is **deny-all in production** |
| JWT / `ExternalApiController` | Dead code | Never registered; nothing inherits it |
| SPA hosting | Dev-only | Reads `wwwroot/index.html`; no `wwwroot` in the repo |

**Build check (2026-10-19, in a scratch copy; the repo was not touched).** The solution does not build as committed. With the 8.0 SDK: (1) `IOfferLoader` and `PAPowerOfferLoader` use nullable reference annotations (`IRequester?`), which fail under the default C# 7.3 of a `netcoreapp2.2` project (CS8370); (2) with a newer `LangVersion`, the next error is the undefined `URL` constant in `PAPowerOfferLoader.cs`; (3) with `URL` stubbed, 19 errors remain: 7 in the Api `PaPower` feature (missing `using`s for `Task`, `HttpClient`, `List`, `CancellationToken`) and 12 in `CompareHare.Tests` (stale signatures, e.g. `LoadOffers(int, string)` vs `LoadOffers(int, string[], IRequester?)`, constructors that now need `IRequesterHelper`). The 2.x SDKs and runtimes are installed here (alongside 3.x, 6.0 and 8.0) but are Intel builds that would not run from this session's shell, so the original toolchain was not tried. Nothing was run. Don't plan on using the .NET app as a live oracle.

### 1.5 Where the docs, the code and the TODOs disagree

- **PA Power integration has three generations.** [ARCHITECTURE.md](ARCHITECTURE.md) describes `POST by-zipcode` / `by-distributor` form calls (2019). The code scrapes `shop-for-rates-results` HTML. The code's own TODO points at `ZipSearch` (JSON) and `RateCSV`. Which still work is unverified — **WS4's first task is to find out**.
- **History retention.** ARCHITECTURE.md says `UtilityPriceHistories` are kept so alerts stay valid. [OfferPersister.cs](src/CompareHare.Api/Features/Offers/Services/OfferPersister.cs) deletes the index's histories on every refresh (with an unresolved "figuring this out" TODO), and the foreign keys cascade into the alert-match links.
- **Assessor outcomes.** ARCHITECTURE.md says that when no offers match, the alert is removed ("remove alert"). In code, "had matches, now zero" returns `NewMatchesAvailable` with a count of 0; `NoNewMatchesFound` only occurs when there was never a match.
- **Product tables.** PRODUCT_ARCH.md says "only 1 table for products". The code has two (`ProductRetailerPrice` current + `…History`) kept in sync by hand in `PricePersister` and `AddManualPriceHandler`.
- **Tooling.** Docs mention `object-hash` (SHA-1, Node era) and Bull; the .NET code uses SHA-1 over Newtonsoft JSON and Hangfire.

### 1.6 Known bugs, shortcuts and hazards — specify, don't port

1. **The login password check is bypassed (deliberate demo shortcut).** [LogInModelCustomValidator.cs](src/CompareHare.Api/Features/Authentication/RequestHandlers/LogIn/LogInModelCustomValidator.cs) discards the result of `PasswordSignInAsync` (the `if (result.Succeeded)` guard is commented out with a "looks like testing" TODO), then `LogInHandler` signs the user in unconditionally, so any password works for a confirmed account. This was done on purpose so the app's features could be shown off. It must not carry over: the Rust version enforces real credential checks from the start and needs a regression test for it.
2. **Contract-length filters are never applied.** `MinimumMonthLength` / `MaximumMonthLength` are stored and validated on alerts but absent from [GetAlertMatchesForPaPower.cs](src/CompareHare.Domain/Features/AlertAssessors/Queries/GetAlertMatchesForPaPower.cs).
3. **Same query:** price bounds use strict `>` / `<` though the intent is inclusive ("at or below $0.0550"); the max-renewable bound is guarded by the *minimum* property's `HasValue`.
4. **Money is `float`** (prices, diffs, hashes). Use decimals.
5. **The hash is order-dependent and null-lossy**: offers hash in scrape order, and nullable bools collapse to `false` in `UtilityPriceHashModel`. That means spurious "changed" results and missed real changes.
6. **Per-state `if` chains** in `AlertAssessor.GetMatches`, `UtilityIndexPersister` (×2) and `OfferLoaderPicker`. `FetchIndexId` also looks the index up by zip only and calls `SingleAsync()`, so two distributors in one zip will throw.
7. **No uniqueness constraint** on `StateUtilityIndices` (state, type, id1–3); uniqueness is by convention in code. Indices are never deactivated either.
8. **Persisters open fresh `DbContext`s** to dodge Hangfire scoping problems (see the "struggled with the async nature" comment). In Rust make each job step an explicit transaction.
9. **Mixed time semantics** (`PriceDate = DateTime.Now.Date` local vs UTC elsewhere) and inconsistent lockout settings (5 vs 100 attempts, and the lockout path is commented out).
10. **Scraper config is stale by design**: hardcoded session cookies (e.g. Lowe's `al_sess`, Akamai tokens), a 3-entry user-agent list, and the full page HTML stored in the exception `Error` column when a selector misses.
11. **Dev toggles baked into code**: the PA distributor lookup points at `localhost:8000/public/…json`, CORS is hardcoded to `localhost:8000`, and the seed file commits a password hash. Make all of this config/env-driven.
12. **Minor**: the scrape "% fail" log uses integer division (it prints 0 unless every scrape failed, then 100); `AlertAssessorRunner` has no per-alert error isolation (one throw aborts the run).
13. **Assessor ordering.** `AlertAssessor` saves the alert's index hash *before* it computes matches, so a crash in between leaves the alert looking up to date and it is never reassessed.
14. **Offer refresh is not atomic.** `OfferPersister` deletes and re-adds offers and histories in one `SaveChanges`, then updates the index hash in a second one, so a failure between them leaves new data under the old hash.

### 1.7 Existing data

**None to preserve.** No instance of the .NET app, and none of its data, matters. The Rust backend is built greenfield: no importer, no data migration, no dual-running and no cutover. The old schema is a reference for intent only; where this plan or your review improves on it (H1, H2), the new design wins.

---

## 2. Target shape (proposal)

### 2.1 Principles

1. **Pure core, impure edges.** Hashing, matching, price-diff and the assessor state machine are pure functions. Fast tests, no mocks, and easy to verify in isolation.
2. **One trait seam per axis of variation**: `OfferSource` keyed by (state, utility type), `ProductScraper` keyed by retailer, `Notifier`. A registry replaces `if` chains; adding a state or retailer touches one place.
3. **Idempotent jobs, explicit transactions.** Every job step is safe to re-run and commits atomically.
4. **Contract-first.** Domain types/traits, the schema and the HTTP contract are each settled and human-reviewed *before* any work that builds on them is queued.
5. **Rewrite from behaviour, not from lines.** The C# is a source of intent; §1.6 is the errata.

### 2.2 Proposed workspace

```text
comparehare/
  Cargo.toml                workspace; edition = "2024" (decided); rust-toolchain.toml pins stable (1.99.1 installed here)
  crates/
    ch-domain/    pure: enums, Offer, AlertCriteria, canonical hash, matcher, assessor state machine, price diff, parsers
    ch-db/        migrations + repositories — the only crate that knows SQL                          [D2]
    ch-sources/   OfferSource + ProductScraper impls, HTTP fetcher, parsers                       [D4a/D4b]
    ch-engine/    orchestration: refresh offers · assess alerts · scrape prices (traits only)
    ch-notify/    templates, SMTP, dispatcher
    ch-jobs/      scheduler / worker runtime                                                          [D3]
    ch-auth/      accounts, sessions/tokens, verify/reset flows                                       [D1]
    ch-api/       routes, DTOs, validation, error envelope, OpenAPI
  bins/           one binary with subcommands: serve | worker | migrate
  fixtures/       captured HTML/JSON/CSV (moved from CompareHare.Tests)
  docker/         compose: PostgreSQL 18 + Redis + mail sink
```

The crate split is a suggestion — collapse crates if compile-time overhead isn't worth it, but **keep the pure-core / IO boundary**. One binary with subcommands keeps deployment simple now while letting API and worker scale separately later.

### 2.3 Non-blocking defaults (swap freely)

| Concern | Default |
|---|---|
| Language edition | **Rust 2024 edition — decided.** Every crate sets `edition = "2024"`; agent briefs must say so (e.g. `unsafe extern`, RPIT lifetime capture and `gen` as a reserved keyword differ from 2021) |
| Runtime / HTTP server | tokio / axum (+ tower-http for CORS, tracing, static files) |
| Serialization | serde, serde_json |
| Money | rust_decimal |
| Time | chrono + chrono-tz (or jiff); schedules are America/New_York today |
| Hashing | SHA-256 over canonical, sorted, null-preserving JSON (no need to keep SHA-1) |
| Errors / logging | thiserror + anyhow / tracing + tracing-subscriber (JSON) |
| Config | typed, env-overridable (figment or config); no secrets in committed files |
| Email | lettre + a template engine (askama, minijinja or handlebars-rust) |
| Validation | validator or garde |
| API contract | utoipa (OpenAPI committed to the repo — this becomes the future frontend's contract) |
| Tests | testcontainers, wiremock, insta (snapshots), proptest |

Crate names listed here and in D1, D3 and D4 were checked against crates.io on 2026-10-19: all exist with a current release. Release age flags worth a look before committing: `figment` (last release 2024-05), `oauth2` (2025-01), `openidconnect` (2025-07), `chrono-tz` (2025-07), `wiremock` (2025-08), `tokio-cron-scheduler` (2025-10). `fang` has very low recent downloads (about 8k). Existence and release dates are all that was checked, not quality or fitness.

### 2.4 Smaller decisions I'm assuming unless you object

| # | Decision | Assumed default |
|---|---|---|
| H1 | Offer history policy | Append-only history, **never deleted**; dedupe so unchanged offers don't create new rows; alert matches link to history ids |
| H2 | Product current vs history tables | **Collapse** to history only; "current" = latest row per tracked retailer (PRODUCT_ARCH.md's original intent; removes the dual-write bugs) |
| H3 | HTTP contract | **Redesign with OpenAPI-first**, don't preserve quirks (e.g. `GET /products/create` returning form data, mixed int/string enums). Keep a short old→new route map for the eventual frontend port (Appendix B) |
| H4 | Matching location | Evaluate alert filters **in memory** over one index's offers (~100 rows) via the pure matcher; drops the dynamic-SQL builder and keeps one source of truth |
| H5 | Existing data | **Greenfield — decided.** Nothing is preserved or imported; fresh baseline schema |
| H6 | Account/alert flow for new indices | Replace the synchronous "populate" request (which loads upstream inside an HTTP call) with: create alert → enqueue refresh job → assess on completion → client polls status |
| H7 | Alert with zero matches | Match set cleared, **alert stays** (user's config); notify only on non-empty change. ARCHITECTURE.md says "remove the alert" — confirm |

---

## 3. Decisions (carved out)

Each section gives the requirements derived from the current system, options to evaluate, coupling to other decisions, an interim seam so other work can proceed, and a suggested timeboxed spike. Candidate crates are starting points to verify, not recommendations.

### D1 — User authentication · **decided (option B); spike confirms**

**Decision:** assemble authentication from vetted crates — we write no cryptography and no protocol code. Social login is **Google only** at launch, alongside email + password, both converging on the same internal user. The only client is the Angular web app. Sign-up is **invite-only** to start. Sessions live in **Redis** (an instance already runs on the local network).

**Needs (from the current system):** roles `Admin` / `User`; email confirmation before an email+password account can log in; forgot / reset / change password (stubbed, never built); every query scoped to the current user; `GET current-user/identity` returns user + roles. Today the SPA authenticates with a cookie (`withCredentials`, CORS allow-credentials from one origin).

**Options considered:** (A) hand-roll hashing, tokens and sessions — rejected. (B) Assemble from maintained crates; we write only the glue (routes, a few tables, token issue/verify). (C) Delegate to an identity service (Ory Kratos, Keycloak, Zitadel, or hosted). B was chosen because C's self-service flows are driven by the frontend (which doesn't exist yet, so Stage A couldn't be tested end to end), Authelia was not considered a fit for self-signup (not verified), hosted services add cost and lock-in, and B keeps the Rust API the only service to deploy. **Fallback:** if the spike shows B is more work or risk than expected, switch to Ory Kratos.

**Design:**

- **Session.** An opaque random session ID in an `HttpOnly`, `Secure`, `SameSite=Lax` cookie; session data in Redis (candidates: `tower-sessions` with its Redis store, or `axum-login` on top). **No JWT** — server-side sessions give instant revocation (logout, password change, disabling a user) and keep tokens out of JavaScript. The choice stays behind the `AuthenticatedUser` extractor. Redis persistence is the operator's concern; a Redis restart that logs everyone out is acceptable.
- **CSRF.** `SameSite=Lax`, Angular's built-in XSRF cookie/header convention (double-submit token), and an `Origin` check on state-changing requests.
- **Google login.** `openidconnect` crate: authorization code + PKCE, `state` and `nonce`, ID-token signature verification. Identity is keyed on Google's `sub`, never the email.
- **Account model.** `users` plus `user_identities (provider, provider_subject, user_id)`; an optional password credential. Keep the code provider-agnostic so a second provider is configuration plus a registration.
- **Linking policy.** A Google login links to an existing account only when the ID token has `email_verified = true`; otherwise it is refused. With invite-only sign-up, a Google login for an email with no invitation or account is also refused.
- **Invites.** Admin-only. An admin creates an invitation for an email address (single-use token, stored hashed, expiring). The invitee accepts by setting a password or signing in with Google; accepting counts as email verification. Roles are assigned by the admin at invite time; the first admin comes from env-driven bootstrap (WS9).
- **Password side.** `argon2` (RustCrypto) with argon2id and its recommended parameters. Verification, reset and invite tokens: 256-bit random, stored hashed, single-use, short expiry (Redis TTLs suit this; the invitation record itself is a DB row). Login throttling per account and per IP in Redis. Signup, login and reset responses must not reveal whether an email exists.
- **Same origin.** Staging (the only deployment for now) serves the Angular app and API on one origin behind a reverse proxy, or on sibling subdomains. That keeps cookies first-party and CORS minimal. Revisit only if production needs a different layout.

**Candidates:** `openidconnect` (pulls in `oauth2`), `tower-sessions` + Redis store / `axum-login`, `argon2` / `password-hash`, `lettre` (via WS5). (Names verified to exist on crates.io 2026-10-19, with the release-age flags in §2.3; maintenance and fit are still for the D1 spike to confirm. `openidconnect` 4.0.1 depends on `oauth2` and `rsa`.) **D2 spike finding:** `tower-sessions-redis-store` 0.16 does not compile against `tower-sessions` 0.15; `tower-sessions` 0.14 + that store compiles, and `axum-login` 0.18 uses 0.14 (see D2 results). Not yet tested end to end.

**Coupling:** Redis becomes a runtime dependency (also relevant to D3 option c and D2's Redis check); D2 (users, identities, invitations tables); WS5 (invite, verification and reset emails); WS8 (extractor, CORS, XSRF).

**Interim seam:** an `AuthenticatedUser { id: UserId, roles }` extractor in `ch-api` plus a `UserId` newtype taken by every repository method. Everything before WS9 runs against a dev-stub extractor.

**Spike (before WS9):** against a local Angular dev origin, Google login and email+password login both produce the same session cookie, including the CSRF behaviour. Must also prove failures: wrong password, unverified email, expired or reused token, uninvited Google account, unverified Google email, and throttling. Output: confirmation of B (or a switch to Kratos) and a verified crate list.

### D2 — Database · **decided: PostgreSQL 18**

**Candidates:** **MariaDB** (cheap, familiar, the current system's engine family) vs **PostgreSQL**. SQLite is dropped: a single writer at a time and no `SKIP LOCKED` (neither tested in the spike). Redis is already a runtime dependency (D1) and is not a candidate for the primary store.

**Is relational right?** Yes. The core data is relational (users → alerts → state-utility indices → offers → matches) and needs foreign keys, uniqueness and multi-row transactions (offer refresh is delete-and-insert). A document or key-value primary would mean re-implementing those guarantees. Non-relational tech only earns supporting roles: Redis for sessions and throttling (decided in D1), possibly the job queue (D3), and optionally TimescaleDB for product price history (stage B, a PostgreSQL-only extra).

**Scale is tiny** (about 100 offers per index, daily refreshes, modest users), so throughput does not decide this. Features, Rust driver behaviour and queue-crate support do.

#### Spike results (run 2026-10-17)

Everything below was measured or read from tool output. Harness, compose file, raw outputs and the check crates are in `spikes/d2-database/` (`results/*.txt`). Re-run with `docker compose up -d` and `d2-harness <mysql|postgres|timescale> <url> <label>` from `harness/`.

**Versions tested:** MariaDB **13.0.2** (`latest`) and **12.3.3** (`lts`); PostgreSQL **18.6**; `timescale/timescaledb:latest-pg18` (PostgreSQL 18.6, TimescaleDB **2.30.2**). sqlx **0.9.0**, rust_decimal 1.43.0, chrono 0.4.45, testcontainers-modules 0.15.0, sqlx-cli 0.9.0. Each container: 2 CPUs, 1 GiB, named volume, default config. MariaDB ran `REPEATABLE-READ` (its default here), PostgreSQL `read committed`.

| # | Check | MariaDB 13.0.2 | MariaDB 12.3.3 | PostgreSQL 18.6 |
|---|---|---|---|---|
| 1a | sqlx migrations from empty | ok | ok | ok |
| 1b | DDL inside a transaction, 2nd statement fails | first statement's table **still exists** after rollback | same | table **rolled back** |
| 1c | sqlx migration with 2 statements, 2nd bad | half-applied: `half_b` left behind, `_sqlx_migrations` row `success=false`, rerun refuses ("migration 2 is partially applied; fix and remove row") | same | clean: no leftover table, no failed row; no leftover table and no failed row recorded (rerun after fixing the file was not tried) |
| 2 | decimal(12,5), microsecond timestamp, bool round-trips | exact | exact | exact |
| 2b/2c | 6 decimal places / overflow into (12,5) | rounds to 0.12346 / error | same | rounds to 0.12346 / error |
| 3 | 100-row delete+insert in one tx; failure at row 50 | rolls back, 100 rows remain | same | same |
| 3 | …median time | 2.45 ms | 2.51 ms | 2.25 ms |
| 4 | filtered query, 5k / 200k rows (median) | 0.67 / 0.61 ms | 0.61 / 0.64 ms | 0.48 / 0.60 ms |
| 5 | `SKIP LOCKED` claim, 2000 jobs, 8 workers, select-then-update | **deadlock errors**; 6 of 8 workers hit the harness's 500-error cap and stopped, so the error count (~3,550) is a floor and the time (5–12 s over three runs) is not like-for-like | **deadlock errors**; 7 of 8 workers hit the cap (~3,520), time not comparable | 0 errors, 0.56 s |
| 5 | same, with `READ COMMITTED` | 0 errors, 1.6 s | 0 errors, 1.4 s | 0 errors, 0.56 s |
| 5 | single statement `UPDATE … WHERE id = (… SKIP LOCKED) RETURNING` | 0 errors, 1.1 s | **syntax error** | 0 errors, 0.32 s |

No job was claimed twice in any run. Each engine ran as one container on Docker Desktop's macOS VM, against small synthetic data, and most numbers are single runs. They are representative of how the engines compare, not of production performance; differences of a few tenths of a millisecond are noise, and the MariaDB deadlock timing varied by more than 2× between runs. The Postgres "READ COMMITTED" row is the same configuration as its default, so it is a repeat run, not a separate test.

**Feature probes** (a failed probe means the statement was rejected):

| Feature | MariaDB 13.0.2 | MariaDB 12.3.3 | PostgreSQL 18.6 |
|---|---|---|---|
| `CHECK` constraint enforced | yes | yes | yes |
| Native partial unique index (`CREATE UNIQUE INDEX … WHERE`) | **no** (syntax error); a unique index over a generated column worked | same | **yes** |
| Enum rejects invalid value | yes | yes | yes |
| JSON | reported column type is `longtext`; invalid JSON rejected by a check; `JSON_VALUE` path query ok | same | `jsonb`; invalid rejected; path query ok; GIN index created ok |
| `INSERT` / `DELETE … RETURNING` | ok | ok | ok |
| `UPDATE … RETURNING` | ok | **syntax error** | ok |
| `REPLACE … RETURNING` | ok | ok | not applicable (syntax error; use `ON CONFLICT`) |
| Upsert | `ON DUPLICATE KEY UPDATE` ok | ok | `ON CONFLICT DO UPDATE` ok |
| `LISTEN` / `NOTIFY` | **none** (syntax error) | none | works (payload received) |

**Compile-time query checking** (sqlx 0.9 `query!`, `spikes/d2-database/results/compile-time-checks.txt`):
- PostgreSQL: all columns inferred correctly with no help (`Decimal`, `bool`, `DateTime<Utc>`, `Option<String>`).
- MariaDB: the macro inferred `BOOLEAN` as `i8` and `DATETIME(6)` as `NaiveDateTime`. It compiles once each such column gets an override (`AS "is_green: bool"`, `AS "fetched_at: DateTime<Utc>"`).
- Both engines reject an unknown column and a wrong Rust type at compile time.
- Offline mode (`cargo sqlx prepare`, then `SQLX_OFFLINE=true` with no database) works for both. A query changed without re-running `prepare` fails the build. If both engines were ever in one workspace, each crate needs its own `.sqlx` directory; a shared one got overwritten.

**Crate support** (crates.io metadata and `cargo check`, `results/crates-survey.txt`):
- Job queues: `apalis-postgres` 1.0.0 and `graphile_worker` 0.14.1 depend on **sqlx 0.9**. `apalis-mysql` is only a release candidate (1.0.0-rc.8) and depends on **sqlx 0.8**. `fang`, `sqlxmq` and `underway` also depend on sqlx 0.8 (`sqlxmq`, `underway` are Postgres only). `apalis-redis` 1.0.0 exists. `apalis-postgres` and `graphile_worker` compiled together with sqlx 0.9 and a single sqlx in the tree. They were **compiled, not run**.
- Sessions (affects D1): `tower-sessions-redis-store` 0.16.0 (last published 2025-01-01) targets `tower-sessions-core` 0.14, while `tower-sessions` 0.15.0 uses core 0.15, and **they do not compile together** (`RedisStore` does not implement the 0.15 `SessionStore`). `tower-sessions` 0.14 + `tower-sessions-redis-store` 0.16 compiles, and `axum-login` 0.18.0 depends on `tower-sessions` 0.14. `tower-sessions-sqlx-store` 0.15.0 depends on sqlx 0.8.
- testcontainers: `testcontainers-modules` 0.15.0 builds on `testcontainers` 0.27 (0.28 is current). Its default images are **PostgreSQL 11.22 and MariaDB 11.3.2**, so tags must be pinned with `.with_tag(..)`. With pinned tags both started, connected and answered a query in about 4 s.

**Operations** (single samples, Docker Desktop VM; `results/ops.txt`):
- Image size: MariaDB 386 MB (13.0.2) / 361 MB (12.3.3), PostgreSQL 480 MB, Timescale image 1.08 GB.
- Memory after the workload (`docker stats`, one sample, default settings): PostgreSQL 82 MiB, MariaDB 257 / 238 MiB.
- Restart to ready: MariaDB 1.1 / 1.3 s. The PostgreSQL reading (0.1 s) is just one `docker exec` round-trip and is not a usable startup measurement.
- Backup and restore of the 200,100-row table: row counts identical after restore on all three. `pg_dump -Fc` 0.30 s and `pg_restore` 0.46 s; `mariadb-dump` 0.21 s and restore 0.96 s. (Dump formats differ, so sizes are not comparable.)

**TimescaleDB** (optional extra; PostgreSQL only): extension installs, `create_hypertable` works, 584,200 rows loaded (200 products × 2 years, 6-hourly). A 90-day daily average for one product took 0.86 ms on a plain table and 1.16 ms on the hypertable. At this size the hypertable showed **no benefit**; this does not say anything about larger data.

#### What was not tested

- **Rust access layers other than sqlx** (SeaORM, Diesel) and their migration tools.
- **Anything on Linux or your server.** All timings are from Docker Desktop's macOS VM and are only valid for comparing the engines with each other.
- **Queue crates in operation.** `apalis-postgres`, `graphile_worker` and `apalis-mysql` were only compiled or read from metadata, not run. D3 still needs that.
- **The Redis session path end to end** (login, cookie, CSRF). That is the D1 spike.
- **Durability and crash recovery, `fsync` settings, replication, major-version upgrades, TLS, connection limits, long soak runs and memory growth.**
- **Realistic data volume or query plans.** The table was synthetic (200k rows, one composite index) and runs were single samples with no variance estimate. Differences of a few tenths of a millisecond should be read as equal.
- **Whether `READ COMMITTED` is acceptable on MariaDB for the rest of the application.** It fixed the claim deadlocks in this test; its effect on other queries was not examined.
- **Production-like auth and hosting** (managed vs self-hosted instances).

#### Verdict

The measured gaps are not about speed (query and write times are the same within noise). They are about correctness and fit:

1. **Migration safety.** On MariaDB a failed migration leaves a half-applied schema and sqlx blocks until it is repaired by hand. On PostgreSQL it rolls back cleanly. A greenfield schema will change often, so this is the most relevant difference.
2. **Job claiming.** With default settings MariaDB deadlocked heavily under concurrent `SKIP LOCKED` claimers; it works with `READ COMMITTED` or a single `UPDATE … RETURNING` (13.0.2 only). PostgreSQL needed neither.
3. **Queue crates.** The sqlx 0.9 queue crates found are Postgres (`apalis-postgres`, `graphile_worker`). The MariaDB path is an `-rc` crate on sqlx 0.8, which would put two sqlx versions in the build.
4. **Type inference.** MariaDB's compile-time checks need manual overrides for booleans and timestamps.
5. **Features used by the plan:** partial unique indexes (for the "one current alert per…" style rules), `RETURNING` on every statement, `LISTEN/NOTIFY` for job wake-ups. All exist in PostgreSQL 18.6; MariaDB needed a workaround or lacked them.
6. **Operations.** The earlier assumption that MariaDB is lighter did not hold on memory in this test (one sample, defaults): PostgreSQL used less. MariaDB's image is smaller. Neither difference matters much.

**Decision (yours, after the spike): PostgreSQL 18.** MariaDB is not carried forward. The spike's main differences were items 1–3 above. Pin the image to a minor tag (the spike used `postgres:18.6`) in compose and CI, and never rely on `testcontainers-modules`' default tag.

**Rust access layer.** sqlx 0.9 with its migrator, compile-time checked queries (`query!` family), and the offline cache (`.sqlx/`, generated by `cargo sqlx prepare`) committed to the repo and verified in CI. SeaORM and Diesel were not evaluated and are not planned.

**Needs:** about a dozen domain tables (account tables per D1, created by WS9); booleans, decimals, UTC timestamps with microsecond precision; unique indexes; FKs with explicit cascade rules; a transactional delete-and-insert for offer refresh; job-claiming if D3 is DB-backed; a test-container-friendly image.

**Coupling:** D3 (queue crate choice follows the database: Postgres has `apalis-postgres` and `graphile_worker`), D1 (users, identities and invitations tables; also the `tower-sessions` 0.14 pin above).

**Decided conventions** (apply to every table; WS2 schema review checks them):

- **IDs: auto-incrementing 64-bit integers.** Columns are `BIGINT GENERATED ALWAYS AS IDENTITY` primary keys; Rust uses newtypes over `i64` (`UserId`, `AlertId`, …). Because IDs are guessable, authorization never relies on ID secrecy: every repository method takes the `UserId` and filters on it (D1's interim seam). Tokens (sessions, invitations, password resets) are random values, not IDs (D1).
- **Timestamps: UTC, always.** Columns are `timestamptz` (microsecond precision, `timestamptz(6)`), mapped to `chrono::DateTime<Utc>`; no `timestamp without time zone`. The API returns RFC 3339 UTC strings and clients convert to the user's locale. The spike round-tripped microseconds exactly. A schedule that depends on a local time zone (for example America/New_York) is a job-runner concern for D3, not a storage one.
- **Money and rates: exact decimals, never floats.** Columns are `numeric(p, s)` mapped to `rust_decimal::Decimal` (the spike used `numeric(12,5)`: exact round-trip, overflow rejected). Precision and scale are set per column in WS2 from the real source data (the CSV endpoint may carry more places than the HTML; unchecked), with rates stored to at least the 4 decimal places the PA Power fixture shows (e.g. `$0.0690`); the spike's `numeric(12,5)` was an arbitrary test shape, not derived from the source. The spike also showed PostgreSQL silently rounds extra decimal places on insert (0.123456 stored as 0.12346), so the API and domain layers must validate or round explicitly rather than rely on the database.
- **Booleans** are `boolean`; **enumerated values** use PostgreSQL enum types or `CHECK`-constrained text (WS2 decides per case; both rejected bad values in the spike).


### D3 — Workers and tasks

**Needs (from the current system):**

- Recurring: offer refresh (all active indices; **≥ 15 s between loads** today), alert assessment (all active alerts), price scraping (all enabled products; 2 s between scrapes today; honours cancellation), notification dispatch (new).
- On-demand: refresh one index then assess one alert (currently done synchronously inside an HTTP request — see H6).
- Ordering: offers → assess → notify. Today this is faked with clock offsets (00:00 / 01:00 / 02:00 ET) and a single worker; prefer explicit chaining.
- Throttling per target host/retailer; retries with backoff; single-flight (no overlapping runs of the same job); per-item error isolation; graceful shutdown; timezone-aware cron; run history + manual trigger (the Hangfire dashboard's job, currently deny-all in prod).
- Jobs differ in weight: a CSV/JSON fetch is cheap, a headless-browser scrape is not. Per-kind concurrency limits (or separate pools) are likely needed — you mentioned a mix of load- and scrape-style sources.

**Options:** (a) in-process scheduler with our own state tables; (b) a DB-backed job-queue crate (must support D2's database); (c) a broker-backed queue (e.g. Redis) — adds infrastructure; (d) hand-rolled on tokio with a claim table and `SKIP LOCKED`.

**Candidates:** `apalis`, `fang`, `tokio-cron-scheduler`, `cron`/`croner` (schedule parsing only), `governor` (rate limiting).

**Interim seam:** write all job bodies as plain async functions in `ch-engine` (`refresh_index(id)`, `assess_alert(id)`, `scrape_tracked_retailer(id)`, `dispatch_notifications()`), plus a minimal `JobQueue::enqueue(kind, payload, run_at)` trait. Which runner calls them becomes swappable.

### D4 — Web scraping crates (two stages)

Utility sources and product scraping have very different risk profiles, so this decision, its spike, and the API work built on it are **split**. Utility prices come first; products are a separate, later stage.

#### D4a — Utility sources (stage A, first)

The .NET code's TODO comments point at structured PA Power endpoints (a `ZipSearch` JSON API and a `RateCSV` download). **Whether they still work is unverified** (see §1.5); if they do, this is mostly plain HTTP plus parsing with low anti-bot exposure, and the spike below decides.

**Needs:** HTTP client with timeouts, retries/backoff, a per-host rate limiter, polite headers; JSON and CSV parsing (serde + a CSV reader); an HTML parser **only as a fallback** if the structured endpoints are gone; fetching behind a trait (`HttpFetcher`) so parsers run on saved fixtures and wiremock.

**Candidates:** `reqwest`, `serde`, `csv`, `governor`; `scraper` for the HTML fallback.

**Spike (step 2 in §5.1, after D2's):** confirm which PA Power endpoints work today (JSON, CSV, old HTML), capture fixtures, document the field mapping to `Offer`. This is WS4 task 1. Output: a short verdict on which source to build on.

#### D4b — Product scraping (stage B, later)

Retailers are the hard case. The existing code already carries hardcoded session cookies and rotating user agents (§1.6 #10), which suggests the sites resisted plain fetching, and retail sites commonly use bot management (general expectation, not tested here). Plan for **per-retailer go/no-go**, not for scraping everything.

**Needs:**

- **A CSS-selector engine is a hard requirement:** users supply an arbitrary selector per tracked retailer URL (`TrackedProductRetailer.PriceSelector`).
- Static fetch with per-site headers/cookies/UA, cookie jar, redirects, per-host throttling.
- Structured data first: JSON-LD / `schema.org` price data embedded in product pages is usually more stable than CSS and cheaper to fetch than rendering.
- Whether a **headless browser** is needed at all (today's code enables JS execution globally and for Home Depot / Lowe's). It dictates worker image size, memory, and concurrency limits.
- Fallbacks that avoid fighting bot defences: official retailer or affiliate product APIs and feeds where they exist, and the already-built manual price entry. Be ready to drop retailers that can't be fetched reliably.
- Offline testing via fixtures and the `HttpFetcher` / `ProductScraper` traits.

**Candidates:** `reqwest` (+ cookie store), `scraper`, `lol_html`; for rendering, `chromiumoxide` / `headless_chrome`, a WebDriver client (`fantoccini` / `thirtyfour`), or a Playwright-style sidecar; `governor`.

**Spike (step 11 in §5.1, start of stage B):** for each of Lowe's, Home Depot, Best Buy and Appliances Connection (plus any retailer you add), test plain HTTP + selector, JSON-LD, and (only if needed) a rendered fetch, from the machine and network the worker will actually run on. Record a go/no-go table with the observed failure mode and a rough reliability read. Output decides the browser dependency and which retailers ship.

**Interim seam:** `trait HttpFetcher` and `trait ProductScraper` (inputs: URL, selector, retailer profile; output: price or typed error).

---

## 4. Workstreams

Each workstream lists **Ports from** (C# to read for intent), **Depends on**, **Done when**, and a suggested **agent effort** reflecting how much judgement it needs. Treat §1.6 as errata for every "Ports from" file.

### WS1 — Foundation and tooling · effort: low–medium

**Goal:** a runnable skeleton that every other workstream builds on.
**Tasks:** cargo workspace and lint/format/deny config; typed layered config replacing `appsettings.*` (DB, SMTP, base URLs, CORS origin, schedules, throttles that are hardcoded today); tracing; shared error type; explicit `AppState` (no DI container); `docker/` compose with PostgreSQL 18 (pinned tag), Redis (D1) and an SMTP sink on :1025 (as in `appsettings.json`); `fixtures/` directory seeded from [CompareHare.Tests](src/CompareHare.Tests) and [app/public](src/CompareHare.React/app/public); CI (build, test, clippy, fmt, query-check); dev tasks (`just` / `xtask`).
**Ports from:** [AppStartup/](src/CompareHare.Api/AppStartup), [appsettings.json](src/CompareHare.Api/appsettings.json), [appsettings.unraid.json](src/CompareHare.Api/appsettings.unraid.json).
**Depends on:** D2 (compose image).
**Done when:** CI is green; `docker compose up` provides DB + Redis + mail sink; `GET /health` checks DB connectivity.

### WS2 — Data layer · effort: high for schema (human review gate), medium for repos

**Goal:** a clean schema and repositories.
**Tasks:** design the baseline from the verified domain types, using the **final** [model snapshot](src/CompareHare.Api/Migrations/CompareHareDbContextModelSnapshot.cs) only as a reference for intent; add the missing unique key on state-utility indices; make FK/cascade behaviour explicit, especially history ↔ alert-match links (H1); decide the product table shape (H2); apply D2's conventions (bigint identity IDs, `timestamptz` UTC, `numeric` money with per-column precision); indexes for hot paths (alerts by user, offers by index, history by retailer + date); automatic created/modified stamps; repositories per aggregate that take a `UserId` where data is user-owned (ownership enforced structurally, not by per-request validators); seeding (roles, initial indices; an env-driven admin bootstrap instead of the committed seed file); migration runner; test-DB harness.
**Ports from:** [Entities/](src/CompareHare.Domain/Entities), [CompareHareDbContext.cs](src/CompareHare.Domain/Entities/CompareHareDbContext.cs), [Seeds/](src/CompareHare.Api/Seeds).
**Depends on:** D2, WS3. **Blocks:** every workstream that persists.
**Done when:** migrations apply to an empty DB; repository integration tests pass; a generated schema doc is committed.

### WS3 — Domain core · effort: high (semantics matter; good fit for your strongest/highest-reasoning agent)

**Goal:** all business rules as pure, heavily tested code.
**Stage scope:** stage A covers utility types only; product types (retailer enum, price diff) are added in stage B alongside WS6.
**Tasks:**
- Types and stable numeric codes for `UtilityState`, `UtilityType` and the 15 retailers + `Other (1001)`; `Offer` (was `UtilityPrice`), `AlertCriteria` with tri-state filters, `AssessOutcome`.
- **Canonical hashing:** sort by offer id, null-preserving, decimal-safe, SHA-256. Also a hash for a *set of matching offers*.
- **Matcher:** `fn matches(&AlertCriteria, &Offer) -> bool` with an explicit, written filter spec (inclusive bounds, contract length, renewable ranges, tri-state booleans), fixing §1.6 #2–3.
- **Assessor state machine** as a pure function of (index hash, alert's stored hash, previous match hash, current matches) → `{NoChangeAtIndex, NoChangeAtMatch, MatchesChanged(n), MatchesCleared}` with a defined rule for "had matches, now none" (H7).
- Price diff (amount/percent, zero-division rule) and parsers (first integer, first float, currency-with-symbol).
- Port the existing hasher/parser/product-helper tests as the first specs; add table-driven matcher tests and property tests (hash is order-independent; boundary values).
**Ports from:** [AlertAssessor.cs](src/CompareHare.Domain/Features/AlertAssessors/AlertAssessor.cs), [GetAlertMatchesForPaPower.cs](src/CompareHare.Domain/Features/AlertAssessors/Queries/GetAlertMatchesForPaPower.cs), [UtilityPriceHasherHelper.cs](src/CompareHare.Domain/Services/UtilityPriceHasherHelper.cs), [ObjectHasher.cs](src/CompareHare.Domain/Services/ObjectHasher.cs), [ParserHelper.cs](src/CompareHare.Domain/Services/ParserHelper.cs), [PriceHelper.cs](src/CompareHare.Api/Features/Prices/Services/PriceHelper.cs), the matching `*Tests.cs`.
**Depends on:** WS1 skeleton only (no database needed). Done before WS2 so the schema is designed from verified domain types.
**Done when:** the spec doc and tests are reviewed; the crate has no IO dependencies.

### WS4 — Utility offer ingestion · effort: medium (spike first)

**Goal:** get PA Power electric offers into the system reliably, with a clean path to add states and gas.
**Tasks:**
1. **Source discovery spike** (D4a): confirm live `ZipSearch` / `RateCSV` behaviour, capture fixtures, write the field mapping to `Offer`.
2. `OfferSource` trait + registry keyed by (state, type); PA Power electric implementation; per-row parse failures are counted and reported rather than silently logged.
3. State-utility-index service: find-or-create-or-reactivate (replaces `UtilityIndexPersister`), plus the missing **deactivate when no alert references it**.
4. Refresh: fetch → canonical hash → compare with `last_updated_hash` → replace current offers and append history in **one transaction** → update hash. Idempotent.
5. Distributor / rate-type lookup for a zip (backs the alert-creation UI), with caching so the upstream isn't hit per keystroke.
6. Polite fetching: configurable per-host spacing (15 s today), UA, retries with backoff, conditional requests where supported.
7. A short "how to add a state/utility" checklist; PA gas is the first real test of the seam.
**Ports from:** [PAPowerOfferLoader.cs](src/CompareHare.Domain/Features/OfferLoaders/PAPowerOfferLoader.cs) (intent/field list only), [OfferLoaderRunner.cs](src/CompareHare.Api/Features/Offers/Services/OfferLoaderRunner.cs), [OfferPersister.cs](src/CompareHare.Api/Features/Offers/Services/OfferPersister.cs), [UtilityIndexPersister.cs](src/CompareHare.Api/Features/Shared/Services/UtilityIndexPersister.cs), [PaPower/](src/CompareHare.Api/Features/PaPower), [PA_Distributor_Response.json](src/CompareHare.React/app/public/PA_Distributor_Response.json).
**Depends on:** D4a, WS2, WS3.
**Done when:** a fixture-driven test and a `#[ignore]` live smoke test both load PA Power offers; a refresh with unchanged data performs zero writes.

### WS5 — Alerting and notifications · effort: medium–high

**Goal:** close the loop the .NET code never closed — assess alerts and actually tell the user.
**Tasks:**
- Alert lifecycle services (create with validation, delete, list, get; edit if you want it — it was never finished).
- Assessor orchestration: index-hash short-circuit → matcher → compare hashes → persist match + links → **enqueue a pending notification on change** (new; never implemented).
- Notification semantics from ARCHITECTURE.md: notify only when the match-set hash changes and matches are non-empty; batch multiple changes per user into one message; define behaviour for cleared matches (H7). Make the payload generic (`kind`, `user`, `payload`) so product alerts can reuse the dispatcher later.
- Dispatcher: read pending → group per user → render → send → mark sent/delete; crash-safe (no double-send), retry/poison handling.
- Email: SMTP config, **templates (none exist today)**, HTML + text, dev mail sink, link building from the configured web URL. Account emails (verify, reset) are added here once D1 lands.
**Ports from:** [AlertAssessor.cs](src/CompareHare.Domain/Features/AlertAssessors/AlertAssessor.cs), [Alerts/](src/CompareHare.Api/Features/Alerts), [CreateAlertModelValidator.cs](src/CompareHare.Api/Features/Alerts/RequestHandlers/CreateAlert/CreateAlertModelValidator.cs) (validation rules — note several are buggy), [Email/](src/CompareHare.Api/Features/Shared/Services/Email), [AlertFeatureResolver.cs](src/CompareHare.Api/Features/Shared/Mapping/AlertFeatureResolver.cs) (display text for alert parameters).
**Depends on:** WS2, WS3; D3 to schedule; WS4 for real data.
**Done when:** with fixture offers, changing an offer triggers exactly one email to the mail sink; re-running changes nothing.

### WS6 — Product tracking and price scraping · effort: medium (scraper strategy: high)

**Goal:** tracked products, reliable price capture, and the history/graph data the original never delivered.
**Tasks:**
- CRUD: tracked product with retailers (retailer enum, URL, selector, "other" display name), enable/disable product and retailer, list, current view (per-retailer price, change, `has_scraping_exceptions`), retailer options.
- `ProductScraper` trait + registry: default CSS-selector scraper plus per-retailer profiles (selector **fallback lists** — an existing TODO — and request profile). Config lives in data/config, not hardcoded session cookies.
- Change detection: persist only on change; amount/percent diff; clear the failure record on success; on failure store a **truncated** error (optional debug snapshot behind a flag).
- Manual price entry with neighbour-diff recomputation, and the "price already exists on that date" check. Decide price-date semantics (calendar date in one named timezone).
- **History endpoint** for graphs: per-product, per-retailer time series with carry-forward, as sketched in the `GetSummaryHistoryHandler` comments; range/limit parameters.
- Design for the future `TrackedProductCriteria` (conditional alerts) without building it.
**Ports from:** [DefaultPriceScraper.cs](src/CompareHare.Domain/Features/PriceScrapers/DefaultPriceScraper.cs), [ProductHelper.cs](src/CompareHare.Domain/Services/ProductHelper.cs), [PriceScraperRunner.cs](src/CompareHare.Api/Features/Prices/Services/PriceScraperRunner.cs), [PricePersister.cs](src/CompareHare.Api/Features/Prices/Services/PricePersister.cs), [AddManualPriceHandler.cs](src/CompareHare.Api/Features/Prices/AddManualPrice/AddManualPriceHandler.cs), [Products/](src/CompareHare.Api/Features/Products), the product fixtures in [CompareHare.Tests](src/CompareHare.Tests/Domain/MockDocs/Products).
**Stage B — starts only after G7.** **Depends on:** WS2, WS3, D4b, WS7. **Done when:** fixture-driven scrape tests pass for the retailers that D4b's go/no-go table lets through; manual entry and history are covered by integration tests.

### WS7 — Job runtime · effort: high (concurrency and failure semantics)

**Goal:** run the work on schedule, safely, and observably.
**Tasks:** implement the scheduler per D3; define jobs (`RefreshAllOffers`, `RefreshIndex(id)`, `AssessAlerts`, `ScrapePrices`, `DispatchNotifications`) with explicit chaining in place of clock offsets; America/New_York cron; configurable per-host throttles and per-kind concurrency; single-flight locking; per-item error isolation; cancellation and graceful shutdown; retries/backoff; a run-history table with last status; manual trigger via CLI and admin endpoint (replacing the Hangfire dashboard); optional run-on-startup flag; tests with a fake clock.
**Ports from:** [BackgroundJobs/](src/CompareHare.Api/BackgroundJobs), [HangfireConfig.cs](src/CompareHare.Api/AppStartup/HangfireConfig.cs), the three `*Runner` services.
**Depends on:** D3, WS1, WS2; engine functions from WS4–6 can be stubbed initially.
**Done when:** a scheduled chain runs end-to-end against fixtures; a killed worker leaves no stuck or duplicated work.

### WS8 — HTTP API · effort: low–medium (high volume, low risk — a good fit for faster/cheaper agents)

**Goal:** the REST surface, contract-first.
**Tasks:** route parity per Appendix B (redesigned per H3); one error envelope (RFC 7807-style with per-field errors — replaces `CustomValidationFailures`); JSON conventions (camelCase, consistent enum representation, ISO-8601 UTC); auth extractor (D1 seam); config-driven CORS; pagination on lists; login-endpoint rate limiting; the create-alert → status-polling flow from H6; committed OpenAPI spec; a static-files hook for the SPA left behind config (hosting the frontend is out of scope).
**Ports from:** controllers and `Models/` under [Features/](src/CompareHare.Api/Features); validators for rules only.
**Depends on:** WS2, WS3; D1 seam.
**Done when:** every route in Appendix B has an integration test and appears in the OpenAPI document.

### WS9 — Accounts and authentication · effort: medium–high (security-sensitive; review closely)

**Goal:** implement D1's outcome.
**Tasks:** account and `user_identities` tables; email+password registration, verification, login/logout, forgot/reset/change password; admin-only invitations (invite-only sign-up), Google OIDC login (code flow with PKCE, `state`/`nonce`), the account-linking policy from D1, Redis-backed session cookie (no JWT), roles, lockout and login throttling, `current-user/identity`, env-driven admin bootstrap, CORS/CSRF for the Angular origin. **Regression tests for §1.6 #1** (wrong password must fail; unconfirmed email must fail).
**Ports from:** [Authentication/](src/CompareHare.Api/Features/Authentication), [CurrentUser/](src/CompareHare.Api/Features/CurrentUser), [User.cs](src/CompareHare.Domain/Entities/User.cs).
**Depends on:** D1, D2, WS5 (email).

### WS10 — Ops, packaging and deployment · effort: low–medium

**Goal:** something you can deploy and keep running.
**Tasks:** multi-stage Dockerfile (image size depends on D4b's browser verdict; stage A ships without a browser); compose for dev and prod; deployment notes for your target (the Unraid config in the repo suggests a home server); secrets via env; Redis as a runtime dependency (sessions, throttling); migrations as an explicit `migrate` step; DB backup routine; health/readiness; structured logs; admin visibility into job runs and **repeated scrape failures** (self-monitoring); retire Vagrant and the Hangfire DB.
**Depends on:** WS1, WS7.

### WS11 — Parity review and retiring the .NET code · effort: low–medium

**Goal:** confirm the Rust backend covers the intended behaviour, then remove the old code.
**Tasks:** an errata checklist confirming each §1.6 item is fixed or deliberately dropped; a spec-and-fixture parity review (there is no live oracle — the .NET app doesn't build as-is); rewrite [ARCHITECTURE.md](ARCHITECTURE.md), [PRODUCT_ARCH.md](PRODUCT_ARCH.md) and the README to describe the Rust implementation (today they describe several generations of design); remove the .NET projects, Vagrant config and Hangfire settings. The React app speaks the old routes and won't work against the new API until the separate frontend project ports it — Appendix B is the route map for that.
**Depends on:** everything above.
**Done when:** the repo contains the Rust backend, the untouched frontend, and docs that match them.

---

## 5. Build order and working with agents

### 5.1 Build order

The work is strictly sequential: every step builds on output you have already verified. The table is meant to be copied into GitHub as one milestone per row, with each task bullet in §4 becoming one or more ordered issues. **G** entries are gates — review issues assigned to you, so nothing that depends on a contract is queued until you have approved it.

**Stage A — utility prices (all of it ships first):**

| Step | Work | Output / gate |
|---|---|---|
| 1 | D2 spike (relational comparison, `SKIP LOCKED`, access layer) | **Done 2026-10-17:** comparison and verdict in D2 (§3); you decided PostgreSQL 18 (this was the first gate, "G1") |
| 2 | D4a spike (PA Power source discovery = WS4 task 1) | Verdict on which source to build on + captured fixtures |
| 3 | WS1 foundation | CI green; compose gives DB + Redis + mail sink |
| 4 | WS3 domain core (utility scope) | **G2:** you review domain types/traits and the matcher/assessor spec |
| 5 | WS2 data layer (utility domain tables) | **G3:** you review the schema |
| 6 | WS4 utility ingestion | PA Power refresh works from fixtures, plus an ignored live smoke test |
| 7 | WS5 assessment + notification outbox + email; WS8 alert routes (dev-stub auth); jobs triggered by CLI | **G4: vertical slice** — PA Power electric: create alert → refresh → assess → email in the mail sink |
| 8 | D3 decision, then WS7 job runtime (utility jobs: refresh, assess, notify) | Scheduled chain runs end-to-end |
| 9 | D1 spike (confirms option B), then WS9 accounts and auth (password + Google, invite-only); account emails in WS5 | **G5:** you review auth, including the wrong-password regression tests |
| 10 | WS8 finish for utilities (error envelope, pagination, rate limiting, OpenAPI), then WS10 ops and packaging | **G6:** utility stage is deployable; OpenAPI reviewed |

**Stage B — products (separate, later; stiffer anti-bot headwinds):**

| Step | Work | Output / gate |
|---|---|---|
| 11 | D4b spike (per-retailer go/no-go, browser verdict) | Go/no-go table. **G7:** you decide which retailers ship and whether a browser dependency exists |
| 12 | WS6 product tracking and scraping: product domain types and tables (added to WS3/WS2 outputs), product routes, product scrape job added to WS7, OpenAPI additions | Fixture-driven scrape tests for the retailers that passed; history endpoint |
| 13 | WS10 image/ops updates (browser, if any); WS11 parity review and retiring the .NET code | **G8:** final review |

You can settle D1 and D3 earlier than their steps; the interim seams in §3 keep everything before them unblocked. Account tables are created by WS9, not WS2 (WS2 only carries a `user_id` reference), so the schema review at G3 doesn't depend on D1. No calendar estimates — pacing is set by your verification gates.

### 5.2 Turning this into issues and working with agents

- **Milestone per workstream, issue per task**, ordered as in §5.1. Keep each issue small enough that you can verify one agent's output in a single sitting.
- **Issue template:** goal; the "Ports from" files; the §1.6 errata that apply; the relevant decision's verdict; acceptance criteria (the workstream's "Done when", narrowed to the issue); tests required with the change; a reminder that every crate is Rust 2024 edition.
- **Gates are issues too,** assigned to you. No dependent issue is queued until the gate issue is closed.
- **Contract changes after a gate** (domain types, schema, OpenAPI) get their own issue so they are reviewed rather than slipping in.
- **Spikes** need a written exit criterion and a defined output appended to this document, so verifying one is a read, not a rerun.
- **Effort routing per issue (suggestion):** highest reasoning for WS3, the WS2 schema, WS7 and WS9; mid-tier for WS4–6; faster/cheaper settings for WS1, WS8, WS10 and WS11.

## 6. Test strategy

- **Pure core:** table-driven and property tests (hash order-independence, matcher boundaries, assessor transitions). Port the existing hasher/parser/product-helper tests first — they're the only behavioural specs that exist.
- **Parsers and scrapers:** saved fixtures, with `HttpFetcher` behind a trait; wiremock for retry/backoff; a small set of `#[ignore]` live smoke tests you run by hand.
- **Persistence and jobs:** testcontainers against the real database; fake clock for the scheduler; crash/restart tests for the dispatcher and refresh transaction.
- **API:** integration tests per route including ownership (user A can't read user B's alerts/products) and the auth regressions from WS9.
- **Snapshots** (insta) for rendered emails and the OpenAPI document.

## 7. Risks

1. **Scraping fragility and anti-bot measures.** Mitigate with the D4b spike (products only; utility sources are structured endpoints), fixtures, per-retailer health reporting, JSON-LD first, and manual entry as a fallback.
2. **D2 ↔ D3 coupling.** Queue crates are tied to specific databases; settle the DB (with the `SKIP LOCKED` spike) before committing to a queue.
3. **Unknown upstream state for PA Power.** Three integration generations and no working loader; WS4's discovery spike is the first real unknown.
4. **Scope is larger than a port.** Notifications, signup/verification/reset, history graphs and a working PA source are new builds.
5. **Hidden behaviour in mapping profiles and per-request validators.** I did not read every AutoMapper profile or validator; port DTO shapes and rules from the handlers, and expect a few surprises.
6. **Headless-browser cost,** if D4b shows it's needed: image size, memory, and worker concurrency limits.
7. **Docs vs code ambiguity.** When they conflict, ask rather than infer — §1.5 and H1–H7 list the known cases.

---

## Appendix A — .NET → Rust concept map

| .NET today | Rust direction |
|---|---|
| Autofac + `IFeatureService` assembly scanning | Explicit `AppState` struct; trait objects / generics; registries for strategy lookup |
| MediatR handler + message per endpoint | Plain handler functions calling services; no mediator |
| AutoMapper profiles / value resolvers | `From`/`TryFrom` impls and small mapping functions |
| FluentValidation + custom validator pipeline | `validator`/`garde` + domain-level checks; one error envelope |
| EF Core + Dapper `SqlBuilder` | D2's query layer (sqlx, PostgreSQL); filters evaluated in memory (H4) |
| EF migrations (25) | One baseline migration + forward migrations |
| ASP.NET Identity | D1 (Google OIDC + password, invite-only, Redis cookie session) |
| Hangfire (MySQL storage) + dashboard | D3 + run-history table + admin endpoint/CLI |
| `RateLimiter` (TimeLimiter) | `governor` or equivalent, per host |
| AngleSharp / AngleSharp.Js | D4a (`reqwest`, serde, csv; `scraper` as fallback) / D4b (selectors + optional headless browser) |
| Serilog | `tracing` |
| `SmtpClient` + Handlebars.Net | `lettre` + template engine |
| NodaTime / mixed `DateTime` | chrono/jiff with UTC storage and one named display/schedule timezone |
| Newtonsoft + `StringEnumConverter` | serde with a documented enum representation |

## Appendix B — Current API surface (route map for the contract redesign)

| Current route | Purpose | Workstream |
|---|---|---|
| `POST api/authentication/sign-in` · `DELETE api/authentication/log-out` | Session login/logout | WS9 |
| `GET api/current-user/identity` | User + roles | WS9 |
| `GET api/alerts/list` · `GET api/alerts/{id}` · `POST api/alerts` · `DELETE api/alerts/{id}` | Alert CRUD (edit is commented out) | WS5 / WS8 |
| `POST api/offers/populate/{alertId}` | Synchronous load-and-assess for a new index (see H6) | WS4 / WS5 |
| `GET api/paPower/distributors/list/{zip}` | Distributors and rate types for a zip | WS4 |
| `GET api/products/list` · `GET api/products/current/{id}` | Product list / current prices | WS6 |
| `GET api/products/create` · `POST api/products/create` | Create-form data / create | WS6 |
| `PUT api/products/{id}/toggle` · `PUT api/products/retailers/{id}/toggle` | Enable/disable | WS6 |
| `GET api/prices/manual/{retailerId}/check` · `POST api/prices/manual/{retailerId}` | Manual price entry | WS6 |
| *(none — stub only)* | Price history for graphs | WS6 |

## Appendix C — Data model inventory

| Table | Purpose | Rust plan |
|---|---|---|
| `AspNetUsers`, `Roles`, `UserRoles`, claims/logins/tokens | Identity | Shape per D1; keep roles `Admin` / `User` |
| `StateUtilityIndices` | Distinct (state, type, id1–3) + `LastUpdatedHash` | Keep; add unique key and deactivation |
| `UtilityPrices` | Current offers per index | Keep (refresh replaces) |
| `UtilityPriceHistories` | Offer snapshots | Keep **append-only** (H1) |
| `Alerts` | User criteria + `StateUtilityIndexHash` | Keep |
| `AlertMatches`, `AlertMatchUtilityPriceHistories` | 1:1 match per alert; ordered links to history | Keep |
| `PendingAlertNotifications` | Outbox | Keep, make generic with a sent state |
| `TrackedProducts`, `TrackedProductRetailers` | Product config (URL, selector, retailer) | Keep |
| `ProductRetailerPriceHistories` | Price time series | Keep |
| `ProductRetailerPrices` | "Current" copy of the latest history row | Drop (H2) |
| `ProductPriceScrapingExceptions` | Last scrape failure per retailer | Keep, truncate payloads |

## Appendix D — Not reviewed

Not read in full: the React app; every AutoMapper profile and per-request validator; `vagrant-config/`; the individual EF migrations (only the final snapshot was consulted for indexes/FKs). The only build attempted was the scratch-copy compile in §1.4; nothing was run. Treat claims about runtime behaviour as read from source, not observed.
