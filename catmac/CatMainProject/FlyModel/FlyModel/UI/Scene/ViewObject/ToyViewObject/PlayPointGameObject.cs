using FlyModel.UI.Scene.ViewObject.Data;
using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace FlyModel.UI.Scene.ViewObject
{
    public class PlayPointGameObject
    {
        public string PointName;
        public int PointIndex;

        private JsonData pointJsonData;

        public List<PetInfo> PetInfoList = new List<PetInfo>();

        public List<string> HideToyLayers = new List<string>();

        public GameObject Root;

        public PlayPointGameObject(JsonData jsonData, int index)
        {
            pointJsonData = jsonData;
            PointIndex = index;

            parseConfig();
        }

        private void parseConfig()
        {
            PointName = pointJsonData["PointName"].ToString();
            string key = ToyGameObject.TOY_CONFIG_KEY_DEPTH[4];
            int petCount = pointJsonData[key].Count;
            JsonData tempJsonData;
            PetInfo temp;
            for (int i = 0; i < petCount; i++)
            {
                tempJsonData = pointJsonData[key][i];
                temp = new PetInfo(tempJsonData);
                PetInfoList.Add(temp);
            }

            JsonData hideToyLayers = pointJsonData["HideToyLayers"];
            foreach (var toyLayer in hideToyLayers)
            {
                HideToyLayers.Add(toyLayer.ToString());
            }
        }

        public void parseGameObject(GameObject go)
        {
            Root = go;
        }
    }
}
