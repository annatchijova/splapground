using System;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    /// <summary>
    /// Red team round 3 (invariants): BreakageRule's whole purpose is that
    /// Fragility orders breakability. If a less-fragile object breaks under some
    /// impulse, a more-fragile object at the same impulse must break too - that's
    /// not a side detail, it's the property the "D2 is independent data, not
    /// coupled to mass" design relies on being true. Fuzzed, not assumed.
    /// </summary>
    public class BreakageRuleInvariantTests
    {
        [Fact]
        public void Monotonic_HigherFragility_NeverBreaksLessOftenThanLowerFragility()
        {
            var rng = new Random(20261009);

            for (int trial = 0; trial < 500; trial++)
            {
                float f1 = (float)rng.NextDouble();
                float f2 = (float)rng.NextDouble();
                float lo = Math.Min(f1, f2);
                float hi = Math.Max(f1, f2);

                float impulse = (float)(rng.NextDouble() * BreakageRule.ReferenceImpulse * 1.5);

                var lessFragile = new PhysicalProperties(mass: 1f, fragility: lo, elasticity: 0.5f, articulation: Articulation.Rigid);
                var moreFragile = new PhysicalProperties(mass: 1f, fragility: hi, elasticity: 0.5f, articulation: Articulation.Rigid);

                bool lessFragileBreaks = BreakageRule.ShouldBreak(impulse, lessFragile);
                bool moreFragileBreaks = BreakageRule.ShouldBreak(impulse, moreFragile);

                if (lessFragileBreaks)
                {
                    Assert.True(moreFragileBreaks,
                        $"trial {trial}: impulse={impulse}, lowerFragility={lo} broke but higherFragility={hi} did not - monotonicity violated");
                }
            }
        }

        [Fact]
        public void Monotonic_HigherImpulse_NeverUnbreaksAFixedObject()
        {
            var rng = new Random(42);
            for (int trial = 0; trial < 500; trial++)
            {
                var props = new PhysicalProperties(
                    mass: 1f,
                    fragility: (float)rng.NextDouble(),
                    elasticity: 0.5f,
                    articulation: Articulation.Rigid);

                float i1 = (float)(rng.NextDouble() * BreakageRule.ReferenceImpulse * 2);
                float i2 = (float)(rng.NextDouble() * BreakageRule.ReferenceImpulse * 2);
                float lo = Math.Min(i1, i2);
                float hi = Math.Max(i1, i2);

                bool breaksAtLow = BreakageRule.ShouldBreak(lo, props);
                bool breaksAtHigh = BreakageRule.ShouldBreak(hi, props);

                if (breaksAtLow)
                {
                    Assert.True(breaksAtHigh,
                        $"trial {trial}: fragility={props.Fragility}, broke at impulse={lo} but not at higher impulse={hi}");
                }
            }
        }
    }
}
