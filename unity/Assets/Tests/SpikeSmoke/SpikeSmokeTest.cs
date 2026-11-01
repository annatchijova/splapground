using System;
using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Slapground.Spike;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SpikeSmokeTest
{
    const string ScenePath = "Assets/SlapgroundSpike/Scenes/Spike_A.unity";

    [UnityTest]
    public IEnumerator FullSession_ProducesCsvsAndCompletes()
    {
        var testStart = DateTime.UtcNow;
        SceneManager.LoadScene(ScenePath);
        yield return null;
        yield return null;

        var session = UnityEngine.Object.FindFirstObjectByType<SpikeSessionController>();
        Assert.IsNotNull(session, "SpikeSessionController present in Spike_A");
        var recorder = UnityEngine.Object.FindFirstObjectByType<SlapImpactRecorder>();
        Assert.IsNotNull(recorder, "SlapImpactRecorder present in Spike_A");

        var hammer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hammer.name = "TestHammer";
        hammer.transform.position = new Vector3(0f, 3.0f, 0.6f);
        hammer.transform.localScale = Vector3.one * 0.3f;
        var hammerBody = hammer.AddComponent<Rigidbody>();
        hammerBody.mass = 5f;

        string dir = Path.Combine(Application.persistentDataPath, "spike_logs");
        Time.timeScale = 20f;
        try
        {
            float deadline = Time.realtimeSinceStartup + 90f;
            string summaryPath = null;
            while (Time.realtimeSinceStartup < deadline)
            {
                if (Directory.Exists(dir))
                {
                    summaryPath = Directory.GetFiles(dir, "session_*.txt")
                        .Where(f => File.GetCreationTimeUtc(f) >= testStart.AddSeconds(-5))
                        .OrderByDescending(f => File.GetCreationTimeUtc(f))
                        .FirstOrDefault();
                    if (summaryPath != null) break;
                }
                yield return null;
            }
            Assert.IsNotNull(summaryPath, "session summary file was written (session ran to completion)");
            string summary = File.ReadAllText(summaryPath);
            Assert.IsTrue(summary.Contains("stage=A3_Finale"), $"summary shows finale stage. Got: {summary}");

            var impactFiles = Directory.GetFiles(dir, "impacts_*.csv")
                .Where(f => File.GetCreationTimeUtc(f) >= testStart.AddSeconds(-5)).ToArray();
            Assert.IsNotEmpty(impactFiles, "impacts CSV exists");
            int impactRows = File.ReadAllLines(impactFiles.OrderByDescending(f => File.GetCreationTimeUtc(f)).First()).Length - 1;
            Assert.GreaterOrEqual(impactRows, 1, $"hammer drop produced >=1 impact row (got {impactRows})");

            var confFiles = Directory.GetFiles(dir, "hand_confidence_*.csv")
                .Where(f => File.GetCreationTimeUtc(f) >= testStart.AddSeconds(-5)).ToArray();
            Assert.GreaterOrEqual(confFiles.Length, 2, $"both hand confidence CSVs exist (got {confFiles.Length})");
            foreach (var f in confFiles)
            {
                int rows = File.ReadAllLines(f).Length - 1;
                Assert.Greater(rows, 100, $"{Path.GetFileName(f)} has substantial rows (got {rows})");
            }
        }
        finally
        {
            Time.timeScale = 1f;
        }
    }
}
