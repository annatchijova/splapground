# SlapgroundSpike

Level 0 interaction-spike instrumentation, per `README_TECHNICAL.md` section 4 at the
repo root. This folder is **explicitly throwaway** — it exists to answer one go/no-go
question (can Quest hand tracking observe and respond to a fast two-handed slap at
competitive fidelity) and is not the start of Level 1's architecture. Do not extend
these scripts into production gameplay systems; Level 1's data-driven
combinable-properties system (decision D2) starts from a clean design once Level 0
has an answer.

## Status of this code — read before trusting it

Written without a Unity Editor or the Meta XR SDK installed in this environment —
there was no way to compile, open in the Editor, or run any of it before handing it
to you. Every API call here was checked against Meta's current public Unity API
reference (`developers.meta.com`, checked 2026-10-08), not reproduced from memory,
but "checked against docs" is not the same as "compiled." Treat this as a first draft
to get into the Editor and fix on first compile, not as validated code.

Specifically unverified:
- `OVRHand.HandConfidence`, `OVRHand.GetFingerConfidence(HandFinger)`, and the
  `HandFinger` enum values — confirmed to exist in Meta's public reference, not
  compiled against the package version you'll actually install.
- `OVRSkeleton.Bones` as `IList<OVRBone>` and `OVRBone.Transform` / `OVRBone.Id` —
  the collection type and `Id` field are documented; `OVRBone.Transform` is the
  commonly-documented field name but was not independently re-verified field-by-field.
- `OVRSkeleton.BoneId` enum members used in `HandPhysicsRig` (`Hand_WristRoot`,
  `Hand_IndexTip`, etc.) — standard OVR Integration naming, not confirmed against the
  specific SDK version you install.

If any of these don't compile, that is expected friction, not a sign the approach is
wrong — fix the names against whatever the installed package's IntelliSense/generated
docs say and move on.

## What's here

- `HandVelocityBuffer.cs` — the D1 windowed/buffered velocity estimator: ring-buffers
  recent positions of one tracked transform, exposes both the naive single-frame
  velocity (for comparison) and a buffered estimate from ~50ms earlier.
- `HandPhysicsRig.cs` + `HandBoneMarker.cs` — attaches small kinematic capsule
  colliders to a subset of `OVRSkeleton` bones (wrist + five fingertips) so a
  hand-tracked hand has real PhysX presence and can transfer momentum to a Rigidbody
  object through genuine collision response, not a scripted impulse.
- `SpikeObjectMarker.cs` — tags the one spike target object.
- `SlapImpactRecorder.cs` — on collision between a hand-bone proxy and the spike
  object, logs buffered vs. instantaneous impact speed and the object's resulting
  velocity to CSV.
- `HandConfidenceLogger.cs` — per-frame `HandConfidence` + per-finger confidence time
  series to CSV, per stage/trial.
- `SpikeSessionController.cs` — stage selector (A1/A2/A3), trial reset/counter,
  session manifest.
- `EditorTrialResetBridge.cs` — keyboard-triggered reset for Editor/Link testing only;
  wire a physical poke button for on-device sessions.

All logs land in `Application.persistentDataPath/spike_logs/` on-device — pull them
with `adb pull` (exact command in the root `docs/setup/QUEST_SETUP.md`).

## Explicitly not built here

- Stage B (audio/deformation/particle feedback) — the protocol gates this behind a
  working Stage A on real hardware; building it now would be exactly the kind of
  premature feature layering the repo's own brainstorm corrected itself out of.
- Any scoring, HUD, printer/email-swarm/phone objects, or async ghost trail — all
  Level 1+, out of scope for the spike by design.
- The `.unity` scene itself. Hand-authoring a Unity scene file's YAML by hand (without
  an Editor to generate correct GUID references to installed-package prefabs) risks a
  corrupted or non-opening scene for no real time saved — `docs/setup/QUEST_SETUP.md`
  gives the exact GameObject/component assembly steps to do in the Editor instead,
  which takes a few minutes and is actually safe.
