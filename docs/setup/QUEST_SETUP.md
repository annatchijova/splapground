# Quest dev environment setup — manual steps

None of this was run. No Unity Hub, Unity Editor, Android SDK, or `adb` exist on the
machine this was written from — only `docs/` text and `unity/Assets/SlapgroundSpike/`
C# source exist on disk right now. Everything below is what you (Anna) need to run
yourself, in order, sourced from Meta's and Unity's current documentation (checked
2026-10-08, not from memory) rather than assumed from older remembered versions.

## 1. Unity Hub + Editor

1. Install Unity Hub if not already present.
2. In Hub → Installs → Install Editor, pick **Unity 6000.0.66f2 or later** (6.1+
   recommended — this is Meta's current stated minimum for the Interaction SDK, not
   an arbitrary pin).
3. During install, add modules: **Android Build Support**, its **OpenJDK**, and
   **Android SDK & NDK Tools** sub-modules.

## 2. Create the project

1. Hub → New Project → **Universal 3D** template (URP-based) → Editor version from
   step 1.
2. Point the project location at `~/splapground/unity` — the `Assets/SlapgroundSpike`
   folder already in this repo will be picked up automatically; Unity will generate
   the rest of the standard project structure (`ProjectSettings/`, `Packages/`,
   `.meta` files) on first open. That generation step is why this repo doesn't
   already contain those folders — a hand-authored `ProjectSettings` was deliberately
   not attempted; Unity's own project creation is the only reliable way to produce it
   correctly.
3. Add the Unity project's generated files to git once the project opens cleanly
   (`.gitignore` for `Library/`, `Temp/`, `Obj/`, `Build/`, `.vs/` — Unity's standard
   list; add before the first commit of generated files so they never get tracked).

## 3. Target Meta Quest

1. File → Build Profiles → Platforms → select **Meta Quest** → Enable Platform (or
   Switch, if Android support is already the active platform).
2. If prompted, let it install `com.unity.xr.openxr`.

## 4. XR Plugin Management

1. Edit → Project Settings → XR Plug-in Management → install if prompted.
2. Enable **OpenXR** under both the "Windows, Mac, Linux" tab (for Quest Link / Editor
   play-mode testing) and the "Android, Meta Quest" tab (for the on-device build).
3. Under the Android tab's OpenXR feature groups, enable: **Meta XR**, **Meta XR
   Foveation**, **Meta XR Subsampled Layout**.

## 5. Install the Meta XR SDK packages

Via Unity Asset Store (Window → Asset Store, or the external store page) → add to
your assets → Open in Unity → Install, for:

- **Meta XR Core SDK**
- **Meta XR Interaction SDK** (or the **Meta XR Interaction SDK Essentials** variant —
  functionally enough for the spike; the full package adds sample content not needed
  here)

After install: Meta XR SDK window (appears in the top menu bar) → **Project Setup
Tool** → **Fix All** → **Apply All**. This resolves most of the fiddly per-platform
settings (quest-compatible graphics API, min Android API level, etc.) automatically —
don't hand-tune those first.

## 6. Scene assembly (the one `.unity` file this spike needs)

Not pre-built — hand-authoring Unity scene YAML without an Editor to generate correct
prefab/GUID references risks a scene that silently fails to open. Build it in-Editor,
a few minutes of GameObject wiring:

1. New scene, e.g. `Assets/SlapgroundSpike/Scenes/Spike_A.unity`.
2. Add an **OVRCameraRig** (or whatever rig prefab the Interaction SDK's own
   hand-tracking sample scene uses — check the sample scene that ships with the
   Interaction SDK package for the current recommended rig prefab name, since this
   has changed across SDK versions and no installed version was available here to
   confirm the current one).
3. On the camera rig's **OVRManager** component: Quest Features → General →
   Passthrough Support → **Supported** or **Required**; Insight Passthrough →
   **Enable Passthrough**.
4. Add an empty GameObject with an **OVRPassthroughLayer** component.
5. Window → Rendering → Lighting → Environment tab → set Skybox Material to **None**
   (passthrough is occluded by a non-null skybox otherwise).
6. On the rig's left and right hand anchors, confirm **OVRHand** + **OVRSkeleton**
   components are present (the Interaction SDK's hand-tracking sample rig has these
   by default; a bare OVRCameraRig may need them added manually — check against the
   sample).
7. On each hand anchor, add `HandPhysicsRig` (from `SlapgroundSpike/Scripts`) and
   `HandConfidenceLogger`. Set `handLabel` to `Left`/`Right` respectively.
8. Create one spike object (a cube or sphere is enough — this stage is about
   tracking fidelity, not content): add `Rigidbody` (non-kinematic, default mass) and
   `SpikeObjectMarker`, then `SlapImpactRecorder`. Position it above the real desk
   height you'll be testing at.
9. Add an empty GameObject with `SpikeSessionController`; wire its `spikeObject`
   field to the object from step 8. Wire the `session` fields on
   `HandConfidenceLogger` (both hands) and `SlapImpactRecorder` to this controller.
10. For Editor/Link testing only, add `EditorTrialResetBridge` to the same object and
    wire its `session` field — press R in Play mode to reset a trial. For an actual
    on-device session, add a physical poke button (Interaction SDK has a prefab for
    this) wired to `SpikeSessionController.ResetTrial()` instead.

## 7. Build and deploy

1. Connect the Quest via USB, accept the on-headset developer-mode USB prompt.
2. File → Build Profiles → Meta Quest → Build and Run, or Build → install the
   resulting `.apk` with `adb install -r <path>.apk` if you built without a device
   attached.
3. Pull logs after a session: `adb pull /sdcard/Android/data/<package.name>/files/spike_logs ./pulled_logs` — replace `<package.name>` with the applicationId set in Player Settings. Confirm the exact persistent-data-path mapping for your Unity version if this path doesn't match; Android's app-sandboxed storage path has shifted across Unity/Android versions and was not independently re-verified here.

## What's actually been done vs. what's still yours to run

Done: the C# scripts, this setup doc, the folder scaffold.
Not done, not claimed as done: anything requiring Unity Editor, Android tooling, or
a physical Quest — none of steps 1-7 above have been executed. No scene exists yet.
No build exists yet. No on-device test has happened. Nothing here should be read as
"the spike works" until you've actually run it and the CSVs in `spike_logs/` show it.
