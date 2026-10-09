# Red Team Round 5 — Slapground.Core composition (time origin)
**Date:** 2026-10-09  **Method:** Abductive Engineering (A-D-I) + Red-Team Auditing, composition/architectural level, continuing from round 4's finding on the same stratum.
**Scope:** The implicit time-origin contract between `SessionClock`, `ChainTracker`, and `GhostPacer` as of `cdfec56`.
**Base:** `main` @ `cdfec56` plus this round's fix. **Reproducible evidence:** `core/Slapground.Core.Tests/GhostPacerTimeOriginCompositionTests.cs` (the finding), `core/Slapground.Core.Tests/SessionClockElapsedSecondsCompositionTests.cs` (the fix's regression guard).

## R5-1 — ChainTracker and GhostPacer share no enforced time origin

**Severity:** medium (silently misleading live gameplay feedback, not a crash) **Epistemic level:** CONFIRMED BY INDUCTION **Bucket:** composition defect, same family as R4-1 (Part 4: "module A's contract and module B's contract are each satisfied, but their combination violates a system invariant") but a different seam.

- **Surprise:** round 4 fixed the `SessionResult.Capture`/`ChainTracker` seam by making one module the source of truth for both pieces of data it combined. `GhostPacer` and `ChainTracker` are a different seam: both take caller-supplied `float` timestamps, and neither has any way to know what clock the other's timestamps are measured on.
- **Abduction:** a ghost's `GhostCheckpoint` timeline recorded during one play session using raw engine time (e.g. `Time.time`, which keeps climbing across the whole app lifetime, not per-session) would have checkpoints in the hundreds or thousands. A live session's `ChainTracker`, fed session-relative elapsed time (correctly reset to 0 at `SessionClock.Start()`), would never reach those raw values during a normal few-minute session. `GhostPacer.GetGhostChainLengthAt` would then always fall through to "before the first checkpoint" and report the ghost at 0 - not an error, a specific, wrong, confident number.
- **Deduction:** construct exactly that scenario - a ghost with checkpoints at raw-time-like values (1001, 1005, 1010) and a live chain queried at session-relative elapsed=8 - and the pacing delta should come back as "player is 6 chain-links ahead," while the real answer (had the bases matched) would be close to even.
- **Induction:** ran it. `GetPacingDelta(8f, 6) == 6` and `GetGhostChainLengthAt(8f) == 0`, confirming the silent-lie path, not a crash and not a hypothetical.
- **Causal chain:**
  ```
  Ghost's checkpoints recorded on raw-engine-time basis (~1000s)
      -> GhostPacer holds them as-is, no origin tagging
  Live ChainTracker fed session-relative elapsed time (~0-10s)
      -> GetGhostChainLengthAt(liveElapsed) finds no checkpoint <= liveElapsed
      -> falls through to the documented "ghost hasn't started yet" default: 0
  Pacing delta now compares a real live chain length against a wrong ghost baseline
      -> reports a confident, wrong "you're way ahead" signal
  ```
- **Fix:** `SessionClock.EffectiveElapsed` (private) became `SessionClock.GetElapsedSeconds(now)` (public) - the one correct conversion from a raw time value to pause-adjusted session-relative elapsed seconds. Documented as the required source for every timestamp fed into both `ChainTracker.RecordImpact` and `GhostCheckpoint.Time`, in both classes' own doc comments, so the contract is visible from either side of the seam, not just in a report.
- **What this fix is and isn't:** this is a documentation-and-canonical-API fix, not a by-construction one like round 4's - `Slapground.Core` cannot force a Unity-side caller to route every timestamp through `GetElapsedSeconds`, the way round 4 could eliminate its bug class by removing a parameter entirely. What it does do: gives the correct behavior exactly one call site instead of leaving "what does elapsed even mean here" ambiguous, and the regression test (`SessionClockElapsedSecondsCompositionTests`) proves that when both sides actually use it, two sessions starting at wildly different raw times (1000 vs. 50000) still compare correctly.

## Why this is logged separately from R4-1 rather than merged into it

Same family (a seam between two individually-correct modules), different mechanism: R4-1 was "two parameters to one function should have been one source of truth" and was fixable by removing a parameter. R5-1 is "two independent call sites across the whole application need to agree on a convention with no shared type enforcing it" - the kind of cross-cutting contract that can only be narrowed, not eliminated, from inside a single class library with no control over its callers.

## Discarded vectors

| Vector | Result | Why it failed |
|---|---|---|
| PhysicalProperties/OfficeObjectCatalog values vs. BreakageRule.ReferenceImpulse override | Not pursued | The override parameter is an explicit, documented caller choice; no implicit shared-assumption risk found |
| SessionResult.ElapsedSeconds vs. ChainTracker's actual last impact time | Noted, not fixed this round | A caller could pass an ElapsedSeconds inconsistent with the chain's own timeline, but the blast radius is confined to that one field (MaxChainLength/Highlight stay correct, derived only from chain.Impacts) - lower severity than R4-1/R5-1, logged here so it isn't forgotten rather than fixed speculatively before it's shown to matter |
