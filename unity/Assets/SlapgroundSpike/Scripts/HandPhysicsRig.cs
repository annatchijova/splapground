using System.Collections.Generic;
using UnityEngine;

namespace Slapground.Spike
{
    /// <summary>
    /// Gives a Meta Quest hand-tracked hand physical presence so it can
    /// transfer momentum to a Rigidbody object via PhysX collision instead of
    /// only driving grab/poke interactors.
    ///
    /// Attaches a small kinematic-rigidbody capsule collider to a subset of
    /// OVRSkeleton bones (palm + fingertips are enough for a slap gesture;
    /// the full 26-joint skeleton is overkill for Level 0 and adds collision
    /// noise). Kinematic because the hand's "velocity" comes from tracking,
    /// not from PhysX simulating it - the Rigidbody only needs to exist so
    /// PhysX computes a real collision response against the dynamic spike
    /// object.
    ///
    /// OVRSkeleton.Bones / OVRBone API verified against Meta's public Unity
    /// API reference on 2026-10-08. Field names on OVRBone (here assumed:
    /// Transform) should be confirmed against the installed package version
    /// before trusting this compiles - no Unity Editor was available to build
    /// this script.
    /// </summary>
    [RequireComponent(typeof(OVRSkeleton))]
    public class HandPhysicsRig : MonoBehaviour
    {
        [SerializeField] private string handLabel = "Hand";
        [SerializeField] private float colliderRadius = 0.012f;
        [SerializeField] private List<OVRSkeleton.BoneId> trackedBoneIds = new List<OVRSkeleton.BoneId>
        {
            OVRSkeleton.BoneId.Hand_WristRoot,
            OVRSkeleton.BoneId.Hand_IndexTip,
            OVRSkeleton.BoneId.Hand_MiddleTip,
            OVRSkeleton.BoneId.Hand_RingTip,
            OVRSkeleton.BoneId.Hand_PinkyTip,
            OVRSkeleton.BoneId.Hand_ThumbTip,
        };

        private OVRSkeleton skeleton;
        private readonly Dictionary<OVRSkeleton.BoneId, HandVelocityBuffer> buffers =
            new Dictionary<OVRSkeleton.BoneId, HandVelocityBuffer>();
        private bool rigBuilt;

        public string HandLabel => handLabel;

        /// <summary>Velocity buffer for a given bone, for SlapImpactRecorder to query on contact.</summary>
        public HandVelocityBuffer GetBuffer(OVRSkeleton.BoneId boneId)
        {
            return buffers.TryGetValue(boneId, out var buf) ? buf : null;
        }

        private void Awake()
        {
            skeleton = GetComponent<OVRSkeleton>();
        }

        private void Update()
        {
            if (rigBuilt || skeleton == null || !skeleton.IsInitialized) return;
            BuildRig();
        }

        private void BuildRig()
        {
            foreach (var bone in skeleton.Bones)
            {
                if (!trackedBoneIds.Contains(bone.Id)) continue;

                var proxy = new GameObject($"{handLabel}_{bone.Id}_PhysicsProxy");
                proxy.transform.SetParent(bone.Transform, worldPositionStays: false);
                proxy.transform.localPosition = Vector3.zero;

                var collider = proxy.AddComponent<SphereCollider>();
                collider.radius = colliderRadius;
                collider.isTrigger = false;

                var rb = proxy.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;

                var buffer = proxy.AddComponent<HandVelocityBuffer>();

                var marker = proxy.AddComponent<HandBoneMarker>();
                marker.HandLabel = handLabel;
                marker.BoneId = bone.Id;
                marker.Rig = this;

                buffers[bone.Id] = buffer;
            }

            rigBuilt = true;
        }
    }

    /// <summary>Tag component on a per-bone physics proxy, read by SlapImpactRecorder on collision.</summary>
    public class HandBoneMarker : MonoBehaviour
    {
        public string HandLabel;
        public OVRSkeleton.BoneId BoneId;
        public HandPhysicsRig Rig;
    }
}
