//! Cron schedules, validated when the configuration is deserialized.

use std::str::FromStr;

use serde::{Deserialize, Serialize, Serializer};
use thiserror::Error;

/// A standard five-field cron expression (`minute hour day-of-month month day-of-week`), the
/// format the .NET Hangfire config used. The `cron` crate wants a leading seconds field, so one
/// is prepended internally; the original text is what is stored and serialized.
///
/// Day-of-week follows standard cron (`0`–`7`, with both `0` and `7` meaning Sunday, or
/// `SUN`–`SAT`). The `cron` crate numbers days `1`–`7` from Sunday, so numbers are translated to
/// day names before parsing; passing them through would shift every day by one.
#[derive(Clone, Debug)]
pub struct CronSchedule {
    expression: String,
    schedule: cron::Schedule,
}

#[derive(Debug, Error)]
pub enum InvalidCron {
    #[error("`{0}` must have exactly five fields (minute hour day-of-month month day-of-week)")]
    FieldCount(String),
    #[error("day-of-week `{field}` is not valid: {reason}")]
    DayOfWeek { field: String, reason: String },
    #[error("`{expression}` is not a valid cron expression: {source}")]
    Parse {
        expression: String,
        source: cron::error::Error,
    },
}

/// Day names indexed by standard-cron day number (`0` = Sunday).
const DAY_NAMES: [&str; 7] = ["SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"];

/// Rewrites a standard-cron day-of-week field as a list of day names the `cron` crate reads
/// unambiguously. `*` and `?` are left alone. Supports lists, ranges, `*/n` and `a-b/n`.
fn day_of_week_to_names(field: &str) -> Result<String, String> {
    if field == "*" || field == "?" {
        return Ok(field.to_owned());
    }

    let mut selected = [false; 7];
    for item in field.split(',') {
        let (range, step) = match item.split_once('/') {
            Some((range, step)) => (range, Some(parse_step(step)?)),
            None => (item, None),
        };
        let (first, last) = match (range, range.split_once('-')) {
            ("*", _) => (0, 6),
            (_, Some((start, end))) => (day_number(start, false)?, day_number(end, true)?),
            (single, None) if step.is_none() => {
                let day = day_number(single, false)?;
                (day, day)
            }
            (_, None) => return Err(format!("`{item}`: a step needs `*` or a range")),
        };
        if first > last {
            return Err(format!("`{item}`: range runs backwards"));
        }
        for day in (first..=last).step_by(step.unwrap_or(1)) {
            selected[day % 7] = true;
        }
    }

    let names: Vec<&str> = DAY_NAMES
        .iter()
        .zip(selected)
        .filter_map(|(name, is_selected)| is_selected.then_some(*name))
        .collect();
    Ok(names.join(","))
}

fn parse_step(text: &str) -> Result<usize, String> {
    match parse_digits(text) {
        Some(step) if step > 0 => Ok(step),
        _ => Err(format!("step `{text}` must be a positive number")),
    }
}

/// A day number `0`–`7` or a name (`SUN`–`SAT`, any case). `SUN` ends a range as `7`, so
/// `FRI-SUN` runs forward.
fn day_number(token: &str, is_range_end: bool) -> Result<usize, String> {
    if let Some(number) = parse_digits(token) {
        return match number {
            0..=7 => Ok(number),
            _ => Err(format!("`{token}` is out of range (0-7)")),
        };
    }
    match DAY_NAMES
        .iter()
        .position(|name| name.eq_ignore_ascii_case(token))
    {
        Some(0) if is_range_end => Ok(7),
        Some(index) => Ok(index),
        None => Err(format!("`{token}` is not a day number or name")),
    }
}

/// Digits only: unlike `str::parse`, no leading `+`.
fn parse_digits(text: &str) -> Option<usize> {
    if text.is_empty() || !text.bytes().all(|byte| byte.is_ascii_digit()) {
        return None;
    }
    text.parse().ok()
}

impl CronSchedule {
    pub fn as_str(&self) -> &str {
        &self.expression
    }

    pub fn schedule(&self) -> &cron::Schedule {
        &self.schedule
    }
}

impl FromStr for CronSchedule {
    type Err = InvalidCron;

    fn from_str(expression: &str) -> Result<Self, Self::Err> {
        let expression = expression.trim();
        let fields: Vec<&str> = expression.split_whitespace().collect();
        let [minute, hour, day_of_month, month, day_of_week] = fields[..] else {
            return Err(InvalidCron::FieldCount(expression.to_owned()));
        };
        let day_of_week =
            day_of_week_to_names(day_of_week).map_err(|reason| InvalidCron::DayOfWeek {
                field: day_of_week.to_owned(),
                reason,
            })?;
        let with_seconds = format!("0 {minute} {hour} {day_of_month} {month} {day_of_week}");
        let schedule =
            cron::Schedule::from_str(&with_seconds).map_err(|source| InvalidCron::Parse {
                expression: expression.to_owned(),
                source,
            })?;
        Ok(Self {
            expression: expression.to_owned(),
            schedule,
        })
    }
}

impl TryFrom<String> for CronSchedule {
    type Error = InvalidCron;

    fn try_from(value: String) -> Result<Self, Self::Error> {
        value.parse()
    }
}

