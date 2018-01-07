using UnityEngine;
using System.Collections;
using System;
using CLRSharp;
using FlyModel;
using Assets.Scripts;
using System.Collections.Generic;
using cn.sharesdk.unity3d;

public class GameApplication : MonoBehaviour {

    public SDKTools SDKTools;

    public IHotFixMain HotFix { get; set; }
    public static GameApplication Instance { get; set; }
    public SocketClient SocketClient { get; set; }

    public Action<string> OnAskQuitGame;

    public ShareSDK SSDK;

    public static Dictionary<string, int> CHANNEL_MAP = new Dictionary<string, int>();

    public void AskQuitGame(string msg)
    {
        if (OnAskQuitGame != null)
        {
            OnAskQuitGame(msg);
        }
        else
        {
            Debug.LogError(msg);
        }
    }

    private string lastIP;
    private int lastPort;
    public int ConnectTimes = 0;

    public void TryConnect(string ip, int port)
    {
        Debug.Log("tryConnect");
        if (SocketClient != null)
        {
            if (SocketClient.IsConnecting)
            {
                return;
            }
            SocketClient.Close();
            SocketClient = null;
        }

        lastIP = ip;
        lastPort = port;
        SocketClient = new SocketClient(ip, port);

        if (!SocketClient.IsConnected)
        {
            try
            {
#if UNITY_EDITOR
                Debug.Log(UnityEditor.EditorUserBuildSettings.activeBuildTarget);
                if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebPlayer
                    || UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebPlayerStreamed)
                {
                    Security.PrefetchSocketPolicy(ip, port);
                }

#else
                if (Application.platform == RuntimePlatform.WindowsWebPlayer)
                {
                    Security.PrefetchSocketPolicy(ip, port);
                }
#endif
                SocketClient.ConnectedCallback = ControllerClient_ConnectedCallback;
                SocketClient.DisconnectedCallback = ControllerClient_DisconnectedCallback;
                SocketClient.ReconnectingCallback = ReconnectingCallback;
                SocketClient.TryConnect();
            }
            catch (Exception e)
            {
                Logger.Error.Write(e.Message + e.StackTrace);
            }
        }
    }

    public void ReconnectingCallback()
    {
        Debug.Log("重连");
        TryConnect(lastIP, lastPort);
    }

    public Action OnConnected;
    void ControllerClient_ConnectedCallback()
    {
        if (OnConnected != null)
        {
            if (HotFixScript.env != null && ThreadContext.activeContext == null)
            {
                var c = new ThreadContext(HotFixScript.env);
                //print(c.ToString());
            }
            OnConnected();
            ConnectTimes++;
        }
    }

    public Action OnDisconnected;
    void ControllerClient_DisconnectedCallback()
    {
        //SocketClient.ConnectedCallback = null;
        //SocketClient.DisconnectedCallback = null;

        if (OnDisconnected != null)
        {
            if (ThreadContext.activeContext == null)
            {
                var c = new ThreadContext(HotFixScript.env);
                print(c.ToString());
            }
            OnDisconnected();
        }
    }

    [HideInInspector]
    public ResourceLoader ResourceLoader;

    public GUIStyle GUIStyle;

    public static AudioSource SoundEffectController;
    public static AudioSource MusicController;

    void Awake()
    {
        SSDK = gameObject.GetComponent<ShareSDK>();

        DontDestroyOnLoad(gameObject);
        GUIStyle = new GUIStyle();
        int fh = Screen.height * 2 / 100;

        GUIStyle.alignment = TextAnchor.UpperLeft;
        GUIStyle.fontSize = fh;
        GUIStyle.normal.textColor = Color.yellow;

        Instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.runInBackground = true;

        SoundEffectController = GameObject.Find("MainRoot").GetComponent<AudioSource>();
        MusicController = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("MAJOR_VERSION") && PlayerPrefs.GetInt("MAJOR_VERSION") == AppConfig.MAJOR_VERSION)
        {
            AppConfig.MAJOR_VERSION = PlayerPrefs.GetInt("MAJOR_VERSION");

            if (PlayerPrefs.HasKey("MINOR_VERSION"))
            {
                AppConfig.MINOR_VERSION = PlayerPrefs.GetInt("MINOR_VERSION");
            }
        }

        Debug.Log(string.Format("当前版本号：{0}{1}.{2}", AppConfig.VERSION_PREFIX, AppConfig.MAJOR_VERSION, AppConfig.MINOR_VERSION));

        LogManager.Initialize();
       
        Application.logMessageReceived += LogCallback;

        ResourceLoader = gameObject.AddComponent<ResourceLoader>();

        CHANNEL_MAP.Add("cat", 0);
        CHANNEL_MAP.Add("4399", 1);
        CHANNEL_MAP.Add("7k7k", 2);
        CHANNEL_MAP.Add("meitu", 3);
        CHANNEL_MAP.Add("taptap", 4);
        CHANNEL_MAP.Add("cathouse", 5);
        CHANNEL_MAP.Add("7k7kcat", 6);
        CHANNEL_MAP.Add("77", 7);
        CHANNEL_MAP.Add("kk", 8);
        CHANNEL_MAP.Add("iOS", 9);

        //Debug.Log("==================Application.persistentDataPath");
        //Debug.Log(Application.persistentDataPath);
        //Debug.Log("==================Application.dataPath");
        //Debug.Log(Application.dataPath);
        //Debug.Log("==================Application.streamingAssetsPath;");
        //Debug.Log(Application.streamingAssetsPath);
    }

    public void LogCallback(string condition, string stackTrace, LogType type)
    {
#if UNITY_WEBPLAYER
        Application.ExternalCall("ShowLog", condition + stackTrace);
#endif
    }

    void Start()
    {
        SDKTools = new SDKTools();

        // 接入talkingdata
        // talkingData文档在sdk目录下
        // AndroidManifest.xml重要加入以下权限（其他的权限，默认都加上了）
        // <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
        TalkingDataGA.OnStart("61E4CD17323896115E3CA83630DC2076", AppConfig.CHANNEL);

        GameObject.Find("StartMask").GetComponent<LogoAniController>().SetOnTimeCallback(()=> {
            var co = new GameObject("CheckUpdate");
            var checkUpdate = co.AddComponent<CheckUpdate>();
            checkUpdate.OnUpdateOver = () =>
            {
                InitializeHotfix();
            };
        });
    }

    private void showLogoAni()
    {

    }

    void InitializeHotfix()
    {
#if !UNITY_EDITOR
            Debug.Log("OnUpdateOver Run");
        
            ResourceLoader.Instance.RegisterAsset("flymodel.pdb");
            ResourceLoader.Instance.RegisterAsset("flymodel.dll", () =>
            {
                Debug.Log("try add HotFixReflector");
                try
                {
                    //GameApplication.Instance.gameObject.AddComponent<HotFixReflector>();
                    GameApplication.Instance.gameObject.AddComponent<HotFixScript>();
                    Debug.Log("add HotFixScript");
                }
                catch (Exception ee)
                {
                     Logger.Error.Write(ee.Message + ee.StackTrace);
                }
            });
#else
        //Debug.Log("OnUpdateOver Editor");
        //      ResourceLoader.Instance.RegisterAsset("flymodel.dll");

        //      ResourceLoader.Instance.RegisterAsset("flymodel.pdb", () =>
        //      {
        //		Debug.Log("try add HotFixReflector");
        //          try
        //          {
        //              //gameObject.AddComponent<HotFixReflector>();
        //              gameObject.AddComponent<HotFixScript>();
        //              Debug.Log("add HotFixReflector");
        //          }
        //          catch (Exception ee)
        //          {
        //              Logger.Error.Write(ee.Message + ee.StackTrace);
        //          }
        //      });

        ResourceLoader.Instance.RegisterAsset("flymodel.pdb");
        ResourceLoader.Instance.RegisterAsset("flymodel.dll", () =>
        {
            Debug.Log("try add HotFixReflector");
            try
            {
                GameApplication.Instance.gameObject.AddComponent<HotFixReflector>();
                //GameApplication.Instance.gameObject.AddComponent<HotFixScript>();
                Debug.Log("add HotFixScript");
            }
            catch (Exception ee)
            {
                Logger.Error.Write(ee.Message + ee.StackTrace);
            }
        });
#endif
    }

    public Action OnUpdate;
    public Action OnLateUpdate;
    public Action DrawGizmos;
    public Action DrawGUI;

    void Update()
    {
        CheckCommand();
       
		if(SocketClient!=null)
		{
			SocketClient.CheckSend();
		}

        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

    public void CheckCommand()
    {
        if (SocketClient != null && SocketClient.IsConnected)
        {
            StartCoroutine(CheckBinaryCommand());
        }

    }

    public IEnumerator CheckBinaryCommand()
    {
        while (SocketClient.ByteCommands.Count > 0)
        {
            var re = SocketClient.ByteCommands.Dequeue();
            HotFix.InvokeScriptMethod(re);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ManulDisconnected()
    {
        if (SocketClient != null)
        {
            SocketClient.Close();
        }
    }

    void OnDestroy()
    {
        Debug.Log("GameApplication OnDestroy");
        if (SocketClient != null)
        {
            SocketClient.Close();
        }
        TalkingDataGA.OnEnd();
    }

    void OnGUI()
    {
        if (DrawGUI != null)
        {
            DrawGUI();
        }
    }

    void LateUpdate()
    {
        if (OnLateUpdate != null)
        {
            OnLateUpdate();
        }
    }

    void OnDrawGizmos()
    {
        if (DrawGizmos != null)
        {
            DrawGizmos();
        }
    }

    public void RecordVersion(int majorVersion, int minorVersion)
    {
        if (majorVersion < 0 || minorVersion < 0)
        {
            majorVersion = AppConfig.MAJOR_VERSION;
            minorVersion = AppConfig.MINOR_VERSION;
        }

        AppConfig.MAJOR_VERSION = majorVersion;
        AppConfig.MINOR_VERSION = minorVersion;

        PlayerPrefs.SetInt("MAJOR_VERSION", majorVersion);
        PlayerPrefs.SetInt("MINOR_VERSION", minorVersion);
        PlayerPrefs.Save();

        Debug.Log(string.Format("当前版本号：{0}{1}.{2}", AppConfig.VERSION_PREFIX, AppConfig.MAJOR_VERSION, AppConfig.MINOR_VERSION));
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
        {
            //Debug.Log("====================OnApplicationFocus true");
        }
        else
        {
            //Debug.Log("====================OnApplicationFocus false");
        }
    }
}
