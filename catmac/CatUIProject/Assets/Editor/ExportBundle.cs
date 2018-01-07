using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using LitJson;
using System;

public class ExportBundle
{
    public static string exportRoot = "Assets/StreamingAssets/";
    public static string bundleExtension = ".ab";

    [MenuItem("ExportBundles/ExportAssetBundles %E", false, 1)]
    public static void BuildChangedBundle()
    {
        BuildTarget buildTarget = GetBuildPlatform();

        importAssets(Application.dataPath + "/Resources/Sound/");

        var names = AssetDatabase.GetAllAssetBundleNames();
        List<AssetBundleBuild> abds = new List<AssetBundleBuild>();
        for (int i = 0; i < names.Length; i++)
        {
            var name = names[i];
            var tempPath = AssetDatabase.GetAssetPathsFromAssetBundle(name);

            AssetBundleBuild abd = new AssetBundleBuild()
            {
                assetBundleName = name + bundleExtension,
                assetBundleVariant = null,
                assetNames = tempPath
            };

            abds.Add(abd);
            Debug.Log(string.Format("{0} AssetBundle加入列表", name));
        }

        BuildPipeline.BuildAssetBundles(exportRoot, abds.ToArray(), BuildAssetBundleOptions.None, buildTarget);
        CopyBundleToClient();
    }

   [MenuItem("ExportBundles/CopyBundleToClient", false, 1)]
    public static void CopyBundleToClient()
    {
        string clientDir = Path.Combine(Application.dataPath, "../../CatMainProject/Assets/StreamingAssets");

        if (GetBuildPlatform() == BuildTarget.iOS)
        {
            //clientDir = Path.Combine(Application.dataPath, "../../../../../client/branches/MacRelease/CatMainProject/Assets/StreamingAssets");
            clientDir = Path.Combine(Application.dataPath, "../../../../client/MacTrunk/CatMainProject/Assets/StreamingAssets");
        }

        CopyDirectory(exportRoot, clientDir);
        EditorUtility.DisplayDialog("Success", "复制成功", "Ok");
    }

    private static void CopyDirectory(string srcDir, string tgtDir)
    {
        DirectoryInfo source = new DirectoryInfo(srcDir);
        DirectoryInfo target = new DirectoryInfo(tgtDir);

        if (target.FullName.StartsWith(source.FullName, System.StringComparison.CurrentCultureIgnoreCase))
        {
            Debug.LogError("父目录不能拷贝到子目录！");
            return;
        }

        if (!source.Exists)
        {
            return;
        }

        if (!target.Exists)
        {
            target.Create();
        }

        FileInfo[] files = source.GetFiles();

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension != ".meta")
            {
                File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
            }
        }

        DirectoryInfo[] dirs = source.GetDirectories();

        for (int j = 0; j < dirs.Length; j++)
        {
            CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
        }
    }

    private static string getBuildConfigPath()
    {
        return Path.Combine(Application.dataPath, "../../../../client/BuildConfig.json");
        // string applicationPath = Application.dataPath;
        // string disk = applicationPath.Split(':')[0];
        // string configPath = disk + @":\Cat\client\BuildConfig.json";

        // return configPath;
    }

    private static string readBuildPath()
    {
        string content = "";

        // 读取文件的源路径及其读取流
        string strReadFilePath = getBuildConfigPath();
        StreamReader srReadFile = new StreamReader(strReadFilePath);
        // 读取流直至文件末尾结束
        while (!srReadFile.EndOfStream)
        {
            content += srReadFile.ReadLine();
        }
        // 关闭读取流文件
        srReadFile.Close();

        return content;
    }

    public static BuildTarget GetBuildPlatform()
    {
        return EditorUserBuildSettings.activeBuildTarget;
        // string platform = JsonMapper.ToObject(readBuildPath())["CurrentPaltform"].ToString();
        // return (BuildTarget)Enum.Parse(typeof(BuildTarget), platform);
    }

    private static void importAssets(string fullPath)
    {
        //var fullPath = Application.dataPath + "/Resources/Prefabs/" + path + "/";
        var relativeLen = Application.dataPath.Length - 6; // Assets 长度
        if (Directory.Exists(fullPath))
        {
            EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 0f);
            var dir = new DirectoryInfo(fullPath);
            var files = dir.GetFiles("*", SearchOption.AllDirectories);
            for (var i = 0; i < files.Length; ++i)
            {
                var fileInfo = files[i];
                EditorUtility.DisplayProgressBar("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);
                if (!fileInfo.Name.EndsWith(".meta"))
                {
                    var basePath = fileInfo.FullName.Replace('\\', '/').Substring(relativeLen);

                    var importer = AssetImporter.GetAtPath(basePath);
                    Debug.Log(importer + " " + basePath);
                    if (importer)
                    {
                        importer.assetBundleName = fileInfo.Name.Split('.')[0];
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }
    }
}
