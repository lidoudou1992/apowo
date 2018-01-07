using FlyModel.Model;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.SignPanel
{
    public class SignCell
    {
        public GameObject GameObejct;

        private GameObject signMarkImage;

        public int Index;

        public EnumConfig.SignState State;

        public SignCell(GameObject go)
        {
            GameObejct = go;

            signMarkImage = go.transform.Find("over").gameObject;

            new SoundButton(go, () =>
            {
                StartSign();
            });
        }

        public void UpdateState()
        {
            if (Index <= UserManager.Instance.Me.signDays)
            {
                //已签到
                State = EnumConfig.SignState.signed;
            }else
            {
                if (UserManager.Instance.Me.todayHasSigned)
                {
                    //未到时间，不能签到
                    State = EnumConfig.SignState.waitSign;
                }
                else
                {
                    if (Index == UserManager.Instance.Me.signDays+1)
                    {
                        //可以签到
                        State = EnumConfig.SignState.canSign;
                    }
                    else
                    {
                        //时间未到，不能签到
                        State = EnumConfig.SignState.waitSign;
                    }
                }
            }

            switch (State)
            {
                case EnumConfig.SignState.signed:
                    signMarkImage.SetActive(true);
                    break;
                case EnumConfig.SignState.canSign:
                    signMarkImage.SetActive(false);
                    break;
                case EnumConfig.SignState.waitSign:
                    signMarkImage.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        public void StartSign()
        {
            switch (State)
            {
                case EnumConfig.SignState.signed:
                    Debug.Log("已签到");
                    // 显示已签到提示
                    {
                        PanelManager.TipPanel.Show(() =>
                        {
                            PanelManager.tipPanel.SetText("今日已经签到过了~");
                            //Debug.Log("提示呢");
                        });
                    }
                    break;
                case EnumConfig.SignState.canSign:
                    Debug.Log("签到");
                    // 发送签到消息给服务器
                    CommandHandle.Send(Proto.ServerMethod.DailySign, null);
                    break;
                case EnumConfig.SignState.waitSign:
                    Debug.Log("时间未到");
                    var data = UserManager.Instance.Me; // 取得本客户端的签到数据
                    Debug.Log("是否可以签到：" + !data.todayHasSigned + "；已签到次数：" + data.signDays);
                    // 显示时间未到提示
                    {
                        PanelManager.TipPanel.Show(() =>
                        {
                            PanelManager.tipPanel.SetText("时间未到");
                        });
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
