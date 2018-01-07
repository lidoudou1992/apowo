using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Common.TableView
{
    public class ContentController : MonoBehaviour, ITableViewDataSource
    {
        public GameObject cellPrefab;
        public TableView tableViewScript;

        public Func<TableView, int, TableViewCell> GetCellForRowInTableViewHandler;

        public int numRows;

        #region ITableViewDataSource
        public float GetHeightForRowInTableView(TableView tableView, int row)
        {
            return (cellPrefab.transform as RectTransform).rect.height;
        }

        //Will be called by the TableView when a cell needs to be created for display
        public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
        {
            return GetCellForRowInTableViewHandler(tableView, row);
        }

        //Will be called by the TableView to know what is the height of each row
        public int GetNumberOfRowsForTableView(TableView tableView)
        {
            return numRows;
        }
        #endregion

        public void Refresh(int dataCounts)
        {
            numRows = dataCounts;
            tableViewScript.dataSource = this;
        }


        #region tableview 滚动控制
        //public void ScrollToTopAnimated()
        //{
        //    StartCoroutine(AnimateToScrollY(0, 2f));
        //}

        //public void ScrollToBottomImmediate()
        //{
        //    tableViewScript.scrollY = tableViewScript.scrollableHeight;
        //}

        //public void ScrollToRow10Animated()
        //{
        //    float scrollY = tableViewScript.GetScrollYForRow(10, true);
        //    StartCoroutine(AnimateToScrollY(scrollY, 2f));
        //}

        //private IEnumerator AnimateToScrollY(float scrollY, float time)
        //{
        //    float startTime = Time.time;
        //    float startScrollY = tableViewScript.scrollY;
        //    float endTime = startTime + time;
        //    while (Time.time < endTime)
        //    {
        //        float relativeProgress = Mathf.InverseLerp(startTime, endTime, Time.time);
        //        tableViewScript.scrollY = Mathf.Lerp(startScrollY, scrollY, relativeProgress);
        //        yield return new WaitForEndOfFrame();
        //    }
        //    tableViewScript.scrollY = scrollY;
        //}
        #endregion
    }
}
