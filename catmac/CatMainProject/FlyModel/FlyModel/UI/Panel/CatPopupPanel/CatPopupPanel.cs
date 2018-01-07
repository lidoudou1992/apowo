using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.CatPopupPanel
{
    public class CatPopupPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "CatPopup";
            }
        }

        private InputField nameTF;
        private Text colorTF;
        private Text characterTF;
        private Text powerTF;
        private Text visitTimesTF;
        private Text desTF;
        private Transform catSpine;

        private Image giftIcon;
        private Text giftName;
        private SoundButton giftBtn;

        private Model.Data.HandbookData Data;

        private GameObject spine;
        //private ShieldKeyword shieldKeyword; // 屏蔽敏感字词

        protected override void Initialize(GameObject go)
        {
            nameTF = go.transform.Find("Name/InputField").GetComponent<InputField>();
            nameTF.onValueChanged.AddListener(ShieldSensitiveWord);       // 每次输入的猫名字改变时调用方法 ShieldSensitiveWord
            nameTF.onEndEdit.AddListener(onEndEditHandler);

            colorTF = go.transform.Find("Info/Color/Color").GetComponent<Text>();
            characterTF = go.transform.Find("Info/Character/Character").GetComponent<Text>();
            powerTF = go.transform.Find("Info/Power/Power").GetComponent<Text>();
            visitTimesTF = go.transform.Find("Info/VisitTimes/VisitTimes").GetComponent<Text>();
            desTF = go.transform.Find("Info/Des/Text").GetComponent<Text>();
            catSpine = go.transform.Find("Info/Cat");

            giftIcon = go.transform.Find("GiftBtn/Gift").GetComponent<Image>();
            giftName = go.transform.Find("GiftBtn/Text").GetComponent<Text>();

            giftBtn = new SoundButton(go.transform.Find("GiftBtn").gameObject, () =>
            {
                var gift = HandbookManager.Instance.FindOneCatGiftByType(Data.GiftType);
                if (gift.IsGetted)
                {
                    PanelManager.CatGiftPopupPanel.Show(() => {
                        if (GuideManager.Instance.IsGestureTouchEffective("GiftBtn"))
                        {
                            GuideManager.Instance.ContinueGuide();
                        }

                        PanelManager.CatGiftPopupPanel.SetData(gift);
                    });
                }
                else  
                {
                    // 未获得弹窗
                    PanelManager.ShowTipString("未获得该宝贝", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                }
            });

            new SoundButton(go.transform.Find("PhotoBtn").gameObject, () =>
            {
                PanelManager.PicturesPanel.Show();
                PanelManager.InfoBar.MoveTransform();  // 关闭交叉广告
            });
        }

        public void SetData(Model.Data.HandbookData data)
        {
            Data = data;

            nameTF.text = CatData.GetCatAlias(data.Name);
            colorTF.text = data.Color;
            characterTF.text = data.Character;
            powerTF.text = data.Power.ToString();
            visitTimesTF.text = data.Count.ToString();
            desTF.text = data.Des;

            ResourceLoader.Instance.TryLoadClone(data.SpineName.ToLower(), data.SpineName, (go) =>
            {
                spine = go;
                go.transform.SetParent(catSpine, false);

                SkeletonGraphic sg = go.transform.GetComponent<SkeletonGraphic>();
                sg.AnimationState.SetAnimation(0, data.ShowAction, true);

                switch (Data.GetCatState())
                {
                    case EnumConfig.HandbookCatState.Unknow:

                        break;
                    case EnumConfig.HandbookCatState.Find_Offline:
                        Color c = ColorConfig.HAND_BOOK_MASK;
                        sg.color = c;
                        break;
                    case EnumConfig.HandbookCatState.Find_Online:
                        c = Color.white;
                        sg.color = c;
                        break;
                    default:
                        break;
                }
            });

            CatGiftData catGiftData = HandbookManager.Instance.FindOneCatGiftByType(data.GiftType);
            ResourceLoader.Instance.TryLoadPic(ResPathConfig.CAT_GIFT_ASSETBUNDLE, catGiftData.PicCode, (texture) =>
            {
                giftIcon.sprite = texture as Sprite;

                giftIcon.color = catGiftData.IsGetted ? Color.white : ColorConfig.HAND_BOOK_MASK;

                giftIcon.SetNativeSize();
            });

            giftName.text = catGiftData.IsGetted ? catGiftData.Name : "????";
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

            if (spine != null)
            {
                GameObject.DestroyImmediate(spine);
                spine = null;
            }

            base.Close(isCloseAllMode);
        }

        private void onEndEditHandler(string text)
        {
            if (text.Length>6)
            {
                PanelManager.ShowTipString("不能大于6个字", EnumConfig.PropPopupPanelBtnModde.OneBtn);
            }
            else
            {
                PlayerPrefs.SetString(Data.Name, text);
            }
        }

        //private string getCatName()
        //{
        //    if (PlayerPrefs.HasKey(Data.Name))
        //    {
        //        return PlayerPrefs.GetString(Data.Name);
        //    }
        //    return Data.Name;
        //}

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
