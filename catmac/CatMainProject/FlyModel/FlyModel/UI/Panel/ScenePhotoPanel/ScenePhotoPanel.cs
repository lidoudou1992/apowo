using Assets.Scripts.Common.TableView;
using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.ScenePhotoPanel
{
    public class ScenePhotoPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "ScenePhotoPanel";
            }
        }

        public List<PhotoData> dataList;

        public TableView tableViewScript;
        private ContentController contentController;
        private GameObject CellItemPrefab;

        private DelayController delayController;

        public static int[] ROTATIN = new int[4] { 2, -2, -2, 0};

        public Dictionary<string, Sprite> TextureDic = new Dictionary<string, Sprite>();

        protected override void Initialize(GameObject go)
        {
            GameObject tableView = go.transform.Find("TableView").gameObject;
            tableViewScript = tableView.AddComponent<TableView>();

            contentController = tableView.AddComponent<ContentController>();
            contentController.tableViewScript = tableViewScript;
            contentController.cellPrefab = go.transform.Find("TableViewItem").gameObject;
            contentController.GetCellForRowInTableViewHandler = GetCellForRowInTableViewHandler;

            TableViewCell awardCell = contentController.cellPrefab.AddComponent<TableViewCell>();

            CellItemPrefab = go.transform.Find("ScenePhotoCell").gameObject;

            delayController = go.AddComponent<DelayController>();

            dataList = new List<PhotoData>();

            PanelManager.BringSystemBarToTop();
        }

        public override void Refresh()
        {
            base.Refresh();

            dataList.Clear();

            dataList = CloneDatas(PhotoManager.Instance.AllScenePhotoDatas);

            contentController.Refresh((int)Mathf.Ceil(dataList.Count / 2f));
        }

        public override void RefreshWhenBack()
        {
            base.RefreshWhenBack();

            Refresh();
        }

        public override void Load()
        {
            base.Load();

            PanelManager.LoadSystemBar(transform);
        }

        public override void SetInfoBar()
        {
            base.SetInfoBar();
            PanelManager.InfoBar.SetMenuClickedHandler(() => { Close(); });
            PanelManager.InfoBar.SetBtnImage(EnumConfig.InfoBarBtnMode.Close);
        }

        private TableViewCell GetCellForRowInTableViewHandler(TableView tableView, int row)
        {
            TableViewCell cell = tableView.GetReusableCell(contentController.cellPrefab.GetComponent<TableViewCell>().reuseIdentifier) as TableViewCell;
            RowCellScenePhotoItem cellDataInstance = null;
            if (cell == null)
            {
                GameObject cellInstance = GameObject.Instantiate(contentController.cellPrefab);
                cell = cellInstance.GetComponent<TableViewCell>();
                cell.GameObject = cellInstance;

                cellDataInstance = new RowCellScenePhotoItem(cellInstance);

                cell.CellDataInstance = cellDataInstance;
                cellInstance.SetActive(true);
            }

            cellDataInstance = cell.CellDataInstance as RowCellScenePhotoItem;
            cellDataInstance.ClearItems();

            int dataIndex = 0;
            ScenePhotoCell tempItem;
            GameObject tempItemGameObject;
            for (int i = 0; i < 2; i++)
            {
                dataIndex = i + row * 2;
                if (dataIndex < dataList.Count)
                {
                    tempItemGameObject = GameObject.Instantiate(CellItemPrefab);
                    tempItemGameObject.SetActive(true);
                    tempItem = new ScenePhotoCell(tempItemGameObject);
                    cellDataInstance.AddItem(tempItem, i);
                    tempItem.UpdateData(dataList[dataIndex], dataIndex);
                }
            }

            return cell;
        }

        public void LoadPhoto(string url, Action<Sprite> callback)
        {
            delayController.StartCoroutine(GetTexture(url, callback));
        }

        IEnumerator GetTexture(string url, Action<Sprite> callback)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.isDone && www.error == null)
            {
                Sprite sp = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
                callback(sp);
                if (TextureDic.ContainsKey(url)==false)
                {
                    TextureDic.Add(url, sp);
                }
            }
            else
            {
                Debug.Log("加载出错了");
            }
        }

        private List<PhotoData> CloneDatas(List<PhotoData> datas)
        {
            List<PhotoData> list = new List<PhotoData>();

            foreach (var data in datas)
            {
                list.Add(data);
            }

            return list;
        }

        public override void Close(bool isCloseAllMode = false)
        {
            base.Close(isCloseAllMode);

            if (isCloseAllMode==false)
            {
                PanelManager.LoadSystemBar(PanelManager.CurrentPanel.transform);
            }
        }
    }
}
