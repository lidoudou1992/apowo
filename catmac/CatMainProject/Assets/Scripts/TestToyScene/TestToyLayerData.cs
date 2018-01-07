using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.TestToyScene
{
    public class TestToyLayerData
    {
        public string Name;

        public List<TestPlayPointLayerData> playPointLayersData = new List<TestPlayPointLayerData>();

        public TestToyLayerData(JsonData jsonData)
        {
            Name = jsonData["LayerName"].ToString();

            JsonData playPointLayer = jsonData["PlayPointLayers"];
            for (int i = 0; i < playPointLayer.Count; i++)
            {
                playPointLayersData.Add(new TestPlayPointLayerData(playPointLayer[i], Name));
            }
        }
    }
}
