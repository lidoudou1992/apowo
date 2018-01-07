using Assets.Scripts.TouchController;
using FlyModel.UI.Component;
using FlyModel.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.GuidePanel
{
    public class TutorialPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "TutorialPanel";
            }
        }

        private GameObject catContent;
        private GameObject toyContent;
        private GameObject foodContent;
        private GameObject handbookContent;
        private GameObject pictureContent;

        private ScrollRect scroller;
        private GameObject contentContainer;

        private enum showType
        {
            cat = 0,
            toy = 1,
            food = 2,
            handbook = 3,
            picture = 4
        }

        private showType mode;

        private Text contentTitleTF;

        protected override void Initialize(GameObject go)
        {
            catContent = go.transform.Find("Content/Scroller/CatContent").gameObject;
            toyContent = go.transform.Find("Content/Scroller/ToyContent").gameObject;
            foodContent = go.transform.Find("Content/Scroller/FoodContent").gameObject;
            handbookContent = go.transform.Find("Content/Scroller/HandbookContent").gameObject;
            pictureContent = go.transform.Find("Content/Scroller/PictureContent").gameObject;

            scroller = go.transform.Find("Content/Scroller").GetComponent<ScrollRect>();

            contentContainer = go.transform.Find("Content").gameObject;

            new SoundButton(go.transform.Find("Buttons/Cat").gameObject, () =>
            {
                ShowContent(showType.cat);
            });

            new SoundButton(go.transform.Find("Buttons/Toy").gameObject, () =>
            {
                ShowContent(showType.toy);
            });

            new SoundButton(go.transform.Find("Buttons/Food").gameObject, () =>
            {
                ShowContent(showType.food);
            });

            new SoundButton(go.transform.Find("Buttons/Handbook").gameObject, () =>
            {
                ShowContent(showType.handbook);
            });

            new SoundButton(go.transform.Find("Buttons/Picture").gameObject, () =>
            {
                ShowContent(showType.picture);
            });

            new SoundButton(go.transform.Find("Buttons/Quite").gameObject, () =>
            {
                //Close();
                PanelManager.AdivisPanel.Show();
            });

            new SoundButton(go.transform.Find("Content/Button").gameObject, () =>
            {
                contentContainer.SetActive(false);
            });

            contentTitleTF = go.transform.Find("Content/Title").GetComponent<Text>();
        }

        private void ShowContent(showType mode)
        {
            this.mode = mode;

            if (contentContainer.activeSelf==false)
            {
                contentContainer.SetActive(true);
            }

            switch (mode)
            {
                case showType.cat:
                    scroller.content = catContent.GetComponent<RectTransform>();
                    contentTitleTF.text = "喵星人";
                    break;
                case showType.toy:
                    scroller.content = toyContent.GetComponent<RectTransform>();
                    contentTitleTF.text = "玩具";
                    break;
                case showType.food:
                    scroller.content = foodContent.GetComponent<RectTransform>();
                    contentTitleTF.text = "猫粮";
                    break;
                case showType.handbook:
                    scroller.content = handbookContent.GetComponent<RectTransform>();
                    contentTitleTF.text = "图鉴";
                    break;
                case showType.picture:
                    scroller.content = pictureContent.GetComponent<RectTransform>();
                    contentTitleTF.text = "更换场景";
                    break;
                default:
                    break;
            }

            scroller.verticalNormalizedPosition = 1;

            catContent.SetActive(mode == showType.cat);
            toyContent.SetActive(mode == showType.toy);
            foodContent.SetActive(mode == showType.food);
            handbookContent.SetActive(mode == showType.handbook);
            pictureContent.SetActive(mode == showType.picture);
        }

        public override void SetInfoBar()
        {
            
        }
        
    }
}
