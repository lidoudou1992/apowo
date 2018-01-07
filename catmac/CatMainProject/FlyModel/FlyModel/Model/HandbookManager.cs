using FlyModel.Model.Data;
using FlyModel.UI;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model
{
    public class HandbookManager
    {
        public static HandbookManager Instance;

        public List<HandbookData> catsList = new List<HandbookData>();
        public List<CatGiftData> giftList = new List<CatGiftData>();

        public static HandbookManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new HandbookManager();
            }

            return Instance;
        }

        public void InitConfigs()
        {
            initCatsConfig();
        }

        private void initCatsConfig()
        {
            ResourceLoader.Instance.TryLoadTextAsset(ResPathConfig.CATS_CONFIG, (textAssert) => {
                string text = ((TextAsset)textAssert).text;
                JsonData cats = JsonMapper.ToObject(text);

                HandbookData tempData;
                CatGiftData tempGiftData;
                JsonData jsonData;
                foreach (var cat in cats)
                {
                    jsonData = cat as JsonData;

                    tempData = new HandbookData();
                    tempData.UpdateData(jsonData);
                    catsList.Add(tempData);

                    if (int.Parse(jsonData["TreasureID"].ToString()) > 0)
                    {
                        tempGiftData = new CatGiftData();
                        tempGiftData.UpdateData(jsonData);
                        giftList.Add(tempGiftData);
                    }
                }
            });
        }

        public HandbookData FindOneCatByType(long type)
        {
            foreach (var cat in catsList)
            {
                if (cat.Type == type)
                {
                    return cat;

                }
            }

            return null;
        }

        public CatGiftData FindOneCatGiftByType(long type)
        {
            foreach (var gift in giftList)
            {
                if (gift.Type == type)
                {
                    return gift;

                }
            }

            return null;
        }

        public void UpdateData(Proto.PicData data, bool needNewFlag=false)
        {
            HandbookData cat = FindOneCatByType(data.Type);
            if (cat != null)
            {
                cat.UpdateData(data, needNewFlag);
            }

            CatGiftData gift = FindOneCatGiftByType(data.TreasureID);
            if (gift != null)
            {
                GuideManager.Instance.HasGettedOneTreasure = true;
                gift.UpdateData(data, needNewFlag);
            }
        }

        public void UpdateCats(List<Proto.PicData> datas)
        {
            foreach (var cat in datas)
            {
                UpdateData(cat);
            }
        }

        public List<HandbookData> GetAppearCats()
        {
            List<HandbookData> cats = new List<HandbookData>();

            foreach (var cat in catsList)
            {
                if (cat.GetCatState() == EnumConfig.HandbookCatState.Find_Online)
                {
                    cats.Add(cat);
                }
            }

            return cats;
        }

        public int GetAppearCount()
        {
            int count = 0;
            foreach (var cat in catsList)
            {
                if (cat.GetCatState() == EnumConfig.HandbookCatState.Find_Online)
                {
                    count++;
                }

            }

            return count;
        }

        public int GetGettedGiftCount()
        {
            int count = 0;

            foreach (var gift in giftList)
            {
                if (gift.IsGetted)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
