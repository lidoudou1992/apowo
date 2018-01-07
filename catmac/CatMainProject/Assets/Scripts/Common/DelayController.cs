using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Component
{
    public class DelayController : MonoBehaviour
    {
        public Action callback;
        private float delayTime;

        private Coroutine currentCO;


        public void DelayInvoke(Action callback, float delayTime)
        {
            this.callback = callback;
            this.delayTime = delayTime;

            currentCO = StartCoroutine(doAction());
        }

        public void StopDelayInvoke()
        {
            if (currentCO != null)
            {
                StopCoroutine(currentCO);
                currentCO = null;
            }
        }

        IEnumerator doAction()
        {
            yield return new WaitForSeconds(delayTime);
            callback();
        }
    }
}
