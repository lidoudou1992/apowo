using FlyModel.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyModel.UI.Panel.LoginPanel
{
    public class DisConnectMonitor: BehaviourBase
    {
        public LoginUtil target;

        public override void Update()
        {
            base.Update();

            if (target.isDisconnect)
            {
                target.isDisconnect = false;

                PanelManager.AlertPanel.Show(() => {
                PropPopupModeStruct modeStruct = new PropPopupModeStruct();

                modeStruct.Mode = EnumConfig.PropPopupPanelBtnModde.TwoBtb;

                modeStruct.LeftBtnString = "重连";
                modeStruct.LeftCallback = () => {
                    PanelManager.alertPanel.Close();

                    GameApplication.Instance.ReconnectingCallback();
                };

                modeStruct.RightBtnString = "退出";
                modeStruct.RightCallback = () => {
                    PanelManager.alertPanel.Close();

                };

                PanelManager.alertPanel.SetData("网络断开，是否重连？", modeStruct);
            });
            }
        }
    }
}
