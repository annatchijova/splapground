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

22/22 tests pass as of this commit (run output not just claimed — re-run the command
above to confirm it still does before trusting this line).

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
