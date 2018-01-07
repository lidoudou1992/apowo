using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using System;

public class ExportConfig {

    public static string configRoot = Path.Combine(Application.dataPath, "Config");

    public static string exportRoot = "Assets/StreamingAssets/";
    public static string bundleExtension = ".ab";

    [MenuItem("AssetBundle/ExportAssetBundles", false, 3)]
    public static void BuildChangedBundle()
    {
        CopyConfigToClient();

        BuildTarget buildTarget = GetBuildPlatform();

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
            //Debug.Log(string.Format("{0} AssetBundle加入列表", name));
        }

        BuildPipeline.BuildAssetBundles(exportRoot, abds.ToArray(), BuildAssetBundleOptions.None, buildTarget);

        Debug.Log("Config/Icon/Item/TestUI AssetBundle打包完成");
    }

    private static string getBuildConfigPath()
    {
        string applicationPath = Application.dataPath;
        string disk = applicationPath.Split(':')[0];
        //string configPath = disk + @":\Cat\client\BuildConfig.json";
        string configPath = Path.Combine(Application.dataPath, "../../../BuildConfig.json"); ;

        return configPath;
    }

    private static string readBuildPath()
    {
        string content="";

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
        string platform = JsonMapper.ToObject(readBuildPath())["CurrentPaltform"].ToString();
        return (BuildTarget)Enum.Parse(typeof(BuildTarget), platform);
    }

    //[MenuItem("AssetBundle/CopyConfig", false, 2)]
    public static void CopyConfigToClient()
    {
        string sourceRoot = Path.Combine(Application.dataPath, "../../Config");

        clearDir(configRoot);
        AssetDatabase.Refresh();
        AssetDatabase.RemoveUnusedAssetBundleNames();
        CopyDirectory(sourceRoot, configRoot);
        AssetDatabase.Refresh();
        importConfigAssets("Config");
        Debug.Log("配置文件更新成功");
    }

    private static void clearDir(string path)
    {
        DirectoryInfo target = new DirectoryInfo(path);
        target.Delete(true);
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
                string sourceFileName = files[i].Name;
                File.Copy(files[i].FullName, target.FullName + @"\" + sourceFileName.Split('.')[0] + "Config.json", true);

                //var importer = AssetImporter.GetAtPath(tgtDir + @"\" + sourceFileName.Split('.')[0] + "Config.json");
                //Debug.Log(tgtDir + @"\" + sourceFileName.Split('.')[0] + "Config.json");
                ////importer.assetBundleName = sourceFileName.Split('.')[0] + "Config";
                //Debug.Log(sourceFileName);
            }
        }

        DirectoryInfo[] dirs = source.GetDirectories();

        for (int j = 0; j < dirs.Length; j++)
        {
            CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
        }
    }

    private static void importConfigAssets(string configPath)
    {
        var fullPath = Application.dataPath + "/" + configPath + "/";
        var relativeLen = configPath.Length + 8; // Assets 长度
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
                    var basePath = fileInfo.FullName.Substring(fullPath.Length - relativeLen).Replace('\\', '/');
                    var importer = AssetImporter.GetAtPath(basePath);
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
