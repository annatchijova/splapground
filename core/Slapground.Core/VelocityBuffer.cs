using System;
using System.Collections.Generic;
using System.Numerics;

namespace Slapground.Core
{
    /// <summary>
    /// D1's windowed/buffered velocity estimator (README_TECHNICAL.md section 2
    /// and 4), ported from the unverified Unity spike script
    /// (unity/Assets/SlapgroundSpike/Scripts/HandVelocityBuffer.cs) into this
    /// engine-independent, actually-tested layer. Uses System.Numerics.Vector3,
    /// not UnityEngine.Vector3 - zero Unity dependency, same as the rest of
    /// Slapground.Core.
    ///
    /// Table Troopers (README_TECHNICAL.md section 5) doesn't trust the hand's
    /// raw position at the triggering frame even for a slow pinch-release; for a
    /// fast slap the single contact frame is a worse estimator still. This ring-
    /// buffers recent (time, position) samples and can answer either "naive"
    /// (last two samples) or "buffered" (anchored ~lookbackSeconds back) velocity.
    /// </summary>
    public class VelocityBuffer
    {
        private readonly struct Sample
        {
            public readonly float Time;
            public readonly Vector3 Position;
            public Sample(float time, Vector3 position) { Time = time; Position = position; }
        }

        public int BufferSize { get; }
        public float LookbackSeconds { get; }

        private readonly Queue<Sample> samples;

        public VelocityBuffer(int bufferSize = 30, float lookbackSeconds = 0.05f)
        {
            if (bufferSize < 2) throw new ArgumentOutOfRangeException(nameof(bufferSize), bufferSize, "Need at least 2 samples to compute any velocity.");
            if (lookbackSeconds <= 0f) throw new ArgumentOutOfRangeException(nameof(lookbackSeconds));

            BufferSize = bufferSize;
            LookbackSeconds = lookbackSeconds;
            samples = new Queue<Sample>(bufferSize);
        }

        public int SampleCount => samples.Count;

        /// <summary>Samples must arrive in non-decreasing time order - this is a live stream, not a sort.</summary>
        public void AddSample(float time, Vector3 position)
        {
            if (samples.Count > 0)
            {
                float lastTime = default;
                foreach (var s in samples) lastTime = s.Time; // last enqueued (Queue has no direct "last" accessor)
                if (time < lastTime) throw new ArgumentOutOfRangeException(nameof(time), time, "Samples must be non-decreasing in time.");
            }

            samples.Enqueue(new Sample(time, position));
            while (samples.Count > BufferSize) samples.Dequeue();
        }

        /// <summary>Velocity from the two most recent samples - the single-contact-frame estimator D1 says not to trust alone.</summary>
        public Vector3 GetInstantaneousVelocity()
        {
            if (samples.Count < 2) return Vector3.Zero;
            var arr = ToArray();
            var a = arr[arr.Length - 2];
            var b = arr[arr.Length - 1];
            float dt = b.Time - a.Time;
            return dt > 0f ? (b.Position - a.Position) / dt : Vector3.Zero;
        }

        /// <summary>Velocity anchored between the latest sample and one approximately LookbackSeconds earlier.</summary>
        public Vector3 GetBufferedVelocity()
        {
            if (samples.Count < 2) return Vector3.Zero;
            var arr = ToArray();
            var latest = arr[arr.Length - 1];
            float targetTime = latest.Time - LookbackSeconds;

            Sample earlier = arr[0];
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                earlier = arr[i];
                if (arr[i].Time <= targetTime) break;
            }

            float dt = latest.Time - earlier.Time;
            return dt > 0f ? (latest.Position - earlier.Position) / dt : Vector3.Zero;
        }

        private Sample[] ToArray()
        {
            var arr = new Sample[samples.Count];
            samples.CopyTo(arr, 0);
            return arr;
        }
    }
}
