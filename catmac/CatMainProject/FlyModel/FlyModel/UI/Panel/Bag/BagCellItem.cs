using FlyModel.UI.Component.PageView;
using System;
using FlyModel.Model.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FlyModel.Model;
using LitJson;
using FlyModel.UI.Component;
using FlyModel.Proto;
using FlyModel.Utils;
using DG.Tweening;

namespace FlyModel.UI.Panel.Bag
{
    //public class BagCellItem : MonoBehaviour, IItem, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    public class BagCellItem:IItem //: MonoBehaviour, IItem, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Text nameTF;
        private Image image;
        private GameObject silverGO;
        private GameObject goldGO;
        private GameObject currentCurrencyGO;
        private GameObject sizeGO;

        public BagItemData Data;

        private JsonData itemConfig;

        private GameObject selectedPic;

        public int PageIndex;
        public int ItemIndex;

        private bool isSelected;
        private float effectY = 50;

        private Image placedImage;

        GameObject countGO;

        public GameObject GameObject;

        public BagCellItem(GameObject go)
        {
            GameObject = go;

            nameTF = go.transform.Find("Name").GetComponent<Text>();
            image = go.transform.Find("PropContent/Prop").GetComponent<Image>();
            image.transform.gameObject.SetActive(false);
            selectedPic = go.transform.Find("PropContent/Selected").gameObject;
            selectedPic.transform.gameObject.SetActive(false);

            silverGO = go.transform.Find("SilverPrice").gameObject;
            silverGO.SetActive(false);
            goldGO = go.transform.Find("GoldPrice").gameObject;
            goldGO.SetActive(false);

            sizeGO = go.transform.Find("Size").gameObject;

            countGO = go.transform.Find("Count").gameObject;

            placedImage = go.transform.Find("Placed").GetComponent<Image>();
        }

        public void Init(BaseProp data, int pageIndex, int itemIndex)
        {
            PageIndex = pageIndex;
            ItemIndex = itemIndex;

            Data = data as BagItemData;

            string configName = "";
            if (Data.SubType == EnumConfig.BagItemType.Food)
            {
                configName = ResPathConfig.FOOD_CONFIG;
                showCount(Data.Type==2001 ? -1 : Data.Count);
            }
            else if (Data.SubType == EnumConfig.BagItemType.Furni)
            {
                configName = ResPathConfig.TOY_CONFIG;
            }

            ConfigUtil.GetConfig(configName, data.Type, (config) =>
            {
                if (config != null)
                {
                    itemConfig = config;

                    ResourceLoader.Instance.TryLoadPic(ResPathConfig.ITEM_ASSETBUNDLE, config["Code"].ToString(), (texture) =>
                    {
                        image.sprite = texture as Sprite;
                        image.SetNativeSize();
                        image.transform.gameObject.SetActive(true);
                    });

                    nameTF.text = config["Name"].ToString();

                    setCurrenyType(int.Parse(itemConfig["Currency"].ToString()));
                    setCurrencyBgDir();
                    setSizeIcon(int.Parse(itemConfig["FitType"].ToString()));

                    UpdatePlaceState();
                }
                else
                {
                    Debug.Log(string.Format("玩具配置 Type:{0} 不存在", data.Type));
                }
            });
        }

        private void showCount(int count)
        {
            if (count>0)
            {
                Text text = countGO.transform.Find("Text").GetComponent<Text>();
                text.text = count.ToString();
                countGO.SetActive(true);
            }
        }

        private void setCurrenyType(int type)
        {
            if (type == (int)Currency.Coin)
            {
                silverGO.SetActive(true);
                goldGO.SetActive(false);
                currentCurrencyGO = silverGO;
            }
            else if (type == (int)Currency.Dollar)
            {
                silverGO.SetActive(false);
                goldGO.SetActive(true);
                currentCurrencyGO = goldGO;
            }
            else
            {
                silverGO.SetActive(true);
                goldGO.SetActive(false);
                currentCurrencyGO = silverGO;
            }
        }

        private void setCurrencyBgDir()
        {
            int pos = ItemIndex % 3;
            if (currentCurrencyGO)
            {
                GameObject left = currentCurrencyGO.transform.Find("bg_1").gameObject;
                GameObject middle = currentCurrencyGO.transform.Find("bg_2").gameObject;
                GameObject right = currentCurrencyGO.transform.Find("bg_3").gameObject;

                left.SetActive(pos == 0);
                middle.SetActive(pos == 1);
                right.SetActive(pos == 2);
                currentCurrencyGO.SetActive(true);
            }
        }

        private void setSizeIcon(int size)
        {
            sizeGO.transform.Find("Small").gameObject.SetActive(size == (int)EnumConfig.ToySizeType.Small);
            sizeGO.transform.Find("Large").gameObject.SetActive(size == (int)EnumConfig.ToySizeType.Large);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isSelected == false)
            {
                SoundUtil.PlaySound(ResPathConfig.SHOP_SELETE);
                PanelManager.bagPanel.ShowItemSelected(PageIndex, ItemIndex);

                if (GuideManager.Instance.IsGestureTouchEffective("BagCellItem"))
                {
                    GuideManager.Instance.ContinueGuide();
                }
            }
            else
            {
                SoundUtil.PlaySound(ResPathConfig.SHOP_POPUPPANEL);
                if (Data.IsPlaced())
                {
                    PanelManager.PropPopupPanel.Show(() =>
                    {
                        PropPopupModeStruct modeStruct = new PropPopupModeStruct();

                        modeStruct.Mode = EnumConfig.PropPopupPanelBtnModde.TwoBtb;

                        modeStruct.RightSound = ResPathConfig.RECYCLE_BAG;

                        modeStruct.LeftBtnString = "重新摆放";
                        modeStruct.LeftCallback = () =>
                        {
                            PanelManager.CurrentPanel.Close();
                            BagManager.Instance.SelectOnePlacePoint(Data);
                        };

                        modeStruct.RightBtnString = "撤回玩具";
                        modeStruct.RightCallback = () =>
                        {
                            CommandHandle.Send(ServerMethod.PickUpObject, new IDMessage() { Id = Data.ID });
                            PanelManager.CurrentPanel.Close();
                        };

                            PanelManager.propPopupPanel.SetData(Data, modeStruct);
                    });
                }
                else
                {
                    BagManager.Instance.SelectOnePlacePoint(Data);

                    if (GuideManager.Instance.IsGestureTouchEffective("BagCellItem"))
                    {
                        GuideManager.Instance.ContinueGuide();
                    }
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void ShowSelectedState(bool isShow)
        {
            selectedPic.SetActive(isShow);

            isSelected = isShow;

            showEffect(isSelected);
        }

        private void showEffect(bool isSelected)
        {
            Tweener t = image.transform.DOLocalMoveY(isSelected ? effectY : 0, 0.2f);
        }

        public void UpdatePlaceState()
        {
            bool placeMark = Data.IsPlaced() && Data.SubType == EnumConfig.BagItemType.Furni;
            Color c;
            if (placeMark)
            {
                c = ColorConfig.SHOP_MASK;
            }
            else
            {
                c = Color.white;
            }

            image.color = c;

            placedImage.gameObject.SetActive(placeMark);
        }
    }
}
