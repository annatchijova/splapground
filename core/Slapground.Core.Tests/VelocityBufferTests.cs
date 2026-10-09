using System;
using System.Numerics;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class VelocityBufferTests
    {
        private const float FrameInterval = 1f / 90f; // Quest-class hand tracking is commonly cited around 90Hz

        [Fact]
        public void ConstantVelocity_BothEstimatorsAgreeWithTruth()
        {
            var buffer = new VelocityBuffer(bufferSize: 30, lookbackSeconds: 0.05f);
            var trueVelocity = new Vector3(2f, 0f, 0f); // 2 m/s along x - a brisk hand swing
            var position = Vector3.Zero;

            for (int frame = 0; frame < 30; frame++)
            {
                float time = frame * FrameInterval;
                buffer.AddSample(time, position);
                position += trueVelocity * FrameInterval;
            }

            float instantaneousError = Vector3.Distance(buffer.GetInstantaneousVelocity(), trueVelocity);
            float bufferedError = Vector3.Distance(buffer.GetBufferedVelocity(), trueVelocity);

            Assert.True(instantaneousError < 0.01f, $"instantaneous error too high: {instantaneousError}");
            Assert.True(bufferedError < 0.01f, $"buffered error too high: {bufferedError}");
        }

        [Fact]
        public void NoSamples_ReturnsZero()
        {
            var buffer = new VelocityBuffer();
            Assert.Equal(Vector3.Zero, buffer.GetInstantaneousVelocity());
            Assert.Equal(Vector3.Zero, buffer.GetBufferedVelocity());
        }

        [Fact]
        public void OneSample_ReturnsZero()
        {
            var buffer = new VelocityBuffer();
            buffer.AddSample(0f, Vector3.Zero);
            Assert.Equal(Vector3.Zero, buffer.GetInstantaneousVelocity());
            Assert.Equal(Vector3.Zero, buffer.GetBufferedVelocity());
        }

        [Fact]
        public void OutOfOrderSample_Throws()
        {
            var buffer = new VelocityBuffer();
            buffer.AddSample(1f, Vector3.Zero);
            Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AddSample(0.5f, Vector3.Zero));
        }

        /// <summary>
        /// The actual empirical question behind D1: does buffering really reduce
        /// error from a single glitched frame, and by how much? Not assumed -
        /// measured. Simulates a hand swinging at a constant 2 m/s for ~300ms
        /// (realistic approach-to-slap duration), then the LAST sample (the
        /// collision frame itself, the worst case - a real glitch is most likely
        /// to land exactly on the frame being queried) is corrupted by a 15cm
        /// positional error, a plausible single-frame occlusion/misdetection
        /// jump at slap speed.
        /// </summary>
        [Fact]
        public void SingleFrameGlitchOnTheLatestSample_BufferedEstimateHasLessError_ButIsNotImmune()
        {
            var buffer = new VelocityBuffer(bufferSize: 30, lookbackSeconds: 0.05f);
            var trueVelocity = new Vector3(2f, 0f, 0f);
            var position = Vector3.Zero;

            int totalFrames = 27; // ~300ms at 90Hz
            for (int frame = 0; frame < totalFrames - 1; frame++)
            {
                buffer.AddSample(frame * FrameInterval, position);
                position += trueVelocity * FrameInterval;
            }

            // Last frame: true position would be `position`, but report a 15cm glitch.
            var glitchedPosition = position + new Vector3(0.15f, 0f, 0f);
            buffer.AddSample((totalFrames - 1) * FrameInterval, glitchedPosition);

            float instantaneousError = Vector3.Distance(buffer.GetInstantaneousVelocity(), trueVelocity);
            float bufferedError = Vector3.Distance(buffer.GetBufferedVelocity(), trueVelocity);

            // Buffered should be meaningfully better - not just numerically different.
            Assert.True(bufferedError < instantaneousError / 2f,
                $"buffered ({bufferedError}) should be well under half of instantaneous ({instantaneousError}) for this to count as a real improvement");

            // Honesty check: buffering dilutes the glitch, it does not erase it,
            // because the glitched sample is still one of the two endpoints used.
            // If this ever starts failing (bufferedError ~= 0), the test data or
            // the implementation changed in a way worth re-examining, not a bug
            // to "fix" by deleting the assertion.
            Assert.True(bufferedError > 0.5f,
                $"buffered error unexpectedly near zero ({bufferedError}) - the glitched sample is still an endpoint of this estimate, some error should remain");
        }
    }
}
