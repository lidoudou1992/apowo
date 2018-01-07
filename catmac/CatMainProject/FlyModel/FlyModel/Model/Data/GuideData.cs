using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class GuideData:BaseProp
    {
        public int MainPhase;

        public List<GuideActionData> ActionsList = new List<GuideActionData>();

        private int subStep = -1;

        public void UpdateData(JsonData data)
        {
            subStep = 0;

            GuideActionData tempActionData;
            JsonData actions = data["Actions"];
            int count = actions.Count;
            for (int i = 0; i < count; i++)
            {
                tempActionData = new GuideActionData();
                tempActionData.MainPhase = MainPhase;
                tempActionData.SubStep = i;
                tempActionData.UpdateData(actions[i]);

                ActionsList.Add(tempActionData);
            }
        }

        public void DoNext( )
        {
            //Debug.Log("DoNext " + subStep + " " + ActionsList.Count);
            if (subStep < ActionsList.Count)
            {
                ActionsList[subStep].DoAction();
                subStep++;
            }

            if (subStep >= ActionsList.Count)
            {
                Debug.Log(string.Format("第 {0} 段引导完成", MainPhase));
                GuideManager.Instance.AddCurrentMainIndex();
            }
        }

        public GuideActionData GetNextGuide()
        {
            int nextSubStep = subStep;
            if (nextSubStep < ActionsList.Count)
            {
                return ActionsList[nextSubStep];
            }

            return null;
        }

        public GuideActionData GetCurrentGuide()
        {
            int currentSubStep = subStep - 1;
            if (currentSubStep > 0)
            {
                return ActionsList[currentSubStep];
            }

            return null;
        }
    }
}
