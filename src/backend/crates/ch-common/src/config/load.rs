//! The layered loader: defaults → `default.toml` → `{COMPAREHARE_ENV}.toml` → environment.

use std::path::{Path, PathBuf};

use figment::Figment;
use figment::providers::{Format, Serialized, Toml};
use figment::value::Value;

use super::{AppConfig, ConfigError, SECRET_KEYS, STRING_KEYS};

const ENV_PREFIX: &str = "COMPAREHARE__";
const ENV_NAME_VAR: &str = "COMPAREHARE_ENV";
const CONFIG_DIR_VAR: &str = "COMPAREHARE_CONFIG_DIR";
const DEFAULT_CONFIG_DIR: &str = "config";

impl AppConfig {
    /// Loads configuration from the process environment and the config directory
    /// (`COMPAREHARE_CONFIG_DIR`, default `./config`), then validates it.
    pub fn load() -> Result<Self, ConfigError> {
        let vars: Vec<(String, String)> = std::env::vars().collect();
        Self::load_with(&vars)
    }

    /// Like [`AppConfig::load`] but reads variables from `vars` instead of the process
    /// environment, so tests need not mutate global state.
    pub fn load_with(vars: &[(String, String)]) -> Result<Self, ConfigError> {
        let lookup = |name: &str| {
            vars.iter()
                .find(|(key, _)| key == name)
                .map(|(_, value)| value.as_str())
        };
        let dir =
            lookup(CONFIG_DIR_VAR).map_or_else(|| PathBuf::from(DEFAULT_CONFIG_DIR), PathBuf::from);
        let env_name = lookup(ENV_NAME_VAR).filter(|name| !name.is_empty());
        Self::load_from(&dir, env_name, vars)
    }

    fn load_from(
        dir: &Path,
        env_name: Option<&str>,
        vars: &[(String, String)],
    ) -> Result<Self, ConfigError> {
        let mut files = Figment::from(Serialized::defaults(Self::built_in()?))
            .merge(Toml::file(dir.join("default.toml")));
        if let Some(name) = env_name {
            if !name
                .chars()
                .all(|c| c.is_ascii_alphanumeric() || c == '-' || c == '_')
            {
                return Err(ConfigError::InvalidEnvName(name.to_owned()));
            }
            files = files.merge(Toml::file(dir.join(format!("{name}.toml"))));
        }

        for (key, env_var) in SECRET_KEYS {
            if files.contains(key) {
                return Err(ConfigError::SecretInFile {
                    key,
                    env_var: env_var.to_owned(),
                });
            }
        }

        let mut figment = files;
        for (key, value) in env_overrides(vars) {
            figment = figment.merge(Serialized::default(&key, value));
        }

        let config: Self = figment.extract()?;
        config.validate()?;
        Ok(config)
    }
}

/// Turns `COMPAREHARE__SECTION__KEY=value` variables into `(section.key, value)` pairs. Values
/// are type-guessed like figment's own `Env` provider (numbers, booleans, `[..]` arrays) except
/// for [`STRING_KEYS`], which stay strings.
fn env_overrides(vars: &[(String, String)]) -> Vec<(String, Value)> {
    vars.iter()
        .filter_map(|(name, raw)| {
            let path = name
                .strip_prefix(ENV_PREFIX)?
                .to_ascii_lowercase()
                .replace("__", ".");
            if path.split('.').any(str::is_empty) {
                return None;
            }
            let value = if STRING_KEYS.contains(&path.as_str()) {
                Value::from(raw.clone())
            } else {
                let Ok(guessed) = raw.parse::<Value>();
                guessed
            };
            Some((path, value))
        })
        .collect()
}

#[cfg(test)]
mod tests {
    use std::fs;
    use std::sync::atomic::{AtomicU32, Ordering};

    use secrecy::ExposeSecret;

    use super::*;
    use crate::config::{LogFormat, LogLevel};

    /// A scratch config directory under the system temp dir, removed on drop.
    struct ConfigDir(PathBuf);

    impl ConfigDir {
        fn new() -> Self {
            static NEXT: AtomicU32 = AtomicU32::new(0);
            let path = std::env::temp_dir().join(format!(
                "comparehare-config-test-{}-{}",
                std::process::id(),
                NEXT.fetch_add(1, Ordering::Relaxed)
            ));
            fs::create_dir_all(&path).unwrap();
            Self(path)
        }

