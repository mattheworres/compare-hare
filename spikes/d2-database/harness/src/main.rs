//! D2 spike harness: the same checks, run through one trait, against MariaDB and PostgreSQL.
//! Usage: d2-harness <mysql|postgres> <admin-url> <label>

use std::collections::HashSet;
use std::path::Path;
use std::str::FromStr;
use std::sync::Arc;
use std::time::{Duration, Instant};

use anyhow::Result;
use chrono::{DateTime, TimeZone, Utc};
use rust_decimal::Decimal;
use sqlx::migrate::Migrator;
use sqlx::{MySqlPool, PgPool, QueryBuilder, Row};

type Line = (String, String);

fn line(k: &str, v: impl Into<String>) -> Line {
    (k.to_string(), v.into())
}

fn err_text(e: impl std::fmt::Display) -> String {
    let s = e.to_string().replace('\n', " ");
    format!("ERROR: {}", s.chars().take(160).collect::<String>())
}

fn ok_or_err<T>(r: std::result::Result<T, impl std::fmt::Display>) -> String {
    match r {
        Ok(_) => "ok".into(),
        Err(e) => err_text(e),
    }
}

#[derive(Clone, Debug, PartialEq)]
struct Offer {
    index_id: i32,
    plan_name: String,
    rate: Decimal,
    term_months: i32,
    is_green: bool,
    fetched_at: DateTime<Utc>,
}

fn sample_offers(index_id: i32, n: usize) -> Vec<Offer> {
    (0..n)
        .map(|i| Offer {
            index_id,
            plan_name: format!("plan-{index_id}-{i}"),
            rate: Decimal::new(80_000 + (i as i64 * 37) % 9_000, 5),
            term_months: [1, 3, 6, 12, 24, 36][i % 6],
            is_green: i % 3 == 0,
            fetched_at: Utc.timestamp_micros(1_700_000_000_123_456 + i as i64).unwrap(),
        })
        .collect()
}

fn median(mut v: Vec<Duration>) -> Duration {
    v.sort();
    v[v.len() / 2]
}

fn ms(d: Duration) -> String {
    format!("{:.2} ms", d.as_secs_f64() * 1000.0)
}

struct ClaimReport {
    processed: usize,
    duplicates: usize,
    errors: usize,
    elapsed: Duration,
    per_worker: Vec<usize>,
    samples: Vec<String>,
}

impl ClaimReport {
    fn lines(&self, label: &str) -> Vec<Line> {
        vec![line(
            label,
            format!(
                "processed={} duplicates={} errors={} elapsed={} per_worker={:?} error_samples={:?}",
                self.processed,
                self.duplicates,
                self.errors,
                ms(self.elapsed),
                self.per_worker,
                self.samples
            ),
        )]
    }
}

fn summarize(results: Vec<(Vec<i64>, usize, Vec<String>)>, elapsed: Duration) -> ClaimReport {
    let mut seen = HashSet::new();
    let mut dup = 0;
    let mut errors = 0;
    let mut per = vec![];
    let mut samples: Vec<String> = vec![];
    for (ids, errs, s) in &results {
        per.push(ids.len());
        errors += errs;
        for x in s { if !samples.contains(x) && samples.len() < 3 { samples.push(x.clone()); } }
        for id in ids {
            if !seen.insert(*id) {
                dup += 1;
            }
        }
    }
    ClaimReport { processed: seen.len(), duplicates: dup, errors, elapsed, per_worker: per, samples }
}

trait Repo {
    async fn version(&self) -> Result<String>;
    async fn migrate(&self) -> Result<()>;
    async fn failing_migration(&self) -> Vec<Line>;
    async fn roundtrips(&self) -> Vec<Line>;
    async fn replace_offers(&self, index_id: i32, offers: &[Offer], fail_at: Option<usize>) -> Result<()>;
    async fn count_offers(&self, index_id: i32) -> Result<i64>;
    async fn seed(&self, n_indices: i32, per_index: i32) -> Result<()>;
    async fn filter(&self, index_id: i32) -> Result<usize>;
    async fn claim(&self, jobs: i32, workers: usize, variant: &str) -> Result<ClaimReport>;
    async fn probes(&self) -> Vec<Line>;
}

async fn run_checks<R: Repo>(repo: &R, label: &str) -> Result<Vec<Line>> {
    let mut out = vec![line("target", label), line("server version", repo.version().await?)];

    repo.migrate().await?;
    out.push(line("1a migrations from empty", "ok"));
    out.extend(repo.failing_migration().await);
    out.extend(repo.roundtrips().await);

    // 3: transactional delete-and-insert
    let offers = sample_offers(1, 100);
    repo.replace_offers(1, &offers, None).await?;
    let after_first = repo.count_offers(1).await?;
    let fail = repo.replace_offers(1, &sample_offers(1, 100), Some(50)).await;
    let after_fail = repo.count_offers(1).await?;
    out.push(line(
        "3 replace 100 rows atomically; failure at row 50 rolls back",
        format!(
            "first={after_first} rows; injected failure -> {}; rows after rollback={after_fail}",
            if fail.is_err() { "error returned" } else { "NO ERROR" }
        ),
    ));
    let mut times = vec![];
    for _ in 0..50 {
        let t = Instant::now();
        repo.replace_offers(1, &offers, None).await?;
        times.push(t.elapsed());
    }
    out.push(line("3 replace 100 rows (median of 50)", ms(median(times))));

    // 4: filtered query
    for (n_idx, per, tag) in [(50, 100, "5,000 rows"), (2000, 100, "200,000 rows")] {
        repo.seed(n_idx, per).await?;
        let mut times = vec![];
        let mut hits = 0;
        for i in 0..200 {
            let t = Instant::now();
            hits = repo.filter(2 + (i % n_idx.max(2) - 1)).await?;
            times.push(t.elapsed());
        }
        out.push(line(
            &format!("4 filtered query, table has ~{tag} (median of 200)"),
            format!("{} (last result {hits} rows)", ms(median(times))),
        ));
    }

    // 5: SKIP LOCKED claiming
    for variant in ["two-step", "two-step-read-committed", "single-statement"] {
        match repo.claim(2000, 8, variant).await {
            Ok(r) => out.extend(r.lines(&format!("5 claim 2000 jobs, 8 workers, {variant}"))),
            Err(e) => out.push(line(&format!("5 claim, {variant}"), err_text(e))),
        }
    }

    out.extend(repo.probes().await);
    Ok(out)
}

