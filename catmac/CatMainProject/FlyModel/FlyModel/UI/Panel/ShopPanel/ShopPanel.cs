using Assets.Scripts.TouchController;
using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI.Component;
using FlyModel.UI.Component.PageView;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Together;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.ShopPanel
{
    public class ShopPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "ShopPanel";
            }
        }

        public override bool IsRoot
        {
            get
            {
                return true;
            }
        }

        private GameObject pageViewGameObject;
        private PageView pageViewScript;
        private GameObject pageItemPrefb;
        private GameObject itemPrefab;

        private SoundButton prevBtn;
        private SoundButton nextBtn;

        public float PageWidth = 640;
        public float PageHeight = 822;
        public float GapH = 0;
        public float ContentWidth;

        public int count;
        public int pagesCount;
        public float itemsOfPage = 6f;
        public List<ShopItemData> itemDatas;

        private RectTransform content;

        private PageMark pageMark;

        private DelayController delayController;

        protected override void Initialize(GameObject go)
        {
            CameraAbsoluteResolution cameraAbsoluteResolution = GameObject.Find("Main Camera").GetComponent<CameraAbsoluteResolution>();
            Vector2 screenSize = cameraAbsoluteResolution.GetScreenPixelDimensions();
            PageWidth = screenSize.x * 1136 / screenSize.y;

            pageViewGameObject = go.transform.Find("PageView").gameObject;
            DragSensor dragSensor = pageViewGameObject.AddComponent<DragSensor>();

            pageViewScript = new PageView();
            pageViewScript.GameObject = pageViewGameObject;
            BehaviourManager.AddGameComponent(pageViewScript);
            pageViewScript.OnPageChanged = OnPageChanged;
            pageViewScript.OnCreatePageItem = createOnePage;

            dragSensor.OnBeinDragHandler = pageViewScript.OnBeginDrag;
            dragSensor.OnDragHandler = pageViewScript.OnDrag;
            dragSensor.OnEndDragHandler = pageViewScript.OnEndDrag;

            content = pageViewGameObject.transform.Find("Content").GetComponent<RectTransform>();

            pageItemPrefb = go.transform.Find("PageView/Content/PageItem").gameObject;
            pageItemPrefb.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(PageWidth, PageHeight);


            itemPrefab = go.transform.Find("ShopCellItem").gameObject;

            prevBtn = new SoundButton(go.transform.Find("Arrows/Left").gameObject, toPrevHandler, ResPathConfig.PAGE_CHANGE_BUTTON);
            nextBtn = new SoundButton(go.transform.Find("Arrows/Right").gameObject, toNextHandler, ResPathConfig.PAGE_CHANGE_BUTTON);

            //页码控制器
            GameObject pageMarkGameObject = pageViewGameObject.transform.Find("PageMark").gameObject;
            pageMark = new PageMark();
            pageMark.GameObject = pageMarkGameObject;
            pageMark.Init(pageViewScript, go.transform.Find("PageMarkItem").gameObject);

            delayController = go.AddComponent<DelayController>();

            PanelManager.BringSystemBarToTop();
        }

        public override void Load()
        {
            base.Load();

            PanelManager.LoadSystemBar(transform, () => { PanelManager.SystemBar.SelectBtn(0); });
        }

        private void OnPageChanged(int index)
        {
            Debug.Log("page changed " + index.ToString());
        }

        public override void Refresh()
        {
            base.Refresh();

            if (PanelManager.SystemBar != null)
            {
                PanelManager.SystemBar.SelectBtn(0);
            }

            Clear();

            itemDatas = ShopManager.Instance.GetShowInStoreList();
            count = itemDatas.Count;
            pagesCount = (int)Mathf.Ceil(count / itemsOfPage);

            ContentWidth = (pagesCount - 1) * GapH + PageWidth * pagesCount;
            content.sizeDelta = new Vector2(ContentWidth, PageHeight);

            pageViewScript.PageCount = pagesCount;
            pageViewScript.InitPages();

            pageViewScript.CreatePageItem(pageMark.GetCurrentPageIndex());

            //GuideManager.Instance.ContinueGuide();//正常流程调用，要打开
            delayController.DelayInvoke(() =>
            {
                GuideManager.Instance.ContinueGuide();//正常流程调用，要打开
            }, 0.5f);

            //测试
            //GuideManager.Instance.StartGuide();
            //PanelManager.InfoBar.Show(() => { GuideManager.Instance.StartGuideManul(3); });
        }

        private void createOnePage(int pageIndex)
        {
            List<BaseProp> datas = new List<BaseProp>();
            for (int i = pageIndex * (int)itemsOfPage; i < pageIndex * itemsOfPage + itemsOfPage && i < itemDatas.Count; i++)
            {
                datas.Add(itemDatas[i]);
            }

            PageItem pageItem = CreatePageItem(pageIndex);
            pageItem.InitItems(datas, pageIndex);
        }

        private void createShopCellItem(BaseProp data, PageItem pageItem, int itemIndex)
        {
            GameObject ItemPrefabInstance = GameObject.Instantiate(itemPrefab);
            ItemPrefabInstance.SetActive(true);
            ItemPrefabInstance.transform.SetParent(pageItem.GameObject.transform.Find("Panel"), false);

            PointerSensor pointerSensor = ItemPrefabInstance.AddComponent<PointerSensor>();

            ShopCellItem cellItem = new ShopCellItem(ItemPrefabInstance);
            cellItem.Init(data, pageViewScript.TempPageIndex, itemIndex);

            pointerSensor.OnPointerClickHandler = cellItem.OnPointerClick;

            pageViewScript.pageItems[pageViewScript.TempPageIndex].listItem.Add(cellItem);
        }

        private PageItem CreatePageItem(int pageIndex)
        {
            GameObject pageItemPrefabInstance = UnityEngine.Object.Instantiate(pageItemPrefb);
            pageItemPrefabInstance.SetActive(true);
            pageItemPrefabInstance.transform.localPosition = new Vector3((GapH + PageWidth) * pageIndex, 0, 0);
            pageItemPrefabInstance.transform.SetParent(pageItemPrefb.transform.parent, false);

            //// 打开充值面板的按钮
            //// 先注释掉使用南北极链接来代替
            //new SoundButton(pageItemPrefabInstance.transform.Find("PayBtn").gameObject, () =>
            //{
            //    // 实例化充值面板
            //    PanelManager.RechargePanel.Show();

            //    CommandHandle.Send(Proto.ServerMethod.BuyCurrency, new IDCountMessage() { Id = 6, Count = 1 });
            //});
            //// 南北极链接
            //new SoundButton(pageItemPrefabInstance.transform.Find("ButtonLink").gameObject, () =>
            //{
            //    Debug.Log("开始支付");
            //    // Pay Demo
            //    GSDKUnityLib.Pay.PayInfo payInfo = new GSDKUnityLib.Pay.PayInfo();
            //    payInfo.PropName = "2个金鱼干";
            //    payInfo.PropID = "1";  // ID:1 1元 2个金鱼干
            //    payInfo.PropDesc = "获得2个金鱼干";
            //    payInfo.ServerID = "2";   // 2 内部测试
            //    payInfo.UserID = UserManager.Instance.Me.ID.ToString();
            //    payInfo.PriceInCurrency = 100;     // 分
            //    String oid = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
            //    oid += new System.Random().Next(1000000).ToString();
            //    payInfo.CallbackExtraInfo = oid;
            //    GSDKUnityLib.GSDK.Instance.StartPayAsync(payInfo, payResultInfo =>
            //    {
            //        if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.Succeed)
            //        {
            //            Debug.Log("支付成功");
            //        }
            //        else if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.Cancelled)
            //        {
            //            Debug.Log("支付取消");

            //        }
            //        else if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.FailedToGetOrder)
            //        {
            //            Debug.Log("支付失败 - GSDK获取订单失败 Info:" + payResultInfo.InfoStr + " InternalErrorMsg:" + payResultInfo.InternalErrorStr);
            //        }
            //        else if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.Failed || payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.InternalError)
            //        {
            //            Debug.Log("支付失败 - Info:" + payResultInfo.InfoStr + " InternalErrorMsg:" + payResultInfo.InternalErrorStr);
            //        }
            //        else {
            //            Debug.Log("支付失败 无法识别的状态码: " + payResultInfo.Status + " Info:" + payResultInfo.InfoStr + " InternalErrorMsg:" + payResultInfo.InternalErrorStr);
            //        }
            //    });
            //    //Debug.Log("打开南北极链接");
            //    //Application.OpenURL("https://itunes.apple.com/cn/app/qq/id1105825658");
            //    Debug.Log("点击事件结束");
            //});
            //pageItemPrefabInstance.transform.Find("ButtonLink").GetComponent<Button>().onClick.AddListener(OnClickLinkBtn);
            //pageItemPrefabInstance.transform.Find("ButtonLink").GetComponent<Button>().onClick.AddListener(PlayAd);
            pageItemPrefabInstance.transform.Find("PayBtn").GetComponent<Button>().onClick.AddListener(OpenRechargeInter);

            PageItem pi = new PageItem(pageItemPrefabInstance);
            pi.OnCreatCellItemCallback = createShopCellItem;

            pageViewScript.pageItems[pageIndex] = pi;
            return pi;
        }

        // 打开充值界面interface
        private void OpenRechargeInter()
        {
            PanelManager.RechargePanel.Show();
            // 取得充值界面的数据 对应GetShopItemDatasResult
            CommandHandle.Send(Proto.ServerMethod.GetShopItemDatas, null);

            //switch (Application.platform)
            //{
            //    case RuntimePlatform.IPhonePlayer:
            //        {
            //            // 猫宅日记苹果版场景1的场景ID
            //            if (TGSDK.CouldShowAd("NupYGtzaCG4EPVg0ISv"))
            //            {
            //                Debug.Log("播放广告");
            //                TGSDK.ShowAd("NupYGtzaCG4EPVg0ISv");
            //                // 奖励类广告达成领奖条件可以发放奖励的回调
            //                TGSDK.AdRewardSuccessCallback = (string ret) =>
            //                {
            //                    Debug.Log("发放奖励");
            //                };
            //            }
            //            else
            //            {
            //                Debug.Log("广告没加载好");
            //            }
            //            break;
            //        }
            //    case RuntimePlatform.Android:
            //        {
            //            // 猫宅日记安卓版场景1的场景ID
            //            if (TGSDK.CouldShowAd("MC3Xep301kSt5QBCyIv"))
            //            {
            //                Debug.Log("播放广告");
            //                TGSDK.ShowAd("MC3Xep301kSt5QBCyIv");
            //                // 奖励类广告达成领奖条件可以发放奖励的回调
            //                TGSDK.AdRewardSuccessCallback = (string ret) =>
            //                {
            //                    Debug.Log("发放奖励");
            //                };
            //            }
            //            else
            //            {
            //                Debug.Log("广告没加载好");
            //            }
            //            break;
            //        }
            //}
        }

        // 播放广告
        private void PlayAd()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    {
                        // 猫宅日记苹果版场景1的场景ID
                        if (TGSDK.CouldShowAd("NupYGtzaCG4EPVg0ISv"))
                        {
                            Debug.Log("播放广告");
                            TGSDK.ShowAd("NupYGtzaCG4EPVg0ISv");
                            // 奖励类广告达成领奖条件可以发放奖励的回调
                            TGSDK.AdRewardSuccessCallback = (string ret) =>
                            {
                                Debug.Log("发放奖励");
                            };
                        }
                        else
                        {
                            Debug.Log("广告没加载好");
                        }
                        break;
                    }
                case RuntimePlatform.Android:
                    {
                        // 猫宅日记安卓版场景1的场景ID
                        if (TGSDK.CouldShowAd("MC3Xep301kSt5QBCyIv"))
                        {
                            Debug.Log("播放广告");
                            TGSDK.ShowAd("MC3Xep301kSt5QBCyIv");
                            // 奖励类广告达成领奖条件可以发放奖励的回调
                            TGSDK.AdRewardSuccessCallback = (string ret) =>
                            {
                                Debug.Log("发放奖励");
                            };
                        }
                        else
                        {
                            Debug.Log("广告没加载好");
                        }
                        break;
                    }
            }
        }

        // 
        private void OnClickLinkBtn()
        {
            Debug.Log("开始支付");

            // Pay Demo
            GSDKUnityLib.Pay.PayInfo payInfo = new GSDKUnityLib.Pay.PayInfo();
            payInfo.PropName = "2个金鱼干";
            payInfo.PropID = "1";  // ID:1 1元 2个金鱼干
            payInfo.PropDesc = "获得2个金鱼干";
            payInfo.ServerID = "2";   // 2 内部测试
            payInfo.UserID = UserManager.Instance.Me.ID.ToString();
            payInfo.PriceInCurrency = 100;     // 分
            String oid = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
            oid += new System.Random().Next(1000000).ToString();
            payInfo.CallbackExtraInfo = oid;
            GSDKUnityLib.GSDK.Instance.StartPayAsync(payInfo, payResultInfo =>
            {
                if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.Succeed)
                {
                    Debug.Log("支付成功");
                }
                else if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.Cancelled)
                {
                    Debug.Log("支付取消");

                }
                else if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.FailedToGetOrder)
                {
                    Debug.Log("支付失败 - GSDK获取订单失败 Info:" + payResultInfo.InfoStr + " InternalErrorMsg:" + payResultInfo.InternalErrorStr);
                }
                else if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.Failed || payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.InternalError)
                {
                    Debug.Log("支付失败 - Info:" + payResultInfo.InfoStr + " InternalErrorMsg:" + payResultInfo.InternalErrorStr);
                }
                else {
                    Debug.Log("支付失败 无法识别的状态码: " + payResultInfo.Status + " Info:" + payResultInfo.InfoStr + " InternalErrorMsg:" + payResultInfo.InternalErrorStr);
                }
            });

            //Application.OpenURL("https://itunes.apple.com/cn/app/qq/id1105825658");
            Debug.Log("点击事件结束");
        }

        public void Clear()
        {
            pageViewScript.ClearPageItems();

            pageViewScript.InitOver = false;
        }

        public override void Dispose()
        {
            base.Dispose();
            Clear();
        }

        public override void SetInfoBar()
        {
            base.SetInfoBar();
            PanelManager.InfoBar.SetMenuClickedHandler(() => { Close(); });
            PanelManager.InfoBar.SetBtnImage(EnumConfig.InfoBarBtnMode.Close);
        }

        private Vector2 getPageIndexAndItemIndex(int dataIndex)
        {
            Vector2 temp = new Vector2();

            temp.x = (int)Mathf.Floor(dataIndex / itemsOfPage);
            temp.y = dataIndex - temp.x * (int)itemsOfPage;

            return temp;
        }

        public void toPrevHandler()
        {
            pageViewScript.ToPrevPage();
        }

        public void toNextHandler()
        {
            pageViewScript.ToNextPage();

            delayController.DelayInvoke(() =>
            {
                if (GuideManager.Instance.IsGestureTouchEffective("Arrows/Right"))
                {
                    GuideManager.Instance.ContinueGuide();
                }
            }, 0.8f);

        }

        public void ShowItemSelected(int pageIndex, int itemIndex, bool autoShowBuy = false)
        {
            ShopCellItem tempCell;
            PageItem tempPage = null;
            List<IItem> items = null;
            for (int i = 0; i < pageViewScript.PageCount; i++)
            {
                tempPage = pageViewScript.GetPageItem(i);
                if (tempPage != null)
                {
                    items = tempPage.listItem;
                    for (int j = 0; j < items.Count; j++)
                    {
                        tempCell = items[j] as ShopCellItem;
                        tempCell.ShowSelectedState(i == pageIndex && j == itemIndex, autoShowBuy);
                    }
                }
            }
        }

        public void AutoSelectOneItem(int pageIdnex, int itemIndex)
        {
            ShowItemSelected(pageIdnex, itemIndex);
        }

        public void UpdateData(ShopItemData data)
        {
            ShopCellItem temp;
            PageItem pageItem = pageViewScript.GetCurrentPageItem();
            for (int i = 0; i < pageItem.listItem.Count; i++)
            {
                temp = pageItem.listItem[i] as ShopCellItem;
                if (temp.Data.Type == data.Type)
                {
                    temp.UpdteData(data);
                }
            }
        }

        public void ShowArrowBtns(bool isShow)
        {
            prevBtn.SetActive(isShow);
            nextBtn.SetActive(isShow);
        }

        public ShopCellItem FindGuidePropCom(int type)
        {
            List<IItem> list = pageViewScript.GetCurrentPageItem().listItem;
            ShopCellItem temp;
            for (int i = 0; i < list.Count; i++)
            {
                temp = list[i] as ShopCellItem;
                if (temp.Data.Type == type)
                {
                    return temp;
                }
            }

            return null;
        }
    }
}
