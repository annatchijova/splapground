using System.Collections.Generic;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    /// <summary>
    /// Regression guard for the round-5 composition fix: when both a ghost's
    /// checkpoints and a live ChainTracker's impact timestamps are produced via
    /// SessionClock.GetElapsedSeconds(now) - the one correct conversion - the
    /// pacing comparison lines up correctly even when the two sessions started
    /// at wildly different raw engine times.
    /// </summary>
    public class SessionClockElapsedSecondsCompositionTests
    {
        [Fact]
        public void TwoSessionsStartingAtDifferentRawTimes_StillCompareCorrectly_WhenBothUseGetElapsedSeconds()
        {
            // Ghost session: engine started at raw time 1000.
            var ghostClock = new SessionClock();
            ghostClock.Start(1000f);
            var ghostChain = new ChainTracker();

            void GhostImpact(float rawTime) => ghostChain.RecordImpact(ghostClock.GetElapsedSeconds(rawTime));

            GhostImpact(1001f); // elapsed 1s, chain=1
            GhostImpact(1005f); // elapsed 5s, chain=2
            GhostImpact(1010f); // elapsed 10s, chain=3

            var ghostCheckpoints = new List<GhostCheckpoint>
            {
                new GhostCheckpoint(ghostClock.GetElapsedSeconds(1001f), 1),
                new GhostCheckpoint(ghostClock.GetElapsedSeconds(1005f), 2),
                new GhostCheckpoint(ghostClock.GetElapsedSeconds(1010f), 3),
            };
            var ghost = new GhostPacer(ghostCheckpoints);

            // Live session: engine started at a completely different raw time (50000).
            var liveClock = new SessionClock();
            liveClock.Start(50000f);
            var liveChain = new ChainTracker();

            void LiveImpact(float rawTime) => liveChain.RecordImpact(liveClock.GetElapsedSeconds(rawTime));

            LiveImpact(50001f); // elapsed 1s, chain=1
            LiveImpact(50002f); // elapsed 2s, gap=1s (within the default 1.5s MaxGapSeconds) - chain continues to 2

            // At live elapsed time 2s, query the ghost at the SAME elapsed basis.
            float liveElapsedNow = liveClock.GetElapsedSeconds(50002f);
            int delta = ghost.GetPacingDelta(liveElapsedNow, liveChain.CurrentChainLength);

            // Ghost at elapsed=2s is still at chain=1 (its elapsed=5s checkpoint hasn't happened yet).
            // Live chain at elapsed=2s is chain=2. Delta should be +1 - a real, correctly-aligned signal,
            // unlike the round-5 finding's mixed-basis scenario which produced a meaningless number instead.
            Assert.Equal(1, delta);
        }
    }
}
