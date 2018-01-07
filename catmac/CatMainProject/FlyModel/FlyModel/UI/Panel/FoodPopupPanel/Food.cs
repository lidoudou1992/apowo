using Assets.Scripts.TouchController;
using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI.Component;
using FlyModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.FoodPopupPanel
{
    //public class Food:MonoBehaviour
    public class Food
    {
        private Text nameTF;
        private Image image;
        private Text currentTF;

        private ShopItemData Data;
        private BagItemData bagItemData;

        public GameObject GameObject;

        public Food(GameObject go)
        {
            GameObject = go;

            nameTF = go.transform.Find("Name").GetComponent<Text>();
            image = go.transform.Find("Image").GetComponent<Image>();
            currentTF = go.transform.Find("Current").GetComponent<Text>();

            PointerSensor sensor = GameObject.transform.gameObject.AddComponent<PointerSensor>();
            sensor.OnPointerClickHandler = clickedHandler;
        }

        //void Awake()
        //{
        //    nameTF = transform.Find("Name").GetComponent<Text>();
        //    image = transform.Find("Image").GetComponent<Image>();
        //    currentTF = transform.Find("Current").GetComponent<Text>();

        //    ClickSensor sensor = transform.gameObject.AddComponent<ClickSensor>();
        //    sensor.onClickedCallback = clickedHandler;
        //}

        private void clickedHandler(PointerEventData eventData)
        {
            SoundUtil.PlaySound(ResPathConfig.COMMON_BUTTON);

            if (bagItemData == null)
            {
                //PanelManager.ShopPanel.Show(() =>
                //{
                //    PanelManager.shopPanel.ShowItemSelected(0, ShopManager.Instance.GetFoodIndex(Data.Type), true);
                //});

                SoundUtil.PlaySound(ResPathConfig.SHOP_POPUPPANEL);
                PanelManager.PropPopupPanel.Show(() =>
                {
                    Action MiddleCallbcak = () =>
                    {
                        User me = UserManager.Instance.Me;
                        int money = Data.CurrencyType == Currency.Coin ? (int)me.LowCurrency : (int)me.HighCurrency;
                        Debug.Log(string.Format("数据Data的ID：{0}，Name：{1}，Type：{2}，Count：{3}，Description：{4}，PicCode：{5},Price：{6},AppendExplaination：{7}",
                            Data.ID,Data.Name,Data.Type,Data.Count,Data.Description,Data.PicCode,Data.Price,Data.AppendExplaination));

                        // 如果是点击购买高级猫粮之外的食物并且没有房屋扩张
                        if (Data.Type != 2002 && false == BagManager.Instance.HasHouseExtensionItem())  
                        {
                            // 显示单确定按钮弹窗
                            PanelManager.ShowTipString("房屋扩张之前只能购买高级猫粮～", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                        }
                        else  // 当购买的食物是高级猫粮或不是高级猫粮时已经房屋扩张
                        {                  
                            if (money >= Data.Price)  // 鱼干足够就购买食物
                            {
                                PanelManager.propPopupPanel.Close();
                                PanelManager.foodPopupPanel.SendBuyAndPlaceFoodMsg(Data);
                            }
                            else  // 鱼干不足以购买就显示鱼干不足的弹窗
                            {
                                PanelManager.ShowNoMoneyTip();
                            }
                        }
                    };

                    PropPopupModeStruct modeStruct = new PropPopupModeStruct();
                    modeStruct.Mode = EnumConfig.PropPopupPanelBtnModde.OneBtn;
                    modeStruct.MiddleBtnString = "购买";
                    modeStruct.MiddleCallback = MiddleCallbcak;
                    modeStruct.MiddleSound = ResPathConfig.PROP_POPUP_PANEL_BUTTON;

                    PanelManager.propPopupPanel.SetData(Data, modeStruct);
                });
            }
            else
            {
                PanelManager.foodPopupPanel.PlaceFood(bagItemData);

                if (GuideManager.Instance.IsGestureTouchEffective("GameObject/Container/Food1"))
                {
                    GuideManager.Instance.ContinueGuide();
                }
            }
        }

        public void SetData(Model.Data.ShopItemData data)
        {
            Data = data;

            UpdateData();

            ResourceLoader.Instance.TryLoadPic(ResPathConfig.ITEM_ASSETBUNDLE, data.PicCode, (texture) =>
            {
                image.sprite = texture as Sprite;
                image.SetNativeSize();
            });
        }

        public void UpdateData()
        {
            string count = " x0";
            bagItemData = BagManager.Instance.GetOneBagItemByType(Data.Type);
            if (bagItemData != null)
            {
                count = " x" + bagItemData.Count.ToString();
            }
            nameTF.text = Data.Name + count;

        }

        public void ShowCurrentMark(bool show)
        {
            currentTF.gameObject.SetActive(show);
        }
    }
}
