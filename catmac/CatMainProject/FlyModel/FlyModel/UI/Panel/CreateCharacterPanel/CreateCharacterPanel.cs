using FlyModel.Proto;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace FlyModel.UI.Panel.CreateCharacterPanel
{
    public class CreateCharacterPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "CreateCharaterPanel";
            }
        }

        private InputField nameTF;

        private Button createBtn;
        private SoundButton createSoundBtn;
        private Text nameTxt;   // 角色名
        //private ShieldKeyword shieldKeyword; // 屏蔽敏感字词

        protected override void Initialize(GameObject go)
        {
            nameTF = go.transform.Find("Name").GetComponent<InputField>();
            nameTF.onValueChanged.AddListener(onInput);
            nameTxt = go.transform.Find("Name/Text").GetComponent<Text>();
            nameTF.onValueChanged.AddListener(ShieldSensitiveWord);       // 每次输入的角色名改变时调用方法 ShieldSensitiveWord 
            //nameTxt.text = "渴了";

            createBtn = go.transform.Find("CreateButton").GetComponent<Button>();

            nameTF.text = RandomName.CreateSurname();   // 随机生成姓名
            createSoundBtn = new SoundButton(createBtn.gameObject, () =>
            {
                if (string.IsNullOrEmpty(nameTF.text))
                {
                    PanelManager.ShowTipString("角色名不能为空", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                }
                else
                {
                    string name = nameTF.text;
                    if (name.Length > 10)
                    {
                        PanelManager.ShowTipString("角色名最多10个字", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    }
                    else if (nameTxt.text.Contains("*"))
                    {
                        // 名字非法弹窗
                        PanelManager.ShowTipString("非法字符无法创角", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    }
                    else
                    {
                        PlayerPrefs.SetString("account", nameTF.text);
                        Debug.Log(PlayerPrefs.GetString("account"));
                        CommandHandle.Send(ServerMethod.CreateCharacter, new CreateCharacter() { NickName = PlayerPrefs.GetString("account"), Gender = Proto.Gender.女 });
                        Close();
                    }
                }
            });
        }

        private void onInput(string s)
        {
            createBtn.interactable = true;
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
            nameTF.text = ShieldKeyword.InputAndOutput(nameTF.text);
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
