using Assets.Scripts;
using FlyModel.Proto;
using FlyModel.UI.Component;
using FlyModel.UI.Panel;
using FlyModel.UI.Panel.LoginPanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening;
using GSDKUnityLib;
using GSDKUnityLib.Account.Login;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI
{
    public class LoginPanel : PanelBase
    {
        public override string BundleName
        {
            get
            {
                return "loginpanel";
            }
        }
        public override string AssetName
        {
            get
            {
                return "LoginPanel";
            }
        }

        public override bool IsRoot
        {
            get
            {
                return true;
            }
        }

        //线上服
        //private string thisIP = "222.73.208.80";
        //private int thisPort = 40000;

        // 点击进入游戏按钮才会使用这个IP和端口，
        // 目前是不用进入游戏按钮的
        //测试服
        private string thisIP = "222.73.208.80";
        private int thisPort = 40001;

        //平台方测试服
        //private string thisIP = "222.73.208.80";
        //private int thisPort = 40002;

        private InputField nameTF;
        private InputField passwordTF;

        private Text versionTF;

        private Text UDIDTF;

        // 游客登录采用直接登陆的方式，取设备号
        private Button visitorBtn;
        private LoginUtil loginUtil;  // 要new的

        // 账号画面
        //private InputField inputFieldAccount;  // 账号
        private InputField iFAccount;  // 账号
        //private InputField inputFieldPassword;  // 密码
        private InputField iFPassword;  // 密码
        private Button buttonRegister;  // 注册按钮
        private Button btnLogin;  // 登录按钮
        private GameObject gameObjectDialogAccount;  // 账号对话框
        private GameObject gameObjectDialogAccountRegister;  // 账号注册对话框

        // 账号注册画面
        //private Button buttonCancel;  // 取消按钮
        //private Button buttonOK;  // “注册并登录”按钮
        private InputField inputFieldRegisterAccount;  // 账号输入字段
        private InputField inputFieldRegisterPassword;  // 密码字段
        private InputField inputFieldRegisterPasswordConfirm;  // “确认密码”字段
        private GameObject goTooltip;  // 提示框
        //private Button btnTooltip;  // 提示框上的按钮

        protected override void Initialize(GameObject go)
        {
            new SoundButton(go.transform.Find("RegistBtn").gameObject, () =>
            {
                PanelManager.AlertPanel.Show(() => {
                    PropPopupModeStruct modeStruct = new PropPopupModeStruct();

                    modeStruct.Mode = EnumConfig.PropPopupPanelBtnModde.TwoBtb;

                    modeStruct.LeftBtnString = "退出";
                    modeStruct.LeftCallback = () => {
                        PanelManager.alertPanel.Close();

                        Application.Quit();
                    };

                    modeStruct.RightBtnString = "取消";
                    modeStruct.RightCallback = () => {
                        PanelManager.alertPanel.Close();

                    };

                    PanelManager.alertPanel.SetData("是否退出游戏？", modeStruct);
                });
            });

            new SoundButton(go.transform.FindChild("StartGame").gameObject, startConnect);

            nameTF = go.transform.Find("Accound").GetComponent<InputField>();
            passwordTF = go.transform.Find("PassWord").GetComponent<InputField>();

            versionTF = go.transform.Find("Version").GetComponent<Text>();
            versionTF.text = string.Format("当前版本：{0}.{1}", AppConfig.MAJOR_VERSION, AppConfig.MINOR_VERSION);

            //if (string.IsNullOrEmpty(PlayerPrefs.GetString("account"))==false)
            //{
            //    nameTF.text = PlayerPrefs.GetString("account");
            //}

            //if (string.IsNullOrEmpty(PlayerPrefs.GetString("password")) == false)
            //{
            //    passwordTF.text = PlayerPrefs.GetString("password");
            //}

            UDIDTF = go.transform.Find("UDID").GetComponent<Text>();
            //UDIDTF.text = GameMain.UDID;

            // 点击游客登陆（应该是登录）其实是通过取设备号的方式登录
            visitorBtn = go.transform.Find("youk").GetComponent<Button>();
            // 直接登录的入口，换成账号登录就注释掉了
            //loginUtil = new LoginUtil();  // 之前没有这一步报错，找了好久才发现
            //visitorBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            //{
            //    loginUtil.StartConnect();
            //}));
            // 使用渠道账户登录
            new SoundButton(go.transform.Find("youk").gameObject, () =>
            {
                Debug.Log("GSDK start login");
                GSDK.Instance.StartLoginAsnyc(loginResultInfo => {
                    if (loginResultInfo.Status == ELoginResultStatus.Succeed)
                    {
                        String id = loginResultInfo.AccountInfo.UID;
                        //String pass = loginResultInfo.AccountInfo.Password;
                        string pass = loginResultInfo.AccountInfo.ExtraJson["Token"].Value;
                        Debug.Log("账号：" + id);
                        Debug.Log("密码：" + pass);
                        // 给服务器发送登录命令
                        // 开始连接
                        loginUtil = new LoginUtil();
                        // 设置登录的账号和密码，改用渠道发来的账号密码
                        loginUtil.account = id;
                        loginUtil.password = pass;
                        loginUtil.isRegisterOrLogin = true;
                        loginUtil.timestamp = loginResultInfo.AccountInfo.ExtraJson["ServerTime"].Value;
                        // 平台登录也就是渠道登录类型是1
                        loginUtil.loginType = 1;
                        // 使用登录工具登录
                        loginUtil.StartConnect();
                    }
                    else if (loginResultInfo.Status == ELoginResultStatus.Cancelled)
                    {

                    }
                    else
                    {
                        // loginResultInfo.ErrorMsg   
                        // loginResultInfo.InternalErrorCode
                        // loginResultInfo.Status
                    }
                });
            });

            // 账号体系
            // 账号画面
            gameObjectDialogAccount = go.transform.FindChild("DialogAccount").gameObject;
            //inputFieldAccount = go.transform.FindChild("DialogAccount/Account").GetComponent<InputField>();
            iFAccount = go.transform.FindChild("DialogAccount/Account").GetComponent<InputField>();
            //inputFieldPassword = go.transform.FindChild("DialogAccount/Password").GetComponent<InputField>();
            iFPassword = go.transform.FindChild("DialogAccount/Password").GetComponent<InputField>();
            buttonRegister = go.transform.FindChild("DialogAccount/ButtonRegister").GetComponent<Button>();
            buttonRegister.onClick.AddListener(ClickButtonRegister);
            btnLogin = go.transform.FindChild("DialogAccount/ButtonLogin").GetComponent<Button>();
            btnLogin.onClick.AddListener(ClickBtnLogin);
            // 账号注册画面
            gameObjectDialogAccountRegister = go.transform.FindChild("DialogAccountRegister").gameObject;
            //buttonCancel = go.transform.FindChild("DialogAccountRegister/ButtonCancel").GetComponent<Button>();
            //buttonCancel.onClick.AddListener(ClickButtonRegisterCancel);
            new SoundButton(go.transform.FindChild("DialogAccountRegister/ButtonCancel").gameObject,
                ClickButtonRegisterCancel);
            //buttonOK = go.transform.FindChild("DialogAccountRegister/ButtonOK").GetComponent<Button>();
            //buttonOK.onClick.AddListener(ClickButtonRegisterOK);
            new SoundButton(go.transform.FindChild("DialogAccountRegister/ButtonOK").gameObject, ClickButtonRegisterOK);
            inputFieldRegisterAccount =
                go.transform.FindChild("DialogAccountRegister/Account/InputField").GetComponent<InputField>();
            inputFieldRegisterPassword =
                go.transform.FindChild("DialogAccountRegister/Password/InputField").GetComponent<InputField>();
            inputFieldRegisterPasswordConfirm =
                go.transform.FindChild("DialogAccountRegister/PasswordConfirm/InputField").GetComponent<InputField>();
            // 提示框
            goTooltip = go.transform.FindChild("Tooltip").gameObject;
            //btnTooltip = go.FindChildByName("Tooltip").GetComponent<Button>();
            //btnTooltip.onClick.AddListener(HideTooltip);
            new SoundButton(go.FindChildByName("Tooltip").gameObject, HideTooltip);
            // 账号和密码在“登录”和“注册并登录”时设置
            // 当账号和密码在本地已经有保存时
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Account")))
            {
                iFAccount.text = PlayerPrefs.GetString("Account");
            }
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Password")))
            {
                iFPassword.text = PlayerPrefs.GetString("Password");
            }
        }

        // 当点击账号界面的登录按钮时
        private void ClickBtnLogin()
        {
            // 1 判断账号和密码的合法性
            // 长度检测
            if (iFAccount.text.Length >= 4 && iFAccount.text.Length <= 12)
            {
                if (iFPassword.text.Length >= 4 && iFPassword.text.Length <= 12)
                {
                    // 检测账号和密码是不是都只由数字和字母组成
                    if (IsNumAndEnCh(iFAccount.text))
                    {
                        if (IsNumAndEnCh(iFPassword.text))
                        {
                            // 给服务器发送登录命令
                            // 开始连接
                            loginUtil = new LoginUtil();
                            // 设置登录的账号和密码
                            loginUtil.account = iFAccount.text;
                            loginUtil.password = iFPassword.text;
                            loginUtil.isRegisterOrLogin = true;
                            loginUtil.loginType = 0;
                            // 使用登录工具登录
                            loginUtil.StartConnect();

                            // 在本地写入账号和密码
                            // 如果本地已写入，那么会覆盖
                            PlayerPrefs.SetString("Account", iFAccount.text);
                            PlayerPrefs.SetString("Password", iFPassword.text);
                            PlayerPrefs.Save();  // 保存写入
                        }
                        else
                        {
                            // 显示密码只能由数字和字母组成的提示框
                            goTooltip.FindChildByName("Image/Text").GetComponent<Text>().text = "请输入由数字和字母组成的密码";
                            goTooltip.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                            goTooltip.SetActive(true);
                            Tweener tweener = goTooltip.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
                            tweener.SetEase(Ease.OutBack);
                        }
                    }
                    else
                    {
                        // 显示账号只能由数字和字母组成的提示框
                        goTooltip.FindChildByName("Image/Text").GetComponent<Text>().text = "请输入由数字和字母组成的账号";
                        goTooltip.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                        goTooltip.SetActive(true);
                        Tweener tweener = goTooltip.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
                        tweener.SetEase(Ease.OutBack);
                    }
                }
                else
                {
                    // 显示密码长度不合法的提示框
                    goTooltip.FindChildByName("Image/Text").GetComponent<Text>().text = "请输入4到12个字符的密码";
                    goTooltip.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                    goTooltip.SetActive(true);
                    Tweener tweener = goTooltip.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
                    tweener.SetEase(Ease.OutBack);
                }
            }
            else
            {
                // 显示账号长度不合法的提示框
                goTooltip.FindChildByName("Image/Text").GetComponent<Text>().text = "请输入4到12个字符的账号";
                goTooltip.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                goTooltip.SetActive(true);
                Tweener tweener = goTooltip.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
                tweener.SetEase(Ease.OutBack);
            }
        }

        // 当点击提示框上的按钮时
        private void HideTooltip()
        {
            // 隐藏提示框
            Tweener tweener = goTooltip.transform.DOScale(new Vector3(0.1f, 0.1f, 1f), 0.25f);
            tweener.SetEase(Ease.InBack);
            tweener.OnComplete(() =>
            {
                goTooltip.SetActive(false);
                goTooltip.transform.localScale = new Vector3(1f, 1f, 1f);
            });
        }

        // 当点击账号注册画面的“注册并登录”按钮时
        private void ClickButtonRegisterOK()
        {
            // 1 判断账号密码和确认密码的合法性
            // 判断账号的合法性
            if (inputFieldRegisterAccount.text.Length >= 4 && inputFieldRegisterAccount.text.Length <= 12)
            {
                // 判断输入的账号是否只包含数字和字母
                if (IsNumAndEnCh(inputFieldRegisterAccount.text))
                {
                    // 账号合法  
                    // 判断密码和确认密码是否一样
                    if (inputFieldRegisterPassword.text == inputFieldRegisterPasswordConfirm.text)
                    {
                        // 密码长度
                        if (inputFieldRegisterPassword.text.Length >= 4 && inputFieldRegisterPassword.text.Length <= 12)
                        {
                            // 判断输入的密码是否只包含数字和字母
                            if (IsNumAndEnCh(inputFieldRegisterPassword.text))
                            {
                                // 开始连接
                                //startConnect();  // 貌似不能这样直接连接
                                // 需要用登录工具LoginUtil.cs
                                // 因为这个工具的登录里多了个断开连接监视器
                                // 貌似断线重连要用到，
                                // 因为韩旭先做的账号后取设备号登录，
                                // 所以就在取设备号的基础上修改吧

                                loginUtil = new LoginUtil();
                                // 设置登录的账号和密码
                                loginUtil.account = inputFieldRegisterAccount.text;
                                loginUtil.password = inputFieldRegisterPassword.text;
                                loginUtil.isRegisterOrLogin = true;
                                loginUtil.loginType = 2;
                                // 使用登录工具登录
                                loginUtil.StartConnect();

                                // 写入存在本地的账号和密码
                                PlayerPrefs.SetString("Account", inputFieldRegisterAccount.text);
                                PlayerPrefs.SetString("Password", inputFieldRegisterPassword.text);
                                PlayerPrefs.Save();  // 保存写入
                            }
                            else
                            {
                                // 显示密码应只包含数字和字母的提示框
                                goTooltip.FindChildByName("Image/Text").GetComponent<Text>().text = "请输入由数字和字母组成的密码";
                                goTooltip.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                                goTooltip.SetActive(true);
                                Tweener tweener = goTooltip.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
                                tweener.SetEase(Ease.OutBack);
                            }
                        }
                        else
                        {
                            // 显示密码长度不合法的提示框
                            goTooltip.FindChildByName("Image/Text").GetComponent<Text>().text = "请输入4到12个字符的密码";
                            goTooltip.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                            goTooltip.SetActive(true);
                            Tweener tweener = goTooltip.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
                            tweener.SetEase(Ease.OutBack);
                        }
                    }
                    else
                    {
                        // 显示密码和确认密码不一样的提示框
                        goTooltip.FindChildByName("Image/Text").GetComponent<Text>().text = "请输入一样的密码和确认密码";
                        goTooltip.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                        goTooltip.SetActive(true);
                        Tweener tweener = goTooltip.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
                        tweener.SetEase(Ease.OutBack);
                    }
                }
                else
                {
                    // 显示账号应只包含数字和字母的提示框
                    goTooltip.FindChildByName("Image/Text").GetComponent<Text>().text = "请输入由数字和字母组成的账号";
                    goTooltip.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                    goTooltip.SetActive(true);
                    Tweener tweener = goTooltip.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
                    tweener.SetEase(Ease.OutBack);
                }
            }
            else
            {
                // 显示账号长度不合法的提示框
                goTooltip.FindChildByName("Image/Text").GetComponent<Text>().text = "请输入4到12个字符的账号";
                goTooltip.transform.localScale = new Vector3(0.1f,0.1f,1f);
                goTooltip.SetActive(true);
                Tweener tweener = goTooltip.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
                tweener.SetEase(Ease.OutBack);
            }
        }

        /// <summary>  
        /// 判断输入的字符串是否只包含数字和英文字母  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsNumAndEnCh(string input)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        // 当点击账号注册画面的取消按钮时
        private void ClickButtonRegisterCancel()
        {
            // 1 隐藏账号注册画面
            Tweener tweener = gameObjectDialogAccountRegister.transform.DOScale(new Vector3(0.1f, 0.1f, 1f), 0.25f);
            tweener.SetEase(Ease.InBack);
            tweener.OnComplete(() =>
            {
                gameObjectDialogAccountRegister.SetActive(false);
                gameObjectDialogAccountRegister.transform.localScale = new Vector3(1f, 1f, 1f);

                // 2 显示账号画面
                gameObjectDialogAccount.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                gameObjectDialogAccount.SetActive(true);
                Tweener tweenerDisplay = gameObjectDialogAccount.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
                tweenerDisplay.SetEase(Ease.OutBack);
            });
        }

        // 当点击账号画面的注册按钮时
        private void ClickButtonRegister()
        {
            // 1 隐藏账号画面
            Tweener tweener = gameObjectDialogAccount.transform.DOScale(new Vector3(0.1f, 0.1f, 1f), 0.25f);
            tweener.SetEase(Ease.InBack);  // 设置缓冲类型
            tweener.OnComplete(() =>
            {
                gameObjectDialogAccount.SetActive(false);
                gameObjectDialogAccount.transform.localScale = new Vector3(1f, 1f, 1f);

                // 2 显示账号注册画面
                gameObjectDialogAccountRegister.transform.localScale = new Vector3(0.1f,0.1f,1f);
                gameObjectDialogAccountRegister.SetActive(true);
                Tweener tweenerDisplay = gameObjectDialogAccountRegister.transform.DOScale(new Vector3(1f, 1f, 1f),
                    0.25f);
                tweenerDisplay.SetEase(Ease.OutBack);                
            });      
        }

        private void startConnect()
        {
            if (GameApplication.Instance.SocketClient == null)
            {
                tryConnect();
            }
            // 直接这样账号登录
        }

        //// 尝试连接
        //// 韩旭版连接服务器
        //private void tryConnect()
        //{
        //    if (string.IsNullOrEmpty(nameTF.text) == false && string.IsNullOrEmpty(passwordTF.text) == false)
        //    {
        //        GameApplication.Instance.OnConnected = onConnected;
        //        GameApplication.Instance.OnDisconnected = onDisconnected;
        //        GameApplication.Instance.TryConnect(thisIP, thisPort);
        //        PlayerPrefs.SetString("account", nameTF.text);
        //        PlayerPrefs.SetString("password", passwordTF.text);
        //        //PlayerPrefs.SetString("serverName", serverText.text);
        //        //PlayerPrefs.SetString("serverIP", thisIP);
        //        //PlayerPrefs.SetInt("serverPort", thisPort);
        //    }
        //    else
        //    {
        //        Debug.Log("请输入账号");
        //    }
        //}

        // 尝试连接服务器
        private void tryConnect()
        {
            GameApplication.Instance.OnConnected = onConnected;
            GameApplication.Instance.OnDisconnected = onDisconnected;
            GameApplication.Instance.TryConnect(thisIP, thisPort);
            // 写入存在本地的账号和密码
            PlayerPrefs.SetString("Account", inputFieldRegisterAccount.text);
            PlayerPrefs.SetString("Password", inputFieldRegisterPassword.text);
            PlayerPrefs.Save();  // 保存写入
        }

        //// 韩旭版发送登录命令
        //private void onConnected()
        //{
        //    Debug.Log("连上服务器,准备登陆");
        //    CommandHandle.Send(ServerMethod.Login, new AccountData() { Name = nameTF.text, Password = "123456", LoginType = 0 });
        //}

        // 发送登录命令
        private void onConnected()
        {
            Debug.Log("连上服务器,准备登录");
            string platform = "";
            if (Application.platform == RuntimePlatform.Android)
            {
                platform = "Android";
            }
            else if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                platform = "iOS";
            }          
            CommandHandle.Send(ServerMethod.Login,new AccountData()
            {
                // 用户名，就是账号
                Name = inputFieldRegisterAccount.text,
                Password = inputFieldRegisterPassword.text,
                Device = platform,
                DeviceNumber = SystemInfo.deviceUniqueIdentifier,
                //Platform = "";  // 后端说平台先不设置
                LoginType = 2
            });
        }

        // 当连接断开时
        public void onDisconnected()
        {
            // 显示警告界面
            PanelManager.AlertPanel.Show(() =>
            {
                PropPopupModeStruct modeStruct = new PropPopupModeStruct();

                modeStruct.Mode = EnumConfig.PropPopupPanelBtnModde.TwoBtb;

                modeStruct.LeftBtnString = "重连";
                modeStruct.LeftCallback = () =>
                {
                    PanelManager.alertPanel.Close();

                    GameApplication.Instance.ReconnectingCallback();
                };

                modeStruct.RightBtnString = "退出";
                modeStruct.RightCallback = () =>
                {
                    // 关闭警告
                    PanelManager.alertPanel.Close();
                };

                PanelManager.alertPanel.SetData("网络断开，是否重连？", modeStruct);
            });
        }

        protected override void InitializeOver()
        {
            base.InitializeOver();
            //ResourceLoader.Instance.tr("pic", AssetName, (go) =>
            //{
            //    PanelPrefab = go;

            //    go.transform.SetParent(transform, false);
            //    TryInitializeBaseUI();
            //    Initialize(go);
            //    InitializeOver();
            //    loaded = true;
            //    if (ShowOnLoaded)
            //    {
            //        Show();
            //    }
            //});
        }

        //public override void OnCloseButtonClick()
        //{
        //    if (serverListRoot.gameObject.activeSelf)
        //    {
        //        CloseServerListRoot();
        //    }
        //}
    }

    public class ServerConfig
    {
        public ServerConfig()
        {

        }
        public string IP
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
