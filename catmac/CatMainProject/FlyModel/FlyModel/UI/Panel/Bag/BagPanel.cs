using Assets.Scripts.TouchController;
using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Component;
using FlyModel.UI.Component.PageView;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.Bag
{
    public class BagPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "BagPanel";
            }
        }

        public override bool IsRoot
        {
            get
            {
                return true;
            }
        }

        public int count;
        public int pagesCount;
        public float itemsOfPage = 9;
        public List<BagItemData> itemDatas;

        public GameObject pageItemPrefb;
        public GameObject cellItemPrefab;
        private GameObject pageViewGameObject;
        private RectTransform content;

        private PageView pageViewScript;

        private SoundButton prevBtn;
        private SoundButton nextBtn;

        public float PageWidth = 640;
        public float PageHeight = 822;
        public float GapH = 0;
        public float ContentWidth;

        private PageMark pageMark;
        protected override void Initialize(GameObject go)
        {
            initBagUI(go);

            //页码控制器
            GameObject pageMarkGameObject = pageViewGameObject.transform.Find("PageMark").gameObject;
            pageMark = new PageMark();
            pageMark.GameObject = pageMarkGameObject;

            pageMark.Init(pageViewScript, go.transform.Find("PageMarkItem").gameObject);

            PanelManager.BringSystemBarToTop();
        }

        //protected override void InitializeOver()
        //{
        //    base.InitializeOver();

        //    PanelManager.CurrentPanel.Close();
        //}

        public override void Load()
        {
            base.Load();

            PanelManager.LoadSystemBar(transform);
        }

        public void initBagUI(GameObject go)
        {
            CameraAbsoluteResolution cameraAbsoluteResolution = GameObject.Find("Main Camera").GetComponent<CameraAbsoluteResolution>();
            Vector2 screenSize = cameraAbsoluteResolution.GetScreenPixelDimensions();
            PageWidth = screenSize.x * 1136 / screenSize.y;

            pageViewGameObject = go.transform.Find("PageView").gameObject;
            DragSensor dragSensor = pageViewGameObject.AddComponent<DragSensor>();

            pageViewScript = new PageView();
            pageViewScript.GameObject = pageViewGameObject;
            BehaviourManager.AddGameComponent(pageViewScript);

            pageViewScript.OnPageChanged = OnPageChanged;
            pageViewScript.OnCreatePageItem = createOnePage;

            dragSensor.OnBeinDragHandler = pageViewScript.OnBeginDrag;
            dragSensor.OnDragHandler = pageViewScript.OnDrag;
            dragSensor.OnEndDragHandler = pageViewScript.OnEndDrag;

            pageItemPrefb = go.transform.Find("PageView/Content/PageItem").gameObject;
            pageItemPrefb.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(PageWidth, PageHeight);

            content = pageViewGameObject.transform.Find("Content").GetComponent<RectTransform>();

            cellItemPrefab = go.transform.Find("BagCellItem").gameObject;

            prevBtn = new SoundButton(go.transform.Find("Arrows/Left").gameObject, toPrevHandler, ResPathConfig.PAGE_CHANGE_BUTTON);
            nextBtn = new SoundButton(go.transform.Find("Arrows/Right").gameObject, toNextHandler, ResPathConfig.PAGE_CHANGE_BUTTON);
        }

        public override void Refresh()
        {
            base.Refresh();

            Clear();

            itemDatas = BagManager.Instance.GetShowDataList();
            count = itemDatas.Count;
            pagesCount = (int)Mathf.Ceil(count / itemsOfPage);

            ContentWidth = (pagesCount - 1) * GapH + PageWidth * pagesCount;
            content.sizeDelta = new Vector2(ContentWidth, PageHeight);

            pageViewScript.PageCount = pagesCount;
            pageViewScript.InitPages();

            pageViewScript.CreatePageItem(pageMark.GetCurrentPageIndex());
        }

        private void createOnePage(int pageIndex)
        {
            List<BaseProp> datas = new List<BaseProp>();
            for (int i = pageIndex * (int)itemsOfPage; i < pageIndex * itemsOfPage + itemsOfPage && i < itemDatas.Count; i++)
            {
                datas.Add(itemDatas[i]);
            }

            PageItem pageItem = CreatePageItem(pageIndex);
            pageItem.InitItems(datas, pageIndex);
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

        private void createBagCellItem(BaseProp data, PageItem pageItem, int itemIndex)
        {
            GameObject ItemPrefabInstance = GameObject.Instantiate(cellItemPrefab);
            ItemPrefabInstance.SetActive(true);
            ItemPrefabInstance.transform.SetParent(pageItem.GameObject.transform.Find("Panel"), false);

            PointerSensor pointerSensor = ItemPrefabInstance.AddComponent<PointerSensor>();

            BagCellItem cellItem = new BagCellItem(ItemPrefabInstance);
            cellItem.Init(data, pageViewScript.TempPageIndex, itemIndex);

            pointerSensor.OnPointerClickHandler = cellItem.OnPointerClick;

            pageViewScript.pageItems[pageViewScript.TempPageIndex].listItem.Add(cellItem);
        }

        private PageItem CreatePageItem(int pageIndex)
        {
            GameObject pageItemPrefabInstance = UnityEngine.Object.Instantiate(pageItemPrefb);
            pageItemPrefabInstance.SetActive(true);
            pageItemPrefabInstance.transform.localPosition = new Vector3((GapH + PageWidth) * pageIndex, 0, 0);
            pageItemPrefabInstance.transform.SetParent(pageItemPrefb.transform.parent, false);

            PageItem pi = new PageItem(pageItemPrefabInstance);
            pi.OnCreatCellItemCallback = createBagCellItem;

            pageViewScript.pageItems[pageIndex] = pi;

            return pi;
        }

        private void OnPageChanged(int pageIndex)
        {
            Debug.Log(string.Format("当前显示页码_{0}", pageIndex));
        }

        public override void SetInfoBar()
        {
            base.SetInfoBar();
            PanelManager.InfoBar.SetMenuClickedHandler(() => { Close(); });
            PanelManager.InfoBar.SetBtnImage(EnumConfig.InfoBarBtnMode.Close);
        }

        public void toPrevHandler()
        {
            pageViewScript.ToPrevPage();
        }

        public void toNextHandler()
        {
            pageViewScript.ToNextPage();
        }

        public void ShowItemSelected(int pageIndex, int itemIndex)
        {
            BagCellItem tempCell;
            List<IItem> items = pageViewScript.GetPageItem(pageIndex).listItem;
            for (int i = 0; i < items.Count; i++)
            {
                tempCell = items[i] as BagCellItem;
                tempCell.ShowSelectedState(i == itemIndex);
            }
        }

        public void ShowArrowBtns(bool isShow)
        {
            prevBtn.SetActive(isShow);
            nextBtn.SetActive(isShow);
        }

        public void UpdatePlaceState(long id)
        {
            BagCellItem tempCell;
            PageItem pageItem = pageViewScript.GetCurrentPageItem();
            for (int i = 0; i < pageItem.listItem.Count; i++)
            {
                tempCell = pageItem.listItem[i] as BagCellItem;
                if (tempCell.Data.ID == id)
                {
                    tempCell.UpdatePlaceState();
                }
            }
        }

        public BagCellItem FindGuidePropCom(int type)
        {
            List<IItem> list = pageViewScript.GetCurrentPageItem().listItem;
            BagCellItem temp;
            for (int i = 0; i < list.Count; i++)
            {
                temp = list[i] as BagCellItem;
                if (temp.Data.Type == type)
                {
                    return temp;

                }
            }

            return null;
        }
    }
}
