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

Keep H1 as a named candidate, not a committed scope. Don't resume feature design (Kitchen Rage, safety cubes, bank-shot physics) until the two gating unknowns — U2 (slap-speed tracking) and U3 (bimanual occlusion) — have an answer from a real Quest device, not from a model's prediction. The right next spend of effort is the cheapest experiment that can kill the idea: one hand grabs a cube, the other slaps it, on-device, nothing else built yet. If that doesn't feel good, no amount of combo design saves it — and that's a real result, not a failure to report.

In parallel, and before writing anything submission-facing: pull Meta's current Hands & Eyes documentation and look at "Hands Physics Lab" and "Table Troopers" directly — both are listed on the competition page itself as sample use cases and may already occupy adjacent ground that the submission needs to explicitly differentiate against.
