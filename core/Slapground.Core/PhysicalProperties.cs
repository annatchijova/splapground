using System;

namespace Slapground.Core
{
    /// <summary>
    /// D2's data-driven combinable-properties system (README_TECHNICAL.md section 2):
    /// mass, fragility, articulation, elasticity as data, not per-object hardcoded
    /// behavior. Exactly these four fields, per the decision record - adding a fifth
    /// property here is a scope change to D2, not a casual extension.
    /// </summary>
    public readonly struct PhysicalProperties
    {
        /// <summary>Kilograms. Feeds Unity's own Rigidbody.mass directly - not reinterpreted here.</summary>
        public readonly float Mass;

        /// <summary>0 = unbreakable, 1 = breaks under any nonzero impact. See BreakageRule.</summary>
        public readonly float Fragility;

        /// <summary>Restitution coefficient, 0..1. Maps directly to a PhysicsMaterial's bounciness - see MaterialMapper.</summary>
        public readonly float Elasticity;

        public readonly Articulation Articulation;

        public PhysicalProperties(float mass, float fragility, float elasticity, Articulation articulation)
        {
            if (mass <= 0f) throw new ArgumentOutOfRangeException(nameof(mass), mass, "Mass must be positive.");
            if (fragility < 0f || fragility > 1f) throw new ArgumentOutOfRangeException(nameof(fragility), fragility, "Fragility must be in [0,1].");
            if (elasticity < 0f || elasticity > 1f) throw new ArgumentOutOfRangeException(nameof(elasticity), elasticity, "Elasticity must be in [0,1].");

            Mass = mass;
            Fragility = fragility;
            Elasticity = elasticity;
            Articulation = articulation;
        }
    }
}
