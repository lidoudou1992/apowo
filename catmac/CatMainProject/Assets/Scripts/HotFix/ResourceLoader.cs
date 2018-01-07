using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.UI;
using CLRSharp;

public class ResourceLoader : MonoBehaviour
{
    #region AssetBundleManager
    public static string bundleExtension = ".ab";
    public static string WebRoot = "http://resource.apowogame.com/cat";

    public static string RemoteAssetBundleUrl
    {
        get
        {
            string re = WebRoot + "/assets/";
#if UNITY_EDITOR
            //re += "FlyWeb/";
            re += "Android/";
            //re += "IOS/";
#elif UNITY_STANDALONE_WIN
			re+="IOS/";
#elif UNITY_IPHONE
			re+="IOS/";
#elif UNITY_ANDROID
			re+="Android/";
#elif UNITY_WEBPLAYER
			re+="FlyWeb/";
#endif
            return re;
        }
    }

    
    public static string AssetBundleCacheUrl
    {
        get
        {
            string pu = "";
#if UNITY_STANDALONE_WIN || UNITY_EDITOR         
            pu = "file://" + Application.dataPath + "/StreamingAssets/";
            //Debug.LogWarning("????" + pu);
#elif UNITY_ANDROID
	        pu="file://" + Application.persistentDataPath+"/";
#elif UNITY_IPHONE
			pu="file://" +Application.persistentDataPath+"/";
#elif UNITY_WEBPLAYER
			pu=RemoteAssetBundleUrl;
			return pu;
#endif
            return pu;
        }
    }

    public static string CatPictureCacheUrl
    {
        get
        {
            string pu = "";
#if UNITY_STANDALONE_WIN || UNITY_EDITOR         
            pu = "file://" + Application.dataPath + "/CatPicture/";
            //Debug.LogWarning("????" + pu);
#elif UNITY_ANDROID
	        pu="file://" + Application.persistentDataPath+"/CatPicture/";
#elif UNITY_IPHONE
			pu="file://" +Application.persistentDataPath+"/CatPicture/";
#elif UNITY_WEBPLAYER
			return pu;
#endif
            return pu;
        }
    }

    public static string CatPictureCacheRoot
    {
        get
        {
            var fileRoot =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            Application.dataPath + "/CatPicture/";
#elif UNITY_ANDROID
	        Application.persistentDataPath + "/CatPicture/";
#elif UNITY_IPHONE
	        Application.persistentDataPath + "/CatPicture/";
	
#else
	        string.Empty;
#endif
            return fileRoot;
        }
    }

    public static string ScenePictureCacheUrl
    {
        get
        {
            string pu = "";
#if UNITY_STANDALONE_WIN || UNITY_EDITOR         
            pu = "file://" + Application.dataPath + "/ScenePicture/";
            //Debug.LogWarning("????" + pu);
#elif UNITY_ANDROID
	        pu="file://" + Application.persistentDataPath+"/ScenePicture/";
#elif UNITY_IPHONE
			pu="file://" +Application.persistentDataPath+"/ScenePicture/";
#elif UNITY_WEBPLAYER
			return pu;
#endif
            return pu;
        }
    }

    public static string ScenePictureCacheRoot
    {
        get
        {
            var fileRoot =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            Application.dataPath + "/ScenePicture/";
#elif UNITY_ANDROID
	        Application.persistentDataPath + "/ScenePicture/";
#elif UNITY_IPHONE
	        Application.persistentDataPath + "/ScenePicture/";
	
#else
	        string.Empty;
#endif
            return fileRoot;
        }
    }

    /// <summary>
    /// 可写更新目录
    /// </summary>
    public static string AssetBundleCacheRoot
    {
        get
        {
            var fileRoot =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            //Application.persistentDataPath;
            Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
	        Application.persistentDataPath;
#elif UNITY_IPHONE
	        Application.persistentDataPath;
	
#else
	        string.Empty;
#endif
            return fileRoot;
        }
    }

