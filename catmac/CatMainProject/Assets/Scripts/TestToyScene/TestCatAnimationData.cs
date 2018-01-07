using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.TestToyScene
{
    public class TestCatAnimationData
    {
        public string Name;

        public TestCatData CatData;

        public float ShadowX;
        public float ShadowY;
        public float RotationZ;
        public float X;
        public float Y;
        public float ScaleX;
        public float ScaleY;
        public float ShadowScaleX;
        public float ShadowScaleY;
        public string ShadowName;


        public TestCatAnimationData(JsonData jsonData, TestCatData catData)
        {
            CatData = catData;

            Name = jsonData["AnimationName"].ToString();

            ShadowX = float.Parse(jsonData["ShadowX"].ToString());
            ShadowY = float.Parse(jsonData["ShadowY"].ToString());
            RotationZ = float.Parse(jsonData["RotationZ"].ToString());
            X = float.Parse(jsonData["X"].ToString());
            Y = float.Parse(jsonData["Y"].ToString());
            ScaleX = float.Parse(jsonData["ScaleX"].ToString());
            ScaleY = float.Parse(jsonData["ScaleY"].ToString());
            ShadowScaleX = float.Parse(jsonData["ShadowScaleX"].ToString());
            ShadowScaleY = float.Parse(jsonData["ShadowScaleY"].ToString());
            if (jsonData["ShadowName"]!=null)
            {
                ShadowName = jsonData["ShadowName"].ToString();
            }
        }
    }
}
