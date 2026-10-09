using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    /// <summary>
    /// Red team round 3: does TimeRemainingInPhase ever go negative right at a
    /// phase boundary due to float accumulation, instead of cleanly hitting zero
    /// or flipping phase? A HUD countdown reading "-0.0003s" would be a visible
    /// product bug, not just a theoretical one.
    /// </summary>
    public class SessionClockFloatBoundaryTests
    {
        [Fact]
        public void TimeRemaining_NeverNegative_AcrossManySmallTimeSteps()
        {
            var clock = new SessionClock(sprintDurationSeconds: 3f, destroyEverythingDurationSeconds: 1f);
            clock.Start(0f);

            float dt = 1f / 90f;
            float t = 0f;

            for (int i = 0; i < 400; i++) // well past the 4s total session
            {
                t += dt;
                float remaining = clock.TimeRemainingInPhase(t);
                Assert.True(remaining >= -0.0001f, $"frame {i}, t={t}: remaining went negative: {remaining}");
            }
        }

        [Fact]
        public void PhaseTransition_IsExactlyOnceAcrossManySmallTimeSteps()
        {
            var clock = new SessionClock(sprintDurationSeconds: 3f, destroyEverythingDurationSeconds: 1f);
            clock.Start(0f);

            float dt = 1f / 90f;
            float t = 0f;
            var seen = new System.Collections.Generic.List<SessionPhase>();
            SessionPhase? last = null;

            for (int i = 0; i < 400; i++)
            {
                t += dt;
                var phase = clock.GetPhase(t);
                if (phase != last)
                {
                    seen.Add(phase);
                    last = phase;
                }
            }

            // Phases should appear in this exact order, each exactly once as a transition.
            Assert.Equal(new[] { SessionPhase.DeskSprint, SessionPhase.DestroyEverything, SessionPhase.Ended }, seen);
        }
    }
}
