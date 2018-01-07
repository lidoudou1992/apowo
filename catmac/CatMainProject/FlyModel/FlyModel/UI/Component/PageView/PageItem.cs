using FlyModel.Model.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlyModel.UI.Component.PageView
{
    //public class PageItem: MonoBehaviour
    public class PageItem
    {
        public List<IItem> listItem = new List<IItem>();

        public int PageIndex;

        public GameObject GameObject;

        public Action<BaseProp, PageItem, int> OnCreatCellItemCallback;

        public PageItem(GameObject go)
        {
            GameObject = go;
        }

        public void InitItems(List<BaseProp> datas, int pageIndex)
        {
            PageIndex = pageIndex;
            int count = datas.Count;
            for (int i = 0; i < count; i++)
            {
                OnCreatCellItemCallback(datas[i], this, i);
            }
        }

        public void Clear()
        {
            listItem.Clear();
        }
    }
}
