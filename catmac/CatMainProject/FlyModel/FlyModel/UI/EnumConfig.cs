using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyModel.UI
{
    public class EnumConfig
    {
        public enum BagItemType
        {
            Null = -1,
            Food = 0,//猫粮之类的
            Item = 1,
            Furni = 2,
        }

        public enum ShopItemType
        {
            Food = 0,
            Item = 1,
            Currency = 2,
            Toy = 3,
        }

        public enum ToySizeType
        {
            Null = 0,
            Small = 1,
            Large = 2,
        }

        public enum PropPopupPanelBtnModde
        {
            OneBtn = 0,
            TwoBtb = 1,
        }

        public enum InfoBarBtnMode
        {
            Menu = 0,
            Close = 1,
        }

        /// <summary>
        /// 场景点 大小点
        /// </summary>
        public enum SubPointType
        {
            small = 0,
            large = 1,
        }

        public enum LoadingType
        {
            Scene=0,
            Toy = 1,
            Cat = 2,
            Sound = 3,
        }

        public enum HandbookMode
        {
            Cat = 0,
            Gift = 1,
        }

        public enum HandbookCatState
        {
            Unknow=0,
            Find_Offline = 1,
            Find_Online = 2,
        }

        public enum InteractivePointDistibution
        {
            In = 0,
            Out = 1,
        }

        public enum SystemMuneBtn
        {
            Null = -1,
            Shop=0,
            Bag = 1,
            PicBook = 2,
            Photo = 3,
            Setting = 4,
            Treasure = 5,
            Scene = 6
        }

        public enum PictureMode
        {
            Null = -1,
            Cat = 0,
            Screen = 1
        }

        public enum LoginType
        {
            LoginPanel=0,
            UDID = 1
        }

        public enum GuideActionType
        {
            Say = 0,
            ShowPanel = 1,
            RecoredPhase = 2,
            ShowGesture = 3,
            SelectProp =4,
            ShowGestureIScene = 5,
            WaitForServer = 6,
            SendMsg = 7
        }
        public static Dictionary<string, int> GuideActionTypeDic = new Dictionary<string, int>();


       
        public EnumConfig()
        {
            GuideActionTypeDic.Add("Say", 0);
            GuideActionTypeDic.Add("ShowPanel", 1);
            GuideActionTypeDic.Add("RecoredPhase", 2);
            GuideActionTypeDic.Add("ShowGesture", 3);
            GuideActionTypeDic.Add("SelectProp", 4);
            GuideActionTypeDic.Add("ShowGestureIScene", 5);
            GuideActionTypeDic.Add("WaitForServer", 6);
            GuideActionTypeDic.Add("SendMsg", 7);
        }

        public enum InfoLayoutMode
        {
            Head = 0,
            Currency = 1
        }

        public enum SignState
        {
            signed = 0,
            canSign = 1,
            waitSign = 2
        }
    }
}
