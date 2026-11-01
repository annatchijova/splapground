using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class UpstBatchFix
{
    public static void FixStandalone()
    {
        var group = BuildTargetGroup.Standalone;
        Type setupType = typeof(OVRProjectSetup);
        var getTasks = setupType.GetMethod("GetTasks", BindingFlags.NonPublic | BindingFlags.Static);
        if (getTasks == null)
        {
            Debug.LogError("[UPST] GetTasks method not found on OVRProjectSetup.");
            return;
        }
        var tasks = (IEnumerable)getTasks.Invoke(null, new object[] { group });
        int fixedCount = 0, asyncCount = 0, manualCount = 0, doneCount = 0;
        foreach (var task in tasks)
        {
            var t = task.GetType();
            var isDone = t.GetProperty("IsDone").GetValue(task) as Func<BuildTargetGroup, bool>;
            if (isDone != null && isDone(group)) { doneCount++; continue; }
            var msgObj = t.GetProperty("Message").GetValue(task);
            string msg = msgObj?.GetType().GetMethod("GetValue")
                .Invoke(msgObj, new object[] { group })?.ToString();
            var lvlObj = t.GetProperty("Level").GetValue(task);
            string level = lvlObj?.GetType().GetMethod("GetValue")
                .Invoke(lvlObj, new object[] { group })?.ToString();
            var fix = t.GetProperty("FixAction").GetValue(task) as Action<BuildTargetGroup>;
            var asyncFix = t.GetProperty("AsyncFixAction").GetValue(task);
            if (fix != null)
            {
                try { fix(group); fixedCount++; Debug.Log($"[UPST] fixed [{level}]: {msg}"); }
                catch (Exception ex) { Debug.LogError($"[UPST] fix threw [{level}]: {msg} :: {ex.Message}"); }
            }
            else if (asyncFix != null) { asyncCount++; Debug.LogWarning($"[UPST] async-only [{level}]: {msg}"); }
            else { manualCount++; Debug.LogWarning($"[UPST] manual [{level}]: {msg}"); }
        }
        AssetDatabase.SaveAssets();
        Debug.Log($"[UPST] summary: alreadyDone={doneCount} fixed={fixedCount} asyncOnly={asyncCount} manual={manualCount}");
    }
}
