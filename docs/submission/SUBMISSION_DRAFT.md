# Meta VR Start Developer Competition 2026 — submission draft

**Status: template, not ready to file.** The rules require a playable build (APK on
a "Competition" release channel + Invite URL) and a demo video before any of this
can actually be submitted — neither exists yet (`README_TECHNICAL.md` section 1).
This fills in everything that's honestly true *today* and marks everything that
depends on Level 1 actually shipping with `[TODO once X exists]`. Don't file this
as-is; finish the brackets first.

## Submission name
SLAPGROUND

## Submission tagline (140 characters max)

Two options, both under the limit (counted with `wc -c`, not eyeballed):

1. **"Hands-first office destruction on your real desk — combinable physics, kinetic chains, zero controllers."** (106 chars) — leads with the two things judges are explicitly scoring (hands-first, original mechanic), reads like a feature list.
2. **"A bad day at the office becomes a physics playground on your real desk - slap, bounce, chain-combo, hands only."** (111 chars) — leads with the catharsis hook from the main README, closer to the brand voice already in `docs/concept-art/hero.png` ("Your desk. Your rules. Hit reality.").

Pick one once there's a build to back it up — a tagline promising "kinetic chains" before Stage A has confirmed the slap gesture even tracks reliably is a claim the submission shouldn't make yet.

## Track
**Gaming**

## Division
**New Experience** — the repo's first commit is 2026-10-05, after the competition window opened 2026-09-24; no pre-existing codebase or shipped title.

## Description (suggested 500 words or less)

> [TODO once Level 1 exists and has been played]: the paragraphs below are a
> structural draft — inspiration and how-it-was-built are honestly written now;
> the results/future-plans paragraphs have bracketed placeholders for numbers
> that don't exist yet (max chain achieved in testing, final session length,
> whether Stage A actually held up on hardware). Don't fill those with invented
> numbers — leave them until there's a real session to report.

**Inspiration.** Everyone's had the day where the printer jams, the inbox won't
stop, and the phone won't shut up. SLAPGROUND asks: what if your actual desk
became where you took that out — not a generic VR room, but your real table, with
the objects that caused the bad day floating over it, reacting to genuine physical
properties instead of a scripted animation. The competitive hook isn't a live
opponent (that's visual clutter in a seated, hands-only game) — it's a ghost trail
of your own best run and a chaos-per-minute score, the same loop that makes a
rhythm game or a speedrun addictive, applied to destruction.

**How it was built.** [TODO once Level 0 reports a real result]: the honest
version of this paragraph depends on what Stage A actually finds. If it holds —
"Before writing gameplay code, we validated the one thing that could have killed
this idea outright: whether Quest's hand tracking survives a fast, two-handed slap
at all, staged across three gesture configurations (A1/A2/A3) to tell tracking
loss apart from a bad physics response. Once that held, [N]% of the game's logic —
object properties, breakage rules, the session clock, the scoring and
highlight-clip selection — was built and unit-tested (102 passing tests) entirely
before touching a Unity Editor, because that logic doesn't need a headset to be
correct, only to be *felt*." If it didn't hold: this paragraph instead honestly
describes what was learned and how the design adapted — a disclosed limitation
reads better to judges than a silently abandoned claim.

**Future plans for improvement.** Kitchen Rage and Workshop Chaos — new object
sets on the same data-driven property system (mass/fragility/elasticity/
articulation), not a rewrite. [TODO: pull 2-3 concrete ideas from
`docs/brainstorm-fun-and-visuals-2026-10-09.md` once some of them have actually
been tried, rather than listing every brainstormed idea as a roadmap promise].

## Adapted/Significantly Updated summary
Not applicable — New Experience division.

## Describe how Hand Interactions are implemented (optional)

Worth including, and worth being specific rather than generic, since this field
is exactly where the project's actual technical discipline shows: "Impact
detection uses a windowed/buffered velocity estimator rather than trusting the
exact contact frame, following the same pattern Table Troopers documented for a
much gentler gesture (Meta's own named sample use case for this competition,
`README_TECHNICAL.md` section 5). [TODO: once measured on real hardware, state
the actual error-reduction number here instead of the synthetic-test number —
`docs/red-team-round-2-core.md` measured ~4x on a synthetic glitch, which is
evidence the mechanism works as designed, not evidence of what Quest's own
tracking does]."

## Target launch date
[TODO — Anna's call, not determined by this document]

## Team
[TODO — names/emails/roles if anyone besides Anna is credited]
