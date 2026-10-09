using System.Numerics;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    /// <summary>Red team round 3: VelocityBuffer's bufferSize cap and eviction was implemented but never actually tested until now.</summary>
    public class VelocityBufferOverflowTests
    {
        private const float FrameInterval = 1f / 90f;

        [Fact]
        public void SampleCount_NeverExceedsBufferSize()
        {
            var buffer = new VelocityBuffer(bufferSize: 10, lookbackSeconds: 0.05f);

            for (int i = 0; i < 100; i++)
            {
                buffer.AddSample(i * FrameInterval, new Vector3(i, 0, 0));
                Assert.True(buffer.SampleCount <= 10, $"frame {i}: SampleCount={buffer.SampleCount} exceeds BufferSize=10");
            }

            Assert.Equal(10, buffer.SampleCount);
        }

        [Fact]
        public void AfterOverflow_OldestSamplesAreGone_BufferedVelocityUsesOnlyRetainedWindow()
        {
            // bufferSize small enough that a 0.05s lookback can't reach past it -
            // GetBufferedVelocity should fall back gracefully to the oldest
            // sample actually retained, not throw or silently misbehave.
            var buffer = new VelocityBuffer(bufferSize: 3, lookbackSeconds: 0.5f); // lookback far exceeds what 3 samples at 90Hz can span
            var trueVelocity = new Vector3(2f, 0f, 0f);
            var position = Vector3.Zero;

            for (int i = 0; i < 20; i++)
            {
                buffer.AddSample(i * FrameInterval, position);
                position += trueVelocity * FrameInterval;
            }

            Assert.Equal(3, buffer.SampleCount);

            // With only 3 samples spanning ~2 frames (~0.022s), far short of the
            // 0.5s lookback, GetBufferedVelocity should use the oldest retained
            // sample as "earlier" (its own documented fallback), not throw.
            var buffered = buffer.GetBufferedVelocity();
            Assert.True(Vector3.Distance(buffered, trueVelocity) < 0.01f,
                $"expected ~{trueVelocity}, got {buffered}");
        }

        [Fact]
        public void MinimumBufferSizeOfTwo_StillWorks()
        {
            var buffer = new VelocityBuffer(bufferSize: 2, lookbackSeconds: 0.05f);
            buffer.AddSample(0f, Vector3.Zero);
            buffer.AddSample(FrameInterval, new Vector3(1f, 0f, 0f));
            buffer.AddSample(2 * FrameInterval, new Vector3(2f, 0f, 0f)); // evicts the first

            Assert.Equal(2, buffer.SampleCount);
            Assert.NotEqual(Vector3.Zero, buffer.GetInstantaneousVelocity());
        }
    }
}
