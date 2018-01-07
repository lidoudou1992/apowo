using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.SceneChangePanel
{
    public class SceneCell
    {
        public GameObject GameObject;

        private Image image;
        private GameObject lockImage;
        private GameObject priceGO;
        private Text priceTF;

        private Model.Data.RoomData Data;

        private Material mat;

        private GameObject halfPriceImage;

        public SceneCell(GameObject go)
        {
            GameObject = go;

            image = go.transform.Find("Image").GetComponent<Image>();
            lockImage = go.transform.Find("lock").gameObject;
            priceGO = go.transform.Find("Price").gameObject;
            priceTF = go.transform.Find("Price/Text").GetComponent<Text>();
            halfPriceImage = go.transform.Find("HalfPrice").gameObject;
            halfPriceImage.SetActive(false);

            mat = new Material(Shader.Find("CatProj/UIGray"));

            new SoundButton(go, () => {
                if (GuideManager.Instance.IsGestureTouchEffective("SceneCell"))
                {
                    GuideManager.Instance.ContinueGuide();
                }
                if (SceneManager.Instance.CurrentSceneName != Data.AssetBundleName)
                {
                    if (Data.own)
                    {
                        PanelManager.ShowTipString("是否更换场景？",
                                EnumConfig.PropPopupPanelBtnModde.TwoBtb,
                                "更换",
                                () => {
                                    SceneManager.Instance.ChangeScene(Data.Type);
                                }
                            );
                    }
                    else
                    {
                        if (BagManager.Instance.HasHouseExtensionItem())
                        {
                            PanelManager.ShowTipString("是否购买该场景？",
                                EnumConfig.PropPopupPanelBtnModde.TwoBtb,
                                "购买",
                                () => {
                                    User me = UserManager.Instance.Me;
                                    int money = (int)me.HighCurrency;
                                    int price = RoomManager.Instance.GetScenePrice();
                                    if (money >= price)
                                    {
                                        CommandHandle.Send(Proto.ServerMethod.BuyScene, new IDMessage() { Id = Data.Type });
                                    }
                                    else
                                    {
                                        PanelManager.ShowNoMoneyTip();
                                    }
                                }
                                );
                        }
                        else
                        {
                            PanelManager.ShowTipString("请先购买房屋扩张道具", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                        }
                    }
                }
            });
        }

        internal void UpdateData(Model.Data.RoomData data)
        {
            Data = data;

            ResourceLoader.Instance.TryLoadPic(ResPathConfig.SCENE_IMAGE, data.PicCode, (texture) =>
            {
                image.sprite = texture as Sprite;
                image.SetNativeSize();
                image.transform.gameObject.SetActive(true);

                //image.material = data.own ? null : mat;

                lockImage.SetActive(data.own == false);

                priceGO.SetActive(data.own == false && RoomManager.Instance.isFirstBuyTime()==false);
                priceTF.text = data.Price.ToString();

                halfPriceImage.SetActive(RoomManager.Instance.isFirstBuyTime() && data.own==false);
            });
        }
    }
}
