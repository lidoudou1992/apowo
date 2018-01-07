using FlyModel.UI;
using LitJson;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class HandbookData:BaseProp
    {
        public bool IsFindOnLine;
        public bool IsSuper;
        public string Color;
        public string Character;
        public int Power;
        public string Des;
        public int GiftType;
        public string ShowAction;
        public string SpineName;
        public string LastShowTime;

        public int PhotoCount;

        public string Sound;

        private bool isNew;

        public void UpdateData(JsonData data)
        {
            Type = int.Parse(data["ID"].ToString());
            PicCode = data["Code"].ToString();
            Name = data["Name"].ToString();
            IsSuper = int.Parse(data["Quality"].ToString()) > 0;

            Color = data["Color"].ToString();
            Character = data["Temper"].ToString();
            Power = int.Parse(data["Capacity"].ToString());
            Des = data["Description"].ToString();

            GiftType = int.Parse(data["TreasureID"].ToString());

            ShowAction = data["ShowAction"].ToString();
            SpineName = data["Animation"].ToString();

            Sound = data["Sound"].ToString();
        }

        public void UpdateData(Proto.PicData data, bool needNewFlag)
        {
            if (needNewFlag && IsFindOnLine==false)
            {
                isNew = data.IsFound > 0;
            }

            ID = data.Id;
            Count = (int)data.ShowTimes;
            IsFindOnLine = data.IsFound > 0;
            LastShowTime = data.LastShowTime;
        }

        public EnumConfig.HandbookCatState GetCatState()
        {
            if (Count<=0)
            {
                return EnumConfig.HandbookCatState.Unknow;
            }else
            {
                if (IsFindOnLine)
                {
                    return EnumConfig.HandbookCatState.Find_Online;
                }
                else
                {
                    return EnumConfig.HandbookCatState.Find_Offline;
                }
            }
        } 

        public bool IsNew()
        {
            bool n = isNew;
            isNew = false;
            return n;
        }
    }
}
