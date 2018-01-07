using FlyModel.Proto;
using FlyModel.UI.Component;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.MailPanel
{
    public class AwardCell
    {
        private Text catNameTF;
        private Image catHead;
        private Image toyPic;
        private Text silverTF;
        private Text goldTF;

        private Image currencyIcon;

        private SoundButton getAwardBtn;

        private Model.Data.AwardData awardData;

        private Model.Data.AwardData currentLoadAwardData;

        public AwardCell(GameObject go)
        {
            catNameTF = go.transform.Find("CatName").GetComponent<Text>();
            catHead = go.transform.Find("CatHead").GetComponent<Image>();
            toyPic = go.transform.Find("ToyPic").GetComponent<Image>();

            currencyIcon = go.transform.Find("Button/Award/Type").GetComponent<Image>();
            silverTF = go.transform.Find("Button/Award/Silver").GetComponent<Text>();
            goldTF = go.transform.Find("Button/Award/Gold").GetComponent<Text>();

            getAwardBtn = new SoundButton(go.transform.Find("Button").gameObject, getAward, ResPathConfig.GET_AWARD);
        }

        public void UpdateData(Model.Data.AwardData data)
        {
            awardData = data;

            catNameTF.text = Model.Data.CatData.GetCatAlias(data.Name);
            updateCurrencyType(data);
            
            string currencyType = data.AwardType == Proto.AwardType.Coin ? ResPathConfig.ICON_SILVER_FISH : ResPathConfig.ICON_GOLD_FISH;

            currentLoadAwardData = data;
            ResourceLoader.Instance.TryLoadPic(ResPathConfig.CAT_HEAD_ASSETBUNDLE, data.CatCode, (texture) =>
            {
                if (data == currentLoadAwardData)
                {
                    catHead.sprite = texture as Sprite;
                    catHead.SetNativeSize();
                    catHead.transform.gameObject.SetActive(true);

                    ResourceLoader.Instance.TryLoadPic(ResPathConfig.ITEM_ASSETBUNDLE, data.ToyCode, (t) =>
                    {
                        toyPic.sprite = t as Sprite;
                        toyPic.SetNativeSize();
                        toyPic.transform.gameObject.SetActive(true);
                    });

                    ResourceLoader.Instance.TryLoadPic(ResPathConfig.ICON_ASSETBUNDLE, currencyType, (s) =>
                    {
                        currencyIcon.sprite = s as Sprite;
                        currencyIcon.SetNativeSize();
                        currencyIcon.transform.gameObject.SetActive(true);
                    });
                }
                else
                {
                    catHead.transform.gameObject.SetActive(false);
                    toyPic.transform.gameObject.SetActive(false);
                    currencyIcon.transform.gameObject.SetActive(false);
                }
            });
        }

        private void updateCurrencyType(Model.Data.AwardData data)
        {
            if (data.AwardType == Proto.AwardType.Coin)
            {
                silverTF.transform.gameObject.SetActive(true);
                goldTF.transform.gameObject.SetActive(false);

                silverTF.text = data.Count.ToString();
            }
            else if (data.AwardType == Proto.AwardType.Dollar)
            {
                silverTF.transform.gameObject.SetActive(false);
                goldTF.transform.gameObject.SetActive(true);

                goldTF.text = data.Count.ToString();
            }
        }
        
        private void getAward()
        {
            //Debug.Log(string.Format("领取奖励 {0}", awardData.ID));
            CommandHandle.Send(Proto.ServerMethod.DrawAward, new IDMessage() { Id = awardData.ID });
        }
    }
}
