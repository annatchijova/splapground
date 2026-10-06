# SLAPGROUND

Working hypothesis for the **Meta VR Start Developer Competition 2026** (track: Gaming, hands-first, $1M prize pool, deadline Nov 18 2026).

> **H1:** A seated mixed-reality playground where your real desk becomes a physical catharsis toy. Objects representing everyday office stress (an alarm clock, a printer, a swarm of emails, a corded phone) spawn above the desk with distinct physical properties. You grab and slap them with your bare hands — no controllers — to trigger chains of destruction that are short, legible, competitive, and clip-shareable.

Full raw brainstorm (unedited, includes the Gemini red-team and the subsequent walk-back) is in [`BRAINSTORM.md`](./BRAINSTORM.md).

This README is Colectivo VIGIA's read on the idea: what to keep, what's actually load-bearing, what's still unproven, and where it sits against existing products. It is a vote, not a verdict — H1 is a candidate, not a committed scope.

---

## What's genuinely strong here

1. **The fantasy is load-bearing, not decorative.** "Your desk becomes the arena" is the one thing a flat-screen or VR-headset-in-a-void game structurally cannot offer. If the MR tracking of the real desk surface holds up, this is a differentiator that survives scrutiny — it's not a skin on a generic idea.
2. **Session shape matches the stated design constraints.** The competition explicitly scores against the "airplane seat test" (two-foot radius, seated) and the "one bus stop test" (a complete satisfying moment in under 10 minutes). The 3-minute Desk Sprint + 10-second Destroy Everything coda is a direct, honest fit — not a retrofit.
3. **Skill ceiling is derived from object physics, not from added rules.** Weight, fragility, articulation (the corded phone as a flail) create emergent combos without needing scripted levels. That's the right shape for replay value in a short-session game: depth from systems, not from content volume.
4. **The asynchronous competitive loop (ghost trail, CPM, Max Kinetic Chain, auto-clip) is the correct answer to "why play twice."** It avoids the cost of real-time multiplayer netcode and arm-hologram occlusion (which the brainstorm already correctly identifies as visual noise) while still giving the two things people actually come back for: a number to beat and a clip to post.
5. **The division choice (New Experience, built from Sept 24 2026) is clean** — no prior-art entanglement, no "is this a significant update" judgment call from the sponsor.

## Where the self-critique in the brainstorm was right, and should be kept

The walk-back inside `BRAINSTORM.md` (the part starting "Sí: esto ya está demasiado 'cerrado' otra vez") is correct and should not be re-closed prematurely:

- The 40×40cm floating cube and "bank-shots only downward" are **safety patches bolted onto an unplayed game**. They may be solving a problem that doesn't exist yet, or solving it in a way that kills the core promise (your *whole* desk, not a fenced-off box over the keyboard).
- Kitchen Rage / Workshop Chaos are roadmap fiction for a mechanic that hasn't been touched. Cut them from any submission-facing material until U1–U4 below are answered.
- Any numeric claim about Quest camera Hz, motion blur behavior, or Scene Understanding precision that came from a model (Gemini or otherwise) is a claim, not a fact, until checked against Meta's current developer documentation. Do not let a hallucinated spec become an engineering constraint.

## Unresolved risks (carried from the brainstorm, unchanged)

U1 impact without haptics — does it feel satisfying. U2 — does Quest track a real slap at speed. U3 — does bimanual interaction survive hand-on-hand occlusion. U4 — can it be physically safe without caging the fantasy. U5 — does MR add real value over VR-in-a-void. U6 — does skill emerge after 30 minutes or just repetition. U7 — are the "emergent" combos real or imagined on paper. U8 — is this different enough from what already exists.

U2 and U3 are the ones that actually gate the project: if a fast double-hand slap loses tracking or the hands occlude each other at the moment of impact, there is no fallback design — the whole pitch is physical, two-handed, and fast. Everything else (scoring, clips, roadmap) is downstream of those two holding up on real hardware.

