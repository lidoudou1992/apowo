using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.AdvisePanel
{
    public class AdvisePanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "SettingPanel_advice";
            }
        }

        private InputField contentTF;
        //private ShieldKeyword shieldKeyword; // 屏蔽敏感字词

        protected override void Initialize(GameObject go)
        {
            contentTF = go.transform.Find("Content").GetComponent<InputField>();
            contentTF.onValueChanged.AddListener(ShieldSensitiveWord);       // 每次输入的内容改变时调用方法 ShieldSensitiveWord

            new SoundButton(go.transform.Find("Button").gameObject, () =>
            {
                if (string.IsNullOrEmpty(contentTF.text)==false)
                {
                    CommandHandle.Send(Proto.ServerMethod.Advise, new Proto.AdviseData() { Content = contentTF.text });
                    contentTF.text = "";
                    Close();
                }
                else
                {
                    PanelManager.ShowTipString("请输入内容", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                }
            });
        }

        public override void SetInfoBar()
        {
            
        }

        public override void Refresh()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.ShowMenuBtn(false);
            }
        }

        public override void Close(bool isCloseAllMode = false)
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.ShowMenuBtn(true);
            }

            base.Close(isCloseAllMode);
        }

        /// <summary>
        /// 屏蔽敏感字词
        /// </summary>
        /// <param name="arg0"></param>
        private void ShieldSensitiveWord(string arg0)
        {
            ////Debug.Log("1");
            ////Debug.Log(nameTxt.text);
            ////Debug.Log("2");
            ////nameTxt.text = shieldKeyword.InputAndOutput(nameTxt.text);
            //shieldKeyword = new ShieldKeyword();    // 这是类，要先分配内存也就是new一个实例，报错好多次才找到这个原因,引以为戒
            //Debug.Log(string.Format("未屏蔽的nameTxt.text:{0}", nameTxt.text));
            contentTF.text = ShieldKeyword.InputAndOutput(contentTF.text);
            //string str = shieldKeyword.InputAndOutput(nameTxt.text);
            //nameTxt.text = str;
            //Debug.Log(string.Format("str:{0}", str));
            //Debug.Log(string.Format("屏蔽过后的nameTxt.text:{0}", nameTxt.text));
            //nameTxt.text = str;
            //Debug.Log(string.Format("再来屏蔽过后的nameTxt.text:{0}", nameTxt.text));
            //nameTF.text = "123";
        }
    }
}
