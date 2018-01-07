using Assets.Scripts.TouchController;
using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Component;
using FlyModel.UI.Component.PageView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.PicturesPanel
{
    /// <summary>
    /// 所有猫的列表面板
    /// </summary>
    public class PicturesPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "PicturesPanel";
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

        public List<HandbookData> Datas;

        public float PageWidth = 640;
        public float PageHeight = 820;
        public float GapH = 0;
        public float ContentWidth;

        private PageMark pageMark;

        protected override void Initialize(GameObject go)
        {
            CameraAbsoluteResolution cameraAbsoluteResolution = GameObject.Find("Main Camera").GetComponent<CameraAbsoluteResolution>();
            Vector2 screenSize = cameraAbsoluteResolution.GetScreenPixelDimensions();
            PageWidth = screenSize.x * 1136 / screenSize.y;

            pageViewGameObject = go.transform.Find("PageView").gameObject;

            pageViewScript = new PageView();
            pageViewScript.GameObject = pageViewGameObject;
            BehaviourManager.AddGameComponent(pageViewScript);

            DragSensor dragSensor = pageViewGameObject.AddComponent<DragSensor>();
            dragSensor.OnBeinDragHandler = pageViewScript.OnBeginDrag;
            dragSensor.OnDragHandler = pageViewScript.OnDrag;
            dragSensor.OnEndDragHandler = pageViewScript.OnEndDrag;

            pageViewScript.OnPageChanged = OnPageChanged;
            pageViewScript.OnCreatePageItem = createOnePage;

            pageItemPrefb = go.transform.Find("PageView/Content/PageItem").gameObject;
            pageItemPrefb.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(PageWidth, PageHeight);

            content = pageViewGameObject.transform.Find("Content").GetComponent<RectTransform>();

            cellItemPrefab = go.transform.Find("CellItem").gameObject;

            prevBtn = new SoundButton(go.transform.Find("Arrows/Left").gameObject, toPrevHandler, ResPathConfig.PAGE_CHANGE_BUTTON);
            nextBtn = new SoundButton(go.transform.Find("Arrows/Right").gameObject, toNextHandler, ResPathConfig.PAGE_CHANGE_BUTTON);

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

        public override void Refresh()
        {
            base.Refresh();

            Clear();

            Datas = fileterPicture(HandbookManager.Instance.GetAppearCats());
            count = Datas.Count;
            pagesCount = (int)Mathf.Ceil(count / itemsOfPage);

            ContentWidth = (pagesCount - 1) * GapH + PageWidth * pagesCount;
            content.sizeDelta = new Vector2(ContentWidth, PageHeight);

            pageViewScript.PageCount = pagesCount;
            pageViewScript.InitPages();

            pageViewScript.CreatePageItem(pageMark.GetCurrentPageIndex());
        }

        public override void RefreshWhenBack()
        {
            base.RefreshWhenBack();

            Refresh();
        }

        public override void SetInfoBar()
        {
            base.SetInfoBar();
            PanelManager.InfoBar.SetMenuClickedHandler(() => { Close(); });
            PanelManager.InfoBar.SetBtnImage(EnumConfig.InfoBarBtnMode.Close);
        }

        private void OnPageChanged(int index)
        {
            Debug.Log("page changed " + index.ToString());
        }

        private void createOnePage(int pageIndex)
        {
            List<BaseProp> datas = new List<BaseProp>();
            for (int i = pageIndex * (int)itemsOfPage; i < pageIndex * itemsOfPage + itemsOfPage && i < count; i++)
            {
                datas.Add(Datas[i]);
            }

            PageItem pageItem = CreatePageItem(pageIndex);
            pageItem.InitItems(datas, pageIndex);
        }

        private PageItem CreatePageItem(int pageIndex)
        {
            GameObject pageItemPrefabInstance = UnityEngine.Object.Instantiate(pageItemPrefb);
            pageItemPrefabInstance.SetActive(true);
            pageItemPrefabInstance.transform.localPosition = new Vector3((GapH + PageWidth) * pageIndex, 0, 0);
            pageItemPrefabInstance.transform.SetParent(pageItemPrefb.transform.parent, false);

            PageItem pi = new PageItem(pageItemPrefabInstance);
            pi.OnCreatCellItemCallback = createCellItem;

            pageViewScript.pageItems[pageIndex] = pi;

            return pi;
        }

        private void createCellItem(BaseProp data, PageItem pageItem, int itemIndex)
        {
            GameObject ItemPrefabInstance = GameObject.Instantiate(cellItemPrefab);
            ItemPrefabInstance.SetActive(true);
            ItemPrefabInstance.transform.SetParent(pageItem.GameObject.transform.Find("Panel"), false);

            PointerSensor pointerSensor = ItemPrefabInstance.AddComponent<PointerSensor>();

            IItem cellItem = null;

            cellItem = new PhotoCellItem(ItemPrefabInstance);
            pointerSensor.OnPointerClickHandler = ((PhotoCellItem)cellItem).OnPointerClick;

            cellItem.Init(data, pageViewScript.TempPageIndex, itemIndex);

            pageViewScript.pageItems[pageViewScript.TempPageIndex].listItem.Add(cellItem);
        }

        public void toPrevHandler()
        {
            pageViewScript.ToPrevPage();
        }

        public void toNextHandler()
        {
            pageViewScript.ToNextPage();
        }

        public void Clear()
        {
            pageViewScript.ClearPageItems();

            pageViewScript.InitOver = false;
        }

        
        private List<HandbookData> fileterPicture(List<HandbookData> datas)
        {
            List<HandbookData> list = new List<HandbookData>();

            HandbookData sceneEntryData = new HandbookData();
            sceneEntryData.Name = "SceneEntryData";
            list.Add(sceneEntryData);

            foreach (var data in datas)
            {
                if (PhotoManager.Instance.AllCatPictureOwners.ContainsKey(data.SpineName))
                {
                    data.PhotoCount = PhotoManager.Instance.AllCatPictureOwners[data.SpineName];

                    list.Add(data);
                }
            }

            return list;
        }
    }
}
