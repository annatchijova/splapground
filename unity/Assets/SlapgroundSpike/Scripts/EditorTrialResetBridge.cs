using UnityEngine;

namespace Slapground.Spike
{
    /// <summary>
    /// Keyboard-triggered trial reset for Editor/Quest Link testing only.
    /// On-device (standalone Android build) there is no keyboard - wire a
    /// physical Interaction SDK poke button to SpikeSessionController.ResetTrial()
    /// for real headset sessions instead of relying on this bridge.
    /// </summary>
    public class EditorTrialResetBridge : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private SpikeSessionController session;
        [SerializeField] private KeyCode resetKey = KeyCode.R;

        private void Update()
        {
            if (session != null && Input.GetKeyDown(resetKey))
            {
                session.ResetTrial();
            }
        }
#endif
    }
}
