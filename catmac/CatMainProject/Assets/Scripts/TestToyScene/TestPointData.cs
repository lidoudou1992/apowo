using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.TestToyScene
{
    public class TestPointData
    {
        public string Name;
        public List<TestCatData> catDatas = new List<TestCatData>();
        public JsonData HideToyLayers;

        public TestPointData(JsonData jsonData, string toyLayerName, string playPointLayerName)
        {
            Name = jsonData["PointName"].ToString();
            HideToyLayers = jsonData["HideToyLayers"];

            JsonData cats = jsonData["Cats"];
            for (int i = 0; i < cats.Count; i++)
            {
                catDatas.Add(new TestCatData(cats[i], toyLayerName, playPointLayerName, this));
            }
        }
    }
}