// ---------------------------------------------------------------- MySQL / MariaDB

struct My {
    pool: MySqlPool,
    admin: MySqlPool,
    db_url: String,
}

impl My {
    async fn connect(admin_url: &str) -> Result<Self> {
        let admin = sqlx::mysql::MySqlPoolOptions::new().max_connections(2).connect(admin_url).await?;
        sqlx::query("DROP DATABASE IF EXISTS d2_main").execute(&admin).await?;
        sqlx::query("CREATE DATABASE d2_main").execute(&admin).await?;
        let db_url = format!("{}/d2_main", admin_url.trim_end_matches('/'));
        let pool = sqlx::mysql::MySqlPoolOptions::new().max_connections(10).connect(&db_url).await?;
        Ok(Self { pool, admin, db_url })
    }
}

impl Repo for My {
    async fn version(&self) -> Result<String> {
        let v: String = sqlx::query_scalar("SELECT VERSION()").fetch_one(&self.pool).await?;
        let m: String = sqlx::query_scalar("SELECT @@sql_mode").fetch_one(&self.pool).await?;
        let c: String = sqlx::query_scalar("SELECT @@transaction_isolation").fetch_one(&self.pool).await?;
        Ok(format!("{v} | sql_mode={m} | isolation={c}"))
    }

    async fn migrate(&self) -> Result<()> {
        sqlx::migrate!("./migrations/mysql").run(&self.pool).await?;
        Ok(())
    }

    async fn failing_migration(&self) -> Vec<Line> {
        let mut out = vec![];
        // raw transactional DDL
        let r: Result<()> = async {
            let mut tx = self.pool.begin().await?;
            sqlx::query("CREATE TABLE txa (id INT)").execute(&mut *tx).await?;
            sqlx::query("CREATE TABLE txb (id INT, x NOSUCHTYPE)").execute(&mut *tx).await?;
            tx.commit().await?;
            Ok(())
        }
        .await;
        let exists: i64 = sqlx::query_scalar(
            "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 'txa'",
        )
        .fetch_one(&self.pool)
        .await
        .unwrap_or(-1);
        out.push(line(
            "1b DDL inside a transaction, 2nd statement fails",
            format!("tx result: {}; table created by 1st statement still exists after failure/rollback: {}", ok_or_err(r), exists == 1),
        ));

        // sqlx migrator with a failing migration, in a fresh db
        let _ = sqlx::query("DROP DATABASE IF EXISTS d2_fail").execute(&self.admin).await;
        let _ = sqlx::query("CREATE DATABASE d2_fail").execute(&self.admin).await;
        let url = self.db_url.replace("/d2_main", "/d2_fail");
        let pool = match sqlx::MySqlPool::connect(&url).await {
            Ok(p) => p,
            Err(e) => return vec![line("1c failing migration", err_text(e))],
        };
        let m = Migrator::new(Path::new("migrations_fail/mysql")).await.unwrap();
        let first = m.run(&pool).await;
        let half: i64 = sqlx::query_scalar(
            "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 'half_b'",
        )
        .fetch_one(&pool)
        .await
        .unwrap_or(-1);
        let state: Vec<String> = sqlx::query("SELECT version, success FROM _sqlx_migrations ORDER BY version")
            .fetch_all(&pool)
            .await
            .map(|rows| rows.iter().map(|r| format!("v{}:success={}", r.get::<i64, _>(0), r.get::<bool, _>(1))).collect())
            .unwrap_or_default();
        let second = m.run(&pool).await;
        pool.close().await;
        out.push(line(
            "1c sqlx migration #2 = `CREATE TABLE half_b; CREATE TABLE half_c (bad type)`",
            format!(
                "run1: {} | leftover half_b table: {} | _sqlx_migrations: {:?} | rerun: {}",
                ok_or_err(first),
                half == 1,
                state,
                ok_or_err(second)
            ),
        ));
        out
    }