    public static string AssetBundleUrl
    {
        get
        {
            string pu = "";
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            pu = "file://" + Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
			//pu= "jar://file:///" + Application.dataPath + "!/assets/";
            pu=  Application.streamingAssetsPath;
#elif UNITY_IPHONE
			pu="file://" +Application.dataPath + "/Raw/";
#elif UNITY_WEBPLAYER
			pu=RemoteAssetBundleUrl;
#endif
            return pu;
        }
    }

    /// <summary>
    /// 发布目录
    /// </summary>
    public static string AssetBundleRoot
    {
        get
        {

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            return Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
	       return Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
	       return Application.dataPath + "/Raw/";
#else
	      return  string.Empty;
#endif
        }
    }

   
    #endregion

    static ResourceLoader _instance = null;
    public static ResourceLoader Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        transform.localPosition = Vector3.zero;
    }

    public Action<int> OnLoadingScene;


    void Update()
    {
        //Logger.Temp.Write("Update " + Time.realtimeSinceStartup);
        CheckShowLoadingInfo();

        if (thisLoader == null && loadQueue.Count > 0)
        {
            Logger.Temp.Write("TryLoadAssetBundleStart");
            thisLoader = loadQueue.Dequeue();
            TryLoadAssetBundle();
            Logger.Temp.Write("TryLoadAssetBundleOver");
        }

        if (ayncOperation != null)
        {
            if (OnLoadingScene != null)
            {
                OnLoadingScene((int)(ayncOperation.progress * 100));
            }
            Logger.Temp.Write("loadScene " + ayncOperation.progress);
        }
    }
    Queue<AssetBundleLoadSet> loadQueue = new Queue<AssetBundleLoadSet>();
    public void AddLoadSet(AssetBundleLoadSet abs)
    {
        Logger.Temp.Write("addLoadSet " + abs.bundleName + " " + abs.assetName);
        loadQueue.Enqueue(abs);
    }
    public void TryLoadClone(string bundleName, string assetName, Action<GameObject> succes, bool isPrefabActive=true)
    {
        AssetBundleLoadSet abs = new AssetBundleLoadSet();
        abs.bundleName = bundleName.ToLower();
        abs.assetName = assetName;
        abs.OnCloneOver = succes;
        abs.ReturnType = AssetBundleReturnType.Object;
        abs.IsPrefabActive = isPrefabActive;
        AddLoadSet(abs);
    }

    public void TryLoadScene(string bundleName, string assetName, Action<UnityEngine.Object> succes)
    {
        AssetBundleLoadSet abs = new AssetBundleLoadSet();
        abs.bundleName = bundleName.ToLower();
        abs.assetName = assetName;
        abs.OnLoadOver = succes;
        abs.ReturnType = AssetBundleReturnType.Scene;
        AddLoadSet(abs);
    }

    public void TryLoadTextAsset(string assetName, Action<UnityEngine.Object> succes , bool loadAsync = true)
    {
        AssetBundleLoadSet abs = new AssetBundleLoadSet();
        abs.loadType = typeof(TextAsset);
        abs.bundleName = assetName.ToLower();
        abs.assetName = assetName;
        abs.OnLoadOver = succes;
        abs.ReturnType = AssetBundleReturnType.TextAsset;
        AddLoadSet(abs);
    }

    public void TryLoadPic(string bundleName, string assetName, Action<UnityEngine.Object> succes)
    {
        AssetBundleLoadSet abs = new AssetBundleLoadSet();
        abs.loadType = typeof(Sprite);
        abs.bundleName = bundleName;
        abs.assetName = assetName;
        abs.OnLoadOver = succes;
        abs.ReturnType = AssetBundleReturnType.Sprite;
        AddLoadSet(abs);
    }

    public void TryLoadSound(string bundleName, Action<UnityEngine.Object> succes)
    {
        AssetBundleLoadSet abs = new AssetBundleLoadSet();
        abs.loadType = typeof(AudioClip);
        abs.bundleName = bundleName;
        abs.assetName = bundleName;
        abs.OnLoadOver = succes;
        abs.ReturnType = AssetBundleReturnType.Sound;
        AddLoadSet(abs);
    }

    /// <summary>
    /// 直接加载资源
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="assetName"></param>
    /// <param name="over"></param>
    /// <param name="loadType"></param>
    public void TryLoadAssetBundle(string bundleName, string assetName, Action<UnityEngine.Object> over, Type loadType)
    {
        AssetBundleLoadSet abs = new AssetBundleLoadSet();
        abs.bundleName = bundleName.ToLower();
        abs.ReturnType = AssetBundleReturnType.Prefab;
        abs.loadType = loadType;
        abs.assetName = assetName;
        abs.OnLoadOver = over;
        AddLoadSet(abs);
    }
    /// <summary>
    /// ls只在第一次注册成功时才生效
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ls"></param>
    public void RegisterAsset(string name, UnityEngine.Events.UnityAction ls = null)
    {

        string keyName = name.ToLower();
        if (!keyName.EndsWith(ResourceLoader.bundleExtension))
        {
            keyName += ResourceLoader.bundleExtension;
        }

        if (dictAssetBundleRefs.ContainsKey(keyName))
        {
            return;
        }

        var abls = new AssetBundleLoadSet();
        abls.bundleName = keyName;
        abls.ReturnType = AssetBundleReturnType.None;
        abls.ShowInfo = 1;
        abls.OnLoadOver = (UnityEngine.Object o) =>
        {
            if (ls != null)
            {
                ls();
            }
        };

        AddLoadSet(abls);

    }

    AssetBundleLoadSet thisLoader;
    WWW thisWWW;
    AssetBundleRequest thisRequest;
    string thisLoadingname;
    void CheckShowLoadingInfo()
    {
        if (thisLoader != null && thisWWW != null)
        {
            string msg = "download" + thisLoader.bundleName + "  " + (int)(thisWWW.progress * 100) + "%";
#if UNITY_WEBPLAYER
            msg += "\n" + Application.dataPath;
            msg += "\n" + Application.persistentDataPath;
            msg += "\n" + thisWWW.url;
#endif
            float progress = thisWWW.progress;
            if (OnLoading != null)
            {
                OnLoading(msg, progress);
            }
        }
        if (thisRequest != null)
        {
            string msg = "loading " + thisLoadingname + "  " + (int)(thisRequest.progress * 100) + "%";
            float progress = thisRequest.progress;
            if (OnLoading != null)
            {
                OnLoading(msg, progress);
            }
        }
    }

    /// <summary>
    /// 加载进度的行为
    /// </summary>
    public Action<string, float> OnLoading;
    public Action<AssetBundleLoadSet> OnBeginLoadAssetBundle;
    public Action<Queue<AssetBundleLoadSet>> OnEndLoadAssetBundle;
    public Action<string> OnDownLoadError;

    float startDT;
    void TryLoadAssetBundle()
    {
        startDT = Time.time;
        Logger.Temp.Write("TryLoadAssetBundle " + thisLoader.FinalBundleName);
        if (OnBeginLoadAssetBundle != null)
        {
            OnBeginLoadAssetBundle(thisLoader);
        }

        if (dictAssetBundleRefs.ContainsKey(thisLoader.FinalBundleName))
        {
            DownloadSuccess(thisLoader, false);
        }
        else
        {
            string url = Path.Combine(GetFileUrl(thisLoader.FinalBundleName), thisLoader.FinalBundleName);
            Logger.Temp.Write("WWWStart--" + url);
            StopCoroutine("Download");
            StartCoroutine(Download(url));
        }

    }

    public IEnumerator Download(string url)
    {
        try
        {
            Logger.Temp.Write("Download--begin " + url);
            thisWWW = new WWW(url);
        }
        catch (Exception ee)
        {
            Logger.Temp.Write(ee.Message + ee.StackTrace);
        }
        Logger.Temp.Write("Download--end " + url);
        yield return thisWWW;
        Logger.Temp.Write("Download--over " + url);
        if (!string.IsNullOrEmpty(thisWWW.error))
        {
            Logger.Error.Write(thisWWW.error + "URL: " + url);
            yield return new WaitForSeconds(3);
            if (OnDownLoadError != null)
            {
                OnDownLoadError(thisWWW.error);
            }

            if (thisWWW != null)
            {
                thisWWW.Dispose();
                thisWWW = null;
            }

            //AddLoadSet(thisLoader);
            thisLoader = null;
        }
        else
        {
            var ab = thisWWW.assetBundle;
            dictAssetBundleRefs[thisLoader.FinalBundleName] = ab;
            DownloadSuccess(thisLoader);
        }
    }

    public string GetFileUrl(string fname)
    {

#if UNITY_WEBPLAYER
        return AssetBundleCacheUrl;
#else
        var cachePath = Path.Combine(AssetBundleCacheRoot, fname);
        Logger.Temp.Write("CachePath " + cachePath);
        if (System.IO.File.Exists(cachePath))
        {
            return AssetBundleCacheUrl;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        string filePath = System.IO.Path.Combine(AssetBundleUrl, fname);
        Logger.Temp.Write("BundlePath " + filePath);
        var www = new WWW(filePath);
        if (string.IsNullOrEmpty(www.error))
        {
             return  AssetBundleUrl;
        }           
#else
        var bundlePath = System.IO.Path.Combine(AssetBundleRoot, fname);
        Logger.Temp.Write("BundlePath " + bundlePath);
        if (System.IO.File.Exists(bundlePath))
        {
            return AssetBundleUrl;
        }
        Logger.Temp.Write("NoPath " + fname);
#endif

#endif
        return "";

    }

    public void DownloadSuccess(AssetBundleLoadSet abl, bool loadAsync = true)
    {
        string keyName = abl.bundleName;
        if (!keyName.EndsWith(ResourceLoader.bundleExtension))
        {
            keyName += ResourceLoader.bundleExtension;
        }
        try
        {
            var ab = dictAssetBundleRefs[keyName];

            Logger.Temp.Write("WWWEnd-[" + keyName + "] cost " + (Time.time - startDT) + " loadAsync:" + loadAsync);
            if (loadAsync && abl.loadAsync)
            {
                StartCoroutine(LoadAssetAsync(abl, ab));
            }
            else
            {
                LoadAsset(abl, ab);
            }
        }
        catch (Exception ee)
        {
            Logger.Error.Write(ee.Message + ee.StackTrace);
        }
    }

    float startGA;
    public void LoadAsset(AssetBundleLoadSet abl, AssetBundle ab)
    {

        try
        {
            startGA = Time.time;
            var aname = string.IsNullOrEmpty(abl.assetName) ? abl.bundleName : abl.assetName;
            var n = aname.Split('/');
            thisLoadingname = aname;

            if (abl.ReturnType == AssetBundleReturnType.Scene)
            {
                StartCoroutine(LoadScene(abl.assetName, abl));
            }
            else
            {
                Type loadType = abl.loadType;
                if (loadType == null)
                {
                    loadType = typeof(GameObject);
                }
                var asset = ab.LoadAsset(n[n.Length - 1], loadType);

                if (abl.ReturnType == AssetBundleReturnType.None)
                {
                    abl.LoadOver(asset);
                }
                else if (abl.ReturnType == AssetBundleReturnType.Prefab)
                {
                    abl.LoadOver(asset);
                }
                else if (abl.ReturnType == AssetBundleReturnType.Object)
                {
                    LoadObject(ab, abl, asset, aname);
                }
                else if (abl.ReturnType == AssetBundleReturnType.TextAsset)
                {
                    LoadTextAssetOver(asset, abl);
                }
                else if (abl.ReturnType == AssetBundleReturnType.Sprite)
                {
                    LoadPicOver(asset, abl);
                }
                else if (abl.ReturnType == AssetBundleReturnType.Sound)
                {
                    LoadSoundOver(asset, abl);
                }
            }

            if (OnEndLoadAssetBundle != null)
            {
                OnEndLoadAssetBundle(loadQueue);
            }

            thisLoader = null;
            if (thisWWW != null)
            {
                thisWWW.Dispose();
                thisWWW = null;
            }
            thisRequest = null;


            Logger.Temp.Write("MemoryLoad[" + aname + "] cost " + (Time.time - startGA));

        }
        catch (Exception e)
        {
            Logger.Error.Write(e.Message + e.StackTrace);
        }
    }

    void LoadTextAssetOver(UnityEngine.Object textAsset, AssetBundleLoadSet abl)
    {
        abl.LoadOver(textAsset);
    }

    void LoadPicOver(UnityEngine.Object sprite, AssetBundleLoadSet abl)
    {
        abl.LoadOver(sprite);
    }

    void LoadSoundOver(UnityEngine.Object sound, AssetBundleLoadSet abl)
    {
        abl.LoadOver(sound);
    }

    IEnumerator LoadScene(string name, AssetBundleLoadSet abl)
    {
        ayncOperation = Application.LoadLevelAsync(name);
        yield return ayncOperation;
        ayncOperation = null;
        abl.LoadOver(null);
    }

    AsyncOperation ayncOperation;
    void LoadObject(AssetBundle ab, AssetBundleLoadSet abl, UnityEngine.Object asset, string aname)
    {
        var b = asset;
        if (b == null)
        {
            b = ab.mainAsset;
            if (b == null)
            {
                var all = ab.LoadAllAssets();

                foreach (var t in all)
                {
                    //Logger.Warning.Write(t.name + t.GetType());
                }
            }

            //Logger.Warning.Write((aname == null ? "aname==null" : aname) + " not find,use default" + (b == null ? " 返回空" : (b.GetType() + "--" + b.name)));
        }
        if (b != null)
        {
            try
            {
                //Logger.Temp.Write("try clone " + b.name);
                var clone = Instantiate(b) as GameObject;
                clone.SetActive(abl.IsPrefabActive);
                abl.CloneOver(clone);
            }
            catch (Exception eee)
            {
                //Logger.Error.Write(eee.Message + eee.StackTrace);
            }
        }
        else
        {
            //Logger.Error.Write((aname == null ? "aname==null" : aname) + " not find,use default" + (b == null ? " 返回空" : (b.GetType() + "--" + b.name)));
        }
    }

    public IEnumerator LoadAssetAsync(AssetBundleLoadSet abl, AssetBundle ab)
    {
        startGA = Time.time;
        var aname = string.IsNullOrEmpty(abl.assetName) ? abl.bundleName : abl.assetName;
        var n = aname.Split('/');
        thisLoadingname = aname;

        try
        {
            if (abl.ReturnType == AssetBundleReturnType.Scene)
            {
                thisRequest = ab.LoadAllAssetsAsync();
            }
            else
            {
                Type loadType = abl.loadType;
                if (loadType == null)
                {
                    loadType = typeof(GameObject);
                }
                thisRequest = ab.LoadAssetAsync(n[n.Length - 1], loadType);
            }

        }
        catch (Exception e)
        {
            Logger.Error.Write(e.Message + e.StackTrace);
        }

        yield return thisRequest;

        if (abl.ReturnType == AssetBundleReturnType.None)
        {
            abl.LoadOver(thisRequest.asset);
        }
        else if (abl.ReturnType == AssetBundleReturnType.Prefab)
        {
            abl.LoadOver(thisRequest.asset);
        }
        else if (abl.ReturnType == AssetBundleReturnType.Object)
        {
            var asset = thisRequest.asset;
            LoadObject(ab, abl, asset, aname);
        }
        else if (abl.ReturnType == AssetBundleReturnType.Scene)
        {

            ayncOperation = Application.LoadLevelAsync(abl.assetName);

            yield return ayncOperation;
            ayncOperation = null;
            abl.LoadOver(null);

        }else if (abl.ReturnType == AssetBundleReturnType.TextAsset)
        {
            LoadTextAssetOver(thisRequest.asset, abl);
        }
        else if (abl.ReturnType == AssetBundleReturnType.Sprite)
        {
            LoadPicOver(thisRequest.asset, abl);
        }
        else if (abl.ReturnType == AssetBundleReturnType.Sound)
        {
            LoadSoundOver(thisRequest.asset, abl);
        }


        if (OnEndLoadAssetBundle != null)
        {
            OnEndLoadAssetBundle(loadQueue);
        }

        thisLoader = null;
        if (thisWWW != null)
        {
            thisWWW.Dispose();
            thisWWW = null;
        }
        thisRequest = null;

        if (Time.time - startGA > 0)
        {
            Logger.Temp.Write("MemoryLoadAsync[" + aname + "] cost " + (Time.time - startGA));
        }

    }


    #region
    static private Dictionary<string, AssetBundle> dictAssetBundleRefs = new Dictionary<string, AssetBundle>();


    // Get an AssetBundle
    public static AssetBundle GetAssetBundle(string name)
    {
        if (!name.EndsWith(ResourceLoader.bundleExtension))
        {
            name += ResourceLoader.bundleExtension;
        }
        AssetBundle abRef;
        lock (dictAssetBundleRefs)
        {
            if (dictAssetBundleRefs.TryGetValue(name, out abRef))
            {
                if (abRef != null)
                {
                    return abRef;
                }
                return null;
            }
            else
            {
                Logger.Error.Write("找不到AssetBundle: " + name);
                foreach (String n in dictAssetBundleRefs.Keys)
                    Debug.LogError(n);
                return null;
            }
        }
    }

    // Unload an AssetBundle
    public static void Unload(string keyName, bool allObjects)
    {
        AssetBundle abRef;
        if (dictAssetBundleRefs.TryGetValue(keyName, out abRef))
        {
            abRef.Unload(allObjects);
            dictAssetBundleRefs.Remove(keyName);
        }
    }

    #endregion
}

