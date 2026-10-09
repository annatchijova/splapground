using System;

namespace Slapground.Core
{
    /// <summary>
    /// Decides whether an impact breaks an object. Deliberately does not
    /// recompute collision response - Unity's PhysX already does real rigid-body
    /// collision resolution (via Rigidbody + PhysicsMaterial, see MaterialMapper);
    /// this takes PhysX's own reported impulse magnitude (Collision.impulse.magnitude
    /// in Unity) as input and applies the Fragility data property to it.
    /// </summary>
    public static class BreakageRule
    {
        /// <summary>
        /// Impulse magnitude (Mass * velocity-change units) at which a Fragility=0
        /// (unbreakable) object would still never break in practice, and a
        /// Fragility=1 object breaks on any nonzero impact. A single shared tuning
        /// constant across all objects, not a per-object property - deliberately,
        /// because D2 names exactly four data fields and this is a design constant,
        /// not a fifth one. Expect this to be retuned once Level 1 objects have
        /// real masses and real playtested impact magnitudes; the value here is a
        /// placeholder, not sourced from any measurement.
        /// </summary>
        public const float ReferenceImpulse = 10f;

        public static bool ShouldBreak(float impactImpulseMagnitude, PhysicalProperties properties, float referenceImpulse = ReferenceImpulse)
        {
            if (impactImpulseMagnitude < 0f) throw new ArgumentOutOfRangeException(nameof(impactImpulseMagnitude));
            if (referenceImpulse <= 0f) throw new ArgumentOutOfRangeException(nameof(referenceImpulse));

            if (properties.Fragility <= 0f) return false; // Fragility=0 is exactly unbreakable, not "very high threshold"

            float breakThreshold = referenceImpulse * (1f - properties.Fragility);
            return impactImpulseMagnitude > breakThreshold;
        }
    }
}
