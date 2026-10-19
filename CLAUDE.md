# Claude guidelines

## Rust Non-Negotiables

For all Rust generated:

1. **Verify before claiming done.** Run `cargo clippy --all-targets --all-features
   -- -D warnings` and `cargo test`. Paste the raw output in your completion
   summary. If either fails, fix it — do not report success.

2. **Do not add dependencies.** If you believe a crate is necessary, stop and
   ask. Do not add it to Cargo.toml on your own initiative.

3. **Search before writing.** Before creating any type, trait, function, or
   module, grep the codebase for something that already does it or could be
   adapted to. Reuse beats invention; preserve backward compatibility.

4. **Tests accompany logic.** Any non-I/O logic path gets a test asserting
   intended behavior, including at least one failure case. A test that passes
   whether or not the logic is correct is not a test.

5. **Model the domain honestly.** Closed sets become enums. Open sets become
   traits. Never a `kind: String` field driving branching — that is an enum
   wearing a disguise.

6. **Control flow reads top-down.** Prefer `match` over `if let` when there are
   two or more structured arms. Prefer `for` over `while` over `loop`.

7. **Borrow before cloning.** Resolve move errors with a borrow, not `.clone()`.
   Never `Box::leak`, never `&'static mut`. Justify any `'static` bound that
   isn't required by the API you're calling.

8. **Mock at the edges.** Network and filesystem boundaries only. If a unit
   test needs in-process mocking, the design is too coupled — extract a trait
   and inject at the boundary instead.

9. **Self-check.** In your completion summary, list items 1–8 with PASS/FAIL
   and one line of evidence each.
