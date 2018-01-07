using DG.Tweening;
using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Component;
using FlyModel.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Collections.Generic;
using System.Collections;

namespace FlyModel.UI.Panel.Toolbar
{
    public class InfoBar : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "InfoBar";
            }
        }
        //关于广告的一些东西******************************************************************************************************************
        private Button AdvertisButton002_1;
        private Button AdvertisButton003_1;
        private Button AdvertisButton003_2;
        private Button AdvertisButton003_3;
        private Transform Advertising002;
        private Transform Advertising003;
        private Image AdvertisButton002_1_Image;
        private Image AdvertisButton003_1_Image;
        private Image AdvertisButton003_2_Image;
        private Image AdvertisButton003_3_Image;
        //处于等待界面的一系列动作
        private Text times;
        private float waitTime = 5;
        private Button SkipButton;

        //当下的图片的一些信息
        private string nowName = " ";
        private string nowIcon = " ";//图标
        private string nowBanner = " ";//横幅图片的广告
        private string nowCover = " ";//处在封面
        private string nowAppleID = " ";
        private string nowAndroidName = " ";
        //盛放json数据的一些容器
        private List<string> nameList = new List<string>();//产品名字
        private List<string> iconList = new List<string>();//icon图片的地址
        private List<string> bannerList = new List<string>();//banner图片的地址
        private List<string> coverList = new List<string>();//封面图片的地址
        private List<string> appleIDList = new List<string>();//苹果的应用ID
        private List<string> androidNameList = new List<string>();//安卓的应用ID
        //用于上传的数据
        Dictionary<string, string> adsbanner = new Dictionary<string, string>();
        Dictionary<string, string> adsicon1 = new Dictionary<string, string>();
        Dictionary<string, string> adsicon2 = new Dictionary<string, string>();
        Dictionary<string, string> adsicon3 = new Dictionary<string, string>();

        private List<string> ID = new List<string>();
        private List<string> text = new List<string>();

        //盛放图片的临时图片
        private Texture2D texturePhoto = null;
        // 要访问的广告数据的网址的前缀
        private string websitePrefix = "";
        private int adId = 0;
        private string selfPlatform = "";
        private string adWebsite = "";
        //对底下的三个小button分别进行命名
        private string button1Name;
        private string button2Name;
        private string button3Name;
        //button身上附着的图片的路径
        private string buttonicon1path;
        private string buttonicon2path;
        private string buttonicon3path;

        //对按钮显示进行判断
        // private bool IsXianshi=false;
        public bool isKong = false;
        //用于判断当前设备是安卓或者IOS
        private string systemname = "安卓";
        //统计数据时候的两个url
        private string adsShow = "https://tuqing.apowo.com/server/ads/adsShow";
        private string adsPress = "https://tuqing.apowo.com/server/ads/adsPress";
        //获取当前的设备ID
        private string devicelID = "66666666666";
        public bool isicon;
        //广告结束************************************************************************************************************************
        private Image btnImage;
        private SoundButton MenuBtn;

        public Action onMenuClickedHandler;

        private GameObject bottomBar;
        private SoundButton awardBtn;

        private Text silverTF;
        private Text goldTF;

        private SoundButton cameraBtn;
        private SoundButton mailBtn;
        //private SoundButton settingBtn;
        private SoundButton signBtn;
        private SoundButton shareBtn;
        private SoundButton exchangeBtn;
        private SoundButton teachBtn;
        private SoundButton lotteryBtn;  // 抽奖按钮
        private SoundButton chatButton;  // 聊天按钮 打开聊天频道

        private Button Exit;
        private Image ExitImage;
        private GameObject topBarGO;

        private Text newMailCountTF;

        private EnumConfig.InfoLayoutMode layoutMode = EnumConfig.InfoLayoutMode.Head;

        private GameObject silverGO;
        private GameObject goldGO;
        private GameObject headGO;
        private GameObject MenuBtnGo;   // 菜单按钮

        private Vector3 goldGOPosHeadMode;
        private Vector3 silverGOPosHeadMode;
        private Vector3 MenuBtnGoPosInfo;   // 菜单按钮平时的位置信息      

        private Vector3 goldGOPosCurrencyMode;
        private Vector3 silverGOPosCurrencyMode;
        private Vector3 MenuBtnGoShopPosInfo;   // 菜单按钮在商店界面时的位置信息

        private DelayController delayController;

        private SkeletonGraphic mailAnimation;

        private Image headImage;

        // 用来完成在兑换领奖对象禁用签到启用时移动签到到兑换领奖的位置
        private GameObject exchangeGO;
        private GameObject signInGO;
        private RectTransform signInRT;

        // 隐藏用的
        private GameObject exchangeObject;
        private GameObject inviteObject;

        public InfoBar(RectTransform parent) : base(parent)
        {

        }

        public override bool Show()
        {
            IsNeedPushToPanelStack = false;
            bool flag = base.Show();
            return flag;
        }

        public override void Load()
        {
            base.Load();

            loadBottomBar();
        }

        protected override void Initialize(GameObject go)
        {
            topBarGO = go;

            MenuBtnGo = go.transform.Find("Button").gameObject;
            MenuBtn = new SoundButton(MenuBtnGo, onMenuClicked, ResPathConfig.MAIN_MENU);
            Exit = go.transform.Find("Button").GetComponent<Button>();
            ExitImage = go.transform.Find("Button").GetComponent<Image>();
            Exit.onClick.AddListener(AdvertiseReturn);


            btnImage = MenuBtnGo.GetComponent<Image>();
            SetBtnImage(EnumConfig.InfoBarBtnMode.Menu);

            silverTF = go.transform.Find("SilverFish/Text").GetComponent<Text>();
            goldTF = go.transform.Find("GoldFish/Text").GetComponent<Text>();
            UpdateData();

            silverGO = go.transform.Find("SilverFish").gameObject;
            goldGO = go.transform.Find("GoldFish").gameObject;

            headGO = go.transform.Find("touxiang").gameObject;
            Vector3 headGOPos = headGO.transform.localPosition;
            Vector3 MenuBtnGoPos = MenuBtnGo.transform.localPosition;   // 取得菜单按钮的位置信息

            //silverGOPosHeadMode = new Vector3(headGOPos.x, headGOPos.y - 120, 0);
            //goldGOPosHeadMode = new Vector3(headGOPos.x, headGOPos.y - 160, 0);
            //silverGOPosHeadMode = new Vector3(headGOPos.x + 140, headGOPos.y + 45, 0);
            //goldGOPosHeadMode = new Vector3(headGOPos.x + 260, headGOPos.y + 45, 0);

            // 设置金银鱼干平时显示时的位置
            goldGOPosHeadMode = new Vector3(headGOPos.x + 144, (float)(headGOPos.y + 115.2) - 81, 0);
            silverGOPosHeadMode = new Vector3((float)(headGOPos.x + 286.1), (float)(headGOPos.y + 34.2), 0);
            // 设置菜单按钮平时显示时的位置
            MenuBtnGoPosInfo = new Vector3(MenuBtnGoPos.x, MenuBtnGoPos.y, 0);

            // 设置菜单按钮在商店界面时的位置
            MenuBtnGoShopPosInfo = new Vector3(MenuBtnGoPos.x, -45, 0);

            // 设置金银鱼干在商店界面显示时的位置
            goldGOPosCurrencyMode = new Vector3((float)(headGOPos.x + 38.2), headGOPos.y + 30, 0);
            silverGOPosCurrencyMode = new Vector3(headGOPos.x + 184, headGOPos.y + 30, 0);

            new SoundButton(go.transform.Find("touxiang/touxiang").gameObject, () =>
            {
                if (PanelManager.IsCurrentPanel(PanelManager.headPanel) == false)
                {
                    PanelManager.HeadPanel.Show();

                    //CommandHandle.Send(Proto.ServerMethod.FullCat, null);
                }

                //if (PanelManager.IsCurrentPanel(PanelManager.headSelectPanel) == false)
                //{
                //    PanelManager.HeadSelectPanel.Show();
                //}
            });

            headImage = go.transform.Find("touxiang/touxiang/Image").GetComponent<Image>();

            delayController = go.AddComponent<DelayController>();

            UpdateHead();

            //广告的获取*************************************
            //
            //*****************

            Debug.Log("lalalalalallalalalalalla");
            AdvertisButton002_1 = go.transform.Find("Advertising002/Button").GetComponent<Button>();
            AdvertisButton002_1.onClick.AddListener(OnClickAdvertisButton002_1);
            AdvertisButton003_1 = go.transform.Find("Advertising003/Advertising003_1").GetComponent<Button>();
            AdvertisButton003_1.onClick.AddListener(OnClickAdvertisButton003_1);
            AdvertisButton003_2 = go.transform.Find("Advertising003/Advertising003_2").GetComponent<Button>();
            AdvertisButton003_2.onClick.AddListener(OnClickAdvertisButton003_2);
            AdvertisButton003_3 = go.transform.Find("Advertising003/Advertising003_3").GetComponent<Button>();
            AdvertisButton003_3.onClick.AddListener(OnClickAdvertisButton003_3);
            Advertising002 = go.transform.Find("Advertising002").GetComponent<Transform>();
            Advertising003 = go.transform.Find("Advertising003").GetComponent<Transform>();
            //需要改变的一系列button图片          
            AdvertisButton002_1_Image = go.transform.Find("Advertising002/Button/Image").GetComponent<Image>();
            AdvertisButton003_1_Image = go.transform.Find("Advertising003/Advertising003_1/Image").GetComponent<Image>();
            AdvertisButton003_2_Image = go.transform.Find("Advertising003/Advertising003_2/Image").GetComponent<Image>();
            AdvertisButton003_3_Image = go.transform.Find("Advertising003/Advertising003_3/Image").GetComponent<Image>();
            GetdeviceID();
            SystemName();
            delayController.StartCoroutine(GetWebsiteData());
            IsClose();
            //***************
            //
            //广告获取结束********************

        }

        private void loadBottomBar()
        {
            string assetName = "BottomBar";
            ResourceLoader.Instance.TryLoadClone(assetName.ToLower(), assetName, (go) =>
            {
                bottomBar = go;

                go.transform.SetParent(PanelManager.ToolbarRectTransform, false);

                awardBtn = new SoundButton(go.transform.Find("AwardBtn").gameObject, () => {
                    PanelManager.AwardPanel.Show();
                });

                cameraBtn = new SoundButton(go.transform.Find("CameraBtn").gameObject, () => {
                    PanelManager.TakePicturePanel.Show();  // 测试聊天面板1
                    //PanelManager.ChatPanel.Show();  // 测试聊天面板2

                    //PanelManager.TaskPanel.Show();
                    //GameApplication.Instance.ManulDisconnected();
                    //CommandHandle.Send(Proto.ServerMethod.FullCat, null);
                    //PanelManager.GuidePanel.Show();
                    //PhoneCameraUtil.ShowWebCam(delayController);
                });

                //settingBtn = new SoundButton(go.transform.Find("seting").gameObject, () =>
                //{
                //    if (PanelManager.IsCurrentPanel(PanelManager.settingPanel) == false)
                //    {
                //        PanelManager.SettingPanel.Show();
                //    }
                //});
                if (true == CommandHandle.isHideButton)  // 隐藏兑换领奖和邀请有礼按钮
                {
                    Debug.Log("isHideButton == true");
                    exchangeObject = go.transform.Find("Task").gameObject;
                    inviteObject = go.transform.Find("ShareBtn").gameObject;
                    exchangeObject.SetActive(false);
                    inviteObject.SetActive(false);
                    Debug.Log("隐藏完毕");
                }
                else
                {
                    shareBtn = new SoundButton(go.transform.Find("ShareBtn").gameObject, () =>
                    {
                        if (PanelManager.IsCurrentPanel(PanelManager.sharePanel) == false)
                        {
                            PanelManager.SharePanel.Show();
                        }
                    });

                    exchangeBtn = new SoundButton(go.transform.Find("Task").gameObject, () =>
                    {
                        if (PanelManager.IsCurrentPanel(PanelManager.exchangePanel) == false)
                        {
                            PanelManager.ExchangePanel.Show();
                        }
                    });
                }

                teachBtn = new SoundButton(go.transform.Find("Techer").gameObject, () =>
                {
                    if (PanelManager.IsCurrentPanel(PanelManager.tutorialPanel) == false)
                    {
                        PanelManager.TutorialPanel.Show();
                    }
                });

                // 邮件按钮
                mailBtn = new SoundButton(go.transform.Find("MailButton").gameObject, () => {
                    CommandHandle.Send(Proto.ServerMethod.GetMails, null);
                    //PanelManager.CatTreasureGetPanel.Show();
                });

                // 签到按钮
                signBtn = new SoundButton(go.transform.Find("seven").gameObject, () =>
                {
                    // 显示签到面板
                    if (PanelManager.IsCurrentPanel(PanelManager.signPanel) == false)
                    {
                        PanelManager.SignPanel.Show();
                    }
                });

                // 点击抽奖按钮
                lotteryBtn = new SoundButton(go.transform.Find("qiandao").gameObject, () =>
                {
                    // 显示抽奖面板
                    if (PanelManager.IsCurrentPanel(PanelManager.lotteryPanel) == false)
                    {
                        PanelManager.LotteryPanel.Show();
                    }
                });

                // 聊天按钮
                chatButton = new SoundButton(go.transform.Find("ChatButton").gameObject, () =>
                {
                    PanelManager.ChatPanel.Show();
                });

                newMailCountTF = go.transform.Find("MailButton/Text").GetComponent<Text>();

                mailAnimation = go.transform.Find("MailButton/mail").GetComponent<SkeletonGraphic>();
                //delayController.DelayInvoke(() =>
                //{
                //    SkeletonGraphic spine = cat.Root.GetComponent<SkeletonGraphic>();
                //    spine.AnimationState.ClearTracks();
                //    spine.AnimationState.SetAnimation(0, info.animationName, true);
                //}, 0.5f);

                UpdateAwardBtnActive();
                updateMailBtnActive();

                exchangeGO = go.transform.Find("Task").gameObject;
                signInGO = go.transform.Find("seven").gameObject;
                signInRT = go.transform.Find("seven").GetComponent<RectTransform>();
                //// 兑换领奖禁用签到启用时，签到移到兑换领奖的位置 1
                //if (exchangeGO.activeSelf == false && signInGO.activeSelf == true)
                //{
                //    signInRT.DOMoveX(335.8f, 0f);
                //}
            });
        }

        private void onMenuClicked()
        {
            if (onMenuClickedHandler != null)
            {
                GuideManager.Instance.IsGestureTouchEffective("Button");

                onMenuClickedHandler();
            }
        }

        public void SetMenuClickedHandler(Action handler)
        {
            onMenuClickedHandler = handler;
        }

        public void SetBtnImage(EnumConfig.InfoBarBtnMode mode)
        {
            switch (mode)
            {
                case EnumConfig.InfoBarBtnMode.Menu:
                    btnImage.sprite = MenuBtn.GameObject.transform.Find("Menu").GetComponent<Image>().sprite;
                    break;
                case EnumConfig.InfoBarBtnMode.Close:
                    btnImage.sprite = MenuBtn.GameObject.transform.Find("Close").GetComponent<Image>().sprite;
                    break;
                default:
                    break;
            }
        }

        public override void SetInfoBar()
        {
            if (PanelPrefab != null)
            {
                PanelPrefab.SetActive(true);
                PanelManager.InfoBar.SetMenuClickedHandler(() => { PanelManager.ShopPanel.Show(); MoveTransform(); });
                //PanelManager.InfoBar.SetMenuClickedHandler(() => { GameApplication.Instance.ManulDisconnected(); });
                PanelManager.InfoBar.SetBtnImage(EnumConfig.InfoBarBtnMode.Menu);

                //// 兑换领奖禁用签到启用时，签到移到兑换领奖的位置 2
                //if (exchangeGO.activeSelf == false && signInGO.activeSelf == true)
                //{
                //    signInRT.DOMoveX(335.8f, 0f);
                //}
            }

            //UpdateInfoLayout(PanelManager.HasPanelOpen() == false ? EnumConfig.InfoLayoutMode.Head : EnumConfig.InfoLayoutMode.Currency);
            if (false == PanelManager.HasPanelOpen())
            {
                PanelManager.InfoBar.UpdateInfoLayout(EnumConfig.InfoLayoutMode.Head);
            }
            else
            {
                UpdateInfoLayout(EnumConfig.InfoLayoutMode.Currency);
            }

            //// 兑换领奖禁用签到启用时，签到移到兑换领奖的位置 3
            //if (exchangeGO.activeSelf == false && signInGO.activeSelf == true)
            //{
            //    signInRT.DOMoveX(335.8f, 0f);
            //}
        }

        public void SetTempMenu(Action callback, EnumConfig.InfoBarBtnMode mode)
        {
            PanelPrefab.SetActive(true);
            SetMenuClickedHandler(callback);
            SetBtnImage(mode);
        }

        public void UpdateAwardBtnActive()
        {
            if (awardBtn != null)
            {
                awardBtn.SetActive(AwardManager.Instance.AwardList.Count > 0 && PanelManager.HasPanelOpen() == false);
            }
        }

        public void updateCamareBtnActive()
        {
            if (cameraBtn != null)
            {
                cameraBtn.SetActive(PanelManager.HasPanelOpen() == false);
            }
        }

        public void updateShareBtnActive()
        {
            if (shareBtn != null)
            {
                shareBtn.SetActive(PanelManager.HasPanelOpen() == false);
            }
        }

        public void updateExchangeBtnActive()
        {
            if (exchangeBtn != null)
            {
                exchangeBtn.SetActive(PanelManager.HasPanelOpen() == false);

                //// 兑换领奖禁用签到启用时，签到移到兑换领奖的位置 4
                //if (exchangeGO.activeSelf == false && signInGO.activeSelf == true)
                //{
                //    signInRT.DOMoveX(335.8f, 0f);
                //}
            }
        }

        public void updateTeachBtnActive()
        {
            if (teachBtn != null)
            {
                teachBtn.SetActive(PanelManager.HasPanelOpen() == false);
            }
        }

        //public void updateSettingBtnActive()
        //{
        //    if (settingBtn != null)
        //    {
        //        settingBtn.SetActive(PanelManager.HasPanelOpen() == false);
        //    }
        //}

        public void updateMailBtnActive()
        {
            if (mailBtn != null)
            {
                newMailCountTF.text = MailManager.Instance.NewMailCount.ToString();
                //mailBtn.SetActive(PanelManager.HasPanelOpen() == false && MailManager.Instance.NewMailCount>0);
                mailBtn.SetActive(PanelManager.HasPanelOpen() == false);

                mailAnimation.AnimationState.SetAnimation(0, MailManager.Instance.NewMailCount > 0 ? "play" : "stop", true);
            }
        }

        // 签到
        public void updateSignBtnActive()
        {
            // 禁用签到
            //if (signBtn != null)
            //{
            //    // 没有面板打开并且没有签完7天时显示签到按钮
            //    signBtn.SetActive(PanelManager.HasPanelOpen() == false && UserManager.Instance.Me.SignAllDays == false);

            //    //// 兑换领奖禁用签到启用时，签到移到兑换领奖的位置 5
            //    //if (exchangeGO.activeSelf == false && signInGO.activeSelf == true)
            //    //{
            //    //    signInRT.DOMoveX(335.8f, 0f);
            //    //}
            //}
        }

        // 更新抽奖按钮是否启用
        public void UpdateLotteryBtnActive()
        {
            if (lotteryBtn != null)
            {
                lotteryBtn.SetActive(PanelManager.HasPanelOpen() == false);
            }
        }

        // 更新聊天按钮是否启用
        public void UpdateChatBtnActive()
        {
            if (chatButton != null)
            {
                chatButton.SetActive(PanelManager.HasPanelOpen() == false);
            }
        }

        //更新广告是否启用
        public void UpdateAdvertiseActive()
        {
        }

        public void UpdateData()
        {
            User me = UserManager.Instance.Me;
            silverTF.text = me.LowCurrency.ToString();
            goldTF.text = me.HighCurrency.ToString();

            //goldTxt.text = me.HighCurrency.ToString();
            //silverTxt.text = me.LowCurrency.ToString();
            Debug.Log("=====================================");
        }

        public void ShowMenuBtn(bool isShow)
        {
            MenuBtn.SetActive(isShow);
        }

        public void ShowTopBar(bool isShow)
        {
            topBarGO.SetActive(isShow);
        }

        public void UpdateInfoLayout(EnumConfig.InfoLayoutMode mode)
        {
            layoutMode = mode;

            switch (mode)
            {
                case EnumConfig.InfoLayoutMode.Head:
                    if (null != silverGO)
                    {
                        silverGO.transform.localPosition = silverGOPosHeadMode;
                        goldGO.transform.localPosition = goldGOPosHeadMode;
                        // 在平时设置菜单按钮的位置为平时的位置
                        MenuBtnGo.transform.localPosition = MenuBtnGoPosInfo;
                    }
                    break;
                case EnumConfig.InfoLayoutMode.Currency:
                    silverGO.transform.localPosition = silverGOPosCurrencyMode;
                    goldGO.transform.localPosition = goldGOPosCurrencyMode;
                    // 在商店界面设置菜单按钮的位置为商店界面时的位置
                    MenuBtnGo.transform.localPosition = MenuBtnGoShopPosInfo;
                    break;
                default:
                    break;
            }

            //headGO.SetActive(mode == EnumConfig.InfoLayoutMode.Head);
            if (mode == EnumConfig.InfoLayoutMode.Head)
            {
                if (null != headGO)
                {
                    headGO.SetActive(true);
                }
            }
            else
            {
                if (null != headGO)
                {
                    headGO.SetActive(false);
                }
            }
        }

        public void UpdateHead()
        {
            ResourceLoader.Instance.TryLoadPic(ResPathConfig.USER_HEAD, UserManager.Instance.GetCurrentHeadCode(), (texture) =>
            {
                headImage.sprite = texture as Sprite;
                headImage.SetNativeSize();
            });
        }

        // 更新金银鱼干的数量显示
        public void UpdateFishDisplay(int type, int amount)
        {
            // 抽奖获得银鱼干时
            if (1 == type)
            {
                // 实例化
                User me = UserManager.Instance.Me;
                // 更新银鱼干数量
                me.LowCurrency += amount;
                // 更新显示
                silverTF.text = me.LowCurrency.ToString();
            }
            // 抽奖获得金鱼干时 type == 2时
            else
            {
                // 实例化
                User me = UserManager.Instance.Me;
                // 更新金鱼干数量
                me.HighCurrency += amount;
                // 更新显示
                goldTF.text = me.HighCurrency.ToString();
            }
        }

        //交叉广告、、、、、、、、*****************************************************************、、、、、、、、、、、、
        /// <summary>
        /// ///////////////////
        /// </summary>
        //获取当前的设备ID
        private void GetdeviceID()
        {

            devicelID = TalkingDataGA.GetDeviceId();
            Debug.Log(devicelID);
            if (devicelID == null)
            {
                devicelID = "Eidor";
            }
        }

        public void MoveTransform()
        {
            Debug.Log(ExitImage.name + "-----------------------------]]]]]]]]]]]]]");
            Debug.Log(Advertising002.localPosition);
            Debug.Log(Advertising003.localPosition);
            Advertising002.localPosition = new Vector3(Advertising002.localPosition.x, Advertising002.localPosition.y - 1000, Advertising002.localPosition.z);
            Advertising003.localPosition = new Vector3(Advertising003.localPosition.x, Advertising003.localPosition.y - 1000, Advertising003.localPosition.z);
        }
        //使广告回到原位 
        private void AdvertiseReturn()
        {
            Advertising002.localPosition = new Vector3(0, -1073, 0);
            Advertising003.localPosition = new Vector3(0, -1065, 0);
        }
        //用于检测当前设备
        private void SystemName()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                systemname = "android";
            }
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                systemname = "ios";
            }
            else
            {
                systemname = "android";
            }
            Debug.Log(systemname);
        }

        //用来跳转到应用商店
        private void TiaoZhuan(string nameself1)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                string[] sArray = nameself1.Split('.');
                Debug.Log(sArray[0] + "))))))))))))))");
                if (sArray[0] == "com")
                {

                    AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                    jo.Call("isAppInstalled", nameself1);
                }
                else
                {
                    Application.OpenURL(nameself1);
                }
            }
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                Debug.Log(nameself1);
                string url = "itms-apps://" + nameself1;
                //IOSadvertise(nameself1);
                Application.OpenURL(url);
            }
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Debug.Log(nameself1);
                string url = "itms-apps://" + nameself1;
                //IOSadvertise(nameself1);
                Application.OpenURL(url);

            }
        }

        //点击横条广告时
        private void OnClickAdvertisButton002_1()
        {
            Debug.Log("////////////////////////////////////////////////////////");
            delayController.StartCoroutine(Post(adsPress, adsbanner));
            TiaoZhuan(nowName);
        }
        //点击第一个小button
        private void OnClickAdvertisButton003_1()
        {
            delayController.StartCoroutine(Post(adsPress, adsicon1));
            TiaoZhuan(button1Name);
        }
        //点击第二个小广告button
        private void OnClickAdvertisButton003_2()
        {
            delayController.StartCoroutine(Post(adsPress, adsicon2));
            TiaoZhuan(button2Name);
        }
        //点击第三个
        private void OnClickAdvertisButton003_3()
        {
            delayController.StartCoroutine(Post(adsPress, adsicon3));
            TiaoZhuan(button3Name);
        }

        // 得到要访问的广告的网址的前缀
        private IEnumerator GetWebsiteData()
        {
            WWW www = new WWW("http://snpole.com/ads.json");
            yield return www;
            if (www.isDone && www.error == null)
            {
                JsonData jsonData = JsonMapper.ToObject(www.text);
                websitePrefix = jsonData["api"].ToString();
                GetAdWebsite();
            }
        }

        // 得到要访问的广告地址
        private void GetAdWebsite()
        {
            if (websitePrefix != "")
            {
                adId = 5;
                if (Application.platform == RuntimePlatform.Android)
                {
                    selfPlatform = "1";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    selfPlatform = "0";
                }
                else
                {
                    selfPlatform = "1";  // 编辑器里时模拟安卓
                }
                adWebsite = websitePrefix + "/" + adId + "/" + selfPlatform;
                delayController.StartCoroutine(GetAdData(adWebsite));
            }
            else
            {
                Debug.Log("没有得到广告网址前缀");
            }
        }

        // 取得广告数据
        private IEnumerator GetAdData(string website)
        {
            WWW www = new WWW(website);
            yield return www;
            if (www.isDone && www.error == null)
            {
                JsonData jsonData = JsonMapper.ToObject(www.text);
                JsonData dataJsonData = jsonData["data"];
                SaveData(dataJsonData);
            }
        }

        //用来将json文件存储起来
        private void SaveData(JsonData jd)
        {
            foreach (JsonData temp in jd as IList)
            {
                nameList.Add(temp["name"].ToString());
                iconList.Add(temp["icon"].ToString());
                bannerList.Add(temp["banner"].ToString());
                coverList.Add(temp["cover"].ToString());
                appleIDList.Add(temp["appleID"].ToString());
                androidNameList.Add(temp["androidName"].ToString());
                JsonData modeJsonData = temp["modes"];
                foreach (JsonData modeTemp in modeJsonData as IList)
                {
                    Debug.Log(modeTemp["text"].ToString());
                    ID.Add(modeTemp["id"].ToString());
                    text.Add(modeTemp["text"].ToString());
                }
            }
            if (nameList.Count == 0)
            {
                isKong = true;
            }
            else
            {
                isKong = false;
                Advertising002.gameObject.SetActive(false);
                Advertising003.gameObject.SetActive(false);
            }
            //当数据为空时，关闭所有广告   
            JudgeButton();
            RePhoto();
            //上传Show的数据
            foreach (var item in adsbanner)
            {
                Debug.Log(item.Key + item.Value);
            }
            if (isKong == false)
            {
                if (isicon == false)
                {
                    delayController.StartCoroutine(Post(adsShow, adsbanner));
                }
                if (isicon == true)
                {
                    delayController.StartCoroutine(Post(adsShow, adsicon1));
                    delayController.StartCoroutine(Post(adsShow, adsicon2));
                    delayController.StartCoroutine(Post(adsShow, adsicon3));
                }
                Debug.Log("数据上传完毕！！！！！！！！！！！！！！！");
            }
            if (isKong == true)
            {
                Advertising002.gameObject.SetActive(false);
                Advertising003.gameObject.SetActive(false);
            }
        }
        //开始加载图片
        private void RePhoto()
        {
            JudgeName();
            JudgeIconName1();
            JudgeIconName2();
            JudgeIconName3();
            ReplacePhoto();
        }

        //通过判断名字来进行显示
        public void JudgeName()
        {
            int i = UnityEngine.Random.Range(0, nameList.Count);
            //选择平台
            if (nameList.Count != 0)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    nowName = androidNameList[i];
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {

                    nowName = appleIDList[i];

                }
                else
                {
                    nowName = androidNameList[i];  // 编辑器里时模拟安卓
                }
                nowIcon = iconList[i];
                nowBanner = bannerList[i];
                nowCover = coverList[i];
                adsbanner.Add("productID", "1");
                // adsbanner.Add("time", systemTime);
                adsbanner.Add("system", systemname);
                adsbanner.Add("deviceID", devicelID);
                adsbanner.Add("adsName", nameList[i]);
                adsbanner.Add("adsType", "2");
                foreach (var item in adsbanner)
                {
                    Debug.Log(item);
                }
            }
        }


        //随机进行iconbutton1的设置
        public void JudgeIconName1()
        {
            int i = UnityEngine.Random.Range(0, nameList.Count);
            string name4 = " ";
            if (nameList.Count != 0)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    name4 = androidNameList[i];
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    name4 = appleIDList[i];
                }
                else
                {
                    name4 = androidNameList[i];  // 编辑器里时模拟安卓
                }
                button1Name = name4;
                buttonicon1path = iconList[i];

                adsicon1.Add("productID", "1");
                // adsicon1.Add("time", systemTime);
                adsicon1.Add("system", systemname);
                adsicon1.Add("deviceID", devicelID);
                adsicon1.Add("adsName", nameList[i]);
                adsicon1.Add("adsType", "3");
            }
        }

        //随机进行iconbutton2的设置
        public void JudgeIconName2()
        {
            int i = UnityEngine.Random.Range(0, nameList.Count);
            string name4 = " ";
            if (nameList.Count != 0)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    name4 = androidNameList[i];
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    name4 = appleIDList[i];
                }
                else
                {
                    name4 = androidNameList[i];  // 编辑器里时模拟安卓
                }
                button2Name = name4;
                buttonicon2path = iconList[i];

                adsicon2.Add("productID", "1");
                //  adsicon2.Add("time", systemTime);
                adsicon2.Add("system", systemname);
                adsicon2.Add("deviceID", devicelID);
                adsicon2.Add("adsName", nameList[i]);
                adsicon2.Add("adsType", "3");
            }
        }

        //随机进行iconbutton3的设置
        public void JudgeIconName3()
        {
            int i = UnityEngine.Random.Range(0, nameList.Count);
            string name4 = " ";
            //判断设备名
            if (nameList.Count != 0)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    name4 = androidNameList[i];
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    name4 = appleIDList[i];
                }
                else
                {
                    name4 = androidNameList[i];  // 编辑器里时模拟安卓
                }
                //记录下button的名字和路径
                button3Name = name4;
                buttonicon3path = iconList[i];
                //存储数据进字典
                adsicon3.Add("productID", "1");
                adsicon3.Add("system", systemname);
                adsicon3.Add("deviceID", devicelID);
                adsicon3.Add("adsName", nameList[i]);
                adsicon3.Add("adsType", "3");
            }
        }

        //更换广告的图片
        private void ReplacePhoto()
        {
            if (isKong == false)
            {
                delayController.StartCoroutine(WaitLoad(buttonicon1path, AdvertisButton003_1_Image));
                delayController.StartCoroutine(WaitLoad(buttonicon2path, AdvertisButton003_2_Image));
                delayController.StartCoroutine(WaitLoad(buttonicon3path, AdvertisButton003_3_Image));
                delayController.StartCoroutine(WaitLoad(nowBanner, AdvertisButton002_1_Image));
            }
        }

        //加载图片
        IEnumerator WaitLoad(string fileName, Image image)
        {
            double startTime = (double)Time.time;
            string path = " ";
            if (Application.platform == RuntimePlatform.Android)
            {
                path = fileName;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                path = fileName;
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                path = fileName;
            }
            WWW wwwTexture = new WWW(path);
            yield return wwwTexture;
            texturePhoto = wwwTexture.texture;
            Sprite sprite = Sprite.Create(texturePhoto, new Rect(0, 0, texturePhoto.width, texturePhoto.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;
            startTime = (double)Time.time - startTime;
        }

        //判断三个button和一个button的显示
        private void JudgeButton()
        {
            if (isKong == false)
            {
                if (text[0] == "固定位banner")
                {
                    Advertising002.gameObject.SetActive(true);
                    Advertising003.gameObject.SetActive(false);
                    isicon = false;
                }
                else if (text[0] == "固定位logo")
                {
                    Advertising002.gameObject.SetActive(false);
                    Advertising003.gameObject.SetActive(true);
                    isicon = true;
                }
                else
                {
                    Advertising002.gameObject.SetActive(false);
                    Advertising003.gameObject.SetActive(false);
                    isicon = false;
                }
            }
        }

        //判断有无数据并关闭
        public void IsClose()
        {
            if (isKong == true)
            {
                Advertising002.gameObject.SetActive(false);
                Advertising003.gameObject.SetActive(false);
            }
        }

        /// <summary>  
        /// 指定Post地址使用Get 方式获取全部字符串  
        /// </summary>  
        /// <param name="url">请求后台地址</param>  
        /// <returns></returns>  
        public static IEnumerator Post(string url, Dictionary<string, string> dic)
        {

            WWWForm form = new WWWForm();
            //从集合中取出所有参数，设置表单参数（AddField()).  
            foreach (KeyValuePair<string, string> post_arg in dic)
            {
                form.AddField(post_arg.Key, post_arg.Value);
                Debug.Log(post_arg.Key + "=" + post_arg.Value);
            }
            WWW www = new WWW(url, form);
            yield return www;
            if (www.error != null)
            {
                //POST请求失败  
                //mContent = "error :" + www.error;
                Debug.Log("error : " + www.error);
            }
            else
            {
                //POST请求成功  
                Debug.Log(www.text);
            }
        }
    }
}
