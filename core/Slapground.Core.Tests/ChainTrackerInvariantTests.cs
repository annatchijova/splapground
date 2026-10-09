using System;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    /// <summary>Red team round 3 (invariants): MaxChainLength must never be less than CurrentChainLength, for any impact stream.</summary>
    public class ChainTrackerInvariantTests
    {
        [Fact]
        public void MaxChainLength_IsNeverLessThanCurrentChainLength_AcrossRandomStreams()
        {
            var rng = new Random(7);

            for (int trial = 0; trial < 300; trial++)
            {
                var tracker = new ChainTracker(maxGapSeconds: 1.5f);
                float time = 0f;
                int impactCount = rng.Next(0, 60);

                for (int i = 0; i < impactCount; i++)
                {
                    time += (float)(rng.NextDouble() * 4); // sometimes within gap, sometimes not
                    tracker.RecordImpact(time);

                    Assert.True(tracker.MaxChainLength >= tracker.CurrentChainLength,
                        $"trial {trial}, impact {i}: Max={tracker.MaxChainLength} < Current={tracker.CurrentChainLength}");
                    Assert.True(tracker.CurrentChainLength >= 1);
                }
            }
        }

        [Fact]
        public void ChaosPerMinute_IsNeverNegativeOrGreaterThanTotalImpacts()
        {
            var rng = new Random(13);

            for (int trial = 0; trial < 100; trial++)
            {
                var tracker = new ChainTracker();
                float time = 0f;
                int impactCount = rng.Next(0, 40);

                for (int i = 0; i < impactCount; i++)
                {
                    time += (float)(rng.NextDouble() * 10);
                    tracker.RecordImpact(time);
                    int cpm = tracker.ChaosPerMinute(time);

                    Assert.True(cpm >= 0);
                    Assert.True(cpm <= i + 1, $"trial {trial}: cpm={cpm} exceeds total impacts so far ({i + 1})");
                }
            }
        }
    }
}
