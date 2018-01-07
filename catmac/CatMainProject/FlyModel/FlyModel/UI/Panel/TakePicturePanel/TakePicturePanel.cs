using Assets.Scripts;
using Assets.Scripts.TouchController;
using cn.sharesdk.unity3d;
using DG.Tweening;
using FlyModel.Model;
using FlyModel.Proto;
using FlyModel.UI.Component;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.TakePicturePanel
{
    public class TakePicturePanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "TakePicturePanel";
            }
        }

        private GameObject catCamareBtnGO;
        private GameObject screenCamareBtnGO;

        private SoundButton close;
        private SoundButton catCameraBtn;
        private SoundButton screenBtn;

        private EnumConfig.PictureMode mode = EnumConfig.PictureMode.Null;

        private GameObject photoSizeController;

        private Vector3 camarePos = new Vector3();

        private Vector2 oldFingerPos = new Vector2();

        private float MaxRectWidth = 224*1.5f;
        private float MaxRectHeight = 168*1.5f;

        private float hwRate = 3 / 4f;
        private float MinRectWidth = 224;
        private float MinRectHeight = 168;
        private float originRectWidth;
        private float originRectHeight;
        private RectTransform rectImageGORT;
        private Vector2 nowSize;

        //1：nowX-oldX nowY-oldY
        private Vector2 dirLeftTop = new Vector2(-1, 1);
        private Vector2 dirRightTop = new Vector2(1, 1);
        private Vector2 dirLeftBottom = new Vector2(-1, -1);
        private Vector2 dirRightBottom = new Vector2(1, -1);
        private Vector2 currentDir;

        private GameObject shaderMask;
        private Material material;
        private bool isShowingController;

        private float rate = Screen.height / 1136f;
        private float relativeWidth;
        private float relativeHeight;

        private DelayController delayController;

        private Model.Data.CatData Data;

        private float modifyShaderScaleFactor = 1f;

        private GameObject sceneCaptureBtnGO;

        private GameObject topBar;

        private GameObject currentCatGo;
        private Model.Data.CatData currentCatData;

        private GameObject blockImage;

        public static GameObject AnchorImage;

        private string currentPhotoPath;

        private Image image;
        private  Tweener tweener;
        private Tweener blinkTweener;

        protected override void Initialize(GameObject go)
        {
            AnchorImage = go.transform.Find("Anchor").gameObject;

            blockImage = go.transform.Find("BlockImage").gameObject;
            blockImage.SetActive(false);

            topBar = go.transform.Find("TopBtns").gameObject;

            close = new SoundButton(go.transform.Find("TopBtns/Close").gameObject, () => { Close(); });

            image = go.GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0);
            //image = go.transform.Find("Image").GetComponent<Image>();

            new SoundButton(go.transform.Find("PhotoSizeController/Rect/done").gameObject, () => {

                updateBtnsState(EnumConfig.PictureMode.Cat);

                if (PhotoManager.Instance.AllCatPictureOwners.ContainsKey(Data.CatSpineName))
                {
                    if (PhotoManager.Instance.AllCatPictureOwners[Data.CatSpineName] < PhotoManager.Instance.GetPhotoMaxCount())
                    {
                        //delayController.StartCoroutine(saveImage(EnumConfig.PictureMode.Cat));

                        // 拍摄时白屏闪现         
                        image.color = new Color(1, 1, 1, 1);
                        tweener = image.material.DOFade(1, 0.2f);
                        tweener.OnComplete(() =>
                        {
                            BlinkCallbackSaveCat(image);
                        });
                    }
                    else
                    {
                        PanelManager.ShowTipString(string.Format("每只猫咪只能拍摄{0}张", PhotoManager.Instance.GetPhotoMaxCount()), EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    }
                }
                else
                {
                    //delayController.StartCoroutine(saveImage(EnumConfig.PictureMode.Screen));


                    // 拍摄时白屏闪现         
                    image.color = new Color(1, 1, 1, 1);
                    tweener = image.material.DOFade(1, 0.2f);
                    tweener.OnComplete(() =>
                    {
                        BlinkCallbackSaveScene(image);
                    });
                }
                //// 拍摄时白屏闪现        
                ////image.gameObject.SetActive(true);  
                //image.color = new Color(1, 1, 1, 1);
                //tweener = image.material.DOFade(1, 0.2f);
                //tweener.OnComplete(() =>
                //{
                //    BlinkCallback(image);
                //});
            }, ResPathConfig.TAKE_PICTURE);

            delayController = go.AddComponent<DelayController>();

            catCamareBtnGO = go.transform.Find("TopBtns/Cat").gameObject;
            catCameraBtn = new SoundButton(catCamareBtnGO, () => {
                updateBtnsState(EnumConfig.PictureMode.Cat);
                });

            screenCamareBtnGO = go.transform.Find("TopBtns/Screen").gameObject;
            screenBtn = new SoundButton(screenCamareBtnGO, () => {
                updateBtnsState(EnumConfig.PictureMode.Screen);
                //PanelManager.ShowTipString("场景拍照暂未开放", EnumConfig.PropPopupPanelBtnModde.OneBtn);
            });

            sceneCaptureBtnGO = go.transform.Find("SceneCameraBtn").gameObject;
            sceneCaptureBtnGO.transform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            new SoundButton(sceneCaptureBtnGO, () => {
                //delayController.StartCoroutine(CaptureSceen()); // 开一个协程保存场景
                
                // 按下拍照按钮先出现白屏然后消失，消失后调用保存场景照片的方法
                // 拍摄时白屏出现              
                image.color = new Color(1, 1, 1, 1);
                tweener = image.material.DOFade(1, 0.2f);
                tweener.OnComplete(() =>
                {
                    BlinkCallback(image);
                });

            }, ResPathConfig.TAKE_PICTURE);

            photoSizeController = go.transform.Find("PhotoSizeController/Rect").gameObject;
            photoSizeController.AddComponent<DragSensor>().OnDragHandler = OnDragHandler;

            rectImageGORT = photoSizeController.GetComponent<RectTransform>();

            PointerSensor ps = photoSizeController.AddComponent<PointerSensor>();
            ps.OnPointerDownHandler = pointerDownHandler;

            go.transform.Find("PhotoSizeController/Rect/leftTop").gameObject.AddComponent<DragSensor>().OnDragHandler = updateCamareSize;
            go.transform.Find("PhotoSizeController/Rect/rightTop").gameObject.AddComponent<DragSensor>().OnDragHandler = updateCamareSize;
            go.transform.Find("PhotoSizeController/Rect/rightBottom").gameObject.AddComponent<DragSensor>().OnDragHandler = updateCamareSize;
            go.transform.Find("PhotoSizeController/Rect/leftBottom").gameObject.AddComponent<DragSensor>().OnDragHandler = updateCamareSize;

            setModifyShaderScaleFactor();
        }

        // 白屏渐隐
        private void BlinkCallback(Image image)
        {
            image.color = new Color(1, 1, 1, 0);
            blinkTweener = image.material.DOFade(0, 0.2f);
            // 动画完成回调
            blinkTweener.OnComplete(() =>
            {
                delayController.StartCoroutine(CaptureSceen()); // 开一个协程保存场景，貌似不需要协程
            });
        }

        // 白屏渐隐
        private void BlinkCallbackSaveCat(Image image)
        {
            image.color = new Color(1, 1, 1, 0);
            blinkTweener = image.material.DOFade(0, 0.2f);
            // 动画完成回调
            blinkTweener.OnComplete(() =>
            {
                delayController.StartCoroutine(saveImage(EnumConfig.PictureMode.Cat));  // 保存猫的照片
            });
        }

        // 白屏渐隐
        private void BlinkCallbackSaveScene(Image image)
        {
            image.color = new Color(1, 1, 1, 0);
            blinkTweener = image.material.DOFade(0, 0.2f);
            // 动画完成回调
            blinkTweener.OnComplete(() =>
            {
                delayController.StartCoroutine(saveImage(EnumConfig.PictureMode.Screen));   // 貌似是拍的时候猫跑了没拍到猫就把照片放到场景照片中
            });
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

        public override void Refresh()
        {
            base.Refresh();

            isShowingController = false;

            mode = EnumConfig.PictureMode.Null;

            photoSizeController.transform.parent.gameObject.SetActive(false);
            updateBtnsState(EnumConfig.PictureMode.Cat);
            PanelManager.infoBar.ShowTopBar(false);

            initCameraRect();
        }

        private void initCameraRect()
        {
            originRectWidth = MinRectWidth;
            originRectHeight = MinRectHeight;
            nowSize.x = originRectWidth;
            nowSize.y = originRectHeight;
            rectImageGORT.sizeDelta = nowSize;
        }

        public override void SetInfoBar()
        {

        }

        public override void Close(bool isCloseAllMode = false)
        {
            if (isShowingController)
            {
                closeCaptureState();
                resetCatMode();
            }
            else
            {
                if (shaderMask != null)
                {
                    shaderMask.SetActive(false);
                }

                showCatAnchorImage(false);

                base.Close(isCloseAllMode);

                PanelManager.infoBar.ShowTopBar(true);
                isShowingController = false;

                currentCatGo = null;
                currentCatData = null;
            }
        }


        private void closeCaptureState()
        {
            shaderMask.SetActive(false);
            isShowingController = false;
            photoSizeController.transform.parent.gameObject.SetActive(false);
        }

        private void resetCatMode()
        {
            if (currentCatGo != null)
            {
                currentCatGo = null;
            }
            mode = EnumConfig.PictureMode.Null;
            updateBtnsState(EnumConfig.PictureMode.Cat);
        }

        private IEnumerator saveImage(EnumConfig.PictureMode mode)
        {
            closeCaptureState();

            yield return new WaitForEndOfFrame();

            ////拍摄时白屏闪现
            //tweener = image.material.DOFade(1, 0.2f);
            //tweener.OnComplete(() =>
            //{
            //    BlinkCallback(image);
            //});

            //图片大小
            Texture2D cutImage = new Texture2D((int)relativeWidth, (int)relativeHeight, TextureFormat.RGB24, false);

            //坐标左下角为0  
            Rect rect = new Rect((int)(camarePos.x - relativeWidth / 2f + 2), (int)(camarePos.y - relativeHeight / 2f + 2), (int)relativeWidth, (int)relativeHeight);
            cutImage.ReadPixels(rect, 0, 0, false);

            cutImage.Apply();
            byte[] byt = cutImage.EncodeToJPG();
            string time = GameMain.TimeTick.ConvertTime(GameMain.TimeTick.GetNow()).ToString("yyyy-MM-dd-HH-mm-ss");
            string photoName = string.Format("{0}-{1}", Data.CatSpineName, time);
            string fileName = string.Format("{0}{1}.jpg", ResourceLoader.CatPictureCacheRoot, photoName);
            var dir = System.IO.Path.GetDirectoryName(ResourceLoader.CatPictureCacheRoot);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            System.IO.FileStream cache = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            cache.Write(byt, 0, byt.Length);
            cache.Close();

            Object.Destroy(cutImage);

            PhotoManager.Instance.AddOneCatPhoto(string.Format("{0}.jpg", photoName));

            currentPhotoPath = fileName;

            initCameraRect();
            
            if (mode == EnumConfig.PictureMode.Cat)
            {
                resetCatMode();
            }
        }

        private void hideUI(bool isHide)
        {
            topBar.SetActive(isHide == false);
            sceneCaptureBtnGO.SetActive(isHide == false);
        }

        private IEnumerator CaptureSceen()
        {
            //Debug.Log("1");
            //// 拍摄时白屏闪现              
            //tweener = image.material.DOFade(1, 0.2f);
            //Debug.Log(image.material.color);
            //tweener.OnComplete(() =>
            //{
            //    Debug.Log("2");
            //    BlinkCallback(image);
            //});

            hideUI(true);

            yield return new WaitForEndOfFrame();

            //图片大小
            Texture2D cutImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            //坐标左下角为0  
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            cutImage.ReadPixels(rect, 0, 0, false);

            hideUI(false);

            cutImage.Apply();
            byte[] byt = cutImage.EncodeToJPG(50);
            string time = GameMain.TimeTick.ConvertTime(GameMain.TimeTick.GetNow()).ToString("yyyy-MM-dd-HH-mm-ss");
            string photoName = string.Format("{0}-{1}", "scene", time);
            string fileName = string.Format("{0}{1}.jpg", ResourceLoader.ScenePictureCacheRoot, photoName);
            //Debug.Log(fileName);
            //Debug.Log(time);
            var dir = System.IO.Path.GetDirectoryName(ResourceLoader.ScenePictureCacheRoot);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            System.IO.FileStream cache = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            cache.Write(byt, 0, byt.Length);
            cache.Close();

            Object.Destroy(cutImage);

            PhotoManager.Instance.AddOneScenePhoto(string.Format("{0}.jpg", photoName));

            currentPhotoPath = fileName;
        }

        private void ShareImage()
        {
            ShareContent content = new ShareContent();
            content.SetImageUrl("http://maocdn.apowogame.com/1.jpg");
            content.SetShareType(ContentType.Image);

            GameApplication.Instance.SSDK.ShowPlatformList(null, content, 100, 100);

            //PlatformType[] platforms = new PlatformType[2] { PlatformType.WeChat, PlatformType.QQ };
            //GameApplication.Instance.SSDK.ShareContent( PlatformType.QQ, content);
            //GameApplication.Instance.SSDK.ShareContent(PlatformType.WeChat, content);
        }

        private void showSceneCameraBtn(bool isShow)
        {
            sceneCaptureBtnGO.SetActive(isShow);
        }

        private void updateBtnsState(EnumConfig.PictureMode mode)
        {
            if (this.mode == mode)
            {
                return;
            }

            this.mode = mode;

            switch (mode)
            {
                case EnumConfig.PictureMode.Cat:
                    catCamareBtnGO.GetComponent<Image>().color = Color.white;
                    screenCamareBtnGO.GetComponent<Image>().color = ColorConfig.CAMARE_BTN_MASK;

                    showSceneCameraBtn(false);

                    if (currentCatGo != null)
                    {
                        ShowPhotoController(currentCatGo, currentCatData);
                    }

                    blockImage.SetActive(false);

                    showCatAnchorImage(true);
                    break;
                case EnumConfig.PictureMode.Screen:
                    catCamareBtnGO.GetComponent<Image>().color = ColorConfig.CAMARE_BTN_MASK;
                    screenCamareBtnGO.GetComponent<Image>().color = Color.white;

                    showSceneCameraBtn(true);

                    photoSizeController.SetActive(false);
                    if (shaderMask != null)
                        shaderMask.SetActive(false);

                    isShowingController = false;

                    blockImage.SetActive(false);

                    showCatAnchorImage(false);
                    break;
                default:
                    break;
            }
        }

        private void showCatAnchorImage(bool isShow)
        {
            SceneManager.Instance.CurrentScene.SceneGameObject.ShowCatAnchorImage(isShow);
        }

        private void OnDragHandler(PointerEventData eventData)
        {
            camarePos.x = eventData.position.x;
            camarePos.y = eventData.position.y;

            camarePos.x = Mathf.Min(Screen.width - nowSize.x * rate/2, Mathf.Max(camarePos.x, nowSize.x / 2 * rate));
            camarePos.y = Mathf.Min(Screen.height - nowSize.y * rate/2, Mathf.Max(camarePos.y, nowSize.y / 2 * rate));

            photoSizeController.transform.parent.position = camarePos;

            showMask(camarePos.x, camarePos.y, nowSize.x, nowSize.y);
        }

        private void pointerDownHandler(PointerEventData eventData)
        {
            if (eventData.position.x < photoSizeController.transform.parent.position.x && eventData.position.y > photoSizeController.transform.parent.position.y)
            {
                //左上
                currentDir = dirLeftTop;
            }
            else if (eventData.position.x > photoSizeController.transform.parent.position.x && eventData.position.y > photoSizeController.transform.parent.position.y)
            {
                //右上
                currentDir = dirRightTop;
            }else if (eventData.position.x < photoSizeController.transform.parent.position.x && eventData.position.y < photoSizeController.transform.parent.position.y)
            {
                //左下
                currentDir = dirLeftBottom;
            }else if (eventData.position.x > photoSizeController.transform.parent.position.x && eventData.position.y < photoSizeController.transform.parent.position.y)
            {
                //右下
                currentDir = dirRightBottom;
            }

            oldFingerPos = eventData.position;

            originRectWidth = nowSize.x;
            originRectHeight = nowSize.y;
        }

        private void updateCamareSize(PointerEventData eventData)
        {
            float disH = currentDir.x * (eventData.position.x - oldFingerPos.x);
            if (isOutScreen(originRectWidth + disH, (originRectWidth + disH) * hwRate) == false)
            {
                nowSize.x = originRectWidth + disH;
                nowSize.y = (originRectWidth + disH) * hwRate;

                nowSize.x = Mathf.Max(MinRectWidth, Mathf.Min(nowSize.x, MaxRectWidth));
                nowSize.y = Mathf.Max(MinRectHeight, Mathf.Min(nowSize.y, MaxRectHeight));

                rectImageGORT.sizeDelta = nowSize;

                updateShaderMask(camarePos.x, camarePos.y, nowSize.x, nowSize.y);
            }
        }

        private bool isOutScreen(float w, float h)
        {
            w = w * rate;
            h = h * rate;
            return rectImageGORT.position.x - w / 2 < 0 || rectImageGORT.position.x + w / 2 > Screen.width
                || rectImageGORT.position.y - h/2 < 0 || rectImageGORT.position.y + h/2 > Screen.height;
        }

        public void ShowPhotoController(GameObject catGO, Model.Data.CatData catData)
        {
            if (isShowingController == false)
            {

                Data = catData;

                currentCatGo = catGO;
                currentCatData = catData;

                isShowingController = true;
                photoSizeController.transform.parent.gameObject.SetActive(true);
                camarePos.x = catGO.transform.position.x;
                camarePos.y = catGO.transform.position.y + catGO.GetComponent<RectTransform>().sizeDelta.y / 3;
                photoSizeController.transform.parent.position = camarePos;

                photoSizeController.SetActive(true);
                blockImage.SetActive(true);
                showCatAnchorImage(false);

                showMask(camarePos.x, camarePos.y, nowSize.x, nowSize.y);
            }
        }

        private void updateShaderMask(float x, float y, float width, float height)
        {
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
        }

        private void showMask(float x, float y, float width, float height)
        {
            if (shaderMask == null)
            {
                string materialName = "RectMask";
                ResourceLoader.Instance.TryLoadClone(materialName.ToLower(), materialName, (rectMask) =>
                {
                    shaderMask = rectMask as GameObject;
                    shaderMask.transform.SetParent(PanelPrefab.transform, true);
                    updateShaderMask(x, y, width, height);
                    topBar.transform.SetAsLastSibling();
                });
            }
            else
            {
                updateShaderMask(x, y, width, height);
                shaderMask.SetActive(true);
                topBar.transform.SetAsLastSibling();
            }
        }

        
    }
}
