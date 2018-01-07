using System;
using UnityEngine;
using UnityEngine.UI;
using FlyModel.Proto;
using System.IO;
using FlyModel.Control;
using FlyModel.Model;
using FlyModel.UI.Component;
using FlyModel.UI;
using FlyModel.UI.Panel.LoginPanel;
using Assets.Scripts;

namespace FlyModel
{
    public static class GameMain
    {
        public static Canvas UI2DRoot;

        public static int terrainLayer;

        public static GameObject SceneRoot;

        public static TimeTick TimeTick;

        public static string UDID;

        // UDID是直接登陆 LoginPanel是账号登陆界面
        //public static EnumConfig.LoginType LoginType = EnumConfig.LoginType.UDID;
        public static EnumConfig.LoginType LoginType = EnumConfig.LoginType.LoginPanel;  // 进入游戏后会在InfoBar的UpdateInfoLayout方法报错

        public static BackKeyController backKeyController;

        //public static TDGAAccount TDGAAccount;

        public static bool GuideDisabled = false;
        public static void DisableGuide()
        {
            GuideDisabled = true;
        }

        public static void Start()
        {
            try
            {
                Logger.Temp.Write("GameMainStart");

                GSDKUnityLib.GSDK.Instance.Initialize();

                GameApplication.Instance.OnUpdate =BehaviourManager.Update;
                //GameApplication.Instance.OnLateUpdate = BehaviourManager.LateUpdate;
                //GameApplication.Instance.DrawGizmos =BehaviourManager.DrawGizmos;
                //GameApplication.Instance.DrawGUI =BehaviourManager.DrawGUI;

                //画布
                var u2go = new GameObject("UI2DRoot");
                UI2DRoot = u2go.AddComponent<Canvas>();
                u2go.AddComponent<DontDestroyOnLoad>();
                u2go.layer = LayerMask.NameToLayer("UI");

                if (Application.platform == RuntimePlatform.Android)
                {
                    backKeyController = new BackKeyController(u2go);
                    BehaviourManager.AddGameComponent(backKeyController);
                }

                var gs = u2go.AddComponent<GraphicRaycaster>();
                gs.ignoreReversedGraphics = true;

                var cs = u2go.AddComponent<CanvasScaler>();
                cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                cs.referenceResolution = new Vector2(640, 1136);
                cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                cs.matchWidthOrHeight = 1;

                //场景层面板
                SceneRoot = new GameObject("SceneRoot");
                SceneRoot.layer = LayerMask.NameToLayer("UI");
                var srt = SceneRoot.AddComponent<RectTransform>();
                srt.sizeDelta = new Vector2(0, 0);
                srt.anchorMin = new Vector2(0, 0);
                srt.anchorMax = new Vector2(1, 1);
                srt.pivot = new Vector2(0.5f, 0.5f);
                SceneRoot.transform.SetParent(u2go.transform, false);

                //UI层面板根节点
                var panelRoot = new GameObject("PanelRoot");
                panelRoot.layer = LayerMask.NameToLayer("UI");
                var pr = panelRoot.AddComponent<RectTransform>();
                pr.SetParent(UI2DRoot.transform, false);
                pr.sizeDelta = new Vector2(0, 0);
                pr.anchorMin = Vector2.zero;
                pr.anchorMax = new Vector2(1, 1);
                pr.pivot = new Vector2(0.5f, 0.5f);
                PanelManager.PanelRectTransform = pr;

                ////--------------------------------------------------------------
                //// UI层面板根节点
                //var panelRoot = new GameObject("PanelRoot");
                //panelRoot.layer = LayerMask.NameToLayer("UI");
                //var pr = panelRoot.AddComponent<>
                ////--------------------------------------------------------------

                //toolbar根节点
                var toolbarRoot = new GameObject("ToolbarRoot");
                toolbarRoot.layer = LayerMask.NameToLayer("UI");
                var toolbarRT = toolbarRoot.AddComponent<RectTransform>();
                toolbarRT.sizeDelta = new Vector2(0, 0);
                toolbarRT.anchorMin = Vector2.zero;
                toolbarRT.anchorMax = new Vector2(1, 1);
                toolbarRT.pivot = new Vector2(0.5f, 0.5f);
                PanelManager.ToolbarRectTransform = toolbarRT;
                toolbarRoot.transform.SetParent(u2go.transform, false);

                // 创建Tip根节点：TipRoot
                var tipRoot = new GameObject("TipRoot");
                tipRoot.layer = LayerMask.NameToLayer("UI");
                var tipRootRT = tipRoot.AddComponent<RectTransform>();
                tipRootRT.sizeDelta = new Vector2(0, 0);
                tipRootRT.anchorMin = Vector2.zero;
                tipRootRT.anchorMax = new Vector2(1, 1);
                tipRootRT.pivot = new Vector2(0.5f, 0.5f);
                PanelManager.TipRectTransform = tipRootRT;
                tipRoot.transform.SetParent(u2go.transform, false);

                // 创建CatTreasurePanel根节点：CatTreasureRoot
                var catTreasureRoot = new GameObject("CatTreasureRoot");
                catTreasureRoot.layer = LayerMask.NameToLayer("UI");
                var CatTreasureRootRT = catTreasureRoot.AddComponent<RectTransform>();
                CatTreasureRootRT.sizeDelta = new Vector2(0, 0);
                CatTreasureRootRT.anchorMin = Vector2.zero;
                CatTreasureRootRT.anchorMax = new Vector2(1, 1);
                CatTreasureRootRT.pivot = new Vector2(0.5f, 0.5f);
                PanelManager.CatTreasureRectTransform = CatTreasureRootRT;
                catTreasureRoot.transform.SetParent(u2go.transform, false);

                // 创建充值面板 RechargePanel 根节点：RechargePanelRoot
                var rechargePanelRoot = new GameObject("RechargePanelRoot");
                rechargePanelRoot.layer = LayerMask.NameToLayer("UI");
                var RechargePanelRootRT = rechargePanelRoot.AddComponent<RectTransform>();
                RechargePanelRootRT.sizeDelta = new Vector2(0, 0);
                RechargePanelRootRT.anchorMin = Vector2.zero;
                RechargePanelRootRT.anchorMax = new Vector2(1, 1);
                RechargePanelRootRT.pivot = new Vector2(0.5f, 0.5f);
                PanelManager.RechargeRectTransform = RechargePanelRootRT;
                rechargePanelRoot.transform.SetParent(u2go.transform, false);

                // 抽奖根节点不创建了，放到 PanelRoot 根节点下
                //// 创建抽奖面板 LotteryPanel 根节点：LotteryPanelRoot
                //var lotteryPanelRoot = new GameObject("LotteryPanelRoot");
                //lotteryPanelRoot.layer = LayerMask.NameToLayer("UI");
                //var LotteryPanelRootRT = lotteryPanelRoot.AddComponent<RectTransform>();
                //LotteryPanelRootRT.sizeDelta = new Vector2(0, 0);
                //LotteryPanelRootRT.anchorMin = Vector2.zero;
                //LotteryPanelRootRT.anchorMax = new Vector2(1, 1);
                //LotteryPanelRootRT.pivot = new Vector2(0.5f, 0.5f);
                //PanelManager.LotteryRectTransform = LotteryPanelRootRT;
                //lotteryPanelRoot.transform.SetParent(u2go.transform, false);

                // 创建聊天面板 ChatPanel 
                // 根节点 ChatPanelRoot
                var chatPanelRoot = new GameObject("ChatPanelRoot");
                chatPanelRoot.layer = LayerMask.NameToLayer("UI");
                var ChatPanelRootRT = chatPanelRoot.AddComponent<RectTransform>();
                ChatPanelRootRT.sizeDelta = new Vector2(0, 0);
                ChatPanelRootRT.anchorMin = Vector2.zero;
                ChatPanelRootRT.anchorMax = new Vector2(1, 1);
                ChatPanelRootRT.pivot = new Vector2(0.5f, 0.5f);
                PanelManager.ChatRectTransform = ChatPanelRootRT;
                chatPanelRoot.transform.SetParent(u2go.transform, false);

                var guideRoot = new GameObject("GuideRoot");
                guideRoot.layer = LayerMask.NameToLayer("UI");
                var guideMaskRootRT = guideRoot.AddComponent<RectTransform>();
                guideMaskRootRT.sizeDelta = new Vector2(0, 0);
                guideMaskRootRT.anchorMin = Vector2.zero;
                guideMaskRootRT.anchorMax = new Vector2(1, 1);
                guideMaskRootRT.pivot = new Vector2(0.5f, 0.5f);
                PanelManager.GuideMaskRectTransform = guideMaskRootRT;
                guideRoot.transform.SetParent(u2go.transform, false);

                //loading根节点
                var loadingRoot = new GameObject("LoadingRoot");
                loadingRoot.layer = LayerMask.NameToLayer("UI");
                var loadingRootRT = loadingRoot.AddComponent<RectTransform>();
                loadingRootRT.sizeDelta = new Vector2(0, 0);
                loadingRootRT.anchorMin = Vector2.zero;
                loadingRootRT.anchorMax = new Vector2(1, 1);
                loadingRootRT.pivot = new Vector2(0.5f, 0.5f);
                PanelManager.LoadingRectTransform = loadingRootRT;
                loadingRoot.transform.SetParent(u2go.transform, false);

                //popupPanel根节点
                //var popupPanelRoot = new GameObject("PopupPanelRoot");
                //loadingRoot.layer = LayerMask.NameToLayer("UI");
                //var popupPanelRootRT = popupPanelRoot.AddComponent<RectTransform>();
                //popupPanelRootRT.sizeDelta = new Vector2(0, 0);
                //popupPanelRootRT.anchorMin = Vector2.zero;
                //popupPanelRootRT.anchorMax = new Vector2(1, 1);
                //popupPanelRootRT.pivot = new Vector2(0.5f, 0.5f);
                //PanelManager.PopupPanelRectTransform = popupPanelRootRT;
                //popupPanelRoot.transform.SetParent(u2go.transform, false);

                UI2DRoot.renderMode = RenderMode.ScreenSpaceOverlay;
                UI2DRoot.transform.SetAsFirstSibling();

                InitializeManager();
                //PanelManager.LoadingPanel.RegisterEvent();
                //DataManager.LoadTypeDataFromFile();

                UDID = SystemInfo.deviceUniqueIdentifier;
                Debug.Log("设备号: " + UDID);
                GameApplication.Instance.SDKTools.SetAccount(UDID);

                if (PlayerPrefs.HasKey("MusicVolume"))
                {
                    GameApplication.MusicController.volume = PlayerPrefs.GetFloat("MusicVolume");
                }

                if (PlayerPrefs.HasKey("SoundVolume"))
                {
                    GameApplication.SoundEffectController.volume = PlayerPrefs.GetFloat("SoundVolume");
                }

                //PanelManager.LoginPanel.Show();
                switch (LoginType)
                {
                    case EnumConfig.LoginType.LoginPanel:
                        PanelManager.LoginPanel.Show();
                        break;
                    case EnumConfig.LoginType.UDID:
                        PanelManager.DirectConnectPanel.Show();
                        break;
                    default:
                        break;
                }

                //GameObject.Find("CheckUpdate").GetComponent<CheckUpdate>().StartHotFix();
                #region 测试入口
                //PanelManager.BagPanel.Show();
                //PanelManager.TableViewPanel.Show();
                //SceneManager.Instance.EnterArtTestScene();
                #endregion

                //behaviourHandle = GameApplication.Instance.gameObject.AddGameCompoent<BehaviourHandle>();  
                //behaviourHandle.OnDrawGUI = TestGUI;
                //behaviourHandle.OnUpdate = TestUpdate;

                TimeTick = new TimeTick();
                BehaviourManager.AddGameComponent(TimeTick);


                #region sdk settings
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    Debug.Log("初始化sdk数据");

                    ////接入友盟分享
                    //Social.SetAppKey("57833c5367e58e653f0022d9");

                    ////接入QQ分享
                    //Social.SetQQAppIdAndAppKey("1105385125", "n3rzpy39rADSCOBa");

                    ////微信
                    //Social.SetWechatAppIdAndSecret("wx77318c029106799c", "42463a3be622cc9c612646d700b52a22");

                    //if (Application.platform == RuntimePlatform.Android)
                    //{
                    //    Social.SetTargetUrl("http://www.apowo.com/");
                    //}
                    //else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    //{
                    //    Social.SetTargetUrl("http://www.apowo.com/");
                    //}
                }
                #endregion

                #region HardWare
                //if (Application.platform == RuntimePlatform.IPhonePlayer)
                //{
                //    //Debug.Log(UnityEngine.iOS.Device.generation);
                //}

                //Debug.Log("deviceModel => " + SystemInfo.deviceModel);
                //Debug.Log("deviceName => " + SystemInfo.deviceName);
                //Debug.Log("deviceType => " + SystemInfo.deviceType);
                //Debug.Log("graphicsDeviceID => " + SystemInfo.graphicsDeviceID);
                //Debug.Log("graphicsDeviceName => " + SystemInfo.graphicsDeviceName);
                //Debug.Log("graphicsDeviceType => " + SystemInfo.graphicsDeviceType);
                //Debug.Log("graphicsDeviceVersion => " + SystemInfo.graphicsDeviceVersion);
                //Debug.Log("operatingSystem => " + SystemInfo.operatingSystem);
                //Debug.Log("systemMemorySize => " + SystemInfo.systemMemorySize);
                //Debug.Log("graphicsMemorySize => " + SystemInfo.graphicsMemorySize);
                //Debug.Log("deviceUniqueIdentifier => " + SystemInfo.deviceUniqueIdentifier);
                #endregion
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message+e.StackTrace);
            }

        }


        static void InitializeManager()
        {
            ShieldKeyword.Initialize();

            new EnumConfig();

            PanelManager.Initialize();
            SceneManager.Initialize();
            CatManager.Initialize();
            SceneGameObjectManager.Initialize();
            BagManager.Initialize();
            UserManager.Initialize();
            AwardManager.Initialize();
            ShopManager.Initialize();
            ShopManager.Instance.InitConfigs();
            HandbookManager.Initialize();
            HandbookManager.Instance.InitConfigs();
            RoomManager.Initialize();
            RoomManager.Instance.InitConfigs();
            PhotoManager.Initialize();
            PhotoManager.Instance.InitAllPictureOwners();
            GuideManager.Initializer();
            MailManager.Initialize();
            TaskManager.Initialize();
            TaskManager.Instance.InitConfigs();
            //EffectManager.Initlaize();      
        }

       
    }
}

