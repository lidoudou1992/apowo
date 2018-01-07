using System;
using System.Collections.Generic;
using FlyModel.UI;
using UnityEngine;
using FlyModel.UI.Panel.Bag;
using FlyModel.UI.Panel.Toolbar;
using FlyModel.UI.Panel.LoadingPanel;
using FlyModel.UI.Panel.MailPanel;
using FlyModel.UI.Panel.ShopPanel;
using FlyModel.UI.Panel.SystemBar;
using FlyModel.UI.Panel;
using FlyModel.UI.Panel.HandbookPanel;
using FlyModel.UI.Panel.CatPopupPanel;
using FlyModel.UI.Panel.GuidePanel;
using FlyModel.UI.Panel.SettingPanel;
using FlyModel.UI.Panel.FoodPopupPanel;
using FlyModel.UI.Panel.Alert;
using FlyModel.UI.Panel.SceneChangePanel;
using FlyModel.UI.Panel.TreasurePanel;
using FlyModel.UI.Panel.TakePicturePanel;
using FlyModel.UI.Panel.PicturesPanel;
using FlyModel.UI.Panel.PicturePreviewPanel;
using FlyModel.UI.Panel.ScenePhotoPanel;
using FlyModel.UI.Panel.ScenePhotoPreviewPanel;
using FlyModel.UI.Panel.CreateCharacterPanel;
using FlyModel.UI.Panel.LoginPanel;
using FlyModel.UI.Panel.AdvisePanel;
using FlyModel.UI.Panel.TaskPanel;
using FlyModel.UI.Panel.TipPanel;
using FlyModel.UI.Panel.HeadPanel;
using FlyModel.UI.Panel.SignPanel;
using FlyModel.UI.Panel.SharePanel;
using FlyModel.UI.Panel.PictureSharePanel;
using FlyModel.UI.Panel.ExchangePanel;
using FlyModel.UI.Panel.HeadSelectPanel;
using FlyModel.UI.Panel.CatTreasurePanel;
using FlyModel.UI.Panel.RechargePanel;
using FlyModel.UI.Panel.LotteryPanel;
using FlyModel.UI.Panel.ChatPanel;

namespace FlyModel
{
    public static class PanelManager
    {
        public static RectTransform PanelRectTransform;
        public static RectTransform ToolbarRectTransform;
        public static RectTransform LoadingRectTransform;
        public static RectTransform GuideMaskRectTransform;
        public static RectTransform TipRectTransform;   // 缓动通知
        public static RectTransform CatTreasureRectTransform;   // 猫咪宝贝
        //public static RectTransform PopupPanelRectTransform;
        public static RectTransform RechargeRectTransform;   // 充值面板
        //public static RectTransform LotteryRectTransform;  // 抽奖面板 不需要了，因为抽奖面板放到了PanelRoot根节点下
        public static RectTransform ChatRectTransform;  // 聊天

        private static Stack<PanelBase> panelStack = new Stack<PanelBase>();

        public static PanelBase GetPanel(string bundleName)
        {
            foreach (var panel in panelStack)
            {
                Debug.Log(panel.BundleName);
                if (panel.BundleName == bundleName)
                {
                    return panel;

                }
            }

            return null;
        }

        public static void OpenPanel(PanelBase p)
        {
            panelStack.Push(p);
            Debug.Log("OpenPanel: " + p.GetType().Name + " " + panelStack.Count);
        }

        public static void CloseCurrent()
        {
            if (panelStack.Count > 0)
            {
                panelStack.Pop();
                Debug.Log("还剩下 " + panelStack.Count + " 个面板");
                if (panelStack.Count > 0)
                {                  
                    panelStack.Peek().SetInfoBar();
                    panelStack.Peek().Focus();
                    panelStack.Peek().RefreshWhenBack();
                }
                else
                {
                    if (infoBar != null)
                    {
                        Debug.Log("=========================" + infoBar);
                        infoBar.SetInfoBar();
                    }
                }
            }
        }

        public static void CloseAll()
        {
            while (panelStack.Count > 0)
            {
                var p = panelStack.Pop();
                //p.Hide();
                p.Close(true);
            }

            if (infoBar != null)
            {
                infoBar.SetInfoBar();
            }
        }

