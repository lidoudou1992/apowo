using FlyModel.Model.Data;
using FlyModel.UI;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.Model
{
    public class GuideManager
    {
        public static GuideManager Instance;

        //第几个新手引导文件
        public int GuideIndex = -1;

        public List<GuideData> guideDataList = new List<GuideData>();

        public int CurrentMainPhase = -1;

        public static Dictionary<string, PanelBase> PanelMap = new Dictionary<string, PanelBase>();

        public bool GuideComplete = true;

        private bool isStartted = false;

        private string waitForServer = "";

        private bool startByServer = false;

        public bool HasGettedOneTreasure;

        public int HandbookAppointCatType = -1;

        public bool CloseGuide = false;

        public bool BlockContinue;

        public static GuideManager Initializer()
        {
            if (Instance == null)
            {
                Instance = new GuideManager();
            }

            PanelMap.Add("InfoBar", PanelManager.infoBar);

            return Instance;
        }

        public void CreateCharacterGudie()
        {
            clearGuide();

            GuideIndex = 0;
            CurrentMainPhase = 0;

            string guideConfigName = string.Format("{0}config", "Guide_CreateCharacter");
            ResourceLoader.Instance.TryLoadTextAsset(guideConfigName, (textAssert) =>
            {
                string text = ((TextAsset)textAssert).text;             
                JsonData guideConfig = JsonMapper.ToObject(text);
                parseConfig(guideConfig);
            }
            );
        }

        public void CreateGuideData(int guideIndex)
        {
            clearGuide();

            GuideIndex = guideIndex;
            CurrentMainPhase = 0;

            string guideConfigName = string.Format("Guide_{0}config", guideIndex);
            ResourceLoader.Instance.TryLoadTextAsset(guideConfigName, (textAssert) =>
            {
                string text = ((TextAsset)textAssert).text;
                JsonData guideConfig = JsonMapper.ToObject(text);
                parseConfig(guideConfig);
            });
        }

        // 解析配置文件
        private void parseConfig(JsonData config)
        {
            GuideData tempData;
            for (int i = 0; i < config.Count; i++)
            {
                tempData = new GuideData();
                tempData.MainPhase = i + 1;
                tempData.UpdateData(config[i]);

                guideDataList.Add(tempData);
            }      
        }

        public void StartGuideByServer(Proto.GuideData data)
        {
            if (CloseGuide)
            {
                return;
            }

            Debug.Log("guide data " + data);
            if (data != null)
            {
                Instance.CreateGuideData((int)data.Category);
                // 引导画面3
                PanelManager.GuidePanel.Show(() =>
                {
                    // 设置新手引导配置文件的序号
                    PanelManager.guidePanel.guideConfigIndex = (int) data.Category;
                    CurrentMainPhase = data.Level-1;//前端子步骤从0开始的
                    startByServer = true;
                });
            }
        }

        public void TakenStartByServer()
        {
            if (startByServer)
            {
                startByServer = false;
                StartGuide();
            }
        }

        public void StartGuideManul(int index)
        {
            if (CloseGuide)
            {
                return;
            }
            Instance.CreateGuideData(index);
            // 引导画面4
            PanelManager.GuidePanel.Show(() =>
            {
                // 设置房屋扩建引导配置文件的序号
                PanelManager.guidePanel.guideConfigIndex = index;
                StartGuide();
            });
        }

        public void StartGuide()
        {
            if (GameMain.GuideDisabled)
                return;
            isStartted = true;
            GuideComplete = false;
            DoNext();
        }

        private void clearGuide()
        {
            guideDataList.Clear();
            isStartted = false;
            GuideComplete = true;
            CurrentMainPhase = -1;
            GuideIndex = -1;
        }
        
        public bool IsGuiding()
        {
            return (isStartted && GuideComplete == false);
        }

        public void DoNext()
        {
            if (isStartted && CurrentMainPhase < guideDataList.Count)
            {
                guideDataList[CurrentMainPhase].DoNext();
            }
        }

        public void AddCurrentMainIndex()
        {
            CurrentMainPhase++;
            if (CurrentMainPhase >= guideDataList.Count)
            {
                GuideComplete = true;
                Debug.Log("本次引导全部完成");
            }
        }

        public void ContinueGuideByCreateCharacterOK()
        {
            if (isStartted && GuideIndex ==1)
            {
                DoNext();
                PanelManager.guidePanel.transform.SetAsLastSibling();
            }
        }

        public void ContinueGuide()
        {
            if (BlockContinue)
            {
                BlockContinue = false;
                return;
            }

            if (isStartted && GuideIndex > -1)
            {
                DoNext();
            }
        }


        public void RecoredCurrentPhase()
        {
            CommandHandle.Send(Proto.ServerMethod.NextGuideIndex, new Proto.IDMessage() { Id = GuideIndex });
            Debug.Log("GuideIndex " + GuideIndex + " --------------------------------------------");
        }

        public void ShowMask(Vector3 targetPos, Vector3 targetSize, Transform parent, string parms, string maskBorderParas)
        {
            PanelManager.GuideMaskPanel.Show(() =>
            {
                float rate = Screen.height / 1136f;

                Debug.Log("-----------------------------------------------------------------------------");
                Debug.Log(string.Format("targetPos:{0},targetSize:{1},parms:{2}", targetPos, targetSize, parms));
                PanelManager.guideMaskPanel.ShowGesture(targetPos, targetSize, parms);

                float x = float.Parse(maskBorderParas.Split('#')[0].ToString());
                float y = float.Parse(maskBorderParas.Split('#')[1].ToString());

                x = x * rate;
                y = y * rate;
                PanelManager.guideMaskPanel.UpdateShaderMask(targetPos.x + x, targetPos.y + y, targetSize.x, targetSize.y);
            });
        }

        public GuideActionData GetNextGuide()
        {
            return guideDataList[CurrentMainPhase].GetNextGuide();
        }

        //public bool IsAppointtedCom(string comName)
        //{
        //    GuideActionData temp = guideDataList[CurrentMainPhase].GetNextGuide();
        //    if (temp != null)
        //    {
        //        return temp.IsAppointtedPanel(comName);
        //    }

        //    return false;
        //}

        public bool IsGestureTouchEffective(string comName)
        {
            if (isStartted==false)
            {
                return false;
            }

            if (GuideComplete)
            {
                if (PanelManager.guideMaskPanel != null)
                {
                    PanelManager.guideMaskPanel.Close();
                }
            }
            else
            {
                bool isSffective = false;
                GuideActionData temp = guideDataList[CurrentMainPhase].GetCurrentGuide();
                if (temp != null)
                {
                    isSffective = temp.IsGestureTouchEffective(comName);

                    if (isSffective)
                    {
                        PanelManager.guideMaskPanel.Close();
                    }

                    return isSffective;
                }
            }

            return false;
        }

        public string TakeWaitForServerTag()
        {
            string s = waitForServer;
            waitForServer = "";
            return s;
        }

        public void SetWaitForServerTag(string tag)
        {
            waitForServer = tag;
        }
    }
}