        fn write(&self, file: &str, contents: &str) -> &Self {
            fs::write(self.0.join(file), contents).unwrap();
            self
        }

        fn load(
            &self,
            env_name: Option<&str>,
            vars: &[(&str, &str)],
        ) -> Result<AppConfig, ConfigError> {
            AppConfig::load_from(&self.0, env_name, &owned(vars))
        }
    }

    impl Drop for ConfigDir {
        fn drop(&mut self) {
            let _ = fs::remove_dir_all(&self.0);
        }
    }

    fn owned(vars: &[(&str, &str)]) -> Vec<(String, String)> {
        vars.iter()
            .map(|(k, v)| ((*k).to_owned(), (*v).to_owned()))
            .collect()
    }

    fn expect_err(result: Result<AppConfig, ConfigError>) -> String {
        result.unwrap_err().to_string()
    }

    #[test]
    fn built_in_defaults_apply_without_files() {
        let dir = ConfigDir::new();
        let config = dir.load(None, &[]).unwrap();
        assert_eq!(config.database.pool_size, 10);
        assert_eq!(config.jobs.request_throttle.offer_refresh_secs, 15);
        assert_eq!(config.jobs.request_throttle.price_scraping_secs, 2);
        assert_eq!(config.jobs.timezone, chrono_tz::Tz::America__New_York);
        assert_eq!(config.log.format, LogFormat::Json);
    }

    #[test]
    fn committed_default_toml_matches_built_in_defaults() {
        let committed = Path::new(env!("CARGO_MANIFEST_DIR")).join("../../config");
        let from_file = AppConfig::load_from(&committed, None, &[]).unwrap();
        let empty = ConfigDir::new();
        let built_in = empty.load(None, &[]).unwrap();
        assert_eq!(format!("{from_file:?}"), format!("{built_in:?}"));
    }

    #[test]
    fn committed_local_example_loads() {
        let committed = Path::new(env!("CARGO_MANIFEST_DIR")).join("../../config");
        let dir = ConfigDir::new();
        let example = fs::read_to_string(committed.join("local.toml.example")).unwrap();
        dir.write("local.toml", &example);
        let config = dir.load(Some("local"), &[]).unwrap();
        assert_eq!(config.log.level, LogLevel::Debug);
    }

    #[test]
    fn default_file_overrides_built_in_defaults() {
        let dir = ConfigDir::new();
        dir.write("default.toml", "[database]\npool_size = 3\n");
        let config = dir.load(None, &[]).unwrap();
        assert_eq!(config.database.pool_size, 3);
        // Untouched keys in the same section keep their defaults (deep merge).
        assert_eq!(config.database.url.host_str(), Some("localhost"));
    }

    #[test]
    fn environment_file_overrides_default_file() {
        let dir = ConfigDir::new();
        dir.write(
            "default.toml",
            "[database]\npool_size = 3\n[log]\nlevel = \"warn\"\n",
        )
        .write("staging.toml", "[database]\npool_size = 7\n");
        let config = dir.load(Some("staging"), &[]).unwrap();
        assert_eq!(config.database.pool_size, 7);
        assert_eq!(config.log.level, LogLevel::Warn);
    }

    #[test]
    fn missing_environment_file_is_ignored() {
        let dir = ConfigDir::new();
        assert!(dir.load(Some("nonexistent"), &[]).is_ok());
    }

    #[test]
    fn environment_variables_override_every_file_layer() {
        let dir = ConfigDir::new();
        dir.write("default.toml", "[database]\npool_size = 3\n")
            .write("staging.toml", "[database]\npool_size = 7\n");
        let config = dir
            .load(
                Some("staging"),
                &[
                    ("COMPAREHARE__DATABASE__POOL_SIZE", "42"),
                    ("COMPAREHARE__JOBS__RUN_ON_STARTUP", "true"),
                    ("COMPAREHARE__JOBS__SCHEDULES__PRICE_SCRAPING", "30 4 * * *"),
                    (
                        "COMPAREHARE__HTTP__CORS_ALLOWED_ORIGINS",
                        "[\"https://a.example\", \"https://b.example\"]",
                    ),
                    ("COMPAREHARE__LOG__FILTERS__SQLX", "warn"),
                ],
            )
            .unwrap();
        assert_eq!(config.database.pool_size, 42);
        assert!(config.jobs.run_on_startup);
        assert_eq!(config.jobs.schedules.price_scraping.as_str(), "30 4 * * *");
        assert_eq!(config.http.cors_allowed_origins.len(), 2);
        assert_eq!(config.log.filters.get("sqlx"), Some(&LogLevel::Warn));
    }

