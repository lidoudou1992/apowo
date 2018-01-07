using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class AppConfig : MonoBehaviour
    {
        /// <summary>
        /// 资料片.大版本.平台版号(用于同一个版本多次打包).大版本.热更版本
        /// 1.2.0.2.1
        /// </summary>
        public static string VERSION_PREFIX = "1.2.0.";
        public static int MAJOR_VERSION = 4;
        public static int MINOR_VERSION = 1;

        public static bool USE_HOT_FIX = true;

        /// <summary>
        /// 默认的渠道
        /// </summary>
        //public static string CHANNEL = "cat";
        //public static string CHANNEL = "4399";
        //public static string CHANNEL = "7k7k";
        //public static string CHANNEL = "meitu";
        //public static string CHANNEL = "taptap";
        //public static string CHANNEL = "cathouse";
        //public static string CHANNEL = "7k7kcat";
        //public static string CHANNEL = "77";
        //public static string CHANNEL = "kk";
        public static string CHANNEL = "iOS";

        /// <summary>
        /// 分享的所有平台
        /// 当点击分享按钮时会弹出一个带有各个平台ICON的面板
        /// 面板上的icon便是通过这个数组来控制的
        /// </summary>
        //public static Platform[] SOCIAL_PLATFORMS = { Platform.WEIXIN, Platform.WEIXIN_CIRCLE, Platform.QQ, Platform.QZONE, Platform.SINA };
        //public static Platform[] SOCIAL_PLATFORMS = {Platform.QQ, Platform.QZONE };

        /// <summary>
        /// 线上版本必须设为 false
        /// </summary>
        public static bool USE_DEVELOP_MODE = false;
        public enum MAINTAIN_TYPE
        {
            //玩家正常获取热更
            Null = 0,
            ////内部测试热更新内容，但玩家拉不到，但是可以正常进游戏玩
            //TestHotFix = 1,
            //停服更新，所有人不能进游戏，即使服务器开了
            CloseServer = 1
        }
    }
}
