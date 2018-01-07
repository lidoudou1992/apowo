using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Assets.Scripts;
using LitJson;

public class CheckUpdate : MonoBehaviour
{
    public Action<FileSpec> OnBeginCheckFile;
    public Action OnUpdateOver;

    public long TotalSize;
    private long completeSize;

    int hotFixMajorVersion = -1;

    int firstHotFixVersion = -1;
    int maxHotFixMinorVersion = -1;

    int totalParseFilesSteps = 0;
    int currentParseFilesStep = 0;

    public bool IsNeedHotFix;

    private Dictionary<string, int> downloadedDic = new Dictionary<string, int>();

    private HotFixPanel hotFixPanel;

    void Start()
    {
#if UNITY_WEBPLAYER &&!UNITY_EDITOR
        Logger.Temp.WriteLog("CheckUpdateInWeb");
        TryLoadAssets();
#else
        if (AppConfig.USE_HOT_FIX)
        {
            GameObject hotFixRoot = GameObject.Find("HotFixRoot").gameObject;
            hotFixPanel = hotFixRoot.transform.Find("HotFixPanel").GetComponent<HotFixPanel>();
            hotFixPanel.gameObject.SetActive(true);

            //StartCoroutine(downloadVersionListFile(System.IO.Path.Combine(ResourceLoader.RemoteAssetBundleUrl, "TestVersionList.json")));
            StartCoroutine(downloadVersionListFile(System.IO.Path.Combine(ResourceLoader.RemoteAssetBundleUrl, "VersionList.json")));
        }
        else
        {
            TryLoadAssets();
        }
#endif
    }

