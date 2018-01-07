using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyModel.Model.Data
{
    public class RoomData:BaseProp
    {
        public string AssetBundleName = "Scene001";

        public bool own;

        public void UpdateData(Proto.RoomData data)
        {
            ID = data.Id;

            own = true;
        }

        public void UpdateData(JsonData data)
        {
            Type = long.Parse(data["ID"].ToString());
            Name = data["Name"].ToString();
            PicCode = data["Icon"].ToString();
            AssetBundleName = data["Code"].ToString();
            CurrencyType = (Proto.Currency)int.Parse(data["Currency"].ToString());
            Price = int.Parse(data["Price"].ToString());
        }
    }
}
