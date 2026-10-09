using System;

namespace Slapground.Core
{
    public enum SessionPhase
    {
        /// <summary>Start() hasn't been called yet.</summary>
        Idle,

        /// <summary>The main 3-minute session (README.md: "a session is short on purpose").</summary>
        DeskSprint,

        /// <summary>The closing ten seconds where everything on the desk is fair game (README.md).</summary>
        DestroyEverything,

        Ended,
    }

    /// <summary>
    /// The Level 1 session shape named in README.md/README_TECHNICAL.md: a 3-minute
    /// Desk Sprint followed by a 10-second Destroy Everything window. Pure
    /// elapsed-time logic over a caller-supplied clock, same testable-without-Unity
    /// style as ChainTracker - this takes a start timestamp once, then phase
    /// queries take "now" rather than reading Time.time internally.
    /// </summary>
    public class SessionClock
    {
        public float SprintDurationSeconds { get; }
        public float DestroyEverythingDurationSeconds { get; }

        private float? startTime;

        public SessionClock(float sprintDurationSeconds = 180f, float destroyEverythingDurationSeconds = 10f)
        {
            if (sprintDurationSeconds <= 0f) throw new ArgumentOutOfRangeException(nameof(sprintDurationSeconds));
            if (destroyEverythingDurationSeconds <= 0f) throw new ArgumentOutOfRangeException(nameof(destroyEverythingDurationSeconds));

            SprintDurationSeconds = sprintDurationSeconds;
            DestroyEverythingDurationSeconds = destroyEverythingDurationSeconds;
        }

        public bool IsStarted => startTime.HasValue;

        public void Start(float now)
        {
            startTime = now;
        }

        public SessionPhase GetPhase(float now)
        {
            if (!startTime.HasValue) return SessionPhase.Idle;

            float elapsed = now - startTime.Value;
            if (elapsed < 0f) throw new ArgumentOutOfRangeException(nameof(now), now, "now is before Start().");

            if (elapsed < SprintDurationSeconds) return SessionPhase.DeskSprint;
            if (elapsed < SprintDurationSeconds + DestroyEverythingDurationSeconds) return SessionPhase.DestroyEverything;
            return SessionPhase.Ended;
        }

        /// <summary>Seconds left in whichever phase GetPhase(now) would return. Zero when Idle (no timer running yet) or Ended.</summary>
        public float TimeRemainingInPhase(float now)
        {
            if (!startTime.HasValue) return 0f;

            float elapsed = now - startTime.Value;
            SessionPhase phase = GetPhase(now);

            return phase switch
            {
                SessionPhase.DeskSprint => SprintDurationSeconds - elapsed,
                SessionPhase.DestroyEverything => SprintDurationSeconds + DestroyEverythingDurationSeconds - elapsed,
                _ => 0f,
            };
        }
    }
}
