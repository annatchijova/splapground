# Red Team Round 3 — Slapground.Core invariants
**Date:** 2026-10-09  **Method:** Abductive Engineering (A-D-I) + Red-Team Auditing, round 2 of the escalation ladder (invariants) — continuing from round 2's single architectural claim (D1) to the properties the rest of the library implicitly promises.
**Scope:** `core/Slapground.Core` as of `f13914a`. **Base:** `main` @ `f13914a` plus this round's test additions (no production code changed this round — see result below).
**Reproducible evidence:** `BreakageRuleInvariantTests.cs`, `ChainTrackerInvariantTests.cs`, `VelocityBufferOverflowTests.cs`, `SessionClockFloatBoundaryTests.cs`, all in `core/Slapground.Core.Tests/`.

## Honest headline: this round found nothing. That's a result, not a non-event.

Four invariants were fuzzed; all four held on the first run, no fixes were needed. Per the method's own rule (`red-team-auditing`, Part 1: "falsification is a first-class result, not a failure"), this gets written up the same way a round with findings would, not quietly skipped because it's less exciting.

## Hypotheses attacked

### R3-1 — Fragility monotonicity in BreakageRule
**Epistemic level:** CONFIRMED BY INDUCTION (property holds) **Bucket:** invariant check, no defect
- **Abduction:** if `BreakageRule`'s threshold math has a sign error or an edge-case inversion, two objects with different Fragility could disagree with "more fragile breaks at least as often" for some impulse.
- **Induction:** 500 random (fragility-pair, impulse) trials (`Monotonic_HigherFragility_NeverBreaksLessOftenThanLowerFragility`) plus 500 random (fragility, impulse-pair) trials for impulse monotonicity (`Monotonic_HigherImpulse_NeverUnbreaksAFixedObject`). 0 violations in 1000 total trials.

### R3-2 — ChainTracker's Max ≥ Current invariant
**Epistemic level:** CONFIRMED BY INDUCTION **Bucket:** invariant check, no defect
- **Abduction:** an off-by-one in the chain-reset logic could let `CurrentChainLength` exceed the max it should have updated.
- **Induction:** 300 random impact streams (0-60 impacts each, random gaps that sometimes exceed `MaxGapSeconds` and sometimes don't), asserting the invariant after every single impact, not just at the end. 0 violations. Also checked `ChaosPerMinute` never reports more impacts than have actually occurred (100 trials) - holds.

### R3-3 — VelocityBuffer eviction under overflow
**Epistemic level:** CONFIRMED BY INDUCTION **Bucket:** untested-until-now code path, not a defect
- **Surprise, honestly stated:** the eviction loop (`while (samples.Count > BufferSize) samples.Dequeue();`) was written in round 2 and never exercised by any test - round 2's tests all stayed within `BufferSize`. This is exactly the kind of gap the method is supposed to catch before shipping, independent of whether a bug actually turns up.
- **Induction:** confirmed `SampleCount` never exceeds `BufferSize` across 100 overflowing samples; confirmed `GetBufferedVelocity` degrades gracefully (falls back to the oldest retained sample) when `lookbackSeconds` exceeds what the retained window can span, rather than throwing or returning garbage; confirmed `bufferSize: 2` (the documented minimum) works. All held.

### R3-4 — SessionClock float-boundary behavior
**Epistemic level:** CONFIRMED BY INDUCTION **Bucket:** invariant check, no defect
- **Abduction:** accumulated float error across hundreds of small time steps could make `TimeRemainingInPhase` dip slightly negative right at a phase boundary - a real, visible product bug if a HUD ever renders this directly (`"-0.0003s"`).
- **Induction:** 400 steps of `1/90s` each (well past the 4s test session), asserting `TimeRemainingInPhase >= -0.0001` every frame, and that `GetPhase` transitions through exactly `DeskSprint -> DestroyEverything -> Ended`, once each, in order. Both held.

## Discarded (non-exploitable / non-defective) vectors

| Vector | Result | Why it failed |
|---|---|---|
| Fragility monotonicity violation | FALSIFIED | 500 random trials, 0 counterexamples |
| Impulse monotonicity violation | FALSIFIED | 500 random trials, 0 counterexamples |
| ChainTracker Max < Current | FALSIFIED | 300 random streams, checked after every impact |
| ChaosPerMinute over-counting | FALSIFIED | 100 random streams |
| VelocityBuffer overflow corruption | FALSIFIED | eviction cap and graceful-lookback-fallback both held |
| SessionClock negative countdown at boundary | FALSIFIED | 400-step walk through all three phase transitions |

## What this round does and doesn't establish

69/69 tests pass, zero production-code changes this round. This says the four checked invariants hold *for the inputs this fuzzing explored* (seeded, bounded-magnitude randoms) - not a formal proof for all floats. It also doesn't touch the parts of the system that still have no code at all (async ghost trail) or that still can't be verified without Unity/hardware (the spike scripts, U2/U3). Round 1 (local defects) and round 2 (D1's architectural claim) found and fixed real things; this round's honest result is that the rest of the library, as far as fuzzed, doesn't have the same class of problem.
