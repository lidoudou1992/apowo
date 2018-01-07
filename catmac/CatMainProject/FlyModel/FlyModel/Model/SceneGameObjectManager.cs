using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model
{
    class SceneGameObjectManager
    {
        public static SceneGameObjectManager Instance;
        public static SceneGameObjectManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new SceneGameObjectManager();
            }
            return Instance;
        }

        public List<ToyData> ToyDataList = new List<ToyData>();
        public List<Data.FoodData> FoodDataList = new List<Data.FoodData>();

        public void AddToyDatas(List<FurniData> datas)
        {
            foreach (var item in datas)
            {
                //if (item.Type == 3035)
                //{
                //    AddOneToyData(item);
                //}

                AddOneToyData(item);
            }
        }

        public ToyData AddOneToyData(FurniData data)
        {
            ToyData toyData = new ToyData();
            toyData.updateData(data);
            ToyDataList.Add(toyData);

            return toyData;
        }

        public EnumConfig.BagItemType DeleteOneSceneGameObject(long id)
        {
            foreach (var toyData in ToyDataList)
            {
                if (toyData.ID == id)
                {
                    ToyDataList.Remove(toyData);
                    return EnumConfig.BagItemType.Furni;
                }
            }

            foreach (var foodData in FoodDataList)
            {
                if (foodData.ID == id)
                {
                    FoodDataList.Remove(foodData);
                    return EnumConfig.BagItemType.Food;
                }
            }

            return EnumConfig.BagItemType.Null;
        }

        public void UpdateToyData(FurniData data)
        {
            for (int i = 0; i < ToyDataList.Count; i++)
            {
                if (ToyDataList[i].ID == data.Id)
                {
                    ToyDataList[i].updateData(data);
                    break;
                }
            }
        }

        public Data.FoodData AddOneFoodData(Proto.FoodData data)
        {
            Data.FoodData foodData = new Data.FoodData();
            foodData.UpdateData(data);
            FoodDataList.Add(foodData);

            return foodData;
        }

        public void AddFoodDatas(List<Proto.FoodData> datas)
        {
            foreach (var data in datas)
            {
                AddOneFoodData(data);
            }
        }

        public void ClearData()
        {
            ToyDataList.Clear();
            FoodDataList.Clear();
        }
    }
}
