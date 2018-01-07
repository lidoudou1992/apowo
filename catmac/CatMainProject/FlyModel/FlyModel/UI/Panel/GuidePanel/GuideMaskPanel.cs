using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.GuidePanel
{
    public class GuideMaskPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "GuideMask";
            }
        }

        private float rate = Screen.height / 1136f;
        private float relativeWidth;
        private float relativeHeight;
        private Material material;
        private GameObject shaderMask;

        private static float modifyShaderScaleFactor = 1f;

        private GameObject ImagePrefab;

        private GameObject topBlock;
        private GameObject middleLeftBlock;
        private GameObject middleRightBlock;
        private GameObject bottomBlock;

        private GameObject gesture;

        public override void Load()
        {
            ResourceLoader.Instance.TryLoadClone(BundleName, AssetName, (go) =>
            {
                PanelPrefab = go;

                go.transform.SetParent(transform, true);
                TryInitializeBaseUI();
                Initialize(go);
                InitializeOver();
                loaded = true;
                if (ShowOnLoaded)
                {
                    Show();
                }
            });
        }

        public override bool Show()
        {
            IsNeedPushToPanelStack = false;
            bool flag = base.Show();
            return flag;
        }

        public override void Close(bool isCloseAllMode = false)
        {
            Hide();
            Dispose();

            updateAwardBtnActive();
            UpdateCamareBtnActive();
            UpdateMailBtnActive();
            //UpdateSettingBtnActive();
            UpdateSignBtnActive();
            UpdateShareBtnActive();
            UpdateExchangeBtnActive();
            UpdateTeachBtnActive();
            UpdateLotterBtnActive();  // 抽奖
            UpdateChitchatBtnActive();  // 聊天
        }

        protected override void Initialize(GameObject go)
        {
            transform.SetParent(PanelManager.GuideMaskRectTransform, true);

            setModifyShaderScaleFactor();

            GameObject prefab = go as GameObject;
            shaderMask = prefab.transform.Find("Mask").gameObject;

            ImagePrefab = go.transform.Find("Image").gameObject;
        }

        public override void Refresh()
        {
            base.Refresh();

            if (gesture != null)
            {
                gesture.transform.SetAsLastSibling();
            }
        }


        public override void SetInfoBar()
        {
            
        }

        private void setModifyShaderScaleFactor()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2)
                {
                    modifyShaderScaleFactor = 1.2748f;
                }
            }
        }

        public void UpdateShaderMask(float x, float y, float width, float height)
        {
            height = width;
            shaderMask.transform.position = new Vector3(Screen.width / 2f, Screen.height / 2f);

            RectTransform rt = shaderMask.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(Screen.width, Screen.height);

            Image image = shaderMask.GetComponent<Image>();
            image.material.shader = Shader.Find("CatProj/MaskShader");

            float HRate = (Screen.width) / 200f / rate / (width / 100f);
            float VRate = (Screen.height) / 200f / rate / (height / 100f);

            relativeWidth = rate * width;
            relativeHeight = rate * height;

            material = image.material;

            material.SetTextureScale("_Mask", new Vector2(HRate / modifyShaderScaleFactor, VRate / modifyShaderScaleFactor));
            material.SetTextureOffset("_Mask", new Vector2((0.5f - x / (relativeWidth) * 0.5f) / modifyShaderScaleFactor, (0.5f - y / (relativeHeight) * 0.5f) / modifyShaderScaleFactor));

            createBlocks(x, y, width, height);
        }

        private void createBlocks(float targetX, float targetY, float targetWidth, float targetHeight)
        {
            targetWidth = targetWidth * rate;
            targetHeight = targetHeight * rate;
            if (topBlock == null)
            {
                topBlock = GameObject.Instantiate(ImagePrefab);
                topBlock.SetActive(true);
                topBlock.transform.SetParent(transform, true);

                middleLeftBlock = GameObject.Instantiate(ImagePrefab);
                middleLeftBlock.SetActive(true);
                middleLeftBlock.transform.SetParent(transform, true);

                middleRightBlock = GameObject.Instantiate(ImagePrefab);
                middleRightBlock.SetActive(true);
                middleRightBlock.transform.SetParent(transform, true);

                bottomBlock = GameObject.Instantiate(ImagePrefab);
                bottomBlock.SetActive(true);
                bottomBlock.transform.SetParent(transform, true);
            }

            float w = Screen.width;
            float h = (Screen.height - targetY) - targetHeight / 2;
            topBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
            topBlock.transform.position = new Vector3(w / 2, Screen.height - h / 2);

            w = targetX - targetWidth / 2;
            h = targetHeight;
            middleLeftBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
            middleLeftBlock.transform.position = new Vector3(w / 2, targetY);

            w = Screen.width - targetX - targetWidth / 2;
            h = targetHeight;
            middleRightBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
            middleRightBlock.transform.position = new Vector3(Screen.width - w/2, targetY);

            w = Screen.width;
            h = targetY - targetHeight/2;
            bottomBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
            bottomBlock.transform.position = new Vector3(w / 2, h/2);
        }

        public void ShowGesture(Vector3 targetPos, Vector2 targetSize, string parms)
        {
            var paramsAry = parms.Split('#');
            int xModify = int.Parse(paramsAry[1].ToString());
            int yModify = int.Parse(paramsAry[2].ToString());
            int rotationModify = int.Parse(paramsAry[3].ToString());
            string dir = paramsAry[0];
            //string dir = "1"; 测试用的，注释掉

            // 显示手势
            if (gesture == null)
            {
                gesture = GameObject.Instantiate(GuidePanel.GestureGO);  // 引导画面1
            }
            gesture.SetActive(true);
            

            //   1
            //4     2
            //   3
            if (dir == "1")
            {
                Debug.Log("1-------------------------------------------------------");
                gesture.transform.position = new Vector3(targetPos.x + xModify, targetPos.y + targetSize.y / 2 + yModify, 0);
                gesture.transform.rotation = Quaternion.Euler(0, 0, 180 + rotationModify);               
            }
            else if (dir == "2")
            {
                //gesture.transform.position = new Vector3(targetPos.x, targetPos.y - targetSize.y / 2 - 70, 0);
            }
            else if (dir == "3")
            {
                Debug.Log("3-------------------------------------------------------");
                gesture.transform.position = new Vector3(targetPos.x + xModify, targetPos.y - targetSize.y / 2 + yModify, 0);
                gesture.transform.rotation = Quaternion.Euler(0, 0, 0 + rotationModify);                           
            }
            else if (dir == "4")
            {
                Debug.Log("4-------------------------------------------------------");
                gesture.transform.position = new Vector3(targetPos.x - targetSize.x / 2 - 70 + xModify, targetPos.y + yModify, 0);
                gesture.transform.rotation = Quaternion.Euler(0, 0, 270 + rotationModify);             
            }

            gesture.transform.SetParent(transform, true);
            gesture.transform.SetAsLastSibling();
        }
    }
}
