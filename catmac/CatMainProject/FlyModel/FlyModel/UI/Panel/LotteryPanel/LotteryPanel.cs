using FlyModel.Model;
using FlyModel.UI.Component;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyModel.Proto;
using FlyModel.Utils;
using Together;
using UnityEngine;
using UnityEngine.UI;
using AnimationState = Spine.AnimationState;

namespace FlyModel.UI.Panel.LotteryPanel
{
    public class LotteryPanel : PanelBase
    {
        // 第一部分 组件声明和新建列表 按照创建顺序排列
        // 奖励1的骨骼图形组件
        public SkeletonGraphic skeletonGraphic01;
        // 奖励2的骨骼图形组件
        public SkeletonGraphic skeletonGraphic02;
        // 奖励3的骨骼图形组件
        public SkeletonGraphic skeletonGraphic03;
        // 奖励4的骨骼图形组件
        public SkeletonGraphic skeletonGraphic04;
        // 奖励5的骨骼图形组件
        public SkeletonGraphic skeletonGraphic05;
        // 奖励6的骨骼图形组件
        public SkeletonGraphic skeletonGraphic06;
        // 奖励7的骨骼图形组件
        public SkeletonGraphic skeletonGraphic07;
        // 奖励8的骨骼图形组件
        public SkeletonGraphic skeletonGraphic08;
        // 奖励9的骨骼图形组件
        public SkeletonGraphic skeletonGraphic09;
        // 奖励10的骨骼图形组件
        public SkeletonGraphic skeletonGraphic10;
        // 奖励11的骨骼图形组件
        public SkeletonGraphic skeletonGraphic11;
        // 奖励12的骨骼图形组件
        public SkeletonGraphic skeletonGraphic12;

        // 使用列表来储存12个奖励
        public List<SkeletonGraphic> skeletonGraphicList = new List<SkeletonGraphic>();

        private Button startLotteryBtn;  // 开始抽奖按钮的按钮组件

        private DelayController delayController;  // 延时控制器 延时出现单确定弹窗

        private Button closeBtn;  // 关闭按钮

        public string silverNumString = "您本次抽奖免费";  // 每次抽奖需要花费多少银鱼干

        private Text priceTxt;  // 每次抽奖价格的文本
        private Button playAdBtn;

        // 第二部分 
        // 找到预设体资源
        public override string AssetName
        {
            get
            {
                return "qiandao";
            }
        }

        // 第三部分
        // 设 LotteryPanelRoot 为父物体
        public LotteryPanel(RectTransform parent) : base(parent)
        {
        }

