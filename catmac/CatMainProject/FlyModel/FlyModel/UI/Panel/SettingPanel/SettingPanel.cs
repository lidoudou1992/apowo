using FlyModel.Model;
using FlyModel.Proto;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.SettingPanel
{
    public class SettingPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "SettingPanel";
            }
        }

        public override bool IsRoot
        {
            get
            {
                return true;
            }
        }

        private Slider musicController;
        private Slider soundController;
        private GameObject newMailIcon;

        protected override void Initialize(GameObject go)
        {
            musicController = go.transform.Find("music").GetComponent<Slider>();
            musicController.value = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : 0.6f;
            musicController.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>((val) =>
            {
                GameApplication.MusicController.volume = val;
                PlayerPrefs.SetFloat("MusicVolume", val);
            }));

            soundController = go.transform.Find("sound").GetComponent<Slider>();
            soundController.value = PlayerPrefs.HasKey("SoundVolume") ? PlayerPrefs.GetFloat("SoundVolume") : 0.6f;
            soundController.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>((val) =>
            {
                GameApplication.SoundEffectController.volume = val;
                PlayerPrefs.SetFloat("SoundVolume", val);
            }));

            new SoundButton(go.transform.Find("Button_yijian").gameObject, () =>
            {
                PanelManager.AdivisPanel.Show();
            });

            newMailIcon = go.transform.Find("Button_set/new").gameObject;
            new SoundButton(go.transform.Find("Button_set").gameObject, () =>
            {
                //PanelManager.MailPanel.Show();
                CommandHandle.Send(ServerMethod.GetMails, null);
            });

            new SoundButton(go.transform.Find("Button_quit").gameObject, () =>
            {
                PanelManager.ShowTipString("是否退出游戏？", EnumConfig.PropPopupPanelBtnModde.TwoBtb, leftCallback: () =>
                {
                    Application.Quit();
                });
            });

            new SoundButton(go.transform.Find("Button_jiaocheng").gameObject, () =>
            {
                PanelManager.TutorialPanel.Show();
            });

            new SoundButton(go.transform.Find("Button_jiangli").gameObject, () =>
            {
                
            });

            //new SoundButton(go.transform.Find("buyBtn").gameObject, () =>
            //{
            //    CommandHandle.Send(ServerMethod.AddMoney, new MoneyData() { Coin = 500, Dollar = 500 });
            //});
            //go.transform.Find("buyBtn").gameObject.SetActive(false);

            //PanelManager.BringSystemBarToTop();
        }

        public override void Refresh()
        {
            base.Refresh();

            //nameTF.text = UserManager.Instance.Me.Name;

            
        }

        //public override void Load()
        //{
        //    base.Load();

        //    PanelManager.LoadSystemBar(transform);
        //}

        public override void SetInfoBar()
        {
            //base.SetInfoBar();
            //PanelManager.InfoBar.SetMenuClickedHandler(() => { Close(); });
            //PanelManager.InfoBar.SetBtnImage(EnumConfig.InfoBarBtnMode.Close);
        }
    }
}
