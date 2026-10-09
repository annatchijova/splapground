using System;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class SessionClockTests
    {
        [Fact]
        public void BeforeStart_PhaseIsIdle()
        {
            var clock = new SessionClock();
            Assert.False(clock.IsStarted);
            Assert.Equal(SessionPhase.Idle, clock.GetPhase(0f));
            Assert.Equal(0f, clock.TimeRemainingInPhase(0f));
        }

        [Fact]
        public void JustAfterStart_IsDeskSprint()
        {
            var clock = new SessionClock(sprintDurationSeconds: 180f, destroyEverythingDurationSeconds: 10f);
            clock.Start(now: 100f);

            Assert.Equal(SessionPhase.DeskSprint, clock.GetPhase(100f));
            Assert.Equal(180f, clock.TimeRemainingInPhase(100f));
        }

        [Fact]
        public void AtSprintBoundary_SwitchesToDestroyEverything()
        {
            var clock = new SessionClock(sprintDurationSeconds: 180f, destroyEverythingDurationSeconds: 10f);
            clock.Start(now: 0f);

            Assert.Equal(SessionPhase.DeskSprint, clock.GetPhase(179.9f));
            Assert.Equal(SessionPhase.DestroyEverything, clock.GetPhase(180f));
            Assert.Equal(10f, clock.TimeRemainingInPhase(180f));
        }

        [Fact]
        public void AfterTotalDuration_IsEnded()
        {
            var clock = new SessionClock(sprintDurationSeconds: 180f, destroyEverythingDurationSeconds: 10f);
            clock.Start(now: 0f);

            Assert.Equal(SessionPhase.DestroyEverything, clock.GetPhase(189.9f));
            Assert.Equal(SessionPhase.Ended, clock.GetPhase(190f));
            Assert.Equal(0f, clock.TimeRemainingInPhase(190f));
            Assert.Equal(0f, clock.TimeRemainingInPhase(500f));
        }

        [Fact]
        public void QueryBeforeStartTime_Throws()
        {
            var clock = new SessionClock();
            clock.Start(now: 100f);
            Assert.Throws<ArgumentOutOfRangeException>(() => clock.GetPhase(99f));
        }

        [Fact]
        public void CustomDurations_AreRespected()
        {
            var clock = new SessionClock(sprintDurationSeconds: 60f, destroyEverythingDurationSeconds: 5f);
            clock.Start(now: 0f);

            Assert.Equal(SessionPhase.DeskSprint, clock.GetPhase(59f));
            Assert.Equal(SessionPhase.DestroyEverything, clock.GetPhase(60f));
            Assert.Equal(SessionPhase.Ended, clock.GetPhase(65f));
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-5f)]
        public void NonPositiveSprintDuration_Throws(float duration)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SessionClock(sprintDurationSeconds: duration));
        }
    }
}
