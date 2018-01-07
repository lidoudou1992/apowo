using FlyModel.UI;
using FlyModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class EffectiveData:BaseProp
    {
        public float Value;
        private int Category;

        public static int PHOTO_NUMS_ADDITION = 0;
        public static float TOY_PRICE_RATE = 1f;

        public void UpdatgeData(int id)
        {
            ConfigUtil.GetConfig(ResPathConfig.Effect_CONFIG, id, (json) =>
            {
                Name = json["EffectName"].ToString();
                Description = json["EffectState"].ToString();
                Value = float.Parse(json["Value"].ToString());

                Category = int.Parse(json["Category"].ToString());

                //Debug.Log("==============Category " + Category);
                switch (Category)
                {
                    case 0:
                        //相册扩容
                        PHOTO_NUMS_ADDITION = (int)Value;
                        break;
                    case 1:
                        //银鱼干加成
                        break;
                    case 2:
                        //金鱼干加成
                        break;
                    case 3:
                        //猫咪出现概率加成
                        break;
                    case 4:
                        //玩具点加一
                        TaskManager.Instance.IsSceneExtendPointAchievementGet = true;
                        break;
                    case 6:
                        //猫粮持续时间加成
                        break;
                    case 7:
                        //玩具打折
                        TOY_PRICE_RATE = Value;
                        break;
                    default:
                        break;
                }
            });
        }
    }
}
