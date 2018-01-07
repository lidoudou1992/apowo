using Assets.Scripts.Common.TableView;
using FlyModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.TaskPanel
{
    public class TaskPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "TaskPanel";
            }
        }

        public override bool IsRoot
        {
            get
            {
                return true;
            }
        }

        public TableView tableViewScript;
        private ContentController contentController;

        private List<Model.Data.TaskData> dataList;

        private Text achievePointsTF;

        protected override void Initialize(GameObject go)
        {
            GameObject tableView = go.transform.Find("TableView").gameObject;
            tableViewScript = tableView.AddComponent<TableView>();

            contentController = tableView.AddComponent<ContentController>();
            contentController.tableViewScript = tableViewScript;
            contentController.cellPrefab = go.transform.Find("AchievementCell").gameObject;
            contentController.GetCellForRowInTableViewHandler = GetCellForRowInTableViewHandler;
            contentController.cellPrefab.AddComponent<TableViewCell>();

            achievePointsTF = go.transform.Find("AchievementPoints").GetComponent<Text>();

            PanelManager.BringSystemBarToTop();
        }

        public override void Refresh()
        {
            base.Refresh();

            dataList = formatData(TaskManager.Instance.TaskDataList);
            contentController.Refresh(dataList.Count);
            //achievePointsTF.text = string.Format("成就点：{0}", UserManager.Instance.Me.AchievementPoints.ToString());
            achievePointsTF.text = "成就点：" + UserManager.Instance.Me.AchievementPoints;
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
            if (cell == null)
            {
                GameObject cellInstance = GameObject.Instantiate(contentController.cellPrefab);
                cell = cellInstance.GetComponent<TableViewCell>();
                cell.GameObject = cellInstance;

                AchievementCell cellDataInstance = new AchievementCell(cellInstance);

                cell.CellDataInstance = cellDataInstance;
                cellInstance.SetActive(true);
            }

            (cell.CellDataInstance as AchievementCell).UpdateData(dataList[row]);

            return cell;
        }

        private List<Model.Data.TaskData> formatData(Dictionary<int, Model.Data.TaskData> datas)
        {
            List<Model.Data.TaskData> list = new List<Model.Data.TaskData>();

            foreach (var data in datas)
            {
                list.Add(data.Value);
            }

            return list;
        }
    }
}
