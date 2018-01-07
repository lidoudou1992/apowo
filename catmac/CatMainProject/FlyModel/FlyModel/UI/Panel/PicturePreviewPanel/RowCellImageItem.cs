using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.PicturePreviewPanel
{
    public class RowCellImageItem
    {
        public GameObject GameObject;
        public List<ImageCell> itemList;
       
        public RowCellImageItem(GameObject go)
        {
            GameObject = go;

            itemList = new List<ImageCell>();
        }

        public void AddItem(ImageCell item, int colIndex)
        {
            item.GameObject.transform.SetParent(GameObject.transform, false);
            item.GameObject.transform.localPosition = new Vector3(colIndex * 295 + 135, -125, 0);
            itemList.Add(item);
        }

        public void ClearItems()
        {
            int count = GameObject.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject.DestroyImmediate(GameObject.transform.GetChild(0).gameObject);
            }

            foreach (var item in itemList)
            {
                item.Clear();
            }
            itemList.Clear();
        }
    }
}
