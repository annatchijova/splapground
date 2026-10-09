using System;
using System.Collections.Generic;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class HighlightWindowFinderTests
    {
        [Fact]
        public void EmptyImpacts_ReturnsNull()
        {
            Assert.Null(HighlightWindowFinder.Find(Array.Empty<ImpactRecord>()));
        }

        [Fact]
        public void SingleImpact_IsItsOwnWindow()
        {
            var impacts = new[] { new ImpactRecord(time: 5f, magnitude: 3f) };
            var window = HighlightWindowFinder.Find(impacts, windowDurationSeconds: 8f);

            Assert.NotNull(window);
            Assert.Equal(5f, window.Value.StartTime);
            Assert.Equal(13f, window.Value.EndTime);
            Assert.Equal(3f, window.Value.Score);
        }

        [Fact]
        public void PicksTheDenseBurstOverScatteredBiggerHits()
        {
            // Three big hits spread far apart (never share an 8s window) vs. a
            // tight burst of smaller hits - the burst's combined score should win.
            var impacts = new List<ImpactRecord>
            {
                new ImpactRecord(0f, 10f),
                new ImpactRecord(50f, 10f),
                new ImpactRecord(100f, 10f),

                new ImpactRecord(200f, 4f),
                new ImpactRecord(202f, 4f),
                new ImpactRecord(204f, 4f),
                new ImpactRecord(206f, 4f), // sum 16 within an 8s window, beats any single 10
            };

            var window = HighlightWindowFinder.Find(impacts, windowDurationSeconds: 8f);

            Assert.NotNull(window);
            Assert.Equal(200f, window.Value.StartTime);
            Assert.Equal(16f, window.Value.Score);
        }

        [Fact]
        public void UnsortedImpacts_Throw()
        {
            var impacts = new[]
            {
                new ImpactRecord(10f, 1f),
                new ImpactRecord(5f, 1f),
            };
            Assert.Throws<ArgumentException>(() => HighlightWindowFinder.Find(impacts));
        }

        [Fact]
        public void NegativeMagnitude_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ImpactRecord(0f, -1f));
        }

        [Fact]
        public void NonPositiveWindowDuration_Throws()
        {
            var impacts = new[] { new ImpactRecord(0f, 1f) };
            Assert.Throws<ArgumentOutOfRangeException>(() => HighlightWindowFinder.Find(impacts, windowDurationSeconds: 0f));
        }

        [Fact]
        public void NullImpacts_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => HighlightWindowFinder.Find(null!));
        }
    }
}
