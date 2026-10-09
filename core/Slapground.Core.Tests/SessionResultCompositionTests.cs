using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    /// <summary>
    /// Red team round 4 (composition): SessionResult.Capture used to take a
    /// ChainTracker AND a separate impacts list, with nothing enforcing they
    /// described the same event stream - a caller could pass an unrelated list
    /// and get a silently inconsistent SessionResult (confirmed by induction
    /// before the fix: MaxChainLength said "3 impacts at t=0..2" while Highlight
    /// said "a 999-magnitude event at t=500" from a totally unrelated list).
    ///
    /// Fixed by making ChainTracker the single source of truth (ChainTracker.
    /// Impacts) and removing the separate parameter from Capture entirely - this
    /// class of bug is now impossible by construction, not just undocumented.
    /// This test is now a regression guard for that: it proves the highlight
    /// always comes from the SAME tracker's own recorded impacts.
    /// </summary>
    public class SessionResultCompositionTests
    {
        [Fact]
        public void Highlight_AlwaysComesFromTheSameTrackersOwnImpacts_NotAnExternalList()
        {
            var chain = new ChainTracker();
            chain.RecordImpact(0f, magnitude: 5f);
            chain.RecordImpact(1f, magnitude: 5f);
            chain.RecordImpact(2f, magnitude: 5f);

            var result = SessionResult.Capture(chain, elapsedSeconds: 190f);

            Assert.Equal(3, result.MaxChainLength);
            Assert.NotNull(result.Highlight);
            Assert.Equal(0f, result.Highlight!.Value.StartTime); // the chain's own t=0, not any externally-supplied value
            Assert.Equal(15f, result.Highlight!.Value.Score);
        }

        [Fact]
        public void ChainTracker_Impacts_RecordsMagnitudeAlongsideTimestamp()
        {
            var chain = new ChainTracker();
            chain.RecordImpact(1f, magnitude: 7f);
            chain.RecordImpact(2f); // magnitude defaults to 0 - callers that only care about timing

            Assert.Equal(2, chain.Impacts.Count);
            Assert.Equal(7f, chain.Impacts[0].Magnitude);
            Assert.Equal(0f, chain.Impacts[1].Magnitude);
        }
    }
}
