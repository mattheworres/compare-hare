# WS1 — Foundation and tooling

**Goal:** a runnable skeleton every other workstream builds on. Source: [RUST_REWRITE.md §4 WS1](../RUST_REWRITE.md).
**Done when:** CI is green; `docker compose up` provides PostgreSQL + Redis + a mail sink; `GET /health` checks DB connectivity.

**Repository layout** (decided): this is a multi-service repo and the Rust backend is one service. The Cargo workspace lives in `src/backend/`; a future Angular app will sit beside it in `src/frontend/`. The legacy `src/CompareHare.*` .NET and React projects stay where they are until WS11 retires them. Shared, cross-service files stay at the repo root: `docker/` (local infrastructure), `justfile`, `.github/`, `README.md`. Unless a path starts with `src/` or `docker/`, paths below are relative to `src/backend/`.

**Applies to every task below** (include in each agent prompt):

- Follow rules laid out in CLAUDE.md
- Rust **2024 edition**, toolchain per `rust-toolchain.toml`. Greenfield: nothing from the .NET code is preserved.
- Decisions already made: PostgreSQL 18 (sqlx 0.9), Redis, UTC timestamps, tokio/axum. Do not revisit them.
- No secrets in committed files. No `latest` image tags.
- Keep the change to what the task names; leave later tasks' work alone.
- Each task ends with `cargo fmt --check`, `cargo clippy --all-targets --all-features -- -D warnings` and `cargo test --workspace` passing (once task 1 exists).
- After completing a task, provide a short and succinct summary of what was completed, including anything that you learned along the way.

---

## 1. Cargo workspace, toolchain and lint configuration

Create the Cargo workspace in `src/backend/` (not the repo root) and the formatting, lint and dependency-policy config. All Cargo commands run from `src/backend/`; the workspace-level files below (`Cargo.toml`, `Cargo.lock`, `rust-toolchain.toml`, `rustfmt.toml`, `clippy.toml`, `deny.toml`, `.gitignore`) live there.

- `src/backend/Cargo.toml`: `[workspace]` with `resolver = "3"`, `members = ["crates/*", "bins/*"]`, `[workspace.package]` (`edition = "2024"`, `rust-version`, `publish = false`, `license`), and `[workspace.dependencies]` as the single place for dependency versions.
- `rust-toolchain.toml` pinning stable `1.99.1` with `rustfmt` and `clippy`.
- Initial members only: `crates/ch-common` (empty lib; later holds config, telemetry and the shared error type) and `bins/comparehare` (binary; prints its version and exits). Do not create the other crates from the plan yet.
- Every member inherits `[lints] workspace = true`.
- Lints live in the workspace `Cargo.toml`, exactly as follows (no additions or relaxations):

  ```toml
  [workspace.lints.clippy]
  all = { level = "deny", priority = -1 }
  unwrap_used = "deny"
  expect_used = "deny"
  panic = "deny"
  undocumented_unsafe_blocks = "deny"
  unwrap_in_result = "deny"
  ok_expect = "deny"
  ```

- Add `clippy.toml` with `allow-unwrap-in-tests = true`, `allow-expect-in-tests = true` and `allow-panic-in-tests = true` so test code may use `unwrap`, `expect` and `panic!`. Production code gets no such exemption; fix violations with `?`, typed errors or explicit handling, never with `#[allow]`.
- `rustfmt.toml`, `deny.toml` (cargo-deny: advisories, licenses, bans, sources; fail on advisories and on unlicensed/unknown registries), and `src/backend/.gitignore` containing `target/`.
- Add a pre-commit check script in a tracked directory (.githooks) as well as the repository-specific git config to use said directory via `core.hooksPath`. Here's the command to run pre-commit: `cargo fmt --check && cargo clippy --all-targets --all-features -- -D warnings.`
- Do not add lint policy beyond what is specified above.

Acceptance: `cargo build`, `cargo fmt --check`, `cargo clippy --workspace --all-targets -- -D warnings` and `cargo deny check` pass on a clean checkout.

Learnings or notes: (to be enumerated by agent)

## 2. CI