    #[test]
    fn config_dir_and_env_name_come_from_variables() {
        let dir = ConfigDir::new();
        dir.write("qa.toml", "[log]\nlevel = \"trace\"\n");
        let dir_path = dir.0.to_str().unwrap().to_owned();
        let vars = owned(&[
            ("COMPAREHARE_CONFIG_DIR", &dir_path),
            ("COMPAREHARE_ENV", "qa"),
        ]);
        let config = AppConfig::load_with(&vars).unwrap();
        assert_eq!(config.log.level, LogLevel::Trace);
    }

    #[test]
    fn unrelated_and_malformed_variables_are_ignored() {
        let dir = ConfigDir::new();
        let config = dir
            .load(
                None,
                &[
                    ("PATH", "/bin"),
                    ("COMPAREHARE_ENV", "x"),
                    ("COMPAREHARE____", "1"),
                    ("COMPAREHARE__", "1"),
                ],
            )
            .unwrap();
        assert_eq!(config.database.pool_size, 10);
    }

    #[test]
    fn rejects_env_name_that_escapes_the_config_dir() {
        let dir = ConfigDir::new();
        assert!(matches!(
            dir.load(Some("../secrets"), &[]),
            Err(ConfigError::InvalidEnvName(_))
        ));
    }

    #[test]
    fn secrets_are_read_from_the_environment_as_strings() {
        let dir = ConfigDir::new();
        let config = dir
            .load(
                None,
                &[
                    ("COMPAREHARE__DATABASE__PASSWORD", "12345"),
                    ("COMPAREHARE__REDIS__PASSWORD", "1.5"),
                    ("COMPAREHARE__SMTP__PASSWORD", "true"),
                    ("COMPAREHARE__SMTP__USERNAME", "007"),
                ],
            )
            .unwrap();
        assert_eq!(
            config.database.password.as_ref().unwrap().expose_secret(),
            "12345"
        );
        assert_eq!(
            config.redis.password.as_ref().unwrap().expose_secret(),
            "1.5"
        );
        assert_eq!(
            config.smtp.password.as_ref().unwrap().expose_secret(),
            "true"
        );
        assert_eq!(config.smtp.username.as_deref(), Some("007"));
    }

    #[test]
    fn debug_output_never_shows_secrets() {
        let dir = ConfigDir::new();
        let config = dir
            .load(
                None,
                &[
                    ("COMPAREHARE__DATABASE__PASSWORD", "hunter2-db"),
                    ("COMPAREHARE__REDIS__PASSWORD", "hunter2-redis"),
                    ("COMPAREHARE__SMTP__PASSWORD", "hunter2-smtp"),
                ],
            )
            .unwrap();
        let debug = format!("{config:?}");
        assert!(!debug.contains("hunter2"), "secret leaked: {debug}");
        assert!(debug.contains("REDACTED"));
    }

    #[test]
    fn secret_in_a_config_file_is_rejected() {
        let dir = ConfigDir::new();
        dir.write("default.toml", "[database]\npassword = \"oops\"\n");
        match dir.load(None, &[]) {
            Err(ConfigError::SecretInFile { key, .. }) => assert_eq!(key, "database.password"),
            other => panic!("expected SecretInFile, got {other:?}"),
        }

        let dir = ConfigDir::new();
        dir.write("default.toml", "")
            .write("prod.toml", "[smtp]\npassword = \"oops\"\n");
        assert!(matches!(
            dir.load(Some("prod"), &[]),
            Err(ConfigError::SecretInFile {
                key: "smtp.password",
                ..
            })
        ));

        let dir = ConfigDir::new();
        dir.write("default.toml", "[redis]\npassword = \"oops\"\n");
        assert!(matches!(
            dir.load(None, &[]),
            Err(ConfigError::SecretInFile {
                key: "redis.password",
                ..
            })
        ));
    }

