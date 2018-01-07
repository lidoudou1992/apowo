using FlyModel.UI;
using FlyModel.UI.Scene.ViewObject.Data;
using System;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class FoodData:BaseProp
    {
        public ScenePointStruct ScenePointIndex;//从1开始
        public long RoomID;
        public Proto.FoodLevel ProgressStep;
        private long duration;
        private long lastPlaceTime;

        public float Progress;

        public FoodData()
        {
            ScenePointIndex = new ScenePointStruct();
        }

        public void UpdateData(Proto.FoodData data)
        {
            ID = data.Id;
            Type = data.Type;
            Count = (int)data.Count;
            Name = string.Format("Food{0}", Type);

            RoomID = data.RoomID;

            ScenePointIndex.ScenePointIndex = data.ScenePointIndex + 1;
            ScenePointIndex.PointType = EnumConfig.SubPointType.small;
            ScenePointIndex.SubPointIndex = 1;
            ScenePointIndex.Distribution = (EnumConfig.InteractivePointDistibution)((int)(data.RoomSectionType));

            duration = data.Duration/1000;
            lastPlaceTime = GameMain.TimeTick.DateTimeToUTCSeconds(new DateTime(data.LastPlaceTime));
            //DateTime dt = GameMain.TimeTick.ConvertTime(lastPlaceTime);
            //Debug.Log(string.Format("上次放置时间:{0}", dt.ToString("yyyy-MM-dd HH-mm-ss")));

            Progress = GetProgress();
            updateProgress(Progress);
        }

        private void updateProgress(float progress)
        {
            if (progress<=0)
            {
                ProgressStep = Proto.FoodLevel._4;
            }else if (progress<=0.33)
            {
                ProgressStep = Proto.FoodLevel._3;
            }
            else if (progress<0.66)
            {
                ProgressStep = Proto.FoodLevel._2;
            }
            else if (progress <= 1)
            {
                ProgressStep = Proto.FoodLevel._1;
            }
        }
        
        public float GetProgress()
        {
            float p = (1 - (float)(GameMain.TimeTick.GetNow()- lastPlaceTime) / duration);
            p = (float)Math.Round(p,2);

            return Mathf.Max(0, p);
        }
    }
}
