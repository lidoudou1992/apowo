using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.TestToyScene
{
    public class TestCatData
    {
        public string Name;

        public string toyLayerName;
        public string playPointLayerName;
        public string pointName;
        public TestPointData PlayPointData;

        public List<TestCatAnimationData> animationDatas = new List<TestCatAnimationData>();

        public TestCatData(JsonData jsonData, string toyLayerName, string playPointLayerName, TestPointData pointData)
        {
            this.toyLayerName = toyLayerName;
            this.playPointLayerName = playPointLayerName;
            this.pointName = pointData.Name;
            PlayPointData = pointData;

            Name = jsonData["CatName"].ToString();

            JsonData animations = jsonData["Animations"];
            for (int i = 0; i < animations.Count; i++)
            {
                animationDatas.Add(new TestCatAnimationData(animations[i], this));
            }
        }

        public string GetCatPath()
        {
            return string.Format("{0}/{1}/{2}", toyLayerName, playPointLayerName, pointName);
        }
    }
}
