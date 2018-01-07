using FlyModel.UI;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class ShopItemData:BaseProp
    {
        public EnumConfig.ShopItemType ItemSubType;
        public EnumConfig.ToySizeType Size;
        public int MaxCount = 1;
        public bool AlwaysInShop;

        public bool isForSell = true;

        //0是所有都能放
        public int SceneID = 0;
        public Proto.ToyCategory OpenNode = Proto.ToyCategory.Init;

        public ShopItemData(EnumConfig.ShopItemType subType)
        {
            ItemSubType = subType;

            Size = EnumConfig.ToySizeType.Small;
        }

        public void UpdateData(JsonData data)
        {
            //Debug.Log(JsonMapper.ToJson(data));
            Name = data["Name"].ToString();
            Type = long.Parse(data["ID"].ToString());
            Description = data["Description"].ToString();
            CurrencyType = (Proto.Currency)int.Parse(data["Currency"].ToString());
            PicCode = data["Code"] == null ? "" : data["Code"].ToString();
            Price = int.Parse(data["Price"].ToString());

            if (data["Code"] == null)
            {
                Debug.LogWarning(string.Format("{0} code字段未设置", Name));
            }

            switch (ItemSubType)
            {
                case EnumConfig.ShopItemType.Food:
                    Size = EnumConfig.ToySizeType.Small;
                    MaxCount = int.Parse(data["OwnerCount"].ToString());
                    AppendExplaination = data["DurationDesc"].ToString();
                    OpenNode = (Proto.ToyCategory)int.Parse(data["Category"].ToString());
                    break;
                case EnumConfig.ShopItemType.Currency:
                    Size = EnumConfig.ToySizeType.Null;
                    MaxCount = 1;
                    break;
                case EnumConfig.ShopItemType.Toy:
                    Size = (EnumConfig.ToySizeType)int.Parse(data["FitType"].ToString());
                    MaxCount = int.Parse(data["OwnerCount"].ToString());

                    SceneID = int.Parse(data["SceneID"].ToString());
                    OpenNode = (Proto.ToyCategory)int.Parse(data["Category"].ToString());

                    Price = (int)Math.Floor(Price * Model.Data.EffectiveData.TOY_PRICE_RATE);

                    isForSell = int.Parse(data["Currency"].ToString()) != 0;                    
                    break;
                case EnumConfig.ShopItemType.Item:
                    AlwaysInShop = (bool)data["AlwaysInShop"];
                    break;
                default:
                    break;
            }

        }

        public ShopItemData UpdateData(Model.Data.BagItemData data)
        {
            Count = data.Count;
            return this;
        }

        public bool CanBuy()
        {
            if (Type == 20000)
            {
                return BagManager.Instance.HasHouseExtensionItem()==false;
            }
            else
            {
                return Count < MaxCount;
            }
        }

        public bool CanShowInStore()
        {
            return CanBuy() || AlwaysInShop;
        }

        public bool IsOpen()
        {
            if (OpenNode == Proto.ToyCategory.Init)
            {
                return true;
            }else if (OpenNode == Proto.ToyCategory.Expand)
            {
                return BagManager.Instance.HasHouseExtensionItem();
            }
            else if(OpenNode == Proto.ToyCategory.Specially)
            {
                return RoomManager.Instance.HasRoom(SceneID);
            }

            return false;
        }

        public bool IsForCell()
        {
            return isForSell;
        }
    }
}