    #[test]
    fn wrong_type_names_the_key() {
        let dir = ConfigDir::new();
        let message = expect_err(dir.load(None, &[("COMPAREHARE__DATABASE__POOL_SIZE", "lots")]));
        assert!(message.contains("pool_size"), "{message}");
    }

    #[test]
    fn unknown_keys_are_rejected() {
        let dir = ConfigDir::new();
        dir.write("default.toml", "[database]\npool_sise = 3\n");
        let message = expect_err(dir.load(None, &[]));
        assert!(message.contains("pool_sise"), "{message}");
    }

    #[test]
    fn malformed_toml_is_an_error() {
        let dir = ConfigDir::new();
        dir.write("default.toml", "[database\n");
        assert!(matches!(dir.load(None, &[]), Err(ConfigError::Load(_))));
    }

    #[test]
    fn invalid_cron_names_the_key() {
        let dir = ConfigDir::new();
        let message = expect_err(dir.load(
            None,
            &[("COMPAREHARE__JOBS__SCHEDULES__OFFER_REFRESH", "every day")],
        ));
        assert!(message.contains("offer_refresh"), "{message}");
    }

    #[test]
    fn invalid_url_names_the_key() {
        let dir = ConfigDir::new();
        let message = expect_err(dir.load(None, &[("COMPAREHARE__REDIS__URL", "not a url")]));
        assert!(message.contains("redis"), "{message}");
    }

    #[test]
    fn invalid_timezone_is_rejected() {
        let dir = ConfigDir::new();
        let message =
            expect_err(dir.load(None, &[("COMPAREHARE__JOBS__TIMEZONE", "Mars/Olympus")]));
        assert!(message.contains("timezone"), "{message}");
    }

    #[test]
    fn out_of_range_port_is_rejected() {
        let dir = ConfigDir::new();
        let message = expect_err(dir.load(None, &[("COMPAREHARE__SMTP__PORT", "70000")]));
        assert!(message.contains("port"), "{message}");
        assert!(matches!(
            dir.load(None, &[("COMPAREHARE__SMTP__PORT", "0")]),
            Err(ConfigError::Invalid {
                key: "smtp.port",
                ..
            })
        ));
        assert!(matches!(
            dir.load(None, &[("COMPAREHARE__HTTP__BIND", "127.0.0.1:0")]),
            Err(ConfigError::Invalid {
                key: "http.bind",
                ..
            })
        ));
    }

    #[test]
    fn password_inside_the_database_url_is_rejected() {
        let dir = ConfigDir::new();
        assert!(matches!(
            dir.load(
                None,
                &[(
                    "COMPAREHARE__DATABASE__URL",
                    "postgres://u:pw@db/comparehare"
                )]
            ),
            Err(ConfigError::Invalid {
                key: "database.url",
                ..
            })
        ));
    }

    #[test]
    fn password_inside_the_redis_url_is_rejected() {
        let dir = ConfigDir::new();
        for url in [
            "redis://:hunter2@cache:6379",
            "redis://user:hunter2@cache:6379",
            "rediss://cache:6379?password=hunter2",
        ] {
            let result = dir.load(None, &[("COMPAREHARE__REDIS__URL", url)]);
            match result {
                Err(ConfigError::Invalid { key, reason }) => {
                    assert_eq!(key, "redis.url", "{url}");
                    assert!(reason.contains("COMPAREHARE__REDIS__PASSWORD"), "{reason}");
                    assert!(!reason.contains("hunter2"), "secret leaked: {reason}");
                }
                other => panic!("expected Invalid for `{url}`, got {other:?}"),
            }
        }
        // A username alone is not a secret.
        assert!(
            dir.load(
                None,
                &[("COMPAREHARE__REDIS__URL", "redis://app@cache:6379")]
            )
            .is_ok()
        );
    }

