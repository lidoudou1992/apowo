using Assets.Scripts.TouchController;
using DG.Tweening;
using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Component;
using FlyModel.UI.Component.PageView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.HandbookPanel
{
    public class HandbookPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "HandbookPanel";
            }
        }

        public override bool IsRoot
        {
            get
            {
                return true;
            }
        }

        private GameObject pageViewGameObject;
        private PageView pageViewScript;
        private GameObject pageItemPrefb;
        private GameObject cellItemPrefab;
        private RectTransform content;

        private SoundButton prevBtn;
        private SoundButton nextBtn;

        public int count;
        public int pagesCount;
        public float itemsOfPage = 9f;

        public List<HandbookData> handbookDataList = HandbookManager.Instance.catsList;

        public EnumConfig.HandbookMode mode = EnumConfig.HandbookMode.Cat;

        private Vector2 screenSize;

        public float PageWidth = 640;
        public float PageHeight = 900;
        public float GapH = 5;
        public float ContentWidth;

        //private PageMark pageMark;

        private Text countTF;

        protected override void Initialize(GameObject go)
        {
            pageViewGameObject = go.transform.Find("PageView").gameObject;
            pageViewScript = new PageView();
            pageViewScript.GameObject = pageViewGameObject;
            BehaviourManager.AddGameComponent(pageViewScript);

            pageViewScript.OnPageChanged = OnPageChanged;
            pageViewScript.OnCreatePageItem = createOnePage;

            DragSensor dragSensor = pageViewGameObject.AddComponent<DragSensor>();
            dragSensor.OnBeinDragHandler = pageViewScript.OnBeginDrag;
            dragSensor.OnDragHandler = pageViewScript.OnDrag;
            dragSensor.OnEndDragHandler = pageViewScript.OnEndDrag;

            content = pageViewGameObject.transform.Find("Content").GetComponent<RectTransform>();

            layoutDoor();

            showCatMode();

            prevBtn = new SoundButton(go.transform.Find("Arrows/Left").gameObject, toPrevHandler, ResPathConfig.PAGE_CHANGE_BUTTON);
            nextBtn = new SoundButton(go.transform.Find("Arrows/Right").gameObject, toNextHandler, ResPathConfig.PAGE_CHANGE_BUTTON);

            //页码控制器
            //GameObject pageMarkGameObject = pageViewGameObject.transform.Find("PageMark").gameObject;
            //pageMark = new PageMark();
            //pageMark.GameObject = pageMarkGameObject;
            //pageMark.Init(pageViewScript, go.transform.Find("PageMarkItem").gameObject);

            countTF = go.transform.Find("Count/Text").GetComponent<Text>();

            PanelManager.BringSystemBarToTop();

            go.AddComponent<DelayController>().DelayInvoke(() =>
            {
                if (GuideManager.Instance.IsGestureTouchEffective("PicBook"))
                {
                    GuideManager.Instance.ContinueGuide();
                }
            }, 0.8f);
        }

        //protected override void InitializeOver()
        //{
        //    base.InitializeOver();

        //    PanelManager.CurrentPanel.Close();
        //}

        private void layoutDoor()
        {
            CameraAbsoluteResolution cameraAbsoluteResolution = GameObject.Find("Main Camera").GetComponent<CameraAbsoluteResolution>();
            screenSize = cameraAbsoluteResolution.GetScreenPixelDimensions();

            PageWidth = screenSize.x * 1136 / screenSize.y;
        }

        private void showCatMode()
        {
            pageItemPrefb = PanelPrefab.transform.Find("PageView/Content/PageItem").gameObject;
            pageItemPrefb.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(PageWidth, PageHeight);

            cellItemPrefab = PanelPrefab.transform.Find("CellItem").gameObject;
        }

        private void changeMode(EnumConfig.HandbookMode mode)
        {
            this.mode = mode;
            showCatMode();

            Clear();
            refreshData();
            pageViewScript.JumpToPage(0);
        }

        public override void Load()
        {
            base.Load();

            PanelManager.LoadSystemBar(transform);
        }

        public override void Refresh()
        {
            base.Refresh();

            refreshData();

            int page = 0;

            if (GuideManager.Instance.HandbookAppointCatType>0)
            {
                int index = -1;
                List<HandbookData> allCats = HandbookManager.Instance.catsList;
                for (int i = 0; i < allCats.Count; i++)
                {
                    if (allCats[i].Type == GuideManager.Instance.HandbookAppointCatType)
                    {
                        index = i;
                        break;
                    }
                }
                page = (int)Mathf.Ceil(index / 9f) - 1;
            }

            pageViewScript.JumpToPage(page, false);

            countTF.text = string.Format("{0}/{1}", HandbookManager.Instance.GetAppearCount(), HandbookManager.Instance.catsList.Count);
        }

        private void refreshData()
        {
            count = handbookDataList.Count;

            pagesCount = (int)Mathf.Ceil(count / itemsOfPage);
            ContentWidth = (pagesCount - 1) * GapH + PageWidth * pagesCount;
            content.sizeDelta = new Vector2(ContentWidth, PageHeight);

            pageViewScript.PageCount = pagesCount;
            pageViewScript.InitPages();
        }

        private void createOnePage(int pageIndex)
        {
            List<BaseProp> datas = new List<BaseProp>();
            for (int i = pageIndex * (int)itemsOfPage; i < pageIndex * itemsOfPage + itemsOfPage && i < count; i++)
            {
                datas.Add(handbookDataList[i]);
            }

            PageItem pageItem = CreatePageItem(pageIndex);
            pageItem.InitItems(datas, pageIndex);
        }

        private void createHandbookCellItem(BaseProp data, PageItem pageItem, int itemIndex)
        {
            GameObject ItemPrefabInstance = GameObject.Instantiate(cellItemPrefab);
            ItemPrefabInstance.SetActive(true);
            ItemPrefabInstance.transform.SetParent(pageItem.GameObject.transform.Find("Panel"), false);

            PointerSensor pointerSensor = ItemPrefabInstance.AddComponent<PointerSensor>();

            IItem cellItem = null;

            cellItem = new HandbookCellItem(ItemPrefabInstance);
            pointerSensor.OnPointerClickHandler = ((HandbookCellItem)cellItem).OnPointerClick;

            cellItem.Init(data, pageViewScript.TempPageIndex, itemIndex);

            pageViewScript.pageItems[pageViewScript.TempPageIndex].listItem.Add(cellItem);
        }

        private PageItem CreatePageItem(int pageIndex)
        {
            GameObject pageItemPrefabInstance = UnityEngine.Object.Instantiate(pageItemPrefb);
            pageItemPrefabInstance.SetActive(true);
            pageItemPrefabInstance.transform.localPosition = new Vector3((GapH + PageWidth) * pageIndex, 0, 0);
            pageItemPrefabInstance.transform.SetParent(pageItemPrefb.transform.parent, false);

            PageItem pi = new PageItem(pageItemPrefabInstance);
            pi.OnCreatCellItemCallback = createHandbookCellItem;

            pageViewScript.pageItems[pageIndex] = pi;

            return pi;
        }

        public void Clear()
        {
            pageViewScript.ClearPageItems();

            pageViewScript.InitOver = false;
        }

        public override void Dispose()
        {
            base.Dispose();
            Clear();
        }

        private void OnPageChanged(int index)
        {
            Debug.Log("page changed " + index.ToString());
        }

        public void toPrevHandler()
        {
            pageViewScript.ToPrevPage();
        }

        public void toNextHandler()
        {
            pageViewScript.ToNextPage();
        }

        public override void SetInfoBar()
        {
            base.SetInfoBar();
            PanelManager.InfoBar.SetMenuClickedHandler(() => { Close(); });
            PanelManager.InfoBar.SetBtnImage(EnumConfig.InfoBarBtnMode.Close);
        }

        public HandbookCellItem FindGuidePropCom(int type)
        {
            type = GuideManager.Instance.HandbookAppointCatType;
            GuideManager.Instance.HandbookAppointCatType = -1;

            List<IItem> list = pageViewScript.GetCurrentPageItem().listItem;
            HandbookCellItem temp;
            for (int i = 0; i < list.Count; i++)
            {
                temp = list[i] as HandbookCellItem;
                
                if (temp.Data.Type == type)
                {
                    return temp;

                }
            }

            return null;
        }
    }
}
