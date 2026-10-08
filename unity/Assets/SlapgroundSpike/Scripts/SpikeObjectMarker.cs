using UnityEngine;

namespace Slapground.Spike
{
    /// <summary>Tags the single Level 0 spike target object so SlapImpactRecorder can find it.</summary>
    [RequireComponent(typeof(Rigidbody))]
    public class SpikeObjectMarker : MonoBehaviour
    {
        private Rigidbody rb;
        public Rigidbody Body => rb ??= GetComponent<Rigidbody>();
    }
}
