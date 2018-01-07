using FlyModel.Model;
using FlyModel.UI.Component;
using FlyModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyModel.Proto;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.GuidePanel
{
    public class GuidePanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "GuidePanel";
            }
        }

        private Text dialogTF;

        private string dialogStr;
        private int writeTimes;

        private TypewriterUtil typewriterUtil;

        public static GameObject GestureGO;

        private DelayController delayController;
        // 引导配置文件的序号
        // 序号1代表新手引导配置文件
        // 序号2代表猫宝贝介绍，但从不出现，先放一放
        // 序号3代表房屋扩建的配置文件
        // 在GuideManager.cs的StartGuideByServer方法和StartGuideManul方法分别设置新手引导和房屋扩建引导的序号
        public int guideConfigIndex = 0;

        protected override void Initialize(GameObject go)
        {
            transform.SetParent(PanelManager.GuideMaskRectTransform, false);
            // 取得手势对象
            GestureGO = go.transform.Find("Gesture").gameObject;

            new SoundButton(go.transform.Find("Dialog/Button").gameObject, () => {
                //GuideManager.Instance.DoNext();
                doNext();
            });

            dialogTF = go.transform.Find("Dialog/Text").GetComponent<Text>();

            typewriterUtil = new TypewriterUtil();
            typewriterUtil.WriteCallback = writting;

            new SoundButton(go.transform.Find("Button").gameObject, () =>
            {
                doNext();
            });

            delayController = go.AddComponent<DelayController>();
            //new SoundButton(go.transform.Find("ButtonSkip").gameObject, () =>
            //{
            //    SkipGuide();
            //});
        }

        // 跳过引导直接获得奖励
        // 出现问题：关闭后会开始下一步引导，先放一放
        private void SkipGuide()
        {
            // 关闭引导画面
            Close();
            // 发送跳过命令
            // 自动获得应得奖励
            Debug.Log("引导配置文件的索引：" + guideConfigIndex);
            CommandHandle.Send(ServerMethod.SkipGuide, new IDMessage() {Id = guideConfigIndex});
        }

        public override bool Show()
        {
            IsNeedPushToPanelStack = false;
            bool flag = base.Show();
            
            return flag;
        }

        public override void Close(bool isCloseAllMode = false)
        {
            Hide();
            Dispose();

            updateAwardBtnActive();
            UpdateCamareBtnActive();
            UpdateMailBtnActive();
            //UpdateSettingBtnActive();
            UpdateSignBtnActive();
            UpdateShareBtnActive();
            UpdateExchangeBtnActive();
            UpdateTeachBtnActive();
            UpdateLotterBtnActive();  // 抽奖
            UpdateChitchatBtnActive();  // 聊天

            PanelManager.InfoBar.MoveTransform();  // 关闭交叉广告
            BehaviourManager.RemoveBehaviour(typewriterUtil);
        }
        
        public override void SetInfoBar()
        {
            
        }

        public void Say(string content)
        {
            dialogStr = content;
            writeTimes = 0;
            dialogTF.text = "";

            delayController.DelayInvoke(() =>
            {
                BehaviourManager.AddGameComponent(typewriterUtil);
            }, 0.2f);
        }

        private void writting()
        {
            if (writeTimes < dialogStr.Length)
            {
                dialogTF.text = dialogStr.Substring(0, writeTimes + 1);
                writeTimes++;
            }
            else
            {
                BehaviourManager.RemoveBehaviour(typewriterUtil);
            }
        }

        private void doNext()
        {
            if (writeTimes < dialogStr.Length)
            {
                //没说完
                BehaviourManager.RemoveBehaviour(typewriterUtil);
                dialogTF.text = dialogStr;
                // 对话长度
                writeTimes = dialogStr.Length;
            }
            else
            {
                GuideManager.Instance.DoNext();
            }
        }
    }
}
