using FlyModel.Proto;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.MailPanel
{
    public class MailContentPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "MailContentPanel";
            }
        }

        private Text titleTF;
        private Text contentTF;
        private Text btnTF;
        private Action btnCallback;

        private Model.Data.MailData Data;

        protected override void Initialize(GameObject go)
        {
            titleTF = go.transform.Find("Title").GetComponent<Text>();
            contentTF = go.transform.Find("Image/Scroller/Content").GetComponent<Text>();
            btnTF = go.transform.Find("GetBtn/lingqu").GetComponent<Text>();
            new SoundButton(go.transform.Find("GetBtn").gameObject, () =>
            {
                if (btnCallback != null)
                {
                    btnCallback();
                }
            });

        }

        public override void SetInfoBar()
        {

        }

        public override void Refresh()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.ShowMenuBtn(false);
            }

        }

        public override void Close(bool isCloseAllMode = false)
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.ShowMenuBtn(true);
            }

            base.Close(isCloseAllMode);
        }

        public void SetData(Model.Data.MailData data)
        {
            Data = data;

            titleTF.text = data.Title;
            contentTF.text = data.Content;

            if (data.IsLoginNotice)
            {

                btnTF.text = "确定";
                btnCallback = () =>
                {
                    Close();
                };
            }
            else
            {
                CommandHandle.Send(Proto.ServerMethod.ReadMail, new IDMessage() { Id = Data.ID });

                //contentTF.text = "是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大是大家是大家是大家是大家是大";
                Vector2 contentSize = contentTF.transform.GetComponent<RectTransform>().sizeDelta;
                contentTF.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, contentTF.preferredHeight);
                if (data.HasAwards())
                {
                    btnTF.text = "领取";
                    btnCallback = () =>
                    {
                        CommandHandle.Send(Proto.ServerMethod.GetMailAward, new IDMessage() { Id = Data.ID });
                        Close();
                    };
                }
                else
                {
                    btnTF.text = "删除";
                    btnCallback = () =>
                    {
                        CommandHandle.Send(ServerMethod.DeleteMail, new IDMessage() { Id = Data.ID });
                        Close();
                    };
                }
            }
        }
    }
}
