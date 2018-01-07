using FlyModel.Model.Data;
using FlyModel.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.PicturePreviewPanel
{
    public class ImageCell
    {
        public GameObject GameObject;
        public PhotoData Data;

        private Texture2D texture;

        private Image catImage;

        private Text dateTF;

        private HandbookData handBookData;
        private GameObject normalTab;
        private GameObject famousTab;

        public ImageCell(GameObject go)
        {
            GameObject = go;

            catImage = go.transform.Find("Photo").GetComponent<Image>();

            dateTF = go.transform.Find("Date").GetComponent<Text>();

            normalTab = go.transform.Find("Tabs/Normal").gameObject;
            famousTab = go.transform.Find("Tabs/Famous").gameObject;

            new SoundButton(go, () =>
            {
                PanelManager.picturePreviewPanel.ShowFullSizePreviewPanel(Data, texture, dateTF.text);
            });
        }

        public void UpdateData(Model.Data.PhotoData data, int index)
        {
            Data = data;

            GameObject.transform.localRotation = Quaternion.Euler(0, 0, PicturePreviewPanel.ROTATIN[index % 6]);

            dateTF.text = formatDate(data.PhotoName);

            PanelManager.picturePreviewPanel.LoadPhoto(string.Format("{0}{1}", ResourceLoader.CatPictureCacheUrl, Data.PhotoName), (texture) => {
                this.texture = texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                catImage.sprite = sprite;
            });

            normalTab.SetActive(handBookData.IsSuper == false);
            famousTab.SetActive(handBookData.IsSuper);
        }

        public void Clear()
        {
            GameObject.Destroy(texture);
        }

        private string formatDate(string name)
        {
            var names = name.Split('-');
            return string.Format("{0}/{1}/{2} {3}:{4}", names[1], names[2], names[3], names[4], names[5]);
        }

        public void SetHandBookData(HandbookData data)
        {
            handBookData = data;
        }
    }
}
