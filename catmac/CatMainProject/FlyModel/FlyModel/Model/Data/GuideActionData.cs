using FlyModel.UI;
using FlyModel.UI.Panel.Bag;
using FlyModel.UI.Panel.GuidePanel;
using FlyModel.UI.Panel.HandbookPanel;
using FlyModel.UI.Panel.SceneChangePanel;
using FlyModel.UI.Panel.ShopPanel;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class GuideActionData:BaseProp
    {
        public int MainPhase;

        public int SubStep;

        public EnumConfig.GuideActionType ActionType;

        public string ActionContent;

        public string GestureParams;

        private string gestureTargetCom;

        private int propParam;

        private string maskBorderParas = "0#0";

        private string startPanel = "";

        private bool blockContinue;

        public void UpdateData(JsonData data)
        {
            ActionType = (EnumConfig.GuideActionType)(EnumConfig.GuideActionTypeDic[data["Type"].ToString()]);
            ActionContent = data["Content"].ToString();

            // 解析JsonData的内容----------------------------
            //if ((data as IDictionary).Contains("GestureParams"))
            //{
            //    GestureParams = data["GestureParams"].ToString();
            //}
            //if ((data as IDictionary).Contains("PropParam"))
            //{
            //    propParam = int.Parse(data["PropParam"].ToString());
            //}
            //if ((data as IDictionary).Contains("MaskBorderParas"))
            //{
            //    maskBorderParas = data["MaskBorderParas"].ToString();
            //}
            //if ((data as IDictionary).Contains("StartPanel"))
            //{
            //    startPanel = data["StartPanel"].ToString();
            //}
            //if ((data as IDictionary).Contains("BlockContinue"))
            //{
            //    blockContinue = (bool)data["BlockContinue"];
            //}
            // L#不支持as，换成强转
            if (((IDictionary)data).Contains("GestureParams"))
            {
                GestureParams = data["GestureParams"].ToString();
            }
            if (((IDictionary)data).Contains("PropParam"))
            {
                propParam = int.Parse(data["PropParam"].ToString());
            }
            if (((IDictionary)data).Contains("MaskBorderParas"))
            {
                maskBorderParas = data["MaskBorderParas"].ToString();
            }
            if (((IDictionary)data).Contains("StartPanel"))
            {
                startPanel = data["StartPanel"].ToString();
            }
            if (((IDictionary)data).Contains("BlockContinue"))
            {
                blockContinue = (bool)data["BlockContinue"];
            }
            // --------------------------------------------------

            if (ActionType == EnumConfig.GuideActionType.ShowGesture || ActionType == EnumConfig.GuideActionType.SelectProp || ActionType == EnumConfig.GuideActionType.ShowGestureIScene)
            {
                var ary = ActionContent.Split('#');
                gestureTargetCom = ary[ary.Length - 1];
            }
        }

        public bool IsGestureTouchEffective(string comName)
        {
            //Debug.Log(gestureTargetCom + " " + comName);
            return gestureTargetCom == comName;
        }

        public void DoAction( )
        {
            Debug.Log(ActionType + " " + ActionContent);
            switch (ActionType)
            {
                case EnumConfig.GuideActionType.Say:
                    if (string.IsNullOrEmpty(startPanel))
                    {
                        startSay();
                    }
                    else
                    {
                        if (startPanel == "ShopPanel")
                        {
                            GuideManager.Instance.BlockContinue = blockContinue;
                            if (PanelManager.IsCurrentPanel(PanelManager.shopPanel))
                            {
                                startSay();
                            }
                            else
                            {
                                PanelManager.ShopPanel.Show(() => { startSay(); });
                            }
                        }
                    }
                    break;
                case EnumConfig.GuideActionType.ShowPanel:
                    if (ActionContent == "CreateCharacterPanel")
                    {
                        PanelManager.guidePanel.Close();
                        PanelManager.CreateCharacterPanel.Show();
                    }
                    break;
                case EnumConfig.GuideActionType.RecoredPhase:
                    Debug.Log(string.Format("记录当前阶段:{0}", MainPhase));
                    //GuideManager.Instance.DoNext();
                    break;
                case EnumConfig.GuideActionType.ShowGesture:
                    var ary = ActionContent.Split('#');

                    GameObject target = null;
                    PanelBase panel = null;
                    if (ary[0] == "InfoBar")
                    {
                        panel = PanelManager.infoBar;
                    }else if (ary[0] == "SystemBar")
                    {
                        target = PanelManager.SystemBar.PanelPrefab.transform.Find(ary[1]).gameObject;
                    }
                    else
                    {
                        panel = PanelManager.GetPanel(ary[0].ToLower());
                    }

                    if (panel != null || target!= null)
                    {
                        Transform parent = null;
                        if (target == null)
                        {
                            target = panel.PanelPrefab.transform.Find(ary[1]).gameObject;
                            parent = panel.transform.parent;
                        }
                        else
                        {
                            parent = target.transform.parent;
                        }

                        Vector2 targetSize = target.GetComponent<RectTransform>().sizeDelta;
                        Vector3 targetPos = target.transform.position;

                        
                        GuideManager.Instance.ShowMask(targetPos, targetSize, parent, GestureParams, maskBorderParas);

                        if (PanelManager.guidePanel != null)
                        {
                            PanelManager.guidePanel.Close();
                        }
                    }
                    else
                    {
                        Debug.Log(string.Format("不存在的面板 {0}", ary[0]));
                    }
                    break;
                case EnumConfig.GuideActionType.SelectProp:
                    ary = ActionContent.Split('#');

                    string panelName = ary[0];
                    panel = null;
                    GameObject targetCom = null;
                    if (panelName == "InfoBar")
                    {
                        panel = PanelManager.infoBar;
                    }
                    else if(panelName == "ShopPanel")
                    {
                        panel = PanelManager.shopPanel;
                        targetCom = (panel as ShopPanel).FindGuidePropCom(propParam).GameObject;
                    }else if (panelName == "BagPanel")
                    {
                        panel = PanelManager.bagPanel;
                        targetCom = (panel as BagPanel).FindGuidePropCom(propParam).GameObject;
                    }else if (panelName == "HandbookPanel")
                    {
                        panel = PanelManager.handbookPanel;
                        targetCom = (panel as HandbookPanel).FindGuidePropCom(propParam).GameObject;
                    }else if (panelName == "SceneChangePanel")
                    {
                        panel = PanelManager.sceneChangePanel;
                        targetCom = (panel as SceneChangePanel).FindGuidePropCom(propParam).GameObject;
                    }

                    if (targetCom != null)
                    {
                        Vector2 targetSize = targetCom.GetComponent<RectTransform>().sizeDelta;
                        Vector3 targetPos = targetCom.transform.position;
                        Debug.Log(targetPos);
                        Debug.Log(targetCom.name);
                        GuideManager.Instance.ShowMask(targetPos, targetSize, panel.transform.parent, GestureParams, maskBorderParas);

                        if (PanelManager.guidePanel != null)
                        {
                            PanelManager.guidePanel.Close();
                        }
                    }
                    else
                    {
                        Debug.Log(string.Format("不存在该物品 {0}", ary[1]));
                    }
                    break;
                case EnumConfig.GuideActionType.ShowGestureIScene:
                    ary = ActionContent.Split('#');
                    targetCom = GameMain.SceneRoot.transform.Find(ary[1]).gameObject;

                    if (targetCom != null)
                    {
                        Vector2 targetSize = targetCom.GetComponent<RectTransform>().sizeDelta;
                        Vector3 targetPos = targetCom.transform.position;

                        GuideManager.Instance.ShowMask(targetPos, targetSize, GameMain.SceneRoot.transform, GestureParams, maskBorderParas);

                        if (PanelManager.guidePanel != null)
                        {
                            PanelManager.guidePanel.Close();
                        }
                    }
                    else
                    {
                        Debug.Log(string.Format("不存在该物品 {0}", ary[1]));
                    }
                    break;
                case EnumConfig.GuideActionType.WaitForServer:
                    GuideManager.Instance.SetWaitForServerTag(ActionContent);
                    break;
                case EnumConfig.GuideActionType.SendMsg:
                    PanelManager.guidePanel.Close();
                    GuideManager.Instance.RecoredCurrentPhase();
                    break;
                default:
                    Debug.Log("未解析的引导行为: " + ActionType);
                    break;
            }
        }

        private void startSay()
        {
            if (PanelManager.guidePanel == null || PanelManager.guidePanel.transform.gameObject.activeSelf == false)
            {
                // 引导画面2
                PanelManager.GuidePanel.Show(() => {
                    PanelManager.guidePanel.Say(ActionContent);
                });
            }
            else
            {
                PanelManager.guidePanel.Say(ActionContent);
            }
        }
    }
}
