using System;

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

        /// <summary>
        /// Builds a result from a session's live ChainTracker, which is now the
        /// single source of truth for both the chain/CPM stats and the impact
        /// log the highlight scan reads - there is no separate impacts
        /// parameter to accidentally pass an unrelated list to (see
        /// ChainTracker.Impacts' own comment for why that changed). Call once,
        /// at session end (SessionPhase.Ended).
        ///
        /// Round-6 composition fix (docs/red-team-round-6-core.md): elapsedSeconds
        /// is cross-checked against the chain's own last recorded impact time -
        /// unlike round 5's ChainTracker/GhostPacer seam, this one CAN be fixed by
        /// construction, because both values are already parameters of this same
        /// call and there's no reason not to check one against the other here.
        /// </summary>
        public static SessionResult Capture(
            ChainTracker chain,
            float elapsedSeconds,
            float highlightWindowDurationSeconds = 8f)
        {
            if (chain == null) throw new ArgumentNullException(nameof(chain));

            if (chain.Impacts.Count > 0)
            {
                float lastImpactTime = chain.Impacts[chain.Impacts.Count - 1].Time;
                if (elapsedSeconds < lastImpactTime)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(elapsedSeconds), elapsedSeconds,
                        $"elapsedSeconds ({elapsedSeconds}) is before the chain's own last recorded impact at {lastImpactTime} - this session's elapsed time and its impact log disagree.");
                }
            }

            var highlight = HighlightWindowFinder.Find(chain.Impacts, highlightWindowDurationSeconds);
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