public class AssetBundleLoadSet
{
    public GameObject root;
    public string bundleName = "";
    public string FinalBundleName
    {
        get
        {
            if (!bundleName.EndsWith(ResourceLoader.bundleExtension))
            {
                return bundleName + ResourceLoader.bundleExtension;
            }
            return bundleName;
        }
    }
    public string assetName = "";

    public Vector3 position;
    public Vector3 rotate;
    public Vector3 scale;
    public int ShowInfo = 0;
    public AssetBundleReturnType ReturnType = AssetBundleReturnType.None;

    public Type loadType;

    public Action<GameObject> OnCloneOver;
    public Action<UnityEngine.Object> OnLoadOver;

    public bool loadAsync = false;

    public bool IsPrefabActive = true;

    public void CloneOver(GameObject asset)
    {
        if (!asset)
        {
            return;
        }
        if (root)
        {
            asset.transform.parent = root.transform;
        }
        asset.transform.localPosition = Vector3.zero;
        if (position != Vector3.zero)
        {
            asset.transform.localPosition = position;
        }
        asset.transform.localScale = new Vector3(1, 1, 1);
        if (scale != Vector3.zero)
        {
            asset.transform.localScale = scale;
        }

        asset.transform.localRotation = new Quaternion();
        if (rotate != Vector3.zero)
        {
            asset.transform.localRotation = Quaternion.Euler(rotate);
        }
        if (OnCloneOver != null)
        {
            try
            {
                OnCloneOver(asset);
            }
            catch (Exception e)
            {
                Logger.Error.Write(e.Message + e.StackTrace);
            }
        }
    }

    public void LoadOver(UnityEngine.Object o)
    {
        if (OnLoadOver != null)
        {
            try
            {
                OnLoadOver(o);
            }
            catch (Exception e)
            {
                Logger.Error.Write(e.Message + e.StackTrace);
            }
        }
    }
}

public enum AssetBundleReturnType
{
    None = 0,
    Object = 1,
    Prefab = 2,
    Scene = 3,
    TextAsset = 4,
    Sprite = 5,
    Sound = 6,
}

