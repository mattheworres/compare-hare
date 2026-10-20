//! Typed, layered application configuration.
//!
//! Layers, lowest to highest precedence: built-in defaults → `default.toml` → optional
//! `{COMPAREHARE_ENV}.toml` → `COMPAREHARE__SECTION__KEY` environment variables. See
//! [`AppConfig::load`]. Secrets are accepted from the environment only and are wrapped in
//! [`SecretString`], so `Debug` output never shows them.

mod error;
mod load;
mod schedule;

use std::collections::BTreeMap;
use std::net::SocketAddr;

use chrono_tz::Tz;
use secrecy::SecretString;
use serde::de::Error as _;
use serde::{Deserialize, Deserializer, Serialize};
use url::Url;

pub use error::ConfigError;
pub use schedule::{CronSchedule, InvalidCron};

/// Config keys that hold secrets: `(dotted key, environment variable)`. They are rejected in
/// config files and always read as strings from the environment.
pub(crate) const SECRET_KEYS: [(&str, &str); 3] = [
    ("database.password", "COMPAREHARE__DATABASE__PASSWORD"),
    ("redis.password", "COMPAREHARE__REDIS__PASSWORD"),
    ("smtp.password", "COMPAREHARE__SMTP__PASSWORD"),
];

/// Keys that must stay strings even when the value looks numeric or boolean (e.g. a password of
/// `12345`), so the environment layer does not guess a type for them.
pub(crate) const STRING_KEYS: [&str; 4] = [
    "database.password",
    "redis.password",
    "smtp.password",
    "smtp.username",
];

#[derive(Debug, Deserialize, Serialize)]
#[serde(deny_unknown_fields)]
pub struct AppConfig {
    pub database: DatabaseConfig,
    pub redis: RedisConfig,
    pub smtp: SmtpConfig,
    pub urls: UrlsConfig,
    pub http: HttpConfig,
    pub jobs: JobsConfig,
    pub log: LogConfig,
}

#[derive(Debug, Deserialize, Serialize)]
#[serde(deny_unknown_fields)]
pub struct DatabaseConfig {
    /// Connection URL without a password, e.g. `postgres://user@host:5432/db`.
    #[serde(deserialize_with = "deserialize_url")]
    pub url: Url,
    /// Maximum pool connections.
    pub pool_size: u32,
    /// Environment only (`COMPAREHARE__DATABASE__PASSWORD`).
    #[serde(skip_serializing)]
    pub password: Option<SecretString>,
}

impl DatabaseConfig {
    fn built_in() -> Result<Self, ConfigError> {
        Ok(Self {
            url: built_in_url(
                "database.url",
                "postgres://comparehare@localhost:5432/comparehare",
            )?,
            pool_size: 10,
            password: None,
        })
    }
}

#[derive(Debug, Deserialize, Serialize)]
#[serde(deny_unknown_fields)]
pub struct RedisConfig {
    /// Connection URL without a password, e.g. `redis://host:6379`.
    #[serde(deserialize_with = "deserialize_url")]
    pub url: Url,
    /// Environment only (`COMPAREHARE__REDIS__PASSWORD`).
    #[serde(skip_serializing)]
    pub password: Option<SecretString>,
}

impl RedisConfig {
    fn built_in() -> Result<Self, ConfigError> {
        Ok(Self {
            url: built_in_url("redis.url", "redis://localhost:6379")?,
            password: None,
        })
    }
}

#[derive(Clone, Copy, Debug, Default, PartialEq, Eq, Deserialize, Serialize)]
#[serde(rename_all = "snake_case")]
pub enum SmtpTls {
    /// Plain SMTP (local mail sink).
    #[default]
    None,
    /// Upgrade a plain connection with STARTTLS.
    StartTls,
    /// TLS from the first byte (implicit TLS).
    Tls,
}

#[derive(Debug, Deserialize, Serialize)]
#[serde(deny_unknown_fields)]
pub struct SmtpConfig {
    pub host: String,
    pub port: u16,
    pub username: Option<String>,
    /// Environment only (`COMPAREHARE__SMTP__PASSWORD`).
    #[serde(skip_serializing)]
    pub password: Option<SecretString>,
    pub from: String,
    pub tls: SmtpTls,
}

