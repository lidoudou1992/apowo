using FlyModel.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class User: BaseProp
    {
        public Proto.Gender Gender;
        public int GenderIndex;
        public long LowCurrency;
        public long HighCurrency;
        public int AdminType;

        public int signDays = 0;
        public bool todayHasSigned = false; // 今天是否已签到，默认为非

        public bool SignAllDays;

        public long AchievementPoints;

        public void UpdateData(CreateCharacterOK data)
        {
            ID = data.Id;
            Name = data.Name;
            Gender = data.Gender;
        }

        public void UpdateData(CharacterData data)
        {
            ID = data.Id;
            if (string.IsNullOrEmpty(data.Name)==false)
            {
                Name = data.Name;
            }
            GenderIndex = data.Gender;

            //Debug.Log("Coin " + data.Coin);
            //Debug.Log("Dollar " + data.Dollar);
            if (data.Coin >= 0)
            {
                LowCurrency = data.Coin;
            }

            if (data.Dollar >= 0)
            {
                HighCurrency = data.Dollar;
            }

            if (data.AdminType >= 0)
            {
                AdminType = data.AdminType;
            }

            if (data.Achieve > 0)
            {
                AchievementPoints = data.Achieve;
            }

            //if (data.DayCount > 0)
            //{
            //    signDays = (int)data.DayCount;
            //}
        }

        // 从服务器端得到签到面板数据
        public void UpdateData(SignData data)
        {
            todayHasSigned = !data.Has; // Has表示今天是否可以签到，默认第一次进入游戏可以签到
            signDays = data.Times;
            Debug.Log("今天是否已签到：" + todayHasSigned + "；已签到次数：" + signDays);
        }
    }
}
