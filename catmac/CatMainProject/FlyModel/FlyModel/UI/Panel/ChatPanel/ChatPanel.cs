using DG.Tweening;
using FlyModel.Model;
using FlyModel.Proto;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace FlyModel.UI.Panel.ChatPanel
{
    public class ChatPanel : PanelBase
    {
        // 1 组件声明和新建列表 按照创建顺序排列
        private GameObject cell;  // 子项
        private Transform contentTf;  // contentTransform
        private InputField inputField;  // 输入
        private Image tableView;  // 用来取得TableView的宽度
        private List<GameObject> cellInstanceList = new List<GameObject>();  // 用来限制聊天子项上限为100条的
        private RectTransform xianshiRT;  // 用来做打开和关闭动画的
        private Button buttonClose;  // 用来禁用和启用关闭按钮

        // 选择房间
        private GameObject selectGO;
        // 显示和选择房间的右上角标签
        private GameObject displayGO;
        // 聊天房间
        private GameObject room1;
        private GameObject room2;
        private GameObject room3;
        private GameObject room4;
        private GameObject room5;
        private GameObject room6;
        private GameObject room7;
        private GameObject room8;
        private List<GameObject> roomList = new List<GameObject>();

        //// 频道1到8的聊天内容子项
        //// 不需要，用之前的一个子项做模板就行
        //private GameObject cell1;
        //private GameObject cell2;
        //private GameObject cell3;
        //private GameObject cell4;
        //private GameObject cell5;
        //private GameObject cell6;
        //private GameObject cell7;
        //private GameObject cell8;
        //private List<GameObject> cellList = new List<GameObject>();

        // 房间Transform
        private Transform content1Tf;
        private Transform content2Tf;
        private Transform content3Tf;
        private Transform content4Tf;
        private Transform content5Tf;
        private Transform content6Tf;
        private Transform content7Tf;
        private Transform content8Tf;
        private List<Transform> contentList = new List<Transform>();

        // 房间聊天内容子项实例集合
        private List<GameObject> cell1InstanceList = new List<GameObject>();  // 用来限制聊天子项上限为100条的
        private List<GameObject> cell2InstanceList = new List<GameObject>();  // 用来限制聊天子项上限为100条的
        private List<GameObject> cell3InstanceList = new List<GameObject>();  // 用来限制聊天子项上限为100条的
        private List<GameObject> cell4InstanceList = new List<GameObject>();  // 用来限制聊天子项上限为100条的
        private List<GameObject> cell5InstanceList = new List<GameObject>();  // 用来限制聊天子项上限为100条的
        private List<GameObject> cell6InstanceList = new List<GameObject>();  // 用来限制聊天子项上限为100条的
        private List<GameObject> cell7InstanceList = new List<GameObject>();  // 用来限制聊天子项上限为100条的
        private List<GameObject> cell8InstanceList = new List<GameObject>();  // 用来限制聊天子项上限为100条的

        // 用来完成显示冷清、良好或热闹的功能
        private GameObject panel1Num1;
        private GameObject panel1Num2;
        private GameObject panel1Num3;
        private GameObject panel2Num1;
        private GameObject panel2Num2;
        private GameObject panel2Num3;
        private GameObject panel3Num1;
        private GameObject panel3Num2;
        private GameObject panel3Num3;
        private GameObject panel4Num1;
        private GameObject panel4Num2;
        private GameObject panel4Num3;
        private GameObject panel5Num1;
        private GameObject panel5Num2;
        private GameObject panel5Num3;
        private GameObject panel6Num1;
        private GameObject panel6Num2;
        private GameObject panel6Num3;
        private GameObject panel7Num1;
        private GameObject panel7Num2;
        private GameObject panel7Num3;
        private GameObject panel8Num1;
        private GameObject panel8Num2;
        private GameObject panel8Num3;

        private Toggle tab1Toggle;  // 世界频道的开关
        private int roomNum = 0;  // 房间号
        // 发送按钮，在选频道界面时禁用，进入某个频道或世界频道时启用
        private Button sendButton;
        // 控制标题的开关
        private GameObject tab1OpenGO;
        private GameObject tab1CloseGO;
        private GameObject tab2OpenGO;
        private GameObject tab2CloseGO;
        // 用来显示频道人数
        private Text txt1;
        private Text txt2;
        private Text txt3;
        private Text txt4;
        private Text txt5;
        private Text txt6;
        private Text txt7;
        private Text txt8;
        private List<Text> txtList = new List<Text>();

        // 2 找到预设体资源
        public override string AssetName
        {
            get
            {
                return "TalkingPanel";
            }
        }

        // 3 
        // 设 ChatPanelRoot 为父物体
        public ChatPanel(RectTransform parent) : base(parent)
        {
        }

        // 4
        // 初始化 主要是取得组件操作 顺序排序
        protected override void Initialize(GameObject go)
        {
            sendButton = go.transform.Find("xianshi/xianshi/Talk/Send").GetComponent<Button>();
            new SoundButton(go.transform.Find("xianshi/xianshi/Talk/Send").gameObject, () =>
             {
                 SendChatContent();
             });

            cell = go.transform.Find("xianshi/TableView/Content/Cell").gameObject;
            contentTf = go.transform.Find("xianshi/TableView/Content").GetComponent<Transform>();
            inputField = go.transform.Find("xianshi/xianshi/Talk/InputField").GetComponent<InputField>();
            inputField.onValueChanged.AddListener(ShieldSensitiveWord);  // 屏蔽敏感词
            tableView = go.transform.Find("xianshi/TableView").GetComponent<Image>();

            xianshiRT = go.transform.Find("xianshi").GetComponent<RectTransform>();
            // 打开动画
            xianshiRT.DOLocalMoveY(0, 0.8f);  // Bottom在0.8秒后变为0f

            // 点击关闭按钮
            new SoundButton(go.transform.Find("xianshi/xianshi/TabGroup/ButtonClose").gameObject, () =>
             {
                 ButtonCloseMethod();
             });
            buttonClose = go.transform.Find("xianshi/xianshi/TabGroup/ButtonClose").GetComponent<Button>();

            //button1 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button1").GetComponent<Button>();
            //button2 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button2").GetComponent<Button>();
            //button3 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button3").GetComponent<Button>();
            //button4 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button4").GetComponent<Button>();
            //button5 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button5").GetComponent<Button>();
            //button6 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button6").GetComponent<Button>();
            //button7 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button7").GetComponent<Button>();
            //button8 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button8").GetComponent<Button>();
            new SoundButton(go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button1").gameObject, () =>
            {
                // 打开房间1
                OpenRoom(1);
                roomNum = 1;
                // 发送进入频道的命令
                //CommandHandle.Send(ServerMethod.EnterSpeakChannel,
                //    new EnterChannelRequest() { Type = SpeakType.Channel, ChannelID = 1 });
                CommandHandle.Send(ServerMethod.RedirectSpeakChannel,
                    new IDMessage() { Id = 1 });

                // 启用发送按钮
                sendButton.enabled = true;
            });
            new SoundButton(go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button2").gameObject, () =>
            {
                // 打开房间2
                OpenRoom(2);
                roomNum = 2;
                //// 发送进入频道的命令
                //CommandHandle.Send(ServerMethod.EnterSpeakChannel,
                //    new EnterChannelRequest() { Type = SpeakType.Channel, ChannelID = 2 });
                CommandHandle.Send(ServerMethod.RedirectSpeakChannel,
                    new IDMessage() { Id = 2 });
                // 启用发送按钮
                sendButton.enabled = true;
            });
            new SoundButton(go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button3").gameObject, () =>
            {
                // 打开房间3
                OpenRoom(3);
                roomNum = 3;
                //// 发送进入频道的命令
                //CommandHandle.Send(ServerMethod.EnterSpeakChannel,
                //    new EnterChannelRequest() { Type = SpeakType.Channel, ChannelID = 3 });
                CommandHandle.Send(ServerMethod.RedirectSpeakChannel,
                    new IDMessage() { Id = 3 });
                // 启用发送按钮
                sendButton.enabled = true;
            });
            new SoundButton(go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button4").gameObject, () =>
            {
                // 打开房间4
                OpenRoom(4);
                roomNum = 4;
                //// 发送进入频道的命令
                //CommandHandle.Send(ServerMethod.EnterSpeakChannel,
                //    new EnterChannelRequest() { Type = SpeakType.Channel, ChannelID = 4 });
                CommandHandle.Send(ServerMethod.RedirectSpeakChannel,
                    new IDMessage() { Id = 4 });
                // 启用发送按钮
                sendButton.enabled = true;
            });
            new SoundButton(go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button5").gameObject, () =>
            {
                // 打开房间5
                OpenRoom(5);
                roomNum = 5;
                //// 发送进入频道的命令
                //CommandHandle.Send(ServerMethod.EnterSpeakChannel,
                //    new EnterChannelRequest() { Type = SpeakType.Channel, ChannelID = 5 });
                CommandHandle.Send(ServerMethod.RedirectSpeakChannel,
                    new IDMessage() { Id = 5 });
                // 启用发送按钮
                sendButton.enabled = true;
            });
            new SoundButton(go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button6").gameObject, () =>
            {
                // 打开房间6
                OpenRoom(6);
                roomNum = 6;
                //// 发送进入频道的命令
                //CommandHandle.Send(ServerMethod.EnterSpeakChannel,
                //    new EnterChannelRequest() { Type = SpeakType.Channel, ChannelID = 6 });
                CommandHandle.Send(ServerMethod.RedirectSpeakChannel,
                    new IDMessage() { Id = 6 });
                // 启用发送按钮
                sendButton.enabled = true;
            });
            new SoundButton(go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button7").gameObject, () =>
            {
                // 打开房间7
                OpenRoom(7);
                roomNum = 7;
                //// 发送进入频道的命令
                //CommandHandle.Send(ServerMethod.EnterSpeakChannel,
                //    new EnterChannelRequest() { Type = SpeakType.Channel, ChannelID = 7 });
                CommandHandle.Send(ServerMethod.RedirectSpeakChannel,
                    new IDMessage() { Id = 7 });
                // 启用发送按钮
                sendButton.enabled = true;
            });
            new SoundButton(go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button8").gameObject, () =>
            {
                // 打开房间8
                OpenRoom(8);
                roomNum = 8;
                //// 发送进入频道的命令
                //CommandHandle.Send(ServerMethod.EnterSpeakChannel,
                //    new EnterChannelRequest() { Type = SpeakType.Channel, ChannelID = 8 });
                CommandHandle.Send(ServerMethod.RedirectSpeakChannel,
                    new IDMessage() { Id = 8 });
                // 启用发送按钮
                sendButton.enabled = true;
            });

            selectGO = go.transform.Find("xianshi/ChatRoom/TableView2/Select").gameObject;
            displayGO = go.transform.Find("xianshi/ChatRoom/Button").gameObject;
            new SoundButton(go.transform.Find("xianshi/ChatRoom/Button").gameObject, () =>
            {
                // 打开选择房间界面
                OpenSelectInterface();
            });

            room1 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect1").gameObject;
            roomList.Add(room1);
            room2 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect2").gameObject;
            roomList.Add(room2);
            room3 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect3").gameObject;
            roomList.Add(room3);
            room4 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect4").gameObject;
            roomList.Add(room4);
            room5 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect5").gameObject;
            roomList.Add(room5);
            room6 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect6").gameObject;
            roomList.Add(room6);
            room7 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect7").gameObject;
            roomList.Add(room7);
            room8 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect8").gameObject;
            roomList.Add(room8);

            //cell1 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect1/Content1/Cell").gameObject;
            //cell2 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect2/Content2/Cell").gameObject;
            //cell3 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect3/Content3/Cell").gameObject;
            //cell4 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect4/Content4/Cell").gameObject;
            //cell5 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect5/Content5/Cell").gameObject;
            //cell6 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect6/Content6/Cell").gameObject;
            //cell7 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect7/Content7/Cell").gameObject;
            //cell8 = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect8/Content8/Cell").gameObject;

            content1Tf = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect1/Content1").GetComponent<Transform>();
            contentList.Add(content1Tf);
            content2Tf = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect2/Content2").GetComponent<Transform>();
            contentList.Add(content2Tf);
            content3Tf = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect3/Content3").GetComponent<Transform>();
            contentList.Add(content3Tf);
            content4Tf = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect4/Content4").GetComponent<Transform>();
            contentList.Add(content4Tf);
            content5Tf = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect5/Content5").GetComponent<Transform>();
            contentList.Add(content5Tf);
            content6Tf = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect6/Content6").GetComponent<Transform>();
            contentList.Add(content6Tf);
            content7Tf = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect7/Content7").GetComponent<Transform>();
            contentList.Add(content7Tf);
            content8Tf = go.transform.Find("xianshi/ChatRoom/TableView2/ScrollRect8/Content8").GetComponent<Transform>();
            contentList.Add(content8Tf);

            panel1Num1 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button1/Num1").gameObject;
            panel1Num2 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button1/Num2").gameObject;
            panel1Num3 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button1/Num3").gameObject;
            panel2Num1 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button2/Num1").gameObject;
            panel2Num2 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button2/Num2").gameObject;
            panel2Num3 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button2/Num3").gameObject;
            panel3Num1 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button3/Num1").gameObject;
            panel3Num2 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button3/Num2").gameObject;
            panel3Num3 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button3/Num3").gameObject;
            panel4Num1 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button4/Num1").gameObject;
            panel4Num2 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button4/Num2").gameObject;
            panel4Num3 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button4/Num3").gameObject;
            panel5Num1 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button5/Num1").gameObject;
            panel5Num2 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button5/Num2").gameObject;
            panel5Num3 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button5/Num3").gameObject;
            panel6Num1 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button6/Num1").gameObject;
            panel6Num2 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button6/Num2").gameObject;
            panel6Num3 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button6/Num3").gameObject;
            panel7Num1 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button7/Num1").gameObject;
            panel7Num2 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button7/Num2").gameObject;
            panel7Num3 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button7/Num3").gameObject;
            panel8Num1 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button8/Num1").gameObject;
            panel8Num2 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button8/Num2").gameObject;
            panel8Num3 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button8/Num3").gameObject;

            tab1Toggle = go.transform.Find("xianshi/xianshi/TabGroup/Tab1").GetComponent<Toggle>();
            // 添加监听事件，监听世界频道Toggle的开启或关闭
            tab1Toggle.onValueChanged.AddListener(EnterOrLeaveChannel);

            tab1OpenGO = go.transform.Find("xianshi/xianshi/TabGroup/Tab1/Open").gameObject;
            tab1CloseGO = go.transform.Find("xianshi/xianshi/TabGroup/Tab1/Close").gameObject;
            tab2OpenGO = go.transform.Find("xianshi/xianshi/TabGroup/Tab2/Open").gameObject;
            tab2CloseGO = go.transform.Find("xianshi/xianshi/TabGroup/Tab2/Colse").gameObject;

            txt1 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button1/Text3").GetComponent<Text>();
            txtList.Add(txt1);
            txt2 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button2/Text3").GetComponent<Text>();
            txtList.Add(txt2);
            txt3 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button3/Text3").GetComponent<Text>();
            txtList.Add(txt3);
            txt4 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button4/Text3").GetComponent<Text>();
            txtList.Add(txt4);
            txt5 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button5/Text3").GetComponent<Text>();
            txtList.Add(txt5);
            txt6 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button6/Text3").GetComponent<Text>();
            txtList.Add(txt6);
            txt7 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button7/Text3").GetComponent<Text>();
            txtList.Add(txt7);
            txt8 = go.transform.Find("xianshi/ChatRoom/TableView2/Select/Button8/Text3").GetComponent<Text>();
            txtList.Add(txt8);

            // 显示世界和其他5个频道的状态和最多20条聊天记录
            // 对应SpeakChannelDataResult2
            CommandHandle.Send(ServerMethod.ShowSpeakChannelData2, null);
            CommandHandle.Send(ServerMethod.EnterSpeakChannel,
                new EnterChannelRequest() { Type = SpeakType.World, ChannelID = 0 });
        }

        //public override bool Show()
        //{
        //    // 如果基础显示方法调用失败
        //    if (!base.Show())
        //    {
        //        return false;
        //    }
        //    //// 显示世界和其他5个频道的状态和最多20条聊天记录
        //    //// 对应SpeakChannelDataResult2
        //    //CommandHandle.Send(ServerMethod.ShowSpeakChannelData2, null);
        //    CommandHandle.Send(ServerMethod.EnterSpeakChannel,
        //        new EnterChannelRequest() { Type = SpeakType.World, ChannelID = 0 });
        //    return true;
        //}

        // 进入或离开频道
        private void EnterOrLeaveChannel(bool arg0)
        {
            if (true != tab1Toggle.isOn)  // 进入聊天频道
            {
                // 在选择频道界面时禁用发送按钮
                // 频道标签隐藏时为选择频道界面
                // 选择频道界面1
                if (false == displayGO.activeSelf)
                {
                    sendButton.enabled = false;
                    // 显示8个频道的各频道人数
                    // 对应CommandHandle.SpeakChannelDataResult方法
                    CommandHandle.Send(ServerMethod.ShowSpeakChannelData, null);
                }

                // 设置标题的开关状态
                tab1OpenGO.SetActive(false);
                tab1CloseGO.SetActive(true);
                tab2OpenGO.SetActive(true);
                tab2CloseGO.SetActive(false);
            }
            else  // 进入世界频道
            {
                // 启用发送按钮
                sendButton.enabled = true;

                // 设置标题的开关状态
                tab1OpenGO.SetActive(true);
                tab1CloseGO.SetActive(false);
                tab2OpenGO.SetActive(false);
                tab2CloseGO.SetActive(true);
            }
        }

        // 打开选择频道界面
        // 选择频道界面2
        private void OpenSelectInterface()
        {
            //// 发送离开房间的命令
            //CommandHandle.Send(ServerMethod.LeaveSpeakChannel, null);
            // 重定向到世界频道，就是认为离开某个聊天频道就到了世界频道
            CommandHandle.Send(ServerMethod.RedirectSpeakChannel,new IDMessage() {Id = 0});

            displayGO.SetActive(false);
            selectGO.SetActive(true);
            // 隐藏所有房间
            for (int i = 0; i < 8; i++)
            {
                roomList[i].SetActive(false);
            }
            // 禁用发送按钮
            sendButton.enabled = false;
            // 显示8个频道的各频道人数
            // 对应CommandHandle.SpeakChannelDataResult方法
            CommandHandle.Send(ServerMethod.ShowSpeakChannelData, null);
        }

        // 打开房间
        private void OpenRoom(int index)
        {
            switch (index)
            {
                case 1:
                    {
                        selectGO.SetActive(false);
                        displayGO.SetActive(true);
                        // 只显示房间1
                        for (int i = 0; i < 8; i++)
                        {
                            if (i == 0)
                            {
                                roomList[i].SetActive(true);
                            }
                            else
                            {
                                roomList[i].SetActive(false);
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        selectGO.SetActive(false);
                        displayGO.SetActive(true);
                        // 只显示房间2
                        for (int i = 0; i < 8; i++)
                        {
                            if (i == 1)
                            {
                                roomList[i].SetActive(true);
                            }
                            else
                            {
                                roomList[i].SetActive(false);
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        selectGO.SetActive(false);
                        displayGO.SetActive(true);
                        // 只显示房间3
                        for (int i = 0; i < 8; i++)
                        {
                            if (i == 2)
                            {
                                roomList[i].SetActive(true);
                            }
                            else
                            {
                                roomList[i].SetActive(false);
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        selectGO.SetActive(false);
                        displayGO.SetActive(true);
                        // 只显示房间4
                        for (int i = 0; i < 8; i++)
                        {
                            if (i == 3)
                            {
                                roomList[i].SetActive(true);
                            }
                            else
                            {
                                roomList[i].SetActive(false);
                            }
                        }
                        break;
                    }
                case 5:
                    {
                        selectGO.SetActive(false);
                        displayGO.SetActive(true);
                        // 只显示房间5
                        for (int i = 0; i < 8; i++)
                        {
                            if (i == 4)
                            {
                                roomList[i].SetActive(true);
                            }
                            else
                            {
                                roomList[i].SetActive(false);
                            }
                        }
                        break;
                    }
                case 6:
                    {
                        selectGO.SetActive(false);
                        displayGO.SetActive(true);
                        // 只显示房间6
                        for (int i = 0; i < 8; i++)
                        {
                            if (i == 5)
                            {
                                roomList[i].SetActive(true);
                            }
                            else
                            {
                                roomList[i].SetActive(false);
                            }
                        }
                        break;
                    }
                case 7:
                    {
                        selectGO.SetActive(false);
                        displayGO.SetActive(true);
                        // 只显示房间7
                        for (int i = 0; i < 8; i++)
                        {
                            if (i == 6)
                            {
                                roomList[i].SetActive(true);
                            }
                            else
                            {
                                roomList[i].SetActive(false);
                            }
                        }
                        break;
                    }
                default:
                    {
                        selectGO.SetActive(false);
                        displayGO.SetActive(true);
                        // 只显示房间8
                        for (int i = 0; i < 8; i++)
                        {
                            if (i == 7)
                            {
                                roomList[i].SetActive(true);
                            }
                            else
                            {
                                roomList[i].SetActive(false);
                            }
                        }
                        break;
                    }
            }
        }

        // 5 刷新面板
        public override void Refresh()
        {
            // 播放打开动画
            xianshiRT.DOLocalMoveY(0, 0.8f);

            base.Refresh();  // 本来就存在的方法 添加的内容放到它前面
        }

        // 6 设置信息条 
        // 这是必须的，不写会出现意外的问题
        public override void SetInfoBar()
        {

        }

        //// 7
        //// 重写关闭按钮的 Close 方法
        //public override void Close(bool isCloseAllMode = false)
        //{
        //    base.Close(isCloseAllMode);  // 本来就存在的方法 添加的内容放到它前面
        //}

        // 发送聊天内容
        private void SendChatContent()
        {
            // 如果输入的内容不为空，发送给服务器
            if (inputField.text != null && inputField.text != "")
            {
                if (true == tab1Toggle.isOn)  // 世界频道开启的时候
                {
                    Debug.Log("--------------------");
                    CommandHandle.Send(ServerMethod.Speak, new SpeakRequest() { Message = inputField.text, Type = SpeakType.World });
                    Debug.Log("================");
                }
                else
                {
                    CommandHandle.Send(ServerMethod.Speak, new SpeakRequest() { Message = inputField.text, Type = SpeakType.Channel, ChannelID = (long)roomNum });
                }

                inputField.text = "";  // 清空输入的聊天内容
            }
        }

        // 在显示收到的聊天内容
        private void DisplayChatContent(string senderName, string chatContent, long senderId, long channelId)
        {
            // 实例化一个世界频道子项
            GameObject cellInstance = GameObject.Instantiate(cell);
            if (0 == channelId)
            {
                cellInstance.SetActive(true);  // 因为隐掉了目标cell，所以实例化出来的子项也是隐藏的，这步把子项显示
                cellInstance.transform.SetParent(contentTf);
                cellInstance.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);  // Scale属性莫名变成1点几，那就改回来
                // 子项上限100条
                cellInstanceList.Add(cellInstance);
                // 设置为==100可以不，会出错吗
                if (cellInstanceList.Count >= 100)
                {
                    UnityEngine.Object.Destroy(cellInstanceList[0]);  // 找这个方法用了好一会时间
                    cellInstanceList.Remove(cellInstanceList[0]);
                }
            }
            else
            {
                // 事实证明不能用新的对象来实例化并赋值，用cell来实例化，然后设置新的父对象即可
                //// 实例化一个房间频道子项并赋值给cellInstance
                //cellInstance = GameObject.Instantiate(cellList[(int)channelId - 1]);

                cellInstance.SetActive(true);  // 因为隐掉了目标cell，所以实例化出来的子项也是隐藏的，这步把子项显示
                cellInstance.transform.SetParent(contentList[(int)channelId - 1]);  // 设置父对象
                cellInstance.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);  // Scale属性莫名变成1点几，那就改回来

                switch ((int)channelId)
                {
                    case 1:
                        {
                            // 子项上限100条
                            cell1InstanceList.Add(cellInstance);
                            // 设置为==100可以不，会出错吗
                            if (cell1InstanceList.Count >= 100)
                            {
                                UnityEngine.Object.Destroy(cell1InstanceList[0]);  // 找这个方法用了好一会时间
                                cellInstanceList.Remove(cell1InstanceList[0]);
                            }
                            break;
                        }
                    case 2:
                        {
                            // 子项上限100条
                            cell2InstanceList.Add(cellInstance);
                            // 设置为==100可以不，会出错吗
                            if (cell2InstanceList.Count >= 100)
                            {
                                UnityEngine.Object.Destroy(cell2InstanceList[0]);  // 找这个方法用了好一会时间
                                cellInstanceList.Remove(cell2InstanceList[0]);
                            }
                            break;
                        }
                    case 3:
                        {
                            // 子项上限100条
                            cell3InstanceList.Add(cellInstance);
                            // 设置为==100可以不，会出错吗
                            if (cell3InstanceList.Count >= 100)
                            {
                                UnityEngine.Object.Destroy(cell3InstanceList[0]);  // 找这个方法用了好一会时间
                                cellInstanceList.Remove(cell3InstanceList[0]);
                            }
                            break;
                        }
                    case 4:
                        {
                            // 子项上限100条
                            cell4InstanceList.Add(cellInstance);
                            // 设置为==100可以不，会出错吗
                            if (cell4InstanceList.Count >= 100)
                            {
                                UnityEngine.Object.Destroy(cell4InstanceList[0]);  // 找这个方法用了好一会时间
                                cellInstanceList.Remove(cell4InstanceList[0]);
                            }
                            break;
                        }
                    case 5:
                        {
                            // 子项上限100条
                            cell5InstanceList.Add(cellInstance);
                            // 设置为==100可以不，会出错吗
                            if (cell5InstanceList.Count >= 100)
                            {
                                UnityEngine.Object.Destroy(cell5InstanceList[0]);  // 找这个方法用了好一会时间
                                cellInstanceList.Remove(cell5InstanceList[0]);
                            }
                            break;
                        }
                    case 6:
                        {
                            // 子项上限100条
                            cell6InstanceList.Add(cellInstance);
                            // 设置为==100可以不，会出错吗
                            if (cell6InstanceList.Count >= 100)
                            {
                                UnityEngine.Object.Destroy(cell6InstanceList[0]);  // 找这个方法用了好一会时间
                                cellInstanceList.Remove(cell6InstanceList[0]);
                            }
                            break;
                        }
                    case 7:
                        {
                            // 子项上限100条
                            cell7InstanceList.Add(cellInstance);
                            // 设置为==100可以不，会出错吗
                            if (cell7InstanceList.Count >= 100)
                            {
                                UnityEngine.Object.Destroy(cell7InstanceList[0]);  // 找这个方法用了好一会时间
                                cellInstanceList.Remove(cell7InstanceList[0]);
                            }
                            break;
                        }
                    default:
                        {
                            // 子项上限100条
                            cell8InstanceList.Add(cellInstance);
                            // 设置为==100可以不，会出错吗
                            if (cell8InstanceList.Count >= 100)
                            {
                                UnityEngine.Object.Destroy(cell8InstanceList[0]);  // 找这个方法用了好一会时间
                                cellInstanceList.Remove(cell8InstanceList[0]);
                            }
                            break;
                        }
                }
            }

            // 别人发的显示在左边
            if (senderId != UserManager.Instance.Me.ID)
            {
                // 发信人名字
                Text senderTxt = cellInstance.transform.Find("Text").GetComponent<Text>();
                senderTxt.text = senderName;

                // 聊天内容
                Text chatContentTxt = cellInstance.transform.Find("Text/Image/Text").GetComponent<Text>();
                // 把换行符替换成逗号
                if (chatContent.Contains("\n"))
                {
                    string str = chatContent.Replace("\n", ",");
                    chatContentTxt.text = str;
                }
                else
                {
                    chatContentTxt.text = chatContent;
                }

                // 检查聊天内容文本框的宽度
                // 如果名字文本框宽度+内容文本框宽度+内容前后间隔>=表格视图TableView的宽度时固定内容文本框的宽度为
                // 表格视图TableView的宽度-名字文本框宽度-内容前后间隔
                // 表格视图的宽度tableView.rectTransform.rect.width可以信任
                if (senderTxt.preferredWidth + chatContentTxt.preferredWidth + 70f > tableView.rectTransform.rect.width)
                {
                    // 事实证明下面的方法是不行的，因为ContentSizeFitter根据字符长度来控制文本框宽度了，
                    // 这种方法不能修改文本框宽度
                    //chatContentTxt.GetComponent<RectTransform>().sizeDelta = new 
                    //    Vector2(tableView.rectTransform.rect.width - senderTxt.preferredWidth - 30f, 48f);

                    // 只能通过LayoutElement组件来强行设置文本框宽度
                    chatContentTxt.GetComponent<LayoutElement>().preferredWidth =
                        tableView.rectTransform.rect.width - senderTxt.preferredWidth - 70f;
                    //Debug.Log("------------------------");
                    //Debug.Log(senderTxt.preferredWidth + " " + chatContentTxt.preferredWidth + " " + tableView.rectTransform.rect.width);
                    //Debug.Log(tableView.rectTransform.rect.width - senderTxt.preferredWidth - 30f);

                    // 聊天内容的气泡框
                    Image image = cellInstance.transform.Find("Text/Image").GetComponent<Image>();
                    image.GetComponent<RectTransform>().sizeDelta = new
                        Vector2(tableView.rectTransform.rect.width - senderTxt.preferredWidth - 35f, 56f);
                    //Debug.Log(chatContentTxt.rectTransform.rect.width + "+++++++++++++++");        
                    //Debug.Log(chatContentTxt.GetComponent<LayoutElement>().preferredWidth + "============");
                    //Debug.Log(chatContentTxt.preferredWidth + "----------");

                    // 设置单元格的宽度
                    cellInstance.GetComponent<LayoutElement>().minHeight = 56f;
                }
                else
                {
                    // 聊天内容的气泡框
                    Image image = cellInstance.transform.Find("Text/Image").GetComponent<Image>();
                    image.GetComponent<RectTransform>().sizeDelta = new Vector2(chatContentTxt.preferredWidth + 31f, 34f);
                    //Debug.Log(chatContentTxt.rectTransform.rect.width + "+++++++++++++++");        
                    //Debug.Log(chatContentTxt.GetComponent<LayoutElement>().preferredWidth + "============");
                    //Debug.Log(chatContentTxt.preferredWidth + "----------");
                }
            }
            else  // 自己发的显示在右边
            {
                // 发信人名字
                Text senderTxt = cellInstance.transform.Find("Text1").GetComponent<Text>();
                senderTxt.text = senderName;

                // 聊天内容
                Text chatContentTxt = cellInstance.transform.Find("Text1/Image/Text").GetComponent<Text>();
                // 把换行符替换成逗号                          
                if (chatContent.Contains("\n"))
                {
                    string str = chatContent.Replace("\n", ",");
                    chatContentTxt.text = str;
                }
                else
                {
                    chatContentTxt.text = chatContent;
                }

                if (senderTxt.preferredWidth + chatContentTxt.preferredWidth + 70f > tableView.rectTransform.rect.width)
                {
                    // 只能通过LayoutElement组件来强行设置文本框宽度
                    chatContentTxt.GetComponent<LayoutElement>().preferredWidth =
                        tableView.rectTransform.rect.width - senderTxt.preferredWidth - 70f;

                    // 聊天内容的气泡框
                    Image image = cellInstance.transform.Find("Text1/Image").GetComponent<Image>();
                    image.GetComponent<RectTransform>().sizeDelta = new
                        Vector2(tableView.rectTransform.rect.width - senderTxt.preferredWidth - 35f, 56f);

                    // 设置单元格的宽度
                    cellInstance.GetComponent<LayoutElement>().minHeight = 56f;
                }
                else
                {
                    // 聊天内容的气泡框
                    Image image = cellInstance.transform.Find("Text1/Image").GetComponent<Image>();
                    image.GetComponent<RectTransform>().sizeDelta = new Vector2(chatContentTxt.preferredWidth + 31f, 34f);
                }
            }
        }

        // 点击关闭按钮时
        private void ButtonCloseMethod()
        {
            //// 发送离开聊天界面的命令
            //CommandHandle.Send(ServerMethod.LeaveSpeakChannel, null);

            // 禁用关闭按钮，使动画在播放时不能再次点击关闭按钮
            buttonClose.enabled = false;
            // 播放关闭动画
            Tweener tweenerClose = xianshiRT.DOLocalMoveY(-602, 0.8f);
            // 动画播放完毕时触发关闭事件
            tweenerClose.OnComplete(() =>
            {
                buttonClose.enabled = true;  // 启用关闭按钮
                Close();

                //OpenSelectInterface();
            });
        }

        // 屏蔽敏感词
        // 没参数会报错，不知为啥
        private void ShieldSensitiveWord(string arg0)
        {
            inputField.text = ShieldKeyword.InputAndOutput(inputField.text);
        }

        //  在相应位置生成一条聊天消息
        public void ProduceOneMessage(SpeakData speakData)
        {
            if (speakData.Type == SpeakType.World)  // 世界聊天时
            {
                // 在世界频道显示收到的聊天内容
                DisplayChatContent(speakData.Character.Name, speakData.Message, speakData.Character.Id, 0);
            }
            else
            {
                // 在相应频道生成一条聊天内容
                DisplayChatContent(speakData.Character.Name, speakData.Message, speakData.Character.Id, speakData.ChannelID);
            }
        }

        // 设置人数状态（空闲、良好和热闹）
        // 只有5个
        public void SetState(SpeakChannelDatas datas)
        {
            //Debug.Log(datas.Datas[0].Id + " " + datas.Datas[0].Count);
            //Debug.Log(datas.Datas[1].Id + " " + datas.Datas[1].Count);
            //Debug.Log(datas.Datas[2].Id + " " + datas.Datas[2].Count);
            //Debug.Log(datas.Datas[3].Id + " " + datas.Datas[3].Count);
            //Debug.Log(datas.Datas[4].Id + " " + datas.Datas[4].Count);
            //Debug.Log(datas.Datas[5].Id + " " + datas.Datas[5].Count);
            //Debug.Log(datas.Datas[6].Id + " " + datas.Datas[6].Count);
            //Debug.Log(datas.Datas[7].Id + " " + datas.Datas[7].Count);
            //Debug.Log(datas.Datas.Count + "-----------------------------");
            // 设置一遍状态
            // 频道1
            //if (0 == datas.Datas[0].Count)  // 空闲状态
            //{
            //    panel1Num1.SetActive(true);
            //    panel1Num2.SetActive(false);
            //    panel1Num3.SetActive(false);
            //}
            //else if (datas.Datas[0].Count > 0 && datas.Datas[0].Count < 20)  // 良好
            //{
            //    panel1Num1.SetActive(false);
            //    panel1Num2.SetActive(true);
            //    panel1Num3.SetActive(false);
            //}
            //else  // 热闹
            //{
            //    panel1Num1.SetActive(false);
            //    panel1Num2.SetActive(false);
            //    panel1Num3.SetActive(true);
            //}
            //// 频道2
            //if (0 == datas.Datas[1].Count)  // 空闲状态
            //{
            //    panel2Num1.SetActive(true);
            //    panel2Num2.SetActive(false);
            //    panel2Num3.SetActive(false);
            //}
            //else if (datas.Datas[1].Count > 0 && datas.Datas[0].Count < 20)  // 良好
            //{
            //    panel2Num1.SetActive(false);
            //    panel2Num2.SetActive(true);
            //    panel2Num3.SetActive(false);
            //}
            //else  // 热闹
            //{
            //    panel2Num1.SetActive(false);
            //    panel2Num2.SetActive(false);
            //    panel2Num3.SetActive(true);
            //}
            //// 频道3
            //if (0 == datas.Datas[2].Count)  // 空闲状态
            //{
            //    panel3Num1.SetActive(true);
            //    panel3Num2.SetActive(false);
            //    panel3Num3.SetActive(false);
            //}
            //else if (datas.Datas[2].Count > 0 && datas.Datas[0].Count < 20)  // 良好
            //{
            //    panel3Num1.SetActive(false);
            //    panel3Num2.SetActive(true);
            //    panel3Num3.SetActive(false);
            //}
            //else  // 热闹
            //{
            //    panel3Num1.SetActive(false);
            //    panel3Num2.SetActive(false);
            //    panel3Num3.SetActive(true);
            //}
            //// 频道4
            //if (0 == datas.Datas[3].Count)  // 空闲状态
            //{
            //    panel4Num1.SetActive(true);
            //    panel4Num2.SetActive(false);
            //    panel4Num3.SetActive(false);
            //}
            //else if (datas.Datas[3].Count > 0 && datas.Datas[3].Count < 20)  // 良好
            //{
            //    panel4Num1.SetActive(false);
            //    panel4Num2.SetActive(true);
            //    panel4Num3.SetActive(false);
            //}
            //else  // 热闹
            //{
            //    panel4Num1.SetActive(false);
            //    panel4Num2.SetActive(false);
            //    panel4Num3.SetActive(true);
            //}
            //// 频道5
            //if (0 == datas.Datas[4].Count)  // 空闲状态
            //{
            //    panel5Num1.SetActive(true);
            //    panel5Num2.SetActive(false);
            //    panel5Num3.SetActive(false);
            //}
            //else if (datas.Datas[4].Count > 0 && datas.Datas[4].Count < 20)  // 良好
            //{
            //    panel5Num1.SetActive(false);
            //    panel5Num2.SetActive(true);
            //    panel5Num3.SetActive(false);
            //}
            //else  // 热闹
            //{
            //    panel5Num1.SetActive(false);
            //    panel5Num2.SetActive(false);
            //    panel5Num3.SetActive(true);
            //}
            //// 频道6
            //if (0 == datas.Datas[5].Count)  // 空闲状态
            //{
            //    panel6Num1.SetActive(true);
            //    panel6Num2.SetActive(false);
            //    panel6Num3.SetActive(false);
            //}
            //else if (datas.Datas[5].Count > 0 && datas.Datas[5].Count < 20)  // 良好
            //{
            //    panel6Num1.SetActive(false);
            //    panel6Num2.SetActive(true);
            //    panel6Num3.SetActive(false);
            //}
            //else  // 热闹
            //{
            //    panel6Num1.SetActive(false);
            //    panel6Num2.SetActive(false);
            //    panel6Num3.SetActive(true);
            //}
            //// 频道7
            //if (0 == datas.Datas[6].Count)  // 空闲状态
            //{
            //    panel7Num1.SetActive(true);
            //    panel7Num2.SetActive(false);
            //    panel7Num3.SetActive(false);
            //}
            //else if (datas.Datas[6].Count > 0 && datas.Datas[6].Count < 20)  // 良好
            //{
            //    panel7Num1.SetActive(false);
            //    panel7Num2.SetActive(true);
            //    panel7Num3.SetActive(false);
            //}
            //else  // 热闹
            //{
            //    panel7Num1.SetActive(false);
            //    panel7Num2.SetActive(false);
            //    panel7Num3.SetActive(true);
            //}
            //// 频道8
            //if (0 == datas.Datas[7].Count)  // 空闲状态
            //{
            //    panel8Num1.SetActive(true);
            //    panel8Num2.SetActive(false);
            //    panel8Num3.SetActive(false);
            //}
            //else if (datas.Datas[7].Count > 0 && datas.Datas[7].Count < 20)  // 良好
            //{
            //    panel8Num1.SetActive(false);
            //    panel8Num2.SetActive(true);
            //    panel8Num3.SetActive(false);
            //}
            //else  // 热闹
            //{
            //    panel8Num1.SetActive(false);
            //    panel8Num2.SetActive(false);
            //    panel8Num3.SetActive(true);
            //}
            //// 显示频道人数
            //for (int i = 0; i < 8; i++)
            //{                
            //    txtList[i].text = datas.Datas[i].Count.ToString();
            //}
            //// 显示频道人数
            //for (int i = 0; i < 5; i++)
            //{
            //    txtList[i].text = datas.Datas[i].Count.ToString();
            //}

            for (int i = 0; i < datas.Datas.Count; i++)
            {
                // 设置每个聊天室的状态 空闲等
                // datas.Datas[i].Id == 0时是世界频道
                if (datas.Datas[i].Id == 1)
                {
                    // 空闲状态
                    if (datas.Datas[i].Count == 0)
                    {
                        panel1Num1.SetActive(true);
                        panel1Num2.SetActive(false);
                        panel1Num3.SetActive(false);
                    }
                    // 良好
                    else if (datas.Datas[i].Count > 0 && datas.Datas[i].Count < 20)
                    {
                        panel1Num1.SetActive(false);
                        panel1Num2.SetActive(true);
                        panel1Num3.SetActive(false);
                    }
                    // 热闹
                    else
                    {
                        panel1Num1.SetActive(false);
                        panel1Num2.SetActive(false);
                        panel1Num3.SetActive(true);
                    }
                    // 设置聊天室的人数
                    txtList[0].text = datas.Datas[i].Count.ToString();
                }
                else if (datas.Datas[i].Id == 2)
                {
                    // 空闲状态
                    if (datas.Datas[i].Count == 0)
                    {
                        panel2Num1.SetActive(true);
                        panel2Num2.SetActive(false);
                        panel2Num3.SetActive(false);
                    }
                    // 良好
                    else if (datas.Datas[i].Count > 0 && datas.Datas[i].Count < 20)
                    {
                        panel2Num1.SetActive(false);
                        panel2Num2.SetActive(true);
                        panel2Num3.SetActive(false);
                    }
                    // 热闹
                    else
                    {
                        panel2Num1.SetActive(false);
                        panel2Num2.SetActive(false);
                        panel2Num3.SetActive(true);
                    }
                    // 设置聊天室的人数
                    txtList[1].text = datas.Datas[i].Count.ToString();
                }
                else if (datas.Datas[i].Id == 3)
                {
                    // 空闲状态
                    if (datas.Datas[i].Count == 0)
                    {
                        panel3Num1.SetActive(true);
                        panel3Num2.SetActive(false);
                        panel3Num3.SetActive(false);
                    }
                    // 良好
                    else if (datas.Datas[i].Count > 0 && datas.Datas[i].Count < 20)
                    {
                        panel3Num1.SetActive(false);
                        panel3Num2.SetActive(true);
                        panel3Num3.SetActive(false);
                    }
                    // 热闹
                    else
                    {
                        panel3Num1.SetActive(false);
                        panel3Num2.SetActive(false);
                        panel3Num3.SetActive(true);
                    }
                    // 设置聊天室的人数
                    txtList[2].text = datas.Datas[i].Count.ToString();
                }
                else if (datas.Datas[i].Id == 4)
                {
                    // 空闲状态
                    if (datas.Datas[i].Count == 0)
                    {
                        panel4Num1.SetActive(true);
                        panel4Num2.SetActive(false);
                        panel4Num3.SetActive(false);
                    }
                    // 良好
                    else if (datas.Datas[i].Count > 0 && datas.Datas[i].Count < 20)
                    {
                        panel4Num1.SetActive(false);
                        panel4Num2.SetActive(true);
                        panel4Num3.SetActive(false);
                    }
                    // 热闹
                    else
                    {
                        panel4Num1.SetActive(false);
                        panel4Num2.SetActive(false);
                        panel4Num3.SetActive(true);
                    }
                    // 设置聊天室的人数
                    txtList[3].text = datas.Datas[i].Count.ToString();
                }
                else if (datas.Datas[i].Id == 5)
                {
                    // 空闲状态
                    if (datas.Datas[i].Count == 0)
                    {
                        panel5Num1.SetActive(true);
                        panel5Num2.SetActive(false);
                        panel5Num3.SetActive(false);
                    }
                    // 良好
                    else if (datas.Datas[i].Count > 0 && datas.Datas[i].Count < 20)
                    {
                        panel5Num1.SetActive(false);
                        panel5Num2.SetActive(true);
                        panel5Num3.SetActive(false);
                    }
                    // 热闹
                    else
                    {
                        panel5Num1.SetActive(false);
                        panel5Num2.SetActive(false);
                        panel5Num3.SetActive(true);
                    }
                    // 设置聊天室的人数
                    txtList[4].text = datas.Datas[i].Count.ToString();
                }
            }
        }

        // 设置聊天状态
        public void SetPreviousContent(SpeakChannelDatas2 datas)
        {
            // 把5个频道的状态设为空闲
            panel1Num1.SetActive(true);
            panel1Num2.SetActive(false);
            panel1Num3.SetActive(false);
            panel2Num1.SetActive(true);
            panel2Num2.SetActive(false);
            panel2Num3.SetActive(false);
            panel3Num1.SetActive(true);
            panel3Num2.SetActive(false);
            panel3Num3.SetActive(false);
            panel4Num1.SetActive(true);
            panel4Num2.SetActive(false);
            panel4Num3.SetActive(false);
            panel5Num1.SetActive(true);
            panel5Num2.SetActive(false);
            panel5Num3.SetActive(false);

            for (int i = 0; i < datas.Datas.Count; i++)
            {
                // 1 设置世界频道的内容
                if (datas.Datas[i].Id == 0)
                {
                    if (datas.Datas[i].Speaks != null)
                    {
                        for (int j = 0; j < datas.Datas[i].Speaks.Count; j++)
                        {
                            DisplayChatContent(datas.Datas[i].Speaks[j].Character.Name, datas.Datas[i].Speaks[j].Message,
                                datas.Datas[i].Speaks[j].Character.Id, datas.Datas[i].Speaks[j].ChannelID);
                        }
                    }
                }
                // 2 设置每个聊天室的状态 空闲等
                else if (datas.Datas[i].Id == 1)
                {
                    // 空闲状态
                    if (datas.Datas[i].Count == 0)
                    {
                        panel1Num1.SetActive(true);
                        panel1Num2.SetActive(false);
                        panel1Num3.SetActive(false);
                    }
                    // 良好
                    else if (datas.Datas[i].Count > 0 && datas.Datas[i].Count < 20)
                    {
                        panel1Num1.SetActive(false);
                        panel1Num2.SetActive(true);
                        panel1Num3.SetActive(false);
                    }
                    // 热闹
                    else
                    {
                        panel1Num1.SetActive(false);
                        panel1Num2.SetActive(false);
                        panel1Num3.SetActive(true);
                    }
                    // 设置聊天室的人数
                    txtList[0].text = datas.Datas[i].Count.ToString();
                    // 3 设置每个聊天室的内容
                    if (datas.Datas[i].Speaks != null)
                    {
                        for (int j = 0; j < datas.Datas[i].Speaks.Count; j++)
                        {
                            DisplayChatContent(datas.Datas[i].Speaks[j].Character.Name, datas.Datas[i].Speaks[j].Message,
                                datas.Datas[i].Speaks[j].Character.Id, datas.Datas[i].Speaks[j].ChannelID);
                        }
                    }
                }
                else if (datas.Datas[i].Id == 2)
                {
                    // 空闲状态
                    if (datas.Datas[i].Count == 0)
                    {
                        panel2Num1.SetActive(true);
                        panel2Num2.SetActive(false);
                        panel2Num3.SetActive(false);
                    }
                    // 良好
                    else if (datas.Datas[i].Count > 0 && datas.Datas[i].Count < 20)
                    {
                        panel2Num1.SetActive(false);
                        panel2Num2.SetActive(true);
                        panel2Num3.SetActive(false);
                    }
                    // 热闹
                    else
                    {
                        panel2Num1.SetActive(false);
                        panel2Num2.SetActive(false);
                        panel2Num3.SetActive(true);
                    }
                    // 设置聊天室的人数
                    txtList[1].text = datas.Datas[i].Count.ToString();
                    if (datas.Datas[i].Speaks != null)
                    {
                        for (int j = 0; j < datas.Datas[i].Speaks.Count; j++)
                        {
                            DisplayChatContent(datas.Datas[i].Speaks[j].Character.Name, datas.Datas[i].Speaks[j].Message,
                                datas.Datas[i].Speaks[j].Character.Id, datas.Datas[i].Speaks[j].ChannelID);
                        }
                    }
                }
                else if (datas.Datas[i].Id == 3)
                {
                    // 空闲状态
                    if (datas.Datas[i].Count == 0)
                    {
                        panel3Num1.SetActive(true);
                        panel3Num2.SetActive(false);
                        panel3Num3.SetActive(false);
                    }
                    // 良好
                    else if (datas.Datas[i].Count > 0 && datas.Datas[i].Count < 20)
                    {
                        panel3Num1.SetActive(false);
                        panel3Num2.SetActive(true);
                        panel3Num3.SetActive(false);
                    }
                    // 热闹
                    else
                    {
                        panel3Num1.SetActive(false);
                        panel3Num2.SetActive(false);
                        panel3Num3.SetActive(true);
                    }
                    // 设置聊天室的人数
                    txtList[2].text = datas.Datas[i].Count.ToString();
                    if (datas.Datas[i].Speaks != null)
                    {
                        for (int j = 0; j < datas.Datas[i].Speaks.Count; j++)
                        {
                            DisplayChatContent(datas.Datas[i].Speaks[j].Character.Name, datas.Datas[i].Speaks[j].Message,
                                datas.Datas[i].Speaks[j].Character.Id, datas.Datas[i].Speaks[j].ChannelID);
                        }
                    }
                }
                else if (datas.Datas[i].Id == 4)
                {
                    // 空闲状态
                    if (datas.Datas[i].Count == 0)
                    {
                        panel4Num1.SetActive(true);
                        panel4Num2.SetActive(false);
                        panel4Num3.SetActive(false);
                    }
                    // 良好
                    else if (datas.Datas[i].Count > 0 && datas.Datas[i].Count < 20)
                    {
                        panel4Num1.SetActive(false);
                        panel4Num2.SetActive(true);
                        panel4Num3.SetActive(false);
                    }
                    // 热闹
                    else
                    {
                        panel4Num1.SetActive(false);
                        panel4Num2.SetActive(false);
                        panel4Num3.SetActive(true);
                    }
                    // 设置聊天室的人数
                    txtList[3].text = datas.Datas[i].Count.ToString();
                    if (datas.Datas[i].Speaks != null)
                    {
                        for (int j = 0; j < datas.Datas[i].Speaks.Count; j++)
                        {
                            DisplayChatContent(datas.Datas[i].Speaks[j].Character.Name, datas.Datas[i].Speaks[j].Message,
                                datas.Datas[i].Speaks[j].Character.Id, datas.Datas[i].Speaks[j].ChannelID);
                        }
                    }
                }
                else if (datas.Datas[i].Id == 5)
                {
                    // 空闲状态
                    if (datas.Datas[i].Count == 0)
                    {
                        panel5Num1.SetActive(true);
                        panel5Num2.SetActive(false);
                        panel5Num3.SetActive(false);
                    }
                    // 良好
                    else if (datas.Datas[i].Count > 0 && datas.Datas[i].Count < 20)
                    {
                        panel5Num1.SetActive(false);
                        panel5Num2.SetActive(true);
                        panel5Num3.SetActive(false);
                    }
                    // 热闹
                    else
                    {
                        panel5Num1.SetActive(false);
                        panel5Num2.SetActive(false);
                        panel5Num3.SetActive(true);
                    }
                    // 设置聊天室的人数
                    txtList[4].text = datas.Datas[i].Count.ToString();
                    if (datas.Datas[i].Speaks != null)
                    {
                        for (int j = 0; j < datas.Datas[i].Speaks.Count; j++)
                        {
                            DisplayChatContent(datas.Datas[i].Speaks[j].Character.Name, datas.Datas[i].Speaks[j].Message,
                                datas.Datas[i].Speaks[j].Character.Id, datas.Datas[i].Speaks[j].ChannelID);
                        }
                    }
                }
            }
        }
    }
}
