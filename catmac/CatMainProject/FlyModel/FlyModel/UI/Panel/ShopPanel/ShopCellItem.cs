using FlyModel.UI.Component.PageView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using FlyModel.Model.Data;
using LitJson;
using FlyModel.Model;
using FlyModel.Proto;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using FlyModel.Utils;

namespace FlyModel.UI.Panel.ShopPanel
{
    //public class ShopCellItem : MonoBehaviour, IItem, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    public class ShopCellItem:IItem //: MonoBehaviour, IItem, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public int PageIndex;
        public int ItemIndex;
        public ShopItemData Data;

        private GameObject selectedPic;
        private Image propImage;
        private GameObject silverPriceGo;
        private GameObject goldPriceGO;
        private GameObject currentPriceGO;
        private GameObject sizeGO;
        private Text nameTF;
        private Image hasBuyImage;

        private float effectY = 50;

        private bool isSelected;

        public GameObject GameObject;

        public ShopCellItem(GameObject go)
        {
            GameObject = go;

            selectedPic = go.transform.Find("PropContent/Selected").gameObject;
            selectedPic.SetActive(false);

            propImage = go.transform.Find("PropContent/Prop").GetComponent<Image>();
            propImage.transform.gameObject.SetActive(false);
            //Debug.Log(propImage);

            hasBuyImage = go.transform.Find("PropContent/HasBuy").GetComponent<Image>();
            hasBuyImage.transform.gameObject.SetActive(false);

            silverPriceGo = go.transform.Find("SilverPrice").gameObject;
            silverPriceGo.SetActive(false);
            goldPriceGO = go.transform.Find("GoldPrice").gameObject;
            goldPriceGO.SetActive(false);

            sizeGO = go.transform.Find("Size").gameObject;

            nameTF = go.transform.Find("Name").GetComponent<Text>();
        }

        public void Init(BaseProp data, int pageIndex, int itemIndex)
        {
            PageIndex = pageIndex;
            ItemIndex = itemIndex;

            Data = data as ShopItemData;

            if (string.IsNullOrEmpty(Data.PicCode) == false)
            {
                ResourceLoader.Instance.TryLoadPic(ResPathConfig.ITEM_ASSETBUNDLE, Data.PicCode, (texture) =>
                {
                    if (propImage != null)
                    {
                        propImage.sprite = texture as Sprite;
                        propImage.SetNativeSize();
                        propImage.transform.gameObject.SetActive(true);
                    }
                });
            }

            nameTF.text = Data.Name;

            setCurrenyType((int)Data.CurrencyType, Data.Price.ToString());
            setCurrencyBgDir();

            setSizeIcon((int)Data.Size);

            setState();
        }

        public void UpdteData(ShopItemData data)
        {
            Data = data;

            setState();
        }

