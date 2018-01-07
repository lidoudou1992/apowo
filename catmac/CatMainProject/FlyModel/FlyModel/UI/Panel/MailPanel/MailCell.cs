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
    public class MailCell
    {
        public GameObject GameObject;
        private Text typeTF;
        private Text titleTF;
        private GameObject awardBtn;
        private Image typePic;

        private Button btn;

        private Model.Data.MailData Data;

        private GameObject newTF;

        public MailCell(GameObject go)
        {
            GameObject = go;

            btn = go.transform.Find("Bg").GetComponent<Button>();
            btn.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
                PanelManager.MailContentPanel.Show(()=> {
                    PanelManager.mailContentPanel.SetData(Data);
                });
            }));

            typeTF = go.transform.Find("Type").GetComponent<Text>();
            titleTF = go.transform.Find("Title").GetComponent<Text>();
            awardBtn = go.transform.Find("Button").gameObject;
            typePic = go.transform.Find("Pic").GetComponent<Image>();

            newTF = go.transform.Find("New").gameObject;
            newTF.SetActive(false);

            new SoundButton(awardBtn, () =>
            {
                CommandHandle.Send(Proto.ServerMethod.GetMailAward, new IDMessage() { Id = Data.ID });
            }, ResPathConfig.GET_AWARDS);
        }

        public void UpdateData(Model.Data.MailData data)
        {
            Data = data;
            typeTF.text = data.Name;
            titleTF.text = data.Title;
            awardBtn.SetActive(data.HasAwards());
            newTF.SetActive(data.HasRead()==false);
        }
    }
}