impl Default for SmtpConfig {
    fn default() -> Self {
        Self {
            host: "localhost".to_owned(),
            port: 1025,
            username: None,
            password: None,
            from: "admin@comparehare.com".to_owned(),
            tls: SmtpTls::None,
        }
    }
}

#[derive(Debug, Deserialize, Serialize)]
#[serde(deny_unknown_fields)]
pub struct UrlsConfig {
    /// Public URL of the web app (used in emails and redirects).
    #[serde(deserialize_with = "deserialize_url")]
    pub web_app: Url,
    /// Public base URL of this API.
    #[serde(deserialize_with = "deserialize_url")]
    pub api_base: Url,
}

impl UrlsConfig {
    fn built_in() -> Result<Self, ConfigError> {
        Ok(Self {
            web_app: built_in_url("urls.web_app", "http://localhost:8000")?,
            api_base: built_in_url("urls.api_base", "http://localhost:8080")?,
        })
    }
}

#[derive(Debug, Deserialize, Serialize)]
#[serde(deny_unknown_fields)]
pub struct HttpConfig {
    pub bind: SocketAddr,
    /// Allowed CORS origins, as bare origins (`scheme://host[:port]`).
    #[serde(deserialize_with = "deserialize_urls")]
    pub cors_allowed_origins: Vec<Url>,
}

impl HttpConfig {
    fn built_in() -> Result<Self, ConfigError> {
        Ok(Self {
            bind: SocketAddr::from(([127, 0, 0, 1], 8080)),
            cors_allowed_origins: vec![built_in_url(
                "http.cors_allowed_origins",
                "http://localhost:8000",
            )?],
        })
    }
}

#[derive(Debug, Deserialize, Serialize)]
#[serde(deny_unknown_fields)]
pub struct JobsConfig {
    /// Run every job once at startup (the legacy `RunJobsOnStartup`).
    pub run_on_startup: bool,
    /// Time zone the cron schedules are evaluated in.
    pub timezone: Tz,
    pub schedules: JobSchedules,
    pub concurrency: JobConcurrency,
    pub request_throttle: RequestThrottle,
}

impl JobsConfig {
    fn built_in() -> Result<Self, ConfigError> {
        Ok(Self {
            run_on_startup: false,
            timezone: Tz::America__New_York,
            schedules: JobSchedules::built_in()?,
            concurrency: JobConcurrency::default(),
            request_throttle: RequestThrottle::default(),
        })
    }
}

#[derive(Debug, Deserialize, Serialize)]
#[serde(deny_unknown_fields)]
pub struct JobSchedules {
    pub offer_refresh: CronSchedule,
    pub alert_assessment: CronSchedule,
    pub price_scraping: CronSchedule,
}

impl JobSchedules {
    fn built_in() -> Result<Self, ConfigError> {
        Ok(Self {
            offer_refresh: built_in_cron("jobs.schedules.offer_refresh", "0 0 * * *")?,
            alert_assessment: built_in_cron("jobs.schedules.alert_assessment", "0 1 * * *")?,
            price_scraping: built_in_cron("jobs.schedules.price_scraping", "0 2 * * *")?,
        })
    }
}

/// Maximum simultaneous runs per job kind.
#[derive(Debug, Deserialize, Serialize)]
#[serde(deny_unknown_fields)]
pub struct JobConcurrency {
    pub offer_refresh: u32,
    pub alert_assessment: u32,
    pub price_scraping: u32,
}

impl Default for JobConcurrency {
    fn default() -> Self {
        Self {
            offer_refresh: 1,
            alert_assessment: 1,
            price_scraping: 1,
        }
    }
}

/// Minimum seconds between consecutive requests to one host. Defaults are the values hardcoded
/// in the .NET runners (15s offer loader, 2s price scraper); `per_host_secs` overrides either
/// for a specific host name.
#[derive(Debug, Deserialize, Serialize)]
#[serde(deny_unknown_fields)]
pub struct RequestThrottle {
    pub offer_refresh_secs: u64,
    pub price_scraping_secs: u64,
    pub per_host_secs: BTreeMap<String, u64>,
}