    async fn roundtrips(&self) -> Vec<Line> {
        let mut out = vec![];
        let d1 = Decimal::from_str("123456.78901").unwrap();
        let t1 = Utc.timestamp_micros(1_710_055_815_123_456).unwrap();
        let r: Result<(Decimal, DateTime<Utc>, DateTime<Utc>, bool)> = async {
            sqlx::query("INSERT INTO rt (d, dt, ts, b) VALUES (?, ?, ?, ?)")
                .bind(d1).bind(t1).bind(t1).bind(true)
                .execute(&self.pool).await?;
            let row = sqlx::query("SELECT d, dt, ts, b FROM rt ORDER BY id DESC LIMIT 1").fetch_one(&self.pool).await?;
            Ok((row.try_get(0)?, row.try_get(1)?, row.try_get(2)?, row.try_get(3)?))
        }
        .await;
        out.push(line(
            "2a round-trip decimal(12,5)/DATETIME(6)/TIMESTAMP(6)/bool as Decimal, DateTime<Utc>, bool",
            match r {
                Ok((d, dt, ts, b)) => format!(
                    "decimal exact={} | datetime(6) micros exact={} | timestamp(6) micros exact={} | bool={}",
                    d == d1, dt == t1, ts == t1, b
                ),
                Err(e) => err_text(e),
            },
        ));
        let r6 = sqlx::query("INSERT INTO rt (d, dt, b) VALUES (?, '2024-01-01 00:00:00', 1)")
            .bind(Decimal::from_str("0.123456").unwrap()).execute(&self.pool).await;
        let stored: Option<Decimal> = sqlx::query_scalar("SELECT d FROM rt ORDER BY id DESC LIMIT 1").fetch_one(&self.pool).await.ok();
        out.push(line("2b insert 6 decimal places into decimal(12,5)", format!("{} (stored {:?})", ok_or_err(r6), stored)));
        let r_over = sqlx::query("INSERT INTO rt (d, dt, b) VALUES (?, '2024-01-01 00:00:00', 1)")
            .bind(Decimal::from_str("99999999.12345").unwrap()).execute(&self.pool).await;
        out.push(line("2c insert value too large for decimal(12,5)", ok_or_err(r_over)));
        let r2040 = sqlx::query("INSERT INTO rt (d, dt, ts, b) VALUES (1, '2040-01-01 00:00:00', '2040-01-01 00:00:00', 1)")
            .execute(&self.pool).await;
        out.push(line("2d TIMESTAMP(6) value in year 2040", ok_or_err(r2040)));
        let rdt = sqlx::query("INSERT INTO rt (d, dt, b) VALUES (1, '2300-01-01 00:00:00', 1)").execute(&self.pool).await;
        out.push(line("2e DATETIME(6) value in year 2300", ok_or_err(rdt)));
        out
    }

    async fn replace_offers(&self, index_id: i32, offers: &[Offer], fail_at: Option<usize>) -> Result<()> {
        let mut tx = self.pool.begin().await?;
        sqlx::query("DELETE FROM offers WHERE index_id = ?").bind(index_id).execute(&mut *tx).await?;
        let mut qb = QueryBuilder::new("INSERT INTO offers (index_id, plan_name, rate, term_months, is_green, fetched_at) ");
        qb.push_values(offers.iter().enumerate(), |mut b, (i, o)| {
            let name = if Some(i) == fail_at { "x".repeat(200) } else { o.plan_name.clone() };
            b.push_bind(o.index_id).push_bind(name).push_bind(o.rate).push_bind(o.term_months).push_bind(o.is_green).push_bind(o.fetched_at);
        });
        qb.build().execute(&mut *tx).await?;
        tx.commit().await?;
        Ok(())
    }

    async fn count_offers(&self, index_id: i32) -> Result<i64> {
        Ok(sqlx::query_scalar("SELECT COUNT(*) FROM offers WHERE index_id = ?").bind(index_id).fetch_one(&self.pool).await?)
    }

    async fn seed(&self, n_indices: i32, per_index: i32) -> Result<()> {
        sqlx::query("DELETE FROM offers WHERE index_id >= 2").execute(&self.pool).await?;
        for chunk in (2..2 + n_indices).collect::<Vec<_>>().chunks(5) {
            let mut qb = QueryBuilder::new("INSERT INTO offers (index_id, plan_name, rate, term_months, is_green, fetched_at) ");
            let rows: Vec<Offer> = chunk.iter().flat_map(|i| sample_offers(*i, per_index as usize)).collect();
            qb.push_values(rows, |mut b, o| {
                b.push_bind(o.index_id).push_bind(o.plan_name).push_bind(o.rate).push_bind(o.term_months).push_bind(o.is_green).push_bind(o.fetched_at);
            });
            qb.build().execute(&self.pool).await?;
        }
        sqlx::query("ANALYZE TABLE offers").fetch_all(&self.pool).await?;
        Ok(())
    }

    async fn filter(&self, index_id: i32) -> Result<usize> {
        let rows = sqlx::query(
            "SELECT id, plan_name, rate, term_months, is_green, fetched_at FROM offers \
             WHERE index_id = ? AND rate <= ? AND term_months >= ? AND is_green = ? ORDER BY rate LIMIT 20",
        )
        .bind(index_id).bind(Decimal::new(85_000, 5)).bind(6).bind(true)
        .fetch_all(&self.pool).await?;
        let _: Vec<DateTime<Utc>> = rows.iter().map(|r| r.get(5)).collect();
        Ok(rows.len())
    }

