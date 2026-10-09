# SLAPGROUND — Technical README

[English](./README.md) · [Español](./README_ES.md) · **Technical README**

Full depth: architecture decisions, the interaction-spike protocol, sourced competitive research, unresolved risks, and the level plan toward the submission. Unsoftened — this is the document for someone auditing or extending the project, not introducing it.

Raw, unedited brainstorm (including the Gemini red-team and the walk-back that corrected it) lives in [`BRAINSTORM.md`](./BRAINSTORM.md).

---

## 1. Status, honestly

**H1 (the hypothesis stated in the primary README) is a candidate, not a committed scope.** Nothing described below as "Level 1" has been built. Concept art in `docs/concept-art/` is AI-generated (Gemini/ChatGPT) illustrative material commissioned to communicate the pitch internally — it is not gameplay footage and must never be presented as such.

As of 2026-10-08: the Level 0 spike's C# scripts (`unity/Assets/SlapgroundSpike/`) and a manual setup guide (`docs/setup/QUEST_SETUP.md`) exist, written against Meta's current public Unity API reference. None of it has been opened in a Unity Editor, compiled, built, or run — no Unity Hub, Android tooling, or physical Quest was available in the environment that wrote it. There is still no `.unity` scene, no build, and no device test. See `unity/Assets/SlapgroundSpike/README.md` for exactly what's unverified in that code.

