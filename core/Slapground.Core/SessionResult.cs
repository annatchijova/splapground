using System;
using System.Collections.Generic;

namespace Slapground.Core
{
    /// <summary>
    /// The complete end-of-session summary: everything the async competitive
    /// loop (README.md/README_TECHNICAL.md section 2) needs to record, compare
    /// against a ghost, or show on an end screen. Deliberately just wires
    /// together ChainTracker and HighlightWindowFinder's already-tested outputs
    /// rather than computing anything new - this is integration, not a new
    /// scoring formula, which is why it only became buildable now that every
    /// piece it reads from has survived its own red-team round.
    /// </summary>
    public readonly struct SessionResult
    {
        public readonly int MaxChainLength;
        public readonly int PeakChaosPerMinute;
        public readonly HighlightWindow? Highlight;
        public readonly float ElapsedSeconds;

        public SessionResult(int maxChainLength, int peakChaosPerMinute, HighlightWindow? highlight, float elapsedSeconds)
        {
            if (maxChainLength < 0) throw new ArgumentOutOfRangeException(nameof(maxChainLength));
            if (peakChaosPerMinute < 0) throw new ArgumentOutOfRangeException(nameof(peakChaosPerMinute));
            if (elapsedSeconds < 0f) throw new ArgumentOutOfRangeException(nameof(elapsedSeconds));

            MaxChainLength = maxChainLength;
            PeakChaosPerMinute = peakChaosPerMinute;
            Highlight = highlight;
            ElapsedSeconds = elapsedSeconds;
        }

        /// <summary>Builds a result from a session's live ChainTracker and its full impact log. Call once, at session end (SessionPhase.Ended).</summary>
        public static SessionResult Capture(
            ChainTracker chain,
            IReadOnlyList<ImpactRecord> impacts,
            float elapsedSeconds,
            float highlightWindowDurationSeconds = 8f)
        {
            if (chain == null) throw new ArgumentNullException(nameof(chain));

            var highlight = HighlightWindowFinder.Find(impacts, highlightWindowDurationSeconds);
            return new SessionResult(chain.MaxChainLength, chain.PeakChaosPerMinute, highlight, elapsedSeconds);
        }

        /// <summary>
        /// Single-axis comparison against a past result (a personal best, or a
        /// ghost's final result), on MaxChainLength - the brief's named headline
        /// stat. Deliberately not a multi-axis weighted verdict; see GhostPacer's
        /// and HighlightWindowFinder's own notes on why that's deferred, not built.
        /// </summary>
        public bool Beats(SessionResult other) => MaxChainLength > other.MaxChainLength;
    }
}
