using FlyModel.Model.Data;
using FlyModel.UI;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model
{
    public class TaskManager
    {
        public static TaskManager Instance;

        public static TaskManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new TaskManager();
            }

            return Instance;
        }

        public Dictionary<int, TaskData> TaskDataList = new Dictionary<int, TaskData>();

        public bool IsSceneExtendPointAchievementGet = false;

        

        public void InitConfigs()
        {
            ResourceLoader.Instance.TryLoadTextAsset(ResPathConfig.TASK_CONFIG, (textAssert) => {
                string text = ((TextAsset)textAssert).text;
                JsonData tasks = JsonMapper.ToObject(text);

                //var jo = JsonSerializer.Deserialize(text);
                //Debug.Log(jo.ToString());
                TaskData tempData;
                foreach (var task in tasks)
                {
                    tempData = new TaskData();
                    tempData.UpdateData(task as JsonData);
                    TaskDataList.Add((int)tempData.Type, tempData);
                }
            });
        }

        public void UpdateTaskDatas(List<Proto.AchievementData> datas)
        {
            foreach (var data in datas)
            {
                TaskDataList[(int)data.Type].UpdateData(data);
            }
        }

        public void UpdateTask(Proto.AchievementData data)
        {
            foreach (var taskData in TaskDataList)
            {
                if (taskData.Value.Type == data.Type)
                {
                    taskData.Value.UpdateData(data);
                    break;
                }
            }
        }

        public int GetChievementPoints()
        {
            int points = 0;

            foreach (var data in TaskDataList)
            {
                points += data.Value.GetCompleteAchievePoints();

            }

            return points;
        }
    }
}
