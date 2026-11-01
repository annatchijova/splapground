using System;
using System.Linq;
using System.Text;

public static class ProbeAssemblies
{
    public static string Run()
    {
        var sb = new StringBuilder();
        var t = Type.GetType("OVRProjectSetup");
        sb.AppendLine("GetType(OVRProjectSetup): " + (t == null ? "null" : t.AssemblyQualifiedName));
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies().OrderBy(x => x.GetName().Name))
        {
            var n = a.GetName().Name;
            if (n.IndexOf("OVR", StringComparison.OrdinalIgnoreCase) >= 0
                || n.IndexOf("Oculus", StringComparison.OrdinalIgnoreCase) >= 0
                || n.IndexOf("Meta", StringComparison.OrdinalIgnoreCase) >= 0
                || n == "Assembly-CSharp" || n == "Assembly-CSharp-Editor")
                sb.AppendLine("ASM: " + n);
        }
        return sb.ToString();
    }
}
