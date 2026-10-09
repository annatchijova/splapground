using System;
using System.Collections.Generic;

namespace Slapground.Core
{
    /// <summary>One logged impact: when it happened and how big it was (same impulse-magnitude unit BreakageRule consumes).</summary>
    public readonly struct ImpactRecord
    {
        public readonly float Time;
        public readonly float Magnitude;

        public ImpactRecord(float time, float magnitude)
        {
            if (magnitude < 0f) throw new ArgumentOutOfRangeException(nameof(magnitude));
            Time = time;
            Magnitude = magnitude;
        }
    }

    public readonly struct HighlightWindow
    {
        public readonly float StartTime;
        public readonly float EndTime;
        public readonly float Score;

        public HighlightWindow(float startTime, float endTime, float score)
        {
            StartTime = startTime;
            EndTime = endTime;
            Score = score;
        }
    }

    /// <summary>
    /// D3 (README_TECHNICAL.md section 2 and 7): "the clip has to be good by
    /// default" - the system has to know where the interesting moment was without
    /// the player editing anything. This picks the fixed-duration window with the
    /// highest total impact magnitude out of a session's impact log, so a highlight
    /// clip can be cut starting there automatically.
    ///
    /// Score is deliberately just "sum of impact magnitudes in the window" for now
    /// - no chain-length bonus, no recency weighting. A longer ChainTracker chain
    /// usually produces more impacts close together, which this already rewards
    /// through density; adding a separate chain-aware term is exactly the kind of
    /// untested weighting choice to defer until Level 1 has real sessions to tune
    /// against, per D2's own revisit trigger.
    /// </summary>
    public static class HighlightWindowFinder
    {
        /// <summary>
        /// impacts must be sorted ascending by Time (ChainTracker already requires
        /// non-decreasing timestamps from the same stream, so this is the natural
        /// precondition to share rather than re-sort defensively here).
        /// Returns null if impacts is empty - a quiet session has no highlight to cut.
        /// </summary>
        public static HighlightWindow? Find(IReadOnlyList<ImpactRecord> impacts, float windowDurationSeconds = 8f)
        {
            if (impacts == null) throw new ArgumentNullException(nameof(impacts));
            if (windowDurationSeconds <= 0f) throw new ArgumentOutOfRangeException(nameof(windowDurationSeconds));
            if (impacts.Count == 0) return null;

            for (int i = 1; i < impacts.Count; i++)
            {
                if (impacts[i].Time < impacts[i - 1].Time)
                    throw new ArgumentException("impacts must be sorted ascending by Time.", nameof(impacts));
            }

            int left = 0;
            float windowSum = 0f;
            float bestScore = float.NegativeInfinity;
            float bestStart = impacts[0].Time;

            for (int right = 0; right < impacts.Count; right++)
            {
                windowSum += impacts[right].Magnitude;

                while (impacts[right].Time - impacts[left].Time > windowDurationSeconds)
                {
                    windowSum -= impacts[left].Magnitude;
                    left++;
                }

                if (windowSum > bestScore)
                {
                    bestScore = windowSum;
                    bestStart = impacts[left].Time;
                }
            }

            return new HighlightWindow(bestStart, bestStart + windowDurationSeconds, bestScore);
        }
    }
}
