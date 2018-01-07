using FlyModel.UI.Component;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Scene.ViewObject
{
    public class ToyLayerGameObject
    {
        public string ToyLayerName;
        public int LayerIndex;
        public List<PlayPointLayerGameObject> PlayPointLayerDataList = new List<PlayPointLayerGameObject>();

        public JsonData toyLayerJsonData;

        public GameObject Root;

        public GameObject Toy;

        private bool isEmptyLayer;
        
        private List<SkeletonGraphic> spineList = new List<SkeletonGraphic>();

        public ToyLayerGameObject(JsonData jsonData, int layerIndex)
        {
            toyLayerJsonData = jsonData;
            LayerIndex = layerIndex;

            parseConfig();
        }

        private void parseConfig()
        {
            ToyLayerName = toyLayerJsonData["LayerName"].ToString();

            string key = ToyGameObject.TOY_CONFIG_KEY_DEPTH[2];
            int playerPointLayerCount = toyLayerJsonData[key].Count;

            //Debug.Log(string.Format("玩具层 配置文件开始解析 {0} 有 {1} 个玩点层", ToyLayerName, playerPointLayerCount));

            PlayPointLayerGameObject temp;
            JsonData tempJsonData;
            for (int i = 0; i < playerPointLayerCount; i++)
            {
                tempJsonData = toyLayerJsonData[key][i];
                temp = new PlayPointLayerGameObject(tempJsonData, i + 1);
                PlayPointLayerDataList.Add(temp);
            }
        }

        public void parseGameObject(GameObject go)
        {
            Root = go;

            isEmptyLayer = go.name == "ToyLayer_stop";

            int count = Root.transform.childCount;
            GameObject temp;
            int playPointIndex = 0;
            SkeletonGraphic spine;
            for (int i = 0; i < count; i++)
            {
                temp = Root.transform.GetChild(i).gameObject;

                //Debug.Log(string.Format("开始解析GameObject 玩具 {0} 的 玩具层 {1} 下的玩点层 {2}", go.transform.parent.name, go.transform.name, temp.name));

                if (temp.name.Contains("Toy"))
                {
                    Toy = temp;
                    spine = Toy.GetComponent<SkeletonGraphic>();
                    if (spine != null)
                    {
                        spineList.Add(spine);
                    }
                }
                else
                {
                    PlayPointLayerDataList[playPointIndex].parseGameObject(temp);
                    playPointIndex += 1;
                }
            }
        }

        public void UpdateState(bool isShow)
        {
            Root.SetActive(isShow);

            string actionName = isEmptyLayer ? "stop" : "play";

            foreach (var spine in spineList)
            {
                spine.freeze = !isShow;

                if (spine.AnimationState != null)
                {
                    spine.AnimationState.SetAnimation(0, actionName, true);
                }
            }
        }

    }
}