    async fn claim(&self, jobs: i32, workers: usize, variant: &str) -> Result<ClaimReport> {
        sqlx::query("DELETE FROM jobs").execute(&self.pool).await?;
        let mut qb = QueryBuilder::new("INSERT INTO jobs (state) ");
        qb.push_values(0..jobs, |mut b, _| { b.push_bind("queued"); });
        qb.build().execute(&self.pool).await?;
        let single = variant == "single-statement";
        let pool = if variant == "two-step-read-committed" {
            Arc::new(
                sqlx::mysql::MySqlPoolOptions::new()
                    .max_connections(10)
                    .after_connect(|c, _| Box::pin(async move {
                        sqlx::query("SET SESSION TRANSACTION ISOLATION LEVEL READ COMMITTED").execute(c).await?;
                        Ok(())
                    }))
                    .connect(&self.db_url).await?,
            )
        } else {
            Arc::new(self.pool.clone())
        };
        let t = Instant::now();
        let mut hs = vec![];
        for w in 0..workers {
            let pool = pool.clone();
            hs.push(tokio::spawn(async move {
                let (mut ids, mut errs, mut samples): (Vec<i64>, usize, Vec<String>) = (vec![], 0usize, vec![]);
                loop {
                    if single {
                        let r = sqlx::query_scalar::<_, i64>(
                            "UPDATE jobs SET state='done', claimed_by=? WHERE id = (SELECT id FROM (SELECT id FROM jobs WHERE state='queued' ORDER BY id LIMIT 1 FOR UPDATE SKIP LOCKED) t) RETURNING id",
                        ).bind(w as i32).fetch_optional(&*pool).await;
                        match r {
                            Ok(Some(id)) => ids.push(id),
                            Ok(None) => break,
                            Err(e) => { errs += 1; if samples.len() < 2 { samples.push(err_text(e)); } if errs > 500 { break } }
                        }
                        continue;
                    }
                    let res: Result<Option<i64>> = async {
                        let mut tx = pool.begin().await?;
                        let id: Option<i64> = sqlx::query_scalar(
                            "SELECT id FROM jobs WHERE state='queued' ORDER BY id LIMIT 1 FOR UPDATE SKIP LOCKED",
                        ).fetch_optional(&mut *tx).await?;
                        if let Some(id) = id {
                            sqlx::query("UPDATE jobs SET state='done', claimed_by=? WHERE id=?").bind(w as i32).bind(id).execute(&mut *tx).await?;
                        }
                        tx.commit().await?;
                        Ok(id)
                    }.await;
                    match res { Ok(Some(id)) => ids.push(id), Ok(None) => break, Err(e) => { errs += 1; if samples.len() < 2 { samples.push(err_text(e)); } if errs > 500 { break } } }
                }
                (ids, errs, samples)
            }));
        }
        let mut results = vec![];
        for h in hs { results.push(h.await?); }
        let elapsed = t.elapsed();
        if variant == "two-step-read-committed" { pool.close().await; }
        Ok(summarize(results, elapsed))
    }

    async fn probes(&self) -> Vec<Line> {
        let p = &self.pool;
        let mut o = vec![];
        let e = |s: &str| { let s = s.to_string(); async move { s } };
        let _ = e;
        o.push(line("P1 CHECK constraint enforced", {
            let _ = sqlx::query("CREATE TABLE chk (rate DECIMAL(12,5) CHECK (rate >= 0))").execute(p).await;
            ok_or_err(sqlx::query("INSERT INTO chk VALUES (-1)").execute(p).await)
        }));
        o.push(line("P2 native partial unique index (`CREATE UNIQUE INDEX .. WHERE`)", {
            let _ = sqlx::query("CREATE TABLE pu (id INT AUTO_INCREMENT PRIMARY KEY, user_id INT, active BOOLEAN)").execute(p).await;
            ok_or_err(sqlx::query("CREATE UNIQUE INDEX ux ON pu (user_id) WHERE active").execute(p).await)
        }));
        o.push(line("P2b workaround: unique index over generated column", {
            let c = sqlx::query("ALTER TABLE pu ADD COLUMN active_key INT AS (IF(active, user_id, NULL)) VIRTUAL, ADD UNIQUE KEY ux2 (active_key)").execute(p).await;
            let a = sqlx::query("INSERT INTO pu (user_id, active) VALUES (1, 1)").execute(p).await;
            let b = sqlx::query("INSERT INTO pu (user_id, active) VALUES (1, 1)").execute(p).await;
            let ok3 = sqlx::query("INSERT INTO pu (user_id, active) VALUES (1, 0)").execute(p).await;
            format!("create={} first={} duplicate_active={} inactive_dup={}", ok_or_err(c), ok_or_err(a), ok_or_err(b), ok_or_err(ok3))
        }));
        o.push(line("P3 enum type", {
            let _ = sqlx::query("CREATE TABLE en (k ENUM('a','b') NOT NULL)").execute(p).await;
            ok_or_err(sqlx::query("INSERT INTO en VALUES ('zzz')").execute(p).await)
        }));
        o.push(line("P4 JSON column", {
            let c = sqlx::query("CREATE TABLE js (j JSON)").execute(p).await;
            let ty: Option<String> = sqlx::query_scalar("SELECT column_type FROM information_schema.columns WHERE table_schema=DATABASE() AND table_name='js'").fetch_one(p).await.ok();
            let bad = sqlx::query("INSERT INTO js VALUES ('{bad')").execute(p).await;
            let good = sqlx::query("INSERT INTO js VALUES ('{\"a\":{\"b\":5}}')").execute(p).await;
            let got: Option<String> = sqlx::query_scalar("SELECT JSON_VALUE(j, '$.a.b') FROM js").fetch_one(p).await.ok();
            format!("create={} reported_type={:?} invalid_insert={} valid_insert={} path_query={:?}", ok_or_err(c), ty, ok_or_err(bad), ok_or_err(good), got)
        }));
        for (k, sql) in [
            ("P5a INSERT .. RETURNING", "INSERT INTO chk VALUES (5) RETURNING rate"),
            ("P5b DELETE .. RETURNING", "DELETE FROM chk WHERE rate = 5 RETURNING rate"),
            ("P5c UPDATE .. RETURNING", "UPDATE jobs SET state='x' WHERE id=-1 RETURNING id"),
            ("P5d REPLACE .. RETURNING", "REPLACE INTO chk VALUES (7) RETURNING rate"),
            ("P5e INSERT .. ON DUPLICATE KEY UPDATE", "INSERT INTO pu (id, user_id, active) VALUES (1, 9, 0) ON DUPLICATE KEY UPDATE user_id = 9"),
            ("P6 LISTEN/NOTIFY equivalent (`LISTEN x`)", "LISTEN x"),
        ] {
            o.push(line(k, ok_or_err(sqlx::query(sql).fetch_all(p).await)));
        }
        o
    }
}