        /// <summary>
        /// 返回当前的面板或空值
        /// </summary>
        public static PanelBase CurrentPanel
        {
            get
            {
                if (panelStack.Count > 0)
                {
                    return panelStack.Peek();
                }
                return null;
            }
        }

        public static void Initialize()
        {

        }

        public static LoginPanel loginPanel;
        public static LoginPanel LoginPanel
        {
            get
            {
                if (loginPanel == null)
                {
                    loginPanel = new LoginPanel();
                }
                return PanelManager.loginPanel;
            }

        }

        public static BagPanel bagPanel;
        public static BagPanel BagPanel
        {
            get
            {
                if (bagPanel == null)
                {
                    bagPanel = new BagPanel();
                }
                else
                {
                    LoadSystemBar(bagPanel.transform);
                    BringSystemBarToTop();
                }
                return bagPanel;
            }

        }

        public static InfoBar infoBar;
        public static InfoBar InfoBar
        {
            get
            {
                if (infoBar == null)
                {
                    infoBar = new InfoBar(PanelRectTransform);
                }
                return infoBar;
            }
        }

        // 实例化一个缓动通知面板：TipPanel
        public static TipPanel tipPanel;
        public static TipPanel TipPanel
        {
            get
            {
                if (tipPanel == null)
                {
                    tipPanel = new TipPanel(TipRectTransform);
                }
                return tipPanel;
            }
        }

        //// 实例化一个猫咪宝贝面板：CatTreasurePanel
        //public static CatTreasurePanel catTreasurePanel;
        //public static CatTreasurePanel CatTreasurePanel
        //{
        //    get
        //    {
        //        if (null == catTreasurePanel)
        //        {
        //            catTreasurePanel = new CatTreasurePanel(CatTreasureRectTransform);
        //        }
        //        return catTreasurePanel;
        //    }
        //}

        // 实例化一个显示得到猫咪宝贝面板：CatTreasureDisplayPanel
        public static CatTreasureDisplayPanel catTreasureDisplayPanel;
        public static CatTreasureDisplayPanel CatTreasureDisplayPanel
        {
            get
            {
                if (null == catTreasureDisplayPanel)
                {
                    catTreasureDisplayPanel = new CatTreasureDisplayPanel(CatTreasureRectTransform);
                }
                return catTreasureDisplayPanel;
            }
        }

        // 实例化一个显示得到猫咪宝贝面板：CatTreasureGetPanel
        public static CatTreasureGetPanel catTreasureGetPanel;
        public static CatTreasureGetPanel CatTreasureGetPanel
        {
            get
            {
                if (null == catTreasureGetPanel)
                {
                    catTreasureGetPanel = new CatTreasureGetPanel(CatTreasureRectTransform);
                }
                return catTreasureGetPanel;
            }
        }

        // 实例化一个充值面板：RechargePanel
        public static RechargePanel rechargePanel;
        public static RechargePanel RechargePanel
        {
            get
            {
                if (null == rechargePanel)
                {
                    rechargePanel = new RechargePanel(RechargeRectTransform);
                }
                return rechargePanel;
            }
        }

        // 实例化一个抽奖面板：LotteryPanel
        public static LotteryPanel lotteryPanel;
        public static LotteryPanel LotteryPanel
        {
            get
            {
                if (null == lotteryPanel)
                {
                    lotteryPanel = new LotteryPanel(PanelRectTransform);  // 放到 PanelRoot 根节点下
                }
                return lotteryPanel;
            }
        }

        // 实例化聊天面板 ChatPanel
        public static ChatPanel chatPanel;
        public static ChatPanel ChatPanel
        {
            get
            {
                if (null == chatPanel)
                {
                    chatPanel = new ChatPanel(ChatRectTransform);  // 放到 ChatPanelRoot 根节点下
                }
                return chatPanel;
            }
        }

        public static LoadingPanel loadingPanel;
        public static LoadingPanel LoadingPanel
        {
            get {
                if (loadingPanel == null)
                {
                    loadingPanel = new LoadingPanel(LoadingRectTransform);
                }
                return loadingPanel;
            }
        }

