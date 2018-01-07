using Assets.Scripts.Common.TableView;
using FlyModel.Model;
using FlyModel.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.TreasurePanel
{
    public class TreasurePanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "TreasurePanel";
            }
        }

        public List<CatGiftData> dataList = HandbookManager.Instance.giftList;

        public TableView tableViewScript;
        private ContentController contentController;
        private GameObject CellItemPrefab;
        private Text countTF;

        protected override void Initialize(GameObject go)
        {
            GameObject tableView = go.transform.Find("TableView").gameObject;
            tableViewScript = tableView.AddComponent<TableView>();

            contentController = tableView.AddComponent<ContentController>();
            contentController.tableViewScript = tableViewScript;
            contentController.cellPrefab = go.transform.Find("TableViewItem").gameObject;
            contentController.GetCellForRowInTableViewHandler = GetCellForRowInTableViewHandler;

            TableViewCell awardCell = contentController.cellPrefab.AddComponent<TableViewCell>();

            CellItemPrefab = go.transform.Find("TreasureCell").gameObject;

            countTF = go.transform.Find("Count/Text").GetComponent<Text>();

            PanelManager.BringSystemBarToTop();
        }

        protected override void InitializeOver()
        {
            base.InitializeOver();

            //PanelManager.CurrentPanel.Close();
        }

        public override void Load()
        {
            base.Load();

            PanelManager.LoadSystemBar(transform);
        }

        public override void Refresh()
        {
            base.Refresh();

            contentController.Refresh((int)Mathf.Ceil(dataList.Count / 3f));

            countTF.text = string.Format("{0}/{1}", HandbookManager.Instance.GetGettedGiftCount(), HandbookManager.Instance.giftList.Count);
        }

        public override void SetInfoBar()
        {
            base.SetInfoBar();
            PanelManager.InfoBar.SetMenuClickedHandler(() => { Close(); });
            PanelManager.InfoBar.SetBtnImage(EnumConfig.InfoBarBtnMode.Close);
        }

        public override void Close(bool isCloseAllMode = false)
        {
            base.Close(isCloseAllMode);

            if (isCloseAllMode==false)
            {
                PanelManager.LoadSystemBar(PanelManager.CurrentPanel.transform);
            }
        }

        private TableViewCell GetCellForRowInTableViewHandler(TableView tableView, int row)
        {
            TableViewCell cell = tableView.GetReusableCell(contentController.cellPrefab.GetComponent<TableViewCell>().reuseIdentifier) as TableViewCell;
            RowCellItem cellDataInstance = null;
            if (cell == null)
            {
                GameObject cellInstance = GameObject.Instantiate(contentController.cellPrefab);
                cell = cellInstance.GetComponent<TableViewCell>();
                cell.GameObject = cellInstance;

                cellDataInstance = new RowCellItem(cellInstance);

                cell.CellDataInstance = cellDataInstance;
                cellInstance.SetActive(true);
            }

            cellDataInstance = cell.CellDataInstance as RowCellItem;
            cellDataInstance.ClearItems();

            int dataIndex = 0;
            TreasureItem tempItem;
            GameObject tempItemGameObject;
            for (int i = 0; i < 3; i++)
            {
                dataIndex = i + row * 3;
                if (dataIndex < dataList.Count)
                {
                    tempItemGameObject = GameObject.Instantiate(CellItemPrefab);
                    tempItemGameObject.SetActive(true);
                    tempItem = new TreasureItem(tempItemGameObject);
                    cellDataInstance.AddItem(tempItem, i);
                    tempItem.UpdateData(dataList[dataIndex]);
                }
            }

            return cell;
        }
    }
}
