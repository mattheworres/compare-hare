//! Errors raised while loading and validating configuration.

use thiserror::Error;

/// Anything that can stop the configuration from loading. Every variant names the offending key
/// (or file) so a misconfigured deployment fails fast with an actionable message.
#[derive(Debug, Error)]
pub enum ConfigError {
    /// A layer could not be read or a value had the wrong type. figment's message includes the
    /// key path and the layer (file or environment variable) it came from.
    #[error("invalid configuration: {0}")]
    Load(Box<figment::Error>),

    /// A value parsed but is not acceptable.
    #[error("invalid value for `{key}`: {reason}")]
    Invalid { key: &'static str, reason: String },

    /// A secret appeared in a config file, where it could end up committed.
    #[error(
        "`{key}` is a secret and must be set with the {env_var} environment variable, not in a config file"
    )]
    SecretInFile { key: &'static str, env_var: String },

    /// `COMPAREHARE_ENV` would escape the config directory or is empty.
    #[error("invalid COMPAREHARE_ENV `{0}`: use only letters, digits, `-` and `_`")]
    InvalidEnvName(String),

    /// A compiled-in default does not parse. This is a bug in the defaults, not in the
    /// deployment's configuration.
    #[error("built-in default for `{key}` is invalid: {reason}")]
    BuiltInDefault { key: &'static str, reason: String },
}

impl From<figment::Error> for ConfigError {
    fn from(error: figment::Error) -> Self {
        Self::Load(Box::new(error))
    }
}
