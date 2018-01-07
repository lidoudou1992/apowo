using Assets.Scripts.Common.TableView;
using Assets.Scripts.TouchController;
using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.PicturePreviewPanel
{
    /// <summary>
    /// 单只猫的所有照片列表
    /// </summary>
    public class PicturePreviewPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "PicturePreviewPanel";
            }
        }

        public List<PhotoData> dataList;

        public TableView tableViewScript;
        private ContentController contentController;
        private GameObject CellItemPrefab;

        private HandbookData Data;

        private Image catImage;

        public static int[] ROTATIN = new int[6] { 5, -9, 0, 4, -6, 2 };

        private DelayController delayController;

        private FullSizePreviewContainer fullSizePreviewPanel;

        protected override void Initialize(GameObject go)
        {
            catImage = go.transform.Find("CatImage").GetComponent<Image>();

            GameObject tableView = go.transform.Find("TableView").gameObject;
            tableViewScript = tableView.AddComponent<TableView>();

            contentController = tableView.AddComponent<ContentController>();
            contentController.tableViewScript = tableViewScript;
            contentController.cellPrefab = go.transform.Find("TableViewItem").gameObject;
            contentController.GetCellForRowInTableViewHandler = GetCellForRowInTableViewHandler;

            TableViewCell awardCell = contentController.cellPrefab.AddComponent<TableViewCell>();

            CellItemPrefab = go.transform.Find("ImageCell").gameObject;

            dataList = new List<PhotoData>();

            delayController = go.AddComponent<DelayController>();

            fullSizePreviewPanel = new FullSizePreviewContainer(go.transform.Find("FullSizePrview").gameObject);

            PanelManager.BringSystemBarToTop();
        }

        public void SetData(HandbookData data)
        {
            Data = data;

            dataList.Clear();

            ResourceLoader.Instance.TryLoadPic(ResPathConfig.CAT_HEAD_ASSETBUNDLE, Data.PicCode, (texture) =>
            {
                catImage.sprite = texture as Sprite;
                catImage.SetNativeSize();
            });

            caculatePhotos(PhotoManager.Instance.AllCatPhotoDatas);

            contentController.Refresh((int)Mathf.Ceil(dataList.Count / 2f));

        }

        public override void RefreshWhenBack()
        {
            base.RefreshWhenBack();
            if (fullSizePreviewPanel.IsShow==false)
            {
                SetData(Data);
            }
        }

        public override void SetInfoBar()
        {
            //base.SetInfoBar();
            //PanelManager.InfoBar.SetMenuClickedHandler(() => { Close(); });
            //PanelManager.InfoBar.SetBtnImage(EnumConfig.InfoBarBtnMode.Close);
        }

        public override void Close(bool isCloseAllMode = false)
        {
            base.Close(isCloseAllMode);

            if (isCloseAllMode==false)
            {
                PanelManager.LoadSystemBar(PanelManager.CurrentPanel.transform);
            }
        }

        public override void Load()
        {
            base.Load();

            PanelManager.LoadSystemBar(transform);
        }

        private TableViewCell GetCellForRowInTableViewHandler(TableView tableView, int row)
        {
            TableViewCell cell = tableView.GetReusableCell(contentController.cellPrefab.GetComponent<TableViewCell>().reuseIdentifier) as TableViewCell;
            RowCellImageItem cellDataInstance = null;
            if (cell == null)
            {
                GameObject cellInstance = GameObject.Instantiate(contentController.cellPrefab);
                cell = cellInstance.GetComponent<TableViewCell>();
                cell.GameObject = cellInstance;

                cellDataInstance = new RowCellImageItem(cellInstance);

                cell.CellDataInstance = cellDataInstance;
                cellInstance.SetActive(true);
            }

            cellDataInstance = cell.CellDataInstance as RowCellImageItem;
            cellDataInstance.ClearItems();

            int dataIndex = 0;
            ImageCell tempItem;
            GameObject tempItemGameObject;
            for (int i = 0; i < 2; i++)
            {
                dataIndex = i + row * 2;
                if (dataIndex < dataList.Count)
                {
                    tempItemGameObject = GameObject.Instantiate(CellItemPrefab);
                    tempItemGameObject.SetActive(true);
                    tempItem = new ImageCell(tempItemGameObject);
                    tempItem.SetHandBookData(Data);
                    cellDataInstance.AddItem(tempItem, i);
                    tempItem.UpdateData(dataList[dataIndex], dataIndex);
                }
            }

            return cell;
        }

        private void caculatePhotos(List<PhotoData> pictureOwners)
        {
            foreach (var owner in pictureOwners)
            {
                if (Data.SpineName == owner.Owner)
                {
                    dataList.Add(owner);
                }
            }
        }

        public void LoadPhoto(string url, Action<Texture2D> callback)
        {
            delayController.StartCoroutine(GetTexture(url, callback));
        }

        IEnumerator GetTexture(string url, Action<Texture2D> callback)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.isDone && www.error == null)
            {
                callback(www.texture);
            }
            else
            {
                Debug.Log("加载出错了");
            }
        }

        public void ShowFullSizePreviewPanel(Model.Data.PhotoData photoData, Texture2D texture, string aliasName)
        {
            fullSizePreviewPanel.ShowSelf(photoData, texture, aliasName);
        }

    }
}
