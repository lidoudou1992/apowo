using FlyModel.UI;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Utils
{
    public class ConfigUtil
    {
        public static Dictionary<string, JsonData> FurniConfigDic = new Dictionary<string, JsonData>();
        public static Dictionary<string, JsonData> FoodConfigDic = new Dictionary<string, JsonData>();
        public static Dictionary<string, JsonData> ItemConfigDic = new Dictionary<string, JsonData>();
        public static Dictionary<string, JsonData> CatsConfigDic = new Dictionary<string, JsonData>();
        public static Dictionary<string, JsonData> EffectConfigDic = new Dictionary<string, JsonData>();

        public static JsonData GetConfig(string configName, long type, Action<JsonData> gettedCallback)
        {
            Dictionary<string, JsonData> tempList = null;
            if (configName == ResPathConfig.TOY_CONFIG)
            {
                tempList = FurniConfigDic;
            }
            else if (configName == ResPathConfig.FOOD_CONFIG)
            {
                tempList = FoodConfigDic;
            }
            else if (configName == ResPathConfig.ITEM_CONFIG)
            {
                tempList = ItemConfigDic;
            }else if (configName == ResPathConfig.CATS_CONFIG)
            {
                tempList = CatsConfigDic;
            }else if (configName == ResPathConfig.Effect_CONFIG)
            {
                tempList = EffectConfigDic;
            }

            if (tempList != null && tempList.Count <= 0)
            {
                ResourceLoader.Instance.TryLoadTextAsset(configName, (textAssert) =>
                {
                    if (tempList.Count <= 0)
                    {
                        JsonData furniConfig = JsonMapper.ToObject((textAssert as TextAsset).text);

                        JsonData temp;
                        for (int i = 0; i < furniConfig.Count; i++)
                        {
                            temp = furniConfig[i];
                            tempList.Add(temp["ID"].ToString(), temp);
                        }
                    }

                    if (tempList.ContainsKey(type.ToString()))
                    {
                        gettedCallback(tempList[type.ToString()]);
                    }
                    else
                    {
                        gettedCallback(null);
                    }
                });
            }
            else
            {
                if (tempList.ContainsKey(type.ToString()))
                {
                    gettedCallback(tempList[type.ToString()]);
                }
                else
                {
                    gettedCallback(null);
                }
            }

            return null;
        }
    }
}
