using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FlyModel.UI.Component
{
    public class PointerSensor :MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public Action<PointerEventData> onClickedCallback;
        public Action<PointerEventData> onDownCallback;
        public Action<PointerEventData> onUpCallback;
        public Action<PointerEventData> onDragCallback;

        public void OnDrag(PointerEventData eventData)
        {
            if (onDragCallback != null)
            {
                onDragCallback(eventData);
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (onClickedCallback != null)
            {
                onClickedCallback(eventData);
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (onDownCallback != null)
            {
                onDownCallback(eventData);
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (onUpCallback != null)
            {
                onUpCallback(eventData);
            }
        }
    }
}
