# Red Team Round 1 — Slapground.Core
**Date:** 2026-10-09  **Method:** Abductive Engineering (A-D-I) + Red-Team Auditing
**Scope:** `core/Slapground.Core` as of commit `7f98c51` (PhysicalProperties, BreakageRule, MaterialMapper, ChainTracker, SessionClock, OfficeObjectCatalog, HighlightWindowFinder). Not in scope: the Unity-side spike scripts (`unity/Assets/SlapgroundSpike/`) - no Editor exists to run them, so nothing there can be CONFIRMED by induction yet, only read.
**Base:** `main` @ `7f98c51`. **Reproducible evidence:** the test files listed per finding below, all in `core/Slapground.Core.Tests/`, runnable with `cd core && ~/.dotnet/dotnet test`.

This isn't a security audit - there's no attacker, no trust boundary, no sealed decision path (per `llm-out-of-the-loop`/`deterministic-core`, those apply to consequential-output systems like VIGIA, not a game's physics tuning). Applying the method anyway because the discipline transfers: don't assert a bug exists, demonstrate it; don't assert code is correct, fuzz it and show the fuzzing ran.

## Epistemic legend
CODE FACT - PLAUSIBLE HYPOTHESIS - CONFIRMED BY INDUCTION - FALSIFIED

## Findings

### R1-1 — ChaosPerMinute silently miscounts when queried with a stale `now`
**Severity:** low (game-score correctness, not safety/security) **Epistemic level:** CONFIRMED BY INDUCTION **Bucket:** vulnerability (correctness defect)
- **Surprise:** `RecordImpact` enforces non-decreasing timestamps; `ChaosPerMinute` took no such guard on its own `now` parameter, despite reading the same `lastImpactTime`-ordered stream.
- **Abduction:** a caller passing an out-of-order `now` (e.g. a UI poll racing a replay seek) would get a count computed as if time had gone backward, silently.
- **Deduction:** `ChaosPerMinute(50f)` called after `RecordImpact(100f)` should either throw or be specified to mean something coherent; nothing in the pre-fix code enforced either.
- **Induction:** `ChainTrackerRedTeamTests.ChaosPerMinute_WithStaleNow_ThrowsInsteadOfSilentlyMiscounting` failed against the pre-fix code ("No exception was thrown"), confirming the gap was real, not hypothetical.
- **Fix:** `ChaosPerMinute` now throws `ArgumentOutOfRangeException` when `now` precedes the most recently recorded impact - same contract `RecordImpact` already had. Test now passes and is kept as a regression guard.

### R1-2 — HighlightWindowFinder's two-pointer scan: suspected but falsified
**Severity:** n/a (no defect found) **Epistemic level:** FALSIFIED **Bucket:** n/a
- **Surprise:** none observed yet - this was a preemptive attack on an algorithm that's easy to get subtly wrong (sliding-window-max-sum with duplicate/tied timestamps).
- **Abduction:** the two-pointer implementation might diverge from a brute-force reference on duplicate timestamps, tied scores, or window-duration boundary cases.
- **Deduction:** if it diverges anywhere, a randomized fuzz test cross-checking against an O(n^2) brute force over hundreds of random streams should find a counterexample.
- **Induction:** `HighlightWindowFinderFuzzTests` ran 300 random streams (1-40 impacts, random durations, random gaps including zero) plus an explicit all-duplicate-timestamp case against the brute-force reference. 0 divergences. **Hypothesis falsified** - the implementation holds under this fuzzing.

### R1-3 — SessionClock had no Pause/Resume, against a named competition Design Guideline
**Severity:** product/UX gap, not a code bug **Epistemic level:** CODE FACT (absence confirmed by reading, then closed) **Bucket:** hygiene / missing feature against a stated requirement
- **Surprise:** the competition's own rules (`BRAINSTORM.md` line 234) require "clean pause/resume" under "Easy to get into and out of" - a scored Design Guideline, the same class of rule D4 already promoted from informal risk to binding constraint. `SessionClock` as shipped had `Start`/`GetPhase`/`TimeRemainingInPhase` and nothing else - no way to freeze the countdown at all.
- **This is not a bug being fixed, it's a gap being closed** - no prior test asserted pause/resume should exist, so there's nothing to "confirm was broken." Logged here anyway because it was found during this same red-team pass and prioritizing product/UX was the explicit instruction for this round.
- **Fix:** `Pause(now)`/`Resume(now)`/`IsPaused` added, tracking accumulated paused duration and subtracting it from elapsed time in `GetPhase`/`TimeRemainingInPhase`. Idempotent pause (calling it twice doesn't restart the pause clock), no-op resume-without-pause, pause-before-start is a no-op rather than an error (a UI might reasonably send a pause signal before a session visually starts).
- **Induction on the fix itself:** `SessionClockPauseResumeTests` covers freeze-while-paused, resume-continues-from-pause-point-not-real-time, double-pause idempotence, resume-without-pause, pause-before-start, pause-across-a-phase-boundary, and multiple pause/resume cycles accumulating correctly. One of these tests (`DoublePause_IsIdempotent...`) initially failed - **but the defect was in the test's own expected value, not the code** (the comment and the arithmetic disagreed); corrected and re-verified. Recorded here because catching your own test bugs via the same induction step is exactly what the method is supposed to do, including against itself.

## Discarded (non-exploitable / non-defective) vectors

| Vector | Result | Why it failed |
|---|---|---|
| HighlightWindowFinder divergence from brute force | FALSIFIED | 300 fuzzed streams + duplicate-timestamp case, 0 mismatches |
| BreakageRule threshold boundary (`>` vs `>=` at exactly the threshold) | Not a defect | Deliberate convention (exactly-at-threshold doesn't break), already covered by `MidFragility_BreaksAboveThresholdNotBelow` |
| ChainTracker unbounded queue growth under a very long session with ChaosPerMinute never called | Deferred, not fixed | Real but low-severity for a 3-minute session's realistic impact count (tens, not thousands); flagged here rather than "fixed" to avoid speculative scalability work with no measured need |

## Product/UX priorities this round surfaced (beyond the one fix made)

Checked `BRAINSTORM.md`'s Design Guidelines against what's implemented. "Clean pause/resume" was the only one with a concrete, buildable gap in the logic layer; the other two don't have a core-logic home yet:
- **"Fast cold start"** - a Unity scene-load/initialization concern, nothing to check or fix in `Slapground.Core`.
- **"Airplane seat test" / two-foot radius (D4)** - already an architectural decision (`README_TECHNICAL.md` D4), not something `Slapground.Core`'s logic can violate or satisfy on its own; it binds spatial/scene design, which doesn't exist yet.
