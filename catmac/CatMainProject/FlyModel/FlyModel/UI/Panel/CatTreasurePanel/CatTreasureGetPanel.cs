using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.CatTreasurePanel
{
    public class CatTreasureGetPanel : PanelBase
    {
        //public static CatGiftData gift;

        // 猫咪礼物队列
        public static List<CatGiftData> catGiftDataList = new List<CatGiftData>();


        // 找到面板资源
        public override string AssetName
        {
            get
            {
                return "catbabybegin";
            }
        }

        // 设 catTreasureRoot 为父物体
        public CatTreasureGetPanel(RectTransform parent) : base(parent)
        {
        }

        // 初始化面板
        protected override void Initialize(GameObject go)
        {

        }

        // 刷新面板(每次打开面板时自动调用)
        public override void Refresh()
        {
            base.Refresh();

            //// 调用定时器
            //TimeTick timeTick = new TimeTick();
            //timeTick.Update();

            //// 调用定时器
            //Timer timer = new Panel.CatTreasurePanel.Timer();
            //timer.Update();
        }

        // 隐掉信息条
        public override void SetInfoBar()
        {
        }

        // 重写关闭按钮点击方法
        public override void OnCloseButtonClick()
        {
            Close();

            //// 打开 CatTreasureDisplayPanel 面板
            //PanelManager.CatTreasureDisplayPanel.Show(()=> {
            //    PanelManager.catTreasureDisplayPanel.MyActive();
            //});

            //// 取消catTreasureDisplayPanel
            //PanelManager.catTreasureDisplayPanel.Conceal();

            //PanelManager.CatTreasureDisplayPanel.MyActive();

            //PanelManager.CatTreasureDisplayPanel.Show(()=> {
            //    PanelManager.catTreasureDisplayPanel.SetData(gift);
            //});

            // 如果猫咪礼物队列中有礼物则显示猫咪礼物
            if (catGiftDataList != null && catGiftDataList.Count != 0)
            {
                PanelManager.CatTreasureDisplayPanel.Show(() => {
                    PanelManager.catTreasureDisplayPanel.SetData(catGiftDataList[0]);                   
                });
            }
        }

        //// 得到 PicData 数据
        //public void GetPicData(CatGiftData catGiftData)
        //{
        //    gift = catGiftData;
        //}

        // 在猫咪礼物队列中增加一个礼物
        public void AddOneGift(CatGiftData catGiftData)
        {
            catGiftDataList.Add(catGiftData);
        }
    }
}
