using FlyModel.Model;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.HeadSelectPanel
{
    public class HeadSelectPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "HeadSelectPanel";
            }
        }

        private Image previewImage;
        private Image oldHeadImage;

        private int currentHeadType = 1;

        protected override void Initialize(GameObject go)
        {
            previewImage = go.transform.Find("Imagebg/Face/Face/Image").GetComponent<Image>();
            oldHeadImage = go.transform.Find("Imagebg/Face/OldFace/Image").GetComponent<Image>();


            new SoundButton(go.transform.Find("Imagebg/chance").gameObject, () => Close());
            new SoundButton(go.transform.Find("Imagebg/ok").gameObject, () =>
            {
                UserManager.Instance.RecordHeadCode(currentHeadType);
                Close();
            });

            new SoundButton(go.transform.Find(string.Format("Imagebg/bg/face{0}", 1)).gameObject, () =>
            {
                updateHead(1);
            });

            new SoundButton(go.transform.Find(string.Format("Imagebg/bg/face{0}", 2)).gameObject, () =>
            {
                updateHead(2);
            });

            new SoundButton(go.transform.Find(string.Format("Imagebg/bg/face{0}", 3)).gameObject, () =>
            {
                updateHead(3);
            });

            new SoundButton(go.transform.Find(string.Format("Imagebg/bg/face{0}", 4)).gameObject, () =>
            {
                updateHead(4);
            });

            new SoundButton(go.transform.Find(string.Format("Imagebg/bg/face{0}", 5)).gameObject, () =>
            {
                updateHead(5);
            });

            new SoundButton(go.transform.Find(string.Format("Imagebg/bg/face{0}", 6)).gameObject, () =>
            {
                updateHead(6);
            });

            new SoundButton(go.transform.Find(string.Format("Imagebg/bg/face{0}", 7)).gameObject, () =>
            {
                updateHead(7);
            });

            new SoundButton(go.transform.Find(string.Format("Imagebg/bg/face{0}", 8)).gameObject, () =>
            {
                updateHead(8);
            });
        }

        public override void Refresh()
        {
            base.Refresh();

            setOldHead();
        }

        public override void SetInfoBar()
        {
            
        }

        private void updateHead(int type)
        {
            currentHeadType = type;

            ResourceLoader.Instance.TryLoadPic(ResPathConfig.USER_HEAD, UserManager.Instance.GetFormatHeadCode(type), (texture) =>
            {
                previewImage.sprite = texture as Sprite;
                previewImage.SetNativeSize();
            });
        }

        private void setOldHead()
        {
            ResourceLoader.Instance.TryLoadPic(ResPathConfig.USER_HEAD, UserManager.Instance.GetCurrentHeadCode(), (texture) =>
            {
                oldHeadImage.sprite = texture as Sprite;
                oldHeadImage.SetNativeSize();
            });
        }
    }
}
