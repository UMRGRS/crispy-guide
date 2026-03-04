using UnityEditor;
using UnityEngine;

public class BuildScript
{
    public static void PerformBuild()
    {
        string buildPath = "dist/crispy-guide.apk";

        BuildPipeline.BuildPlayer(
            EditorBuildSettings.scenes,
            buildPath,
            BuildTarget.StandaloneLinux64,
            BuildOptions.None
        );

        Debug.Log("Build completed");
    }
}