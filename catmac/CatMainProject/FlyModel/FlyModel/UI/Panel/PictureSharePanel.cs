using Assets.Scripts.TouchController;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FlyModel.UI.Panel.PictureSharePanel
{
    public class PictureSharePanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "PictureSharePanel";
            }
        }

        protected override void Initialize(GameObject go)
        {
            PointerSensor pointerSensor = go.AddComponent<PointerSensor>();
            pointerSensor.OnPointerClickHandler = closeSelf;

            new SoundButton(go.transform.Find("Image/sina").gameObject, () =>
            {
                Debug.Log("新浪");
            });

            new SoundButton(go.transform.Find("Image/qq").gameObject, () =>
            {
                Debug.Log("qq好友");
            });

            new SoundButton(go.transform.Find("Image/qqz").gameObject, () =>
            {
                Debug.Log("qq空间");
            });

            new SoundButton(go.transform.Find("Image/weixin").gameObject, () =>
            {
                Debug.Log("微信好友");
            });

            new SoundButton(go.transform.Find("Image/friends").gameObject, () =>
            {
                Debug.Log("朋友圈");
            });
        }

        public override void Refresh()
        {
            base.Refresh();


        }

        public override void SetInfoBar()
        {

        }

        public void closeSelf(PointerEventData eventData)
        {
            Close();
        }
    }
}
