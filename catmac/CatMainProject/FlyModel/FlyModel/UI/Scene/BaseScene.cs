using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI.Panel.LoadingPanel;
using FlyModel.Utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlyModel.UI.Scene
{
    public abstract class BaseScene : DisposeObject
    {

        public string SceneBundleName;
        public string SceneAssetName;

        public virtual bool ShowOnLoaded
        {
            get
            {
                return true;
            }
        }

        public GameObject SceneRoot;
        public RectTransform Transform;

        protected bool loaded = false;

        public SceneGameObject SceneGameObject;

        public BaseScene()
        {
            
        }

        public void StartShow(string sceneAseetName)
        {
            SceneBundleName = sceneAseetName.ToLower();
            SceneAssetName = sceneAseetName;

            //loadRes();
            CreateLoadingInfos();
        }

        private void CreateLoadingInfos()
        {
            LoadingQueue loadingQueue = PanelManager.loadingPanel.LoadingQueue;


            //背景音乐
            LoadingInfo bgSoundInfo = new LoadingInfo();
            bgSoundInfo.Type = EnumConfig.LoadingType.Sound;
            bgSoundInfo.AssetType = typeof(AudioClip);
            bgSoundInfo.AssetName = ResPathConfig.SOUND_BG;
            bgSoundInfo.LoadOverCallback = (sound) => {
                Debug.Log("===load over 背景音乐加载完毕");

                SoundUtil.PlayMusic(sound as AudioClip, true);
            };
            loadingQueue.AddLoadingInfo(bgSoundInfo);

            //猫
            LoadingInfo catInfo;
            List<Model.Data.CatData> catDatas = CatManager.Instance.CatDataList;
            int count = catDatas.Count;
            for (int i = 0; i < count; i++)
            {
                catInfo = new LoadingInfo();
                catInfo.Type = EnumConfig.LoadingType.Cat;
                catInfo.AssetType = typeof(GameObject);
                catInfo.AssetName = catDatas[i].CatSpineName;
                catInfo.LoadOverCallback = (go) => { Debug.Log(string.Format("===load over 猫 {0} 加载完毕", go.name)); };
                loadingQueue.AddLoadingInfo(catInfo);
            }

            //玩具
            LoadingInfo toyInfo;
            List<ToyData> toyDatas = SceneGameObjectManager.Instance.ToyDataList; //SceneManager.Instance.CurrentSceneData.ToyDatas;
            count = toyDatas.Count;
            for (int i = 0; i < count; i++)
            {
                toyInfo = new LoadingInfo();
                toyInfo.Type = EnumConfig.LoadingType.Toy;
                toyInfo.AssetType = typeof(GameObject);
                toyInfo.AssetName = toyDatas[i].Name;
                toyInfo.LoadOverCallback = (go) => { Debug.Log(string.Format("===load over 玩具 {0} 加载完毕", go.name)); };
                loadingQueue.AddLoadingInfo(toyInfo);
            }

            //场景
            LoadingInfo sceneInfo = new LoadingInfo();
            sceneInfo.Type = EnumConfig.LoadingType.Scene;
            sceneInfo.AssetType = typeof(GameObject);
            sceneInfo.AssetName = SceneAssetName;
            sceneInfo.LoadOverCallback = (go) => {
                Debug.Log("===load over 场景加载完毕");

                //场景不会重用，不需要clone
                SceneRoot = UnityEngine.Object.Instantiate(go as GameObject);
                Transform = SceneRoot.GetComponent<RectTransform>();
                SceneRoot.transform.SetParent(GameMain.SceneRoot.transform, false);
                SceneRoot.SetActive(true);
            };
            loadingQueue.AddLoadingInfo(sceneInfo);

            loadingQueue.RegisterCompleteCallback(() => {
                Debug.Log("===========全部资源加载完毕===========");
                initializeAndShowGameObject();
            });

            loadingQueue.StartLoad();
        }

        private void initializeAndShowGameObject()
        {
            initializeSceneGameObject();
            initializeToyGameObject();
            initializeCatGameObject();
            initializeFoodGameObject();
            Initialize();
            InitializeOver();

            loaded = true;

            if (ShowOnLoaded)
            {
                Show();
            }
        }

        /// <summary>
        /// 无loading模式
        /// </summary>
        void loadRes()
        {
            ResourceLoader.Instance.TryLoadClone(SceneBundleName, SceneAssetName, (go) =>
            {
                //Debug.Log(string.Format("Load {0} {1}", SceneBundleName, SceneAssetName));
                go.transform.SetParent(GameMain.SceneRoot.transform, false);

                SceneRoot = go;
                Transform = go.GetComponent<RectTransform>();

                initializeAndShowGameObject();
            });
        }

        public virtual void Show()
        {
            if (loaded)
            {
                SceneRoot.SetActive(true);
                Transform.SetAsLastSibling();

                //PanelManager.LoadingPanel.Close();
                PanelManager.loadingPanel.Close();


            }
        }

        protected virtual void Initialize() { }

        protected virtual void InitializeOver() { }

        public override void Dispose()
        {
            
        }

        private void initializeSceneGameObject()
        {
            SceneGameObject = new SceneGameObject(SceneRoot);

            SceneGameObject.onUpCallback = OnUpHandler;
            SceneGameObject.onDownCallback = OnDownHandler;
        }

        protected virtual void initializeToyGameObject()
        {
            List<ToyData> toyDatas = SceneGameObjectManager.Instance.ToyDataList;
            int count = toyDatas.Count;
            for (int i = 0; i < count; i++)
            {
                SceneGameObject.AddOneToyGameObject(toyDatas[i]);
            }

            showInSceneToys();
        }

        protected virtual void showInSceneToys()
        {
            List<ToyGameObject> inSceneToys = SceneGameObject.GetAllInSceneToyGameObject();
            foreach (var toy in inSceneToys)
            {
                toy.ShowToy();
            }
        }

        protected virtual void initializeCatGameObject()
        {
            List<Model.Data.CatData> catDatas = CatManager.Instance.CatDataList;
            int count = catDatas.Count;
            for (int i = 0; i < count; i++)
            {
                SceneGameObject.AddOneCatGameObject(catDatas[i]);
            }

            showInSceneCats();
        }

        protected virtual void showInSceneCats()
        {
            List<CatGameObject> inSceneCats = SceneGameObject.GetAllInSceneCatGameObject();
            foreach (var catData in inSceneCats)
            {
                catData.ShowCat();
            }
        }

        protected virtual void initializeFoodGameObject()
        {
            List<Model.Data.FoodData> foodDatas = SceneGameObjectManager.Instance.FoodDataList;
            if (foodDatas != null)
            {
                int count = foodDatas.Count;
                for (int i = 0; i < count; i++)
                {
                    SceneGameObject.AddOneFoodGameObjet(foodDatas[i]).ShowFood();
                }
            }
        }

        public virtual void OnUpHandler(PointerEventData eventData)
        {
            //Debug.Log("Base Scene Up");
        }

        public virtual void OnDownHandler(PointerEventData eventData)
        {
            //Debug.Log("Base Scene Down");
        }
    }
}
