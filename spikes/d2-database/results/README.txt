Caveats for every file in this directory (added 2026-10-19 after an audit):
- One container per engine on Docker Desktop (macOS VM), small synthetic data, mostly single runs. Valid for comparing engines, not for production numbers.
- SKIP LOCKED two-step on MariaDB: each worker stops after 500 errors (harness cap), so error counts are floors and elapsed times are not like-for-like. The MariaDB two-step time varied from 5.3 s to 12 s across runs.
- The Postgres two-step-read-committed row repeats its default (Postgres defaults to read committed).
- The Postgres restart-to-ready number is not meaningful (see ops.txt).