// ---------------------------------------------------------------- PostgreSQL

struct Pg {
    pool: PgPool,
    admin: PgPool,
    db_url: String,
}

impl Pg {
    async fn connect(admin_url: &str) -> Result<Self> {
        let admin = sqlx::postgres::PgPoolOptions::new().max_connections(2).connect(admin_url).await?;
        sqlx::query("DROP DATABASE IF EXISTS d2_main WITH (FORCE)").execute(&admin).await?;
        sqlx::query("CREATE DATABASE d2_main").execute(&admin).await?;
        let db_url = format!("{}/d2_main", admin_url.trim_end_matches('/'));
        let pool = sqlx::postgres::PgPoolOptions::new().max_connections(10).connect(&db_url).await?;
        Ok(Self { pool, admin, db_url })
    }
}

impl Repo for Pg {
    async fn version(&self) -> Result<String> {
        let v: String = sqlx::query_scalar("SELECT version()").fetch_one(&self.pool).await?;
        let c: String = sqlx::query_scalar("SHOW transaction_isolation").fetch_one(&self.pool).await?;
        Ok(format!("{v} | isolation={c}"))
    }

    async fn migrate(&self) -> Result<()> {
        sqlx::migrate!("./migrations/postgres").run(&self.pool).await?;
        Ok(())
    }

    async fn failing_migration(&self) -> Vec<Line> {
        let mut out = vec![];
        let r: Result<()> = async {
            let mut tx = self.pool.begin().await?;
            sqlx::query("CREATE TABLE txa (id INT)").execute(&mut *tx).await?;
            sqlx::query("CREATE TABLE txb (id INT, x NOSUCHTYPE)").execute(&mut *tx).await?;
            tx.commit().await?;
            Ok(())
        }
        .await;
        let exists: i64 = sqlx::query_scalar("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public' AND table_name = 'txa'")
            .fetch_one(&self.pool).await.unwrap_or(-1);
        out.push(line(
            "1b DDL inside a transaction, 2nd statement fails",
            format!("tx result: {}; table created by 1st statement still exists after failure/rollback: {}", ok_or_err(r), exists == 1),
        ));

        let _ = sqlx::query("DROP DATABASE IF EXISTS d2_fail WITH (FORCE)").execute(&self.admin).await;
        let _ = sqlx::query("CREATE DATABASE d2_fail").execute(&self.admin).await;
        let url = self.db_url.replace("/d2_main", "/d2_fail");
        let pool = match sqlx::PgPool::connect(&url).await {
            Ok(p) => p,
            Err(e) => return vec![line("1c failing migration", err_text(e))],
        };
        let m = Migrator::new(Path::new("migrations_fail/postgres")).await.unwrap();
        let first = m.run(&pool).await;
        // Same pool, immediately: does the migrator's advisory lock block a retry?
        let same_pool_retry = match tokio::time::timeout(std::time::Duration::from_secs(10), m.run(&pool)).await {
            Ok(r) => ok_or_err(r),
            Err(_) => "BLOCKED >10s (advisory lock still held by pooled connection after the failed run)".to_string(),
        };
        pool.close().await;
        let pool = sqlx::PgPool::connect(&url).await.unwrap();
        let half: i64 = sqlx::query_scalar("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public' AND table_name = 'half_b'")
            .fetch_one(&pool).await.unwrap_or(-1);
        let state: Vec<String> = sqlx::query("SELECT version, success FROM _sqlx_migrations ORDER BY version")
            .fetch_all(&pool).await
            .map(|rows| rows.iter().map(|r| format!("v{}:success={}", r.get::<i64, _>(0), r.get::<bool, _>(1))).collect())
            .unwrap_or_default();
        let second = m.run(&pool).await;
        pool.close().await;
        out.push(line(
            "1c sqlx migration #2 = `CREATE TABLE half_b; CREATE TABLE half_c (bad type)`",
            format!(
                "run1: {} | leftover half_b table: {} | _sqlx_migrations: {:?} | rerun: {}",
                ok_or_err(first), half == 1, state, ok_or_err(second)
            ) + &format!(" | retry on same pool: {}", same_pool_retry),
        ));
        out
    }

