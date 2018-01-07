using FlyModel.UI.Component;
using FlyModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.Alert
{
    public class AlertPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "AlertPanel";
            }
        }

        private PropPopupModeStruct modeStruct;

        private Text text;

        private SoundButton leftBtn;
        private SoundButton rightBtn;
        private SoundButton middleBtn;

        protected override void Initialize(GameObject go)
        {
            text = go.transform.Find("Text").GetComponent<Text>();

            leftBtn = new SoundButton(go.transform.Find("ButtonGroup/Left").gameObject);
            leftBtn.SetActive(false);
            rightBtn = new SoundButton(go.transform.Find("ButtonGroup/Right").gameObject);
            rightBtn.SetActive(false);
            middleBtn = new SoundButton(go.transform.Find("ButtonGroup/Middle").gameObject);
            middleBtn.SetActive(false);
        }

        public void SetData(string alertString, PropPopupModeStruct modeStruct)
        {
            SoundUtil.PlaySound(ResPathConfig.SHOW_ALERT);

            this.modeStruct = modeStruct;

            text.text = alertString;

            if (modeStruct.Mode == EnumConfig.PropPopupPanelBtnModde.TwoBtb)
            {
                leftBtn.GameObject.transform.Find("Text").GetComponent<Text>().text = modeStruct.LeftBtnString;
                rightBtn.GameObject.transform.Find("Text").GetComponent<Text>().text = modeStruct.RightBtnString;

                leftBtn.SetCallback(modeStruct.LeftCallback);
                rightBtn.SetCallback(modeStruct.RightCallback);

                leftBtn.SetActive(true);
                rightBtn.SetActive(true);
                middleBtn.SetActive(false);
            }
            else if (modeStruct.Mode == EnumConfig.PropPopupPanelBtnModde.OneBtn)
            {
                middleBtn.GameObject.transform.Find("Text").GetComponent<Text>().text = modeStruct.MiddleBtnString;
                if (modeStruct.MiddleCallback == null)
                {
                    modeStruct.MiddleCallback = () => { PanelManager.CurrentPanel.Close(); };
                }
                middleBtn.SetCallback(modeStruct.MiddleCallback);

                leftBtn.SetActive(false);
                rightBtn.SetActive(false);
                middleBtn.SetActive(true);
            }
        }

        public override void SetInfoBar()
        {

        }
    }
}
