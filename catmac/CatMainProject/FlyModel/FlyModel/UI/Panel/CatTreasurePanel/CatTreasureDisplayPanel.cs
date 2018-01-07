using DG.Tweening;
using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI.Component;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.CatTreasurePanel
{
    public class CatTreasureDisplayPanel : PanelBase
    {
        // 找到面板资源
        public override string AssetName
        {
            get
            {
                return "catbaby";
            }
        }

        public SkeletonGraphic skeletonGraphic;
        public SkeletonDataAsset skeletonDataAsset;
        public SkeletonAnimation animation;
        public Spine.Animation startAnimation;

        private Text nameTxt;    // 猫咪珍宝的名字
        private Text desTxt; // 猫咪珍宝的描述
        private Image treasureImage;    // 猫咪珍宝的图片
        private Image catImage; // 猫咪图片

        public DelayController delayController;

        Spine.AnimationState state;

        public GameObject Go;

        public event Spine.AnimationState.EventDelegate animEvent;
        private Spine.Event e;

        // 设 catTreasureRoot 为父物体
        public CatTreasureDisplayPanel(RectTransform parent) : base(parent)
        {
        }

        // 初始化面板
        protected override void Initialize(GameObject go)
        {
            skeletonGraphic = go.transform.Find("Bg").
                GetComponent<SkeletonGraphic>();
            //skeletonDataAsset = skeletonGraphic.GetComponent<SkeletonDataAsset>();
            //skeletonDataAsset = GameObject.Find("catbaby_SkeletonData").
            //    GetComponent<SkeletonDataAsset>();
            //animation = skeletonGraphic.GetComponent<SkeletonAnimation>();
            //startAnimation = animation.skeleton.Data.FindAnimation("begin");

            //Debug.Log(string.Format("(Initialize1)Name.color:{0}", name.color));
            //name = go.transform.Find("Name").GetComponent<Text>();
            //Debug.Log(string.Format("(Initialize)Name.color:{0}", name.color));

            nameTxt = go.transform.Find("Name").GetComponent<Text>();
            desTxt = go.transform.Find("Des").GetComponent<Text>();
            treasureImage = go.transform.Find("GiftIcon").GetComponent<Image>();
            catImage = go.transform.Find("Cat").GetComponent<Image>();
            Debug.Log(string.Format("Name:{0}", nameTxt.name));

            delayController = go.AddComponent<DelayController>();

            //Go = go;
        }

        // 刷新面板(每次打开面板时自动调用)
        public override void Refresh()
        {
            base.Refresh();
            //if (null == skeletonGraphic)
            //{
            //    Debug.Log("123");
            //}
            //Debug.Log(string.Format("动画名字：{0}", skeletonGraphic));
            //Debug.Log(string.Format("动画名字：{0}", skeletonDataAsset));
            //Debug.Log(string.Format("动画名字：{0}", animation));
            //Debug.Log(string.Format("动画名字：{0}", startAnimation));
            //animation.state.SetAnimation(0, startAnimation, false);

            //delayController.DelayInvoke(() =>
            //{               
            //    DisplayInfo();
            //}, 1.533f);

            DisplayInfo();

            TrackEntry trackEntry = skeletonGraphic.AnimationState.SetAnimation(0, "begin", false);
            //skeletonGraphic.AnimationState.AddAnimation(0, "play", true,7.7f);

            //startAnimation = animation.skeleton.Data.FindAnimation("begin");

            //animEvent = new Spine.AnimationState.EventDelegate(skeletonGraphic.AnimationState,0,e);
            trackEntry.Event += TrackEntry_Event;

            //Debug.Log(string.Format("(Refresh)Name:{0}", name.name));
            //Debug.Log(string.Format("(Refresh)Name.color:{0}", name.color));

            //name.SetColor(new Color(251, 250, 196, 255));
            //Debug.Log(string.Format("(Refresh)Name.color2:{0}", name.color));
            //name.CrossFadeAlpha(255f, 0f, false);
            //Debug.Log(string.Format("(Refresh)Name.color3:{0}", name.color));
            //name.DOFade(255, 5f);
            //Debug.Log(string.Format("(Refresh)Name.color4:{0}", name.color));
        }

        // 需要的方法
        private void TrackEntry_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
        {
            if (e.Data.Name == "over")
            {
                skeletonGraphic.AnimationState.SetAnimation(0, "play", true);
            }
        }

        // 显示宝贝图片、名字、描述和猫的图片
        private void DisplayInfo()
        {
            //nameTxt.DOFade(255, 1f);
            //desTxt.DOFade(255, 1f);
            //treasureImage.DOFade(255, 1f);
            //catImage.DOFade(255, 1f);

            nameTxt.material.DOFade(0, 0);
            desTxt.material.DOFade(0, 0);
            treasureImage.material.DOFade(0, 0);
            catImage.material.DOFade(0, 0);

            delayController.DelayInvoke(() =>
            {
                nameTxt.material.DOFade(1, 1f);
                desTxt.material.DOFade(1, 1f);
                treasureImage.material.DOFade(1, 1f);
                catImage.material.DOFade(1, 1f);
            }, 1.533f);

            Debug.Log(string.Format("nameTxt:{0}", nameTxt.text));
        }

        private void DisplayText()
        {
            //name.color = new Color(251, 250, 196, 255);
            //Debug.Log(string.Format("Name.color:{0}", name.color));

            //Debug.//Debug.Log(string.Format("Name.color:{0}", name.color));
            nameTxt.DOFade(255, 0f);
        }

        // 隐掉信息条
        public override void SetInfoBar()
        {
        }

        // 重写关闭按钮点击方法
        public override void OnCloseButtonClick()
        {
            // 隐掉宝贝信息
            nameTxt.material.DOFade(0, 0f);
            desTxt.material.DOFade(0, 0f);
            treasureImage.material.DOFade(0, 0f);
            catImage.material.DOFade(0, 0f);

            Close();

            CatTreasureGetPanel.catGiftDataList.RemoveAt(0);

            if (CatTreasureGetPanel.catGiftDataList != null && CatTreasureGetPanel.catGiftDataList.Count != 0)
            {
                PanelManager.CatTreasureDisplayPanel.Show(() => {
                    PanelManager.catTreasureDisplayPanel.SetData(CatTreasureGetPanel.catGiftDataList[0]);
                });
            }
        }

        // 设置猫咪珍藏的数据
        public void SetData(Model.Data.CatGiftData treasureData)
        {
            nameTxt.text = treasureData.Name;
            desTxt.text = treasureData.Description;

            // 加载猫咪珍宝图片
            ResourceLoader.Instance.TryLoadPic(ResPathConfig.CAT_GIFT_ASSETBUNDLE, treasureData.PicCode, (treasureTexture) =>
              {
                  treasureImage.sprite = treasureTexture as Sprite;
                  treasureImage.SetNativeSize();
              });

            // 加载猫咪图片
            ResourceLoader.Instance.TryLoadPic(ResPathConfig.CAT_HEAD_ASSETBUNDLE, treasureData.CatPicCode, (treasureTexture) =>
            {
                catImage.sprite = treasureTexture as Sprite;
                catImage.SetNativeSize();
            });
        }

        //// 杀死
        //public void Conceal()
        //{
        //    Go.SetActive(false);
        //}

        //// 激活 
        //public void MyActive()
        //{
        //    Go.SetActive(true);
        //}
    }
}
