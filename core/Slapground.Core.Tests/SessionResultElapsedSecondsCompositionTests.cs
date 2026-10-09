using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    /// <summary>
    /// Red team round 6 (composition): SessionResult.Capture takes chain and
    /// elapsedSeconds as two independent parameters, with nothing (before this
    /// round's fix) checking elapsedSeconds against the chain's own last
    /// recorded impact time. Logged as a lower-severity deferred item in round 5
    /// (docs/red-team-round-5-core.md); investigated for real here.
    /// </summary>
    public class SessionResultElapsedSecondsCompositionTests
    {
        [Fact]
        public void ElapsedSecondsBeforeTheChainsLastImpact_IsRejected()
        {
            var chain = new ChainTracker();
            chain.RecordImpact(300f, magnitude: 5f); // the chain's own timeline says this session ran to at least t=300

            // A caller claims the session only lasted 5 seconds - inconsistent
            // with an impact the chain itself recorded at t=300.
            Assert.Throws<System.ArgumentOutOfRangeException>(() =>
                SessionResult.Capture(chain, elapsedSeconds: 5f));
        }

        [Fact]
        public void ElapsedSecondsAtOrAfterTheChainsLastImpact_IsAccepted()
        {
            var chain = new ChainTracker();
            chain.RecordImpact(300f, magnitude: 5f);

            var exact = SessionResult.Capture(chain, elapsedSeconds: 300f);
            var after = SessionResult.Capture(chain, elapsedSeconds: 305f);

            Assert.Equal(300f, exact.ElapsedSeconds);
            Assert.Equal(305f, after.ElapsedSeconds);
        }

        [Fact]
        public void EmptyChain_AnyNonNegativeElapsedSecondsIsAccepted()
        {
            var chain = new ChainTracker();
            var result = SessionResult.Capture(chain, elapsedSeconds: 0f);
            Assert.Equal(0f, result.ElapsedSeconds);
        }
    }
}
