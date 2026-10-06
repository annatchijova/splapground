# SLAPGROUND — Technical README

[English](./README.md) · [Español](./README_ES.md) · **Technical README**

Full depth: architecture decisions, the interaction-spike protocol, sourced competitive research, unresolved risks, and the level plan toward the submission. Unsoftened — this is the document for someone auditing or extending the project, not introducing it.

Raw, unedited brainstorm (including the Gemini red-team and the walk-back that corrected it) lives in [`BRAINSTORM.md`](./BRAINSTORM.md).

---

## 1. Status, honestly

**H1 (the hypothesis stated in the primary README) is a candidate, not a committed scope.** Nothing described below as "Level 1" has been built. The project is currently pre-spike: no Unity project, no device test, no validated interaction. Concept art in `docs/concept-art/` is AI-generated (Gemini/ChatGPT) illustrative material commissioned to communicate the pitch internally — it is not gameplay footage and must never be presented as such.

Conceptually: strong. Competitively: researched, not yet fully decomposed. Technically: plausible, not validated. Core interaction: not validated. Fun: completely unvalidated.

---

## 2. The levels toward the destination

Per house method (`destination-driven-construction`): the target is the ambitious final product, named up front, not an MVP grown in phases. What varies under the competition's 44-day window is how many coherent levels get reached — not what the destination is, and not what "done" means for a level that is attempted.

**Destination:** a shippable Meta Quest title — Unity 6, C#, OpenXR, Meta XR Interaction SDK — where a real desk is an extensible hands-first physical-catharsis platform. Multiple object "worlds" (Office, Kitchen, Workshop) share one combinable-properties physics system; an asynchronous competitive loop (ghost trail, CPM, Max Kinetic Chain) and clip-good-by-default sharing are present from the first shippable world, not added after. Submitted to Meta VR Start Developer Competition 2026, Gaming track, New Experience division.

- **Level 0 — interaction spike (pre-level, disposable).** The protocol in §4 below. Explicitly throwaway instrumentation, not shaped like a game and not kept. Its only job: a go/no-go on whether the core gesture is buildable at the fidelity a competitive game needs. This is validation, not progress toward the destination — it decides whether Level 1 is worth starting.
- **Level 1 — The Office (Desk Rage), complete.** Not a tech demo. A real, shippable single-world game: the four Office objects (alarm clock, printer, email swarm, corded phone) built on a **data-driven combinable-properties system** (mass, fragility, articulation, elasticity as data, not per-object hardcoded behavior) so later worlds extend it instead of forcing a rewrite; the full 3-minute Desk Sprint + Destroy Everything session loop; the async ghost-trail/CPM/Max Kinetic Chain competitive loop; and clip-generation that is good by default — because that property is destination-defining, not a feature to retrofit after launch. Windowed/buffered impact detection (§4, Table Troopers precedent) from the start. **This is the level actually submitted if time runs out here** — a complete, honest entry, not an apology for an unfinished one.
- **Level 2+ — additional worlds (Kitchen Rage, Workshop Chaos).** Pure content extension on Level 1's property architecture: new data, same system, no redesign. Explicitly not attempted, not designed, before Level 1 is real — per the brainstorm's own correction of the Gemini red-team's premature roadmap.

Under the deadline, the correct response is **fewer levels, fully built** — one complete Office world with a real competitive loop beats three half-built worlds wearing feature labels.

### Decision record

**D1 — Impact detection uses a windowed/buffered estimator, not single-frame contact, from Level 1's first implementation.**
Forces: Table Troopers (§5) does not trust the hand's raw instantaneous position at a game-critical frame even for a far gentler gesture (pinch-release), buffering recent positions and using "an earlier valid point" instead.
Assumption: the same buffering approach generalizes from a slow pinch-release to a fast two-handed slap.
Revisit trigger: if Stage A/B (§4) shows frame rate is high enough that buffering adds perceptible, unacceptable latency to the felt impact.

