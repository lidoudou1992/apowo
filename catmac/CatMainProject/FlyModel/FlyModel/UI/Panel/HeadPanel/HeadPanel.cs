using Assets.Scripts;
using FlyModel.Model;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.HeadPanel
{
    public class HeadPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "HeadPanel";
            }
        }

        private Text nameTF;
        private Text UIDTF;
        private Text chengjiudianTF;
        private Text yinyuganTF;
        private Text jinyuganTF;
        private Text achievementTF;

        private Text versionTF;

        private Image headImage;

        public override bool IsRoot
        {
            get
            {
                return true;
            }
        }

        private Slider musicController;
        private Slider soundController;

        protected override void Initialize(GameObject go)
        {
            nameTF = go.transform.Find("Imagebg/Name").GetComponent<Text>();
            UIDTF = go.transform.Find("Imagebg/xinxi/UID/Text").GetComponent<Text>();
            chengjiudianTF = go.transform.Find("Imagebg/xinxi/chengjiudian/Text").GetComponent<Text>();
            yinyuganTF = go.transform.Find("Imagebg/xinxi/yinyugan/Text").GetComponent<Text>();
            jinyuganTF = go.transform.Find("Imagebg/xinxi/jinyugan/Text").GetComponent<Text>();
            achievementTF = go.transform.Find("Imagebg/xinxi/level/Text").GetComponent<Text>();

            versionTF = go.transform.Find("Imagebg/Image/banben").GetComponent<Text>();

            musicController = go.transform.Find("Imagebg/music").GetComponent<Slider>();
            musicController.value = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : 0.6f;
            musicController.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>((val) =>
            {
                GameApplication.MusicController.volume = val;
                PlayerPrefs.SetFloat("MusicVolume", val);
            }));

            soundController = go.transform.Find("Imagebg/sound").GetComponent<Slider>();
            soundController.value = PlayerPrefs.HasKey("SoundVolume") ? PlayerPrefs.GetFloat("SoundVolume") : 0.6f;
            soundController.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>((val) =>
            {
                GameApplication.SoundEffectController.volume = val;
                PlayerPrefs.SetFloat("SoundVolume", val);
            }));

            new SoundButton(go.transform.Find("Imagebg/Button_jiangli").gameObject, () =>
            {
                PanelManager.ExchangePanel.Show();
            });

            new SoundButton(go.transform.Find("Imagebg/Button_jiaocheng").gameObject, () =>
            {
                PanelManager.TutorialPanel.Show();
            });

            new SoundButton(go.transform.Find("Imagebg/Button_quit").gameObject, () =>
            {
                PanelManager.ShowTipString("是否退出游戏？", EnumConfig.PropPopupPanelBtnModde.TwoBtb, leftCallback: () =>
                {
                    Application.Quit();
                });
            });

            new SoundButton(go.transform.Find("Imagebg/Button_ChangeHead").gameObject, () =>
            {
                PanelManager.HeadSelectPanel.Show();
            });

            headImage = go.transform.Find("Imagebg/face/Image").GetComponent<Image>();
        }

        public override void Refresh()
        {
            base.Refresh();

            nameTF.text = UserManager.Instance.Me.Name;
            UIDTF.text = UserManager.Instance.Me.ID.ToString();
            chengjiudianTF.text = TaskManager.Instance.GetChievementPoints().ToString();
            yinyuganTF.text = UserManager.Instance.Me.LowCurrency.ToString();
            jinyuganTF.text = UserManager.Instance.Me.HighCurrency.ToString();
            versionTF.text = string.Format("{0}{1}.{2}", AppConfig.VERSION_PREFIX, AppConfig.MAJOR_VERSION, AppConfig.MINOR_VERSION);

            UpdateHead();
        }

        public override void RefreshWhenBack()
        {
            base.RefreshWhenBack();

            UpdateHead();
        }

        public void UpdateHead()
        {
            ResourceLoader.Instance.TryLoadPic(ResPathConfig.USER_HEAD, UserManager.Instance.GetCurrentHeadCode(), (texture) =>
            {
                headImage.sprite = texture as Sprite;
                headImage.SetNativeSize();
            });
        }

        public override void SetInfoBar()
        {
            //base.SetInfoBar();
            //PanelManager.InfoBar.SetMenuClickedHandler(() => { Close(); });
            //PanelManager.InfoBar.SetBtnImage(EnumConfig.InfoBarBtnMode.Close);
        }
    }
}
