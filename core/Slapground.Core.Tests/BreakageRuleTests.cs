using System;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class BreakageRuleTests
    {
        [Fact]
        public void Fragility0_NeverBreaks_EvenUnderHugeImpact()
        {
            var unbreakable = new PhysicalProperties(mass: 1f, fragility: 0f, elasticity: 0.5f, articulation: Articulation.Rigid);
            Assert.False(BreakageRule.ShouldBreak(impactImpulseMagnitude: 1_000_000f, unbreakable));
        }

        [Fact]
        public void Fragility1_BreaksOnAnyNonzeroImpact()
        {
            var glass = new PhysicalProperties(mass: 0.1f, fragility: 1f, elasticity: 0.1f, articulation: Articulation.Rigid);
            Assert.True(BreakageRule.ShouldBreak(impactImpulseMagnitude: 0.0001f, glass));
        }

        [Fact]
        public void Fragility1_DoesNotBreakOnExactlyZeroImpact()
        {
            var glass = new PhysicalProperties(mass: 0.1f, fragility: 1f, elasticity: 0.1f, articulation: Articulation.Rigid);
            Assert.False(BreakageRule.ShouldBreak(impactImpulseMagnitude: 0f, glass));
        }

        [Fact]
        public void MidFragility_BreaksAboveThresholdNotBelow()
        {
            var midway = new PhysicalProperties(mass: 1f, fragility: 0.5f, elasticity: 0.5f, articulation: Articulation.Rigid);
            float threshold = BreakageRule.ReferenceImpulse * 0.5f; // (1 - 0.5)

            Assert.False(BreakageRule.ShouldBreak(threshold - 0.01f, midway));
            Assert.True(BreakageRule.ShouldBreak(threshold + 0.01f, midway));
        }

        [Fact]
        public void NegativeImpulse_IsRejected()
        {
            var props = new PhysicalProperties(mass: 1f, fragility: 0.5f, elasticity: 0.5f, articulation: Articulation.Rigid);
            Assert.Throws<ArgumentOutOfRangeException>(() => BreakageRule.ShouldBreak(-1f, props));
        }
    }
}
