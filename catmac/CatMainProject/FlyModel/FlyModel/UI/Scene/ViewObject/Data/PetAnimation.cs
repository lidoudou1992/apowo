using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyModel.UI.Scene.Data
{
    public class PetAnimation
    {
        public string Name;
        public float X;
        public float Y;

        public float ScaleX;
        public float ScaleY;
        public float RotationZ;

        public string ShadowName;
        public float ShadowX;
        public float ShadowY;
        public float ShadowScaleX = 1;
        public float ShadowScaleY = 1;

        private JsonData petAnimationJsonData;

        public PetAnimation(JsonData jsonData)
        {
            petAnimationJsonData = jsonData;
            parseConfig();
        }

        private void parseConfig()
        {
            Name = petAnimationJsonData["AnimationName"].ToString();
            X = float.Parse(petAnimationJsonData["X"].ToString());
            Y = float.Parse(petAnimationJsonData["Y"].ToString());

            ScaleX = float.Parse(petAnimationJsonData["ScaleX"].ToString());
            ScaleY = float.Parse(petAnimationJsonData["ScaleY"].ToString());
            RotationZ = float.Parse(petAnimationJsonData["RotationZ"].ToString());

            ShadowX = float.Parse(petAnimationJsonData["ShadowX"].ToString());
            ShadowY = float.Parse(petAnimationJsonData["ShadowY"].ToString());
            if (((IDictionary)petAnimationJsonData).Contains("ShadowScaleX"))
            {
                ShadowScaleX = float.Parse(petAnimationJsonData["ShadowScaleX"].ToString());
                ShadowScaleY = float.Parse(petAnimationJsonData["ShadowScaleY"].ToString());
            }
            ShadowName = petAnimationJsonData["ShadowName"] == null ? "" : petAnimationJsonData["ShadowName"].ToString();

            //Debug.Log(string.Format("{0} {1} {2}", Name, X, Y));
        }
    }
}
