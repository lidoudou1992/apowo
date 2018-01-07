using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.TestToyScene
{
    public class TestPlayPointLayerData
    {
        public string Name;
        public List<TestPointData> pointDatas = new List<TestPointData>();

        public TestPlayPointLayerData(JsonData jsonData, string toyLayerName)
        {
            Name = jsonData["LayerName"].ToString();

            JsonData points = jsonData["Points"];
            for (int i = 0; i < points.Count; i++)
            {
                pointDatas.Add(new TestPointData(points[i], toyLayerName, Name));
            }
        }
    }
}
