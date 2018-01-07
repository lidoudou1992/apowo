using Assets.Scripts.TouchController;
using FlyModel.Model;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.PicturePreviewPanel
{
    public class FullSizePreviewContainer
    {
        public GameObject GameObject;

        private Image image;

        private GameObject nameGO;
        private Text nameTF;

        private Vector2 nameSize = new Vector2(176, 30);
        private RectTransform imageRectTransform;

        private float rate = (Screen.height / 1136f);
        private Vector2 textureAbsoluteSize = new Vector2();

        private Model.Data.PhotoData photoData;

        private Texture2D texture;

        public bool IsShow;

        public FullSizePreviewContainer(GameObject go)
        {
            GameObject = go;
            GameObject.transform.SetParent(go.transform.parent.parent, false);

            image = go.transform.Find("Image").GetComponent<Image>();
            imageRectTransform = image.transform.GetComponent<RectTransform>();

            nameGO = go.transform.Find("Name").gameObject;
            nameTF = nameGO.transform.Find("Text").GetComponent<Text>();

            PointerSensor pointerSensor = go.AddComponent<PointerSensor>();
            pointerSensor.OnPointerClickHandler = closeSelf;

            new SoundButton(go.transform.Find("del").gameObject, () =>
            {
                PanelManager.ShowTipString("确认删除照片？", EnumConfig.PropPopupPanelBtnModde.TwoBtb, leftCallback: () =>
                {
                    PhotoManager.Instance.DeleteOneCatPhoto(photoData.PhotoName);
                    closeSelf(null);
                    PanelManager.picturePreviewPanel.RefreshWhenBack();
                }
                );
            });

            new SoundButton(go.transform.Find("share ").gameObject, () =>
            {
                PanelManager.PictureSharePanel.Show(() =>
                {
                    PanelManager.pictureSharePanel.SetData(texture.EncodeToJPG(75), photoData.PhotoName);
                });
            });
        }

        private void closeSelf(PointerEventData eventData)
        {
            GameObject.SetActive(false);

            image.sprite = null;
            image.gameObject.SetActive(false);

            IsShow = false;
        }

        public void ShowSelf(Model.Data.PhotoData photoData, Texture2D texture, string aliasName)
        {
            IsShow = true;

            showImage(texture);

            GameObject.SetActive(true);
            GameObject.transform.SetAsLastSibling();

            nameGO.transform.GetComponent<RectTransform>().sizeDelta = nameSize;
            nameGO.transform.localPosition = new Vector3(nameGO.transform.localPosition.x, image.transform.localPosition.y + textureAbsoluteSize.y / 2 + 30, 0);
            nameTF.text = aliasName;

            this.photoData = photoData;
        }

        private void showImage(Texture2D texture)
        {
            this.texture = texture;

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;

            textureAbsoluteSize.x = texture.width / rate;
            textureAbsoluteSize.y = texture.height / rate;
            imageRectTransform.sizeDelta = textureAbsoluteSize;

            image.gameObject.SetActive(true);
        }
    }
}
