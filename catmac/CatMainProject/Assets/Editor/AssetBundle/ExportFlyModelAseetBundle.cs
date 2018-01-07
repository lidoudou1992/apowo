using UnityEngine;
using System.Collections;
using UnityEditor;

public class ExportFlyModelAseetBundle : MonoBehaviour {

    [MenuItem("AssetBundle/BuildHotFixDllBundle %I", false, 0)]
    public static void BuildHotFixDllBundle()
    {
        BuildTarget bt = ExportConfig.GetBuildPlatform();
        string root = "Assets/StreamingAssets";

        AssetBundleBuild lib = new AssetBundleBuild()
        {
            assetBundleName = "flymodel.dll" + ResourceLoader.bundleExtension,
            assetBundleVariant = null,
            assetNames = new string[] { "Assets/HotFix/FlyModel.dll.bytes" }
        };
        AssetBundleBuild pdb = new AssetBundleBuild()
        {
            assetBundleName = "flymodel.pdb" + ResourceLoader.bundleExtension,
            assetBundleVariant = null,
            assetNames = new string[] { "Assets/HotFix/FlyModel.pdb.bytes" }
        };

        BuildPipeline.BuildAssetBundles(root, new AssetBundleBuild[] { lib, pdb }, BuildAssetBundleOptions.ForceRebuildAssetBundle, bt);
    }
}
