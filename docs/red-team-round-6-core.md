# Red Team Round 6 — Slapground.Core composition (closing a deferred item)
**Date:** 2026-10-09  **Method:** Abductive Engineering (A-D-I) + Red-Team Auditing, composition level, closing the one deferred item from round 5 plus a fresh survey of the full public API surface for any other seam of the same family.
**Scope:** `SessionResult.Capture`'s `elapsedSeconds` parameter vs. `ChainTracker.Impacts`, as of `96f412c`; a full re-read of every public member in `core/Slapground.Core/*.cs` for comparable seams.
**Base:** `main` @ `96f412c` plus this round's fix. **Reproducible evidence:** `core/Slapground.Core.Tests/SessionResultElapsedSecondsCompositionTests.cs`.

## R6-1 — SessionResult.Capture didn't check elapsedSeconds against the chain's own timeline

**Severity:** low-medium (one field could misreport; `MaxChainLength`/`Highlight` stayed correct regardless, derived only from `chain.Impacts`) **Epistemic level:** CONFIRMED BY INDUCTION, then FIXED BY CONSTRUCTION **Bucket:** composition defect, same family as R4-1/R5-1, different seam.

- **Background:** flagged as a deferred, lower-severity item at the end of round 5 (`docs/red-team-round-5-core.md`'s discarded-vectors table) rather than fixed speculatively before being shown to matter. This round picked it up and actually tested it.
- **Deduction:** feed `chain` one impact at `t=300`, then call `SessionResult.Capture(chain, elapsedSeconds: 5f)` - a session claiming to have lasted 5 seconds but whose own chain recorded an impact at 300s. Before any fix, nothing should stop this from succeeding and returning a `SessionResult` with a nonsensical `ElapsedSeconds`.
- **Induction:** ran it first. Confirmed - no exception, `ElapsedSeconds` silently stored the wrong value.
- **Fix:** unlike R5-1 (`ChainTracker`/`GhostPacer`, two independent call sites across the whole application with no shared function to route through), this seam **can** be fixed by construction, because `chain` and `elapsedSeconds` are both already parameters of the same `Capture` call. Added a direct check: if `chain.Impacts` is non-empty, `elapsedSeconds` must be `>=` the last recorded impact's time, or `Capture` throws `ArgumentOutOfRangeException` naming both values. The bug class is now caught at the one call site that can see both pieces of data, the same day it was identified rather than left open indefinitely.
- **Why this one was fixable and R5-1 wasn't:** the general lesson, not just this instance - a composition defect is fixable by construction exactly when the inconsistent data arrives at one function that can see both sides at once (R4-1, R6-1). When the two sides are set by independent call sites with no shared chokepoint (R5-1), the best available fix is narrowing to one correct conversion function and documenting the contract, not elimination.

## Survey: any other seam of this family in the current public API?

Re-read every public type/member in `core/Slapground.Core/` looking for another "two values that should correlate but aren't checked against each other." Nothing else rose to the level of a defect:

- `HighlightWindowFinder`'s `windowDurationSeconds` vs. `ChainTracker.MaxGapSeconds` - two independently meaningful tuning parameters (clip length vs. chain-continuity threshold), not a correctness coupling; a design-tuning question for real playtesting, not a bug.
- `PeakChaosPerMinute`'s "only updates when polled" behavior - already identified and tested in round 1 (`ChainTrackerPeakCpmTests.PeakChaosPerMinute_NeverUpdatedWithoutCallingChaosPerMinute`), a documented and intentional limitation, not a new finding.
- `SessionResult.Beats` (final-result comparison) vs. `GhostPacer.GetPacingDelta` (live comparison) - two deliberately distinct concepts per their own doc comments; a possible source of a Unity-side caller's *confusion* about which one to show where, but not a code defect to fix in this library.

## Discarded vectors

| Vector | Result | Why it failed |
|---|---|---|
| elapsedSeconds vs. chain's last impact time | CONFIRMED, then fixed | See R6-1 |
| HighlightWindowFinder windowDuration vs. ChainTracker MaxGapSeconds coupling | Not a defect | Independently meaningful tuning parameters, not a correctness coupling |
| PeakChaosPerMinute poll-dependency | Not new | Already surfaced and tested in round 1 |
| SessionResult.Beats vs. GhostPacer.GetPacingDelta overlap | Not a code defect | Distinct concepts by design; a documentation/UX-clarity concern at most |
