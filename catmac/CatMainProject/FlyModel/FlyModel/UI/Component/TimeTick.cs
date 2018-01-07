using FlyModel.Control;
using FlyModel.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Component
{
    public class TimeTick:BehaviourBase
    {
        public long StartTime;
        private float passTime;

        public void SetStartTime(long time)
        {
            StartTime = time;
            passTime = 0;
        }

        public override void Update()
        {
            base.Update();

            passTime += Time.deltaTime;
        }

        public DateTime ConvertTime(long time)
        {
           return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(time).ToLocalTime();
        }

        public long GetNow()
        {
            return (long)passTime + StartTime;
        }

        public long DateTimeToUTCSeconds(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalSeconds;
        }
    }
}
