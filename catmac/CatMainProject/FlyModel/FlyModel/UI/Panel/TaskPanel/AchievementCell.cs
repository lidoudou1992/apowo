using FlyModel.Proto;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.TaskPanel
{
    public class AchievementCell
    {
        public GameObject GameObject;

        private Text nameTF;
        private Text desTF;
        private Text awardTF;
        private Image icon;
        private Slider progress;
        private Text progressTF;
        private GameObject completeFlag;

        private Model.Data.SubTaskData subTaskData;
        private Model.Data.TaskData taskData;

        public AchievementCell(GameObject gameObject)
        {
            GameObject = gameObject;

            nameTF = gameObject.transform.Find("Button/Icon/Text").GetComponent<Text>();
            desTF = gameObject.transform.Find("Button/Name").GetComponent<Text>();
            awardTF = gameObject.transform.Find("Button/Des").GetComponent<Text>();
            progress = gameObject.transform.Find("Button/Slider").GetComponent<Slider>();
            progressTF = gameObject.transform.Find("Button/Slider/Text").GetComponent<Text>();

            icon = gameObject.transform.Find("Button/Icon/neir").GetComponent<Image>();

            completeFlag = gameObject.transform.Find("Button/Task").gameObject;

            new SoundButton(gameObject.transform.Find("Button").gameObject, () =>
            {
                if (taskData.IsAllComplete == false && subTaskData.HasAward())
                {
                    CommandHandle.Send(Proto.ServerMethod.DrawAchieve, new IDMessage() { Id = taskData.ID });
                }
            });
        }

        public void UpdateData(Model.Data.TaskData data)
        {
            taskData = data;

            subTaskData = data.GetNextSubTask();

            nameTF.text = subTaskData.Name;
            desTF.text = subTaskData.Description;
            progress.value = subTaskData.GetProgress();
            progressTF.text = string.Format("{0}/{1}", subTaskData.CompleteValue, subTaskData.ConditionValue);
            awardTF.text = subTaskData.AwardStr;

            updateBg(subTaskData.Quality);

            ResourceLoader.Instance.TryLoadPic(ResPathConfig.ICON_ASSETBUNDLE, subTaskData.PicCode, (texture) =>
            {
                icon.sprite = texture as Sprite;
            });

            completeFlag.SetActive(data.IsAllComplete==false && subTaskData.HasAward());
        }

        public void updateBg(int quality)
        {
            Transform container = GameObject.transform.Find("Button/Icon/bg").transform;
            for (int i = 0; i < container.childCount; i++)
            {
                container.GetChild(i).gameObject.SetActive(quality == (i + 1));
            }
        }
    }
}
