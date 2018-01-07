using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.TreasurePanel
{
    public class TreasureItem
    {
        public GameObject GameObject;

        public GameObject knowGO;
        public GameObject unknowGO;
        public Image treasureImage;

        private Model.Data.CatGiftData Data;

        public TreasureItem(GameObject go)
        {
            GameObject = go;

            knowGO = go.transform.Find("konw").gameObject;
            treasureImage = knowGO.transform.Find("Image").GetComponent<Image>();
            unknowGO = go.transform.Find("unkonw").gameObject;

            new SoundButton(go, () =>
            {
                if (Data.IsGetted)
                {
                    PanelManager.CatGiftPopupPanel.Show(() =>
                    {
                        PanelManager.CatGiftPopupPanel.SetData(Data);
                    });
                }
                else
                {
                    Debug.Log("未被发现");
                }

            }, "Button");
        }

        public void UpdateData(Model.Data.CatGiftData data)
        {
            Data = data;

            knowGO.SetActive(data.IsGetted);
            unknowGO.SetActive(data.IsGetted == false);

            ResourceLoader.Instance.TryLoadPic(ResPathConfig.CAT_GIFT_ASSETBUNDLE, data.PicCode, (texture) =>
            {
                if (treasureImage != null)
                {
                    treasureImage.sprite = texture as Sprite;
                    treasureImage.SetNativeSize();
                }
            });
        }
    }
}
