using FlyModel.Model;
using FlyModel.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using FlyModel.Proto;
using GSDKUnityLib.Pay;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.RechargePanel
{
    public class RechargePanel : PanelBase
    {
        //// 我的金鱼干数量
        //private Text goldDriedfish;

        // 用来做动画
        private GameObject interGo;
        private Button buyBtn1;
        private Button buyBtn2;
        private Button buyBtn3;
        private Button buyBtn4;
        private Button buyBtn5;
        private Button buyBtn6;
        // 双倍图标
        private GameObject dou1Go;
        private GameObject dou2Go;
        private GameObject dou3Go;
        private GameObject dou4Go;
        private GameObject dou5Go;
        private GameObject dou6Go;
        // 是否显示双倍图标
        private bool isDou1;  // 默认是False
        private bool isDou2;
        private bool isDou3;
        private bool isDou4;
        private bool isDou5;
        private bool isDou6;

        // 找到面板在资源包中的预设体
        public override string AssetName
        {
            get
            {
                //return "money"; // 会显示moneyRoot
                return "Money"; // 会显示MoneyRoot
            }
        }

        // 设 RechargePanelRoot 为父物体
        public RechargePanel(RectTransform parent) : base(parent)
        {
        }

        // 重写初始化方法
        protected override void Initialize(GameObject go)
        {
            //goldDriedfish = go.transform.Find("Image/Gold/Text/Text1").GetComponent<Text>();
            //UpdateGoldDriedfishQuantity();

            interGo = go;
            buyBtn1 = go.transform.FindChild("Image/Image/Money1").GetComponent<Button>();
            //buyBtn1.onClick.AddListener(Buy);
            buyBtn1.onClick.AddListener(delegate ()
            {
                Buy(1);
            });
            buyBtn2 = go.transform.FindChild("Image/Image/Money2").GetComponent<Button>();
            buyBtn2.onClick.AddListener(delegate ()
            {
                Buy(2);
            });
            buyBtn3 = go.transform.FindChild("Image/Image/Money3").GetComponent<Button>();
            buyBtn3.onClick.AddListener(delegate ()
            {
                Buy(3);
            });
            buyBtn4 = go.transform.FindChild("Image/Image/Money4").GetComponent<Button>();
            buyBtn4.onClick.AddListener(delegate ()
            {
                Buy(4);
            });
            buyBtn5 = go.transform.FindChild("Image/Image/Money5").GetComponent<Button>();
            buyBtn5.onClick.AddListener(delegate ()
            {
                Buy(5);
            });
            buyBtn6 = go.transform.FindChild("Image/Image/Money6").GetComponent<Button>();
            buyBtn6.onClick.AddListener(delegate ()
            {
                Buy(6);
            });

            dou1Go = go.transform.FindChild("Image/Image/Money1/DouImage").gameObject;
            dou2Go = go.transform.FindChild("Image/Image/Money2/DouImage").gameObject;
            dou3Go = go.transform.FindChild("Image/Image/Money3/DouImage").gameObject;
            dou4Go = go.transform.FindChild("Image/Image/Money4/DouImage").gameObject;
            dou5Go = go.transform.FindChild("Image/Image/Money5/DouImage").gameObject;
            dou6Go = go.transform.FindChild("Image/Image/Money6/DouImage").gameObject;
        }

        // 接收充值界面的参数
        public void ReceivePars(PayItemDatas datas)
        {
            for (int i = 0; i < datas.Items.Count; i++)
            {
                if (i == 0)
                {
                    isDou1 = datas.Items[i].Double;
                }
                else if (i == 1)
                {
                    isDou2 = datas.Items[i].Double;
                }
                else if (i == 2)
                {
                    isDou3 = datas.Items[i].Double;
                }
                else if (i == 3)
                {
                    isDou4 = datas.Items[i].Double;
                }
                else if (i == 4)
                {
                    isDou5 = datas.Items[i].Double;
                }
                else if (i == 5)
                {
                    isDou6 = datas.Items[i].Double;
                }
            }
        }

        // 控制是否显示双倍图标
        public void UpdateDoubleIcon()
        {
            if (isDou1)
            {
                dou1Go.SetActive(true);
            }
            else
            {
                dou1Go.SetActive(false);
            }
            if (isDou2)
            {
                dou2Go.SetActive(true);
            }
            else
            {
                dou2Go.SetActive(false);
            }
            if (isDou3)
            {
                dou3Go.SetActive(true);
            }
            else
            {
                dou3Go.SetActive(false);
            }
            if (isDou4)
            {
                dou4Go.SetActive(true);
            }
            else
            {
                dou4Go.SetActive(false);
            }
            if (isDou5)
            {
                dou5Go.SetActive(true);
            }
            else
            {
                dou5Go.SetActive(false);
            }
            if (isDou6)
            {
                dou6Go.SetActive(true);
            }
            else
            {
                dou6Go.SetActive(false);
            }
        }

        // 购买
        private void Buy(int i)
        {
            // Pay Demo
            GSDKUnityLib.Pay.PayInfo payInfo = new GSDKUnityLib.Pay.PayInfo();
            switch (i)
            {
                case 1:
                    {
                        payInfo.PropName = "6个金鱼干";
                        payInfo.PropID = "1";  // ID:1 6元 6个金鱼干     
                        payInfo.PriceInCurrency = 600;     // 分
                                                           //payInfo.PriceInCurrency = 1;     // 分  
                        payInfo.PropDesc = "获得6个金鱼干";
                        //payInfo.ServerID = "2";   // 2 内部测试服务器 啥意思
                        payInfo.ServerID = "3";   // 苹果1区
                        if (isDou1)
                        {
                            payInfo.PropName = "12个金鱼干";
                            payInfo.PropDesc = "获得12个金鱼干";
                        }
                        break;
                    }
                case 2:
                    {
                        payInfo.PropName = "30个金鱼干";
                        payInfo.PropID = "2";  // ID:2 30元 30个金鱼干   
                        payInfo.PriceInCurrency = 3000;     // 分   
                        //payInfo.PriceInCurrency = 1;  // 分 
                        payInfo.PropDesc = "获得30个金鱼干";
                        //payInfo.ServerID = "2";   // 2 内部测试服务器 啥意思
                        payInfo.ServerID = "3";   // 苹果1区
                        if (isDou2)
                        {
                            payInfo.PropName = "60个金鱼干";
                            payInfo.PropDesc = "获得60个金鱼干";
                        }
                        break;
                    }
                case 3:
                    {
                        payInfo.PropName = "98个金鱼干";
                        payInfo.PropID = "3";  // ID:3 98元 98个金鱼干    
                        payInfo.PriceInCurrency = 9800;     // 分     
                        payInfo.PropDesc = "获得98个金鱼干";
                        //payInfo.ServerID = "2";   // 2 内部测试服务器 啥意思
                        payInfo.ServerID = "3";   // 苹果1区
                        if (isDou3)
                        {
                            payInfo.PropName = "196个金鱼干";
                            payInfo.PropDesc = "获得196个金鱼干";
                        }
                        break;
                    }
                case 4:
                    {
                        payInfo.PropName = "198个金鱼干";
                        payInfo.PropID = "4";  // ID:4 198元 198个金鱼干   
                        payInfo.PriceInCurrency = 19800;     // 分      
                        payInfo.PropDesc = "获得198个金鱼干";
                        //payInfo.ServerID = "2";   // 2 内部测试服务器 啥意思
                        payInfo.ServerID = "3";   // 苹果1区
                        if (isDou4)
                        {
                            payInfo.PropName = "396个金鱼干";
                            payInfo.PropDesc = "获得396个金鱼干";
                        }
                        break;
                    }
                case 5:
                    {
                        payInfo.PropName = "328个金鱼干";
                        payInfo.PropID = "5";  // ID:5 328元 328个金鱼干       
                        payInfo.PriceInCurrency = 32800;     // 分  
                        payInfo.PropDesc = "获得328个金鱼干";
                        //payInfo.ServerID = "2";   // 2 内部测试服务器 啥意思
                        payInfo.ServerID = "3";   // 苹果1区
                        if (isDou5)
                        {
                            payInfo.PropName = "656个金鱼干";
                            payInfo.PropDesc = "获得656个金鱼干";
                        }
                        break;
                    }
                case 6:
                    {
                        payInfo.PropName = "648个金鱼干";
                        payInfo.PropID = "6";  // ID:6 648元 648个金鱼干      
                        payInfo.PriceInCurrency = 64800;     // 分   
                        payInfo.PropDesc = "获得648个金鱼干";
                        //payInfo.ServerID = "2";   // 2 内部测试服务器 啥意思
                        payInfo.ServerID = "3";   // 苹果1区
                        if (isDou6)
                        {
                            payInfo.PropName = "1296个金鱼干";
                            payInfo.PropDesc = "获得1296个金鱼干";
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            //payInfo.PropName = "2个金鱼干";
            //payInfo.PropID = "1";  // ID:1 1元 2个金鱼干
            //payInfo.PropDesc = "获得2个金鱼干";
            //payInfo.ServerID = "2";   // 2 内部测试
            payInfo.UserID = UserManager.Instance.Me.ID.ToString();
            //payInfo.PriceInCurrency = 100;     // 分
            String oid = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
            oid += new System.Random().Next(1000000).ToString();
            payInfo.CallbackExtraInfo = oid;
            GSDKUnityLib.GSDK.Instance.StartPayAsync(payInfo, payResultInfo =>
            {
                if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.Succeed)
                {
                    Debug.Log("支付成功");
                    // 取得充值界面的数据 对应GetShopItemDatasResult
                    CommandHandle.Send(Proto.ServerMethod.GetShopItemDatas, null);
                    Refresh();
                }
                else if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.Cancelled)
                {
                    Debug.Log("支付取消");
                }
                else if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.FailedToGetOrder)
                {
                    Debug.Log("支付失败 - GSDK获取订单失败 Info:" + payResultInfo.InfoStr + " InternalErrorMsg:" + payResultInfo.InternalErrorStr);
                }
                else if (payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.Failed || payResultInfo.Status == GSDKUnityLib.Pay.EPayResultStatus.InternalError)
                {
                    Debug.Log("支付失败 - Info:" + payResultInfo.InfoStr + " InternalErrorMsg:" + payResultInfo.InternalErrorStr);
                }
                else {
                    Debug.Log("支付失败 无法识别的状态码: " + payResultInfo.Status + " Info:" + payResultInfo.InfoStr + " InternalErrorMsg:" + payResultInfo.InternalErrorStr);
                }
            });
        }

        // 重写刷新面板方法
        // 每次打开面板时自动调用
        public override void Refresh()
        {
            base.Refresh();
            UpdateDoubleIcon();
            OpenAni();
            //UpdateGoldDriedfishQuantity();
        }

        // 打开时的动画
        private void OpenAni()
        {
            interGo.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            Tweener tweener = interGo.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
            tweener.SetEase(Ease.OutCirc);
        }

        //// 更新金鱼干数量
        //private void UpdateGoldDriedfishQuantity()
        //{
        //    User self = UserManager.Instance.Me;    // 取得本账号的数据
        //    //goldDriedfish.text = self.HighCurrency.ToString();  // 设置金鱼干数量
        //}

        // 隐掉信息条
        public override void SetInfoBar()
        {
        }
    }
}