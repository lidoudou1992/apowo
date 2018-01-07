using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.LoginPanel
{
    public class DirectConnectPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "DirectConnectPanel";
            }
        }

        private Button connectBtn;

        private LoginUtil loginUtil;

        protected override void Initialize(GameObject go)
        {
            loginUtil = new LoginUtil();
            connectBtn = go.transform.Find("StartGame").GetComponent<Button>();
            // 点击开始游戏按钮
            connectBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
                loginUtil.StartConnect();
            }));

            new SoundButton(go.transform.Find("QuiteBtn").gameObject, () =>
            {
                PanelManager.AlertPanel.Show(() => {
                    PropPopupModeStruct modeStruct = new PropPopupModeStruct();

                    modeStruct.Mode = EnumConfig.PropPopupPanelBtnModde.TwoBtb;

                    modeStruct.LeftBtnString = "退出";
                    modeStruct.LeftCallback = () =>
                    {
                        PanelManager.alertPanel.Close();

                        Application.Quit();
                    };

                    modeStruct.RightBtnString = "取消";
                    modeStruct.RightCallback = () =>
                    {
                        PanelManager.alertPanel.Close();

                    };

                    PanelManager.alertPanel.SetData("是否退出游戏？", modeStruct);
                });
            });
        }
    }
}
