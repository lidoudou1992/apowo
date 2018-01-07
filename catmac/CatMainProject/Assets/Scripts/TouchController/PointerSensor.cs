using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.TouchController
{
    public class PointerSensor : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Action<PointerEventData> OnPointerClickHandler;
        public Action<PointerEventData> OnPointerDownHandler;
        public Action<PointerEventData> OnPointerUpHandler;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnPointerClickHandler != null)
            {
                OnPointerClickHandler(eventData);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (OnPointerDownHandler != null)
            {
                OnPointerDownHandler(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (OnPointerUpHandler != null)
            {
                OnPointerUpHandler(eventData);
            }
        }
    }
}
