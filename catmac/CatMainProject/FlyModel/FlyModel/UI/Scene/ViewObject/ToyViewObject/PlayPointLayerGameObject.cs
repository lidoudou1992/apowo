using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Scene.ViewObject
{
    public class PlayPointLayerGameObject
    {
        public string PlayPointLayerName;
        public int PlayPointLayerIndex;
        public List<PlayPointGameObject> PointDataList = new List<PlayPointGameObject>();

        private JsonData playPointLayerJsonData;

        public GameObject Root;

        public PlayPointLayerGameObject(JsonData jsonData, int layerIndex)
        {
            playPointLayerJsonData = jsonData;
            PlayPointLayerIndex = layerIndex;

            parseConfig();
        }

        private void parseConfig()
        {
            PlayPointLayerName = playPointLayerJsonData["LayerName"].ToString();
            string key = ToyGameObject.TOY_CONFIG_KEY_DEPTH[3];
            int pointCount = playPointLayerJsonData[key].Count;
            JsonData tempJsonData;
            PlayPointGameObject temp;
            for (int i = 0; i < pointCount; i++)
            {
                tempJsonData = playPointLayerJsonData[key][i];
                temp = new PlayPointGameObject(tempJsonData, i + 1);
                PointDataList.Add(temp);
            }
        }

        public void parseGameObject(GameObject go)
        {
            Root = go;

            int count = PointDataList.Count;
            GameObject temp;
            for (int i = 0; i < count; i++)
            {
                temp = Root.transform.GetChild(i).gameObject;
                PointDataList[i].parseGameObject(temp);
            }
        }
    }
}