impl PartialEq for CronSchedule {
    fn eq(&self, other: &Self) -> bool {
        self.expression == other.expression
    }
}

impl Eq for CronSchedule {}

impl Serialize for CronSchedule {
    fn serialize<S: Serializer>(&self, serializer: S) -> Result<S::Ok, S::Error> {
        serializer.serialize_str(&self.expression)
    }
}

impl<'de> Deserialize<'de> for CronSchedule {
    fn deserialize<D: serde::Deserializer<'de>>(deserializer: D) -> Result<Self, D::Error> {
        let text = String::deserialize(deserializer)?;
        text.parse().map_err(serde::de::Error::custom)
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn parses_five_field_expression() {
        let schedule: CronSchedule = "0 2 * * *".parse().unwrap();
        assert_eq!(schedule.as_str(), "0 2 * * *");
        assert!(
            schedule
                .schedule()
                .upcoming(chrono_tz::UTC)
                .next()
                .is_some()
        );
    }

    #[test]
    fn rejects_wrong_field_count() {
        assert!(matches!(
            "0 0 0 2 * * *".parse::<CronSchedule>(),
            Err(InvalidCron::FieldCount(_))
        ));
        assert!(matches!(
            "".parse::<CronSchedule>(),
            Err(InvalidCron::FieldCount(_))
        ));
    }

    #[test]
    fn rejects_out_of_range_field() {
        assert!(matches!(
            "61 0 * * *".parse::<CronSchedule>(),
            Err(InvalidCron::Parse { .. })
        ));
    }

    /// The weekdays (`Mon`, `Tue`, ...) the schedule fires on, from its next 14 runs. These
    /// schedules fire once a day at most, so 14 runs cover every weekday it can hit.
    fn weekdays(expression: &str) -> Vec<String> {
        let schedule: CronSchedule = expression.parse().unwrap();
        let mut days: Vec<String> = schedule
            .schedule()
            .upcoming(chrono_tz::UTC)
            .take(14)
            .map(|run| run.format("%a").to_string())
            .collect();
        days.sort();
        days.dedup();
        days
    }

    fn sorted(days: &[&str]) -> Vec<String> {
        let mut days: Vec<String> = days.iter().map(|day| (*day).to_owned()).collect();
        days.sort();
        days
    }

    #[test]
    fn numeric_day_of_week_follows_standard_cron() {
        // Passed straight to the `cron` crate, `1` would be Sunday and `0` would be an error.
        assert_eq!(weekdays("0 3 * * 1"), sorted(&["Mon"]));
        assert_eq!(weekdays("0 3 * * 6"), sorted(&["Sat"]));
        assert_eq!(weekdays("0 3 * * 0"), sorted(&["Sun"]));
        assert_eq!(weekdays("0 3 * * 7"), sorted(&["Sun"]));
    }

    #[test]
    fn day_of_week_lists_ranges_and_steps() {
        assert_eq!(
            weekdays("0 3 * * 1-5"),
            sorted(&["Mon", "Tue", "Wed", "Thu", "Fri"])
        );
        assert_eq!(weekdays("0 3 * * 1,3"), sorted(&["Mon", "Wed"]));
        assert_eq!(weekdays("0 3 * * 5-7"), sorted(&["Fri", "Sat", "Sun"]));
        assert_eq!(weekdays("0 3 * * 1-5/2"), sorted(&["Mon", "Wed", "Fri"]));
        assert_eq!(
            weekdays("0 3 * * */2"),
            sorted(&["Sun", "Tue", "Thu", "Sat"])
        );
        assert_eq!(weekdays("0 3 * * 0-6").len(), 7);
    }

    #[test]
    fn day_of_week_names_work_in_any_case() {
        assert_eq!(weekdays("0 3 * * MON-FRI").len(), 5);
        assert_eq!(weekdays("0 3 * * mon,wed"), sorted(&["Mon", "Wed"]));
        // SUN ends a range as day 7, so a range that finishes on Sunday is not backwards.
        assert_eq!(weekdays("0 3 * * fri-sun"), sorted(&["Fri", "Sat", "Sun"]));
    }

    #[test]
    fn wildcard_day_of_week_is_every_day() {
        assert_eq!(weekdays("0 3 * * *").len(), 7);
        assert_eq!(weekdays("0 3 * * ?").len(), 7);
    }

    #[test]
    fn rejects_invalid_day_of_week() {
        for field in [
            "8", "5-1", "1/2", "*/0", "MON-", "FUNDAY", "1,,2", "+1", "-1",
        ] {
            let expression = format!("0 3 * * {field}");
            assert!(
                matches!(
                    expression.parse::<CronSchedule>(),
                    Err(InvalidCron::DayOfWeek { .. })
                ),
                "`{expression}` should be rejected as a day-of-week error"
            );
        }
    }

    #[test]
    fn day_of_week_error_names_the_field() {
        let message = "0 3 * * 9".parse::<CronSchedule>().unwrap_err().to_string();
        assert!(message.contains("day-of-week `9`"), "{message}");
    }

    #[test]
    fn original_text_is_kept_after_translation() {
        let schedule: CronSchedule = "0 3 * * 1-5".parse().unwrap();
        assert_eq!(schedule.as_str(), "0 3 * * 1-5");
    }
}
