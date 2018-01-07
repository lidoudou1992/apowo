using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI;
using FlyModel.UI.Panel.Bag;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlyModel.Model
{
    public class BagManager
    {
        public static BagManager Instance;
        public static BagManager Initialize()
        {
            if (Instance==null)
            {
                Instance = new BagManager();
            }

            return Instance;
        }

        //背包面板当前选中的玩具
        private BagItemData currentSelectedCellInBag;

        // 背包道具列表
        public List<BagItemData> BagItemList = new List<BagItemData>();

        public BagItemData CurrentSelectedCellInBag
        {
            get
            {
                return currentSelectedCellInBag;
            }

            set
            {
                currentSelectedCellInBag = value;
            }
        }

        public void AddOneBagItem(ItemData item, bool isBatch=false)
        {
            BagItemData temp = GetOneBagItemByType(item.Type);
            if (temp == null)
            {
                temp = new BagItemData();
                temp.UpdateData(item);
                temp.LoadConfig();
                BagItemList.Add(temp);

                if (isBatch==false)
                {
                    ShopManager.Instance.UpdateDataAndUI(temp);
                }
            }
            else
            {
                temp.UpdateCount(temp.Count + item.Count);
            }

            SceneManager.Instance.IsOpenFullSize = HasHouseExtensionItem();
            if (SceneManager.Instance.CurrentScene!=null && SceneManager.Instance.IsOpenFullSize)
            {
                SceneManager.Instance.CurrentScene.SceneGameObject.UpdateSceneSize();
            }
        }

        public void AddBagItems(List<Proto.FoodData> foods)
        {
            foreach (var food in foods)
            {
                AddOneBagItem(food, true);
            }
        }

        public void AddOneBagItem(Proto.FoodData food, bool isBatch=false)
        {
            BagItemData temp = GetOneBagItemByType(food.Type);
            if (temp == null)
            {
                temp = new BagItemData();
                temp.UpdateData(food);
                BagItemList.Add(temp);

                if (isBatch == false)
                {
                    ShopManager.Instance.UpdateDataAndUI(temp);
                }
            }
            else
            {
                //temp.UpdateCount(temp.Count + (int)food.Count);
                temp.UpdateCount((int)food.Count);
            }
        }

        public void AddBagItems(List<ItemData> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                AddOneBagItem(items[i], true);
            }
        }

        public void AddOneBagItem(FurniData furni, bool isBatch=false)
        {
            BagItemData temp = GetOneBagItemByType(furni.Type);
            if (temp == null)
            {
                temp = new BagItemData();
                temp.UpdateData(furni);
                temp.LoadConfig();
                BagItemList.Add(temp);

                if (isBatch == false)
                {
                    ShopManager.Instance.UpdateDataAndUI(temp);
                }
            }
            else
            {
                temp.UpdateCount(temp.Count + furni.Count);
            }
        }

        public void UpdateFurni(Proto.FurniData data) {
            BagItemData temp = GetOneBagItemByType(data.Type);
            if (temp != null)
            {
                temp.UpdateData(data);
            }
            else
            {
                Debug.LogError(string.Format("更新不存在的玩具{0}", data.Id));
            }
        }

        public void AddBagItems(List<FurniData> furnis)
        {
            for (int i = 0; i < furnis.Count; i++)
            {
                AddOneBagItem(furnis[i], true);
            }
        }

        public BagItemData GetOneBagItemByType(long type)
        {
            foreach (var item in BagItemList)
            {
                if (item.Type==type)
                {
                    return item;
                }
            }

            return null;
        }

        //public List<BagItemData> GetFoodsByType(long type)
        //{
        //    List<BagItemData> foods = new List<BagItemData>();

        //    foreach (var item in BagItemList)
        //    {
        //        if (item.Type == type)
        //        {
        //            foods.Add(item);
        //        }
        //    }

        //    return foods;
        //} 

        public List<BagItemData> GetAllItems() {
            List<BagItemData> list = new List<BagItemData>();
            foreach (var item in BagItemList)
            {
                if (item.SubType == EnumConfig.BagItemType.Item)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public List<BagItemData> GetAllFurnis()
        {
            List<BagItemData> list = new List<BagItemData>();
            foreach (var item in BagItemList)
            {
                if (item.SubType == EnumConfig.BagItemType.Furni)
                {

                }list.Add(item);
            }

            return list;
        }

        public void DeleteItem(DeleteData data)
        {
            foreach (var item in BagItemList)
            {
                if (data.Id == item.ID)
                {
                    BagItemList.Remove(item);
                    break;
                }
            }
        }

        public BagItemData FindOneItemData(long id)
        {
            foreach (var item in BagItemList)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            return null;
        }

        public void Sort()
        {
            Instance.BagItemList.Sort((BagItemData a, BagItemData b)=> {
                if (a.SubType < b.SubType)
                {
                    return -1;
                }
                else if(a.SubType>b.SubType)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });
        }

        public void SelectOnePlacePoint(BagItemData data)
        {
            PanelManager.CurrentPanel.Close();
            SceneManager.Instance.MainScene.ShowAvailable(data);
            Instance.CurrentSelectedCellInBag = data;

            PanelManager.InfoBar.SetTempMenu(() =>
            {
                SceneManager.Instance.CurrentScene.SceneGameObject.ClosePointMark();
            }, EnumConfig.InfoBarBtnMode.Close);
        }

        public void UpdateRoomID(Proto.FurniData data)
        {
            foreach (var d in BagItemList)
            {
                if (d.ID == data.Id)
                {
                    d.UpdateRoomID(data);
                    return;
                }
            }
        }

        public void UpdateRoomID(Proto.FoodData data)
        {
            foreach (var d in BagItemList)
            {
                if (d.ID == data.Id)
                {
                    d.UpdateRoomID(data);
                    return;
                }
            }
        }

        public List<BagItemData> GetAllFoods()
        {
            List<BagItemData> foods = new List<BagItemData>();

            foreach (var item in BagItemList)
            {
                if (item.SubType == EnumConfig.BagItemType.Food)
                {
                    foods.Add(item);
                }
            }

            return foods;
        }

        public List<BagItemData> GetShowDataList()
        {
            List<BagItemData> list = new List<BagItemData>();

            foreach (var item in BagItemList)
            {
                if (item.CanInBag)
                {
                    list.Add(item);

                }
            }

            return list;
        }

        public bool HasHouseExtensionItem()
        {
            foreach (var item in BagItemList)
            {
                if (item.Type == 20000)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
