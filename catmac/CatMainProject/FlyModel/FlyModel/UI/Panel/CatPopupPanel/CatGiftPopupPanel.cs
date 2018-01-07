using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.CatPopupPanel
{
    public class CatGiftPopupPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "CatGiftPopup";
            }
        }

        private Text nameTF;
        private Text DesTF;
        private Image giftIamge;
        private Image catImage;

        protected override void Initialize(GameObject go)
        {
            nameTF = go.transform.Find("Name").GetComponent<Text>();
            DesTF = go.transform.Find("Des").GetComponent<Text>();

            giftIamge = go.transform.Find("GiftIcon").GetComponent<Image>();
            catImage = go.transform.Find("Cat").GetComponent<Image>();
        }

        public void SetData(Model.Data.CatGiftData data)
        {
            nameTF.text = data.Name;
            DesTF.text = data.Description;

            ResourceLoader.Instance.TryLoadPic(ResPathConfig.CAT_GIFT_ASSETBUNDLE, data.PicCode, (texture) =>
            {
                giftIamge.sprite = texture as Sprite;
                giftIamge.SetNativeSize();
            });

            ResourceLoader.Instance.TryLoadPic(ResPathConfig.CAT_HEAD_ASSETBUNDLE, data.CatPicCode, (texture) =>
            {
                catImage.sprite = texture as Sprite;
                catImage.SetNativeSize();
            });
        }

        public override void SetInfoBar()
        {

        }

        public override void Refresh()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.ShowMenuBtn(false);
            }
        }

        public override void Close(bool isCloseAllMode = false)
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.ShowMenuBtn(true);
            }

            base.Close(isCloseAllMode);
        }
    }
}
