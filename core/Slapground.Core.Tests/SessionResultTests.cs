using System;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class SessionResultTests
    {
        [Fact]
        public void Capture_WiresUpChainTrackerAndHighlightCorrectly()
        {
            var chain = new ChainTracker();
            foreach (float time in new[] { 0f, 1f, 2f })
            {
                chain.RecordImpact(time, magnitude: 5f);
                chain.ChaosPerMinute(time);
            }

            var result = SessionResult.Capture(chain, elapsedSeconds: 190f);

            Assert.Equal(3, result.MaxChainLength);
            Assert.Equal(3, result.PeakChaosPerMinute);
            Assert.NotNull(result.Highlight);
            Assert.Equal(15f, result.Highlight!.Value.Score);
            Assert.Equal(190f, result.ElapsedSeconds);
        }

        [Fact]
        public void Capture_WithNoImpacts_HasNullHighlightAndZeroStats()
        {
            var chain = new ChainTracker();
            var result = SessionResult.Capture(chain, elapsedSeconds: 190f);

            Assert.Equal(0, result.MaxChainLength);
            Assert.Null(result.Highlight);
        }

        [Fact]
        public void Capture_NullChainTracker_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SessionResult.Capture(null!, elapsedSeconds: 0f));
        }

        [Theory]
        [InlineData(5, 3, true)]
        [InlineData(3, 5, false)]
        [InlineData(4, 4, false)] // a tie does not "beat" the other
        public void Beats_ComparesOnMaxChainLengthOnly(int currentChain, int otherChain, bool expected)
        {
            var current = new SessionResult(currentChain, peakChaosPerMinute: 0, highlight: null, elapsedSeconds: 0f);
            var other = new SessionResult(otherChain, peakChaosPerMinute: 999, highlight: null, elapsedSeconds: 0f); // CPM shouldn't matter

            Assert.Equal(expected, current.Beats(other));
        }

        [Theory]
        [InlineData(-1, 0, 0)]
        [InlineData(0, -1, 0)]
        [InlineData(0, 0, -1)]
        public void Constructor_RejectsNegativeValues(int maxChain, int peakCpm, float elapsed)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new SessionResult(maxChain, peakCpm, highlight: null, elapsedSeconds: elapsed));
        }
    }
}
