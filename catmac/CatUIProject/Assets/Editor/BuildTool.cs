using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class BuildTool {

    static void HelloWorld() 
    {
        Debug.Log("Hello World!");
    }

    static void PreparePackage() {
        // make sure Assets/StreamingAssets exists
        if (!Directory.Exists("Assets/StreamingAssets"))
            Directory.CreateDirectory("Assets/StreamingAssets");

        ExportBundle.BuildChangedBundle();
        ExportBundle.CopyBundleToClient();
    }

    static void BuildAndroid()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        List<string> scenePathList = new List<string>();

        for (int i = 0; i < scenes.Length; i++)
        {
            EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
            if (scene.enabled)
                scenePathList.Add(scene.path);
        }
        if (!Directory.Exists("Bin\\Android"))
            Directory.CreateDirectory("Bin\\Android");
        EditorUserBuildSettings.androidBuildSubtarget = EditorUserBuildSettings.androidBuildSubtarget | MobileTextureSubtarget.ETC2;
        BuildPipeline.BuildPlayer(scenePathList.ToArray(), "Bin\\Android\\Android.apk", BuildTarget.Android, BuildOptions.None);
    }

    static void BuildWindows()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        List<string> scenePathList = new List<string>();

        for (int i = 0; i < scenes.Length; i++)
        {
            EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
            if (scene.enabled)
                scenePathList.Add(scene.path);
        }
        if (!Directory.Exists("Bin\\Win32"))
            Directory.CreateDirectory("Bin\\Win32");
        EditorUserBuildSettings.androidBuildSubtarget = EditorUserBuildSettings.androidBuildSubtarget | MobileTextureSubtarget.ETC2;
        BuildPipeline.BuildPlayer(scenePathList.ToArray(), "Bin\\Win32\\PicaWorld.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }
}
