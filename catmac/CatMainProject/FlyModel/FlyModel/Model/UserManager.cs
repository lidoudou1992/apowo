using FlyModel.Model.Data;
using FlyModel.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model
{
    public class UserManager
    {
        public static UserManager Instance;
        public static UserManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new UserManager();
            }

            return Instance;
        }

        public User Me;
        public void UpdateMe(CreateCharacterOK data)
        {
            if (Me == null)
            {
                Me = new User();
            }

            Me.UpdateData(data);
        }

        public void UpdateMe(CharacterData data)
        {
            if (Me == null)
            {
                Me = new User();
            }

            Me.UpdateData(data);
        }

        // 更新本客户端的签到数据
        public void UpdateMe(SignData data)
        {
            if (Me == null)
            {
                Me = new Data.User();
            }

            Me.UpdateData(data);
        }

        public string GetFormatHeadCode(int type)
        {
            return string.Format("user10{0}", type);
        }

        public string GetCurrentHeadCode()
        {
            if (PlayerPrefs.HasKey("HeadCode"))
            {
                return GetFormatHeadCode(PlayerPrefs.GetInt("HeadCode"));

            }

            return "user101";
        }

        public void RecordHeadCode(int type)
        {
            PlayerPrefs.SetInt("HeadCode", type);

            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.UpdateHead();
            }
        }
    }
}
