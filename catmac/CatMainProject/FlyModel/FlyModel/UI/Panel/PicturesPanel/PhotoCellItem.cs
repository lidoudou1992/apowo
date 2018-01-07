using FlyModel.UI.Component.PageView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyModel.Model.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using FlyModel.Utils;
using UnityEngine.UI;
using FlyModel.Model;

namespace FlyModel.UI.Panel.PicturesPanel
{
    public class PhotoCellItem : IItem
    {
        public GameObject GameObject;

        public HandbookData Data;

        private Text nameTF;
        private Image catImage;
        private Text timeTF;

        private GameObject sceneEntrance;
        private GameObject normalCatBg;
        private GameObject superCatBg;

        public PhotoCellItem(GameObject go)
        {
            GameObject = go;

            nameTF = go.transform.Find("Name").GetComponent<Text>();
            catImage = go.transform.Find("CatContainer/CatImage").GetComponent<Image>();
            timeTF = go.transform.Find("Time").GetComponent<Text>();

            sceneEntrance = go.transform.Find("Image_3").gameObject;
            normalCatBg = go.transform.Find("Image_1").gameObject;
            superCatBg = go.transform.Find("Image_2").gameObject;
        }

        public void Init(BaseProp data, int pageIndex, int itemIndex)
        {
            Data = data as HandbookData;

            if (Data.Name == "SceneEntryData")
            {
                sceneEntrance.SetActive(true);
            }
            else
            {
                nameTF.text = Model.Data.CatData.GetCatAlias(Data.Name);
                timeTF.text = string.Format("{0}/{1}", Data.PhotoCount, PhotoManager.Instance.GetPhotoMaxCount());

                normalCatBg.SetActive(Data.IsSuper == false);
                superCatBg.SetActive(Data.IsSuper);

                ResourceLoader.Instance.TryLoadPic(ResPathConfig.CAT_HEAD_ASSETBUNDLE, Data.PicCode, (texture) =>
                {
                    catImage.sprite = texture as Sprite;
                    catImage.SetNativeSize();
                });
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SoundUtil.PlaySound(ResPathConfig.COMMON_BUTTON);

            if (Data.Name == "SceneEntryData")
            {
                PanelManager.ScenePhotoPanel.Show();
            }
            else
            {
                PanelManager.PicturePreviewPanel.Show(()=> {
                    PanelManager.picturePreviewPanel.SetData(Data);
                });
            }
        }
    }
}
