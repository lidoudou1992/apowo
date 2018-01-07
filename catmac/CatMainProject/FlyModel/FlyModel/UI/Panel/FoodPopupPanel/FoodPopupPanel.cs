using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FlyModel.Model.Data;
using UnityEngine.UI;
using FlyModel.Model;
using FlyModel.UI.Scene.ViewObject.Data;
using FlyModel.Proto;
using FlyModel.Utils;
using Assets.Scripts.TouchController;
using FlyModel.UI.Scene;
using LitJson;

namespace FlyModel.UI.Panel.FoodPopupPanel
{
    public class FoodPopupPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "FoodPopupPanel";
            }
        }

        private Transform container;
        private Slider progress;

        private Model.Data.FoodData foodGOData;

        private FoodGameObject selectedFoodPoint;

        private List<Food> foodList = new List<Food>();

        private GameObject containerParent;

        protected override void Initialize(GameObject go)
        {
            PointerSensor sensor = PanelPrefab.AddComponent<PointerSensor>();
            sensor.OnPointerClickHandler = clickedHandler;

            containerParent = go.transform.Find("GameObject").gameObject;

            container = go.transform.Find("GameObject/Container");
            progress = go.transform.Find("GameObject/Slider").GetComponent<Slider>();

            Transform food;
            for (int i = 0; i < container.childCount; i++)
            {
                food = container.GetChild(i);
                foodList.Add(new Food(food.gameObject));
            }
        }

        private void clickedHandler(PointerEventData eventData)
        {
            SoundUtil.PlaySound(ResPathConfig.COMMON_BUTTON);
            Close();
        }

        private void showFoods(Model.Data.FoodData foodData)
        {
            List<ShopItemData> datas = ShopManager.Instance.GetAllFoods();
            List<ShopItemData> foods = new List<ShopItemData>();

            foods.Add(ShopManager.Instance.FreeFoodShopItemData);

            foreach (var data in datas)
            {
                foods.Add(data);
            }

            List<ShopItemData> fs = new List<ShopItemData>();
            fs.Add(new ShopItemData(EnumConfig.ShopItemType.Food));
            for (int i = 0; i < foods.Count; i++)
            {
                if (foods[i].Type == foodData.Type)
                {
                    fs[0] = foods[i];
                }
                else
                {
                    fs.Add(foods[i]);
                }
            }

            ShopItemData tempBagItemData;
            for (int i = 0; i < fs.Count; i++)
            {
                tempBagItemData = fs[i];
                foodList[i].SetData(tempBagItemData);
                foodList[i].ShowCurrentMark(tempBagItemData.Type == foodData.Type);
            }
        }

        private void UpdateFoods()
        {
            foreach (var foodCom in foodList)
            {
                foodCom.UpdateData();

            }
        }

        public override void Close(bool isCloseAllMode = false)
        {
            base.Close(isCloseAllMode);

            SceneManager.Instance.CurrentScene.SceneGameObject.AutoRestoreFoodPos();
        }

        public override void SetInfoBar()
        {
            
        }

        public override void RefreshWhenBack()
        {
            base.RefreshWhenBack();

            PanelManager.infoBar.SetInfoBar();
            PanelManager.infoBar.transform.SetAsFirstSibling();

            UpdateFoods();
        }

        private void setPosition(GameObject selectedFoodPoint)
        {
            float x = (selectedFoodPoint.transform.position.x);
            float y = selectedFoodPoint.transform.position.y;
            containerParent.transform.position = new Vector3(x, y, 0);
        }

        public void SetData(Model.Data.FoodData foodData, FoodGameObject selectedFoodPoint)
        {
            setPosition(selectedFoodPoint.Root);

            foodGOData = foodData;
            this.selectedFoodPoint = selectedFoodPoint;

            progress.value = foodData.GetProgress();

            showFoods(foodData);
        }

        public void PlaceFood(BagItemData bagItemData)
        {
            if (foodGOData.ProgressStep< FoodLevel._4)
            {
                PanelManager.AlertPanel.Show(() => {
                    PropPopupModeStruct modeStruct = new PropPopupModeStruct();

                    modeStruct.Mode = EnumConfig.PropPopupPanelBtnModde.TwoBtb;

                    modeStruct.LeftCallback = () => {
                        PanelManager.alertPanel.Close();
                    };

                    modeStruct.RightBtnString = "替换";
                    modeStruct.RightCallback = () => {
                        PanelManager.alertPanel.Close();
                        sendPlaceFoodMsg(bagItemData);
                    };

                    PanelManager.alertPanel.SetData("猫粮尚未吃完，是否强制替换？", modeStruct);
                });
            }
            else
            {
                sendPlaceFoodMsg(bagItemData);
            }
        }

        private void sendPlaceFoodMsg(BagItemData bagItemData)
        {
            ScenePointStruct pointStruct = selectedFoodPoint.PlaceIndexStruct;

            CommandHandle.Send(ServerMethod.PlaceObject, new PutObjectRequest()
            { Id = bagItemData.ID, ScenePointIndex = pointStruct.ScenePointIndex - 1, SubPointIndex = pointStruct.SubPointIndex, RoomSectionType = (RoomSectionType)((int)pointStruct.Distribution) });

            PanelManager.CurrentPanel.Close();
        }

        public void SendBuyAndPlaceFoodMsg(ShopItemData data)
        {
            ScenePointStruct pointStruct = selectedFoodPoint.PlaceIndexStruct;

            PanelManager.CurrentPanel.Close();
            CommandHandle.Send(ServerMethod.EasyBuyFood, new EasyIDMessage()
            { Id = data.Type, ScenePointIndex = pointStruct.ScenePointIndex - 1, SubPointIndex = pointStruct.SubPointIndex, RoomSectionType = (RoomSectionType)((int)pointStruct.Distribution) });
        }
    }
}
