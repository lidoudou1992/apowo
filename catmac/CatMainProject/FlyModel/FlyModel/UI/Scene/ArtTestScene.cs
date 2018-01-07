using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Component;
using FlyModel.UI.Scene.Data;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Scene
{
    public class ArtTestScene : BaseScene
    {
        private Text currentAnimNameTF;
        private int animNums;
        private ToyData toyData;
        private List<AnimInfo> animationInfoList = new List<AnimInfo>();

        public CatGameObject CurrentCatSpine;

        protected override void initializeToyGameObject()
        {
            toyData = new ToyData();
            toyData.Name = string.Format("ToyRoot_Toy{0}", GameTestConfig.ToyCode);
            toyData.ID = 9999;

            toyData.ScenePointIndex.ScenePointIndex = 1;
            toyData.ScenePointIndex.SubPointIndex = 1;
            toyData.ScenePointIndex.PointType = EnumConfig.SubPointType.small;

            SceneGameObject.AddOneToyGameObject(toyData);


            showInSceneToys();
        }

        protected override void initializeCatGameObject()
        {
            
        }

        private void showCat(AnimInfo animInfo)
        {
            CatData catData = new CatData();
            catData.CatSpineName = animInfo.catName;
            catData.ToyID = 9999;

            //ToyStructInfo path = new ToyStructInfo();
            //path.ToyLayerIndex = animInfo.catPath.ToyLayerIndex;
            //path.PlayPointLayerIndex = animInfo.catPath.PlayPointLayerIndex;
            //path.PointIndex = animInfo.catPath.PointIndex;
            catData.CatPath = animInfo.catPath;

            if (CurrentCatSpine != null)
            {
                UnityEngine.Object.DestroyImmediate(CurrentCatSpine.Root);
                CurrentCatSpine = null;
            }
            SceneManager.Instance.CurrentScene.SceneGameObject.CatGameObjectList.Clear();

            CurrentCatSpine = SceneGameObject.AddOneCatGameObject(catData);

            showInSceneCats();
        }

        protected override void InitializeOver()
        {
            base.InitializeOver();

            loadConfig();
        }

        private void initUI()
        {
            GameObject UIRoot = GameMain.UI2DRoot.transform.gameObject;

            //GameObject nextBtn = new GameObject("next", typeof(Image), typeof(Button));
            //ResourceLoader.Instance.TryLoadPic(ResPathConfig.TEST_UI_ASSETBUNDLE, ResPathConfig.TEST_UI_NEXT_BUTTON, (s) => {
            //    nextBtn.GetComponent<Image>().sprite = s as Sprite;
            //});
            //nextBtn.transform.SetParent(UIRoot.transform, false);
            //nextBtn.transform.localPosition = new Vector3(200, 400, 0);
            //nextBtn.GetComponent<Button>().onClick.AddListener(nextAnimation);

            //GameObject prevBtn = new GameObject("prev", typeof(Image), typeof(Button));
            //ResourceLoader.Instance.TryLoadPic(ResPathConfig.TEST_UI_ASSETBUNDLE, ResPathConfig.TEST_UI_NEXT_BUTTON, (s) => {
            //    prevBtn.GetComponent<Image>().sprite = s as Sprite;
            //});
            //prevBtn.transform.SetParent(UIRoot.transform, false);
            //prevBtn.transform.localPosition = new Vector3(-200, 400, 0);
            //prevBtn.transform.localScale = new Vector3(-1, 1, 1);
            //prevBtn.GetComponent<Button>().onClick.AddListener(prevAnimation);

            //GameObject currentAnimName = new GameObject("animationName", typeof(Text));
            //currentAnimName.transform.SetParent(UIRoot.transform, false);
            //currentAnimName.transform.localPosition = new Vector3(0, 400, 0);
            //currentAnimName.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 40);
            //currentAnimNameTF = currentAnimName.GetComponent<Text>();
            //Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            //currentAnimNameTF.font = ArialFont;//new Font("Arial");
            //currentAnimNameTF.fontSize = 24;
            //currentAnimNameTF.text = "当前显示动作为";
            //currentAnimNameTF.alignment = TextAnchor.MiddleCenter;

            GameObject animationList = new GameObject("animationList", typeof(ScrollRect), typeof(Image));
            RectTransform scrollviewRT = animationList.GetComponent<RectTransform>();
            scrollviewRT.sizeDelta = new Vector2(400, 400);
            scrollviewRT.pivot = new Vector2(0f, 1f);
            animationList.transform.SetParent(UIRoot.transform, false);
            animationList.transform.localPosition = new Vector3(-200, 350, 0);

            ScrollRect scroller = animationList.GetComponent<ScrollRect>();
            scroller.horizontal = false;
            animationList.AddComponent<Mask>();

            animNums = animationInfoList.Count;

            GameObject content = new GameObject("content", typeof(Image));
            RectTransform contentTF = content.GetComponent<RectTransform>();
            contentTF.sizeDelta = new Vector2(400, 40 * animNums);
            contentTF.SetParent(animationList.transform, false);
            contentTF.anchorMin = new Vector2(0, 1);
            contentTF.anchorMax = new Vector2(0, 1);
            contentTF.pivot = new Vector2(0, 1f);
            contentTF.localPosition = new Vector3(0, 0, 0);
            scroller.content = content.GetComponent<RectTransform>();

            content.AddComponent<VerticalLayoutGroup>().childForceExpandHeight = true;

            for (int i = 0; i < animationInfoList.Count; i++)
            {
                new AnimationBtn(content, animationInfoList[i], showCat);
            }
        }

        private void changeAnimation()
        {

        }

        private void nextAnimation()
        {
            //showCat(info);
            //cat = (SceneManager.Instance.CurrentScene as ArtTestScene).CurrentCatSpine;

            //delayController.DelayInvoke(() =>
            //{
            //    SkeletonGraphic spine = cat.Root.GetComponent<SkeletonGraphic>();
            //    spine.AnimationState.ClearTracks();
            //    spine.AnimationState.SetAnimation(0, info.animationName, true);
            //}, 0.5f);
        }

        private void prevAnimation()
        {

        }

        private void loadConfig()
        {
            string toyConfigName = string.Format("{0}Config", toyData.Name);
            ResourceLoader.Instance.TryLoadTextAsset(toyConfigName, (textAssert) =>
            {
                JsonData config = JsonMapper.ToObject((textAssert as TextAsset).text);
                parseConfig(config);
                initUI();
            });
        }

        private void parseConfig(JsonData config)
        {
            JsonData toyLayer;
            for (int toyLayerIndex = 0; toyLayerIndex < config["ToyLayers"].Count; toyLayerIndex++)
            {
                toyLayer = config["ToyLayers"][toyLayerIndex];

                JsonData playPointLayer;
                for (int playPointLayerIndex = 0; playPointLayerIndex < toyLayer["PlayPointLayers"].Count; playPointLayerIndex++)
                {
                    playPointLayer = toyLayer["PlayPointLayers"][playPointLayerIndex];

                    JsonData point;
                    for (int pointIndex = 0; pointIndex < playPointLayer["Points"].Count; pointIndex++)
                    {
                        point = playPointLayer["Points"][pointIndex];

                        JsonData cat;
                        for (int catIndex = 0; catIndex < point["Cats"].Count; catIndex++)
                        {
                            cat = point["Cats"][catIndex];

                            JsonData catAnim;
                            AnimInfo tempInfo;
                            ToyStructInfo tempPath;
                            for (int animIndex = 0; animIndex < cat["Animations"].Count; animIndex++)
                            {
                                catAnim = cat["Animations"][animIndex];

                                tempInfo = new AnimInfo();
                                tempInfo.catName = cat["CatName"].ToString();
                                tempInfo.animationName = catAnim["AnimationName"].ToString();

                                tempPath = new ToyStructInfo();
                                tempPath.ToyLayerIndex = toyLayerIndex+1;
                                tempPath.PlayPointLayerIndex = playPointLayerIndex+1;
                                tempPath.PointIndex = pointIndex+1;

                                tempInfo.catPath = tempPath;

                                animationInfoList.Add(tempInfo);
                            }
                        }
                    }
                }
            }

            Debug.Log(animationInfoList.Count);
        }
    }

    internal class AnimationBtn
    {
        private GameObject BtnRoot;
        private Text tf;

        private CatGameObject cat;
        private AnimInfo info;

        private Action<AnimInfo> showCat;

        private DelayController delayController;

        public GameObject BtnRoot1
        {
            get
            {
                return BtnRoot;
            }

            set
            {
                BtnRoot = value;
            }
        }

        public AnimationBtn(GameObject parent, AnimInfo info, Action<AnimInfo> showCat)
        {
            BtnRoot1 = new GameObject("animationBtn", typeof(Button), typeof(Text));

            tf = BtnRoot1.GetComponent<Text>();
            tf.text = string.Format("{0} {1}", info.catName, info.animationName);
            tf.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            tf.alignment = TextAnchor.MiddleCenter;
            tf.color = Color.red;
            tf.fontSize = 24;

            Button btn = BtnRoot1.GetComponent<Button>();
            btn.onClick.AddListener(playAnimation);

            BtnRoot1.transform.SetParent(parent.transform, false);

            LayoutElement lye = BtnRoot1.AddComponent<LayoutElement>();
            lye.preferredHeight = 60;

            this.info = info;
            this.showCat = showCat;

            delayController = new DelayController(); //BtnRoot1.AddComponent<DelayController>();
        }

        private void playAnimation()
        {
            showCat(info);
            cat = (SceneManager.Instance.CurrentScene as ArtTestScene).CurrentCatSpine;

            delayController.DelayInvoke(() =>
            {
                SkeletonGraphic spine = cat.CatSpine.GetComponent<SkeletonGraphic>();
                spine.AnimationState.ClearTracks();
                spine.AnimationState.SetAnimation(0, info.animationName, true);
            }, 0.5f);
        }
    }

    internal class AnimInfo
    {
        public string animationName;
        public string catName;
        public ToyStructInfo catPath;

        public AnimInfo()
        {

        }
    }
}