Add GitHub Actions in `.github/workflows/ci.yml` at the repo root, triggered on pull requests and pushes to `develop`. Every Rust step runs with `working-directory: src/backend` (so `rust-toolchain.toml` is picked up); cache paths are `src/backend/target/` and `~/.cargo`. Add a path filter so the workflow runs only when `src/backend/**`, `docker/**` or the workflow file change.

- Jobs: build + test, `cargo fmt --check`, `cargo clippy --workspace --all-targets -- -D warnings`, `cargo deny check`.
- Use the pinned toolchain from `rust-toolchain.toml`; cache `~/.cargo` and `target/`; set `RUSTFLAGS`/`CARGO_TERM_COLOR` sensibly; pin third-party actions to a version.
- Leave a clearly commented placeholder for the sqlx offline check (`cargo sqlx prepare --check`); WS2 enables it when the first query exists.
- A service container for integration tests is not needed yet.

Acceptance: workflow file passes `actionlint`; the workflow is green on the PR that adds it.

Learnings or notes: (to be enumerated by agent)

## 3. Local services: docker compose

Create `docker/compose.yaml` for local development.

- `postgres`: official `postgres` image pinned to `18.6`; named volume; healthcheck (`pg_isready`); database `comparehare`, dev-only credentials via an env file.
- `redis`: pinned official image tag; healthcheck; no persistence required.
- `mail`: an SMTP sink on port `1025` with a web UI (for example Mailpit), pinned tag; UI port published.
- Publish ports on `127.0.0.1` only. Provide a committed `docker/.env.example` and gitignore the real `.env`.
- Add a short `docker/README.md`: start, stop, reset volumes, ports, UI URL.

Acceptance: `docker compose -f docker/compose.yaml up -d` reaches `healthy` for postgres and redis; a test message sent to `localhost:1025` appears in the sink UI.

Learnings or notes: (to be enumerated by agent)

## 4. Typed, layered configuration

In `ch-common`, add a typed config struct and loader replacing the .NET `appsettings.*` ([appsettings.json](../src/CompareHare.Api/appsettings.json), [appsettings.unraid.json](../src/CompareHare.Api/appsettings.unraid.json)).

- Layers, lowest to highest precedence: built-in defaults → `config/default.toml` → optional `config/{COMPAREHARE_ENV}.toml` (the config directory defaults to `./config` relative to the working directory, i.e. `src/backend/config`, and is overridable with `COMPAREHARE_CONFIG_DIR`) → environment variables (`COMPAREHARE__SECTION__KEY`). Use `figment` or `config`; pick one and say why in the PR.
- Sections: `database` (URL, pool size), `redis` (URL), `smtp` (host, port, username, password, `from`, tls), `urls` (public web app URL, API base URL), `http` (bind address, CORS allowed origins), `jobs` (cron schedules for offer refresh, alert assessment, price scraping in `America/New_York`; run-on-startup flag; per-host request throttle and per-kind concurrency, defaulting to the values currently hardcoded), `log` (level, format).
- Secrets (DB password, SMTP password) are read from env only and wrapped in a redacting type (`secrecy` or equivalent); `Debug` output never shows them.
- Validate at load (URLs parse, cron expressions parse, ports valid) and fail fast with an error naming the offending key.
- Commit `config/default.toml` and `config/local.toml.example`; gitignore `config/local.toml`.

Acceptance: unit tests cover layering precedence, env override, missing/invalid values, and secret redaction.

Learnings or notes: (to be enumerated by agent)

## 5. Tracing and logging

In `ch-common`, add an `init_telemetry(&LogConfig)` function using `tracing` and `tracing-subscriber`.

- JSON output by default; human-readable output when `log.format = "pretty"`. Level and per-target filters from config, overridable by `RUST_LOG`.
- Include timestamp (UTC), level, target and span fields. Initialise once; calling it twice must not panic.
- Add a request-id convention (field name `request_id`) for later use by the HTTP layer; no HTTP code in this task.
- Do not log secrets or full configuration at startup; log a redacted summary.

Acceptance: tests assert JSON output contains the expected fields and that the level filter is honoured.

Learnings or notes: (to be enumerated by agent)

## 6. Shared error type

In `ch-common`, define the application error model using `thiserror`.

