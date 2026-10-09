# Red Team Round 4 — Slapground.Core composition
**Date:** 2026-10-09  **Method:** Abductive Engineering (A-D-I) + Red-Team Auditing, round 3 of the escalation ladder (emergent/architectural) — round 1 (local defects) and round 3 (invariants) are both exhausted for now; this round asks whether modules that are each individually correct compose into something that still is.
**Scope:** The integration point between `ChainTracker`, `HighlightWindowFinder`, and `SessionResult` as of `97ced60`.
**Base:** `main` @ `97ced60` plus this round's fix. **Reproducible evidence:** `core/Slapground.Core.Tests/SessionResultCompositionTests.cs`.

## R4-1 — SessionResult.Capture let two unrelated data sources be combined silently

**Severity:** medium (data-integrity defect in the summary/scoring path, not a crash) **Epistemic level:** CONFIRMED BY INDUCTION **Bucket:** composition defect (Part 4, "module A's contract and module B's contract are each satisfied, but their combination violates a system invariant")

- **Surprise:** `ChainTracker` and `HighlightWindowFinder` each passed their own red-team rounds (1-3) individually. `SessionResult.Capture(ChainTracker chain, IReadOnlyList<ImpactRecord> impacts, ...)` combined them - but `chain` and `impacts` were two independently-supplied parameters, with no code anywhere checking they described the same event stream.
- **Abduction:** a caller could pass a `ChainTracker` built from one sequence of `RecordImpact` calls and a completely unrelated `impacts` list to the same `Capture` call - a copy-paste mistake, stale data held over from a previous session, or (in a future multi-object scene) another object's impact log - and get back a `SessionResult` that looks perfectly well-formed.
- **Deduction:** feed `chain` events at t=0,1,2 and pass an unrelated single impact at t=500 as `impacts`; `MaxChainLength` should report 3 (from `chain`) while `Highlight` should report the t=500 event (from the unrelated list) - an internally contradictory result, with nothing in the API surface flagging it.
- **Induction:** ran exactly that before changing any production code. Confirmed: `MaxChainLength == 3`, `Highlight.StartTime == 500f`. The inconsistency is real, not hypothetical.
- **Causal chain:**
  ```
  ChainTracker fed events A (t=0,1,2)
      -> chain.MaxChainLength = 3 (correct, re: A)
  Capture() also given impacts list B (t=500, unrelated)
      -> HighlightWindowFinder.Find(B) -> Highlight re: B, not A
  SessionResult now reports stats from A and B as if they were one session
  ```
- **Fix, by construction rather than by convention:** `ChainTracker` now logs every impact it's ever recorded (`ChainTracker.Impacts`, with magnitude added to `RecordImpact`'s signature as an optional parameter), making it the single source of truth for both the chain/CPM stats and the highlight scan's input. `SessionResult.Capture` dropped the separate `impacts` parameter entirely - it now reads `chain.Impacts` directly. The mismatch is no longer expressible, not just discouraged in a doc comment.
- **Regression guard:** `SessionResultCompositionTests.Highlight_AlwaysComesFromTheSameTrackersOwnImpacts_NotAnExternalList` proves the highlight always derives from the tracker's own log.

## Why this is a round-4 (composition) finding, not a round-1 (local) one

Neither `ChainTracker` nor `HighlightWindowFinder` had a bug in isolation - both did exactly what their own, separately-tested contracts promised. The defect only existed at the seam between them, in a third function that assumed (without enforcing) that two independently-passed arguments were correlated. This is exactly the class of fracture round 1's per-function fuzzing (rounds 1-3 in this repo) structurally cannot find, because every round-1/round-3 test exercised each module alone.

## Discarded vectors

| Vector | Result | Why it failed |
|---|---|---|
| GhostPacer/SessionResult cross-contamination (does GhostPacer's timeline need to match a SessionResult's chain?) | Not applicable | GhostPacer takes an explicit, separately-constructed `GhostCheckpoint` timeline by design - it's meant to compare *two different* runs (a past one vs. the live one), so "the two inputs are different sessions" is the intended contract here, not a bug. No fix needed; noted so this isn't re-investigated as if it were the same class of issue. |