impl Default for RequestThrottle {
    fn default() -> Self {
        Self {
            offer_refresh_secs: 15,
            price_scraping_secs: 2,
            per_host_secs: BTreeMap::new(),
        }
    }
}

#[derive(Clone, Copy, Debug, Default, PartialEq, Eq, Deserialize, Serialize)]
#[serde(rename_all = "snake_case")]
pub enum LogLevel {
    Error,
    Warn,
    #[default]
    Info,
    Debug,
    Trace,
}

#[derive(Clone, Copy, Debug, Default, PartialEq, Eq, Deserialize, Serialize)]
#[serde(rename_all = "snake_case")]
pub enum LogFormat {
    #[default]
    Json,
    Pretty,
}

#[derive(Debug, Default, Deserialize, Serialize)]
#[serde(deny_unknown_fields)]
pub struct LogConfig {
    pub level: LogLevel,
    pub format: LogFormat,
    /// Per-target level overrides, e.g. `sqlx = "warn"`.
    pub filters: BTreeMap<String, LogLevel>,
}

impl AppConfig {
    /// The compiled-in defaults, the lowest configuration layer. They target the local docker
    /// compose services. Fallible only because the URL and cron literals are parsed; a failure
    /// is a bug in the literals and is caught by the tests.
    pub fn built_in() -> Result<Self, ConfigError> {
        Ok(Self {
            database: DatabaseConfig::built_in()?,
            redis: RedisConfig::built_in()?,
            smtp: SmtpConfig::default(),
            urls: UrlsConfig::built_in()?,
            http: HttpConfig::built_in()?,
            jobs: JobsConfig::built_in()?,
            log: LogConfig::default(),
        })
    }

    /// Checks the cross-field rules serde cannot express. Called by [`AppConfig::load`].
    pub fn validate(&self) -> Result<(), ConfigError> {
        let db = &self.database;
        require_scheme("database.url", &db.url, &["postgres", "postgresql"])?;
        reject_embedded_password("database.url", &db.url, "COMPAREHARE__DATABASE__PASSWORD")?;
        if db.pool_size == 0 {
            return invalid("database.pool_size", "must be at least 1");
        }
        require_scheme("redis.url", &self.redis.url, &["redis", "rediss"])?;
        reject_embedded_password("redis.url", &self.redis.url, "COMPAREHARE__REDIS__PASSWORD")?;

        let smtp = &self.smtp;
        if smtp.host.trim().is_empty() {
            return invalid("smtp.host", "must not be empty");
        }
        if smtp.port == 0 {
            return invalid("smtp.port", "must be between 1 and 65535");
        }
        match smtp.from.split_once('@') {
            Some((local, domain)) if !local.is_empty() && !domain.is_empty() => {}
            _ => return invalid("smtp.from", "must be an email address"),
        }

        require_scheme("urls.web_app", &self.urls.web_app, &["http", "https"])?;
        require_scheme("urls.api_base", &self.urls.api_base, &["http", "https"])?;

        if self.http.bind.port() == 0 {
            return invalid("http.bind", "port must be between 1 and 65535");
        }
        for origin in &self.http.cors_allowed_origins {
            require_scheme("http.cors_allowed_origins", origin, &["http", "https"])?;
            let has_credentials = !origin.username().is_empty() || origin.password().is_some();
            if has_credentials
                || origin.path() != "/"
                || origin.query().is_some()
                || origin.fragment().is_some()
            {
                // Echo only the origin part: the rest may carry credentials.
                return invalid(
                    "http.cors_allowed_origins",
                    format!(
                        "`{}` must be a bare origin without credentials, a path, query or fragment",
                        origin.origin().ascii_serialization()
                    ),
                );
            }
        }

        let concurrency = &self.jobs.concurrency;
        for (key, value) in [
            ("jobs.concurrency.offer_refresh", concurrency.offer_refresh),
            (
                "jobs.concurrency.alert_assessment",
                concurrency.alert_assessment,
            ),
            (
                "jobs.concurrency.price_scraping",
                concurrency.price_scraping,
            ),
        ] {
            if value == 0 {
                return invalid(key, "must be at least 1");
            }
        }
        Ok(())
    }
}