As of 2026-10-09: `core/Slapground.Core` exists — the D2 combinable-properties logic (breakage rule, physics-material mapping, Max Kinetic Chain/CPM scoring, the Desk Sprint + Destroy Everything session clock with pause/resume, a first-pass property catalog for the four Office objects, and D3's highlight-window auto-clip selection) as a plain .NET class library with 55 passing xUnit tests, genuinely run with `dotnet test`, not just written against documentation. A red-team pass the same day found and fixed one real correctness defect, falsified one suspected one, and closed one product/UX gap against the competition's own Design Guidelines — see `docs/red-team-round-1-core.md`. This is the one piece of the project actually verified end-to-end so far, precisely because it needs no Unity Editor, device, or Simulator — see `core/README.md`.

Conceptually: strong. Competitively: researched, not yet fully decomposed. Technically: plausible, spike code drafted but unverified. Core interaction: not validated. Fun: completely unvalidated.

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

**D4 — The two-foot radius is a binding Level 1 design constraint, not an open safety question.**
Forces: this is not a self-imposed caution, it is the competition's own written Design Guideline — "Seated-optimized: Design for seated, stationary use. Limit roomscale, no large physical movement. Apply the airplane seat test: does every interaction work in a two-foot radius?" (official rules, `BRAINSTORM.md` lines 231-233). A 2026-10-09 audit of an earlier ChatGPT-authored build brief found that document's product framing ("throw... increasingly complex chains of destruction" across the desk) never named this constraint, and could drift the pitch toward something the airplane-seat test fails. This was previously tracked only as U4, an open "is it safe" risk — that undersold it: it is a scored eligibility criterion, known on day one, not something Level 0 needs to discover.
Assumption: a two-foot radius is not actually a limitation on the core fantasy — grab/slap/ricochet/chain-reaction all plausibly fit within arm's reach of a seated desk; the earlier brainstorm's "whole desk, not a fenced-off box" framing was reacting to a strawman (a small floating cube *above* the desk), not to the competition's actual, more generous two-foot-radius allowance.
Revisit trigger: none anticipated — this is an external rule, not a hypothesis. If Level 1 playtesting finds the two-foot radius makes chains feel visibly cramped, the fix is combo/trajectory design within the radius, not lobbying to exceed it.

---

## 3. Unresolved risks (carried from the brainstorm)

U1 — is impact without haptics satisfying? U2 — does Quest track a real slap at speed? U3 — does bimanual interaction survive hand-on-hand occlusion? U5 — does MR add real value over VR-in-a-void? U6 — does skill emerge after 30 minutes or just repetition? U7 — are the "emergent" combos real or imagined on paper? U8 — is this different enough from what already exists?

(U4 was "can it be physically safe without caging the fantasy" — resolved out of this list by D4 above: the radius is a known competition rule, not an open question, so there is nothing left to discover about *whether* to bound the play space, only about designing well inside that bound.)

U2 and U3 gate the project: if a fast double-hand slap loses tracking or the hands occlude each other at the moment of impact, there is no fallback design — the pitch is physical, two-handed, and fast by construction. Everything else (scoring, clips, roadmap) is downstream of those two holding up on real hardware.

Where the brainstorm's own self-critique was right, and stays frozen until U1–U3 answer: the 40×40cm floating interaction cube and "bank-shots only downward" were reacting to a strawman tighter than the competition actually requires — the real constraint (D4) is a two-foot radius around a seated player, materially larger and more natural than a floating cube fixed above the keyboard. Any numeric claim about Quest camera Hz, motion blur, or Scene Understanding precision that originated from a model (Gemini or otherwise) is a claim, not a fact, until checked against Meta's current developer documentation.

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

### 4.1 Testing without a physical headset

Corrected 2026-10-09: an earlier version of this document implicitly assumed Stage A requires a physical Quest from the first run. It doesn't, and the competition's own rules say so directly — the demo-video requirement explicitly accepts footage "as viewed on a Meta VR device, via XR Simulator, or another equivalent emulator," and Meta's own competition messaging for this cycle states Quest/Glasses hardware target general availability is spring 2027, so the entire developer tooling story this cycle is built around not requiring the device in hand: **Meta XR Simulator** (`com.meta.xr.simulator`), a desktop OpenXR runtime that runs inside Unity Play Mode.

What it actually buys, checked against Meta's current Simulator docs (2026-10-09) rather than assumed:
- Runs in the Editor via Play Mode — no Android Build Support module, no APK, no USB pairing needed to get this far. This is strictly less setup than `docs/setup/QUEST_SETUP.md`'s on-device path, and should be the *first* thing tried once the project and SDK packages exist — it validates that the spike scripts actually compile and run, independent of any hardware question.
- Hand input is driven by keyboard/mouse as one of four discrete poses (aim/poke/pinch/grab, keys 1-4; left mouse = pinch) — this is pose simulation, not a continuous tracked gesture, and will not produce a realistic fast-slap velocity profile.
- v207 (current) adds **camera-driven hand tracking**: simulated hands can be driven from a webcam instead of keyboard poses, per-hand. This is the closest no-headset approximation to an actual physical slap motion available right now, but a webcam's tracking volume, framerate, and occlusion behavior are not the same sensor stack as Quest's onboard cameras — a result here is evidence about the *code path*, not about Quest's real tracking fidelity.

What it does **not** answer: U2/U3 (does Quest's own hand-tracking stack survive a fast two-handed slap at competitive fidelity). That is a claim about Quest's specific onboard sensors and firmware, which no desktop simulator — webcam-driven or not — actually runs. `OVRHand.HandConfidence` under Simulator should be assumed to report whatever the simulator's pose/webcam driver feeds it (likely a constant high-confidence value for keyboard poses), not a real tracking-fidelity signal — unverified in either direction since no Simulator install was available to check directly, but do not report a clean Simulator run as "tracking confidence held up," because the thing being measured is different.

Practical sequencing this changes: run Stage A first in Simulator (keyboard-pose mode, then webcam-driven mode if installed) purely to shake out code bugs and get the logging pipeline producing real CSVs cheaply and repeatedly; treat any result from that as "the instrumentation works," not as "the hardware question is answered." The real U2/U3 verdict still needs either real Quest hardware or, at minimum, an explicit caveat in any submission material that webcam-driven Simulator footage is a proxy, not proof, of on-device fidelity.

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

## 7. Stack — language/engine decision record

Per house method (`language-selection`): the language is a hypothesis explaining the constraints, not a default. Re-derived on 2026-10-05, after Anna flagged that this project — unlike the rest of her portfolio — starts at zero across Unity, C#, XR, hand tracking, MR, and 3D physics simultaneously, which made the original brainstorm's justification ("the AI already knows C# well") worth re-examining rather than re-asserting.

**Problem shape:** a real-time spatial-interaction application — hand-tracking input, bimanual contact physics, MR passthrough collision against real room geometry. Not a security/determinism problem (no sealed decision path, no hostile input parsing); the dominant forces are SDK/engine maturity for contact-physics-on-a-real-surface and solo-developer iteration speed under a fixed deadline.

**Language imposed?** Partially. Meta defines the supported Quest development paths: [Unity → C#](https://developers.meta.com/horizon/develop/unity/), [Unreal → C++/Blueprints](https://developers.meta.com/horizon/develop/unreal/), [Native OpenXR → C/C++](https://developers.meta.com/horizon/develop/native/), [Spatial SDK → Kotlin](https://developers.meta.com/horizon/develop/spatial-sdk/), [WebXR/IWSDK → JavaScript/TypeScript](https://developers.meta.com/horizon/develop/web/). Arbitrary language choice isn't available, but more than one of these paths is genuinely viable — so the decision below compares them, rather than treating any one as given.

**Decisive forces:**
- The gating risk for the whole project (U2/U3, §3) is contact-physics/hand-tracking fidelity on real hardware — whichever path has the most mature tooling for exactly that reduces confounds during the Level 0 spike.
- Both titles Meta itself names as references (§5) — Hand Physics Lab, Table Troopers — are native-engine builds. The engineering patterns already researched and locked into D1/D3 (windowed impact detection, room-geometry physics split) come from that world; porting them to a different stack is translation work with no public precedent found yet.
- The maintainer (Anna) has zero prior fluency in every XR-capable candidate equally — this is **not** a fluency tie-breaker situation, because no candidate starts ahead on that axis.
- AI assistance absorbs syntax/boilerplate/API-lookup cost (per `language-selection`'s framing) but not architecture, on-device debugging, or judgment calls like "does this feel like 80ms of lag" — the real bottleneck in this project is hands-on hardware judgment, which is identical regardless of language chosen.
- [Unity](https://unity.com/) + [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) specifically carries far more public training-corpus density for Meta hand-tracking integration than Unity+WebXR or Unreal+Blueprint equivalents, which lowers friction specifically on the delegable cost category (syntax/boilerplate), not on the non-delegable one.

**Candidates surfaced:**

```
Candidate: TypeScript / WebXR (IWSDK) — the strongest rival
Why it entered: it is Anna's actual existing stack (annaconda, koine, locoporcolt,
                 the COMPASS frontend) and a Meta-supported competition path.
What it buys: zero new-language and new-IDE cost; a deploy pipeline she already
              owns (Vercel/GitHub Pages) instead of Meta's Release Channel tooling;
              collapses "six unfamiliar domains" down to "three" (hand-tracking,
              MR, physics) by removing language and tooling from the pile.
What it costs: less mature room-geometry-aware contact physics and hand-tracking
              fidelity than Meta's native Interaction SDK, as far as currently
              available documentation and sample titles show.
Where the guarantee ends: no public evidence yet on whether IWSDK's hand-tracking
              API exposes confidence/joint data comparable to HandConfidence/
              FingerConfidences (§4) with comparable fidelity — unverified, not
              assumed absent.
Why not chosen: the two reference titles and all the contact-physics engineering
              precedent found in §5 are native-engine, not WebXR — this candidate
              trades the gating-risk-relevant tooling maturity for language
              familiarity, and the bottleneck this project actually has
              (hardware judgment, not syntax) is the one axis language
              familiarity doesn't help with.
How we'd verify: a fast, direct comparison during Level 0 — if IWSDK's hand API
              turns out to expose comparable confidence data with materially less
              setup friction, this reopens.
```

```
Candidate: Unreal Engine + C++/Blueprints
Why it entered: Meta-supported path; Blueprint's visual scripting could lower the
              zero-background barrier; strong native physics/rendering fidelity.
What it buys: Blueprint prototyping without hand-written C++ for simple behavior.
What it costs: Meta's own developer guidance and the original brainstorm's own
              comparison already rank it "possible, but unnecessarily heavy" for
              this scope; the D2 data-driven combinable-properties object system
              needs real code eventually, past what Blueprint-only comfortably
              expresses.
Why not chosen: loses on scope-appropriate weight — heavier engine/toolchain for
              no force that specifically favors it here.
```

```
Candidate: Native OpenXR (C/C++)
Why it entered: it's Meta's lowest-level officially supported path, full control.
What it costs: reimplements hand-tracking interaction, physics, and rendering
              from near-zero — Meta's own documentation states implementing hand
              interactions manually is "considerably more difficult" than using
              the Interaction SDK.
Why not chosen: wrong shape for a solo 44-day build; eliminated immediately, not
              a close call.
```

**Chosen:** [Unity 6](https://unity.com/) (6000.0.66f2+) + [C#](https://learn.microsoft.com/en-us/dotnet/csharp/), [OpenXR](https://www.khronos.org/openxr/) via [Unity OpenXR + Meta OpenXR](https://developers.meta.com/horizon/documentation/unity/unity-openxr/), the [Meta XR All-in-One SDK](https://developers.meta.com/horizon/downloads/package/meta-xr-sdk-all-in-one-upm/) (`com.meta.xr.sdk.all`, v207 — this is literally the package the competition's own rules name under "Suggested SDKs: Unity: Meta XR SDKs v81+ (v207) All-in-One XR"; it bundles Core SDK and Interaction SDK rather than installing them as two separate packages, which is what an earlier pass at this document had assumed), plus the [Meta XR Simulator](https://developers.meta.com/vr/downloads/package/meta-xr-simulator-windows/) (`com.meta.xr.simulator`, section 4.1) for no-headset iteration, Unity Physics.

**Why:** the decisive force is SDK/engine maturity for the project's actual gating risk (contact-physics and hand-tracking fidelity, §3's U2/U3) and direct continuity with the engineering precedent already researched in §5 — not language familiarity, which is absent equally across every XR-capable candidate and which AI assistance substantially offsets for the parts of the cost (syntax, boilerplate, API lookup) that it actually offsets.

**Guarantees relied on:** grab/poke/pose detection — provided by Meta XR Interaction SDK, not by C# itself. Cross-runtime portability of the hand-tracking/MR extensions — provided by OpenXR plus Meta's OpenXR layer, not by Unity alone.

**Guarantees NOT relied on:** that C# or Unity make the project easier to *learn* for a zero-background developer than WebXR would — they don't; that choice trades a steeper tooling/domain learning curve for SDK maturity on the hardest risk, honestly, not for free.

**Accepted cost:** Anna starts genuinely at zero in Unity, C#, the Unity editor workflow, and Meta's XR tooling simultaneously — mitigated by (a) implementation work going through the AI/agent layer per the working arrangement below, and (b) the cost being concentrated on exactly the domains (hand-tracking, MR, physics judgment) that no candidate language would have spared her anyway.

**Reopen if:** Level 0 (§4) finds IWSDK/WebXR exposes hand-tracking confidence data at comparable fidelity with materially less setup friction than the native path — or finds the native Interaction SDK doesn't actually expose the confidence/joint metrics §4 assumes.

**Working arrangement:** the code is written collaboratively by the agent/model layer: Anna owns architecture calls, acceptance criteria, and on-device judgment ("does this feel right"); implementation (C#, Unity wiring, SDK integration, tests) is agent-assisted throughout, with Anna learning the domain by reading and directing rather than typing syntax from memory first — consistent with how `language-selection` frames AI's actual cost reduction (syntax/boilerplate/API lookup) versus what it doesn't reduce (architecture, debugging, on-device judgment).

Full original stack reasoning and the brainstorm-stage rejected-alternatives table are in [`BRAINSTORM.md`](./BRAINSTORM.md). Full submission requirements (division, hard requirements, what to submit) are also captured there — this document does not duplicate them.
