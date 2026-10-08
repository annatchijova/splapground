using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Slapground.Spike
{
    /// <summary>
    /// Listens for collisions between a hand-bone physics proxy (HandPhysicsRig)
    /// and the spike object (SpikeObjectMarker), and records one impact event
    /// per contact: the buffered pre-impact hand velocity (D1 estimator, not the
    /// raw contact-frame velocity), the object's resulting velocity, and which
    /// bone made contact. This is the actual metric the protocol in
    /// README_TECHNICAL.md section 4 needs: "estimated impact velocity,
    /// collisions and failures."
    /// </summary>
    [RequireComponent(typeof(SpikeObjectMarker))]
    public class SlapImpactRecorder : MonoBehaviour
    {
        [SerializeField] private SpikeSessionController session;

        private StreamWriter writer;
        private SpikeObjectMarker marker;

        private void Awake()
        {
            marker = GetComponent<SpikeObjectMarker>();
        }

        private void OnEnable()
        {
            string dir = Path.Combine(Application.persistentDataPath, "spike_logs");
            Directory.CreateDirectory(dir);
            string stamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            string path = Path.Combine(dir, $"impacts_{stamp}.csv");
            writer = new StreamWriter(path, append: false, Encoding.UTF8);
            writer.WriteLine("time,stage,trial,hand,bone,buffered_impact_speed,instantaneous_speed,object_post_impact_speed");
        }

        private void OnDisable()
        {
            writer?.Flush();
            writer?.Dispose();
            writer = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (writer == null) return;

            var handMarker = collision.collider.GetComponent<HandBoneMarker>();
            if (handMarker == null) return; // not a hand-bone contact (e.g. desk, floor)

            var buffer = handMarker.Rig != null ? handMarker.Rig.GetBuffer(handMarker.BoneId) : null;
            float bufferedSpeed = buffer != null ? buffer.GetBufferedVelocity().magnitude : -1f;
            float instantSpeed = buffer != null ? buffer.GetInstantaneousVelocity().magnitude : -1f;
            float objectSpeed = marker.Body != null ? marker.Body.linearVelocity.magnitude : -1f;

            var sb = new StringBuilder();
            sb.Append(Time.time.ToString("F4")).Append(',');
            sb.Append(session != null ? session.CurrentStage.ToString() : "UNSET").Append(',');
            sb.Append(session != null ? session.TrialCount.ToString() : "0").Append(',');
            sb.Append(handMarker.HandLabel).Append(',');
            sb.Append(handMarker.BoneId).Append(',');
            sb.Append(bufferedSpeed.ToString("F4")).Append(',');
            sb.Append(instantSpeed.ToString("F4")).Append(',');
            sb.Append(objectSpeed.ToString("F4"));

            writer.WriteLine(sb.ToString());
            writer.Flush(); // one event at a time; worth the I/O cost for crash-safety during a spike
        }
    }
}
