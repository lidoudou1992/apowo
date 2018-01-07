using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Scene.Data;
using FlyModel.UI.Scene.ViewObject;
using FlyModel.UI.Scene.ViewObject.Data;
using FlyModel.UI.Scene.ViewObject.SceneViewObject;
using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace FlyModel.UI.Scene
{
    public class ToyGameObject
    {
        public static string[] TOY_CONFIG_KEY_DEPTH = new string[] {"", "ToyLayers", "PlayPointLayers", "Points", "Cats", "Animations"};

        public string ToyName;
        public ScenePointStruct PlaceIndexStruct;
        public GameObject Root;

        public bool IsInScene;

        public JsonData ToyConfig;

        private Dictionary<string, ToyLayerGameObject> toyLayerGameObjectList = new Dictionary<string, ToyLayerGameObject>();

        public ToyData toyData;

        public Dictionary<string, ToyLayerGameObject> ToyLayerGameObjectList
        {
            get
            {
                return toyLayerGameObjectList;
            }

            set
            {
                toyLayerGameObjectList = value;
            }
        }

        public ToyGameObject(BaseProp data) {
            initData(data);
        }

        public virtual void initData(BaseProp data)
        {
            toyData = data as ToyData;
            PlaceIndexStruct = toyData.ScenePointIndex;
            ToyName = toyData.Name;
            IsInScene = PlaceIndexStruct.ScenePointIndex > 0;
            LoadConfig();
        }

        public virtual void LoadConfig()
        {
            string toyConfigName = string.Format("{0}Config",ToyName);
            ResourceLoader.Instance.TryLoadTextAsset(toyConfigName, (textAssert) =>
            {
                ToyConfig = JsonMapper.ToObject((textAssert as TextAsset).text);
                parseConfig();
            });
        }

        public void ShowToy()
        {
            loadRes();
        }

        public virtual void loadRes()
        {
            ResourceLoader.Instance.TryLoadClone(ToyName.ToLower(), ToyName, (go) =>
            {
                ToyPointGameObject toyGO = SceneManager.Instance.CurrentScene.SceneGameObject.FindToyPointByIndex(PlaceIndexStruct);
                GameObject toyPoint = toyGO.Root;
                toyGO.ClosePointMark();

                ToyMarkPointGameObject subPoint = toyGO.FindSubToyPointByIndex(PlaceIndexStruct);
                //Debug.Log("==============PlaceIndexStruct " + subPoint.IsSmallPoint + " " + PlaceIndexStruct.PointType);
                //Debug.Log(string.Format("{0} {1} {2}", PlaceIndexStruct.ScenePointIndex, PlaceIndexStruct.SubPointIndex, PlaceIndexStruct.PointType));
                subPoint.AddToyChild(go);

                //ToyIdentity toyIdentity = go.AddComponent<ToyIdentity>();
                //toyIdentity.Data = toyData;

                parseGameObject(go);

                go.SetActive(true);
            });
        }

        private void parseConfig()
        {
            string key = TOY_CONFIG_KEY_DEPTH[1];
            int toyLayersCount = ToyConfig[key].Count;

            //Debug.Log(string.Format("玩具配置文件 {0} 开始解析， 有 {1} 个玩具层", ToyName, toyLayersCount));

            ToyLayerGameObject temp;
            JsonData toyLayerJsonData;
            for (int i = 0; i < toyLayersCount; i++)
            {
                toyLayerJsonData = ToyConfig[key][i];
                temp = new ToyLayerGameObject(toyLayerJsonData, i+1);
                ToyLayerGameObjectList.Add(toyLayerJsonData["LayerName"].ToString().Split('_')[1], temp);
            }

            parseConfigOver();
        }

        private void parseConfigOver()
        {

        }

        protected virtual void parseGameObject(GameObject go)
        {
            Root = go;

            int count = Root.transform.childCount;
            GameObject temp;
            for (int i = 0; i < count; i++)
            {
                temp = Root.transform.GetChild(i).gameObject;
                if (temp.name.Contains("Image_"))
                {
                    //玩具地影对象
                }
                else if (temp.name.Contains("ToyLayer_"))
                {
                    ToyLayerGameObjectList[temp.name.Split('_')[1]].parseGameObject(temp);
                }
            }

            ShowEmptyState();

            parseGameObjectOver();
        }

        private void parseGameObjectOver()
        {
            
        }

        public void ShowEmptyState()
        {
            foreach (var toyLayer in toyLayerGameObjectList)
            {
                toyLayer.Value.UpdateState(toyLayer.Key.Contains("stop"));
            }
        }

        private void closeEmptyState()
        {
            foreach (var toyLayer in toyLayerGameObjectList)
            {
                if (toyLayer.Key.Contains("stop"))
                {
                    toyLayer.Value.UpdateState(false);
                }
            }
        }

        //找到玩具的一个玩点
        public GameObject FindPlayPoint(ToyStructInfo toyStructInfo)
        {
            Debug.Log(toyStructInfo + "111111111111111111111111");
            GameObject go = null;
            //Debug.Log(string.Format("查找玩点 {0} {1}", Root.transform.name, toyStructInfo.GetStructPath()));
            go = Root.transform.FindChild(toyStructInfo.GetStructPath()).gameObject;
            return go;
        }

        //获取玩具某一个点下能容纳的所有的猫的数据
        public List<PetInfo> GetAllPetInfo(ToyStructInfo toyStructInfo)
        {
            List<PetInfo> petInfoList;

            ToyLayerGameObject tld = ToyLayerGameObjectList[toyStructInfo.ToyLayerIndex.ToString()];
            PlayPointLayerGameObject ppld = tld.PlayPointLayerDataList[toyStructInfo.PlayPointLayerIndex - 1];
            PlayPointGameObject pd = ppld.PointDataList[toyStructInfo.PointIndex - 1];
            petInfoList = pd.PetInfoList;

            return petInfoList;
        }

        public PetInfo GetOnePetInfo(string petName, ToyStructInfo toyStructInfo)
        {
            List<PetInfo> petInfoList = GetAllPetInfo(toyStructInfo);
            foreach (var info in petInfoList)
            {
                if (info.Name == petName)
                {
                    return info;
                }
            }
            return null;
        }

        public void UpdateToyState(CatGameObject catGO)
        {
            updateActiveLayersByPlayerPoint(catGO);
            closeEmptyState();//强制调用下，因为有些玩具店没有设置HideLayers
        }

        //更新下各个玩具层的状态，顺便设置下动画
        private void updateActiveLayersByPlayerPoint(CatGameObject catGO)
        {
            PlayPointGameObject pointData = GetPiontDataByCat(catGO);
            List<string> hideLayers = pointData.HideToyLayers;
            ToyLayerGameObject tempToyLayer;
            bool activeFlag;
            foreach (var toyLayer in toyLayerGameObjectList)
            {
                activeFlag = true;
                tempToyLayer = toyLayer.Value;

                for (int j = 0; j < hideLayers.Count; j++)
                {
                    if (tempToyLayer.ToyLayerName == hideLayers[j].ToString())
                    {
                        activeFlag = false;
                        break;
                    }
                }

                tempToyLayer.UpdateState(activeFlag);
            }
        }

        public PlayPointGameObject GetPiontDataByCat(CatGameObject catGO)
        {
            PlayPointGameObject pd;

            ToyStructInfo catInfo = catGO.CatPathInfo;
            int toyLayerIndex = catInfo.ToyLayerIndex;
            int playPointLayerIndex = catInfo.PlayPointLayerIndex;
            int pointIndex = catInfo.PointIndex;
            Debug.Log(string.Format("{0}/ToyLayer_{1}/PlayPointLayer_{2}/Point_{3}", Root.name, toyLayerIndex, playPointLayerIndex, pointIndex));

            ToyLayerGameObject tld = ToyLayerGameObjectList[toyLayerIndex.ToString()];
            PlayPointLayerGameObject ppld = tld.PlayPointLayerDataList[playPointLayerIndex - 1];
            pd = ppld.PointDataList[pointIndex - 1];
            Debug.Log(ppld.PointDataList.Count + " " + pointIndex);

            return pd;
        }

        public void HideCat(ToyStructInfo toyStructInfo)
        {
            GameObject playPoint = FindPlayPoint(toyStructInfo);
            if (playPoint != null)
            {
                playPoint.SetActive(false);
            }
        }

        public void ReShowCat(ToyStructInfo toyStructInfo)
        {
            GameObject playPoint = FindPlayPoint(toyStructInfo);
            if (playPoint != null)
            {
                playPoint.SetActive(true);
            }
        }
    }
}