## Competitive landscape — read with the same discipline as above

This is a comparison of *genres and mechanics* I can speak to with reasonable confidence, not a verified feature audit of named competitors' current builds — Meta's store catalog changes continuously and specific current specs should be checked directly before being used as a positioning claim.

- **VR "rage room" style experiences already exist** as a category on Quest and other VR storefronts — the pure "break things for stress relief" loop is not novel by itself. SLAPGROUND's differentiation has to come from (a) real-desk MR passthrough instead of a generic virtual room, and (b) a systemic skill ceiling (object properties interacting), not from "destruction" as the headline.
- **Asynchronous ghost-trail competition and auto-generated shareable clips are a proven retention pattern** in short-session VR titles — it's a sound mechanic to borrow, and the brainstorm already made the right call rejecting a live rival hologram in favor of a faint light trail (avoids occlusion *and* avoids the visual-clutter failure mode of that pattern).
- **Short-session, high-skill-ceiling, no-controller titles are the exact genre the competition is explicitly recruiting for** ("hands-first," "one bus stop test"), which means judges will have a well-formed comparison set in their heads. The differentiator can't just be "stress relief" or "short sessions" — those are table stakes for this competition, not a pitch.
- **The real competitive risk isn't a named product — it's the null hypothesis that MR passthrough contact-physics-on-a-real-surface has been tried before and quietly dropped** because of exactly U2/U3. That's worth a direct search against Meta's current Hands & Eyes documentation and sample use cases (the competition page itself lists "Hands Physics Lab" and "Table Troopers" as sample use cases — those should be looked at directly, not assumed, since they may already be close prior art worth differentiating against explicitly in the submission text).

## What actually makes people come back (mapped to mechanics already in H1)

- **Cheap dopamine, fast** → the slap-to-destruction loop has near-zero input-to-payoff latency by design; keep that latency low, it's the whole hook.
- **Sharing accomplishment** → the auto-clip + CPM/Max Kinetic Chain score already target this; the clip has to be *automatic* and *good by default*, because a stressed user will not spend time editing a highlight reel.
- **Something fast** → 3-minute Desk Sprint is correctly sized against the competition's own 10-minute bar.
- **Stress release** → the frame ("the printer that represents every problem you had today gets the treatment it deserves") is the right emotional register — keep it irreverent, not wellness-coded ("breathe deeply, visualize a beach" is explicitly the wrong tone, and the brainstorm already says so).

## VIGÍA's vote

Keep H1 as a named candidate, not a committed scope. Don't resume feature design (Kitchen Rage, safety cubes, bank-shot physics) until the gating unknowns have an answer from a real Quest device, not from a model's prediction.

In parallel, and before writing anything submission-facing: pull Meta's current Hands & Eyes documentation and look at "Hands Physics Lab" and "Table Troopers" directly — both are listed on the competition page itself as sample use cases and may already occupy adjacent ground that the submission needs to explicitly differentiate against.

### Correction (2026-10-05): the grab-and-slap spike as originally proposed is confounded

A first version of this document proposed "one hand grabs a cube, the other slaps it, on-device, nothing else built" as the experiment that validates or kills U2 (slap-speed tracking) and U3 (bimanual occlusion). That's necessary but not sufficient, and treating it as sufficient is a real risk: if a first rough implementation of the slap feels bad, there are at least six rival explanations —

1. tracking doesn't support the gesture,
2. impact detection is poorly tuned,
3. the physics impulse is miscalibrated,
4. audiovisual feedback doesn't compensate for the missing haptic contact,
5. bimanual occlusion breaks the hand-pose estimate,
6. or the core interaction genuinely does not work on Quest.

Only explanation 6 kills H1. Shipping a mediocre first-pass slap and concluding "Quest can't do this" would be exactly the kind of overinterpretation this document already flagged in the Gemini red-team — mistaking an artifact of the first implementation for a property of the hardware.

