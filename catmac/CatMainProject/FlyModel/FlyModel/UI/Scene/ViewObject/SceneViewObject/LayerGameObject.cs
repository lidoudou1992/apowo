using FlyModel.Model.Data;
using FlyModel.UI.Scene.ViewObject.Data;
using System.Collections.Generic;
using UnityEngine;

namespace FlyModel.UI.Scene.ViewObject.SceneViewObject
{
    public class LayerGameObject
    {
        public bool IsInteractiveLayer;

        public GameObject Layer;

        public List<ToyPointGameObject> toyPointGameObjectsList = new List<ToyPointGameObject>();
        public List<FoodPointGameObject> foodPointGameObjectsList = new List<FoodPointGameObject>();

        public LayerGameObject(GameObject layerGameObject)
        {
            IsInteractiveLayer = layerGameObject.name.Contains("MiddleLayer");
            Layer = layerGameObject;
            initToyPoints();
        }

        private void initToyPoints()
        {
            if (IsInteractiveLayer)
            {
                GameObject interactivePoints = Layer.transform.Find("Content/InteractivePoints").gameObject;
                int count = interactivePoints.transform.childCount;
                ToyPointGameObject tp;
                FoodPointGameObject fp;
                GameObject temp;
                for (int i = 0; i < count; i++)
                {
                    temp = interactivePoints.transform.GetChild(i).gameObject;
                    if (temp.name.Contains("ToyPoint_"))
                    {
                        tp = new ToyPointGameObject(temp, int.Parse(temp.name.Split('_')[1].ToString()));
                        toyPointGameObjectsList.Add(tp);
                    }
                    else if (temp.name.Contains("FoodPoint_"))
                    {
                        fp = new FoodPointGameObject(temp, int.Parse(temp.name.Split('_')[1].ToString()));
                        fp.IsSmallPoint = true;
                        foodPointGameObjectsList.Add(fp);
                    }
                }
            }
        }

        public ToyPointGameObject FindToyPointByIndex(ScenePointStruct pointStruct)
        {
            if (pointStruct.ScenePointIndex <= toyPointGameObjectsList.Count)
            {
                foreach (var toyPoin in toyPointGameObjectsList)
                {
                    if (toyPoin.ScenePointIndex == pointStruct.ScenePointIndex)
                    {
                        return toyPoin;
                    }
                }
            }
            else
            {
                Debug.Log("不存在该放置点");
            }

            return null;
        }

        public FoodPointGameObject FindFoodPointByIndex(ScenePointStruct pointStruct)
        {
            if (pointStruct.ScenePointIndex <= foodPointGameObjectsList.Count)
            {
                foreach (var foodPoint in foodPointGameObjectsList)
                {
                    if (foodPoint.pointStruct.ScenePointIndex == pointStruct.ScenePointIndex)
                    {
                        return foodPoint;
                    }
                }
            }
            else
            {
                Debug.Log("不存在该放置点");
            }

            return null;
        }

        public void ShowAvailable(BagItemData data)
        {
            if (data.SubType == EnumConfig.BagItemType.Furni)
            {
                foreach (var pointGO in toyPointGameObjectsList)
                {
                    pointGO.ShowAvailable(data);
                }
            }
            else if (data.SubType == EnumConfig.BagItemType.Food)
            {
                foreach (var foodPoint in foodPointGameObjectsList)
                {
                    foodPoint.ShowAvailable(data);
                }
            }
        }

        public void ClosePointMark()
        {
            foreach (var pointGO in toyPointGameObjectsList)
            {
                pointGO.ClosePointMark();
            }

            foreach (var foodGO in foodPointGameObjectsList)
            {
                foodGO.ClosePointMark();
            }
        }
    }
}