    async fn roundtrips(&self) -> Vec<Line> {
        let mut out = vec![];
        let d1 = Decimal::from_str("123456.78901").unwrap();
        let t1 = Utc.timestamp_micros(1_710_055_815_123_456).unwrap();
        let r: Result<(Decimal, DateTime<Utc>, DateTime<Utc>, bool)> = async {
            sqlx::query("INSERT INTO rt (d, dt, ts, b) VALUES ($1, $2, $3, $4)")
                .bind(d1).bind(t1.naive_utc()).bind(t1).bind(true)
                .execute(&self.pool).await?;
            let row = sqlx::query("SELECT d, dt, ts, b FROM rt ORDER BY id DESC LIMIT 1").fetch_one(&self.pool).await?;
            let dt: chrono::NaiveDateTime = row.try_get(1)?;
            Ok((row.try_get(0)?, dt.and_utc(), row.try_get(2)?, row.try_get(3)?))
        }
        .await;
        out.push(line(
            "2a round-trip numeric(12,5)/timestamp(6)/timestamptz(6)/bool as Decimal, DateTime<Utc>, bool",
            match r {
                Ok((d, dt, ts, b)) => format!(
                    "decimal exact={} | timestamp(6) micros exact={} | timestamptz(6) micros exact={} | bool={}",
                    d == d1, dt == t1, ts == t1, b
                ),
                Err(e) => err_text(e),
            },
        ));
        let r6 = sqlx::query("INSERT INTO rt (d, dt, b) VALUES ($1, '2024-01-01 00:00:00', true)")
            .bind(Decimal::from_str("0.123456").unwrap()).execute(&self.pool).await;
        let stored: Option<Decimal> = sqlx::query_scalar("SELECT d FROM rt ORDER BY id DESC LIMIT 1").fetch_one(&self.pool).await.ok();
        out.push(line("2b insert 6 decimal places into numeric(12,5)", format!("{} (stored {:?})", ok_or_err(r6), stored)));
        let r_over = sqlx::query("INSERT INTO rt (d, dt, b) VALUES ($1, '2024-01-01 00:00:00', true)")
            .bind(Decimal::from_str("99999999.12345").unwrap()).execute(&self.pool).await;
        out.push(line("2c insert value too large for numeric(12,5)", ok_or_err(r_over)));
        let r2040 = sqlx::query("INSERT INTO rt (d, dt, ts, b) VALUES (1, '2040-01-01 00:00:00', '2040-01-01 00:00:00+00', true)")
            .execute(&self.pool).await;
        out.push(line("2d timestamptz(6) value in year 2040", ok_or_err(r2040)));
        let rdt = sqlx::query("INSERT INTO rt (d, dt, b) VALUES (1, '2300-01-01 00:00:00', true)").execute(&self.pool).await;
        out.push(line("2e timestamp(6) value in year 2300", ok_or_err(rdt)));
        out
    }

    async fn replace_offers(&self, index_id: i32, offers: &[Offer], fail_at: Option<usize>) -> Result<()> {
        let mut tx = self.pool.begin().await?;
        sqlx::query("DELETE FROM offers WHERE index_id = $1").bind(index_id).execute(&mut *tx).await?;
        let mut qb = QueryBuilder::new("INSERT INTO offers (index_id, plan_name, rate, term_months, is_green, fetched_at) ");
        qb.push_values(offers.iter().enumerate(), |mut b, (i, o)| {
            let name = if Some(i) == fail_at { "x".repeat(200) } else { o.plan_name.clone() };
            b.push_bind(o.index_id).push_bind(name).push_bind(o.rate).push_bind(o.term_months).push_bind(o.is_green).push_bind(o.fetched_at);
        });
        qb.build().execute(&mut *tx).await?;
        tx.commit().await?;
        Ok(())
    }

    async fn count_offers(&self, index_id: i32) -> Result<i64> {
        Ok(sqlx::query_scalar("SELECT COUNT(*) FROM offers WHERE index_id = $1").bind(index_id).fetch_one(&self.pool).await?)
    }

    async fn seed(&self, n_indices: i32, per_index: i32) -> Result<()> {
        sqlx::query("DELETE FROM offers WHERE index_id >= 2").execute(&self.pool).await?;
        for chunk in (2..2 + n_indices).collect::<Vec<_>>().chunks(5) {
            let mut qb = QueryBuilder::new("INSERT INTO offers (index_id, plan_name, rate, term_months, is_green, fetched_at) ");
            let rows: Vec<Offer> = chunk.iter().flat_map(|i| sample_offers(*i, per_index as usize)).collect();
            qb.push_values(rows, |mut b, o| {
                b.push_bind(o.index_id).push_bind(o.plan_name).push_bind(o.rate).push_bind(o.term_months).push_bind(o.is_green).push_bind(o.fetched_at);
            });
            qb.build().execute(&self.pool).await?;
        }
        sqlx::query("ANALYZE offers").execute(&self.pool).await?;
        Ok(())
    }

    async fn filter(&self, index_id: i32) -> Result<usize> {
        let rows = sqlx::query(
            "SELECT id, plan_name, rate, term_months, is_green, fetched_at FROM offers \
             WHERE index_id = $1 AND rate <= $2 AND term_months >= $3 AND is_green = $4 ORDER BY rate LIMIT 20",
        )
        .bind(index_id).bind(Decimal::new(85_000, 5)).bind(6).bind(true)
        .fetch_all(&self.pool).await?;
        let _: Vec<DateTime<Utc>> = rows.iter().map(|r| r.get(5)).collect();
        Ok(rows.len())
    }

