using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyModel.UI.Scene.Data
{
    public class ToyStructInfo
    {
        public string[] Struct = new string[] { "ToyRoot", "ToyLayer;", "PlayPointLayer", "Point" };
        public string ToyRoot = "";
        public int ToyLayerIndex;
        public int PlayPointLayerIndex;
        public int PointIndex;

        public string GetStructPath()
        {
            return string.Format("ToyLayer_{0}/PlayPointLayer_{1}/Point_{2}", ToyLayerIndex, PlayPointLayerIndex, PointIndex);
        }
    }
}
