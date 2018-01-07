using FlyModel.Model.Data;
using FlyModel.Proto;
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
        public enum BtnModde
        {
            OneBtn = 0,
            TwoBtb = 1,
        }

        public override string AssetName
        {
            get
            {
                return "PropPopup";
            }
        }

        private PropPopupModeStruct modeStruct;
        private BaseProp data;
        private string titleString;

        private Text titleTF;
        private Text propNameTF;
        private Text propDesTF;
        private Image PropImage;
        private Image priceIcon;
        private Text priceTF;

        private GameObject leftBtn;
        private GameObject rightBtn;
        private GameObject middleBtn;

        private bool isDataPrepared;

        public PropPopupPanel(RectTransform parent) : base(parent)
        {

        }

        public void SetData(string titleString, BaseProp config, PropPopupModeStruct modeStruct)
        {
            this.modeStruct = modeStruct;
            this.data = config;
            this.titleString = titleString;
            isDataPrepared = true;

            updateData();
        }

        protected override void Initialize(GameObject go)
        {
            titleTF = go.transform.Find("Title").GetComponent<Text>();
            propNameTF = go.transform.Find("PropInfo/Name").GetComponent<Text>();
            propDesTF = go.transform.Find("PropInfo/Des").GetComponent<Text>();
            PropImage = go.transform.Find("PropInfo/Image").GetComponent<Image>();
            PropImage.gameObject.SetActive(false);
            priceIcon = go.transform.Find("PropInfo/PriceGroup/Icon").GetComponent<Image>();
            priceIcon.gameObject.SetActive(false);
            priceTF = go.transform.Find("PropInfo/PriceGroup/Price").GetComponent<Text>();

            leftBtn = go.transform.Find("ButtonGroup/Left").gameObject;
            leftBtn.SetActive(false);
            rightBtn = go.transform.Find("ButtonGroup/Right").gameObject;
            rightBtn.SetActive(false);
            middleBtn = go.transform.Find("ButtonGroup/Middle").gameObject;
            middleBtn.SetActive(false);
        }

        public override void Refresh()
        {
            base.Refresh();

            PanelPrefab.SetActive(false);
        }

        private void updateData()
        {
            if (isDataPrepared)
            {
                titleTF.text = titleString;
                propNameTF.text = data.Name;
                propDesTF.text = data.Description;

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
                if (modeStruct.Mode == BtnModde.TwoBtb)
                {
                    leftBtn.transform.Find("Text").GetComponent<Text>().text = modeStruct.LeftBtnString;
                    rightBtn.transform.Find("Text").GetComponent<Text>().text = modeStruct.RightBtnString;

                    leftBtn.transform.GetComponent<Button>().onClick.AddListener(modeStruct.LeftCallback);
                    rightBtn.transform.GetComponent<Button>().onClick.AddListener(modeStruct.RightCallback);

                    leftBtn.SetActive(true);
                    rightBtn.SetActive(true);
                    middleBtn.SetActive(false);
                }
                else if (modeStruct.Mode == BtnModde.OneBtn)
                {
                    middleBtn.transform.Find("Text").GetComponent<Text>().text = modeStruct.MiddleBtnString;
                    if (modeStruct.MiddleCallback == null)
                    {
                        modeStruct.MiddleCallback = () => { PanelManager.CurrentPanel.Close(); };
                    }
                    middleBtn.transform.GetComponent<Button>().onClick.AddListener(modeStruct.MiddleCallback);

                    leftBtn.SetActive(false);
                    rightBtn.SetActive(false);
                    middleBtn.SetActive(true);
                }

                PanelPrefab.SetActive(true);
            }
        }

        public override void Close()
        {
            isDataPrepared = false;
            modeStruct = null;
            leftBtn.transform.GetComponent<Button>().onClick.RemoveAllListeners();
            rightBtn.transform.GetComponent<Button>().onClick.RemoveAllListeners();
            middleBtn.transform.GetComponent<Button>().onClick.RemoveAllListeners();

            base.Close();
        }
    }

    public class PropPopupModeStruct
    {
        public PropPopupPanel.BtnModde Mode;
        public string LeftBtnString;
        public string RightBtnString;
        public string MiddleBtnString;

        public UnityAction LeftCallback;
        public UnityAction RightCallback;
        public UnityAction MiddleCallback;

        public PropPopupModeStruct()
        {

        }
    }
}
