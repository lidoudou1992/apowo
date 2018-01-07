using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FlyModel.UI.Component
{
    //public class DragSensor : MonoBehaviour,IDragHandler
    public class DragSensorOld
    {
        public Action<PointerEventData> onDragCallback;

        public void OnDrag(PointerEventData eventData)
        {
            if (onDragCallback != null)
            {
                onDragCallback(eventData);
            }
        }
    }
}
