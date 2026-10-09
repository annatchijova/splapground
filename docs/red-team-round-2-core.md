# Red Team Round 2 — D1's actual premise
**Date:** 2026-10-09  **Method:** Abductive Engineering (A-D-I) + Red-Team Auditing, round 2 (invariants) per the house escalation ladder — round 1 (local defects, `docs/red-team-round-1-core.md`) is exhausted for now, this round asks whether a named architectural decision's own justification actually holds, not just whether individual functions are bug-free.
**Scope:** D1 ("impact detection uses a windowed/buffered estimator, not single-frame contact") as implemented in the new `core/Slapground.Core/VelocityBuffer.cs` — a verified port of the previously-unverified `unity/Assets/SlapgroundSpike/Scripts/HandVelocityBuffer.cs`.
**Base:** `main` @ `9c47fab` plus this round's changes. **Reproducible evidence:** `core/Slapground.Core.Tests/VelocityBufferTests.cs`, runnable with `cd core && ~/.dotnet/dotnet test --filter VelocityBufferTests`.

## The question, stated as a falsifiable prediction

D1's whole justification (`README_TECHNICAL.md` section 2, citing Table Troopers in section 5) is: a single-frame position estimate is untrustworthy at the exact moment of contact, so anchor the velocity estimate on a sample from further back instead. That's an architectural *claim*, not yet a measured one anywhere in this repo until now.

**Abduction:** if the claim is true, a synthetic tracking glitch landing exactly on the queried frame should produce a smaller velocity error under the buffered estimator than under the naive last-two-samples estimator.

**Deduction, stated before running anything:** for a hand swinging at a realistic 2 m/s, sampled at 90Hz, with a single 15cm positional glitch on the very last (collision) frame, the buffered estimator (50ms lookback) should show meaningfully less error than the instantaneous one.

## Induction — actually run, not assumed

`SingleFrameGlitchOnTheLatestSample_BufferedEstimateHasLessError_ButIsNotImmune` constructs exactly that scenario and measures both estimators against the true 2 m/s.

**Measured (not estimated from reading the code):**
- Instantaneous estimate: ~15.5 m/s — error ≈ **13.5 m/s** (7.75x the true speed)
- Buffered estimate (50ms lookback): ~5.4 m/s — error ≈ **3.4 m/s** (1.7x the true speed)

**Result: CONFIRMED BY INDUCTION, with an honest caveat the prediction itself already contained.** Buffering cuts the single-glitch-frame error by roughly 4x in this scenario — a real, substantial, measured improvement, not a hand-wave. But it does **not** erase the error: the glitched sample is still one of the two endpoints the buffered estimate is computed from (only the *other* endpoint moves further back), so a 15cm glitch still shows up as a ~3.4 m/s error, not zero. The error reduction is approximately the ratio of the lookback window to the per-frame interval (50ms / 11ms ≈ 4.5x), which is the actual mechanism at work — dilution over a longer baseline, not immunity.

## Why this matters more than a passing unit test

This is the first time anything in this repository has put a number on D1's claim instead of citing Table Troopers' precedent and asserting it transfers. It still doesn't answer U2/U3 (does *Quest's actual hand-tracking stack* glitch this way, and does the buffered estimator's residual ~3.4 m/s error actually matter for gameplay feel) — that remains gated on real hardware or at minimum Simulator webcam-driven testing (`README_TECHNICAL.md` section 4.1). What this *does* establish: the buffering mechanism itself does what it's supposed to do, mechanically, given a glitch of this shape — so if Stage A on real hardware eventually shows the buffered estimator still isn't good enough, the fix is a bigger lookback window or a different glitch-rejection strategy (e.g., discard outlier samples instead of just diluting them), not "throw out D1's whole approach," because the dilution mechanism is now confirmed to work as designed.

## Follow-on hypothesis this surfaces, not yet investigated

The current algorithm always anchors one endpoint on the *latest* sample, even when that sample is itself the suspected glitch. An alternative design — detect and discard outlier samples before computing velocity, rather than just using a longer baseline — might fully remove a single-frame glitch's contribution instead of diluting it. Not built: this would be new scope beyond what D1 currently specifies, and per `destination-driven-construction`, redesigning an already-working, tested mechanism on a hypothesis with no gameplay evidence yet (no real slap has been thrown on real or simulated hardware) would be exactly the premature-feature-layering this project has already self-corrected against twice. Logged here so it isn't lost if Stage A's real results eventually call for it.
