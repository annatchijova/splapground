# Fun and visual-direction brainstorm — 2026-10-09

**Status: ideas, not commitments.** Same convention as `BRAINSTORM.md` — candidate
material to question and improve, not scope that's been decided. Nothing here
should be built before Stage A answers whether the core gesture even works; this
is useful to have written down *now* so it's ready to evaluate once that happens,
not a signal to start building any of it today.

Every idea below is tagged:
- **[D2-compatible]** — fits the existing data-driven property system (Mass/
  Fragility/Elasticity/Articulation) as new data, no architecture change.
- **[new mechanic]** — needs its own design pass and playtesting before it's
  more than a guess; don't build from this doc alone.
- **[visual direction]** — concerns look/feel, not game logic; doesn't depend on
  Level 0/1 code at all, could start whenever art work does.

---

## Visual direction

**The core MR problem this project will hit: passthrough video is noisier and
lower-contrast than CG, so photoreal virtual objects tend to look like a floating
sticker rather than something actually sitting on the desk.** This isn't specific
to SLAPGROUND — it's a known MR art-direction lesson — but it's worth deciding on
purpose rather than discovering it after modeling four photoreal objects.

- **[visual direction] Stylized over photoreal.** Flat-shaded or cel-shaded
  objects with a consistent thick rim-light outline read more clearly against a
  real camera feed than an attempt at photorealism, and a confident "obviously not
  real, deliberately cartoonish" look also sidesteps the uncanny-valley risk of a
  near-photoreal alarm clock that moves slightly wrong.
- **[D2-compatible] A shared damage-state visual language, driven by Fragility.**
  Three damage stages (pristine → cracked/sparking → exploded-fragments), with
  the same crack-decal and spark-particle system reused across every object
  regardless of which world it's from. Fragility already determines *when* an
  object breaks (`BreakageRule`) — the same value could gate which damage stage
  is showing, so a Level 2+ Kitchen/Workshop object instantly reads to a player
  who already learned the pattern from the Office world, without new per-object
  VFX work. This is genuinely just reusing existing data for a new purpose, not
  new architecture.
- **[visual direction] Contact shadows, not floating objects.** A simple blob
  shadow anchored to the real desk surface under each object helps it read as
  "sitting on your desk" rather than "floating in space," and incidentally helps
  a player judge real-world reach before swinging — relevant to playing safely
  within D4's two-foot radius, not just aesthetics.
- **[visual direction] Rethink the chain-reaction graphic, given the D4 audit.**
  `docs/concept-art-d4-audit.md` flagged the concept art's cross-desk lightning-
  bolt chain graphic as visually promising a wider play space than D4 allows.
  A direct fix once real UI art gets made: render chain feedback as a localized
  ripple/pulse bursting outward from each impact point instead of a line drawn
  between two distant points — same "this hit triggered that hit" readability,
  without implying the objects are far apart.

## Fun and depth, without expanding past the two-foot radius

D4 means depth can't come from spreading objects wider. It can come from other
axes:

- **[new mechanic] Vertical layering instead of lateral spread.** Stack objects
  at different heights within arm's reach (a floating printer above a desk-level
  alarm clock, say) rather than side-by-side across the desk. Directly answers
  "how do we keep the destruction fantasy dramatic without violating D4" with a
  concrete alternative, rather than just accepting a smaller-feeling play space.
  Needs a real playtest — vertical reach inside a two-foot radius while seated
  has its own ergonomics that aren't obvious on paper.
- **[new mechanic] A "pressure" build-up on neglected objects.** An object left
  unhit for a while slowly builds a small score multiplier (framed as mounting
  office stress), resetting the moment it's hit — rewards picking targets
  deliberately instead of just flailing at whatever's closest. Would reuse the
  same time-tracking pattern already in `Slapground.Core` (`VelocityBuffer`/
  `ChainTracker` already do "something changing over elapsed time"), so it's not
  architecturally novel, but the actual numbers (how fast the multiplier builds,
  whether it's fun or just stressful) need real playtesting, not a guess now.
- **[verify, don't build] Chain-triggered object-to-object transitions might
  already be free.** The brief's own pitch ("an alarm clock impact triggers a
  printer explosion that scatters an email swarm," `docs/concept-art/chain-
  reaction.jpg`) sounds like a bespoke scripted sequence, but if debris from
  object A has enough momentum to hit object B within `ChainTracker`'s own gap
  window, the system may already produce this for free, with no new code — worth
  explicitly testing for once Level 1 exists, before writing any object-specific
  trigger logic.
- **[new mechanic] A clear "Destroy Everything" escalation cue.** The 10-second
  finale (`SessionClock`) currently has no distinct feedback of its own — audio/
  lighting/particle intensity ramping specifically in those last seconds (Stage
  B territory, already gated behind Stage A per the existing protocol) would
  help the "one bus stop test" ("a complete moment in under 10 minutes") land
  as a deliberate climax rather than the clock just running out.

## Onboarding — "fast cold start" (the other named Design Guideline)

- **[new mechanic] Teach the verb before the systems.** First-ever session:
  spawn exactly one object, one giant "SLAP ME" callout, no HUD, no score, no
  chain counter. Land the one core gesture with zero cognitive load before
  showing combos/CPM/ghost trail at all — standard onboarding practice, and a
  direct, concrete way to satisfy "fast cold start" rather than leaving it as an
  abstract goal.

## What this doesn't decide

None of this is scheduled. The vertical-layering idea in particular deserves a
real conversation before anything gets built on top of it, since it's a
non-obvious enough change to the physical play pattern that it should go through
the same scrutiny D4 itself got, not get adopted just because it's written down
here first.