        public static AwardPanel awardPanel;
        public static AwardPanel AwardPanel
        {
            get {
                if (awardPanel == null)
                {
                    awardPanel = new AwardPanel();
                }

                return awardPanel;
            }
        }

        public static ShopPanel shopPanel;
        public static ShopPanel ShopPanel
        {
            get
            {
                if (shopPanel == null)
                {
                    shopPanel = new ShopPanel();
                }
                else
                {
                    LoadSystemBar(shopPanel.transform);
                    BringSystemBarToTop();
                }

                return shopPanel;
            }
        }

        public static SystemBar SystemBar;
        public static void LoadSystemBar(RectTransform parent, Action loadOverCallback = null)
        {
            if (SystemBar == null)
            {
                string assetName = "SystemMuneBar";
                ResourceLoader.Instance.TryLoadClone(assetName.ToLower(), assetName, (go) =>
                {
                    SystemBar = new SystemBar();
                    SystemBar.InitUI(go);
                    go.transform.SetParent(parent, false);
                    BringSystemBarToTop();

                    if (loadOverCallback != null)
                    {
                        loadOverCallback();
                    }
                });
            }
            else
            {
                SystemBar.PanelPrefab.transform.SetParent(parent, false);
                BringSystemBarToTop();
            }
        }

        public static void BringSystemBarToTop()
        {
            if (SystemBar != null)
            {
                SystemBar.PanelPrefab.transform.SetAsLastSibling();
            }
        }

        public static bool IsCurrentPanel(PanelBase current)
        {
            if (current != null && CurrentPanel == current)
            {
                return true;
            }

            return false;
        }

        public static PropPopupPanel propPopupPanel;
        public static PropPopupPanel PropPopupPanel
        {
            get {
                if (propPopupPanel == null)
                {
                    //propPopupPanel = new PropPopupPanel(PopupPanelRectTransform);
                    propPopupPanel = new PropPopupPanel();
                }

                return propPopupPanel;
            }
        }

        public static bool HasPanelOpen()
        {
            if (infoBar != null && CurrentPanel!=null && CurrentPanel != infoBar)
            {
                return true;
            }

            return false;
        }

        public static HandbookPanel handbookPanel;
        public static HandbookPanel HandbookPanel
        {
            get
            {
                if (handbookPanel == null)
                {
                    handbookPanel = new HandbookPanel();
                }
                else
                {
                    LoadSystemBar(handbookPanel.transform);
                    BringSystemBarToTop();
                }

                return handbookPanel;
            }
        }

        public static CatPopupPanel catPopupPanel;
        public static CatPopupPanel CatPopupPanel
        {
            get
            {
                if (catPopupPanel == null)
                {
                    catPopupPanel = new CatPopupPanel();

                }

                return catPopupPanel;
            }
        }

        public static CatGiftPopupPanel catGiftPopupPanel;
        public static CatGiftPopupPanel CatGiftPopupPanel
        {
            get {
                if (catGiftPopupPanel == null)
                {
                    catGiftPopupPanel = new CatGiftPopupPanel();

                }

                return catGiftPopupPanel;
            }
        }

        public static TutorialPanel tutorialPanel;
        public static TutorialPanel TutorialPanel
        {
            get {
                if (tutorialPanel == null)
                {
                    tutorialPanel = new TutorialPanel();

                }

                return tutorialPanel;
            }
        }

        public static SettingPanel settingPanel;
        public static SettingPanel SettingPanel
        {
            get
            {
                if (settingPanel == null)
                {
                    settingPanel = new SettingPanel();
                }
                else
                {
                    //LoadSystemBar(settingPanel.transform);
                    //BringSystemBarToTop();
                }
                return settingPanel;
            }
        }

        public static HeadPanel headPanel;
        public static HeadPanel HeadPanel
        {
            get
            {
                if (headPanel == null)
                {
                    headPanel = new HeadPanel();
                }

                return headPanel;
            }
        }

        public static FoodPopupPanel foodPopupPanel;
        public static FoodPopupPanel FoodPopupPanel
        {
            get {
                if (foodPopupPanel == null)
                {
                    foodPopupPanel = new FoodPopupPanel();

                }

                return foodPopupPanel;
            }
        }

