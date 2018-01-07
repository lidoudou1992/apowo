using Assets.Scripts;
using FlyModel.Control;
using FlyModel.Proto;
using FlyModel.UI.Component;
using UnityEngine;

namespace FlyModel.UI.Panel.LoginPanel
{
    public class LoginUtil
    {
        ////private string thisIP = "login.cat.apowogame.com";  // 用域名去登陆，老域名
        ////private string thisIP = "login.cat1.apowogame.com"; // 新域名
        ////private string thisIP = "login.cat2.apowogame.com"; // 新域名
        ////private int thisPort = 40001;   // 内部测试服务器
        ////private int thisPort = 40000;   // 媒体测试服 苹果审核后映射到苹果线上服
        ////private string thisIP = "123.155.154.117";
        ////private int thisPort = 40002;             
        ////private int thisPort = 40005;   // 公测服务器-3区，正式上线用的，给4399，meitu之类的都是这个 安卓1区

        //////公测服务器-1区
        ////private string thisIP = "222.73.208.80";
        ////private int thisPort = 40000;

        ////测试服
        ////private string thisIP = "222.73.208.80";
        ////private int thisPort = 40001;

        ////王颖颖测试服
        ////private string thisIP = "222.73.208.80";
        ////private int thisPort = 40004;

        ////小旭
        ////private string thisIP = "77.100.10.29";
        ////private int thisPort = 40000;

        ////平台方(公测二服)
        ////private string thisIP = "222.73.208.80";
        ////private int thisPort = 40002;

        //// 1 安卓1区
        //private string thisIP = "login.cat.apowogame.com";
        //private int thisPort = 40005;

        //// 2 内部测试服务器
        //private string thisIP = "login.cat.apowogame.com";
        //private int thisPort = 40001;

        //// 3 媒体测试服务器 待确认
        ////private string thisIP = "login.cat2.apowogame.com";
        //private string thisIP = "114.141.172.205";
        //private int thisPort = 40000;

        //// 3 媒体测试服务器 待确认
        //private string thisIP = "login.cat3.apowogame.com";
        //private int thisPort = 40000;


        ////// 114 媒体测试服
        //// 115 苹果1区
        //private string thisIP = "login.cat3.apowogame.com";
        //private int thisPort = 40000;

        // 媒体测试服 苹果1区
        private string thisIP = "login.cat1.apowogame.com";
        private int thisPort = 40000;

        private DisConnectMonitor monitor;

        public bool isDisconnect;

        // 账号登录
        // 在登录画面LoginPanel.cs里设置
        public string account;
        public string password;
        // 是否是注册或登录，如果是那么建立新的连接，目的是刷新注册或登录用的用户名和密码
        // 如果不是那么重连
        public bool isRegisterOrLogin = false;
        // 登录类型
        public int loginType = 2;
        // 时间戳，只在渠道登录时使用
        public string timestamp;

        /// <summary>
        /// 开始连接
        /// </summary>
        public void StartConnect()
        {
            if (monitor == null)
            {
                monitor = new DisConnectMonitor();
                monitor.target = this;
                BehaviourManager.AddGameComponent(monitor);

            }

            if (GameApplication.Instance.SocketClient == null)
            {
                tryConnect();
            }
            // 当连接不为空，但是正在注册时
            else if (GameApplication.Instance.SocketClient != null && isRegisterOrLogin == true)
            {
                isRegisterOrLogin = false;
                // 关闭现有连接
                GameApplication.Instance.SocketClient.Close();
                // 新建连接
                tryConnect();
            }
            // 重连
            else
            {
                GameApplication.Instance.ReconnectingCallback();
            }
        }

        // 尝试登录
        // 账号登录
        private void tryConnect()
        {
            GameApplication.Instance.OnConnected = onConnected;
            GameApplication.Instance.OnDisconnected = onDisconnected;
            Debug.Log(thisIP + " " + thisPort);
            GameApplication.Instance.TryConnect(thisIP, thisPort); 
        }

        //// 取设备号登录
        //private void onConnected()
        //{
        //    Debug.Log("已连上服务器,准备登录");
        //    Debug.Log("设备号：" + GameMain.UDID);
        //    //Debug.Log(GameApplication.CHANNEL_MAP[AppConfig.CHANNEL]);
        //    // 发送登录命令
        //    CommandHandle.Send(ServerMethod.Login, new AccountData() { Name = GameMain.UDID, Password = "123456", LoginType = GameApplication.CHANNEL_MAP[AppConfig.CHANNEL] });
        //    //CommandHandle.Send(ServerMethod.Login, new AccountData() { Name = GameMain.UDID, Password = "123456", LoginType = 0 });
        //    //CommandHandle.Send(ServerMethod.Login, new AccountData() { Name = "9b12ecaa677c5b883baa32989e5591438ba29757", Password = "123456", LoginType = 0 });
        //}

        // 账号登录
        private void onConnected()
        {
            //Debug.Log("已连上服务器,准备登录");
            //Debug.Log("设备号：" + GameMain.UDID);
            ////Debug.Log(GameApplication.CHANNEL_MAP[AppConfig.CHANNEL]);
            //// 发送登录命令
            //CommandHandle.Send(ServerMethod.Login, new AccountData() { Name = GameMain.UDID, Password = "123456", LoginType = GameApplication.CHANNEL_MAP[AppConfig.CHANNEL] });
            ////CommandHandle.Send(ServerMethod.Login, new AccountData() { Name = GameMain.UDID, Password = "123456", LoginType = 0 });
            ////CommandHandle.Send(ServerMethod.Login, new AccountData() { Name = "9b12ecaa677c5b883baa32989e5591438ba29757", Password = "123456", LoginType = 0 });

            Debug.Log("连上服务器,准备登录");
            string platform = "";
            // 安卓真机
            if (Application.platform == RuntimePlatform.Android)
            {
                platform = "Android";
            }
            // 苹果真机，包括iPad吧。。
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                platform = "iOS";
            }
            // Win编辑器里
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                platform = "Android";
            }
            // Mac编辑器里，黑苹果也有效吧。。
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                platform = "iOS";
            }
            Debug.Log("名字" + account);
            Debug.Log("密码" + password);
            Debug.Log("运行平台，也就是设备" + platform);
            Debug.Log("设备号" + GameMain.UDID);
            Debug.Log("登录类型" + loginType);
            CommandHandle.Send(ServerMethod.Login, new AccountData()
            {
                // 用户名，就是账号
                Name = account,
                Password = password,
                Device = platform,
                //DeviceNumber = SystemInfo.deviceUniqueIdentifier,  // 报了取设备号只能在主线程里取的错误。。
                DeviceNumber = GameMain.UDID,
                //Platform = "";  // 后端说平台先不设置
                LoginType = loginType,
                CheckTime = timestamp
            });
        }

        // 当连接断开时
        public void onDisconnected()
        {
            Debug.Log("连接断开！");
            isDisconnect = true;
        }
    }
}
