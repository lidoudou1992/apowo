using Assets.Scripts.Common.TableView;
using FlyModel.Model;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.SceneChangePanel
{
    public class SceneChangePanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "SceneChangePanel";
            }
        }

        public TableView tableViewScript;
        private ContentController contentController;

        private List<Model.Data.RoomData> dataList;

        protected override void Initialize(GameObject go)
        {
            GameObject tableView = go.transform.Find("TableView").gameObject;
            tableViewScript = tableView.AddComponent<TableView>();

            contentController = tableView.AddComponent<ContentController>();
            contentController.tableViewScript = tableViewScript;
            contentController.cellPrefab = go.transform.Find("SceneCell").gameObject;
            contentController.GetCellForRowInTableViewHandler = GetCellForRowInTableViewHandler;

            TableViewCell awardCell = contentController.cellPrefab.AddComponent<TableViewCell>();

            PanelManager.BringSystemBarToTop();

            go.AddComponent<DelayController>().DelayInvoke(() =>
            {
                if (GuideManager.Instance.IsGestureTouchEffective("Scene"))
                {
                    GuideManager.Instance.ContinueGuide();
                }
            }, 0.8f);
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

            dataList = RoomManager.Instance.RoomsList;
            contentController.Refresh(dataList.Count);
            
        }

        public override void Close(bool isCloseAllMode = false)
        {
            base.Close(isCloseAllMode);

            if (isCloseAllMode==false)
            {
                PanelManager.LoadSystemBar(PanelManager.CurrentPanel.transform);
            }
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
            if (cell == null)
            {
                GameObject cellInstance = GameObject.Instantiate(contentController.cellPrefab);
                cell = cellInstance.GetComponent<TableViewCell>();
                cell.GameObject = cellInstance;

                SceneCell cellDataInstance = new SceneCell(cellInstance);

                cell.CellDataInstance = cellDataInstance;
                cellInstance.SetActive(true);
            }

            (cell.CellDataInstance as SceneCell).UpdateData(dataList[row]);

            return cell;
        }

        public SceneCell FindGuidePropCom(int index)
        {
            TableViewCell t = tableViewScript.GetCellAtRow(index);
            SceneCell s = t.CellDataInstance as SceneCell;
            return s; 
        }
    }
}
