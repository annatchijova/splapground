using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class SessionClockPauseResumeTests
    {
        [Fact]
        public void Pause_FreezesTimeRemaining()
        {
            var clock = new SessionClock(sprintDurationSeconds: 180f, destroyEverythingDurationSeconds: 10f);
            clock.Start(0f);

            clock.Pause(50f);
            Assert.True(clock.IsPaused);
            float remainingAtPause = clock.TimeRemainingInPhase(50f);

            // Real time passes while paused - 500 "seconds" go by in the outside world.
            Assert.Equal(remainingAtPause, clock.TimeRemainingInPhase(550f));
            Assert.Equal(SessionPhase.DeskSprint, clock.GetPhase(550f));
        }

        [Fact]
        public void Resume_ContinuesFromWhereItPaused_NotFromRealTime()
        {
            var clock = new SessionClock(sprintDurationSeconds: 180f, destroyEverythingDurationSeconds: 10f);
            clock.Start(0f);

            clock.Pause(50f);   // 50s of sprint elapsed
            clock.Resume(550f); // paused for 500s of real time
            Assert.False(clock.IsPaused);

            // 10s after resuming, only 60s of sprint should have elapsed (50 + 10),
            // not 560s (which would already be well past Ended).
            Assert.Equal(SessionPhase.DeskSprint, clock.GetPhase(560f));
            Assert.Equal(180f - 60f, clock.TimeRemainingInPhase(560f), precision: 3);
        }

        [Fact]
        public void DoublePause_IsIdempotent_DoesNotDoubleCountPausedTime()
        {
            var clock = new SessionClock();
            clock.Start(0f);

            clock.Pause(10f);
            clock.Pause(20f); // should be a no-op; the pause already started at 10
            clock.Resume(30f);

            // The pause ran from 10 to 30 (20s), because the Pause(20) no-op did not
            // reset pausedAt to 20 - that's what "idempotent" is asserting here. If
            // Pause(20) had wrongly restarted the pause clock, only 10s (20->30)
            // would have been subtracted and this would be 170, not 160.
            Assert.Equal(180f - 20f, clock.TimeRemainingInPhase(40f), precision: 3);
        }

        [Fact]
        public void ResumeWithoutPause_IsANoOp()
        {
            var clock = new SessionClock();
            clock.Start(0f);
            clock.Resume(50f); // never paused

            Assert.Equal(180f - 50f, clock.TimeRemainingInPhase(50f), precision: 3);
        }

        [Fact]
        public void PauseBeforeStart_IsANoOp_DoesNotThrow()
        {
            var clock = new SessionClock();
            clock.Pause(5f); // not started yet
            Assert.False(clock.IsPaused);
            Assert.Equal(SessionPhase.Idle, clock.GetPhase(5f));
        }

        [Fact]
        public void PauseAcrossPhaseBoundary_StillTransitionsCorrectlyAfterResume()
        {
            var clock = new SessionClock(sprintDurationSeconds: 180f, destroyEverythingDurationSeconds: 10f);
            clock.Start(0f);

            clock.Pause(179f);   // 1s left in DeskSprint
            clock.Resume(1000f); // huge real-time gap

            Assert.Equal(SessionPhase.DeskSprint, clock.GetPhase(1000f));
            Assert.Equal(SessionPhase.DestroyEverything, clock.GetPhase(1001f)); // 2s of effective elapsed past resume = 181s total
            Assert.Equal(SessionPhase.Ended, clock.GetPhase(1011f)); // 191s effective elapsed
        }

        [Fact]
        public void MultiplePauseResumeCycles_AccumulateCorrectly()
        {
            var clock = new SessionClock(sprintDurationSeconds: 180f);
            clock.Start(0f);

            clock.Pause(10f);
            clock.Resume(20f);  // +10s paused
            clock.Pause(30f);
            clock.Resume(45f);  // +15s paused (25s total paused)

            // Real time is 50; effective elapsed = 50 - 25 = 25
            Assert.Equal(180f - 25f, clock.TimeRemainingInPhase(50f), precision: 3);
        }
    }
}