        // 第四部分
        // 初始化 主要是取得组件操作 顺序排序
        protected override void Initialize(GameObject go)
        {
            // 开始抽奖按钮
            new SoundButton(go.transform.Find("Image/GetAll").gameObject, () =>
            {
                SendLotteryCommand();
            });

            // 取到奖励1的SkeletonGraphic组件
            skeletonGraphic01 = go.transform.Find("Image/Image/jiangli1/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic01);  // 添加到列表中
            // 取到奖励2的SkeletonGraphic组件
            skeletonGraphic02 = go.transform.Find("Image/Image/jiangli2/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic02);  // 添加到列表中
            // 取到奖励3的SkeletonGraphic组件
            skeletonGraphic03 = go.transform.Find("Image/Image/jiangli3/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic03);  // 添加到列表中
            // 取到奖励4的SkeletonGraphic组件
            skeletonGraphic04 = go.transform.Find("Image/Image/jiangli4/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic04);  // 添加到列表中
            // 取到奖励5的SkeletonGraphic组件
            skeletonGraphic05 = go.transform.Find("Image/Image/jiangli5/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic05);  // 添加到列表中
            // 取到奖励6的SkeletonGraphic组件
            skeletonGraphic06 = go.transform.Find("Image/Image/jiangli6/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic06);  // 添加到列表中
            // 取到奖励7的SkeletonGraphic组件
            skeletonGraphic07 = go.transform.Find("Image/Image/jiangli7/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic07);  // 添加到列表中
            // 取到奖励8的SkeletonGraphic组件
            skeletonGraphic08 = go.transform.Find("Image/Image/jiangli8/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic08);  // 添加到列表中
            // 取到奖励9的SkeletonGraphic组件
            skeletonGraphic09 = go.transform.Find("Image/Image/jiangli9/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic09);  // 添加到列表中
            // 取到奖励10的SkeletonGraphic组件
            skeletonGraphic10 = go.transform.Find("Image/Image/jiangli10/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic10);  // 添加到列表中
            // 取到奖励11的SkeletonGraphic组件
            skeletonGraphic11 = go.transform.Find("Image/Image/jiangli11/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic11);  // 添加到列表中
            // 取到奖励12的SkeletonGraphic组件
            skeletonGraphic12 = go.transform.Find("Image/Image/jiangli12/Ani").GetComponent<SkeletonGraphic>();
            skeletonGraphicList.Add(skeletonGraphic12);  // 添加到列表中

            startLotteryBtn = go.transform.Find("Image/GetAll").GetComponent<Button>();  // 取得按钮组件

            delayController = go.AddComponent<DelayController>();  // 添加延时控制器组件

            closeBtn = go.transform.Find("Image/CloseButton").GetComponent<Button>();  // 取得关闭按钮上的按钮组件

            priceTxt = go.transform.Find("Image/GetAll/Text").GetComponent<Text>();
            // 请求抽奖次数 自动刷新抽奖次数 对应ShowLotteryResult
            CommandHandle.Send(Proto.ServerMethod.ShowLottery, null);
            playAdBtn = go.transform.FindChild("Text/Button").GetComponent<Button>();
            playAdBtn.onClick.AddListener(PlayAd);
        }

        // 播放广告
        private void PlayAd()
        {
            //// 测试用
            //CommandHandle.Send(Proto.ServerMethod.AdvertLottery, null);

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
                                Debug.Log("发放奖励");
                                CommandHandle.Send(Proto.ServerMethod.AdvertLottery, null);
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
                                Debug.Log("发放奖励");
                                CommandHandle.Send(Proto.ServerMethod.AdvertLottery, null);
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

        // 第五部分
        // 刷新面板
        public override void Refresh()
        {
            // 打开或刷新抽奖面板时恢复无动画的空白状态
            // 把12个奖励的 Play 动画以100倍速度播放一次
            FastPlay();

            CommandHandle.Send(Proto.ServerMethod.ShowLottery, null);  // 请求抽奖次数 自动刷新抽奖次数

            base.Refresh();
        }

        // 第六部分
        // 设置信息条
        public override void SetInfoBar()
        {

        }

        // 第七部分
        // 重写关闭按钮的Close方法
        public override void Close(bool isCloseAllMode = false)
        {
            // 关闭面板时清除12个奖励上的动画
            ClearAllAnimation();

            // 启用开始抽奖按钮
            startLotteryBtn.enabled = true;
            //// 启用播放广告按钮
            //playAdBtn.enabled = true;

            base.Close(isCloseAllMode);  // 原本存在的方法
        }

        // 第八部分
        // 打开或刷新抽奖面板时恢复无动画的空白状态
        // 把12个奖励的 Play 动画以20倍速度播放一次
        private void FastPlay()
        {
            for (int i = 0; i < skeletonGraphicList.Count; i++)
            {
                skeletonGraphicList[i].AnimationState.TimeScale = 20f;  // 需要在点击开始抽奖按钮时修改成1f
                skeletonGraphicList[i].AnimationState.SetAnimation(0, "Play", false);
            }
        }

        // 发送抽奖命令
        private void SendLotteryCommand()
        {
            // 发送抽奖命令给服务器
            CommandHandle.Send(Proto.ServerMethod.Lottery, null);
        }

        // 第九部分
        // 开始抽奖
        public void StartLottery(Proto.AwardSpecData awardSpecData)
        {
            startLotteryBtn.enabled = false;  // 禁用开始按钮组件 在抽奖动画播放完毕和点击关闭按钮时重新启用
            closeBtn.enabled = false;  // 禁用关闭按钮组件，在获得奖励弹窗弹出时重新启用
            //playAdBtn.enabled = false;  // 禁用播放广告按钮组件 在抽奖动画播放完毕和点击关闭按钮时重新启用

            int i = 0;
            int randomNumber = UnityEngine.Random.Range(1, 101);  // 左闭右开，取个1到100之间的随机数  
            int prizeNumber = 0;
            string awardTip = "";

            // 抽到第1个奖品的概率是百分之五
            if (randomNumber >= 1 && randomNumber <= 5)
            {
                prizeNumber = 1;
            }
            // 抽到第2个奖品的概率是百分之五
            if (randomNumber >= 6 && randomNumber <= 10)
            {
                prizeNumber = 2;
            }
            // 抽到第3个奖品的概率是百分之五
            if (randomNumber >= 11 && randomNumber <= 15)
            {
                prizeNumber = 3;
            }
            // 抽到第4个奖品的概率是百分之五
            if (randomNumber >= 16 && randomNumber <= 20)
            {
                prizeNumber = 4;
            }
            // 抽到第5个奖品的概率是百分之五
            if (randomNumber >= 21 && randomNumber <= 25)
            {
                prizeNumber = 5;
            }
            // 抽到第6个奖品的概率是百分之五
            if (randomNumber >= 26 && randomNumber <= 30)
            {
                prizeNumber = 6;
            }
            // 抽到第7个奖品的概率是百分之五
            if (randomNumber >= 31 && randomNumber <= 35)
            {
                prizeNumber = 7;
            }
            // 抽到第8个奖品的概率是百分之五
            if (randomNumber >= 36 && randomNumber <= 40)
            {
                prizeNumber = 8;
            }
            // 抽到第9个奖品的概率是百分之5
            if (randomNumber >= 41 && randomNumber <= 45)
            {
                prizeNumber = 9;
            }
            // 抽到第10个奖品的概率是百分之五
            if (randomNumber >= 46 && randomNumber <= 50)
            {
                prizeNumber = 10;
            }
            // 抽到第11个奖品的概率是百分之五
            if (randomNumber >= 51 && randomNumber <= 55)
            {
                prizeNumber = 11;
            }
            // 抽到第12个奖品的概率是百分之45
            if (randomNumber >= 56 && randomNumber <= 100)
            {
                prizeNumber = 12;
            }

            switch (awardSpecData.Type)
            {
                case 0:
                    {
                        if (awardSpecData.Name == "Coin")  // 银鱼干 1 4 7 9 10
                        {
                            if (awardSpecData.Value == 50)
                            {
                                int index = UnityEngine.Random.Range(1, 3);
                                if (index == 1)
                                {
                                    prizeNumber = 1;
                                    awardTip = "恭喜获得银鱼干X50";
                                }
                                else
                                {
                                    prizeNumber = 9;
                                    awardTip = "恭喜获得银鱼干X50";
                                }
                            }
                            else if (awardSpecData.Value == 200)
                            {
                                prizeNumber = 4;
                                awardTip = "恭喜获得银鱼干X200";
                            }
                            else if (awardSpecData.Value == 100)
                            {
                                prizeNumber = 7;
                                awardTip = "恭喜获得银鱼干X100";
                            }
                            else
                            {
                                prizeNumber = 10;
                                awardTip = "恭喜获得银鱼干X300";
                            }
                        }
                        else  // 金鱼干 3 6 12
                        {
                            if (100 == awardSpecData.Value)
                            {
                                prizeNumber = 3;
                                awardTip = "恭喜获得金鱼干X100";
                            }
                            else if (20 == awardSpecData.Value)
                            {
                                prizeNumber = 6;
                                awardTip = "恭喜获得金鱼干X20";
                            }
                            else
                            {
                                prizeNumber = 12;
                                awardTip = "恭喜获得金鱼干X50";
                            }
                        }

                        break;
                    }
                case 2002:  // 高级猫粮 2 8
                    {
                        if (1 == awardSpecData.Value)
                        {
                            prizeNumber = 2;
                            awardTip = "恭喜获得高级猫粮X1";
                        }
                        else
                        {
                            prizeNumber = 8;
                            awardTip = "恭喜获得高级猫粮X2";
                        }

                        break;
                    }
                case 2004:  // 营养猫饭 11
                    {
                        prizeNumber = 11;
                        awardTip = "恭喜获得营养猫饭X1";
                        break;
                    }
                default:  // 大马哈鱼 5
                    {
                        prizeNumber = 5;
                        awardTip = "恭喜获得大马哈鱼X1";
                        break;
                    }
            }
            //转第一圈
            // skeletonGraphicList.Count就是12，这样写感觉更高端
            for (; i < skeletonGraphicList.Count; i++)
            {
                skeletonGraphicList[i].AnimationState.TimeScale = 1f;
                TrackEntry trackEntry1 = skeletonGraphicList[i].AnimationState.SetAnimation(0, "Play", false);
                TrackEntry trackEntry2 = skeletonGraphicList[i].AnimationState.AddAnimation(0, "Play", false, 0.1f + i * 0.1f);
                //// 在动画开始播放的时候播放音效
                //// 这个不知怎么回事不播放，先放一放
                //// 转第二圈时
                //trackEntry1.Start += delegate (Spine.AnimationState state, int trackIndex)
                //{
                //    SoundUtil.PlaySound("Lottery1");
                //};
                //// 在动画开始播放的时候播放音效
                //trackEntry2.Start += delegate (Spine.AnimationState state, int trackIndex)
                //{
                //    SoundUtil.PlaySound("Lottery1");
                //};
            }

            // 转第二圈
            i = i - 12;
            for (; i < skeletonGraphicList.Count; i++)
            {
                TrackEntry trackEntry = skeletonGraphicList[i].AnimationState.AddAnimation(0, "Play", false,
                    0.1f + i * 0.1f + 11 * 0.1f);
                //// 在动画开始播放的时候播放音效
                //trackEntry.Start += delegate (Spine.AnimationState state, int trackIndex)
                //{
                //    SoundUtil.PlaySound("Lottery1");
                //};
            }

            // 转第三圈
            i = i - 12;
            for (; i < skeletonGraphicList.Count; i++)
            {
                // 最后加的0.1秒是为了第2圈第12个奖励和第3圈第1个奖励时间重叠而加的
                TrackEntry trackEntry = skeletonGraphicList[i].AnimationState.AddAnimation(0, "Play", false,
                    0.1f + i * 0.1f + 11 * 0.1f + 11 * 0.1f + 0.1f);
                //// 在动画开始播放的时候播放音效
                //trackEntry.Start += delegate (Spine.AnimationState state, int trackIndex)
                //{
                //    SoundUtil.PlaySound("Lottery1");
                //};
            }

            // 第4圈 半圈或一圈
            i = i - 12;
            for (; i < prizeNumber; i++)
            {
                // 最后加的两个0.1秒是为了第3圈第12个奖励和第4圈第1个奖励时间重叠而加的
                TrackEntry trackEntry1 = skeletonGraphicList[i].AnimationState.AddAnimation(0, "Play", false,
                    0.1f + i * 0.1f + 11 * 0.1f + 11 * 0.1f + 11 * 0.1f + 0.1f + 0.1f);
                //// 在动画开始播放的时候播放音效
                //trackEntry1.Start += delegate (Spine.AnimationState state, int trackIndex)
                //{
                //    SoundUtil.PlaySound("Lottery1");
                //};
                if (i + 1 == prizeNumber)
                {
                    TrackEntry trackEntry2 = skeletonGraphicList[i].AnimationState.AddAnimation(0, "Play", true, 0);
                    //// 在最后获得的奖励的动画开始播放时播放得到奖励的音效
                    //trackEntry2.Start += delegate (Spine.AnimationState state, int trackIndex)
                    //{
                    //    SoundUtil.PlaySound("Lottery2");
                    //};

                    // 4.6秒后出现单确定弹窗
                    delayController.DelayInvoke(() =>
                    {
                        // 如果奖励是银鱼干
                        if (0 == awardSpecData.Type && awardSpecData.Name == "Coin")
                        {
                            if (PanelManager.infoBar != null)
                            {
                                PanelManager.infoBar.UpdateFishDisplay(1,awardSpecData.Value);
                            }
                        }
                        // 当抽到的奖励是金鱼干
                        else if (0 == awardSpecData.Type && awardSpecData.Name == "Dollar")
                        {
                            if (PanelManager.infoBar != null)
                            {
                                PanelManager.infoBar.UpdateFishDisplay(2, awardSpecData.Value);
                            }
                        }
                        PanelManager.ShowTipString(awardTip, EnumConfig.PropPopupPanelBtnModde.OneBtn, middleCallback: () =>
                        {
                            ClearAllAnimation();  // 清除所有动画
                            FastPlay();  // 恢复奖励的空白状态
                        });
                        startLotteryBtn.enabled = true;  // 启用开始抽奖按钮
                        closeBtn.enabled = true;  // 启用关闭按钮                      
                        //playAdBtn.enabled = true;  // 启用播放广告按钮
                        CommandHandle.Send(Proto.ServerMethod.ShowLottery, null);  // 请求抽奖次数 自动刷新抽奖次数
                    }, 7.2f + i * 0.46f + 1f);  // 最后加的1秒是用来播放获奖音效
                }
            }
        }

        // 第十部分 关闭面板时
        // 关闭12个奖励上的动画
        private void ClearAllAnimation()
        {
            for (int i = 0; i < skeletonGraphicList.Count; i++)
            {
                skeletonGraphicList[i].AnimationState.ClearTracks();  // 清除所有动画
            }
        }

        // 本次抽奖的花费提示
        public void costTip(ShowLotteryData data)
        {
            if (data.AdvertTimes >= 5)
            {
                playAdBtn.gameObject.SetActive(false);
            }
            int times = data.BuyTimes;
            if (data.LeftTimes > 0)
            {
                silverNumString = "免费抽奖X" + data.LeftTimes;
            }
            else if (0 == times)
            {
                silverNumString = "100银鱼干抽奖";
            }
            else if (1 == times)
            {
                silverNumString = "180银鱼干抽奖";
            }
            else if (2 == times)
            {
                silverNumString = "350银鱼干抽奖";
            }
            else if (3 == times)
            {
                silverNumString = "680银鱼干抽奖";
            }
            else
            {
                silverNumString = "1200银鱼干抽奖";
            }

            priceTxt.text = silverNumString;  // 本次抽奖花费提示
        }
    }
}
