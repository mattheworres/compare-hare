use chrono::{DateTime, Utc};
use rust_decimal::Decimal;

#[tokio::main]
async fn main() -> anyhow::Result<()> {
    let pool = sqlx::PgPool::connect(&std::env::var("DATABASE_URL")?).await?;
    let min = Decimal::new(10_000, 5);
    let rows = sqlx::query!(
        "SELECT id, plan_name, rate, term_months, is_green, fetched_at, note FROM offers WHERE index_id = $1 AND rate >= $2 LIMIT 3",
        1i32, min
    )
    .fetch_all(&pool)
    .await?;
    for r in &rows {
        // types are inferred by the macro; these annotations fail to compile if inference differs
        let _: i64 = r.id;
        let _: &String = &r.plan_name;
        let _: Decimal = r.rate;
        let _: i32 = r.term_months;
        let _: bool = r.is_green;
        let _: DateTime<Utc> = r.fetched_at;
        let _: &Option<String> = &r.note;
    }
    println!("ok, {} rows", rows.len());
    #[cfg(feature = "bad-column")]
    let _ = sqlx::query!("SELECT no_such_column FROM offers").fetch_all(&pool).await?;
    #[cfg(feature = "bad-type")]
    {
        let _: String = sqlx::query!("SELECT rate FROM offers LIMIT 1").fetch_one(&pool).await?.rate;
    }
    Ok(())
}
