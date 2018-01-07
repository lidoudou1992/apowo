using Assets.Scripts.Common.TableView;
using FlyModel.Model;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.MailPanel
{
    public class MailPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "SettingPanel_sys";
            }
        }

        public TableView tableViewScript;
        private ContentController contentController;
        private SoundButton getAllBtn;
        private Scrollbar scrollbar;

        private List<Model.Data.MailData> dataList;


        protected override void Initialize(GameObject go)
        {
            GameObject tableView = go.transform.Find("TableView").gameObject;
            tableViewScript = tableView.AddComponent<TableView>();

            contentController = tableView.AddComponent<ContentController>();
            contentController.tableViewScript = tableViewScript;
            contentController.cellPrefab = go.transform.Find("AwardCell").gameObject;
            contentController.GetCellForRowInTableViewHandler = GetCellForRowInTableViewHandler;

            contentController.cellPrefab.AddComponent<TableViewCell>();

            getAllBtn = new SoundButton(go.transform.Find("GetAll").gameObject, getAllAwards, ResPathConfig.GET_AWARDS);

            scrollbar = go.transform.Find("Scrollbar").GetComponent<Scrollbar>();
        }

        public override void SetInfoBar()
        {

        }

        public override void Refresh()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.ShowMenuBtn(false);
            }

            dataList = MailManager.Instance.MailDataList;
            contentController.Refresh(dataList.Count);
        }

        public override void Close(bool isCloseAllMode = false)
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.ShowMenuBtn(true);
            }

            base.Close(isCloseAllMode);
        }

        private void getAllAwards()
        {
            if (MailManager.Instance.HasAwards())
            {
                CommandHandle.Send(Proto.ServerMethod.GetAllMailAward, null);
            }
        }

        private TableViewCell GetCellForRowInTableViewHandler(TableView tableView, int row)
        {
            TableViewCell cell = tableView.GetReusableCell(contentController.cellPrefab.GetComponent<TableViewCell>().reuseIdentifier) as TableViewCell;
            if (cell == null)
            {
                GameObject cellInstance = GameObject.Instantiate(contentController.cellPrefab);
                cell = cellInstance.GetComponent<TableViewCell>();
                cell.GameObject = cellInstance;

                MailCell cellDataInstance = new MailCell(cellInstance);

                cell.CellDataInstance = cellDataInstance;
                cellInstance.SetActive(true);
            }

            (cell.CellDataInstance as MailCell).UpdateData(dataList[row]);

            return cell;
        }
    }
}
