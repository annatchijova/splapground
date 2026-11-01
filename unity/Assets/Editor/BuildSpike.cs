using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class BuildSpike
{
    public static void BuildLinux()
    {
        var scenes = new[] { "Assets/SlapgroundSpike/Scenes/Spike_A.unity" };
        var list = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        foreach (var s in scenes)
        {
            bool found = false;
            foreach (var e in list) { if (e.path == s) { found = true; break; } }
            if (!found) list.Add(new EditorBuildSettingsScene(s, true));
        }
        EditorBuildSettings.scenes = list.ToArray();
        string dir = "Builds/Spike";
        Directory.CreateDirectory(dir);
        var opts = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = dir + "/Spike",
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.None,
        };
        var report = BuildPipeline.BuildPlayer(opts);
        Debug.Log($"[BUILD] result={report.summary.result} errors={report.summary.totalErrors} warnings={report.summary.totalWarnings} size={report.summary.totalSize} path={opts.locationPathName}");
        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            EditorApplication.Exit(1);
    }
}