    #[test]
    fn password_in_the_database_url_query_is_rejected() {
        let dir = ConfigDir::new();
        assert!(matches!(
            dir.load(
                None,
                &[(
                    "COMPAREHARE__DATABASE__URL",
                    "postgres://u@db/comparehare?password=hunter2"
                )]
            ),
            Err(ConfigError::Invalid {
                key: "database.url",
                ..
            })
        ));
    }

    #[test]
    fn unparseable_urls_never_echo_credentials() {
        let dir = ConfigDir::new();
        for (var, value, key) in [
            (
                "COMPAREHARE__DATABASE__URL",
                "postgres://u:hunter2@db:notaport/x",
                "database.url",
            ),
            (
                "COMPAREHARE__REDIS__URL",
                "redis://:hunter2@cache:notaport",
                "redis.url",
            ),
            (
                "COMPAREHARE__URLS__WEB_APP",
                "https://u:hunter2@app:notaport",
                "urls.web_app",
            ),
            (
                "COMPAREHARE__URLS__API_BASE",
                "https://u:hunter2@api:notaport",
                "urls.api_base",
            ),
            (
                "COMPAREHARE__HTTP__CORS_ALLOWED_ORIGINS",
                "[\"https://u:hunter2@web:notaport\"]",
                "cors_allowed_origins",
            ),
        ] {
            let message = expect_err(dir.load(None, &[(var, value)]));
            assert!(message.contains(key), "`{key}` not named: {message}");
            assert!(!message.contains("hunter2"), "secret leaked: {message}");
        }
    }

    #[test]
    fn cors_origin_with_credentials_is_rejected_without_echoing_them() {
        let dir = ConfigDir::new();
        let message = expect_err(dir.load(
            None,
            &[(
                "COMPAREHARE__HTTP__CORS_ALLOWED_ORIGINS",
                "[\"https://u:hunter2@web.example\"]",
            )],
        ));
        assert!(message.contains("http.cors_allowed_origins"), "{message}");
        assert!(message.contains("https://web.example"), "{message}");
        assert!(!message.contains("hunter2"), "secret leaked: {message}");
    }

    #[test]
    fn wrong_url_scheme_is_rejected() {
        let dir = ConfigDir::new();
        assert!(matches!(
            dir.load(
                None,
                &[("COMPAREHARE__DATABASE__URL", "mysql://u@db/comparehare")]
            ),
            Err(ConfigError::Invalid {
                key: "database.url",
                ..
            })
        ));
        assert!(matches!(
            dir.load(None, &[("COMPAREHARE__URLS__WEB_APP", "ftp://example.com")]),
            Err(ConfigError::Invalid {
                key: "urls.web_app",
                ..
            })
        ));
    }

    #[test]
    fn cors_origin_with_a_path_is_rejected() {
        let dir = ConfigDir::new();
        assert!(matches!(
            dir.load(
                None,
                &[(
                    "COMPAREHARE__HTTP__CORS_ALLOWED_ORIGINS",
                    "[\"https://a.example/app\"]"
                )]
            ),
            Err(ConfigError::Invalid {
                key: "http.cors_allowed_origins",
                ..
            })
        ));
    }

    #[test]
    fn zero_pool_size_and_concurrency_are_rejected() {
        let dir = ConfigDir::new();
        assert!(matches!(
            dir.load(None, &[("COMPAREHARE__DATABASE__POOL_SIZE", "0")]),
            Err(ConfigError::Invalid {
                key: "database.pool_size",
                ..
            })
        ));
        assert!(matches!(
            dir.load(
                None,
                &[("COMPAREHARE__JOBS__CONCURRENCY__PRICE_SCRAPING", "0")]
            ),
            Err(ConfigError::Invalid {
                key: "jobs.concurrency.price_scraping",
                ..
            })
        ));
    }

    #[test]
    fn empty_smtp_host_and_bad_sender_are_rejected() {
        let dir = ConfigDir::new();
        assert!(matches!(
            dir.load(None, &[("COMPAREHARE__SMTP__HOST", " ")]),
            Err(ConfigError::Invalid {
                key: "smtp.host",
                ..
            })
        ));
        assert!(matches!(
            dir.load(None, &[("COMPAREHARE__SMTP__FROM", "nobody")]),
            Err(ConfigError::Invalid {
                key: "smtp.from",
                ..
            })
        ));
    }
}
