using FlyModel.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.TableViewPanel
{
    //public class TableViewController: MonoBehaviour, ITableViewDataSource
    public class TableViewController: ITableViewDataSource
    {
        public GameObject GameObject;

        public GameObject cellPrefab;
        public TableView tableViewScript;

        public int numRows = 100;
        private int numInstancesCreated = 0;

        public TableViewController()
        {
            tableViewScript.dataSource = this;
        }

        //void Start()
        //{
        //    tableViewScript.dataSource = this;
        //}

        #region ITableViewDataSource
        public float GetHeightForRowInTableView(TableView tableView, int row)
        {
            return (cellPrefab.transform as RectTransform).rect.height;
        }

        //Will be called by the TableView when a cell needs to be created for display
        public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
        {
            TableCellItem cell = tableView.GetReusableCell(cellPrefab.GetComponent<TableCellItem>().reuseIdentifier) as TableCellItem;
            if (cell == null)
            {
                GameObject cellInstance = UnityEngine.Object.Instantiate(cellPrefab);
                cell = cellInstance.GetComponent<TableCellItem>();
                cellInstance.name = "TableCellItem_" + (++numInstancesCreated).ToString();

                cellInstance.SetActive(true);
            }

            cell.updateData(row);

            return cell;
        }

        //Will be called by the TableView to know what is the height of each row
        public int GetNumberOfRowsForTableView(TableView tableView)
        {
            return numRows;
        }
        #endregion

        
        //Will be called by the TableView when a cell's visibility changed
        public void onCellVisibilityChanged(int row, bool isVisible)
        {
            if (isVisible)
            {
                TableCellItem cell = (TableCellItem)tableViewScript.GetCellAtRow(row);
                cell.onCellVisibilityChanged();
            }
        }


        #region tableview 滚动控制
        //public void ScrollToTopAnimated()
        //{
        //    StartCoroutine(AnimateToScrollY(0, 2f));
        //}

        public void ScrollToBottomImmediate()
        {
            tableViewScript.scrollY = tableViewScript.scrollableHeight;
        }

        //public void ScrollToRow10Animated()
        //{
        //    float scrollY = tableViewScript.GetScrollYForRow(10, true);
        //    StartCoroutine(AnimateToScrollY(scrollY, 2f));
        //}

        private IEnumerator AnimateToScrollY(float scrollY, float time)
        {
            float startTime = Time.time;
            float startScrollY = tableViewScript.scrollY;
            float endTime = startTime + time;
            while (Time.time < endTime)
            {
                float relativeProgress = Mathf.InverseLerp(startTime, endTime, Time.time);
                tableViewScript.scrollY = Mathf.Lerp(startScrollY, scrollY, relativeProgress);
                yield return new WaitForEndOfFrame();
            }
            tableViewScript.scrollY = scrollY;
        }
        #endregion
    }
}