        public static AlertPanel alertPanel;
        public static AlertPanel AlertPanel
        {
            get {
                if (alertPanel == null)
                {
                    alertPanel = new AlertPanel();

                }

                return alertPanel;
            }
        }

        public static SceneChangePanel sceneChangePanel;
        public static SceneChangePanel SceneChangePanel
        {
            get
            {
                if (sceneChangePanel == null)
                {
                    sceneChangePanel = new SceneChangePanel();
                }
                else
                {
                    LoadSystemBar(sceneChangePanel.transform);
                    BringSystemBarToTop();
                }

                return sceneChangePanel;
            }
        }

        public static TreasurePanel treasurPanel;
        public static TreasurePanel TreasurePanel
        {
            get {
                if (treasurPanel == null)
                {
                    treasurPanel = new TreasurePanel();
                }
                else
                {
                    LoadSystemBar(treasurPanel.transform);
                    BringSystemBarToTop();
                }

                return treasurPanel;
            }
        }

        public static TakePicturePanel takePicturePanel;
        public static TakePicturePanel TakePicturePanel
        {
            get
            {
                if (takePicturePanel == null)
                {
                    takePicturePanel = new TakePicturePanel();

                }
                return takePicturePanel;
            }
        }

        public static PicturesPanel picturesPanel;
        public static PicturesPanel PicturesPanel
        {
            get {
                if (picturesPanel == null)
                {
                    picturesPanel = new PicturesPanel();

                }
                else
                {
                    LoadSystemBar(picturesPanel.transform);
                    BringSystemBarToTop();
                }

                return picturesPanel;
            }
        }

        public static PicturePreviewPanel picturePreviewPanel;
        public static PicturePreviewPanel PicturePreviewPanel
        {
            get
            {
                if (picturePreviewPanel == null)
                {
                    picturePreviewPanel = new PicturePreviewPanel();
                }
                else
                {
                    LoadSystemBar(picturePreviewPanel.transform);
                    BringSystemBarToTop();
                }

                return picturePreviewPanel;
            }
        }

        public static ScenePhotoPanel scenePhotoPanel;
        public static ScenePhotoPanel ScenePhotoPanel
        {
            get
            {
                if (scenePhotoPanel == null)
                {
                    scenePhotoPanel = new ScenePhotoPanel();

                }
                else
                {
                    LoadSystemBar(scenePhotoPanel.transform);
                    BringSystemBarToTop();
                }

                return scenePhotoPanel;
            }
        }

        public static ScenePhotoPreviewPanel scenePhotoPreviewPanel;
        public static ScenePhotoPreviewPanel ScenePhotoPreviewPanel
        {
            get
            {
                if (scenePhotoPreviewPanel == null)
                {
                    scenePhotoPreviewPanel = new ScenePhotoPreviewPanel();

                }

                return scenePhotoPreviewPanel;
            }
        }

        public static GuidePanel guidePanel;
        public static GuidePanel GuidePanel
        {
            get
            {
                if (guidePanel == null)
                {
                    guidePanel = new GuidePanel();

                }

                return guidePanel;
            }
        }

        public static CreateCharacterPanel createChatacterPanel;
        public static CreateCharacterPanel CreateCharacterPanel
        {
            get
            {
                if (createChatacterPanel == null)
                {
                    createChatacterPanel = new CreateCharacterPanel();

                }

                return createChatacterPanel;
            }
        }

        public static GuideMaskPanel guideMaskPanel;
        public static GuideMaskPanel GuideMaskPanel
        {
            get
            {
                if (guideMaskPanel == null)
                {
                    guideMaskPanel = new GuideMaskPanel();

                }

                return guideMaskPanel;
            }
        }

        public static DirectConnectPanel directConnectPanel;
        public static DirectConnectPanel DirectConnectPanel
        {
            get
            {
                if (directConnectPanel == null)
                {
                    directConnectPanel = new DirectConnectPanel();

                }

                return directConnectPanel;
            }
        }

