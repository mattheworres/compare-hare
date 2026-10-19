//! CompareHare backend entry point.

/// Version line printed by the binary.
fn version_line() -> String {
    format!("{} {}", env!("CARGO_PKG_NAME"), env!("CARGO_PKG_VERSION"))
}

fn main() {
    println!("{}", version_line());
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn version_line_names_package_and_version() {
        let line = version_line();
        assert!(line.starts_with("comparehare "));
        assert!(line.ends_with(env!("CARGO_PKG_VERSION")));
    }
}
