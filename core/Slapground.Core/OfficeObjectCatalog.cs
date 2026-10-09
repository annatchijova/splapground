using System;
using System.Collections.Generic;

namespace Slapground.Core
{
    /// <summary>The four Level 1 Office objects named in README.md/README_TECHNICAL.md section 2.</summary>
    public enum OfficeObjectKind
    {
        AlarmClock,
        Printer,

        /// <summary>One archetype instance; "the swarm" is many of these spawned by the Unity-side scene, not a new core type.</summary>
        EmailNotification,
        CordedPhone,
    }

    /// <summary>
    /// Concrete PhysicalProperties for the four Office objects. Every value here is
    /// a first-pass design placeholder reasoned from the product brief's own
    /// descriptions (README.md: "something liviano puede rebotar violentamente;
    /// algo pesado puede absorber varios impactos..."), not from any playtest or
    /// measurement - expect to retune all of it once Level 1 has real play
    /// sessions, per D2's own revisit trigger.
    /// </summary>
    public static class OfficeObjectCatalog
    {
        private static readonly Dictionary<OfficeObjectKind, PhysicalProperties> Catalog = new Dictionary<OfficeObjectKind, PhysicalProperties>
        {
            // Small, light, hard plastic - rattles and bounces when struck, cracks
            // open after a few good hits rather than one light tap.
            [OfficeObjectKind.AlarmClock] = new PhysicalProperties(
                mass: 0.3f, fragility: 0.4f, elasticity: 0.5f, articulation: Articulation.Rigid),

            // Heavy office hardware. Low Fragility so it survives several hits before
            // the "printer explosion" moment the concept art depicts (docs/concept-art),
            // low Elasticity because a heavy object thuds and slides rather than
            // bouncing - the "needs a few hits before it goes flying" feel is mostly
            // the mass itself resisting velocity change under PhysX, not this value.
            [OfficeObjectKind.Printer] = new PhysicalProperties(
                mass: 8f, fragility: 0.3f, elasticity: 0.15f, articulation: Articulation.Rigid),

            // Near-weightless, dismissed on the lightest touch - a notification is
            // meant to pop, not survive a hit. High Elasticity so a swarm of these
            // scatters energetically rather than clumping.
            [OfficeObjectKind.EmailNotification] = new PhysicalProperties(
                mass: 0.05f, fragility: 0.9f, elasticity: 0.7f, articulation: Articulation.Rigid),

            // README.md: "un teléfono con cable puede convertirse en una boleadora" -
            // Flail is the whole point of this object. Moderate mass/fragility/
            // elasticity; the interesting behavior comes from Articulation, not these.
            [OfficeObjectKind.CordedPhone] = new PhysicalProperties(
                mass: 0.6f, fragility: 0.35f, elasticity: 0.4f, articulation: Articulation.Flail),
        };

        public static PhysicalProperties Get(OfficeObjectKind kind)
        {
            if (!Catalog.TryGetValue(kind, out var properties))
                throw new ArgumentOutOfRangeException(nameof(kind), kind, "No catalog entry for this object kind.");
            return properties;
        }
    }
}
