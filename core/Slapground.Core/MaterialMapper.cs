namespace Slapground.Core
{
    /// <summary>Physics-material parameters to hand to Unity's PhysicsMaterial - the mapping lives in one place instead of being re-derived per object prefab.</summary>
    public readonly struct PhysicsMaterialParams
    {
        public readonly float Bounciness;
        public readonly float DynamicFriction;
        public readonly float StaticFriction;

        public PhysicsMaterialParams(float bounciness, float dynamicFriction, float staticFriction)
        {
            Bounciness = bounciness;
            DynamicFriction = dynamicFriction;
            StaticFriction = staticFriction;
        }
    }

    public static class MaterialMapper
    {
        private const float DefaultFriction = 0.6f; // Unity's own PhysicsMaterial default
        private const float FlailFriction = 0.2f;   // a flailed end should slide/swing, not stick

        public static PhysicsMaterialParams ToPhysicsMaterialParams(PhysicalProperties properties)
        {
            float friction = properties.Articulation == Articulation.Flail ? FlailFriction : DefaultFriction;
            return new PhysicsMaterialParams(
                bounciness: properties.Elasticity,
                dynamicFriction: friction,
                staticFriction: friction);
        }
    }
}
