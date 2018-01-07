using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI.Component;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FlyModel.UI.Panel
{
    public class PropPopupPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "PropPopup";
            }
        }

        private PropPopupModeStruct modeStruct;
        private BaseProp data;

        private Text propNameTF;
        private Text propDesTF;
        private Image PropImage;
        private Image priceIcon;
        private Text priceTF;

        private SoundButton leftBtn;
        private SoundButton rightBtn;
        private SoundButton middleBtn;

        private bool isDataPrepared;

        private Text additionTF;

        //public PropPopupPanel(RectTransform parent) : base(parent)
        //{

        //}

        public void SetData(BaseProp config, PropPopupModeStruct modeStruct)
        {
            this.modeStruct = modeStruct;
            this.data = config;
            isDataPrepared = true;

            setLeftBtnSound(modeStruct.LeftSound);
            setRightBtnSound(modeStruct.RightSound);
            setMiddleBtnSound(modeStruct.MiddleSound);

            updateData();
        }

        protected override void Initialize(GameObject go)
        {
            propNameTF = go.transform.Find("PropInfo/Name").GetComponent<Text>();
            propDesTF = go.transform.Find("PropInfo/Des").GetComponent<Text>();
            PropImage = go.transform.Find("PropInfo/Image").GetComponent<Image>();
            PropImage.gameObject.SetActive(false);
            priceIcon = go.transform.Find("PropInfo/PriceGroup/Icon").GetComponent<Image>();
            priceIcon.gameObject.SetActive(false);
            priceTF = go.transform.Find("PropInfo/PriceGroup/Price").GetComponent<Text>();

            additionTF = go.transform.Find("PropInfo/Text").GetComponent<Text>();

            leftBtn = new SoundButton(go.transform.Find("ButtonGroup/Left").gameObject);
            leftBtn.SetActive(false);
            rightBtn = new SoundButton(go.transform.Find("ButtonGroup/Right").gameObject);
            rightBtn.SetActive(false);
            middleBtn = new SoundButton(go.transform.Find("ButtonGroup/Middle").gameObject);
            middleBtn.SetActive(false);
        }

        public override void Refresh()
        {
            base.Refresh();

            PanelPrefab.SetActive(false);

            if (PanelManager.shopPanel != null)
            {
                PanelManager.shopPanel.ShowArrowBtns(false);
            }

            if (PanelManager.bagPanel != null)
            {
                PanelManager.bagPanel.ShowArrowBtns(false);
            }

            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.ShowMenuBtn(false);
            }
        }

        public override void SetInfoBar()
        {
            
        }

        private void updateData()
        {
            if (isDataPrepared)
            {
                propNameTF.text = data.Name;
                propDesTF.text = data.Description;
                additionTF.text = data.AppendExplaination;

                PanelPrefab.transform.Find("PropInfo/PriceGroup").gameObject.SetActive(data.GetType() != typeof(BagItemData));

                ResourceLoader.Instance.TryLoadPic(ResPathConfig.ITEM_ASSETBUNDLE, data.PicCode, (texture) =>
                {
                    PropImage.sprite = texture as Sprite;
                    PropImage.SetNativeSize();
                    PropImage.gameObject.SetActive(true);
                });

                string currencyType = data.CurrencyType == Currency.Coin ? ResPathConfig.ICON_SILVER_FISH : ResPathConfig.ICON_GOLD_FISH;
                ResourceLoader.Instance.TryLoadPic(ResPathConfig.ICON_ASSETBUNDLE, currencyType, (texture) =>
                {
                    priceIcon.sprite = texture as Sprite;
                    priceIcon.SetNativeSize();
                    priceIcon.gameObject.SetActive(true);
                });

                priceTF.text = data.Price.ToString();

                //设置按钮
                if (modeStruct.Mode == EnumConfig.PropPopupPanelBtnModde.TwoBtb)
                {
                    leftBtn.GameObject.transform.Find("Text").GetComponent<Text>().text = modeStruct.LeftBtnString;
                    rightBtn.GameObject.transform.Find("Text").GetComponent<Text>().text = modeStruct.RightBtnString;

                    leftBtn.SetCallback(modeStruct.LeftCallback);
                    rightBtn.SetCallback(modeStruct.RightCallback);

                    leftBtn.SetActive(true);
                    rightBtn.SetActive(true);
                    middleBtn.SetActive(false);
                }
                else if (modeStruct.Mode ==  EnumConfig.PropPopupPanelBtnModde.OneBtn)
                {
                    middleBtn.GameObject.transform.Find("Text").GetComponent<Text>().text = modeStruct.MiddleBtnString;
                    if (modeStruct.MiddleCallback == null)
                    {
                        modeStruct.MiddleCallback = () => { PanelManager.CurrentPanel.Close(); };
                    }
                    middleBtn.SetCallback(modeStruct.MiddleCallback);

                    leftBtn.SetActive(false);
                    rightBtn.SetActive(false);
                    middleBtn.SetActive(true);
                }

                PanelPrefab.SetActive(true);
            }
        }

        public override void Close(bool isCloseAllMode = false)
        {
            isDataPrepared = false;
            modeStruct = null;
            leftBtn.RemoveListener();
            rightBtn.RemoveListener();
            middleBtn.RemoveListener();

            if (PanelManager.shopPanel != null)
            {
                PanelManager.shopPanel.ShowArrowBtns(true);
            }

            if (PanelManager.bagPanel != null)
            {
                PanelManager.bagPanel.ShowArrowBtns(true);
            }

            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.ShowMenuBtn(true);
            }

            base.Close(isCloseAllMode);
        }

        private void setLeftBtnSound(string sound)
        {
            leftBtn.SetSound(sound);
        }

        private void setMiddleBtnSound(string sound)
        {
            middleBtn.SetSound(sound);
        }

        private void setRightBtnSound(string sound)
        {
            rightBtn.SetSound(sound);
        }
    }

    public class PropPopupModeStruct
    {
        public EnumConfig.PropPopupPanelBtnModde Mode;
        public string LeftBtnString = "取消";
        public string RightBtnString = "确定";
        public string MiddleBtnString = "确定";

        public string LeftSound = ResPathConfig.MAIN_MENU;
        public string RightSound = ResPathConfig.MAIN_MENU;
        public string MiddleSound = ResPathConfig.MAIN_MENU;

        public Action LeftCallback;
        public Action RightCallback;
        public Action MiddleCallback;

        public PropPopupModeStruct()
        {

        }
    }
}
