using FlyModel.Model.Data;
using FlyModel.UI;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model
{
    public class ShopManager
    {
        public static ShopManager Instance;

        public static ShopManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new ShopManager();
            }

            return Instance;
        }

        public List<ShopItemData> ShopItemList = new List<ShopItemData>();

        public ShopItemData FreeFoodShopItemData;

        public void InitConfigs()
        {
            initFoodsConfig();
        }

        private void initFoodsConfig()
        {
            ResourceLoader.Instance.TryLoadTextAsset(ResPathConfig.FOOD_CONFIG, (textAssert) => {
                string text = ((TextAsset)textAssert).text;
                JsonData foods = JsonMapper.ToObject(text);
                
                //var jo = JsonSerializer.Deserialize(text);
                //Debug.Log(jo.ToString());

                ShopItemData tempData;
                foreach (var food in foods)
                {
                    if (int.Parse((food as IDictionary)["ID"].ToString()) != 2001)
                    {
                        tempData = new ShopItemData(EnumConfig.ShopItemType.Food);
                        tempData.UpdateData(food as JsonData);
                        ShopItemList.Add(tempData);
                    }
                    else
                    {
                        FreeFoodShopItemData = new ShopItemData(EnumConfig.ShopItemType.Food);
                        FreeFoodShopItemData.UpdateData(food as JsonData);
                    }
                }

                InitItemsConfig();
            });
        }

        public void InitItemsConfig()
        {
            ResourceLoader.Instance.TryLoadTextAsset(ResPathConfig.ITEM_CONFIG, (textAssert) => {
                JsonData items = JsonMapper.ToObject((textAssert as TextAsset).text);

                ShopItemData tempData;
                foreach (var item in items)
                {
                    tempData = new ShopItemData(EnumConfig.ShopItemType.Item);
                    tempData.UpdateData(item as JsonData);
                    ShopItemList.Add(tempData);
                }

                initCurrencyExchangeConfig();
            });
        }        

        private void initCurrencyExchangeConfig()
        {
            ResourceLoader.Instance.TryLoadTextAsset(ResPathConfig.Currency_Exchange_CONFIG, (textAssert) => {
                string text = ((TextAsset)textAssert).text;
                 
                JsonData exchanges = JsonMapper.ToObject(text);

                ShopItemData tempData;
                foreach (var exchange in exchanges)
                {
                    tempData = new ShopItemData(EnumConfig.ShopItemType.Currency);
                    tempData.UpdateData(exchange as JsonData);
                    ShopItemList.Add(tempData);
                }

                initToysConfig();
            });
        }

        private void initToysConfig()
        {
            ResourceLoader.Instance.TryLoadTextAsset(ResPathConfig.TOY_CONFIG, (textAssert) => {
                string text = ((TextAsset)textAssert).text;
                JsonData toys = JsonMapper.ToObject(text);

                ShopItemData tempData;
                foreach (var toy in toys)
                {
                    tempData = new ShopItemData(EnumConfig.ShopItemType.Toy);
                    tempData.UpdateData(toy as JsonData);
                    ShopItemList.Add(tempData);
                }
            });
        }

        //public ShopItemData UpdateData(Proto.ShopData data)
        //{
        //    foreach (var shopItemData in ShopItemList)
        //    {
        //        if (shopItemData.Type == data.Id)
        //        {
        //            return shopItemData.UpdateData(data);
        //        }
        //    }

        //    return null;
        //}

        //public void UpdateDatas(List<Proto.ShopData> datas)
        //{
        //    foreach (var data in datas)
        //    {
        //        UpdateData(data);
        //    }
        //}

        public void UpdateDataAndUI(BagItemData data)
        {
            var shopItemData = ShopManager.Instance.UpdateData(data);
            if (PanelManager.IsCurrentPanel(PanelManager.shopPanel))
            {
                PanelManager.shopPanel.UpdateData(shopItemData);
            }
        }

        public ShopItemData UpdateData(Model.Data.BagItemData data)
        {
            foreach (var shopItemData in ShopItemList)
            {
                if (shopItemData.Type == data.Type)
                {
                    return shopItemData.UpdateData(data);
                }
            }

            return null;
        }

        public void UpdateDatas(List<Model.Data.BagItemData> datas)
        {
            foreach (var data in datas)
            {
                UpdateData(data);
            }
        }

        public List<ShopItemData> GetAllFoods()
        {
            List<ShopItemData> foods = new List<ShopItemData>();

            foreach (var item in ShopItemList)
            {
                if (item.ItemSubType == EnumConfig.ShopItemType.Food)
                {
                    foods.Add(item);
                }

            }

            return foods;
        }

        public int GetFoodIndex(long type)
        {
            for (int i = 0; i < ShopItemList.Count; i++)
            {
                if (ShopItemList[i].Type == type)
                {
                    return i;

                }
            }

            return -1;
        }

        public List<ShopItemData> GetShowInStoreList()
        {
            List<ShopItemData> list = new List<ShopItemData>();

            foreach (var item in ShopItemList)
            {
                if (item.IsForCell() && item.CanShowInStore() && item.IsOpen())
                {
                    list.Add(item);
                }
            }

            return list;
        }
    }
}