        private void setState()
        {
            bool canBuy = Data.CanShowInStore();

            currentPriceGO.transform.Find("Text").gameObject.SetActive(canBuy);

            Color c;
            if (canBuy)
            {
                c = Color.white;
            }
            else
            {
                c = ColorConfig.SHOP_MASK;
            }
            propImage.color = c;

            hasBuyImage.gameObject.SetActive(!canBuy);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isSelected == false)
            {
                if (GuideManager.Instance.IsGestureTouchEffective("ShopCellItem"))
                {
                    GuideManager.Instance.ContinueGuide();
                }

                if (Data.CanShowInStore())
                {
                    SoundUtil.PlaySound(ResPathConfig.SHOP_SELETE);
                    PanelManager.shopPanel.ShowItemSelected(PageIndex, ItemIndex);
                }
            }
            else
            {
                if (Data.CanBuy()==false)
                {
                    return;
                }

                SoundUtil.PlaySound(ResPathConfig.SHOP_POPUPPANEL);
                PanelManager.PropPopupPanel.Show(() => {
                    if (GuideManager.Instance.IsGestureTouchEffective("ShopCellItem"))
                    {
                        GuideManager.Instance.ContinueGuide();
                    }

                    string leftStr = "购买";
                    string rightStr = "立即使用";
                    string middleStr = "购买";
                    EnumConfig.PropPopupPanelBtnModde btnMode = EnumConfig.PropPopupPanelBtnModde.TwoBtb;
                    Action LeftCallBack = null;
                    Action MiddleCallbcak = null;
                    Action RightCallbcak = null;
                    
                    switch (Data.ItemSubType)
                    {
                        case EnumConfig.ShopItemType.Food:
                            LeftCallBack = () => {
                                if (Data.CanBuy())
                                {
                                    User me = UserManager.Instance.Me;
                                    int money = Data.CurrencyType == Currency.Coin ? (int)me.LowCurrency : (int)me.HighCurrency;
                                    if (money >= Data.Price)
                                    {
                                        CommandHandle.Send(Proto.ServerMethod.BuyShop, new BuyShopData() { Id = Data.Type, Count = 1, Imm = false });
                                        // 显示购买成功提示
                                        PanelManager.TipPanel.Show(() =>
                                            PanelManager.tipPanel.SetText(string.Format("购买了{0}", nameTF.text))
                                        );
                                        PanelManager.propPopupPanel.Close();                                   
                                    }
                                    else
                                    {
                                        PanelManager.ShowNoMoneyTip();
                                    }
                                }
                                else
                                {
                                    Debug.Log("背包已有该物品");
                                }
                            };

                            RightCallbcak = () => {
                                User me = UserManager.Instance.Me;
                                int money = Data.CurrencyType == Currency.Coin ? (int)me.LowCurrency : (int)me.HighCurrency;
                                if (money >= Data.Price)
                                {
                                    CommandHandle.Send(Proto.ServerMethod.BuyShop, new BuyShopData() { Id = Data.Type, Count = 1, Imm = true });
                                    PanelManager.propPopupPanel.Close();
                                }
                                else
                                {
                                    PanelManager.ShowNoMoneyTip();
                                }
                            };
                            break;
                        case EnumConfig.ShopItemType.Item:
                            btnMode = EnumConfig.PropPopupPanelBtnModde.OneBtn;

                            middleStr = "购买";

                            MiddleCallbcak = () =>
                            {
                                if (Data.CanBuy())
                                {
                                    User me = UserManager.Instance.Me;
                                    int money = Data.CurrencyType == Currency.Coin ? (int)me.LowCurrency : (int)me.HighCurrency;
                                    if (money >= Data.Price)
                                    {
                                        CommandHandle.Send(Proto.ServerMethod.BuyShop, new BuyShopData() { Id = Data.Type, Count = 1, Imm = false });
                                        PanelManager.propPopupPanel.Close();
                                    }
                                    else
                                    {
                                        PanelManager.ShowNoMoneyTip();
                                    }
                                }
                            };
                            break;
                        case EnumConfig.ShopItemType.Currency:
                            break;
                        case EnumConfig.ShopItemType.Toy:
                            LeftCallBack = () => {
                                if (Data.CanBuy())
                                {
                                    User me = UserManager.Instance.Me;
                                    int money = Data.CurrencyType == Currency.Coin ? (int)me.LowCurrency : (int)me.HighCurrency;
                                    if (money >= Data.Price)
                                    {
                                        if (GuideManager.Instance.IsGestureTouchEffective("ButtonGroup/Left"))
                                        {
                                            GuideManager.Instance.ContinueGuide();
                                        }
                                        CommandHandle.Send(Proto.ServerMethod.BuyShop, new BuyShopData() { Id = Data.Type, Count = 1, Imm = false });
                                        PanelManager.propPopupPanel.Close();
                                    }
                                    else
                                    {
                                        PanelManager.ShowNoMoneyTip();
                                    }
                                }
                                else
                                {
                                    Debug.Log("背包已有该物品");
                                }
                            };

                            RightCallbcak = () => {
                                User me = UserManager.Instance.Me;
                                int money = Data.CurrencyType == Currency.Coin ? (int)me.LowCurrency : (int)me.HighCurrency;
                                if (money >= Data.Price)
                                {
                                    CommandHandle.Send(Proto.ServerMethod.BuyShop, new BuyShopData() { Id = Data.Type, Count = 1, Imm = true });
                                    PanelManager.propPopupPanel.Close();
                                }
                                else
                                {
                                    PanelManager.ShowNoMoneyTip();
                                }
                            };
                            break;
                        default:
                            break;
                    }

                    PropPopupModeStruct modeStruct = new PropPopupModeStruct();

                    modeStruct.Mode = btnMode;

                    modeStruct.LeftBtnString = leftStr;
                    modeStruct.LeftCallback = LeftCallBack;
                    modeStruct.MiddleBtnString = middleStr;

                    modeStruct.RightBtnString = rightStr;
                    modeStruct.RightCallback = RightCallbcak;
                    modeStruct.MiddleCallback = MiddleCallbcak;

                    modeStruct.LeftSound = ResPathConfig.PROP_POPUP_PANEL_BUTTON;
                    modeStruct.RightSound = ResPathConfig.PROP_POPUP_PANEL_BUTTON;
                    modeStruct.MiddleSound = ResPathConfig.PROP_POPUP_PANEL_BUTTON;

                    Debug.Log(string.Format("数据Data的ID：{0}，Name：{1}，Type：{2}，Count：{3}，Description：{4}，PicCode：{5},Price：{6},AppendExplaination：{7}",
                            Data.ID, Data.Name, Data.Type, Data.Count, Data.Description, Data.PicCode, Data.Price, Data.AppendExplaination));
                    PanelManager.propPopupPanel.SetData(Data, modeStruct);
                });
                
            }
        }

        public void ShowSelectedState(bool isShow, bool autoShowBuy)
        {
            selectedPic.SetActive(isShow);

            isSelected = isShow;

            showEffect(isSelected);

            if (isShow)
            {
                UpdateDescription();
            }

            if (autoShowBuy)
            {
                OnPointerClick(null);
            }
        }

        private void showEffect(bool isSelected)
        {
            Tweener t = propImage.transform.DOLocalMoveY(isSelected ? effectY : 0, 0.2f);
        }

        private void setCurrenyType(int type, string price)
        {
            if (type == (int)Currency.Coin)
            {
                silverPriceGo.SetActive(true);
                goldPriceGO.SetActive(false);
                currentPriceGO = silverPriceGo;
            }
            else if (type == (int)Currency.Dollar)
            {
                silverPriceGo.SetActive(false);
                goldPriceGO.SetActive(true);
                currentPriceGO = goldPriceGO;
            }
            else if(type == (int)Currency.None)
            {
                
            }
            else
            {
                Debug.LogWarning(string.Format("{0} 的Currency字段未设置", Data.Name));
            }

            Text priceTF = currentPriceGO.transform.Find("Text").GetComponent<Text>();
            priceTF.text = price;
        }

        private void setCurrencyBgDir()
        {
            int pos = ItemIndex % 3;
            if (currentPriceGO)
            {
                GameObject left = currentPriceGO.transform.Find("bg_1").gameObject;
                GameObject middle = currentPriceGO.transform.Find("bg_2").gameObject;
                GameObject right = currentPriceGO.transform.Find("bg_3").gameObject;

                left.SetActive(pos == 0);
                middle.SetActive(pos == 1);
                right.SetActive(pos == 2);
                currentPriceGO.SetActive(true);
            }
        }

        private void setSizeIcon(int size)
        {
            sizeGO.transform.Find("Small").gameObject.SetActive(size == (int)EnumConfig.ToySizeType.Small);
            sizeGO.transform.Find("Large").gameObject.SetActive(size == (int)EnumConfig.ToySizeType.Large);
        }

        public void UpdateDescription()
        {
            if (Data != null)
            {
                GameObject blackBoard= GameObject.transform.parent.parent.Find("BlcakBoard").gameObject;
                blackBoard.transform.Find("Name").GetComponent<Text>().text = Data.Name;
                blackBoard.transform.Find("Description").GetComponent<Text>().text = Data.Description;
            }
        }
    }
}
