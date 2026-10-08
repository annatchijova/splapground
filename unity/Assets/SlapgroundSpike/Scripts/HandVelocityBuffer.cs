using System.Collections.Generic;
using UnityEngine;

namespace Slapground.Spike
{
    /// <summary>
    /// Ring buffer of recent (time, position) samples for one tracked transform.
    ///
    /// D1 (README_TECHNICAL.md section 4): Table Troopers does not trust the hand's exact
    /// instantaneous position at the triggering frame, even for a slow pinch-release.
    /// For a fast two-handed slap the single contact frame is an even worse estimator
    /// of true impact velocity. This buffer lets callers ask for velocity computed
    /// from an earlier valid sample pair instead of the current frame alone.
    /// </summary>
    public class HandVelocityBuffer : MonoBehaviour
    {
        [SerializeField] private Transform tracked;
        [SerializeField] private int bufferSize = 30; // ~0.5s at 60Hz, ~0.25s at 120Hz
        [SerializeField] private float lookbackSeconds = 0.05f;

        private struct Sample
        {
            public float time;
            public Vector3 position;
        }

        private readonly Queue<Sample> samples = new Queue<Sample>();

        public Transform Tracked => tracked;

        private void Reset()
        {
            tracked = transform;
        }

        private void Update()
        {
            if (tracked == null) return;

            samples.Enqueue(new Sample { time = Time.time, position = tracked.position });
            while (samples.Count > bufferSize)
            {
                samples.Dequeue();
            }
        }

        /// <summary>
        /// Instantaneous velocity from the two most recent samples. Exposed for
        /// comparison against the buffered estimate, not for production use -
        /// this is exactly the single-frame signal D1 says not to trust.
        /// </summary>
        public Vector3 GetInstantaneousVelocity()
        {
            if (samples.Count < 2) return Vector3.zero;
            var arr = samples.ToArray();
            var a = arr[arr.Length - 2];
            var b = arr[arr.Length - 1];
            float dt = b.time - a.time;
            if (dt <= 0f) return Vector3.zero;
            return (b.position - a.position) / dt;
        }

        /// <summary>
        /// Velocity estimated from a sample taken approximately lookbackSeconds
        /// before now, up to the most recent sample. This is the D1 estimator:
        /// it deliberately avoids anchoring on the single frame closest to the
        /// reported collision, which is the frame most likely to carry a
        /// tracking glitch (occlusion, fast motion blur) during a real impact.
        /// </summary>
        public Vector3 GetBufferedVelocity()
        {
            if (samples.Count < 2) return Vector3.zero;

            var arr = samples.ToArray();
            var latest = arr[arr.Length - 1];
            float targetTime = latest.time - lookbackSeconds;

            Sample earlier = arr[0];
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                if (arr[i].time <= targetTime)
                {
                    earlier = arr[i];
                    break;
                }
                earlier = arr[i];
            }

            float dt = latest.time - earlier.time;
            if (dt <= 0f) return Vector3.zero;
            return (latest.position - earlier.position) / dt;
        }
    }
}
