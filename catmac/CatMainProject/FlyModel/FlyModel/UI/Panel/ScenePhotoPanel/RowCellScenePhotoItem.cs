using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.ScenePhotoPanel
{
    public class RowCellScenePhotoItem
    {
        public GameObject GameObject;
        public List<ScenePhotoCell> itemList;

        public RowCellScenePhotoItem(GameObject go)
        {
            GameObject = go;

            itemList = new List<ScenePhotoCell>();
        }

        public void AddItem(ScenePhotoCell item, int colIndex)
        {
            item.GameObject.transform.SetParent(GameObject.transform, false);
            item.GameObject.transform.localPosition = new Vector3(colIndex * 300 + 150, -410/2f-10, 0);
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