**D2 — Object behavior is data (mass, fragility, articulation, elasticity), not per-object script, starting at Level 1.**
Forces: the skill ceiling has to come from systems interacting, not content volume (per the brainstorm's own combo design); Level 2+ worlds must extend the system by adding data, not rewriting it.
Assumption: a small property set is expressive enough to produce emergent, non-scripted combos.
Revisit trigger: if Level 1 playtesting shows combos feel samey regardless of object mix, meaning the property set is too thin and needs a richer model before Level 2.

**D3 — Clip-good-by-default is a Level 1 architectural requirement, not a post-launch feature.**
Forces: "the clip has to be good by default" (§6) is the mechanism the entire social/sharing loop depends on; retrofitting highlight detection into a codebase not built to record it is far more expensive than building it in from the start.
Assumption: Quest's capture/compositing pipeline (third-person virtual camera, per the original brainstorm) can be driven programmatically to detect and export a highlight window without manual editing.
Revisit trigger: if investigation of the current SDK capture tooling shows this cannot be triggered/clipped programmatically within what the competition's submission process allows.

---

## 3. Unresolved risks (carried from the brainstorm)

U1 — is impact without haptics satisfying? U2 — does Quest track a real slap at speed? U3 — does bimanual interaction survive hand-on-hand occlusion? U4 — can it be physically safe without caging the fantasy? U5 — does MR add real value over VR-in-a-void? U6 — does skill emerge after 30 minutes or just repetition? U7 — are the "emergent" combos real or imagined on paper? U8 — is this different enough from what already exists?

U2 and U3 gate the project: if a fast double-hand slap loses tracking or the hands occlude each other at the moment of impact, there is no fallback design — the pitch is physical, two-handed, and fast by construction. Everything else (scoring, clips, roadmap) is downstream of those two holding up on real hardware.

Where the brainstorm's own self-critique was right, and stays frozen until U1–U4 answer: the 40×40cm floating interaction cube and "bank-shots only downward" are safety patches bolted onto a game nobody has played yet — they may solve a problem that doesn't exist, or solve it by killing the core promise (the *whole* desk, not a fenced-off box over the keyboard). Any numeric claim about Quest camera Hz, motion blur, or Scene Understanding precision that originated from a model (Gemini or otherwise) is a claim, not a fact, until checked against Meta's current developer documentation.

---

## 4. Interaction-spike protocol (Level 0)

An instrumentation exercise, not a game. Explicitly not sufficient to validate or kill H1 from a single rough implementation — a bad first-pass slap has at least six rival explanations (broken tracking, poor impact detection, miscalibrated physics impulse, missing feedback compensation, bimanual occlusion, or a genuinely dead gesture), and only the last one kills the hypothesis. Shipping a mediocre first pass and concluding "Quest can't do this" repeats exactly the overinterpretation the brainstorm already flagged in the Gemini red-team.

- **Stage 0 — table, one object, two hands, nothing else.** No score, no printer, no email swarm, no particles. Particles and audio are exactly the kind of multimodal compensation that can mask a mediocre underlying signal; they're added later, deliberately, once the raw signal is characterized.
- **Stage A — can Quest observe the action at all?** Measured by instrumentation, not by feel, staged to localize *where* degradation enters instead of one pass/fail number:
  - **A1 — striking hand + fixed/anchored object.** Isolates the fast-hand gesture with no second-hand confound.
  - **A2 — striking hand + second hand present nearby, not holding.** Adds proximity/occlusion without adding the anchor hand's own motion/absorption as a variable.
  - **A3 — second hand holding the object.** Full bimanual interaction as intended in-game.
  A1 holding while A2 degrades and A3 breaks down further is a clean localization — far more useful than one confounded "slapped a cube and it got weird" result.
  - **Metrics bounded to what the SDK actually exposes:** `HandConfidence` (LOW/HIGH per hand), `FingerConfidences` (per-finger array), across the standard 26 OpenXR hand joints, logged as a continuous time series through the impact window — not a single boolean. The goal is a characterized *region* of acceptable degradation (confidence may dip during the impact frame and recover immediately, and still be perfectly playable), not an arbitrary threshold set before anything has been played. Table Troopers' real-time degradation warning (red tint on confidence drop) is reusable here too, both as a spike diagnostic and a candidate production safety net.
  - Vary gesture shape within each stage — short wrist flick vs. wider swing, palm vs. back of hand, varying velocities — logging what degrades tracking independent of subjective judgment.
- **Stage B — does the interaction feel like a satisfying impact without haptics?** Runs only once Stage A finds a usable gesture variant. Impact detection uses the windowed/buffered estimator from D1, not naive single-frame collision. Compares bare interaction (no feedback) against the same interaction with audio + deformation + particles added deliberately, isolating how much of the "feel" is tracking fidelity versus compensatory feedback design.
- **U1 — is it fun?** Explicitly orthogonal to A and B. Perfect tracking and well-tuned feedback can still produce something boring; only answerable by playing, asked honestly after A and B resolve, never assumed as their byproduct.

If Stage A fails across gesture variants, that's the result that actually kills H1. A bad Stage B result with a working Stage A means iterate on feedback design, not abandon the hypothesis.

---

## 5. Competitive research: Hand Physics Lab and Table Troopers (2026-10-05)

Meta's own competition page names these two as sample use cases for hands-first design. Researched directly rather than assumed, with facts and inference kept explicitly separate.

**Hand Physics Lab (Holonautic)** — [developer success story](https://developers.meta.com/horizon/discover/success-stories/hand-physics-lab-holonautic/), [UploadVR review](https://www.uploadvr.com/hand-physics-lab-review/)

- Facts: grab/pinch/push/paint/build sandbox, >1.3M copies sold, iterative "Hand Tracking 2.0+" updates since 2021. No haptics; compensates with "simulated weight" — physics-based resistance that makes a heavy object feel hard to push despite moving through empty air. No functional MR/passthrough role described. No mastery/progression system described beyond general engagement.
- Facts (review): even careful, slow, deliberate interactions (precise grabbing, pressing a button) are reported as "not necessarily doable on the very first try" without haptic confirmation. One precision puzzle was frustrating enough that the reviewer switched to controllers to finish it.
- What's absent: no documented fast/impact-style gesture anywhere in the material found — every interaction described is continuous, deliberate manipulation, not an instantaneous high-velocity contact event.
- Inference (flagged, not sourced): if careful low-speed manipulation already fails on first try without haptic confirmation, a fast two-handed slap — higher velocity, larger displacement, two hands converging near each other — is a strictly harder case than anything HPL has publicly solved. This raises, not lowers, the prior risk on U1/U2; it should not be read as "contact without haptics is already solved on Quest."

**Table Troopers (Cosmorama/Altlab)** — [developer success story](https://developers.meta.com/vr/discover/success-stories/table-troopers/)

- Facts: pinch-first, **turn-based** (not real-time) tactical artillery game. The real table is the battlefield via passthrough; physics simulation is explicitly split between the virtual game world and the player's real room, so projectiles leaving the play area collide with actual room geometry and fall to the real floor. Fully destructible terrain. Multiplayer/leaderboard competitive loop (synchronous, "gather around a physical table"), not async ghost-trail/clips.
- Facts: the team does **not** trust the hand's exact instantaneous position at the triggering frame (pinch release/fire) — they buffer recent pinch positions and use "an earlier valid point" to infer intended aim, because raw single-frame position at the critical instant was unreliable even for a comparatively gentle gesture. They required 99.9% hand-tracking reliability before shipping hand-tracking support at all, specifically for competitive fairness, delaying launch to meet that bar. They show a real-time visual warning (red tint) when tracking confidence degrades.
- Inference: the "infer the action from a recent window, don't trust the single triggering frame" pattern is very likely directly transferable to SLAPGROUND's impact detection (D1). If a gentle pinch-release needed this treatment, a slap's exact contact frame is a reasonable candidate for the same unreliability.
- The real-geometry physics split (virtual object ↔ player's actual room/table) is a genuine, directly relevant engineering precedent for SLAPGROUND's desk bank-shots, worth studying in depth before designing that system from scratch. It is commercial IP — study the approach, don't copy the implementation.

**What this changes about H1:** neither sample title Meta itself points to validates SLAPGROUND's actual core gesture — a fast, two-handed, impact-based slap. The two closest hands-first titles on Quest deliberately avoided exactly that case: one through slow continuous manipulation, the other by going turn-based and explicitly distrusting raw instantaneous tracking even for a much gentler gesture. This strengthens the differentiation claim (nobody has publicly shown this solved on Quest) but removes any implicit assumption that the core mechanic is already known to be buildable at competitive-grade fidelity. It raises the priority of the Level 0 spike and fixed D1 (windowed/buffered detection) as the default, not a fallback.

---

## 6. Positioning and product requirements

"VR but you break things," or even "MR but you break things on a table," is weak and trivially copyable — some version of the rage-room genre already exists. The claim worth defending:

> A real everyday surface becomes a competitive physical system. Objects have combinable properties, and mastering those properties lets you build increasingly complex kinetic chains. Destruction is the fantasy; combinatorial physics is the game.

Before resuming any design work beyond Level 1, the competitive-landscape pass should decompose the space along four axes, not just ask "has rage-room been done": what interaction already exists, what fantasy already exists, what use of MR already exists, what mastery system already exists — then identify what combination of those four is actually novel here.

**The clip has to be good by default (D3).** Not "you can record and share" — the system has to know where the interesting moment was and produce something worth sending without the player editing anything. If a spectacular run requires camera wrangling, replay-scrubbing, trimming, framing, and exporting before it's shareable, the social behavior the whole loop is built around doesn't happen.

---

## 7. Stack

Unity 6, C#, OpenXR, Meta OpenXR, Meta XR Core SDK, Meta XR Interaction SDK, Unity Physics — chosen over Unreal/Native OpenXR/Godot/WebXR/Spatial SDK because Meta's Interaction SDK already solves grab/poke/pose detection for Unity specifically, OpenXR is Meta's currently supported path for new Quest development, and the language layer (C#) is not expected to be the actual bottleneck — the Unity/XR domain knowledge is. Full reasoning and the rejected-alternatives table are in [`BRAINSTORM.md`](./BRAINSTORM.md).

Full submission requirements (division, hard requirements, what to submit) are in the competition material captured in `BRAINSTORM.md` — this document does not duplicate them.
