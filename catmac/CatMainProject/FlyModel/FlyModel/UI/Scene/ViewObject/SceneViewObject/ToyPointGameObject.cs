using FlyModel.Model.Data;
using FlyModel.UI.Scene.ViewObject.Data;
using System.Collections.Generic;
using UnityEngine;

namespace FlyModel.UI.Scene.ViewObject.SceneViewObject
{
    public class ToyPointGameObject
    {
        public Dictionary<int, ToyMarkPointGameObject> SmallMarkLsit = new Dictionary<int, ToyMarkPointGameObject>();
        public ToyMarkPointGameObject LargeMark;
        public int ScenePointIndex;

        public GameObject Root;

        public ToyPointGameObject(GameObject point, int scenePointIndex)
        {
            Root = point;
            ScenePointIndex = scenePointIndex;
            initPointMarks();
        }

        private void initPointMarks()
        {
            int count = Root.transform.childCount;
            GameObject temp;
            ToyMarkPointGameObject mpGO;
            int tempIndex;
            for (int i = 0; i < count; i++)
            {
                temp = Root.transform.GetChild(i).gameObject;
                temp.SetActive(false);

                if (temp.name.Contains("Small_"))
                {
                    tempIndex = int.Parse(temp.name.Split('_')[1]);
                    mpGO = new ToyMarkPointGameObject(temp, true, ScenePointIndex, tempIndex);
                    mpGO.IsSmallPoint = true;
                    SmallMarkLsit.Add(tempIndex, mpGO);
                }
                else if (temp.name.Contains("Large_"))
                {
                    mpGO = new ToyMarkPointGameObject(temp, false, ScenePointIndex, 0);
                    mpGO.IsSmallPoint = false;
                    LargeMark = mpGO;
                }
            }
        }

        public void ShowAvailable(BagItemData data)
        {
            foreach (var small in SmallMarkLsit.Values)
            {
                small.ShowAvailable(data);
            }

            if (LargeMark != null)
            {
                LargeMark.ShowAvailable(data);
            }
        }

        public void ClosePointMark()
        {
            foreach (var small in SmallMarkLsit.Values)
            {
                small.ClosePointMark();
            }

            if (LargeMark != null)
            {
                LargeMark.ClosePointMark();
            }
        }

        public void AddToyGameObject(ScenePointStruct pointStruct, GameObject toyGO)
        {
            if (pointStruct.PointType == EnumConfig.SubPointType.small)
            {
                toyGO.transform.SetParent(SmallMarkLsit[pointStruct.SubPointIndex - 1].Root.transform);
            }
            else if (pointStruct.PointType == EnumConfig.SubPointType.large)
            {
                toyGO.transform.SetParent(LargeMark.Root.transform);
            }
        }

        public ToyMarkPointGameObject FindSubToyPointByIndex(ScenePointStruct pointStruct)
        {
            if (pointStruct.PointType == EnumConfig.SubPointType.small)
            {
                return SmallMarkLsit[pointStruct.SubPointIndex];
            }
            else if (pointStruct.PointType == EnumConfig.SubPointType.large)
            {
                return LargeMark;
            }

            return null;
        }
    }
}
