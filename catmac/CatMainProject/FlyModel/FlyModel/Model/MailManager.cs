using FlyModel.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model
{
    public class MailManager
    {
        public static MailManager Instance;

        public List<Model.Data.MailData> MailDataList = new List<Model.Data.MailData>();

        public int NewMailCount = 0;

        public static MailManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new MailManager();
            }

            return Instance;
        }

        public void Initialize(List<Proto.MailData> datas)
        {
            NewMailCount = 0;
            MailDataList.Clear();

            foreach (var data in datas)
            {
                AddOneMail(data);
            }
        }

        public void AddOneMail(Proto.MailData data)
        {
            Data.MailData tempData = new Data.MailData();
            tempData.UpdateData(data);
            MailDataList.Add(tempData);

            if (tempData.HasRead()==false)
            {
                NewMailCount++;
            }
        }

        public Model.Data.MailData GetOneMail(long id)
        {
            foreach (var mail in MailDataList)
            {
                if (mail.ID == id)
                {
                    return mail;

                }
            }

            return null;
        }

        public bool HasAwards()
        {
            foreach (var mail in MailDataList)
            {
                if (mail.HasAwards()) {
                    return true;

                }
            }

            return false;
        }

        public void GetAllMailAward()
        {
            foreach (var mail in MailDataList)
            {
                if (mail.HasAwards())
                {
                    //NewMailCount--;
                    mail.ClearAwards();
                    mail.UpdateState(Proto.MailStatus.Read);
                }
            }
        }

        public void RemoveMail(IDMessage data) {
            Model.Data.MailData d = GetOneMail(data.Id);

            //Debug.Log(NewMailCount + " ==========RemoveMail");
            //if (d.HasAwards())
            //{
            //    Debug.Log("==============RemoveMail");
            //    NewMailCount--;
            //}

            MailDataList.Remove(d);
        }

        public void CaculateNewCount()
        {
            int count = 0;
            foreach (var mail in MailDataList)
            {
                if (mail.HasRead()==false)
                {
                    count++;

                }
            }

            NewMailCount = count;
        }
    }
}
