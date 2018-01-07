using FlyModel.Model.Data;
using FlyModel.UI.Scene;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model
{
    public class CatManager
    {
        public static CatManager Instance;

        public List<CatData> CatDataList = new List<CatData>();

        public static CatManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new CatManager();
            }
            return Instance;
        }

        public void AddCatDatas(List<Proto.CatData> datas)
        {
            foreach (var item in datas)
            {
                AddOneCatData(item);
            }
        }

        public CatData AddOneCatData(Proto.CatData data)
        {
            CatData cat = new CatData(data);
            cat.updateData(data);
            CatDataList.Add(cat);

            return cat;
        }

        public void DeleteOneCatData(long id)
        {
            foreach (var catData in CatDataList)
            {
                if (catData.ID == id)
                {       
                    if (SceneGameObject.showLeaveTip)
                    {
                        // 显示扔走猫的提示
                        PanelManager.TipPanel.Show(() =>
                        {
                            string catAlias = Model.Data.CatData.GetCatAlias(catData.Name);
                            PanelManager.tipPanel.SetText(catAlias + "\n生气地离开了");
                        });
                        SceneGameObject.showLeaveTip = false;                 
                    }
                    CatDataList.Remove(catData);
                    break;
                }
            }
        }

        public void ClearData()
        {
            CatDataList.Clear();
        }
    }
}
