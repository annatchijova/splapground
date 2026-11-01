using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Slapground.Spike;

public static class SpikeSceneBuilder
{
    const string ScenePath = "Assets/SlapgroundSpike/Scenes/Spike_A.unity";
    const string RigPrefabPath = "Packages/com.meta.xr.sdk.core/Prefabs/OVRCameraRig.prefab";

    public static void BuildSpikeA()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        Debug.Log("[SCENE] new empty scene created");

        var lightGO = new GameObject("Directional Light");
        var light = lightGO.AddComponent<Light>();
        light.type = LightType.Directional;
        lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        var rigPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RigPrefabPath);
        if (rigPrefab == null)
        {
            Debug.LogError($"[SCENE] rig prefab not found at {RigPrefabPath}");
            return;
        }
        var rig = PrefabUtility.InstantiatePrefab(rigPrefab) as GameObject;
        rig.name = "OVRCameraRig";
        Debug.Log("[SCENE] OVRCameraRig instantiated");

        var leftAnchor = FindDeep(rig.transform, "LeftHandAnchor");
        var rightAnchor = FindDeep(rig.transform, "RightHandAnchor");
        if (leftAnchor == null || rightAnchor == null)
        {
            Debug.LogError($"[SCENE] hand anchors missing: left={leftAnchor != null} right={rightAnchor != null}");
            return;
        }
        Debug.Log($"[SCENE] anchors: {FullPath(leftAnchor)} | {FullPath(rightAnchor)}");

        var leftRig = SetupHand(leftAnchor, "Left", OVRHand.Hand.HandLeft);
        var rightRig = SetupHand(rightAnchor, "Right", OVRHand.Hand.HandRight);
        var leftLogger = SetupLogger(leftAnchor, "Left");
        var rightLogger = SetupLogger(rightAnchor, "Right");
        if (leftRig == null || rightRig == null || leftLogger == null || rightLogger == null) return;

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "SpikeObject";
        cube.transform.position = new Vector3(0f, 1.0f, 0.6f);
        cube.transform.localScale = Vector3.one * 0.15f;
        var rb = cube.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        cube.AddComponent<SpikeObjectMarker>();
        var recorder = cube.AddComponent<SlapImpactRecorder>();
        Debug.Log("[SCENE] spike cube created at (0, 1.0, 0.6), RigidBody non-kinematic");

        var sessionGO = new GameObject("SpikeSession");
        var session = sessionGO.AddComponent<SpikeSessionController>();
        var bridge = sessionGO.AddComponent<EditorTrialResetBridge>();

        SetObjRef(session, "spikeObject", cube.transform, "SpikeSessionController");
        SetObjRef(leftLogger, "session", session, "HandConfidenceLogger(Left)");
        SetObjRef(rightLogger, "session", session, "HandConfidenceLogger(Right)");
        SetObjRef(recorder, "session", session, "SlapImpactRecorder");
        SetObjRef(bridge, "session", session, "EditorTrialResetBridge");

        var ptGO = new GameObject("PassthroughLayer");
        ptGO.AddComponent<OVRPassthroughLayer>();
        RenderSettings.skybox = null;
        Debug.Log("[SCENE] passthrough layer added, skybox cleared (passthrough tuning is a Path B concern)");

        Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.SaveAssets();
        Debug.Log($"[SCENE] saved {ScenePath}");
    }

    static HandPhysicsRig SetupHand(Transform anchor, string label, OVRHand.Hand handSide)
    {
        var skel = anchor.GetComponent<OVRSkeleton>() ?? anchor.gameObject.AddComponent<OVRSkeleton>();
        var hand = anchor.GetComponent<OVRHand>() ?? anchor.gameObject.AddComponent<OVRHand>();
        SetHandType(hand, handSide, label);
        Debug.Log($"[SCENE] {label} anchor: OVRHand HandType={handSide}, OVRSkeleton present={skel != null}, IsInitialized={skel.IsInitialized}");
        var rig = anchor.GetComponent<HandPhysicsRig>() ?? anchor.gameObject.AddComponent<HandPhysicsRig>();
        SetString(rig, "handLabel", label, $"HandPhysicsRig({label})");
        return rig;
    }

    static void SetHandType(OVRHand hand, OVRHand.Hand value, string label)
    {
        var so = new SerializedObject(hand);
        var p = so.FindProperty("HandType");
        if (p == null) { Debug.LogError($"[SCENE] {label}: OVRHand.HandType property not found"); return; }
        p.intValue = (int)value;
        so.ApplyModifiedProperties();
    }

    static HandConfidenceLogger SetupLogger(Transform anchor, string label)
    {
        var logger = anchor.GetComponent<HandConfidenceLogger>() ?? anchor.gameObject.AddComponent<HandConfidenceLogger>();
        SetString(logger, "handLabel", label, $"HandConfidenceLogger({label})");
        return logger;
    }

    static Transform FindDeep(Transform root, string name)
    {
        if (root.name == name) return root;
        foreach (Transform child in root)
        {
            var found = FindDeep(child, name);
            if (found != null) return found;
        }
        return null;
    }

    static string FullPath(Transform t)
    {
        string p = t.name;
        while (t.parent != null) { t = t.parent; p = t.name + "/" + p; }
        return p;
    }

    static void SetString(Object comp, string prop, string value, string who)
    {
        var so = new SerializedObject(comp);
        var p = so.FindProperty(prop);
        if (p == null) { Debug.LogError($"[SCENE] {who}: property '{prop}' not found"); return; }
        p.stringValue = value;
        so.ApplyModifiedProperties();
    }

    static void SetObjRef(Object comp, string prop, Object value, string who)
    {
        var so = new SerializedObject(comp);
        var p = so.FindProperty(prop);
        if (p == null) { Debug.LogError($"[SCENE] {who}: property '{prop}' not found"); return; }
        p.objectReferenceValue = value;
        so.ApplyModifiedProperties();
    }
}