**Revised protocol — an interaction spike, not a game, and the questions kept deliberately separate so a bad result in one doesn't contaminate the others:**

- **Stage 0 — table, one object, two hands, nothing else.** No score, no printer, no email swarm, no particles. Particles and audio are exactly the kind of multimodal compensation that can mask a mediocre underlying signal — they come later, deliberately, once the raw signal is characterized.
- **Stage A — can Quest observe the action at all?** Measured by instrumentation, not by how it feels, and staged to isolate *where* degradation enters rather than lumping it into one pass/fail number:
  - **A1 — striking hand + fixed object** (mounted/anchored, not held). Characterizes the fast-hand gesture alone, with no second-hand confound.
  - **A2 — striking hand + second hand present nearby but not holding.** Introduces proximity/occlusion without adding the anchor hand's own absorption/motion as a variable.
  - **A3 — second hand holding the object**, full bimanual interaction as actually intended in-game.
  If A1 holds up but A2 degrades and A3 breaks down further, that's a clean localization of where the problem actually lives — much more useful than a single confounded "slapped a cube and it got weird" result.
  - **Metrics, bounded to what the SDK actually exposes rather than invented:** `HandConfidence` (LOW/HIGH per hand), `FingerConfidences` (per-finger array), tracked across the standard 26 OpenXR hand joints, logged as a continuous time series through the impact window — not a single boolean "tracking worked/didn't." The goal is to characterize a *region* of acceptable degradation (confidence may dip during the impact frame and recover immediately, and still be perfectly playable), not to set an arbitrary pass/fail threshold before anything has been played. Table Troopers' real-time visual degradation warning (red tint when confidence drops) is a reusable pattern here too — both as a diagnostic aid during the spike and as a candidate production safety net later.
  - Vary gesture shape within each stage — short wrist flick vs. wider swing, palm vs. back of hand, varying velocities — and log what degrades tracking, independent of subjective judgment.
- **Stage B — does the interaction feel like a satisfying impact without haptics?** Only run once Stage A has established the tracking signal is usable for at least one gesture variant, and build impact detection on a windowed/buffered estimator from the start (per Table Troopers' precedent above — infer contact from a recent position/velocity window, don't trust the single triggering frame) rather than naive single-frame collision detection. Compare bare interaction (no feedback) against the same interaction with audio + deformation + particles added deliberately, to isolate how much of the "feel" is tracking fidelity versus compensatory feedback design.
- **U1 — is it fun?** Explicitly orthogonal to A and B. Perfect tracking and well-tuned feedback can still produce something boring. This is only answerable by playing, not by instrumenting, and should be asked honestly after A and B are both resolved — not assumed as a byproduct of solving the other two.

If Stage A fails across gesture variants, that's the result that actually kills H1. A bad Stage B result with a working Stage A means iterate on feedback design, not abandon the hypothesis.

### Competitive research: Hand Physics Lab and Table Troopers (2026-10-05)

Meta's own competition page names these two as sample use cases for hands-first design. Researched directly rather than assumed. Facts below are sourced; inference is marked explicitly and kept separate, per house method.

