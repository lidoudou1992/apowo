using Assets.Scripts.Common;
using Assets.Scripts.TouchController;
using DG.Tweening;
using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI.Component;
using FlyModel.UI.Panel.TakePicturePanel;
using FlyModel.UI.Scene.ViewObject.Data;
using FlyModel.UI.Scene.ViewObject.SceneViewObject;
using FlyModel.Utils;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FlyModel.UI.Scene
{
    public class SceneGameObject
    {
        public LayerGameObject BackLayer;
        public LayerGameObject MiddleLayer;
        public LayerGameObject FrontLayer;

        public JsonData configJsonData;

        public GameObject Scene;

        public List<ToyGameObject> toyGameObjectList = new List<ToyGameObject>();
        private List<CatGameObject> catGameObjectList = new List<CatGameObject>();
        public List<CatGameObject> CatGameObjectList
        {
            get
            {
                return catGameObjectList;
            }

            set
            {
                catGameObjectList = value;
            }
        }
        private List<FoodGameObject> foodGameObjectList = new List<FoodGameObject>();

        public PointerSensor clickSensor;
        public Action<PointerEventData> onDownCallback;
        public Action<PointerEventData> onUpCallback;

        public DragSensor dragSensor;

        private float backSpeed = 0.8f;
        private float middleSpeed = 1f;
        private float frontSpeed = 1.3f;

        private Vector3 backLayerPos;
        private Vector3 middleLayerPos;
        private Vector3 frontLayerPos;

        public float BackLayerWidth = 1320f;
        public float MiddleLayerWidth = 1650f;
        public float FrontLayerWidth = 1980f;

        private float backMScrollerX;
        private float middleMScrollerX;
        private float frontMScrollerX;
        private float halfScrollingDistence;
        private float currentLimitScrollingX;

        private float startDragPosX;
        private float distence;

        private Vector3 frontLayerStartDragPos;
        private Vector3 middleLayerStartDragPos;
        private Vector3 backLayerStartDragPos;

        private float lastPointerPosX;
        private float dir = 0;

        private float easingSpeed;
        private float downTime;
        private float downPosX;

        private bool useEasing;
        private float minSpeed = 20;
        private float maxSpeed = 40;

        private float holdOnTime;

        private EmptyMonoBehaviour frameTool;

        private bool isMoved;

        private Vector2 cameraSize;

        private bool isTouchedCat;
        private bool isStopDragScene;
        private Model.Data.CatData lastGragCat;
        private GameObject draggingCatGO;
        private bool canPlayingDraggingEffect;

        private DelayController delayController;

        //private float oldFoodWorldPos;
        private float foodMoveXBackLayer;
        private float foodMoveXMiddleLayer;
        private float foodMoveXFrontLayer;

        // 是否需要显示猫生气离开的提示
        public static bool showLeaveTip = false;

        public SceneGameObject(GameObject scene)
        {
            CameraAbsoluteResolution cameraAbsoluteResolution = GameObject.Find("Main Camera").GetComponent<CameraAbsoluteResolution>();
            cameraSize = cameraAbsoluteResolution.GetScreenPixelDimensions();

            float rate = 1136 / cameraSize.y;

            UpdateSceneSize();

            backMScrollerX = BackLayerWidth - cameraSize.x * rate;
            middleMScrollerX = MiddleLayerWidth - cameraSize.x * rate;
            frontMScrollerX = FrontLayerWidth - cameraSize.x * rate;
            //Debug.Log(backMScrollerX + " " + middleMScrollerX + " " + frontMScrollerX);

            Scene = scene;
            initLayers(rate);

            mountScriptToSceneRoot();
        }

        public void UpdateSceneSize()
        {
            halfScrollingDistence = SceneManager.Instance.IsOpenFullSize ? 0 : -650;
            currentLimitScrollingX = halfScrollingDistence;
        }

        private void mountScriptToSceneRoot()
        {
            clickSensor = Scene.AddComponent<PointerSensor>();
            clickSensor.OnPointerUpHandler = OnUpHandler;
            clickSensor.OnPointerDownHandler = OnDownHandler;
            clickSensor.OnPointerClickHandler = OnClickHandler;

            dragSensor = Scene.AddComponent<DragSensor>();
            dragSensor.OnDragHandler = OnDragHandler;

            frameTool = Scene.AddComponent<EmptyMonoBehaviour>();
            frameTool.UpdateHandler = easingScrolling;

            delayController = Scene.AddComponent<DelayController>();
        }

        public void initLayers(float rate)
        {
            LayerGameObject layerData;
            GameObject temp = Scene.transform.Find("MiddleLayer").gameObject;
            layerData = new LayerGameObject(temp);
            MiddleLayer = layerData;

            temp = Scene.transform.Find("BackLayer").gameObject;
            layerData = new LayerGameObject(temp);
            BackLayer = layerData;

            temp = Scene.transform.Find("FrontLayer").gameObject;
            layerData = new LayerGameObject(temp);
            FrontLayer = layerData;

            float modifyX = 160 - (cameraSize.x * rate - 640) / 2;
            if (modifyX < 0)
            {
                modifyX = 0;
            }

            stopOnRight(modifyX);
            middleLayerPos = MiddleLayer.Layer.transform.localPosition;
            backLayerPos = BackLayer.Layer.transform.localPosition;
            frontLayerPos = FrontLayer.Layer.transform.localPosition;

        }

        public ToyPointGameObject FindToyPointByIndex(ScenePointStruct pointStruct)
        {
            return MiddleLayer.FindToyPointByIndex(pointStruct);
        }

        public FoodPointGameObject FindFoodPointByIndex(ScenePointStruct pointStruct)
        {
            return MiddleLayer.FindFoodPointByIndex(pointStruct);
        }

        public ToyGameObject AddOneToyGameObject(BaseProp data)
        {
            ToyGameObject toyGameObject = new ToyGameObject(data);
            toyGameObjectList.Add(toyGameObject);

            return toyGameObject;
        }

        public FoodGameObject AddOneFoodGameObjet(Model.Data.FoodData data)
        {
            FoodGameObject foodGameObject = new FoodGameObject(data);
            foodGameObjectList.Add(foodGameObject);

            return foodGameObject;
        }

        public CatGameObject AddOneCatGameObject(Model.Data.CatData data)
        {
            CatGameObject catGameObject = new CatGameObject(data);
            CatGameObjectList.Add(catGameObject);

            return catGameObject;
        }

        public void DeleteOneToyGameObject(long id)
        {
            ToyGameObject temp;
            for (int i = 0; i < toyGameObjectList.Count; i++)
            {
                temp = toyGameObjectList[i];
                if (temp.toyData.ID == id)
                {
                    toyGameObjectList.Remove(temp);
                    if (temp.Root.transform.parent != null)
                    {
                        UnityEngine.Object.Destroy(temp.Root);
                    }
                    else
                    {
                        Debug.Log("删除了已经被删除的玩具");
                    }
                    break;
                }
            }
        }

        public void DeleteOneFoodGameObject(long id)
        {
            foreach (var foodGO in foodGameObjectList)
            {
                if (foodGO.FoodData.ID == id)
                {
                    foodGameObjectList.Remove(foodGO);
                    if (foodGO.Root.transform.parent != null)
                    {
                        UnityEngine.Object.Destroy(foodGO.Root);
                    }
                    else
                    {
                        Debug.Log("删除了已经被删除的猫粮");
                    }
                    break;
                }
            }
        }      

        public void DeleteOneCatGameObject(long id)
        {
            CatGameObject temp;
            for (int i = 0; i < catGameObjectList.Count; i++)
            {
                temp = catGameObjectList[i];
                if (temp.CatData.ID == id)
                {
                    catGameObjectList.Remove(temp);
                    temp.DeleteSelf();

                    //TODO 对应删除toyGameObject中的猫数据
                    break;
                }
            }
        }

        public List<ToyGameObject> GetAllInSceneToyGameObject()
        {
            List<ToyGameObject> inSceneToys = new List<ToyGameObject>();
            foreach (var toyData in toyGameObjectList)
            {
                if (toyData.IsInScene)
                {
                    inSceneToys.Add(toyData);
                }
            }

            return inSceneToys;
        }

        public List<CatGameObject> GetAllInSceneCatGameObject()
        {
            List<CatGameObject> cats = new List<CatGameObject>();
            foreach (var catData in CatGameObjectList)
            {
                if (catData.IsInScene)
                {
                    cats.Add(catData);
                }
            }

            return cats;
        }

        public ToyGameObject FindToyGameObjectByPointIndex(int pointIndex)
        {
            foreach (var toyGameObject in toyGameObjectList)
            {
                if (toyGameObject.PlaceIndexStruct.ScenePointIndex == pointIndex)
                {
                    return toyGameObject;
                }
            }
            return null;
        }

        public ToyGameObject FindToyGameObjectByID(long toyID)
        {
            foreach (var toyGameObject in toyGameObjectList)
            {
                if (toyGameObject.toyData.ID == toyID)
                {
                    return toyGameObject;
                }
            }
            return null;
        }

        public void ShowAvailable(BagItemData data)
        {
            MiddleLayer.ShowAvailable(data);
        }

        public void ClosePointMark()
        {
            MiddleLayer.ClosePointMark();
            PanelManager.InfoBar.SetInfoBar();
        }

        public void OnClickHandler(PointerEventData eventData)
        {
            delayController.StopDelayInvoke();

            if (isMoved)
            {
                return;
            }

            touchClickModeHandler(eventData);
        }

        private void touchClickModeHandler(PointerEventData eventData)
        {
            Debug.Log(eventData.pointerCurrentRaycast);
            GameObject currentTargetGameObjec = eventData.pointerCurrentRaycast.gameObject;
            string name = currentTargetGameObjec.name;

            if (name == "Toy")
            {
                ToyGameObject toyGo = FindToyGameObjectByGameObject(currentTargetGameObjec.transform.parent.transform.parent.gameObject);
                GameObject toyRoot = toyGo.Root;
                RaycastHit2D hit;
                if (toyRoot.GetComponent<PolygonCollider2D>() != null)
                {
                    //空白像素太多的物体，用射线辅助检测
                    hit = Physics2D.Raycast(new Vector3(eventData.position.x, eventData.position.y, 0), Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.name.Contains("ToyRoot_"))
                        {
                            Debug.Log(string.Format("点到了玩具 {0}", toyGo.toyData.Name));

                            return;
                        }
                        else if (hit.collider.gameObject.name.Contains("catani"))
                        {
                            //点了玩具后层的猫或者玩具里面的猫
                            Model.Data.CatData catData = FindCatGameObjectByGameObject(hit.collider.gameObject).CatData;

                            clickCat(catData, hit.collider.gameObject);

                            return;
                        }
                    }
                }
                else
                {

                    hit = Physics2D.Raycast(new Vector3(eventData.position.x, eventData.position.y, 0), Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.name.Contains("catani"))
                        {
                            //点了玩具后层的猫或者玩具里面的猫
                            Model.Data.CatData catData = FindCatGameObjectByGameObject(hit.collider.gameObject).CatData;

                            clickCat(catData, hit.collider.gameObject);

                            return;
                        }
                    }

                    Debug.Log(string.Format("点到了玩具 {0}", toyGo.toyData.Name));
                }

            }
            else if (name == "Available")
            {
                ToyMarkPointGameObject markPoint = FindToyMarkPointGameObjectByGameObject(currentTargetGameObjec.transform.parent.gameObject);
                ScenePointStruct pointStruct = markPoint.PointStruct;

                Debug.Log(string.Format("点击了第 {0} 个场景点的中的 {1}", pointStruct.ScenePointIndex, pointStruct.SubPointIndex));

                if (BagManager.Instance.CurrentSelectedCellInBag != null)
                {
                    CommandHandle.Send(ServerMethod.PlaceObject, new PutObjectRequest()
                    { Id = BagManager.Instance.CurrentSelectedCellInBag.ID, ScenePointIndex = pointStruct.ScenePointIndex - 1, SubPointIndex = pointStruct.SubPointIndex, RoomSectionType = (RoomSectionType)((int)pointStruct.Distribution) });

                    BagManager.Instance.CurrentSelectedCellInBag = null;

                    SoundUtil.PlaySound(ResPathConfig.PLACE_TOY);
                }
            }
            else if (name.Contains("catani") && name.Contains("drag_") == false)
            {
                Model.Data.CatData catData = FindCatGameObjectByGameObject(currentTargetGameObjec).CatData;

                clickCat(catData, currentTargetGameObjec);
            }
            else if (name.Contains("Food"))
            {
                FoodPointGameObject foodPointGO = FindFoodPointGameObjectByGameObject(currentTargetGameObjec.transform.parent.gameObject);
                ScenePointStruct pointStruct = foodPointGO.pointStruct;
                Debug.Log(string.Format("点击了第 {0} 个食物点", pointStruct.ScenePointIndex));

                if (BagManager.Instance.CurrentSelectedCellInBag != null)
                {
                    CommandHandle.Send(ServerMethod.PlaceObject, new PutObjectRequest()
                    { Id = BagManager.Instance.CurrentSelectedCellInBag.ID, ScenePointIndex = pointStruct.ScenePointIndex - 1, SubPointIndex = pointStruct.SubPointIndex, RoomSectionType = (RoomSectionType)((int)pointStruct.Distribution) });

                    BagManager.Instance.CurrentSelectedCellInBag = null;

                    SoundUtil.PlaySound(ResPathConfig.PLACE_FOOD);
                }
            }
            else if (currentTargetGameObjec.transform.parent.name.Contains("Food"))
            {
                FoodGameObject foodGO = FindFoodGameObjectByGameObject(currentTargetGameObjec.transform.parent.gameObject);
                ScenePointStruct pointStruct = foodGO.PlaceIndexStruct;
                AutoMoveFoodToCenter(foodGO.FoodData, foodGO);
                
                if (GuideManager.Instance.IsGestureTouchEffective("Scene001(Clone)/MiddleLayer/Content/InteractivePoints/FoodPoint_1"))
                {
                    GuideManager.Instance.ContinueGuide();
                }
            }
            else
            {
                Debug.Log(string.Format("点到了非交互物体 {0}", name));
            }
        }

        private void touchDownModeHandler(PointerEventData eventData)
        {
            //Debug.Log(eventData.pointerId + " " + Input.touchCount);

            if (draggingCatGO != null)
            {
                return;
            }

            isTouchedCat = false;
            lastGragCat = null;

            GameObject currentTargetGameObjec = eventData.pointerCurrentRaycast.gameObject;
            string name = currentTargetGameObjec.name;

            if (name == "Toy")
            {
                ToyGameObject toyGo = FindToyGameObjectByGameObject(currentTargetGameObjec.transform.parent.transform.parent.gameObject);
                GameObject toyRoot = toyGo.Root;
                RaycastHit2D hit;
                if (toyRoot.GetComponent<PolygonCollider2D>() != null)
                {
                    //空白像素太多的物体，用射线辅助检测
                    hit = Physics2D.Raycast(new Vector3(eventData.position.x, eventData.position.y, 0), Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.name.Contains("catani"))
                        {
                            isTouchedCat = true;

                            //点了玩具后层的猫或者玩具里面的猫
                            Model.Data.CatData catData = FindCatGameObjectByGameObject(hit.collider.gameObject).CatData;

                            dragCat(catData);

                            return;
                        }
                    }
                }
                else
                {
                    hit = Physics2D.Raycast(new Vector3(eventData.position.x, eventData.position.y, 0), Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.name.Contains("catani"))
                        {
                            isTouchedCat = true;

                            //点了玩具后层的猫或者玩具里面的猫
                            Model.Data.CatData catData = FindCatGameObjectByGameObject(hit.collider.gameObject).CatData;

                            dragCat(catData);

                            return;
                        }
                    }
                }

            }
            else if (name.Contains("catani"))
            {
                isTouchedCat = true;

                Model.Data.CatData catData = FindCatGameObjectByGameObject(currentTargetGameObjec).CatData;

                dragCat(catData);
            }
        }

        private void driveCat()
        {
            if (lastGragCat != null)
            {
                ToyGameObject toyGameObject = FindToyGameObjectByID(lastGragCat.ToyID);

                if (draggingCatGO != null)
                {
                    float distance = Vector3.Distance(draggingCatGO.transform.position, toyGameObject.Root.transform.position);

                    UnityEngine.Object.DestroyImmediate(draggingCatGO);
                    draggingCatGO = null;

                    if (distance > 170)
                    {
                        showLeaveTip = true;
                        CommandHandle.Send(ServerMethod.KickCat, new IDMessage() { Id = lastGragCat.ID });
                    }
                    else
                    {
                        toyGameObject.ReShowCat(lastGragCat.CatPath);
                    }

                    //toyGameObject.ReShowCat(lastGragCat.CatPath);
                }
            }
        }

        private void clickCat(Model.Data.CatData catData, GameObject catGO)
        {
            Debug.Log(string.Format("点击了猫 {0} {1}", catData.Name, catData.CatSpineName));

            if (PanelManager.IsCurrentPanel(PanelManager.takePicturePanel))
            {
                PanelManager.takePicturePanel.ShowPhotoController(catGO, catData);
            }
            else
            {
                ConfigUtil.GetConfig(ResPathConfig.CATS_CONFIG, catData.Type, (config) =>
                {
                    SoundUtil.PlaySound(config["Sound"].ToString());
                });

                //SoundUtil.PlaySound(ResPathConfig.CLICK_CAT);
                HandbookData data = HandbookManager.Instance.FindOneCatByType(catData.Type);
                PanelManager.CatPopupPanel.Show(() =>
                {
                    PanelManager.catPopupPanel.SetData(data);
                });
            }
        }

        private void dragCat(Model.Data.CatData catData)
        {
            if (PanelManager.IsCurrentPanel(PanelManager.takePicturePanel))
            {
                return;
            }

            delayController.DelayInvoke(() =>
            {
                if (isTouchedCat)
                {
                    isStopDragScene = true;

                    lastGragCat = catData;

                    string assetName = "drag_" + catData.CatSpineName.ToLower();
                    ResourceLoader.Instance.TryLoadClone(assetName, assetName, (go) =>
                    {
                        ConfigUtil.GetConfig(ResPathConfig.CATS_CONFIG, catData.Type, (config) =>
                        {
                            SoundUtil.PlaySound(config["Sound"].ToString());
                        });

                        ToyGameObject toyGameObject = FindToyGameObjectByID(catData.ToyID);
                        toyGameObject.HideCat(catData.CatPath);

                        draggingCatGO = go;
                        go.SetActive(false);
                        go.transform.SetParent(Scene.transform, false);
                        updateDraggingCatGOPosition();

                        SkeletonGraphic sg = go.GetComponent<SkeletonGraphic>();
                        sg.AnimationState.SetAnimation(0, "animation1", true);

                        go.SetActive(true);

                        //canPlayingDraggingEffect = true;
                    });
                }
            }, 0.5f);
        }

        private void updateDraggingCatGOPosition()
        {
            if (draggingCatGO != null)
            {
                draggingCatGO.transform.position = Input.mousePosition;
            }
        }

        //private void playDraggingEffect()
        //{
        //    if (draggingCatGO != null && canPlayingDraggingEffect)
        //    {
        //        canPlayingDraggingEffect = false;
        //        float startAngle = 60 * dir;
        //        rotationAnimation(startAngle, dir);
        //    }
        //}

        //private void rotationAnimation(float angle, float startDir)
        //{
        //    if (Mathf.Abs(angle)>0)
        //    {
        //        float time = 0.4f * Mathf.Abs(angle) / 60;
        //        time = 0.2f;
        //        Tweener tweener = draggingCatGO.transform.DORotate(new Vector3(0, 0, angle), time);
        //        tweener.OnComplete(() =>
        //        {
        //            tweener = draggingCatGO.transform.DORotate(new Vector3(0, 0, -angle), time);
        //            tweener.OnComplete(() => { rotationAnimation(angle - 10 * startDir, startDir); });
        //        });
        //    }
        //    else
        //    {
        //        Tweener tweener = draggingCatGO.transform.DORotate(new Vector3(0, 0, 0), 0.3f);
        //    }
        //}

        public virtual void OnDownHandler(PointerEventData eventData)
        {
            isMoved = false;
            isStopDragScene = false;

            touchDownModeHandler(eventData);

            if (onDownCallback != null)
            {
                onDownCallback(eventData);
            }

            lastPointerPosX = eventData.position.x;
            startDragPosX = lastPointerPosX;
            frontLayerStartDragPos = FrontLayer.Layer.transform.localPosition;
            middleLayerStartDragPos = MiddleLayer.Layer.transform.localPosition;
            backLayerStartDragPos = BackLayer.Layer.transform.localPosition;

            downTime = Time.time;
            downPosX = eventData.position.x;
            easingSpeed = 0;
        }

        public virtual void OnUpHandler(PointerEventData eventData)
        {
            driveCat();

            if (isStopDragScene || SceneManager.Instance.IsOpenFullSize == false)
            {
                return;
            }

            if (onUpCallback != null)
            {
                onUpCallback(eventData);
            }

            easingSpeed = (eventData.position.x - downPosX) / (Time.time - downTime);

            if (easingSpeed > 0)
            {
                easingSpeed = Mathf.Clamp(easingSpeed, minSpeed, maxSpeed);
            }
            else if (easingSpeed < 0)
            {
                easingSpeed = Mathf.Clamp(easingSpeed, -maxSpeed, -minSpeed);
            }

            if (Mathf.Abs(easingSpeed) > 0 && (Time.time - holdOnTime) < 0.05)
            {
                useEasing = true;
            }
            else
            {
                useEasing = false;
            }
        }

        public void OnDragHandler(PointerEventData eventData)
        {
            if (eventData.position.x - lastPointerPosX > 0)
            {
                //向右
                //Debug.Log("right");
                dir = 1;
            }
            else if (eventData.position.x - lastPointerPosX < 0)
            {
                //向左
                //Debug.Log("left");
                dir = -1;
            }
            else
            {
                dir = 0;
            }

            //猫的拖拽
            updateDraggingCatGOPosition();
            //playDraggingEffect();

            if (isStopDragScene || SceneManager.Instance.IsOpenFullSize == false)
            {
                return;
            }

            delayController.StopDelayInvoke();

            lastPointerPosX = eventData.position.x;

            distence = eventData.position.x - startDragPosX;
            onScrolling();
        }

        private void onScrolling()
        {
            isMoved = true;

            middleLayerPos.x = middleLayerStartDragPos.x + distence * middleSpeed;

            if (middleLayerPos.x>= -middleMScrollerX && middleLayerPos.x<=0)
            {
                frontLayerPos.x = frontLayerStartDragPos.x + distence * frontSpeed;
                frontLayerPos.x = Mathf.Clamp(frontLayerPos.x, -frontMScrollerX, Math.Max(currentLimitScrollingX, -frontMScrollerX));
                FrontLayer.Layer.transform.localPosition = frontLayerPos;

                backLayerPos.x = backLayerStartDragPos.x + distence * backSpeed;
                backLayerPos.x = Mathf.Clamp(backLayerPos.x, -backMScrollerX, Math.Max(currentLimitScrollingX, -backMScrollerX));
                BackLayer.Layer.transform.localPosition = backLayerPos;
            }

            middleLayerPos.x = Mathf.Clamp(middleLayerPos.x, -middleMScrollerX, Math.Max(currentLimitScrollingX, -middleMScrollerX));
            MiddleLayer.Layer.transform.localPosition = middleLayerPos;

            holdOnTime = Time.time;
        }

        private void easingScrolling()
        {
            if (useEasing)
            {
                easingSpeed = Mathf.Lerp(easingSpeed, 0, (Time.time - downTime)*0.5f);
                backLayerPos.x = backLayerPos.x + backSpeed * easingSpeed;
                backLayerPos.x = Mathf.Clamp(backLayerPos.x, -backMScrollerX, Math.Max(currentLimitScrollingX, -backMScrollerX));
                BackLayer.Layer.transform.localPosition = backLayerPos;

                middleLayerPos.x = middleLayerPos.x + middleSpeed * easingSpeed;
                middleLayerPos.x = Mathf.Clamp(middleLayerPos.x, -middleMScrollerX, Math.Max(currentLimitScrollingX, -middleMScrollerX));
                MiddleLayer.Layer.transform.localPosition = middleLayerPos;

                frontLayerPos.x = frontLayerPos.x + frontSpeed * easingSpeed;
                frontLayerPos.x = Mathf.Clamp(frontLayerPos.x, -frontMScrollerX, Math.Max(currentLimitScrollingX, -frontMScrollerX));
                FrontLayer.Layer.transform.localPosition = frontLayerPos;

                if (Mathf.Abs(easingSpeed) < 0.1)
                {
                    useEasing = false;
                }
            }
        }

        private void stopOnRight(float rightEdge)
        {
            BackLayer.Layer.transform.localPosition = new Vector3(-backMScrollerX + rightEdge * 0.8f, 0, 0);
            MiddleLayer.Layer.transform.localPosition = new Vector3(-middleMScrollerX + rightEdge, 0, 0);
            FrontLayer.Layer.transform.localPosition = new Vector3(-frontMScrollerX + rightEdge * 1.2f, 0, 0);
        }

        public void AutoMoveFoodToCenter(Model.Data.FoodData data, FoodGameObject food)
        {
            float x = Screen.width / 2 - food.Root.transform.position.x;
            x = x * 1136 / Screen.height;

            //Debug.Log(middleMScrollerX + " " + middleLayerPos.x + x);

            float bx = Mathf.Max(Mathf.Min(0, backLayerPos.x + x), -backMScrollerX);
            float mx = Mathf.Max(Mathf.Min(0, middleLayerPos.x + x), -middleMScrollerX);
            float fx = Mathf.Max(Mathf.Min(0, frontLayerPos.x + x), -frontMScrollerX);

            foodMoveXBackLayer = backLayerPos.x - bx;
            foodMoveXMiddleLayer = middleLayerPos.x - mx;
            foodMoveXFrontLayer = frontLayerPos.x - fx;

            Tweener backTween = BackLayer.Layer.transform.DOLocalMove(new Vector3(bx, backLayerPos.y, backLayerPos.z), 0.3f);
            backTween.OnComplete(() =>
            {
                backLayerPos = BackLayer.Layer.transform.localPosition;
            });

            Tweener middleTween = MiddleLayer.Layer.transform.DOLocalMove(new Vector3(mx, middleLayerPos.y, middleLayerPos.z), 0.3f);
            middleTween.OnComplete(() =>
            {
                middleLayerPos = MiddleLayer.Layer.transform.localPosition;
                PanelManager.FoodPopupPanel.Show(() => { PanelManager.foodPopupPanel.SetData(data, food); });
            });

            Tweener frontTween = FrontLayer.Layer.transform.DOLocalMove(new Vector3(fx, frontLayerPos.y, frontLayerPos.z), 0.3f);
            frontTween.OnComplete(() =>
            {
                frontLayerPos = FrontLayer.Layer.transform.localPosition;
            });
        }

        public void AutoRestoreFoodPos()
        {
            Tweener backTween = BackLayer.Layer.transform.DOLocalMove(new Vector3(backLayerPos.x + foodMoveXBackLayer, backLayerPos.y, backLayerPos.z), 0.3f);
            backTween.OnComplete(() =>
            {
                backLayerPos = BackLayer.Layer.transform.localPosition;
            });

            Tweener middleTween = MiddleLayer.Layer.transform.DOLocalMove(new Vector3(middleLayerPos.x + foodMoveXMiddleLayer, middleLayerPos.y, middleLayerPos.z), 0.3f);
            middleTween.OnComplete(() =>
            {
                middleLayerPos = MiddleLayer.Layer.transform.localPosition;
            });

            Tweener frontTween = FrontLayer.Layer.transform.DOLocalMove(new Vector3(frontLayerPos.x + foodMoveXFrontLayer, frontLayerPos.y, frontLayerPos.z), 0.3f);
            frontTween.OnComplete(() =>
            {
                frontLayerPos = FrontLayer.Layer.transform.localPosition;
            });
        }

        public CatGameObject FindCatGameObjectByGameObject(GameObject go)
        {
            foreach (var catGameObject in CatGameObjectList)
            {
                if (catGameObject.CatSpine == go)
                {
                    return catGameObject;

                }
            }

            return null;
        }

        public ToyGameObject FindToyGameObjectByGameObject(GameObject go)
        {
            foreach (var toyGameObject in toyGameObjectList)
            {
                if (toyGameObject.Root == go)
                {
                    return toyGameObject;
                }
            }

            return null;
        }

        public FoodPointGameObject FindFoodPointGameObjectByGameObject(GameObject go)
        {
            foreach (var foodPointGameObject in MiddleLayer.foodPointGameObjectsList)
            {
                if (foodPointGameObject.Root == go)
                {
                    return foodPointGameObject;

                }
            }

            return null;
        }

        public FoodGameObject FindFoodGameObjectByGameObject(GameObject go)
        {
            foreach (var foodGameObject in foodGameObjectList)
            {
                if (go == foodGameObject.Root)
                {
                    return foodGameObject;
                }
            }

            return null;
        }

        public ToyMarkPointGameObject FindToyMarkPointGameObjectByGameObject(GameObject go)
        {
            List<ToyPointGameObject> toyPoints = MiddleLayer.toyPointGameObjectsList;

            foreach (var toyPoint in toyPoints)
            {
                foreach (var markPoint in toyPoint.SmallMarkLsit)
                {
                    if (go == markPoint.Value.Root)
                    {
                        return markPoint.Value;
                    }
                }
            }

            foreach (var toyPoint in toyPoints)
            {
                if (toyPoint.LargeMark != null && go == toyPoint.LargeMark.Root)
                {
                    return toyPoint.LargeMark;
                }
            }

            return null;
        }

        public void ShowCatAnchorImage(bool isShow)
        {
            GameObject tempAnchorImage = null;
            Transform anchorImageContainer = MiddleLayer.Layer.transform.Find("CameraAnchorIamge");

            if (isShow)
            {
                foreach (var catGO in CatGameObjectList)
                {
                    //toyRoot = catGO.ToyGameObject.Root;
                    //toyRoot.SetActive(false);


                    if (anchorImageContainer == null)
                    {
                        GameObject anchorGO = new GameObject("CameraAnchorIamge");
                        anchorImageContainer = anchorGO.transform;
                        anchorGO.transform.SetParent(MiddleLayer.Layer.transform, true);
                    }
                    anchorImageContainer.SetAsLastSibling();

                    tempAnchorImage = GameObject.Instantiate(TakePicturePanel.AnchorImage);
                    tempAnchorImage.SetActive(true);
                    tempAnchorImage.transform.SetParent(anchorImageContainer, true);

                    GameObject catSpine = catGO.CatSpine;
                    Vector3 pos = new Vector3(catSpine.transform.position.x, catSpine.transform.position.y + catSpine.GetComponent<RectTransform>().sizeDelta.y / 3f, 0);
                    tempAnchorImage.transform.position = pos;
                }
            }
            else
            {
                if (anchorImageContainer != null)
                {
                    int count = anchorImageContainer.childCount;
                    for (int i = 0; i < count; i++)
                    {
                        GameObject.DestroyImmediate(anchorImageContainer.GetChild(0).gameObject);
                    }
                }
            }
        }
    }

}
