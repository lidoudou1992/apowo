using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class CatGiftData:BaseProp
    {
        public bool IsGetted;
        public string CatPicCode;

        private bool isNew;

        public void UpdateData(JsonData data)
        {
            Type = int.Parse(data["TreasureID"].ToString());
            Name = data["TreasureName"].ToString();
            PicCode = data["TreasurePicName"].ToString();
            Description = data["TreasureDescription"].ToString();

            CatPicCode = data["Code"].ToString();
        }

        public void UpdateData(Proto.PicData data, bool needNewFlag)
        {
            if (needNewFlag && IsGetted==false)
            {
                isNew = data.IsSentTreasure;
            }

            IsGetted = data.IsSentTreasure;
        }

        public bool IsNew()
        {
            bool n = isNew;
            isNew = false;
            return n;
        }
    }
}