**Hand Physics Lab (Holonautic)** — [developer success story](https://developers.meta.com/horizon/discover/success-stories/hand-physics-lab-holonautic/), [UploadVR review](https://www.uploadvr.com/hand-physics-lab-review/)

- Facts: grab/pinch/push/paint/build sandbox, >1.3M copies sold, iterative "Hand Tracking 2.0+" updates since 2021. No haptics; compensates with "simulated weight" — physics-based resistance that gives a heavy object the *feel* of being hard to push despite moving through empty air. No functional MR/passthrough role described. No mastery/progression system described beyond general engagement.
- Facts (review): even careful, slow, deliberate interactions (grabbing precisely, pressing a button) are reported as "not necessarily doable on the very first try" without haptic confirmation. One precision puzzle was frustrating enough that the reviewer switched to controllers to finish it.
- What's absent: no documented fast/impact-style gesture (slap, strike) anywhere in the material found — every interaction described is continuous, deliberate manipulation, not an instantaneous high-velocity contact event.
- Inference (not sourced, flagged as such): if careful low-speed manipulation already fails on first try without haptic confirmation, a fast two-handed slap — higher velocity, larger displacement, two hands converging near each other — is a strictly harder case than anything HPL has publicly solved. This should raise, not lower, the prior risk on U1/U2, and should not be read as "contact without haptics is already a solved problem on Quest."

**Table Troopers (Cosmorama/Altlab)** — [developer success story](https://developers.meta.com/vr/discover/success-stories/table-troopers/)

- Facts: pinch-first, **turn-based** (not real-time) tactical artillery game. Real table becomes the battlefield via passthrough; physics simulation is explicitly split between the virtual game world and the player's real room, so projectiles leaving the play area collide with actual room geometry and fall to the real floor. Fully destructible terrain. Multiplayer/leaderboard competitive loop (synchronous, "gather around a physical table"), not async ghost-trail/clips.
- Facts: the team does **not** trust the hand's exact instantaneous position at the triggering frame (pinch release/fire) — they buffer recent pinch positions and use "an earlier valid point" to infer intended aim, because raw single-frame position at the critical instant was unreliable even for a comparatively gentle gesture. They required 99.9% hand-tracking reliability before shipping hand-tracking support at all, specifically for competitive fairness, and delayed launch to meet that bar. They show a real-time visual warning (red tint) when tracking confidence degrades.
- Inference: the "infer the action from a recent window, don't trust the single triggering frame" pattern is very likely directly transferable to SLAPGROUND's impact detection. If a gentle pinch-release needed this treatment, a slap's exact contact frame is a reasonable candidate for the same kind of unreliability — this should be the default detection design in the spike, not a fix bolted on after naive single-frame detection fails.
- The real-geometry physics split (virtual object ↔ player's actual room/table) is a genuine, directly relevant engineering precedent for SLAPGROUND's desk bank-shots and worth studying in depth before designing that system from scratch — it is commercial IP, so study the approach, don't copy implementation.

**What this changes about H1:** neither sample title Meta itself points to validates SLAPGROUND's actual core gesture — a fast, two-handed, impact-based slap. The two closest hands-first titles on Quest deliberately avoided exactly that case: one through slow continuous manipulation, the other by going turn-based and explicitly distrusting raw instantaneous tracking even for a much gentler gesture. This strengthens the differentiation claim (nobody has publicly shown this solved on Quest) but removes any implicit assumption that the core mechanic is already known to be buildable at competitive-grade fidelity. It does not kill H1 — it raises the priority of Stage A/A1–A3 below and fixes the default detection approach to a windowed/buffered estimator instead of raw single-frame contact.

### The hypothesis is stronger than "VR but you break things"

"VR but you break things," or even "MR but you break things on a table," is weak and trivially copyable — some version of the rage-room genre already exists. The claim worth defending is more specific:

> A real everyday surface becomes a competitive physical system. Objects have combinable properties, and mastering those properties lets you build increasingly complex kinetic chains. Destruction is the fantasy; combinatorial physics is the game.

Before resuming any design work, the competitive-landscape pass should decompose the space along four axes, not just ask "has rage-room been done": what interaction already exists, what fantasy already exists, what use of MR already exists, what mastery system already exists — then identify what combination of those four is actually novel here.

### Product requirement carried forward as a hard constraint, not a feature

**The clip has to be good by default.** Not "you can record and share" — the system has to know where the interesting moment was and produce something worth sending without the player doing any editing. If a spectacular run requires camera wrangling, replay-scrubbing, trimming, framing, and exporting before it's shareable, the social behavior the whole loop is built around doesn't happen.

### Current state of H1

Conceptually: strong. Competitively: not yet demonstrated — genre decomposition still to be done. Technically: plausible, not validated. Core interaction: not validated. Fun: completely unvalidated, and not answerable until the above is sequenced correctly.
