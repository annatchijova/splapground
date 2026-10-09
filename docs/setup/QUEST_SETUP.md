# Unity + Meta XR dev environment setup — manual steps

None of this was run. No Unity Hub, Unity Editor, Android SDK, or `adb` exist on the
machine this was written from — only `docs/` text and `unity/Assets/SlapgroundSpike/`
C# source exist on disk right now. Everything below is what you (Anna) need to run
yourself, sourced from Meta's and Unity's current documentation (checked 2026-10-08
and 2026-10-09, not from memory) rather than assumed from older remembered versions.

**Two independent paths from step 5 onward — do Path A first.** The competition's own
rules accept demo-video footage "as viewed on a Meta VR device, via XR Simulator, or
another equivalent emulator," and Meta's current messaging for this cycle says Quest
Pro-class / Glasses hardware for this competition doesn't reach general availability
until spring 2027 — the tooling is explicitly built around not needing the physical
device in hand. **Path A (Meta XR Simulator)** needs no Android module, no USB, no
headset, and gets the spike's code running in Play Mode in minutes. **Path B (real
Quest)** is still the only way to answer the project's actual gating question (does
Quest's own hand-tracking hardware survive a fast slap, U2/U3) — Simulator validates
the code, not the hardware claim. Do A first to deflate ordinary bugs cheaply, then B
for the result that actually matters.

## 1. Unity Hub + Editor

1. Install Unity Hub if not already present.
2. In Hub → Installs → Install Editor, pick **Unity 6000.0.66f2 or later** (6.1+
   recommended — this is Meta's current stated minimum for the Interaction SDK, not
   an arbitrary pin).
3. Add the **Android Build Support** module (with its **OpenJDK** and **Android SDK &
   NDK Tools** sub-modules) only if you intend to do Path B — Path A doesn't need it
   and you can add it later via Hub → Installs → the Editor's gear icon → Add Modules.

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
3. Confirm `unity/.gitignore` (already in the repo) is actually being respected before
   your first commit of generated files — `git status` should not show `Library/`,
   `Temp/`, or `Obj/`.

## 3. XR Plugin Management

1. Edit → Project Settings → XR Plug-in Management → install if prompted.
2. Enable **OpenXR** under the "Windows, Mac, Linux" tab — this alone is enough for
   Path A (Simulator). Enable it under "Android, Meta Quest" too, later, only if doing
   Path B.

## 4. Install the Meta XR packages

Via Unity Asset Store (Window → Asset Store, or the external store page) → add to
your assets → Open in Unity → Install:

- **Meta XR All-in-One SDK** (`com.meta.xr.sdk.all`) — this is the package the
  competition's rules literally name ("Unity: Meta XR SDKs v81+ (v207) All-in-One
  XR"); it bundles Core SDK and Interaction SDK, so there's no separate Interaction
  SDK install step.
- **Meta XR Simulator** (`com.meta.xr.simulator`) — needed for Path A.

After install: Meta XR SDK window (appears in the top menu bar) → **Project Setup
Tool** → **Fix All** → **Apply All**. This resolves most of the fiddly per-platform
settings automatically — don't hand-tune those first.

## 5. Scene assembly (the one `.unity` file this spike needs)

Not pre-built — hand-authoring Unity scene YAML without an Editor to generate correct
prefab/GUID references risks a scene that silently fails to open. Build it in-Editor,
a few minutes of GameObject wiring, and it serves both Path A and Path B unchanged:

1. New scene, e.g. `Assets/SlapgroundSpike/Scenes/Spike_A.unity`.
2. Add an **OVRCameraRig** (or whatever rig prefab the Interaction SDK's own
   hand-tracking sample scene uses — check the sample scene that ships with the
   All-in-One SDK for the current recommended rig prefab name, since this has changed
   across SDK versions and no installed version was available here to confirm the
   current one).
