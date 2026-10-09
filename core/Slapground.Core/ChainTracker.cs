using System;
using System.Collections.Generic;

namespace Slapground.Core
{
    /// <summary>
    /// The Max Kinetic Chain / Chaos-Per-Minute (CPM) scoring named in Level 1
    /// (README_TECHNICAL.md section 2). Pure event-stream analysis - takes
    /// caller-supplied timestamps rather than reading Unity's Time.time or the
    /// system clock, so it's testable without either.
    ///
    /// A "chain" is a run of impacts where each one lands within
    /// <see cref="MaxGapSeconds"/> of the previous one; a longer gap starts a new
    /// chain instead of continuing it. CPM is a rolling 60-second impact count,
    /// not a session-average - a quiet start shouldn't make a hot finish look
    /// worse than it is.
    /// </summary>
    public class ChainTracker
    {
        public float MaxGapSeconds { get; }

        private readonly Queue<float> recentImpactTimes = new Queue<float>();
        private float? lastImpactTime;

        public int CurrentChainLength { get; private set; }
        public int MaxChainLength { get; private set; }

        /// <summary>The highest value ChaosPerMinute has ever returned for this tracker - updated each time ChaosPerMinute is called, not continuously, so a session that never polls it never gets a peak.</summary>
        public int PeakChaosPerMinute { get; private set; }

        public ChainTracker(float maxGapSeconds = 1.5f)
        {
            if (maxGapSeconds <= 0f) throw new ArgumentOutOfRangeException(nameof(maxGapSeconds));
            MaxGapSeconds = maxGapSeconds;
        }

        /// <summary>Record one impact. Timestamps must be non-decreasing - this is a live stream, not a sort.</summary>
        public void RecordImpact(float timestamp)
        {
            if (lastImpactTime.HasValue && timestamp < lastImpactTime.Value)
                throw new ArgumentOutOfRangeException(nameof(timestamp), timestamp, "Timestamps must be non-decreasing.");

            bool continuesChain = lastImpactTime.HasValue && (timestamp - lastImpactTime.Value) <= MaxGapSeconds;
            CurrentChainLength = continuesChain ? CurrentChainLength + 1 : 1;
            MaxChainLength = Math.Max(MaxChainLength, CurrentChainLength);

            lastImpactTime = timestamp;
            recentImpactTimes.Enqueue(timestamp);
        }

        /// <summary>Impacts in the trailing 60 seconds as of <paramref name="now"/>. Call after RecordImpact for the live rate, or on its own to decay a stale session.</summary>
        public int ChaosPerMinute(float now)
        {
            if (lastImpactTime.HasValue && now < lastImpactTime.Value)
                throw new ArgumentOutOfRangeException(nameof(now), now, "now is before the most recently recorded impact.");

            while (recentImpactTimes.Count > 0 && now - recentImpactTimes.Peek() > 60f)
            {
                recentImpactTimes.Dequeue();
            }
            PeakChaosPerMinute = Math.Max(PeakChaosPerMinute, recentImpactTimes.Count);
            return recentImpactTimes.Count;
        }
    }
}
