using FlyModel.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Utils
{
    public class TypewriterUtil: BehaviourBase
    {
        private float interval = 0.05f;
        private int currentTimes;
        public Action WriteCallback;

        public TypewriterUtil()
        {
            interval = interval / Time.deltaTime;
        }

        public override void Update()
        {
            base.Update();

            if (currentTimes>=interval)
            {
                write();
                currentTimes = 0;
            }
            else
            {
                currentTimes++;
            }

        }

        private void write()
        {
            if (WriteCallback != null)
            {
                WriteCallback();
                
            }
        }
    }
}
