using System;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class ChainTrackerTests
    {
        [Fact]
        public void FirstImpact_StartsChainOfOne()
        {
            var tracker = new ChainTracker(maxGapSeconds: 1.5f);
            tracker.RecordImpact(0f);
            Assert.Equal(1, tracker.CurrentChainLength);
            Assert.Equal(1, tracker.MaxChainLength);
        }

        [Fact]
        public void ImpactsWithinGap_ExtendTheChain()
        {
            var tracker = new ChainTracker(maxGapSeconds: 1.5f);
            tracker.RecordImpact(0f);
            tracker.RecordImpact(1.0f);
            tracker.RecordImpact(2.0f);

            Assert.Equal(3, tracker.CurrentChainLength);
            Assert.Equal(3, tracker.MaxChainLength);
        }

        [Fact]
        public void GapLongerThanThreshold_ResetsCurrentChain_ButKeepsMax()
        {
            var tracker = new ChainTracker(maxGapSeconds: 1.5f);
            tracker.RecordImpact(0f);
            tracker.RecordImpact(1.0f);
            tracker.RecordImpact(2.0f); // chain of 3

            tracker.RecordImpact(10.0f); // gap of 8s, far beyond 1.5s threshold

            Assert.Equal(1, tracker.CurrentChainLength);
            Assert.Equal(3, tracker.MaxChainLength);
        }

        [Fact]
        public void GapExactlyAtThreshold_StillCountsAsContinuing()
        {
            var tracker = new ChainTracker(maxGapSeconds: 1.5f);
            tracker.RecordImpact(0f);
            tracker.RecordImpact(1.5f);
            Assert.Equal(2, tracker.CurrentChainLength);
        }

        [Fact]
        public void DecreasingTimestamp_Throws()
        {
            var tracker = new ChainTracker();
            tracker.RecordImpact(5f);
            Assert.Throws<ArgumentOutOfRangeException>(() => tracker.RecordImpact(4.9f));
        }

        [Fact]
        public void ChaosPerMinute_CountsOnlyTrailingSixtySeconds()
        {
            var tracker = new ChainTracker();
            tracker.RecordImpact(0f);
            tracker.RecordImpact(30f);
            tracker.RecordImpact(90f); // now 90s; impact at 0s is 90s old, outside the 60s window

            Assert.Equal(2, tracker.ChaosPerMinute(90f)); // the 30s and 90s impacts
        }

        [Fact]
        public void ChaosPerMinute_WithNoImpacts_IsZero()
        {
            var tracker = new ChainTracker();
            Assert.Equal(0, tracker.ChaosPerMinute(100f));
        }
    }
}