        public static GameObject CreateCharacterScenePanelPrefab;
        public static void HideCreateCharacterScenePrefab()
        {
            if (CreateCharacterScenePanelPrefab != null)
            {
                CreateCharacterScenePanelPrefab.SetActive(false);
            }
        }
        public static void LoadCreateCharacterScenePrefab(Action callback) { 
            string bundleName = "CreateCharacterScenePanel";
            ResourceLoader.Instance.TryLoadClone(bundleName.ToLower(), bundleName, (go) =>
            {
                CreateCharacterScenePanelPrefab = go;

                go.transform.SetParent(GameMain.UI2DRoot.transform, false);
                go.transform.SetAsFirstSibling();

                if (callback != null)
                {
                    callback();
                }
            });
        }

        public static AdvisePanel advisePanel;
        public static AdvisePanel AdivisPanel
        {
            get {
                if (advisePanel == null)
                {
                    advisePanel = new AdvisePanel();

                }

                return advisePanel;
            }
        }

        public static MailPanel mailPanel;
        public static MailPanel MailPanel
        {
            get {
                if (mailPanel == null)
                {
                    mailPanel = new MailPanel();

                }

                return mailPanel;
            }
        }

        public static MailContentPanel mailContentPanel;
        public static MailContentPanel MailContentPanel
        {
            get {
                if (mailContentPanel == null)
                {
                    mailContentPanel = new MailContentPanel();

                }

                return mailContentPanel;
            }
        }

        public static TaskPanel taskPanel;
        public static TaskPanel TaskPanel
        {
            get
            {
                if (taskPanel == null)
                {
                    taskPanel = new TaskPanel();

                }
                else
                {
                    LoadSystemBar(taskPanel.transform);
                    BringSystemBarToTop();
                }

                return taskPanel;
            }
        }

        public static SignPanel signPanel;
        public static SignPanel SignPanel
        {
            get
            {
                if (signPanel == null)
                {
                    signPanel = new SignPanel();

                }

                return signPanel;
            }
        }

        public static SharePanel sharePanel;
        public static SharePanel SharePanel
        {
            get {
                if (sharePanel == null)
                {
                    sharePanel = new SharePanel();
                }

                return sharePanel;
            }
        }

        public static PictureSharePanel pictureSharePanel;
        public static PictureSharePanel PictureSharePanel
        {
            get
            {
                if (pictureSharePanel == null)
                {
                    pictureSharePanel = new PictureSharePanel();

                }

                return pictureSharePanel;
            }
        }

        public static ExchangePanel exchangePanel;
        public static ExchangePanel ExchangePanel
        {
            get
            {
                if (exchangePanel == null)
                {
                    exchangePanel = new ExchangePanel();

                }

                return exchangePanel;
            }
        }

        public static HeadSelectPanel headSelectPanel;
        public static HeadSelectPanel HeadSelectPanel
        {
            get
            {
                if (headSelectPanel == null)
                {
                    headSelectPanel = new HeadSelectPanel();
                }

                return headSelectPanel;
            }
        }

        public static void ShowTipString(string tipString, EnumConfig.PropPopupPanelBtnModde mode, string leftString="确定", Action leftCallback=null, string rightString="取消", Action rightCallback=null, string middleString="确定", Action middleCallback=null)
        {
            PanelManager.AlertPanel.Show(() => {
                PropPopupModeStruct modeStruct = new PropPopupModeStruct();

                modeStruct.Mode = mode;

                modeStruct.LeftBtnString = leftString;
                modeStruct.LeftCallback = () => {
                    PanelManager.alertPanel.Close();

                    if (leftCallback != null)
                    {
                        leftCallback();
                    }
                };

                modeStruct.RightBtnString = rightString;
                modeStruct.RightCallback = () => {
                    PanelManager.alertPanel.Close();

                    if (rightCallback != null)
                    {
                        rightCallback();
                    }
                };

                modeStruct.MiddleBtnString = middleString;
                modeStruct.MiddleCallback = () => {
                    PanelManager.alertPanel.Close();

                    if (middleCallback != null)
                    {
                        middleCallback();
                    }
                };

                PanelManager.alertPanel.SetData(tipString, modeStruct);
            });
        }

        // 显示鱼干不足的单确定按钮弹窗
        public static void ShowNoMoneyTip()
        {
            PanelManager.ShowTipString("鱼干不足", EnumConfig.PropPopupPanelBtnModde.OneBtn);
        }
    }
}
