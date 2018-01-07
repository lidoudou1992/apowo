using FlyModel.UI.Component;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.TipPanel
{
    public class TipPanel:PanelBase
    {
        // 面板名字,从AssetBundle里取到预设体
        public override string AssetName
        {
            get
            {
                return "TipsPanel";
            }
        }

        //// 把TipPanel设置为根面板
        //public override bool IsRoot
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}

        public TipPanel(RectTransform parent) : base(parent)    // 设 TipRoot 为父物体
        {
        }

        public  Text tipText;   // 提示信息      
        private RectTransform buttonRt;  // 用来播放缓动动画

        private Image buttonImage;
        private Text text;
        private Image image;

        private Tweener pauseTweener;   // 暂停动画
        private Tweener moveUpTweener;  // 上移动画

        // 初始化
        protected override void Initialize(GameObject go)
        {
            tipText = go.transform.Find("Button/Text").GetComponent<Text>();    // 取到提示信息文本   
            buttonRt = go.transform.Find("Button").GetComponent<RectTransform>();   // 用来播放动画 
               
            buttonImage = go.transform.Find("Button").GetComponent<Image>();
            text = go.transform.Find("Button/Text").GetComponent<Text>();
            image = go.transform.Find("Button/Image").GetComponent<Image>();
        }

        // 设置文本内容
        public void SetText(string txtValue)
        {
            tipText.text = txtValue;
        }

        // 刷新面板(每次打开面板时自动调用)
        public override void Refresh()
        {
            base.Refresh();

            PlayPauseAnimation();
        }

        // 播放暂停动画
        private void PlayPauseAnimation()
        {
            pauseTweener = buttonRt.DOLocalMoveY(-382, 0.5f);  // 在初始位置停留0.5f秒
            pauseTweener.OnComplete(() => PlayMoveUpAnimation());
        }

        // 播放上移动画
        private void PlayMoveUpAnimation()
        {
            moveUpTweener = buttonRt.DOLocalMoveY(-342, 0.5f);   // 动画默认先快后慢

            // 渐隐
            buttonImage.material.DOFade(0, 0.5f);
            text.material.DOFade(0, 0.5f);
            image.material.DOFade(0, 0.5f);

            //moveUpTweener.OnComplete(() => PanelManager.tipPanel.Close());
            moveUpTweener.OnComplete(() => Close());
        }

        // 重写Close方法
        // 关闭面板前重置提示的位置和阿尔法值
        public override void Close(bool isCloseAllMode = false)
        {
            buttonRt.localPosition = new Vector3(0, -382, 0);   
            buttonImage.material.DOFade(1, 0);
            text.material.DOFade(1, 0);
            image.material.DOFade(1, 0);

            base.Close(isCloseAllMode);
        }

        public override void SetInfoBar()
        {
        }
    }
}