    public IEnumerator downloadVersionListFile(string url)
    {
        using (WWW www = new WWW(url))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                GameApplication.Instance.AskQuitGame("网络连接出错[" + www.error + "],点击退出游戏!");
            }
            else
            {
                JsonData versionListFile = JsonMapper.ToObject(www.text);
                int flag = checkVersionListFile(versionListFile);
                if (flag == 1)
                {
                    downloadedDic.Clear();

                    if (maxHotFixMinorVersion == firstHotFixVersion)
                        //if (maxHotFixMinorVersion >= firstHotFixVersion)
                    {
                        totalParseFilesSteps = 1;
                        StartCoroutine(downloadFilesJsonFile(System.IO.Path.Combine(ResourceLoader.RemoteAssetBundleUrl, string.Format("{0}.{1}/files.json", hotFixMajorVersion, firstHotFixVersion))));
                    }
                    else if(maxHotFixMinorVersion > firstHotFixVersion)
                    {
                        int minorVersion = 0;
                        for (int i = 0; i < totalParseFilesSteps; i++)
                        {
                            minorVersion = firstHotFixVersion + i;
                            StartCoroutine(downloadFilesJsonFile(System.IO.Path.Combine(ResourceLoader.RemoteAssetBundleUrl, string.Format("{0}.{1}/files.json", hotFixMajorVersion, minorVersion))));
                        }
                    }
                }
                else if(flag == 0)
                {
                    Debug.Log("热更版本一致，跳过更新");
                    TryLoadAssets();
                }else if (flag == 2)
                {
                    Debug.Log("大版本不一致，请重新下载安装包");
                    hotFixPanel.SetTwoBtnMode("喵！有新的安装包可以更新了！", "安装包大版本不一致，请重新下载安装包", "下载", () =>
                    {
                        Application.Quit();

                        if (Application.platform == RuntimePlatform.Android)
                        {
                            Application.OpenURL("http://box37.yxdown.com/2016/7/11/636038462561189702/1001582/setup.apk");
                        }
                        else if (Application.platform == RuntimePlatform.IPhonePlayer)
                        {
                            Application.OpenURL("https://itunes.apple.com/cn/app/mao-zhai-ri-ji/id1124854554");
                        }
                        else
                        {
                            Application.OpenURL("http://box37.yxdown.com/2016/7/11/636038462561189702/1001582/setup.apk");
                        }
                    }, "退出", () =>
                    {
                        Application.Quit();
                    });

                    //hotFixPanel.SetOneBtnMode("喵！有新的安装包可以更新了！", "安装包大版本不一致，请重新下载安装包","确定",
                    //() =>
                    //{
                    //    maxHotFixMinorVersion = AppConfig.MINOR_VERSION;
                    //    hotFixPanel.CloseAlert();
                    //    files.Clear();
                    //    //TryLoadAssets();

                    //    Application.Quit();

                    //    if (Application.platform == RuntimePlatform.Android)
                    //    {
                    //        Application.OpenURL("http://box37.yxdown.com/2016/7/11/636038462561189702/1001582/setup.apk");
                    //    }
                    //    else if(Application.platform == RuntimePlatform.IPhonePlayer)
                    //    {
                    //        Application.OpenURL("https://itunes.apple.com/cn/app/mao-zhai-ri-ji/id1124854554");
                    //    }
                    //    else
                    //    {
                    //        Application.OpenURL("http://box37.yxdown.com/2016/7/11/636038462561189702/1001582/setup.apk");
                    //    }

                    //});
                }
            }
        }
    }

    private int checkVersionListFile(JsonData versionListFile)
    {
        AppConfig.MAINTAIN_TYPE maintype = AppConfig.MAINTAIN_TYPE.Null;
        if ((versionListFile as IDictionary).Contains("Maintain_Type"))
        {
            maintype = (AppConfig.MAINTAIN_TYPE)int.Parse(versionListFile["Maintain_Type"].ToString());
        }

        switch (maintype)
        {
            case AppConfig.MAINTAIN_TYPE.Null:
                break;
            //case AppConfig.MAINTAIN_TYPE.TestHotFix:
            //    //内部测试热更新内容，但玩家拉不到(可以拉到本次热更之前的所有版本)，但是可以正常进游戏玩
            //    if (AppConfig.USE_DEVELOP_MODE==false)
            //    {
            //        Debug.Log("内部测试热更新内容，但玩家拉不到，但是可以正常进游戏玩");
            //        return 0;
            //    }
            //    break;
            case AppConfig.MAINTAIN_TYPE.CloseServer:
                //停服更新，所有人不能进游戏，即使服务器开了
                if (AppConfig.USE_DEVELOP_MODE == false)
                {
                    Debug.Log("停服更新，所有人不能进游戏，即使服务器开了");

                    string title = "";
                    string tipStr = "";

                    if ((versionListFile as IDictionary).Contains("TipTitle"))
                    {
                        title = versionListFile["TipTitle"].ToString();
                    }

                    if ((versionListFile as IDictionary).Contains("TipString"))
                    {
                        tipStr = versionListFile["TipString"].ToString();
                    }

                    hotFixPanel.SetOneBtnMode(title, tipStr, "确定",
                    () =>
                    {
                        Application.Quit();
                    });

                    return -1;
                }
                break;
            default:
                break;
        }



        int majorVersion = 0;
        int minorVersion = 0;

        var versions = versionListFile["Versions"];

        if (versions.Count <= 0)
        {
            
            Debug.Log("没有热更版本");
            return 0;
        }

        int blockVersion = -1;
        maxHotFixMinorVersion = (int)versions[versions.Count - 1]["MinorVersion"];
        if (AppConfig.USE_DEVELOP_MODE == false)
        {
            if ((versionListFile as IDictionary).Contains("BlockVersion"))
            {
                blockVersion = int.Parse(versionListFile["BlockVersion"].ToString());
                if (blockVersion > 0)
                {
                    //内部测试热更新内容，但玩家拉不到(可以拉到本次热更之前的所有版本)，但是可以正常进游戏玩
                    maxHotFixMinorVersion = blockVersion - 1;
                    maxHotFixMinorVersion = Mathf.Max(0, maxHotFixMinorVersion);
                    Debug.Log("内部测试热更新内容，但玩家拉不到，但是可以正常进游戏玩");
                }
            }
        }

        JsonData temp;
        for (int i = 0; i < versions.Count; i++)
        {
            temp = versions[i];
            majorVersion = (int)temp["MajorVersion"];
            minorVersion = (int)temp["MinorVersion"];
            if (AppConfig.MAJOR_VERSION == majorVersion)
            {
                if (minorVersion > AppConfig.MINOR_VERSION)
                {
                    if (minorVersion == blockVersion)
                    {
                        return 0;
                    }
                    else
                    {
                        hotFixMajorVersion = majorVersion;

                        firstHotFixVersion = minorVersion;
                        totalParseFilesSteps = maxHotFixMinorVersion - firstHotFixVersion + 1;
                        return 1;
                    }
                }
            }
            else if (AppConfig.MAJOR_VERSION < majorVersion)
            {
                return 2;
            }
            else if (AppConfig.MAJOR_VERSION > majorVersion)
            {
                Debug.Log(string.Format("安装包大版本{0}对应配置了热更大版本{1}中的内容，请检查！", AppConfig.MAJOR_VERSION, majorVersion));
                return 0;
            }
        }
        
        return 0;
    }

    public IEnumerator downloadFilesJsonFile(string url)
    {
        using (WWW www = new WWW(url))
        {
            JsonData fileListText;
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                GameApplication.Instance.AskQuitGame("网络连接出错[" + www.error + "],点击退出游戏!");
            }
            else
            {
				fileListText = JsonMapper.ToObject(www.text);
                StartCoroutine(CheckFiles(fileListText));
            }
        }
    }

	
	public IEnumerator CheckFiles(JsonData fileListText)
	{
		var file = fileListText["Files"];

        int count = file.Count;
        JsonData temp;
        for (int i = 0; i < count; i++)
        {
            temp = file[i];

            var fs = new FileSpec(temp as JsonData);
            fs.MajorVersion = (int)fileListText["MajorVersion"];
            fs.MinorVersion = (int)fileListText["MinorVersion"];
            //if (NeedUpdateFile(fs))
            //{
            //    //files.Enqueue(fs);
            //    files.Add(fs);
            //}

            files.Add(fs);
            yield return new WaitForSeconds(0.001f);
        }
		
        currentParseFilesStep += 1;

        if (currentParseFilesStep == totalParseFilesSteps)
        {
            files = filterSameFile();
            totalCount = files.Count;

            if (totalCount == 0)
            {
                TryLoadAssets();
            }
            else
            {
                IsNeedHotFix = true;
                StartHotFix();
            }
        }
    }

    public void StartHotFix()
    {
        hotFixPanel.SetActive();
        files.ForEach(p => TotalSize += p.Length);

        //hotFixPanel.SetTwoBtnMode("喵！有新的安装包可以更新了！", "安装包大版本不一致，请重新下载安装包", "下载", () =>
        //{
        //    Application.Quit();

        //    if (Application.platform == RuntimePlatform.Android)
        //    {
        //        Application.OpenURL("http://box37.yxdown.com/2016/7/11/636038462561189702/1001582/setup.apk");
        //    }
        //    else if (Application.platform == RuntimePlatform.IPhonePlayer)
        //    {
        //        Application.OpenURL("https://itunes.apple.com/cn/app/mao-zhai-ri-ji/id1124854554");
        //    }
        //    else
        //    {
        //        Application.OpenURL("http://box37.yxdown.com/2016/7/11/636038462561189702/1001582/setup.apk");
        //    }
        //}, "退出", () =>
        //{
        //    Application.Quit();
        //});

        hotFixPanel.SetAlert(string.Format("有{0}MB文件需要更新", ((float)TotalSize / (1024 * 1024)).ToString("f2")),
            () =>
            {
                hotFixPanel.SetProgressInfoActive();
                hotFixPanel.CloseAlert();
                TryLoadAssets();
            },
            () =>
            {
                maxHotFixMinorVersion = AppConfig.MINOR_VERSION;
                hotFixPanel.CloseAlert();
                files.Clear();
                TryLoadAssets();
            }
            );
    }

    //public bool NeedUpdateFile(FileSpec f)
    //{
    //    var cachePath = System.IO.Path.Combine(ResourceLoader.AssetBundleCacheRoot, f.Name);
    //    if (System.IO.File.Exists(cachePath))
    //    {
    //        using (System.IO.Stream bs = new System.IO.StreamReader(cachePath).BaseStream)
    //        {
    //            var md5 = FlyModel.CryptoHelp.MD5(bs);

    //            if (md5 == f.MD5)
    //            {
    //                return false;
    //            }
    //        }
    //        //System.IO.File.Delete(cachePath);//不需要删除，因为到时会被下载的新文件覆盖
    //    }

    //    var bundlePath = System.IO.Path.Combine(ResourceLoader.AssetBundleRoot, f.Name);
    //    if (System.IO.File.Exists(bundlePath))
    //    {
    //        using (System.IO.Stream bs = new System.IO.StreamReader(bundlePath).BaseStream)
    //        {
              
    //            var md5 = FlyModel.CryptoHelp.MD5(bs);

    //            if (md5 == f.MD5)
    //            {
    //                return false;
    //            }
    //        }
    //    }

    //    return true;
    //}

    private List<FileSpec> filterSameFile()
    {
        files.Sort((FileSpec a, FileSpec b) => {
            return (a.MinorVersion > b.MinorVersion ? -1 : 1);
        });

        Dictionary<string, FileSpec> dic = new Dictionary<string, FileSpec>();

        string name;
        FileSpec temp;
        for (int i = 0; i < files.Count; i++)
        {
            temp = files[i];
            name = temp.Name;
            if (dic.ContainsKey(name)==false)
            {
                dic.Add(name, temp);
            }
        }

        List<FileSpec> list = new List<FileSpec>();
        foreach (var item in dic)
        {
            list.Add(item.Value);
        }

        return list;
    }

    public void TryLoadAssets()
    {
        if (files.Count > 0)
        {
            //thisFile = files.Dequeue();
            thisFile = files[0];
            files.RemoveAt(0);
            StartCoroutine(CacheAsset(thisFile));
        }
        else
        {
            Logger.Temp.Write("OnUpdateOver==null? " + (OnUpdateOver == null));

            GameApplication.Instance.RecordVersion(hotFixMajorVersion, maxHotFixMinorVersion);
            //GameApplication.Instance.RecordVersion(hotFixMajorVersion, firstHotFixVersion);

            if (OnUpdateOver != null)
            {
                try
                {
                    OnUpdateOver();
                }
                catch (Exception e)
                {
                    Logger.Temp.Write(e.Message+e.StackTrace);
                }
            }
        }
    }

    public List<FileSpec> files = new List<FileSpec>();
    int totalCount = 1;
    public bool started;
    WWW thisWWW;
    FileSpec thisFile;

    void Update()
    {
        if (thisWWW != null)
        {
            string info = string.Format("文件下载进度 {0}/{1} {2}%", (totalCount - files.Count), totalCount, (thisWWW.progress * 100).ToString("0.00"));
            Debug.Log(info);

            if (downloadedDic.ContainsKey(thisWWW.url))
            {
                downloadedDic[thisWWW.url] = thisWWW.bytesDownloaded;
            }
            else
            {
                downloadedDic.Add(thisWWW.url, thisWWW.bytesDownloaded);
            }
            completeSize = getDownloadedBytes();

            float progressVal = ((float)completeSize / TotalSize);
            info = string.Format("已完成{0}%", (progressVal*100).ToString("f1"));
            Debug.Log(info);

            hotFixPanel.UpdateInfo(info, progressVal);
        }
    }

    private int getDownloadedBytes()
    {
        int size = 0;
        foreach (var download in downloadedDic)
        {
            size += download.Value;
        }

        return size;
    }

    // 下载单AssetBundle
    public IEnumerator CacheAsset(FileSpec file)
    {
        Debug.Log(string.Format("{0}.{1}/{2}", hotFixMajorVersion, file.MinorVersion, file.Name));
        string path = System.IO.Path.Combine(ResourceLoader.RemoteAssetBundleUrl, string.Format("{0}.{1}/{2}",hotFixMajorVersion, file.MinorVersion, file.Name));
        thisWWW = new WWW(path);
        {
            yield return thisWWW;
            string cachedAssetBundle = System.IO.Path.Combine(ResourceLoader.AssetBundleCacheRoot, file.Name);
            if (!string.IsNullOrEmpty(thisWWW.error))
            {
                GameApplication.Instance.AskQuitGame("网络连接出错[" + thisWWW.error + "],点击退出游戏!");
            }
            else
            {
                var dir = System.IO.Path.GetDirectoryName(cachedAssetBundle);
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }

                if (System.IO.File.Exists(cachedAssetBundle))
                {
                    //System.IO.File.Delete(cachedAssetBundle);//不需要删除，因为到时会被下载的新文件覆盖
                }
                System.IO.FileStream cache = new System.IO.FileStream(cachedAssetBundle, System.IO.FileMode.Create);
                cache.Write(thisWWW.bytes, 0, thisWWW.bytes.Length);
                cache.Close();
#if UNITY_IPHONE
                UnityEngine.iOS.Device.SetNoBackupFlag(cachedAssetBundle);
#endif
                thisWWW.Dispose();
                thisWWW = null;
                TryLoadAssets();
            }
        }
        //		Debug.Log(mds+"Cache saved: " + cachedAssetBundle);
    }
    	
 
}

public class FileSpec
{
    public FileSpec(JsonData info)
    {
        Name = info["Name"].ToString();
        MD5 = info["MD5"].ToString();
        Length = (int)info["Size"];
    }
    public string Name
    {
        get;
        set;
    }

    public string MD5
    {
        get;
        set;
    }

    public int Length
    {
        get;
        set;
    }

    public int MajorVersion
    {
        get;
        set;
    }

    public int MinorVersion
    {
        get;
        set;
    }
}
