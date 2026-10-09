using System;
using System.Collections.Generic;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class GhostPacerTests
    {
        private static GhostPacer MakeExampleGhost() => new GhostPacer(new[]
        {
            new GhostCheckpoint(10f, 1),
            new GhostCheckpoint(25f, 3),
            new GhostCheckpoint(60f, 7),
        });

        [Fact]
        public void BeforeFirstCheckpoint_GhostChainLengthIsZero()
        {
            var ghost = MakeExampleGhost();
            Assert.Equal(0, ghost.GetGhostChainLengthAt(5f));
        }

        [Fact]
        public void ExactlyAtACheckpoint_UsesThatCheckpoint()
        {
            var ghost = MakeExampleGhost();
            Assert.Equal(3, ghost.GetGhostChainLengthAt(25f));
        }

        [Fact]
        public void BetweenCheckpoints_UsesTheEarlierOne()
        {
            var ghost = MakeExampleGhost();
            Assert.Equal(1, ghost.GetGhostChainLengthAt(24.9f));
            Assert.Equal(3, ghost.GetGhostChainLengthAt(25.1f));
        }

        [Fact]
        public void AfterLastCheckpoint_HoldsAtTheFinalValue()
        {
            var ghost = MakeExampleGhost();
            Assert.Equal(7, ghost.GetGhostChainLengthAt(1000f));
        }

        [Fact]
        public void EmptyGhost_IsAlwaysZero()
        {
            var ghost = new GhostPacer(Array.Empty<GhostCheckpoint>());
            Assert.Equal(0, ghost.GetGhostChainLengthAt(0f));
            Assert.Equal(0, ghost.GetGhostChainLengthAt(999f));
        }

        [Theory]
        [InlineData(5f, 0, 0)]    // before ghost starts, tied at 0
        [InlineData(25f, 5, 2)]  // ahead by 2
        [InlineData(25f, 1, -2)] // behind by 2
        [InlineData(60f, 7, 0)]  // tied
        public void GetPacingDelta_HasCorrectSignAndMagnitude(float time, int current, int expectedDelta)
        {
            var ghost = MakeExampleGhost();
            Assert.Equal(expectedDelta, ghost.GetPacingDelta(time, current));
        }

        [Fact]
        public void NegativeCurrentChainLength_Throws()
        {
            var ghost = MakeExampleGhost();
            Assert.Throws<ArgumentOutOfRangeException>(() => ghost.GetPacingDelta(0f, -1));
        }

        [Fact]
        public void UnsortedCheckpoints_Throw()
        {
            var checkpoints = new[]
            {
                new GhostCheckpoint(10f, 1),
                new GhostCheckpoint(5f, 2),
            };
            Assert.Throws<ArgumentException>(() => new GhostPacer(checkpoints));
        }

        [Fact]
        public void NegativeChainLengthInCheckpoint_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new GhostCheckpoint(0f, -1));
        }

        [Fact]
        public void NullCheckpoints_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new GhostPacer(null!));
        }

        /// <summary>Red-team invariant: a ghost built from a real (monotonic) session must report a non-decreasing chain length as elapsed time increases.</summary>
        [Fact]
        public void GhostChainLength_IsMonotonicNonDecreasing_OverARandomRealisticTimeline()
        {
            var rng = new Random(2026);
            var checkpoints = new List<GhostCheckpoint>();
            float time = 0f;
            int chain = 0;

            for (int i = 0; i < 50; i++)
            {
                time += (float)(rng.NextDouble() * 5);
                chain += rng.Next(0, 3); // MaxChainLength never decreases within a real session
                checkpoints.Add(new GhostCheckpoint(time, chain));
            }

            var ghost = new GhostPacer(checkpoints);

            int previous = 0;
            for (float t = 0f; t <= time + 10f; t += 0.37f)
            {
                int value = ghost.GetGhostChainLengthAt(t);
                Assert.True(value >= previous, $"t={t}: ghost chain length decreased from {previous} to {value}");
                previous = value;
            }
        }
    }
}
