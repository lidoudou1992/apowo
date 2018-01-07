using FlyModel.UI.Component.PageView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyModel.Model.Data;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using FlyModel.Utils;
using FlyModel.Model;

namespace FlyModel.UI.Panel.HandbookPanel
{
    //public class HandbookCellItem : MonoBehaviour, IItem, IPointerClickHandler
    public class HandbookCellItem : IItem//MonoBehaviour, IItem, IPointerClickHandler
    {
        public HandbookData Data;

        private Image catImage;
        private Image unknowImage;

        private bool isSuper = false;

        private Text nameTF;
        private Text timeTF;

        public GameObject GameObject;

        private GameObject newIcon;

        public HandbookCellItem(GameObject go)
        {
            GameObject = go;

            nameTF = go.transform.Find("Name").GetComponent<Text>();
            timeTF = go.transform.Find("Time").GetComponent<Text>();
            catImage = go.transform.Find("Cat").GetComponent<Image>();
            unknowImage = go.transform.Find("Unknow").GetComponent<Image>();
            newIcon = go.transform.Find("new").gameObject;
        }

        //void Awake()
        //{
        //    nameTF = transform.Find("Name").GetComponent<Text>();
        //    timeTF = transform.Find("Time").GetComponent<Text>();
        //    catImage = transform.Find("Cat").GetComponent<Image>();
        //    unknowImage = transform.Find("Unknow").GetComponent<Image>();
        //}

        public void Init(BaseProp data, int pageIndex, int itemIndex)
        {
            Data = data as HandbookData;

            nameTF.text = CatData.GetCatAlias(Data.Name);

            timeTF.text = Data.LastShowTime;

            ResourceLoader.Instance.TryLoadPic(ResPathConfig.CAT_HEAD_ASSETBUNDLE, Data.PicCode, (texture) =>
            {
                catImage.sprite = texture as Sprite;
                catImage.SetNativeSize();
                //catImage.transform.gameObject.SetActive(true);
            });

            isSuper = Data.IsSuper;
            setCushionPic();
            updateState();

            newIcon.SetActive(Data.IsNew());
        }

        private void setCushionPic()
        {
            GameObject.transform.Find("Cushion/Normal").gameObject.SetActive(!isSuper);
            GameObject.transform.Find("Cushion/Super").gameObject.SetActive(isSuper);
            GameObject.transform.Find("InfoBg/Normal").gameObject.SetActive(!isSuper);
            GameObject.transform.Find("InfoBg/Super").gameObject.SetActive(isSuper);
        }

        private void updateState()
        {
            switch (Data.GetCatState())
            {
                case EnumConfig.HandbookCatState.Unknow:
                    catImage.gameObject.SetActive(false);
                    unknowImage.gameObject.SetActive(true);
                    GameObject.transform.Find("InfoBg").gameObject.SetActive(false);
                    nameTF.gameObject.SetActive(false);
                    timeTF.gameObject.SetActive(false);
                    break;
                case EnumConfig.HandbookCatState.Find_Offline:
                    catImage.gameObject.SetActive(true);

                    Color c = ColorConfig.HAND_BOOK_MASK;
                    catImage.color = c;

                    unknowImage.gameObject.SetActive(false);
                    GameObject.transform.Find("InfoBg").gameObject.SetActive(true);
                    nameTF.gameObject.SetActive(true);
                    timeTF.gameObject.SetActive(true);
                    break;
                case EnumConfig.HandbookCatState.Find_Online:
                    catImage.gameObject.SetActive(true);

                    c = Color.white;
                    catImage.color = c;

                    unknowImage.gameObject.SetActive(false);
                    GameObject.transform.Find("InfoBg").gameObject.SetActive(true);
                    nameTF.gameObject.SetActive(true);
                    timeTF.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (Data.GetCatState())
            {
                case EnumConfig.HandbookCatState.Unknow:
                    Debug.Log("未被发现");
                    break;
                case EnumConfig.HandbookCatState.Find_Offline:
                    //SoundUtil.PlaySound(Data.Sound);
                    //PanelManager.CatPopupPanel.Show(() =>
                    //{
                    //    PanelManager.catPopupPanel.SetData(Data);
                    //});
                    break;
                case EnumConfig.HandbookCatState.Find_Online:
                    if (string.IsNullOrEmpty(Data.Sound)==false)
                    {
                        SoundUtil.PlaySound(Data.Sound);
                    }
                    PanelManager.CatPopupPanel.Show(() =>
                    {
                        if (GuideManager.Instance.IsGestureTouchEffective("HandbookCellItem"))
                        {
                            GuideManager.Instance.ContinueGuide();
                        }
                        PanelManager.catPopupPanel.SetData(Data);
                    });
                    break;
                default:
                    break;
            }
        }
    }
}