- A top-level `AppError` with variants for: `NotFound`, `Validation` (carrying field-level details), `Unauthenticated`, `Forbidden`, `Conflict`, `Upstream` (an external source/service failed), `Database`, `Internal`. Carry a source error where relevant; keep `anyhow` for binaries only.
- A stable machine-readable `code()` string per variant and an `is_retryable()` helper for the job runtime.
- No HTTP types in this crate. Mapping to status codes and the response envelope belongs to the API workstream (WS8).
- `impl From<sqlx::Error>` is deferred to WS2; do not add an sqlx dependency here.

Acceptance: unit tests for `code()` stability and `is_retryable()` per variant.

Learnings or notes: (to be enumerated by agent)

## 7. Application skeleton: `AppState`, subcommands and `GET /health`

Create `crates/ch-api` and wire the binary.

- `bins/comparehare` uses `clap` with subcommands `serve`, `worker`, `migrate`. Only `serve` does anything; `worker` and `migrate` print "not implemented" and exit non-zero.
- `serve`: load config (task 4), init telemetry (task 5), build an explicit `AppState { config, db: PgPool, redis }` (no DI container, no globals), and run an axum server with graceful shutdown on SIGINT/SIGTERM.
- `GET /health`: returns `200` with `{"status":"ok"}` when `SELECT 1` succeeds against Postgres and Redis answers `PING`; otherwise `503` with the failing component named. No auth. A short timeout on each check.
- Use `sqlx` 0.9 (`postgres`, `runtime-tokio`, TLS feature chosen explicitly) for the pool; no migrations or queries beyond `SELECT 1`.
- Add `tower-http` request tracing that sets the `request_id` field.
- Integration test using `testcontainers` (PostgreSQL 18.6 and a pinned Redis tag): `/health` returns 200 when both are up and 503 when one is stopped.

Acceptance: with compose from task 3 running, `cargo run -p comparehare -- serve` answers `/health` as described; integration test passes in CI (add the Docker requirement to the workflow if needed).

Learnings or notes: (to be enumerated by agent)

## 8. Developer task runner

Add a `justfile` at the repo root wrapping the common commands. Recipes that run Cargo change into `src/backend/` (`set working-directory` or `cd`); infrastructure recipes use `docker/`.

- Recipes: `up`, `down`, `reset` (drop volumes), `fmt`, `lint` (clippy `-D warnings`), `test`, `deny`, `check` (fmt + lint + deny + test, matching CI), `run` (serve against the compose services), `psql`.
- Recipes must use the pinned toolchain and the compose file from task 3; no recipe may need secrets beyond `docker/.env`.
- Document prerequisites (Rust toolchain, Docker, `just`, `cargo-deny`) and the recipe list in a new "Rust backend" section of the root `README.md` (add a section; do not rewrite the existing README).

Acceptance: from a clean checkout, `just up && just check && just run` works as documented.

Learnings or notes: (to be enumerated by agent)

## 9. Fixtures directory

Create `src/backend/fixtures/` and seed it from the existing captured responses.

- Copy, with `git mv`-style history not required: `AppliancesConnection_Response.html`, `BB_Response.html`, `HD_Response.html`, `Lowes_Response.html`, `PA_Distributor_Response.json`, `PA_Response.html` from [src/CompareHare.React/app/public](../src/CompareHare.React/app/public) into `src/backend/fixtures/legacy/`, unmodified.
- Review [src/CompareHare.Tests](../src/CompareHare.Tests) for any embedded sample data and copy it under `src/backend/fixtures/legacy/` the same way; if there is none, say so.
- Add `src/backend/fixtures/README.md`: where each file came from, what it represents (utility offers vs retailer product pages), capture date if known, and the convention for new fixtures (`fixtures/<source>/<name>.<ext>` plus a one-line provenance note).
- Add a small helper in `ch-common` (test-only, behind `cfg(test)` or a `test-support` feature) that loads a fixture by relative path from the workspace root.
- Do not parse or interpret the fixtures; parsers belong to the ingestion workstreams.

Acceptance: files are byte-identical to the originals (checksums listed in the PR); the helper has a test loading one fixture.

Learnings or notes: (to be enumerated by agent)
