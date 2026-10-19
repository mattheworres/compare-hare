#![cfg(test)]
use testcontainers_modules::{mariadb::Mariadb, postgres::Postgres, testcontainers::{ImageExt, runners::AsyncRunner}};

#[tokio::test]
async fn postgres_container() {
    let c = Postgres::default().with_tag("18.6").start().await.expect("start");
    let url = format!("postgres://postgres:postgres@{}:{}/postgres", c.get_host().await.unwrap(), c.get_host_port_ipv4(5432).await.unwrap());
    let pool = sqlx::PgPool::connect(&url).await.unwrap();
    let v: String = sqlx::query_scalar("SELECT version()").fetch_one(&pool).await.unwrap();
    println!("PG: {v}");
}

#[tokio::test]
async fn mariadb_container() {
    let c = Mariadb::default().with_tag("13.0.2").start().await.expect("start");
    let url = format!("mysql://root@{}:{}/test", c.get_host().await.unwrap(), c.get_host_port_ipv4(3306).await.unwrap());
    let pool = sqlx::MySqlPool::connect(&url).await.unwrap();
    let v: String = sqlx::query_scalar("SELECT VERSION()").fetch_one(&pool).await.unwrap();
    println!("MariaDB: {v}");
}
