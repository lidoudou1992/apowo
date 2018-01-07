using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class MailData:BaseProp
    {
        public string Title;
        public string Content;
        public Proto.MailStatus State;
        public List<Proto.AwardSpecData> Awards;
        public Proto.MailType MailType;

        public bool IsLoginNotice;

        public void UpdateData(Proto.MailData data)
        {
            ID = data.Id;
            Name = data.SenderName;
            Title = data.Title;
            Content = data.Content;
            State = data.Status;
            Debug.Log("=============data.Awards " + data.Awards.Count);
            Awards = data.Awards;
            MailType = data.Type;
        }

        public void SetLoginNoticeData(Proto.SMMessage data)
        {
            IsLoginNotice = true;

            Title = data.Title;
            Content = data.Content;

            Debug.Log(data);

            Name = "公告";
        }

        public bool HasAwards()
        {
            return Awards != null && Awards.Count > 0;
        }

        public bool HasRead()
        {
            return HasAwards() == false && State == Proto.MailStatus.Read; 
        }

        public void ClearAwards()
        {
            Awards.Clear();
        }

        public void UpdateState(Proto.MailStatus state)
        {
            State = state;
        }
    }
}