fn invalid(key: &'static str, reason: impl Into<String>) -> Result<(), ConfigError> {
    Err(ConfigError::Invalid {
        key,
        reason: reason.into(),
    })
}

fn require_scheme(key: &'static str, url: &Url, schemes: &[&str]) -> Result<(), ConfigError> {
    if !schemes.contains(&url.scheme()) {
        return invalid(
            key,
            format!(
                "scheme `{}` must be one of: {}",
                url.scheme(),
                schemes.join(", ")
            ),
        );
    }
    if url.host_str().is_none() {
        return invalid(key, "must include a host");
    }
    Ok(())
}

/// Rejects a URL that embeds a password (as userinfo or a `password` query parameter). It would
/// show up in `Debug` output and logs, so secrets travel in their own environment variable.
fn reject_embedded_password(
    key: &'static str,
    url: &Url,
    env_var: &str,
) -> Result<(), ConfigError> {
    let in_query = url
        .query_pairs()
        .any(|(name, _)| name.eq_ignore_ascii_case("password"));
    if url.password().is_some() || in_query {
        return invalid(
            key,
            format!("must not contain a password; set {env_var} instead"),
        );
    }
    Ok(())
}

/// Deserializes a URL without echoing the input in the error. `Url`'s own `Deserialize` quotes
/// the whole string, which would print a password-bearing value in the load error.
fn deserialize_url<'de, D: Deserializer<'de>>(deserializer: D) -> Result<Url, D::Error> {
    let text = String::deserialize(deserializer)?;
    Url::parse(&text).map_err(|source| D::Error::custom(format!("not a valid URL: {source}")))
}

/// [`deserialize_url`] for a list; errors name the entry's position instead of quoting it.
fn deserialize_urls<'de, D: Deserializer<'de>>(deserializer: D) -> Result<Vec<Url>, D::Error> {
    let texts = Vec::<String>::deserialize(deserializer)?;
    texts
        .iter()
        .enumerate()
        .map(|(index, text)| {
            Url::parse(text).map_err(|source| {
                D::Error::custom(format!("entry {index} is not a valid URL: {source}"))
            })
        })
        .collect()
}

fn built_in_url(key: &'static str, literal: &str) -> Result<Url, ConfigError> {
    Url::parse(literal).map_err(|source| ConfigError::BuiltInDefault {
        key,
        reason: format!("`{literal}` is not a valid URL: {source}"),
    })
}

fn built_in_cron(key: &'static str, literal: &str) -> Result<CronSchedule, ConfigError> {
    literal
        .parse()
        .map_err(|source: InvalidCron| ConfigError::BuiltInDefault {
            key,
            reason: source.to_string(),
        })
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn built_in_defaults_parse_and_validate() {
        AppConfig::built_in().unwrap().validate().unwrap();
    }

    #[test]
    fn bad_built_in_url_literal_names_the_key() {
        match built_in_url("some.key", "not a url") {
            Err(ConfigError::BuiltInDefault { key, reason }) => {
                assert_eq!(key, "some.key");
                assert!(reason.contains("not a url"), "{reason}");
            }
            other => panic!("expected BuiltInDefault, got {other:?}"),
        }
    }

    #[test]
    fn bad_built_in_cron_literal_names_the_key() {
        assert!(matches!(
            built_in_cron("some.key", "every day"),
            Err(ConfigError::BuiltInDefault {
                key: "some.key",
                ..
            })
        ));
    }

    #[test]
    fn embedded_password_is_rejected_in_userinfo_and_query() {
        let reject = |literal: &str| {
            let url = Url::parse(literal).unwrap();
            reject_embedded_password("some.key", &url, "SOME_VAR")
        };
        assert!(reject("redis://localhost:6379").is_ok());
        assert!(reject("redis://user@localhost:6379").is_ok());
        assert!(matches!(
            reject("redis://user:pw@localhost"),
            Err(ConfigError::Invalid { .. })
        ));
        assert!(matches!(
            reject("redis://:pw@localhost"),
            Err(ConfigError::Invalid { .. })
        ));
        assert!(matches!(
            reject("postgres://u@localhost/db?sslmode=require&Password=pw"),
            Err(ConfigError::Invalid { .. })
        ));
    }
}
