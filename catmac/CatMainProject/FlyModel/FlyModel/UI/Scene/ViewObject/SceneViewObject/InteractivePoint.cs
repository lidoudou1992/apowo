using Assets.Scripts.TouchController;
using FlyModel.Model.Data;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Scene.ViewObject.SceneViewObject
{
    public class InteractivePoint
    {
        public bool IsSmallPoint;

        public GameObject Root;

        public int PointIndex;

        //public PointerSensor clickSensor;

        public InteractivePoint(GameObject root)
        {
            Root = root;

            string pointName = root.name;
            PointIndex = int.Parse(pointName.Split('_')[1]);
        }

        public virtual void ShowAvailable(BagItemData data)
        {

        }

        public virtual void ClosePointMark()
        {

        }
    }
}
