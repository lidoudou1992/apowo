using Assets.Scripts.Common.TableView;
using FlyModel.Model;
using FlyModel.Proto;
using FlyModel.UI.Component;
using System.Collections.Generic;
using Together;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.MailPanel
{
    public class AwardPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "AwardPanel";
            }
        }

        public TableView tableViewScript;
        private ContentController contentController;
        private SoundButton getAllBtn;
        private Scrollbar scrollbar;

        private List<Model.Data.AwardData> dataList;

        // 收获界面
        private GameObject gainGo;
        private Text goldTxt;
        private Text silverTxt;
        private Button okBtn;
        private Text adGainTxt;
        private Button playBtn;

        protected override void Initialize(GameObject go)
        {
            GameObject tableView = go.transform.Find("TableView").gameObject;
            tableViewScript = tableView.AddComponent<TableView>();

            contentController = tableView.AddComponent<ContentController>();
            contentController.tableViewScript = tableViewScript;
            contentController.cellPrefab = go.transform.Find("AwardCell").gameObject;
            contentController.GetCellForRowInTableViewHandler = GetCellForRowInTableViewHandler;

            TableViewCell awardCell = contentController.cellPrefab.AddComponent<TableViewCell>();

            getAllBtn = new SoundButton(go.transform.Find("GetAll").gameObject, getAllAwards, ResPathConfig.GET_AWARDS);

            scrollbar = go.transform.Find("Scrollbar").GetComponent<Scrollbar>();

            gainGo = go.transform.FindChild("Tip").gameObject;
            goldTxt = go.transform.FindChild("Tip/Image/Text/Text2").GetComponent<Text>();
            silverTxt = go.transform.FindChild("Tip/Image/Text/Text1").GetComponent<Text>();
            okBtn = go.transform.FindChild("Tip/Image/Exit").GetComponent<Button>();
            okBtn.onClick.AddListener(CloseThisInter);
            adGainTxt = go.transform.FindChild("Tip/Image/Button/Text/Money").GetComponent<Text>();
            playBtn = go.transform.FindChild("Tip/Image/Button").GetComponent<Button>();
            playBtn.onClick.AddListener(PlayAd);
        }

        // 播放广告
        private void PlayAd()
        {
            //// 测试用
            //CommandHandle.Send(Proto.ServerMethod.Advert, null);
            //playBtn.enabled = false;

            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    {
                        // 猫宅日记苹果版场景1的场景ID
                        if (TGSDK.CouldShowAd("NupYGtzaCG4EPVg0ISv"))
                        {
                            Debug.Log("播放广告");
                            TGSDK.ShowAd("NupYGtzaCG4EPVg0ISv");
                            // 奖励类广告达成领奖条件可以发放奖励的回调
                            TGSDK.AdRewardSuccessCallback = (string ret) =>
                            {
                                //Debug.Log("发放观看视频的奖励");
                                // 对应AdvertResult
                                CommandHandle.Send(Proto.ServerMethod.Advert, null);
                                // 禁止再次播放
                                playBtn.enabled = false;
                            };
                        }
                        else
                        {
                            Debug.Log("广告没加载好");
                        }
                        break;
                    }
                case RuntimePlatform.Android:
                    {
                        // 猫宅日记安卓版场景1的场景ID
                        if (TGSDK.CouldShowAd("MC3Xep301kSt5QBCyIv"))
                        {
                            Debug.Log("播放广告");
                            TGSDK.ShowAd("MC3Xep301kSt5QBCyIv");
                            // 奖励类广告达成领奖条件可以发放奖励的回调
                            TGSDK.AdRewardSuccessCallback = (string ret) =>
                            {
                                //Debug.Log("发放观看视频的奖励");
                                CommandHandle.Send(Proto.ServerMethod.Advert, null);
                                // 禁止再次播放
                                playBtn.enabled = false;
                            };
                        }
                        else
                        {
                            Debug.Log("广告没加载好");
                        }
                        break;
                    }
            }
        }

        // 提示观看广告后获得的奖励
        public void PromptAward(int goldNum,int silverNum)
        {
            string str = "另外获得" + adGainTxt.text;
            PanelManager.ShowTipString(str, EnumConfig.PropPopupPanelBtnModde.OneBtn);
            CloseThisInter();
        }

        // 关闭当前的领奖界面
        private void CloseThisInter()
        {
            Close();
        }

        public override void Refresh()
        {
            base.Refresh();

            dataList = AwardManager.Instance.AwardList;
            contentController.Refresh(dataList.Count);
            // 禁用收获界面
            if (gainGo.activeSelf)
            {
                gainGo.SetActive(false);
            }
            // 启用播放广告按钮
            if (playBtn.enabled == false)
            {
                playBtn.enabled = true;
            }
        }

        public override void SetInfoBar()
        {
            //base.SetInfoBar();
            //PanelManager.InfoBar.SetMenuClickedHandler(() => { Close(); });
            //PanelManager.InfoBar.SetBtnImage(Toolbar.InfoBar.BtnMode.Close);
        }

        private void getAllAwards()
        {
            if (AwardManager.Instance.AwardList.Count > 0)
            {
                CommandHandle.Send(Proto.ServerMethod.DrawAwards, null);
            }
        }

        // 领取奖励（全部领取）后
        // 显示收获界面
        public void DisplayGainInterface(int goldNum, int silverNum)
        {
            gainGo.SetActive(true);
            goldTxt.text = goldNum.ToString();
            silverTxt.text = silverNum.ToString();
            if (goldNum / 2 >= 1)  // 奖励金鱼干
            {
                adGainTxt.text = (goldNum / 2).ToString() + " 金鱼干";
            }
            else
            {
                // 奖励银鱼干
                if (silverNum / 5 >= 1)
                {
                    adGainTxt.text = (silverNum / 5).ToString() + "银鱼干";
                }
                else
                {
                    adGainTxt.text = 1.ToString() + "银鱼干";
                }
            }
        }

        private TableViewCell GetCellForRowInTableViewHandler(TableView tableView, int row)
        {
            TableViewCell cell = tableView.GetReusableCell(contentController.cellPrefab.GetComponent<TableViewCell>().reuseIdentifier) as TableViewCell;
            if (cell == null)
            {
                GameObject cellInstance = GameObject.Instantiate(contentController.cellPrefab);
                cell = cellInstance.GetComponent<TableViewCell>();
                cell.GameObject = cellInstance;

                AwardCell cellDataInstance = new AwardCell(cellInstance);

                cell.CellDataInstance = cellDataInstance;
                cellInstance.SetActive(true);
            }

            (cell.CellDataInstance as AwardCell).UpdateData(dataList[row]);

            return cell;
        }
    }
}
