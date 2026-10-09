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
    ///
    /// Pause/Resume added 2026-10-09 during a red-team pass: the competition's own
    /// Design Guideline ("Easy to get into and out of: Fast cold start, clean
    /// pause/resume...") requires this, and the first version of this class had no
    /// way to honor it - elapsed time just kept accumulating with no way to freeze
    /// it. That's a product/UX gap against a named rule, not a hypothetical.
    /// Pausing freezes the phase/countdown in place rather than introducing a
    /// separate Paused value into SessionPhase - callers that want to show "paused"
    /// in the HUD check IsPaused alongside whatever GetPhase already reports,
    /// instead of every phase-based switch elsewhere needing a new case.
    /// </summary>
    public class SessionClock
    {
        public float SprintDurationSeconds { get; }
        public float DestroyEverythingDurationSeconds { get; }

        private float? startTime;
        private float? pausedAt;
        private float totalPausedDuration;

        public SessionClock(float sprintDurationSeconds = 180f, float destroyEverythingDurationSeconds = 10f)
        {
            if (sprintDurationSeconds <= 0f) throw new ArgumentOutOfRangeException(nameof(sprintDurationSeconds));
            if (destroyEverythingDurationSeconds <= 0f) throw new ArgumentOutOfRangeException(nameof(destroyEverythingDurationSeconds));

            SprintDurationSeconds = sprintDurationSeconds;
            DestroyEverythingDurationSeconds = destroyEverythingDurationSeconds;
        }

        public bool IsStarted => startTime.HasValue;
        public bool IsPaused => pausedAt.HasValue;

        public void Start(float now)
        {
            startTime = now;
            pausedAt = null;
            totalPausedDuration = 0f;
        }

        /// <summary>No-op if not started or already paused - pausing is idempotent, not an error to call twice.</summary>
        public void Pause(float now)
        {
            if (!startTime.HasValue || pausedAt.HasValue) return;
            pausedAt = now;
        }

        /// <summary>No-op if not paused.</summary>
        public void Resume(float now)
        {
            if (!pausedAt.HasValue) return;
            totalPausedDuration += now - pausedAt.Value;
            pausedAt = null;
        }

        private float EffectiveElapsed(float now)
        {
            if (!startTime.HasValue) throw new InvalidOperationException("EffectiveElapsed called before Start().");

            float rawElapsed = now - startTime.Value;
            float pausedSoFar = totalPausedDuration + (pausedAt.HasValue ? now - pausedAt.Value : 0f);
            return rawElapsed - pausedSoFar;
        }

        public SessionPhase GetPhase(float now)
        {
            if (!startTime.HasValue) return SessionPhase.Idle;

            float elapsed = EffectiveElapsed(now);
            if (elapsed < 0f) throw new ArgumentOutOfRangeException(nameof(now), now, "now is before Start().");

            if (elapsed < SprintDurationSeconds) return SessionPhase.DeskSprint;
            if (elapsed < SprintDurationSeconds + DestroyEverythingDurationSeconds) return SessionPhase.DestroyEverything;
            return SessionPhase.Ended;
        }

        /// <summary>Seconds left in whichever phase GetPhase(now) would return. Zero when Idle (no timer running yet) or Ended. Frozen while IsPaused.</summary>
        public float TimeRemainingInPhase(float now)
        {
            if (!startTime.HasValue) return 0f;

            float elapsed = EffectiveElapsed(now);
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
