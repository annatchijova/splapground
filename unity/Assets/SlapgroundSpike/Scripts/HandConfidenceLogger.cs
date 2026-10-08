using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Slapground.Spike
{
    /// <summary>
    /// Logs OVRHand.HandConfidence and per-finger confidence as a continuous
    /// time series, per README_TECHNICAL.md section 4: "logged as a continuous
    /// time series through the impact window - not a single boolean."
    ///
    /// OVRHand API used here (verified against Meta's public Unity API reference
    /// on 2026-10-08, not assumed from memory):
    ///   - OVRHand.HandConfidence -> OVRHand.TrackingConfidence (Low/High)
    ///   - OVRHand.GetFingerConfidence(HandFinger) -> OVRHand.TrackingConfidence
    /// This is unverified against the actual installed package version, because
    /// no Unity Editor with the Meta XR SDK installed was available to compile
    /// this script. Confirm field/enum names compile against the package
    /// version you install before trusting this logger's output.
    /// </summary>
    [RequireComponent(typeof(OVRHand))]
    public class HandConfidenceLogger : MonoBehaviour
    {
        [SerializeField] private string handLabel = "Hand";
        [SerializeField] private SpikeSessionController session;

        private OVRHand hand;
        private StreamWriter writer;
        private static readonly OVRHand.HandFinger[] Fingers =
        {
            OVRHand.HandFinger.Thumb,
            OVRHand.HandFinger.Index,
            OVRHand.HandFinger.Middle,
            OVRHand.HandFinger.Ring,
            OVRHand.HandFinger.Pinky,
        };

        private void Awake()
        {
            hand = GetComponent<OVRHand>();
        }

        private void OnEnable()
        {
            OpenWriter();
        }

        private void OnDisable()
        {
            writer?.Flush();
            writer?.Dispose();
            writer = null;
        }

        private void OpenWriter()
        {
            string dir = Path.Combine(Application.persistentDataPath, "spike_logs");
            Directory.CreateDirectory(dir);
            string stamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            string path = Path.Combine(dir, $"hand_confidence_{handLabel}_{stamp}.csv");
            writer = new StreamWriter(path, append: false, Encoding.UTF8);
            writer.WriteLine("time,stage,trial,hand,hand_confidence,thumb,index,middle,ring,pinky");
        }

        private void Update()
        {
            if (hand == null || writer == null) return;

            var sb = new StringBuilder();
            sb.Append(Time.time.ToString("F4")).Append(',');
            sb.Append(session != null ? session.CurrentStage.ToString() : "UNSET").Append(',');
            sb.Append(session != null ? session.TrialCount.ToString() : "0").Append(',');
            sb.Append(handLabel).Append(',');
            sb.Append(hand.HandConfidence);

            foreach (var finger in Fingers)
            {
                sb.Append(',').Append(hand.GetFingerConfidence(finger));
            }

            writer.WriteLine(sb.ToString());
        }
    }
}
