using System;
using System.IO;
using UnityEngine;

namespace Slapground.Spike
{
    public enum SpikeStage
    {
        A1_FixedObject,
        A2_SecondHandNearby,
        A3_SecondHandHolding,
    }

    /// <summary>
    /// Orchestrates one Level 0 spike session (README_TECHNICAL.md section 4).
    /// Selects the active stage (A1/A2/A3), resets the spike object between
    /// trials without restarting the app ("the interaction can be repeated
    /// without restarting the experience" - product brief), and stamps a
    /// session manifest so every CSV produced this run can be tied back to
    /// one stage/trial sequence.
    ///
    /// This is throwaway Level 0 instrumentation per the repo's own README -
    /// not shaped like a game, not meant to be extended into Level 1.
    /// </summary>
    public class SpikeSessionController : MonoBehaviour
    {
        [SerializeField] private SpikeStage currentStage = SpikeStage.A1_FixedObject;
        [SerializeField] private Transform spikeObject;
        [SerializeField] private Vector3 spikeObjectResetPosition;
        [SerializeField] private Quaternion spikeObjectResetRotation = Quaternion.identity;

        public SpikeStage CurrentStage => currentStage;
        public int TrialCount { get; private set; }

        private string sessionId;

        private void Awake()
        {
            sessionId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            if (spikeObject != null)
            {
                spikeObjectResetPosition = spikeObject.position;
                spikeObjectResetRotation = spikeObject.rotation;
            }
        }

        private void Start()
        {
            WriteSessionManifest();
        }

        /// <summary>
        /// Wire this to a controller-free trigger reachable on-device (an
        /// Interaction SDK poke button in the scene, or - during Editor/Link
        /// testing only - a keyboard key via a small bridge script). Resets
        /// the spike object's pose and velocity and advances the trial
        /// counter so loggers can segment the CSV by trial.
        /// </summary>
        public void ResetTrial()
        {
            if (spikeObject != null)
            {
                spikeObject.SetPositionAndRotation(spikeObjectResetPosition, spikeObjectResetRotation);
                var rb = spikeObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }

            TrialCount++;
        }

        public void SetStage(SpikeStage stage)
        {
            currentStage = stage;
        }

        private void WriteSessionManifest()
        {
            string dir = Path.Combine(Application.persistentDataPath, "spike_logs");
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, $"session_{sessionId}.txt");
            File.WriteAllText(path,
                $"session_id={sessionId}\n" +
                $"initial_stage={currentStage}\n" +
                $"app_version={Application.version}\n" +
                $"unity_version={Application.unityVersion}\n" +
                $"platform={Application.platform}\n");
        }
    }
}
