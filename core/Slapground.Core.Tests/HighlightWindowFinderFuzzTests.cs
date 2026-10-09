using System;
using System.Collections.Generic;
using System.Linq;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    /// <summary>
    /// Red-team round 1 on HighlightWindowFinder: cross-check the two-pointer
    /// sliding window against a brute-force O(n^2) reference on random inputs.
    /// If the two algorithms ever disagree on the achievable max score, the
    /// sliding window has a real bug - this is an executed induction, not a
    /// read-and-assume.
    /// </summary>
    public class HighlightWindowFinderFuzzTests
    {
        private static float BruteForceBestScore(List<ImpactRecord> impacts, float windowDurationSeconds)
        {
            float best = float.NegativeInfinity;
            for (int i = 0; i < impacts.Count; i++)
            {
                float sum = 0f;
                float start = impacts[i].Time;
                for (int j = i; j < impacts.Count; j++)
                {
                    if (impacts[j].Time - start > windowDurationSeconds) break;
                    sum += impacts[j].Magnitude;
                }
                best = Math.Max(best, sum);
            }
            return best;
        }

        [Fact]
        public void SlidingWindow_MatchesBruteForce_OnManyRandomStreams()
        {
            var rng = new Random(20261009);

            for (int trial = 0; trial < 300; trial++)
            {
                int count = rng.Next(1, 40);
                float windowDuration = (float)(rng.NextDouble() * 20 + 0.5);

                var times = new List<float>();
                float t = 0f;
                for (int i = 0; i < count; i++)
                {
                    t += (float)(rng.NextDouble() * 5); // random non-negative gaps, allows duplicates via 0-gap
                    times.Add(t);
                }

                var impacts = times.Select(time => new ImpactRecord(time, (float)(rng.NextDouble() * 10))).ToList();

                var result = HighlightWindowFinder.Find(impacts, windowDuration);
                float expected = BruteForceBestScore(impacts, windowDuration);

                Assert.NotNull(result);
                Assert.True(
                    Math.Abs(result.Value.Score - expected) < 0.0001f,
                    $"trial {trial}: sliding window scored {result.Value.Score}, brute force found {expected} (count={count}, windowDuration={windowDuration})");
            }
        }

        [Fact]
        public void DuplicateTimestamps_AreHandledCorrectly()
        {
            // Several impacts at the exact same instant - e.g. both hands landing
            // on a swarm at once. All logged at time 5.
            var impacts = new List<ImpactRecord>
            {
                new ImpactRecord(5f, 1f),
                new ImpactRecord(5f, 1f),
                new ImpactRecord(5f, 1f),
                new ImpactRecord(5f, 1f),
            };

            var result = HighlightWindowFinder.Find(impacts, windowDurationSeconds: 8f);

            Assert.NotNull(result);
            Assert.Equal(4f, result.Value.Score);
        }
    }
}
