using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.TouchController
{
    public class DragSensor :MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public Action<PointerEventData> OnBeinDragHandler;
        public Action<PointerEventData> OnDragHandler;
        public Action<PointerEventData> OnEndDragHandler;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (OnBeinDragHandler != null)
            {
                OnBeinDragHandler(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (OnDragHandler != null)
            {
                OnDragHandler(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (OnEndDragHandler != null)
            {
                OnEndDragHandler(eventData);
            }
        }
    }
}
