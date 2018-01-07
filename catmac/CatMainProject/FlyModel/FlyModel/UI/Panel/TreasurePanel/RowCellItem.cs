using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.TreasurePanel
{
    public class RowCellItem
    {
        public GameObject GameObject;
        public List<TreasureItem> itemList;

        public RowCellItem(GameObject go)
        {
            GameObject = go;

            itemList = new List<TreasureItem>();
        }

        public void AddItem(TreasureItem item, int colIndex)
        {
            item.GameObject.transform.SetParent(GameObject.transform, false);
            item.GameObject.transform.localPosition = new Vector3(colIndex * 162 + 6, 0, 0);
            itemList.Add(item);
        }

        public void ClearItems()
        {
            int count = GameObject.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject.DestroyImmediate(GameObject.transform.GetChild(0).gameObject);
            }

            itemList.Clear();
        }
    }
}
