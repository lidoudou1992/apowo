using FlyModel.Control;
using FlyModel.UI.Panel;
using FlyModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Component
{
    //public class BackKeyController:MonoBehaviour
    public class BackKeyController: BehaviourBase
    {
        public GameObject GameObject;

        private List<string> ignorBackPanelList;

        public BackKeyController(GameObject go)
        {
            GameObject = go;

            ignorBackPanelList = new List<string>();
            ignorBackPanelList.Add("LoginPanel");
            ignorBackPanelList.Add("LoadingPanel");
        }

        //void Awake()
        //{
        //    ignorBackPanelList = new List<string>();
        //    ignorBackPanelList.Add("LoginPanel");
        //    ignorBackPanelList.Add("LoadingPanel");
        //}

        public override void Update()
        {
            base.Update();

            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (PanelManager.CurrentPanel == null)
                    {
                        PanelManager.AlertPanel.Show(() => {
                            PropPopupModeStruct modeStruct = new PropPopupModeStruct();

                            modeStruct.Mode = EnumConfig.PropPopupPanelBtnModde.TwoBtb;

                            modeStruct.LeftBtnString = "退出";
                            modeStruct.LeftCallback = () => {
                                PanelManager.alertPanel.Close();

                                Application.Quit();
                            };

                            modeStruct.RightBtnString = "取消";
                            modeStruct.RightCallback = () => {
                                PanelManager.alertPanel.Close();

                            };

                            PanelManager.alertPanel.SetData("是否退出游戏？", modeStruct);
                        });
                    }
                    else
                    {
                        string currentPanelName = PanelManager.CurrentPanel.GetType().Name;
                        foreach (var name in ignorBackPanelList)
                        {
                            if (currentPanelName == name)
                            {
                                return;
                            }
                        }

                        SoundUtil.PlaySound(ResPathConfig.CLOSE_PANEL);
                        PanelManager.CurrentPanel.Close();
                    }
                }
            }
        }
    }
}