    async fn claim(&self, jobs: i32, workers: usize, variant: &str) -> Result<ClaimReport> {
        sqlx::query("DELETE FROM jobs").execute(&self.pool).await?;
        let mut qb = QueryBuilder::new("INSERT INTO jobs (state) ");
        qb.push_values(0..jobs, |mut b, _| { b.push_bind("queued"); });
        qb.build().execute(&self.pool).await?;
        let single = variant == "single-statement";
        let pool = Arc::new(self.pool.clone());
        let t = Instant::now();
        let mut hs = vec![];
        for w in 0..workers {
            let pool = pool.clone();
            hs.push(tokio::spawn(async move {
                let (mut ids, mut errs, mut samples): (Vec<i64>, usize, Vec<String>) = (vec![], 0usize, vec![]);
                loop {
                    let res: Result<Option<i64>> = async {
                        if single {
                            Ok(sqlx::query_scalar(
                                "UPDATE jobs SET state='done', claimed_by=$1 WHERE id = \
                                 (SELECT id FROM jobs WHERE state='queued' ORDER BY id LIMIT 1 FOR UPDATE SKIP LOCKED) RETURNING id",
                            ).bind(w as i32).fetch_optional(&*pool).await?)
                        } else {
                            let mut tx = pool.begin().await?;
                            let id: Option<i64> = sqlx::query_scalar(
                                "SELECT id FROM jobs WHERE state='queued' ORDER BY id LIMIT 1 FOR UPDATE SKIP LOCKED",
                            ).fetch_optional(&mut *tx).await?;
                            if let Some(id) = id {
                                sqlx::query("UPDATE jobs SET state='done', claimed_by=$1 WHERE id=$2").bind(w as i32).bind(id).execute(&mut *tx).await?;
                            }
                            tx.commit().await?;
                            Ok(id)
                        }
                    }.await;
                    match res { Ok(Some(id)) => ids.push(id), Ok(None) => break, Err(e) => { errs += 1; if samples.len() < 2 { samples.push(err_text(e)); } if errs > 500 { break } } }
                }
                (ids, errs, samples)
            }));
        }
        let mut results = vec![];
        for h in hs { results.push(h.await?); }
        Ok(summarize(results, t.elapsed()))
    }

    async fn probes(&self) -> Vec<Line> {
        let p = &self.pool;
        let mut o = vec![];
        o.push(line("P1 CHECK constraint enforced", {
            let _ = sqlx::query("CREATE TABLE chk (rate NUMERIC(12,5) CHECK (rate >= 0))").execute(p).await;
            ok_or_err(sqlx::query("INSERT INTO chk VALUES (-1)").execute(p).await)
        }));
        o.push(line("P2 native partial unique index (`CREATE UNIQUE INDEX .. WHERE`)", {
            let _ = sqlx::query("CREATE TABLE pu (id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY, user_id INT, active BOOLEAN)").execute(p).await;
            let c = sqlx::query("CREATE UNIQUE INDEX ux ON pu (user_id) WHERE active").execute(p).await;
            let a = sqlx::query("INSERT INTO pu (user_id, active) VALUES (1, true)").execute(p).await;
            let b = sqlx::query("INSERT INTO pu (user_id, active) VALUES (1, true)").execute(p).await;
            let ok3 = sqlx::query("INSERT INTO pu (user_id, active) VALUES (1, false)").execute(p).await;
            format!("create={} first={} duplicate_active={} inactive_dup={}", ok_or_err(c), ok_or_err(a), ok_or_err(b), ok_or_err(ok3))
        }));
        o.push(line("P3 enum type", {
            let _ = sqlx::query("CREATE TYPE ab AS ENUM ('a','b')").execute(p).await;
            let _ = sqlx::query("CREATE TABLE en (k ab NOT NULL)").execute(p).await;
            ok_or_err(sqlx::query("INSERT INTO en VALUES ('zzz')").execute(p).await)
        }));
        o.push(line("P4 JSON column (jsonb)", {
            let c = sqlx::query("CREATE TABLE js (j JSONB)").execute(p).await;
            let ty: Option<String> = sqlx::query_scalar("SELECT data_type FROM information_schema.columns WHERE table_name='js'").fetch_one(p).await.ok();
            let bad = sqlx::query("INSERT INTO js VALUES ('{bad')").execute(p).await;
            let good = sqlx::query("INSERT INTO js VALUES ('{\"a\":{\"b\":5}}')").execute(p).await;
            let got: Option<String> = sqlx::query_scalar("SELECT j #>> '{a,b}' FROM js").fetch_one(p).await.ok();
            let gin = sqlx::query("CREATE INDEX js_gin ON js USING gin (j)").execute(p).await;
            format!("create={} reported_type={:?} invalid_insert={} valid_insert={} path_query={:?} gin_index={}", ok_or_err(c), ty, ok_or_err(bad), ok_or_err(good), got, ok_or_err(gin))
        }));
        for (k, sql) in [
            ("P5a INSERT .. RETURNING", "INSERT INTO chk VALUES (5) RETURNING rate"),
            ("P5b DELETE .. RETURNING", "DELETE FROM chk WHERE rate = 5 RETURNING rate"),
            ("P5c UPDATE .. RETURNING", "UPDATE jobs SET state='x' WHERE id=-1 RETURNING id"),
            ("P5d REPLACE .. RETURNING", "REPLACE INTO chk VALUES (7) RETURNING rate"),
            ("P5e INSERT .. ON CONFLICT DO UPDATE", "INSERT INTO pu (id, user_id, active) OVERRIDING SYSTEM VALUE VALUES (1, 9, false) ON CONFLICT (id) DO UPDATE SET user_id = 9"),
        ] {
            o.push(line(k, ok_or_err(sqlx::query(sql).fetch_all(p).await)));
        }
        o.push(line("P6 LISTEN/NOTIFY", {
            let r: Result<String> = async {
                let mut l = sqlx::postgres::PgListener::connect(&self.db_url).await?;
                l.listen("jobs").await?;
                sqlx::query("NOTIFY jobs, 'hello'").execute(p).await?;
                let n = tokio::time::timeout(Duration::from_secs(5), l.recv()).await??;
                Ok(n.payload().to_string())
            }.await;
            match r { Ok(p) => format!("received payload {p:?}"), Err(e) => err_text(e) }
        }));
        o
    }
}

