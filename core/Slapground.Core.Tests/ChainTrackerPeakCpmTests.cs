using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class ChainTrackerPeakCpmTests
    {
        [Fact]
        public void PeakChaosPerMinute_StartsAtZero()
        {
            var tracker = new ChainTracker();
            Assert.Equal(0, tracker.PeakChaosPerMinute);
        }

        [Fact]
        public void PeakChaosPerMinute_TracksTheHighestValueSeen_EvenAfterItDrops()
        {
            var tracker = new ChainTracker();

            // Burst of 5 impacts close together.
            for (int i = 0; i < 5; i++)
            {
                float t = i * 1f;
                tracker.RecordImpact(t);
                tracker.ChaosPerMinute(t);
            }
            Assert.Equal(5, tracker.PeakChaosPerMinute);

            // Long quiet stretch - CPM decays back toward 0, but the peak should stick.
            int quietCpm = tracker.ChaosPerMinute(500f);
            Assert.Equal(0, quietCpm);
            Assert.Equal(5, tracker.PeakChaosPerMinute);
        }

        [Fact]
        public void PeakChaosPerMinute_NeverUpdatedWithoutCallingChaosPerMinute()
        {
            var tracker = new ChainTracker();
            // 10 impacts recorded, but ChaosPerMinute never polled.
            for (int i = 0; i < 10; i++)
            {
                tracker.RecordImpact(i * 0.1f);
            }
            Assert.Equal(0, tracker.PeakChaosPerMinute); // documented behavior: peak only updates on poll
        }
    }
}
