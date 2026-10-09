using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    /// <summary>
    /// Red-team round 1 on ChainTracker: RecordImpact enforces non-decreasing
    /// timestamps, but ChaosPerMinute(now) took no such guard - a caller could
    /// query with a stale "now" earlier than impacts already recorded and get a
    /// silently wrong count instead of an error. Confirmed by induction below
    /// before the fix; kept as a regression test after.
    /// </summary>
    public class ChainTrackerRedTeamTests
    {
        [Fact]
        public void ChaosPerMinute_WithStaleNow_ThrowsInsteadOfSilentlyMiscounting()
        {
            var tracker = new ChainTracker();
            tracker.RecordImpact(100f);

            // Querying "now" = 50, before the impact we just recorded at 100.
            Assert.Throws<System.ArgumentOutOfRangeException>(() => tracker.ChaosPerMinute(50f));
        }
    }
}
