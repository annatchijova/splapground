# Slapground.Core

Engine-independent logic for the Level 1 data-driven combinable-properties system
(decision D2, `README_TECHNICAL.md` section 2). Plain C#/.NET, no `UnityEngine`
reference — this is the one piece of SLAPGROUND that doesn't need Unity Editor, a
device, or the Meta XR Simulator to build and verify, so it's where real
implementation started (2026-10-09) while the Unity-side spike from
`unity/Assets/SlapgroundSpike/` is still waiting on an actual Editor run.

**Unlike the Unity spike scripts, this is genuinely verified, not "written against
docs":**

```
cd core
~/.dotnet/dotnet test
```

69/69 tests pass as of this commit (run output not just claimed — re-run the command
above to confirm it still does before trusting this line).

**2026-10-09, red team round 1:** found one real correctness defect (`ChaosPerMinute`
accepted an out-of-order `now` and silently miscounted — now throws, matching
`RecordImpact`'s own contract), falsified one suspected defect (`HighlightWindowFinder`
fuzzed against a brute-force reference over 300 random streams, 0 divergences), and
closed one product/UX gap against the competition's own Design Guidelines
(`SessionClock` had no pause/resume — see below). Full writeup, including the
discarded vectors, in `docs/red-team-round-1-core.md` at the repo root.

**2026-10-09, red team round 2:** ported D1's windowed/buffered velocity estimator
out of the unverified Unity spike script into `VelocityBuffer.cs`, then actually
measured its central claim with a synthetic tracking-glitch test instead of citing
precedent and assuming it transfers — buffering cut a single-glitch-frame velocity
error by ~4x (13.5 m/s down to 3.4 m/s in the test scenario) but does not erase it.
Full writeup in `docs/red-team-round-2-core.md`.

**2026-10-09, red team round 3:** fuzzed four invariants (Fragility/impulse
monotonicity in `BreakageRule`, the Max≥Current chain invariant, `VelocityBuffer`'s
never-before-tested overflow/eviction path, `SessionClock`'s float-boundary
behavior across phase transitions). All four held — an honest no-findings round,
written up the same as the rounds that found things. `docs/red-team-round-3-core.md`.

## What's here

- `PhysicalProperties.cs` — the four D2 fields (Mass, Fragility, Elasticity,
  Articulation) as one validated value type. Exactly four, per the decision record;
  adding a fifth is a scope change to D2, not a casual extension.
- `Articulation.cs` — Rigid / Hinged / Flail. Data only — the Unity-side factory
  that turns this into an actual Rigidbody/HingeJoint/joint-chain setup is unbuilt
  and belongs in the Unity project, not here.
- `BreakageRule.cs` — turns a PhysX-reported impact impulse plus an object's
  Fragility into a break/no-break decision. Deliberately does not reimplement
  collision response; PhysX already does real rigid-body physics via Rigidbody +
  the PhysicsMaterial that `MaterialMapper` produces. `ReferenceImpulse` is a
  placeholder tuning constant, not sourced from any measurement — expect to retune
  once Level 1 objects have real masses and playtested impact magnitudes.
- `MaterialMapper.cs` — Elasticity/Articulation to Unity `PhysicsMaterial`
  parameters (bounciness, friction), one mapping instead of one per prefab.
- `ChainTracker.cs` — the Max Kinetic Chain / Chaos-Per-Minute scoring named in
  Level 1. Pure event-stream analysis over caller-supplied timestamps, not
  `Time.time` or the system clock, which is exactly what makes it testable without
  Unity and reusable from either the Unity side or a future async-leaderboard
  backend unchanged.
- `SessionClock.cs` — the 3-minute Desk Sprint + 10-second Destroy Everything
  session shape named in `README.md`/`README_TECHNICAL.md`, as a phase state
  machine over caller-supplied timestamps, same style as `ChainTracker`. Has
  `Pause`/`Resume`/`IsPaused` — added in the 2026-10-09 red-team pass once it was
  checked against the competition's "clean pause/resume" Design Guideline and
  found missing.
- `OfficeObjectCatalog.cs` — concrete `PhysicalProperties` for the four named
  Office objects (alarm clock, printer, email notification/swarm archetype,
  corded phone). Every number in it is a first-pass design placeholder reasoned
  from the product brief's own object descriptions, not from any playtest or
  measurement — see the inline comments per object and expect to retune all of it
  once Level 1 has real play sessions.
- `VelocityBuffer.cs` — D1's windowed/buffered velocity estimator, ported from
  the unverified Unity spike script (`unity/Assets/SlapgroundSpike/Scripts/
  HandVelocityBuffer.cs`) into this tested layer. Uses `System.Numerics.Vector3`,
  not `UnityEngine.Vector3` — still zero Unity dependency. The Unity spike script
  is unchanged and still duplicates this logic for now; consolidating it to call
  into Core instead is future work once a Unity Editor actually exists to verify
  the wiring (`unity/Assets/SlapgroundSpike/README.md` notes this).
- `HighlightWindowFinder.cs` — D3's "the clip has to be good by default": a
  sliding-window scan over a session's logged impacts that returns the
  fixed-duration window with the highest total impact magnitude, so a highlight
  clip can be auto-cut without the player scrubbing anything. Scoring is
  deliberately simple (sum of magnitudes, no chain-length or recency bonus) —
  see the file's own comment for why a fancier weighting is deferred rather than
  guessed at now.

## Why this and not the collision math itself

An earlier draft of this library computed full 1D elastic-collision velocities by
hand. Dropped: Unity's own PhysX already does correct 3D rigid-body collision
response once `Rigidbody` and `PhysicsMaterial` are set up correctly — reimplementing
that here would be duplicating a physics engine Unity already ships, which is exactly
the kind of unneeded abstraction to avoid. What's actually D2-specific, and what
PhysX can't decide on its own from data alone, is the breakage threshold and the
scoring layer above individual collisions — that's what's implemented here.

## How this connects to Unity, later

Once a Unity Editor is available: this project targets `netstandard2.1` specifically
so its compiled DLL (or the source files directly) can drop into
`unity/Assets/SlapgroundSpike/` or a future `Assets/SlapgroundCore/` folder and run
inside Unity's Mono/IL2CPP runtime unchanged. Not done yet — no Editor existed to
confirm that import step actually works cleanly, so treat "compiles under .NET 8
outside Unity" and "compiles inside Unity" as two separate claims until the second
one is actually checked.
