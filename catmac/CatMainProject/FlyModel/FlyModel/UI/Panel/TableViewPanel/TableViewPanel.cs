using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.TableViewPanel
{
    public class TableViewPanel : PanelBase
    {
        public override string BundleName
        {
            get
            {
                return "tableviewpanel";
            }
        }
        public override string AssetName
        {
            get
            {
                return "TableViewPanel";
            }
        }

        public override bool IsRoot
        {
            get
            {
                return true;
            }
        }

        private TableView tableViewScript;
        private TableViewController tabeleViewController;

        protected override void Initialize(GameObject go)
        {
            #region 固定高度单元格
            //初始化预制体
            GameObject tableView = go.transform.Find("TableView").gameObject;
            tableViewScript = new TableView(); //tableView.AddComponent<TableView>();

            tabeleViewController = new TableViewController();
            tabeleViewController.GameObject = tableView;
            tabeleViewController.tableViewScript = tableViewScript;
            tableViewScript.onCellVisibilityChanged.AddListener(tabeleViewController.onCellVisibilityChanged);

            //GameObject tableItemCellPrefab = go.transform.Find("TableCell").gameObject;
            //tableItemCellPrefab.AddComponent<TableCellItem>();
            TableCellItem tableItemCellPrefab = new TableCellItem(go.transform.Find("TableCell").gameObject);
            tabeleViewController.cellPrefab = tableItemCellPrefab.GameObject;

            Button ToBottomBtn = go.transform.Find("ButtonGroup/ToBottom").gameObject.GetComponent<Button>();
            ToBottomBtn.onClick.AddListener(tabeleViewController.ScrollToBottomImmediate);
            Button ToTopBtn = go.transform.Find("ButtonGroup/ToTop").gameObject.GetComponent<Button>();
            //ToTopBtn.onClick.AddListener(tabeleViewController.ScrollToTopAnimated);
            Button ToIndexBtn = go.transform.Find("ButtonGroup/ToIndex").gameObject.GetComponent<Button>();
            //ToIndexBtn.onClick.AddListener(tabeleViewController.ScrollToRow10Animated);
            #endregion

            #region 动态高度单元格
            ////初始化预制体
            //GameObject tableView = go.transform.Find("TableView").gameObject;
            //tableViewScript = tableView.AddComponent<TableView>();

            //GameObject tableItemCellPrefab = go.transform.Find("DynamicCell").gameObject;
            //DynamicHeightCell dynamicHeightCell = tableItemCellPrefab.AddComponent<DynamicHeightCell>();

            //dynamicTableViewController = tableView.AddComponent<DynamicHeightTableViewController>();
            //dynamicTableViewController.tableViewScript = tableViewScript;
            //dynamicHeightCell.onCellHeightChanged.AddListener(dynamicTableViewController.OnCellHeightChanged);

            //dynamicTableViewController.CellPrefab = tableItemCellPrefab;

            //Button ToBottomBtn = go.transform.Find("ButtonGroup/ToBottom").gameObject.GetComponent<Button>();
            //ToBottomBtn.onClick.AddListener(dynamicTableViewController.ScrollToBottomImmediate);
            //Button ToTopBtn = go.transform.Find("ButtonGroup/ToTop").gameObject.GetComponent<Button>();
            //ToTopBtn.onClick.AddListener(dynamicTableViewController.ScrollToTopAnimated);
            //Button ToIndexBtn = go.transform.Find("ButtonGroup/ToIndex").gameObject.GetComponent<Button>();
            //ToIndexBtn.onClick.AddListener(dynamicTableViewController.ScrollToRow10Animated);
            #endregion
        }
    }
}
