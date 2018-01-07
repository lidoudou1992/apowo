using FlyModel.Proto;
using FlyModel.UI;
using FlyModel.Utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class BagItemData:BaseProp
    {
        public EnumConfig.BagItemType SubType;
        public long RoomID;
        public bool IsSmallType = true;

        private JsonData config;

        public bool CanInBag = true;

        public BagItemData()
        {

        }

        public void UpdateData(Proto.ItemData data)
        {
            ItemData d = data as ItemData;
            ID = d.Id;
            Type = d.Type;
            Count = d.Count;
            SubType = EnumConfig.BagItemType.Item;
        }

        public void UpdateData(Proto.FurniData data)
        {
            FurniData f = data as FurniData;
            ID = f.Id;
            Type = f.Type;
            Count = f.Count;
            SubType = EnumConfig.BagItemType.Furni;

            RoomID = f.RoomID;
        }

        public void UpdateData(Proto.FoodData data)
        {
            Proto.FoodData food = data as Proto.FoodData;
            ID = food.Id;
            Type = food.Type;
            Count = (int)food.Count;
            SubType = EnumConfig.BagItemType.Food;

            RoomID = food.RoomID;
        }

        public void UpdateCount(int count)
        {
            Count = count;
        }

        public void LoadConfig()
        {
            if (SubType == EnumConfig.BagItemType.Furni)
            {
                ConfigUtil.GetConfig(ResPathConfig.TOY_CONFIG, Type, (json) =>
                {
                    config = json;

                    parseConfig();
                });
            }
            else if (SubType == EnumConfig.BagItemType.Item)
            {
                ConfigUtil.GetConfig(ResPathConfig.ITEM_CONFIG, Type, (json) =>
                {
                    config = json;

                    CanInBag = (bool)config["CanInBag"];
                });
            }
        }

        private void parseConfig()
        {
            IsSmallType = (EnumConfig.ToySizeType)int.Parse(config["FitType"].ToString()) == EnumConfig.ToySizeType.Small;

            Name = config["Name"].ToString();
            Type = long.Parse(config["ID"].ToString());
            Description = config["Description"].ToString();
            CurrencyType = (Proto.Currency)int.Parse(config["Currency"].ToString());
            PicCode = config["Code"].ToString();
            Price = int.Parse(config["Price"].ToString());
        }

        public bool IsPlaced()
        {
            return RoomID > 0;
        }

        public void UpdateRoomID(Proto.FurniData data)
        {
            RoomID = data.RoomID;
        }

        public void UpdateRoomID(Proto.FoodData data)
        {
            RoomID = data.RoomID;
        }
    }
}
