using System;
using System.Collections;
using System.Reflection;
using System.Text;
using UnityEditor;

public static class ListUpstIssuesStandalone
{
    public static string Run()
    {
        var group = BuildTargetGroup.Standalone;
        var getTasks = typeof(OVRProjectSetup).GetMethod(
            "GetTasks", BindingFlags.NonPublic | BindingFlags.Static);
        if (getTasks == null) return "GetTasks not found";
        var tasks = (IEnumerable)getTasks.Invoke(null, new object[] { group });
        var sb = new StringBuilder();
        int n = 0;
        foreach (var task in tasks)
        {
            var t = task.GetType();
            var isDone = t.GetProperty("IsDone").GetValue(task) as Func<BuildTargetGroup, bool>;
            if (isDone != null && isDone(group)) continue;
            n++;
            var msgObj = t.GetProperty("Message").GetValue(task);
            string msg = msgObj?.GetType().GetMethod("GetValue")
                .Invoke(msgObj, new object[] { group })?.ToString();
            var lvlObj = t.GetProperty("Level").GetValue(task);
            string level = lvlObj?.GetType().GetMethod("GetValue")
                .Invoke(lvlObj, new object[] { group })?.ToString();
            bool auto = t.GetProperty("FixAction").GetValue(task) != null
                     || t.GetProperty("AsyncFixAction").GetValue(task) != null;
            sb.AppendLine($"[{level}] {(auto ? "auto" : "manual")} — {msg}");
        }
        if (n == 0) return "UPST Standalone: all tasks done.";
        return sb.ToString();
    }
}

public static class FixAllStandalone
{
    public static string Run()
    {
        OVRProjectSetup.FixAllAsync(BuildTargetGroup.Standalone);
        return "FixAllAsync(Standalone) queued; verify with ListUpstIssuesStandalone in a follow-up call.";
    }
}
