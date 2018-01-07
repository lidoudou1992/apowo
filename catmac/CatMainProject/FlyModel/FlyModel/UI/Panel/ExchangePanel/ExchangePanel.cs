using FlyModel.Proto;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.ExchangePanel
{
    public class ExchangePanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "ExchangePanel";
            }
        }

        private InputField codeText;
        //private ShieldKeyword shieldKeyword; // 屏蔽敏感字词

        protected override void Initialize(GameObject go)
        {
            codeText = go.transform.Find("Image/InputField").GetComponent<InputField>();
            codeText.onValueChanged.AddListener(ShieldSensitiveWord);       // 每次输入的内容改变时调用方法 ShieldSensitiveWord
            new SoundButton(go.transform.Find("Image/Button_queding").gameObject, () =>
            {
                Debug.Log(string.Format("领奖 {0}", codeText.text));
                if (string.IsNullOrEmpty(codeText.text)==false)
                {
                    PanelManager.ShowTipString("应好友邀请,可领取一份新手礼包,确定发送邀请码么？", EnumConfig.PropPopupPanelBtnModde.TwoBtb,
                        leftCallback: () =>
                        {
                            CommandHandle.Send(Proto.ServerMethod.Invitee, new IDMessage() { Id = long.Parse(codeText.text) });
                        }
                    );
                    //CommandHandle.Send(Proto.ServerMethod.Invitee, new IDMessage() { Id = long.Parse(codeText.text) });
                }
                else
                {
                    PanelManager.ShowTipString("请输入推广码", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                }
            });
        }

        public override void SetInfoBar()
        {
            
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
            codeText.text = ShieldKeyword.InputAndOutput(codeText.text);
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
