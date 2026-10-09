using System.Collections.Generic;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    /// <summary>
    /// Red team round 5 (composition): ChainTracker and GhostPacer both take
    /// caller-supplied timestamps, with no code anywhere enforcing they share a
    /// time origin. If a ghost's timeline is recorded on one basis (e.g. raw
    /// engine time across a whole play session) and the live chain is recorded
    /// on another (session-relative elapsed time, reset to 0 each SessionClock.
    /// Start()), the pacing comparison doesn't crash - it silently lies, because
    /// it has no way to detect the mismatch.
    /// </summary>
    public class GhostPacerTimeOriginCompositionTests
    {
        [Fact]
        public void MixedTimeBases_SilentlyProducesMisleadingPacing_NotAnError()
        {
            // Ghost recorded using "raw engine time" from a session that started
            // at engine time 1000 (a realistic value if the app had been running
            // a while before that play session started).
            var ghostCheckpoints = new List<GhostCheckpoint>
            {
                new GhostCheckpoint(1001f, 1), // engine time 1001 = 1s into that play
                new GhostCheckpoint(1005f, 3), // engine time 1005 = 5s in
                new GhostCheckpoint(1010f, 6), // engine time 1010 = 10s in, chain of 6
            };
            var ghost = new GhostPacer(ghostCheckpoints);

            // Current session uses SessionClock-relative elapsed time (correctly
            // reset to 0 at Start()) - the "correct" convention. The player is
            // actually doing BETTER than the ghost at the same real elapsed
            // point (reaching chain=6 by 8s instead of 10s), but comparing against
            // the ghost's raw-engine-time checkpoints produces nonsense.
            int currentChainAt8sElapsed = 6;
            int delta = ghost.GetPacingDelta(elapsedTime: 8f, currentChainLength: currentChainAt8sElapsed);

            // Because every ghost checkpoint has Time > 8 (they're all in the
            // 1000s due to the different origin), GetGhostChainLengthAt(8) falls
            // through to the "before first checkpoint" case and returns 0 - so
            // the player is reported as "+6 ahead" when the real answer (had the
            // time bases matched) would have been "+0, right on pace, maybe
            // slightly ahead." The direction of the error is harmless-looking
            // here (always reports the player as further ahead than reality) but
            // it is reporting a specific, wrong, confident number from garbage
            // input - no exception, no warning.
            Assert.Equal(6, delta); // confirms the silent-lie path is real, not a crash
            Assert.Equal(0, ghost.GetGhostChainLengthAt(8f));
        }
    }
}