3. On the camera rig's **OVRManager** component: Quest Features → General →
   Passthrough Support → **Supported** or **Required**; Insight Passthrough →
   **Enable Passthrough**. (Simulator may render passthrough as a flat color or skip
   it depending on version — this is a Path B concern, don't block Path A on it.)
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
10. Add `EditorTrialResetBridge` to the same object and wire its `session` field —
    press R in Play mode to reset a trial (works in both Path A and Editor/Link
    testing). For an actual on-device Path B session, add a physical poke button
    (Interaction SDK has a prefab for this) wired to
    `SpikeSessionController.ResetTrial()` instead, since there's no keyboard on Quest.

## Path A — Meta XR Simulator (no headset, do this first)

1. Open the **Meta XR Simulator** window (menu added by the package) and toggle it as
   the active OpenXR runtime.
2. In the Simulator window → Inputs → Global Input Settings, set Left and/or Right
   input device to **Hand**.
3. Enter Play mode. Drive hand pose with keyboard keys `1`/`2`/`3`/`4` (aim/poke/
   pinch/grab) or hold the left mouse button for a pinch gesture. This is discrete
   pose simulation, not a real swing — it will exercise the collision/logging code
   paths but will not produce a realistic slap velocity.
4. If the installed Simulator version exposes **camera-driven hand tracking** (added
   in v207 per Meta's changelog — confirm it's present in your installed version,
   not assumed), enable it per-hand and try an actual physical slap motion in front
   of the webcam instead of keyboard poses. Closer to the real gesture, still not
   Quest's own sensor stack — treat results as "the code handled noisy/fast input
   without crashing," not as "Quest tracking holds up" (that's `README_TECHNICAL.md`
   section 4.1).
5. Logs land in `Application.persistentDataPath/spike_logs/` on the desktop machine
   itself (a normal OS user-data folder, e.g. under `%APPDATA%`/`~/Library/Application
   Support`/`~/.config` depending on platform and Unity version — check the Console
   window's first log line if `Application.persistentDataPath` is printed anywhere,
   or add a temporary `Debug.Log(Application.persistentDataPath)` if not). No `adb`
   needed for this path.

## Path B — real Quest build and deploy

1. Add the Android Build Support module (step 1) if not already installed.
2. File → Build Profiles → Platforms → select **Meta Quest** → Enable Platform (or
   Switch, if Android support is already the active platform). Let it install
   `com.unity.xr.openxr` if prompted.
3. Edit → Project Settings → XR Plug-in Management → enable OpenXR under the
   "Android, Meta Quest" tab too; under its OpenXR feature groups enable **Meta XR**,
   **Meta XR Foveation**, **Meta XR Subsampled Layout**.
4. Connect the Quest via USB, accept the on-headset developer-mode USB prompt.
5. File → Build Profiles → Meta Quest → Build and Run, or Build → install the
   resulting `.apk` with `adb install -r <path>.apk` if you built without a device
   attached.
6. Pull logs after a session: `adb pull /sdcard/Android/data/<package.name>/files/spike_logs ./pulled_logs` — replace `<package.name>` with the applicationId set in Player Settings. Confirm the exact persistent-data-path mapping for your Unity version if this path doesn't match; Android's app-sandboxed storage path has shifted across Unity/Android versions and was not independently re-verified here.

## What's actually been done vs. what's still yours to run

Done: the C# scripts, this setup doc, the folder scaffold.
Not done, not claimed as done: anything requiring Unity Editor, Android tooling,
Meta XR Simulator, or a physical Quest — none of the steps above have been executed.
No scene exists yet. No build exists yet. No Simulator run and no on-device test has
happened. Nothing here should be read as "the spike works" until you've actually run
it — Path A or Path B — and the CSVs in `spike_logs/` show it. A clean Path A run is
evidence the code works, not evidence H1's hardware question (U2/U3) is answered —
only Path B, or at minimum a clearly-labeled webcam-driven Path A result, speaks to
that.
