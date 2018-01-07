using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.TableViewPanel
{
    public class TableCellItem:TableViewCell
    {
        private Text text;
        private Text visibleCountText;

        private int numTimesBecameVisible;

        public TableCellItem(GameObject go):base(go)
        {
            GameObject cellLabel = go.transform.Find("CellLabel").gameObject;
            text = cellLabel.GetComponent<Text>();

            GameObject showCountGO = go.transform.Find("ShowCountLabel").gameObject;
            visibleCountText = showCountGO.GetComponent<Text>();
        }

        public void updateData(int index)
        {
            text.text = string.Format("num_{0}", index);

        }

        public void onCellVisibilityChanged()
        {
            numTimesBecameVisible++;
            visibleCountText.text = "# rows this cell showed : " + numTimesBecameVisible.ToString();
        }
    }
}
