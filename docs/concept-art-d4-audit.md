# Concept art vs. D4 — honesty audit
**Date:** 2026-10-09  **Method:** Abductive Engineering (Firstness/Secondness/Thirdness), product/UX terrain per Anna's request to change focus away from code.
**Scope:** all four images in `docs/concept-art/` against D4 (`README_TECHNICAL.md` section 2: the two-foot-radius Design Guideline).

## Firstness — what the images actually show

- `hero.png`: two hands spread to opposite sides of a visibly wide desk — one reaching for a coffee mug on the left, the other slapping an exploding printer cluster on the right; a corded phone is flying off toward the upper-right corner of frame, well outside the hands' own span.
- `chain-reaction.jpg`: an explicit "Kinetic 9.2 N·s → Printer" chain graphic connects the alarm clock (left edge) to the printer (center-right); the email swarm scatters all the way to the right edge of frame.
- `hud-states.jpeg`: the bottom-right panel draws a "CHAIN REACTION" arc from the corded phone to the printer, again spanning the full visible desk width; the top-right panel does show a drawn rectangular "tracking area" grid on the desk, which is the one element in this set that gestures toward a bounded play surface, though its own extent still looks wider than a tight two-foot radius.
- `async-leaderboard.jpeg`: least literal about desk geometry — it's a floating cartoon "boxing ring" portal above the desk rather than a depiction of objects on the desk surface itself, so it's the weakest evidence either way, not a clear violation.

## Secondness — contrast against the baseline

D4's exact text: "Seated-optimized: Design for seated, stationary use. Limit roomscale, no large physical movement. Apply the airplane seat test: does every interaction work in a two-foot radius?" Three of the four images (hero, chain-reaction, hud-states) depict interaction spans and chain-reaction graphics crossing the full width of a desk — wider than a two-foot radius would allow, and in `chain-reaction.jpg`/`hud-states.jpeg`'s case, drawing the cross-desk span as the explicit selling point of the "kinetic chain" mechanic itself.

## Thirdness — the general rule explaining the pattern

This isn't inconsistent art direction - it's a timing artifact. The concept art was commissioned 2026-10-05, illustrating the brainstorm's original "the whole desk, not a fenced-off box" framing. D4 didn't exist as a binding constraint until the 2026-10-09 rules audit (`README_TECHNICAL.md`'s own D4 entry, and the walk-back of the brainstorm's "40x40cm floating cube" strawman). Image generation has no way to retroactively honor a constraint adopted after it ran. Any commissioned art will drift from a design decision made after it was produced, unless explicitly revisited - this is the same class of staleness risk as a README's own prose going stale after a code change, just in a medium (images) that can't be `grep`'d for the outdated claim.

## Action taken

Added an explicit caveat to `README.md` and `README_ES.md`, right where the art is introduced in the "Where this stands" section: names the date mismatch, states plainly that the art shows a wider play span than the committed design will deliver, and tells a future reader (including future-me) not to reuse this art in submission material without the caveat, and not to commission new art against the old brief.

## What this doesn't decide

Whether to commission new, D4-compliant concept art is a product decision, not a documentation fix - flagged for Anna, not decided here. The honest caveat is the minimum bar (don't let existing material silently overpromise); replacing the art entirely is a separate call about budget/time/whether illustrative art is even still worth commissioning before Level 0's hardware question is answered.
