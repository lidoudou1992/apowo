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

namespace FlyModel.UI.Panel.ScenePhotoPreviewPanel
{
    public class ScenePhotoPreviewPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "ScenePhotoPreviewPanel";
            }
        }

        public GameObject GameObject;

        private Image image;

        private float rate = (Screen.height / 1136f);
        private Vector2 textureAbsoluteSize = new Vector2();
        private RectTransform imageRectTransform;

        private Model.Data.PhotoData photoData;

        private GameObject imageContainer;
        private float height;

        private GameObject nameGO;
        private Text nameTF;

        private Texture2D texture;

        protected override void Initialize(GameObject go)
        {
            GameObject = go;

            imageContainer = go.transform.Find("ImageContainer").gameObject;
            height = imageContainer.GetComponent<RectTransform>().sizeDelta.y;

            image = go.transform.Find("ImageContainer/Image").GetComponent<Image>();
            imageRectTransform = image.transform.GetComponent<RectTransform>();

            PointerSensor pointerSensor = go.AddComponent<PointerSensor>();
            pointerSensor.OnPointerClickHandler = closeSelf;

            new SoundButton(go.transform.Find("del").gameObject, () =>
            {
                PanelManager.ShowTipString("确认删除照片？", EnumConfig.PropPopupPanelBtnModde.TwoBtb, leftCallback: () =>
                {
                    PhotoManager.Instance.DeleteOneScenePhoto(photoData.PhotoName);
                    Close();
                }
                );
            });

            new SoundButton(go.transform.Find("share ").gameObject, () =>
            {
                PanelManager.PictureSharePanel.Show(()=> {
                    PanelManager.pictureSharePanel.SetData(texture.EncodeToJPG(75), photoData.PhotoName);
                });
            });

            nameGO = go.transform.Find("Name").gameObject;
            nameTF = nameGO.transform.Find("Text").GetComponent<Text>();
        }
        private void closeSelf(PointerEventData eventData)
        {
            Close();
        }

        public override void SetInfoBar()
        {
            
        }

        public void SetData(Model.Data.PhotoData photoData, Texture2D texture, string aliasName)
        {
            nameTF.text = aliasName;

            this.texture = texture;

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;

            textureAbsoluteSize.x = texture.width / rate;
            textureAbsoluteSize.y = texture.height / rate;
            imageRectTransform.sizeDelta = textureAbsoluteSize;

            float scale = height / textureAbsoluteSize.y;
            image.transform.localScale = new Vector3(scale, scale, 1);

            this.photoData = photoData;
        }
    }
}
