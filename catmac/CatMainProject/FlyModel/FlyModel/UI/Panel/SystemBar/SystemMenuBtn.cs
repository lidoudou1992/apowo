using Assets.Scripts.TouchController;
using FlyModel.UI.Component;
using FlyModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FlyModel.UI.Panel.SystemBar
{
    public class SystemMenuBtn
    {
        private SkeletonGraphic spine;

        public int Index;

        public bool IsSelected;

        private Action<int> clickCallback;

        public GameObject GameObject;

        public SystemMenuBtn(GameObject go)
        {
            GameObject = go;

            PointerSensor sensor = go.AddComponent<PointerSensor>();
            sensor.OnPointerClickHandler = OnPointerClick;

            spine = go.transform.Find("Spine").GetComponent<SkeletonGraphic>();
            //if (spine.AnimationState != null)
            //{
            //    spine.AnimationState.SetAnimation(0, "animation_open", false);
            //}
        }

        public void ShowActive(bool b)
        {
            GameObject.SetActive(b);

            spine.AnimationState.SetAnimation(0, "animation_open", false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SoundUtil.PlaySound(ResPathConfig.SYSTEM_MENU);

            ShowSelected(IsSelected);

            if (clickCallback != null)
            {
                clickCallback(Index);
            }
        }

        public void ShowSelected(bool isSelected)
        {
            if (IsSelected != isSelected)
            {
                if (isSelected)
                {
                    spine.AnimationState.SetAnimation(0, "animation_begin", false);
                }
                else
                {
                    spine.AnimationState.SetAnimation(0, "animation_over", false);
                }

                IsSelected = isSelected;
            }
        }

       public void AddListener(Action<int> callback)
        {
            clickCallback = callback;
        }
    }
}