// ---------------------------------------------------------------- Timescale extra

async fn timescale_check(url: &str) -> Result<Vec<Line>> {
    let admin = sqlx::postgres::PgPoolOptions::new().max_connections(2).connect(url).await?;
    sqlx::query("DROP DATABASE IF EXISTS d2_ts WITH (FORCE)").execute(&admin).await?;
    sqlx::query("CREATE DATABASE d2_ts").execute(&admin).await?;
    let pool = sqlx::postgres::PgPoolOptions::new().max_connections(4)
        .connect(&format!("{}/d2_ts", url.trim_end_matches('/'))).await?;
    let mut out = vec![];
    let v: String = sqlx::query_scalar("SELECT version()").fetch_one(&pool).await?;
    out.push(line("T0 server", v));
    out.push(line("T1 CREATE EXTENSION timescaledb", ok_or_err(sqlx::query("CREATE EXTENSION IF NOT EXISTS timescaledb").execute(&pool).await)));
    let ext: Option<String> = sqlx::query_scalar("SELECT extversion FROM pg_extension WHERE extname='timescaledb'").fetch_optional(&pool).await?;
    out.push(line("T1b extension version", format!("{ext:?}")));
    for t in ["ph_plain", "ph_hyper"] {
        sqlx::query(sqlx::AssertSqlSafe(format!("CREATE TABLE {t} (product_id INT NOT NULL, observed_at TIMESTAMPTZ NOT NULL, price NUMERIC(12,2) NOT NULL)"))).execute(&pool).await?;
    }
    out.push(line("T2 create_hypertable", ok_or_err(sqlx::query("SELECT create_hypertable('ph_hyper', 'observed_at')").fetch_all(&pool).await)));
    for t in ["ph_plain", "ph_hyper"] {
        sqlx::query(sqlx::AssertSqlSafe(format!(
            "INSERT INTO {t} SELECT p, now() - (g || ' hours')::interval, (10 + (p*g) % 90)::numeric FROM generate_series(1,200) p, generate_series(0, 17520, 6) g"
        ))).execute(&pool).await?;
        sqlx::query(sqlx::AssertSqlSafe(format!("CREATE INDEX ON {t} (product_id, observed_at DESC)"))).execute(&pool).await?;
        sqlx::query(sqlx::AssertSqlSafe(format!("ANALYZE {t}"))).execute(&pool).await?;
    }
    let n: i64 = sqlx::query_scalar("SELECT COUNT(*) FROM ph_plain").fetch_one(&pool).await?;
    out.push(line("T3 rows per table (200 products x 2 years every 6h)", n.to_string()));
    for (t, bucket) in [("ph_plain", "date_trunc('day', observed_at)"), ("ph_hyper", "time_bucket('1 day', observed_at)")] {
        let mut times = vec![];
        for i in 0..100 {
            let ts = Instant::now();
            let rows = sqlx::query(sqlx::AssertSqlSafe(format!(
                "SELECT {bucket} d, avg(price) FROM {t} WHERE product_id = $1 AND observed_at > now() - interval '90 days' GROUP BY 1 ORDER BY 1"
            ))).bind(1 + (i % 200)).fetch_all(&pool).await?;
            assert!(!rows.is_empty());
            times.push(ts.elapsed());
        }
        out.push(line(&format!("T4 daily avg, one product, last 90 days, {t} (median of 100)"), ms(median(times))));
    }
    Ok(out)
}

#[tokio::main]
async fn main() -> Result<()> {
    let args: Vec<String> = std::env::args().collect();
    let (kind, url, label) = (args[1].as_str(), args[2].as_str(), args[3].as_str());
    let lines = match kind {
        "mysql" => {
            let r = My::connect(url).await?;
            let out = run_checks(&r, label).await?;
            r.pool.close().await;
            r.admin.close().await;
            out
        }
        "postgres" => {
            let r = Pg::connect(url).await?;
            let out = run_checks(&r, label).await?;
            r.pool.close().await;
            r.admin.close().await;
            out
        }
        "timescale" => timescale_check(url).await?,
        _ => anyhow::bail!("unknown engine"),
    };
    for (k, v) in lines {
        println!("{k}\n    {v}");
    }
    Ok(())
}
