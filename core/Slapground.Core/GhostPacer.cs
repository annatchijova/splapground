using System;
using System.Collections.Generic;

namespace Slapground.Core
{
    /// <summary>
    /// One point on a recorded past run's chain-length-over-time curve. Record
    /// one of these every time ChainTracker.MaxChainLength changes during a
    /// session to build a ghost's timeline - the raw material GhostPacer compares
    /// against.
    /// </summary>
    public readonly struct GhostCheckpoint
    {
        public readonly float Time;
        public readonly int MaxChainLength;

        public GhostCheckpoint(float time, int maxChainLength)
        {
            if (maxChainLength < 0) throw new ArgumentOutOfRangeException(nameof(maxChainLength));
            Time = time;
            MaxChainLength = maxChainLength;
        }
    }

    /// <summary>
    /// The async ghost-trail competitive loop (README.md: "a faint ghost trail of
    /// a past run - competitive without the visual clutter of a live opponent's
    /// arms"). That's a live pacing comparison, not just a final-score banner: at
    /// any moment in the current session, how does the current MaxChainLength
    /// compare to where the ghost's own MaxChainLength was at the same elapsed
    /// time? This is the one piece of "ghost trail" that's actually pure logic -
    /// rendering a visible trail is a Unity-side concern this does not attempt.
    ///
    /// Deliberately tracks only MaxChainLength, the one stat the product brief
    /// names as the headline leaderboard metric ("Max Kinetic Chain"). A
    /// multi-axis score blending CPM/total-chaos/chain-length would be the same
    /// kind of unvalidated weighting choice HighlightWindowFinder's scoring
    /// deferred - not invented here either, for the same reason.
    /// </summary>
    public class GhostPacer
    {
        private readonly List<GhostCheckpoint> timeline;

        /// <summary>checkpoints must be sorted ascending by Time - the natural order a live session records them in.</summary>
        public GhostPacer(IReadOnlyList<GhostCheckpoint> checkpoints)
        {
            if (checkpoints == null) throw new ArgumentNullException(nameof(checkpoints));
            for (int i = 1; i < checkpoints.Count; i++)
            {
                if (checkpoints[i].Time < checkpoints[i - 1].Time)
                    throw new ArgumentException("checkpoints must be sorted ascending by Time.", nameof(checkpoints));
            }
            timeline = new List<GhostCheckpoint>(checkpoints);
        }

        /// <summary>
        /// The ghost's MaxChainLength as of elapsedTime: the last checkpoint at or
        /// before elapsedTime. If elapsedTime is before the ghost's first
        /// checkpoint (or the ghost has no checkpoints at all), the ghost hasn't
        /// landed its first impact yet at this point in its own run - 0.
        /// </summary>
        public int GetGhostChainLengthAt(float elapsedTime)
        {
            int best = 0;
            foreach (var checkpoint in timeline)
            {
                if (checkpoint.Time > elapsedTime) break;
                best = checkpoint.MaxChainLength;
            }
            return best;
        }

        /// <summary>
        /// currentChainLength minus the ghost's chain length at the same elapsed
        /// time. Positive: ahead of the ghost's pace. Negative: behind. Zero: tied.
        /// </summary>
        public int GetPacingDelta(float elapsedTime, int currentChainLength)
        {
            if (currentChainLength < 0) throw new ArgumentOutOfRangeException(nameof(currentChainLength));
            return currentChainLength - GetGhostChainLengthAt(elapsedTime);
        }
    }
}
