using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class TaskData:BaseProp
    {
        public List<SubTaskData> SubTaskDatas = new List<SubTaskData>();

        private int totalStep;
        private int completeStep = 0;
        private int completeAchievePoints;
        private int Category;
        public EffectiveData effectiveData;

        //有没有领过奖
        public bool hasGetAward = true;

        public bool IsAllComplete;

        public void UpdateData(JsonData data)
        {
            Type = long.Parse(data["ID"].ToString());
            Name = data["Name"].ToString();
            Category = int.Parse(data["Category"].ToString());
            
            JsonData subDatas = data["SubAchieves"];
            SubTaskData temp;
            foreach (var subData in subDatas)
            {
                temp = new SubTaskData();
                temp.UpdateData(subData as JsonData);
                SubTaskDatas.Add(temp);
            }

            totalStep = subDatas.Count;
        }

        public SubTaskData GetNextSubTask()
        {
            if (hasGetAward)
            {
                if (completeStep < SubTaskDatas.Count)
                {
                    return SubTaskDatas[completeStep];
                }
                else
                {
                    return SubTaskDatas[SubTaskDatas.Count - 1];
                }
            }
            else
            {
                return SubTaskDatas[Math.Max(completeStep - 1, 0)];
            }
        }

        public SubTaskData GetSubTask(int index)
        {
            return SubTaskDatas[index];
        }

        public SubTaskData GetCurrentCompleteSubTask()
        {
            if (completeStep > 0)
            {
                return SubTaskDatas[completeStep - 1];
            }

            return null;
        }

        public void UpdateData(Proto.AchievementData data)
        {
            hasGetAward = true;

            ID = data.Id;

            Debug.Log("============data.Type " + data.Type + " "  + data.Completed + " " + data.Count);
            SubTaskData temp;
            completeAchievePoints = 0;
            for (int i = 0; i < SubTaskDatas.Count; i++)
            {
                temp = SubTaskDatas[i];

                if (i <= data.Completed)
                {
                    //if (data.Type == 8001)
                    //{
                    //    Debug.Log("=============aaa " + data.Count + " " + temp.ConditionValue + " " + i);
                    //}
                    hasGetAward = data.Count >= temp.ConditionValue;
                }

                if (i == data.Completed - 1)
                {
                    temp.IsCompleted = true;
                    completeAchievePoints += temp.AchievePoint;

                    //更新每一步的成就
                    updateEffectData(temp);
                    break;
                }
                else if(i < data.Completed - 1)
                {
                    completeAchievePoints += temp.AchievePoint;
                }
            }

            if (data.Completed >= totalStep)
            {
                hasGetAward = true;
                IsAllComplete = true;
            }

            completeStep = data.Completed;
            GetNextSubTask().UpdateData(data);
        }

        public bool IsCompleted()
        {
            foreach (var item in SubTaskDatas)
            {
                if (item.IsCompleted == false)
                {
                    return false;

                }
            }

            return true;
        }

        public int GetTotalSteps()
        {
            return SubTaskDatas.Count;
        }

        public int GetCompleteStep()
        {
            return Mathf.Max(0, completeStep);
        }

        public int GetCompleteValue()
        {
            if (GetCurrentCompleteSubTask() != null)
            {
                return GetCurrentCompleteSubTask().CompleteValue;

            }

            return 0;
        }

        public float GetCompleteProgress()
        {
            return (float)completeStep / totalStep;
        }

        public int GetCompleteAchievePoints()
        {
            return completeAchievePoints;
        }

        private void updateEffectData(SubTaskData currentCompleteStep)
        {
            if (currentCompleteStep.EffectID > 0)
            {
                if (effectiveData == null)
                {
                    effectiveData = new EffectiveData();
                }

                effectiveData.UpdatgeData(currentCompleteStep.EffectID);
            }
        }
    }

    public class SubTaskData : BaseProp
    {
        public int TitleID;
        public int AchievePoint;
        public bool IsCompleted;
        public int ConditionValue;
        public int Quality;
        public int EffectID;
        public string AwardStr = "";
        public int CompleteValue;


        public void UpdateData(JsonData data)
        {
            Name = data["Name"].ToString();
            TitleID = int.Parse(data["DesignID"].ToString());
            AchievePoint = int.Parse(data["AchievePoint"].ToString());
            Quality = int.Parse(data["Rank"].ToString());

            PicCode = data["Code"].ToString();
            Description = data["Description"].ToString();

            ConditionValue = int.Parse(data["Value"].ToString());

            EffectID = int.Parse(data["DesignID"].ToString());

            AwardStr = data["AwardStr"].ToString();
        }

        public void UpdateData(Proto.AchievementData data)
        {
            CompleteValue = data.Count;
        }

        public string GetAwardString()
        {
            string str = "";
            return str;
        }

        public bool HasAward()
        {
            return CompleteValue >= ConditionValue;
        }

        public float GetProgress()
        {
            //Debug.Log(CompleteValue + " " + ConditionValue);
            float f = (float)CompleteValue / ConditionValue;
            return Mathf.Min(f, 1f);
        }
    }
}
