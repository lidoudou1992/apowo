using FlyModel.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.CatBookPanel
{
    //public class CatBookController : MonoBehaviour, ITableViewDataSource
    public class CatBookController: ITableViewDataSource
    {
        public GameObject cellPrefab;
        public TableView tableViewScript;

        public int numRows;
        public List<Hashtable> animations;

        public float GetHeightForRowInTableView(TableView tableView, int row)
        {
            return (cellPrefab.transform as RectTransform).rect.height;
        }

        //Will be called by the TableView when a cell needs to be created for display
        public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
        {
            BookCellItem cell = tableView.GetReusableCell(cellPrefab.GetComponent<BookCellItem>().reuseIdentifier) as BookCellItem;
            if (cell == null)
            {
                GameObject cellInstance = GameObject.Instantiate(cellPrefab);
                cell = cellInstance.GetComponent<BookCellItem>();

                cellInstance.SetActive(true);
            }

            cell.updateData(animations[row]);

            return cell;
        }

        //Will be called by the TableView to know what is the height of each row
        public int GetNumberOfRowsForTableView(TableView tableView)
        {
            return numRows;
        }

        public void Refresh()
        {
            tableViewScript.dataSource = this;
        }

    }
}
