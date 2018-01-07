using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Control
{
    public class RotateBehaviour:BehaviourBase
    {
        public int speed;
        public override void Update()
        {
            base.Update();
            Transform.Rotate(Vector3.up, speed*Time.deltaTime);
        }
    }
}
