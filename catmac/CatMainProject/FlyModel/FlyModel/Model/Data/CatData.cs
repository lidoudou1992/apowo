using FlyModel.Proto;
using FlyModel.UI.Scene.Data;
using System.Collections.Generic;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class CatData:BaseProp
    {
        public long ToyID = -1;
        public ToyStructInfo CatPath;
        public string CatSpineName;
        public string Animation;

        public CatData()
        {

        }

        public CatData(Proto.CatData data)
        {
            InitToyStruct(data);
        }

        public void updateData(Proto.CatData data)
        {
            ID = data.Id;
            Name = data.Name;
            CatSpineName = data.Animation;
            Type = data.Type;

            ToyID = data.RootID;

            Animation = data.Gesture;
        }

        public void InitToyStruct(Proto.CatData data)
        {
            ToyData temp = null;
            List<ToyData> toyDatas = SceneGameObjectManager.Instance.ToyDataList;
            foreach (var toyData in toyDatas)
            {
                if (toyData.ID == data.RootID)
                {
                    temp = toyData;
                    break;
                }
            }

            SpreadData playPoint = null; 
            if (temp != null)
            {
                foreach (var point in temp.PlayPointList)
                {
                    //Debug.Log("======point.CatID " + point.CatID + " data.Id " + data.Id);
                    if (point.CatID == data.Id)
                    {
                        playPoint = point;
                        break;
                    }
                }
            }

            if (playPoint != null)
            {
                createToyStruct(playPoint.Path);
            }
        }

        private void createToyStruct(string path)
        {
            string[] structs = path.Split('\\');

            CatPath = new ToyStructInfo();
            CatPath.ToyLayerIndex = int.Parse(structs[0].Split('_')[1]);
            CatPath.PlayPointLayerIndex = int.Parse(structs[1].Split('_')[1]);
            CatPath.PointIndex = int.Parse(structs[2].Split('_')[1]);

            //Debug.Log(string.Format("{0} {1} {2}", CatPath.ToyLayerIndex, CatPath.PlayPointLayerIndex, CatPath.PointIndex));
        }

        public static string GetCatAlias(string configName)
        {
            if (PlayerPrefs.HasKey(configName))
            {
                return PlayerPrefs.GetString(configName);
            }
            return configName;
        }
    }
}
