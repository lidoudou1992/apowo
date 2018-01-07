using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI;
using FlyModel.UI.Panel.LotteryPanel;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using UnityEngine;

namespace FlyModel
{
    public partial class CommandHandle
    {
        private static int _enterGameTimes;

        public static bool IsReconnect => _enterGameTimes >= 2;

        public static List<Proto.SMMessage> LoginNoticeDatas;

        // 标志是否隐藏兑换领奖和邀请有礼按钮的全局变量
        public static bool isHideButton = false;
        // 是否显示绑定提醒消息
        private static bool isDisplayBindMessage = false;

        public static void Send(ServerMethod _oc, ProtoObject po)
        {
            byte[] bytes = null;
            if (po != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    po.WriteTo(ms);
                    bytes = ms.ToArray();
                }
            }
            GameApplication.Instance.SocketClient.SendOpcodeData((int)_oc, bytes);
            Debug.Log(string.Format("send opcode={0}, name={1}, length={2}", (int)_oc, _oc, (bytes == null ? 0 : bytes.Length)));
            //Debug.Log(string.Format("发送的数据:{0}", JsonMapper.ToJson(po)));
            //Debug.Log(JsonSerializer.Serialize(po).ToString());
        }

        //// 韩版
        ///// <summary>
        ///// 登录
        ///// </summary>
        ///// <param name="loginData"></param>
        //public static void Login(LoginData loginData)
        //{
        //    if (PanelManager.IsCurrentPanel(PanelManager.directConnectPanel))
        //    {
        //        PanelManager.directConnectPanel.Close();
        //    }
        //    if (GameMain.LoginType == EnumConfig.LoginType.LoginPanel)
        //    {
        //        if (loginData.Actors.Count <= 0)
        //        {
        //            //没有角色，需要创角
        //            Send(ServerMethod.CreateCharacter, new CreateCharacter() { NickName = PlayerPrefs.GetString("account"), Gender = Proto.Gender.女 });
        //        }
        //        else
        //        {
        //            //选角色
        //            Debug.Log(string.Format("有 {0} 个角色", loginData.Actors.Count));
        //            foreach (var character in loginData.Actors)
        //            {
        //                Debug.Log(string.Format("角色信息：ID={0}, Name={1}, Gender={2}, Dollar={3}, Coin={4}, AdminType={5}", character.Id, character.Name, character.Gender, character.Dollar, character.Coin, character.AdminType));
        //            }
        //            Debug.Log("-------------角色列表显示结束-------------");
        //            // 发送命令，服务器返回进入游戏命令EnterGame
        //            Send(ServerMethod.SelectCharacter, new IDMessage() { Id = loginData.Actors[0].Id });
        //        }
        //    }
        //    else
        //    {
        //        bool onLineMode = true;
        //        if (onLineMode)
        //        {
        //            if (loginData.Actors.Count <= 0)
        //            {
        //                //没有角色，需要创角
        //                //GuideManager.Instance.CreateCharacterGudie();
        //                //PanelManager.LoadCreateCharacterScenePrefab(() =>
        //                //{
        //                //    PanelManager.GuidePanel.Show(() => { GuideManager.Instance.StartGuide(); });
        //                //});

        //                // 打开创建角色面板
        //                PanelManager.LoadCreateCharacterScenePrefab(() =>
        //                {
        //                    PanelManager.CreateCharacterPanel.Show();
        //                });
        //            }
        //            else
        //            {
        //                //选角色
        //                Debug.Log(string.Format("有 {0} 个角色", loginData.Actors.Count));
        //                foreach (var character in loginData.Actors)
        //                {
        //                    Debug.Log(string.Format("角色信息：ID={0}, Name={1}, Gender={2}, Dollar={3}, Coin={4}, AdminType={5}", character.Id, character.Name, character.Gender, character.Dollar, character.Coin, character.AdminType));
        //                }
        //                Debug.Log("-------------角色列表显示结束-------------");

        //                Send(ServerMethod.SelectCharacter, new IDMessage() { Id = loginData.Actors[loginData.Actors.Count - 1].Id });
        //            }
        //        }
        //        //else
        //        //{
        //        //    //GuideManager.Instance.CreateCharacterGudie();
        //        //    //PanelManager.LoadCreateCharacterScenePrefab(() =>
        //        //    //{
        //        //    //    PanelManager.GuidePanel.Show(() => { GuideManager.Instance.StartGuide(); });
        //        //    //});

        //        //    PanelManager.LoadCreateCharacterScenePrefab(() =>
        //        //    {
        //        //        PanelManager.CreateCharacterPanel.Show();
        //        //    });
        //        //}

        //        //GuideManager.Instance.StartGuideManul(1);
        //    }
        //}

        // 新账号体系的登录
        public static void Login(LoginData loginData)
        {
            // 当不需要显示绑定提醒消息的时候
            if (isDisplayBindMessage == false)
            {
                // 如果当前界面是登录界面
                if (PanelManager.IsCurrentPanel(PanelManager.loginPanel))
                {
                    // 关闭登录界面
                    PanelManager.loginPanel.Close();
                }
                if (PanelManager.IsCurrentPanel(PanelManager.directConnectPanel))
                {
                    PanelManager.directConnectPanel.Close();
                }
                if (GameMain.LoginType == EnumConfig.LoginType.LoginPanel)
                {
                    if (loginData.Actors.Count <= 0)
                    {
                        // 打开创建角色界面
                        PanelManager.LoadCreateCharacterScenePrefab(() =>
                        {
                            PanelManager.CreateCharacterPanel.Show();
                        });
                    }
                    else
                    {
                        //选角色
                        Debug.Log(string.Format("有 {0} 个角色", loginData.Actors.Count));
                        foreach (var character in loginData.Actors)
                        {
                            Debug.Log(string.Format("角色信息：ID={0}, Name={1}, Gender={2}, Dollar={3}, Coin={4}, AdminType={5}", character.Id, character.Name, character.Gender, character.Dollar, character.Coin, character.AdminType));
                        }
                        Debug.Log("-------------角色列表显示结束-------------");
                        // 发送命令，服务器返回进入游戏命令EnterGame
                        Send(ServerMethod.SelectCharacter, new IDMessage() { Id = loginData.Actors[0].Id });
                    }
                }
                else
                {
                    bool onLineMode = true;
                    if (onLineMode)
                    {
                        if (loginData.Actors.Count <= 0)
                        {
                            //没有角色，需要创角
                            //GuideManager.Instance.CreateCharacterGudie();
                            //PanelManager.LoadCreateCharacterScenePrefab(() =>
                            //{
                            //    PanelManager.GuidePanel.Show(() => { GuideManager.Instance.StartGuide(); });
                            //});

                            // 打开创建角色面板
                            PanelManager.LoadCreateCharacterScenePrefab(() =>
                            {
                                PanelManager.CreateCharacterPanel.Show();
                            });
                        }
                        else
                        {
                            //选角色
                            Debug.Log(string.Format("有 {0} 个角色", loginData.Actors.Count));
                            foreach (var character in loginData.Actors)
                            {
                                Debug.Log(string.Format("角色信息：ID={0}, Name={1}, Gender={2}, Dollar={3}, Coin={4}, AdminType={5}", character.Id, character.Name, character.Gender, character.Dollar, character.Coin, character.AdminType));
                            }
                            Debug.Log("-------------角色列表显示结束-------------");

                            Send(ServerMethod.SelectCharacter, new IDMessage() { Id = loginData.Actors[loginData.Actors.Count - 1].Id });
                        }
                    }
                }
            }
            else
            {
                // 显示绑定提醒消息
                if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Account")) && loginData.Actors.Count >= 1)
                {
                    // 单确定弹窗
                    PanelManager.ShowTipString("账号 " + PlayerPrefs.GetString("Account") + "和角色名 " + loginData.Actors[0].Name + " 已成功绑定", 
                        EnumConfig.PropPopupPanelBtnModde.OneBtn, middleCallback: () =>
                    {
                        // 如果当前界面是登录界面
                        if (PanelManager.IsCurrentPanel(PanelManager.loginPanel))
                        {
                            // 关闭登录界面
                            PanelManager.loginPanel.Close();
                        }
                        if (PanelManager.IsCurrentPanel(PanelManager.directConnectPanel))
                        {
                            PanelManager.directConnectPanel.Close();
                        }
                        //选角色
                        Debug.Log(string.Format("有 {0} 个角色", loginData.Actors.Count));
                        foreach (var character in loginData.Actors)
                        {
                            Debug.Log(string.Format("角色信息：ID={0}, Name={1}, Gender={2}, Dollar={3}, Coin={4}, AdminType={5}", character.Id, character.Name, character.Gender, character.Dollar, character.Coin, character.AdminType));
                        }
                        Debug.Log("-------------角色列表显示结束-------------");
                        // 发送命令，服务器返回进入游戏命令EnterGame
                        Send(ServerMethod.SelectCharacter, new IDMessage() { Id = loginData.Actors[0].Id });
                    });
                }
                else  // 还是不显示绑定信息弹窗
                {                   
                    // 如果当前界面是登录界面
                    if (PanelManager.IsCurrentPanel(PanelManager.loginPanel))
                    {
                        // 关闭登录界面
                        PanelManager.loginPanel.Close();
                    }
                    if (PanelManager.IsCurrentPanel(PanelManager.directConnectPanel))
                    {
                        PanelManager.directConnectPanel.Close();
                    }
                    if (GameMain.LoginType == EnumConfig.LoginType.LoginPanel)
                    {
                        if (loginData.Actors.Count <= 0)
                        {
                            // 打开创建角色界面
                            PanelManager.LoadCreateCharacterScenePrefab(() =>
                            {
                                PanelManager.CreateCharacterPanel.Show();
                            });
                        }
                        else
                        {
                            //选角色
                            Debug.Log(string.Format("有 {0} 个角色", loginData.Actors.Count));
                            foreach (var character in loginData.Actors)
                            {
                                Debug.Log(string.Format("角色信息：ID={0}, Name={1}, Gender={2}, Dollar={3}, Coin={4}, AdminType={5}", character.Id, character.Name, character.Gender, character.Dollar, character.Coin, character.AdminType));
                            }
                            Debug.Log("-------------角色列表显示结束-------------");
                            // 发送命令，服务器返回进入游戏命令EnterGame
                            Send(ServerMethod.SelectCharacter, new IDMessage() { Id = loginData.Actors[0].Id });
                        }
                    }
                    else
                    {
                        bool onLineMode = true;
                        if (onLineMode)
                        {
                            if (loginData.Actors.Count <= 0)
                            {
                                //没有角色，需要创角
                                //GuideManager.Instance.CreateCharacterGudie();
                                //PanelManager.LoadCreateCharacterScenePrefab(() =>
                                //{
                                //    PanelManager.GuidePanel.Show(() => { GuideManager.Instance.StartGuide(); });
                                //});

                                // 打开创建角色面板
                                PanelManager.LoadCreateCharacterScenePrefab(() =>
                                {
                                    PanelManager.CreateCharacterPanel.Show();
                                });
                            }
                            else
                            {
                                //选角色
                                Debug.Log(string.Format("有 {0} 个角色", loginData.Actors.Count));
                                foreach (var character in loginData.Actors)
                                {
                                    Debug.Log(string.Format("角色信息：ID={0}, Name={1}, Gender={2}, Dollar={3}, Coin={4}, AdminType={5}", character.Id, character.Name, character.Gender, character.Dollar, character.Coin, character.AdminType));
                                }
                                Debug.Log("-------------角色列表显示结束-------------");

                                Send(ServerMethod.SelectCharacter, new IDMessage() { Id = loginData.Actors[loginData.Actors.Count - 1].Id });
                            }
                        }
                    }
                }
            }
        }

        public static void SystemTime(SystemTime time)
        {
            GameMain.TimeTick.SetStartTime(time.Time);
            DateTime dt = GameMain.TimeTick.ConvertTime(time.Time);
            Debug.Log(string.Format("当前系统时间:{0}", dt.ToString("yyyy-MM-dd HH-mm-ss")));
        }

        public static void CreateCharacterOK(CreateCharacterOK data)
        {
            UserManager.Instance.UpdateMe(data);
        }

        public static void GameError(GameErrorData data)
        {
            switch (data.Code)
            {
                case GameErrorCode.E_ID:
                    Debug.LogError("ID错误");
                    break;
                case GameErrorCode.E_Type:
                    Debug.LogError("Type错误");
                    break;
                case GameErrorCode.E_CE:
                    Debug.LogError("角色已经存在");
                    break;
                case GameErrorCode.E_NE:
                    //Debug.LogError("角色名已经存在");
                    PanelManager.ShowTipString("角色名已经存在", EnumConfig.PropPopupPanelBtnModde.OneBtn, middleCallback: () =>
                    {
                        PanelManager.CreateCharacterPanel.Show();
                    });
                    break;
                case GameErrorCode.E_SF:
                    Debug.LogError("输入字符串不合法");
                    break;
                case GameErrorCode.E_LD:
                    Debug.LogError("加载数据失败");
                    break;
                case GameErrorCode.E_CNC:
                    Debug.LogError("上次操作未结束");
                    break;
                case GameErrorCode.E_AN:
                    Debug.LogError("权限不够");
                    break;
                case GameErrorCode.E_Un:
                    Debug.LogError("不知道出啥错了");
                    break;
                // 解析服务器端的报错
                case GameErrorCode.LoginFailed:
                {
                    if (!string.IsNullOrEmpty(data.Msg))
                    {
                        // 显示登录失败的信息的单按钮弹窗
                        PanelManager.ShowTipString(data.Msg, EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    }
                    break;
                }
                // 解析服务器端的报错
                case GameErrorCode.InvalidAction:
                {
                    if (!string.IsNullOrEmpty(data.Msg))
                    {
                        // 显示各种无效的行为的信息的单按钮弹窗
                        PanelManager.ShowTipString(data.Msg,EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    }
                    break;
                }
                // 解析服务器端的报错
                case GameErrorCode.ServerBusy:
                {
                    Debug.Log("服务器忙");
                    if (!string.IsNullOrEmpty(data.Msg))
                    {
                        // 显示服务器忙信息的单按钮弹窗
                        PanelManager.ShowTipString(data.Msg,EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    }
                    break;
                }
                // 解析服务器端的报错
                case GameErrorCode.RegisterFailed:
                    {
                        Debug.Log("注册失败");
                        // 错误信息是可选项，可以没有
                        if (!string.IsNullOrEmpty(data.Msg))
                        {
                            // 显示注册失败信息的单按钮弹窗
                            PanelManager.ShowTipString(data.Msg, EnumConfig.PropPopupPanelBtnModde.OneBtn);
                        }
                        break;
                    }
                default:
                    Debug.LogError(string.Format("未解析的GameError: {0}", data.Code));
                    break;
            }
        }

        /// <summary>
        /// 隐藏兑换领奖和邀请有礼按钮
        /// </summary>
        public static void ShieldFunction()
        {
            Debug.Log("进入ShieldFunction");
            // 用全局变量好了。。
            isHideButton = true;
            Debug.Log("隐藏兑换领奖按钮和邀请有礼按钮");
        }

        /// <summary>
        /// 进入游戏
        /// </summary>
        /// <param name="data"></param>
        public static void EnterGame(EnterGameData data)
        {
            _enterGameTimes++;
            // 隐藏创建角色面板
            PanelManager.HideCreateCharacterScenePrefab();

            if (GameApplication.Instance.ConnectTimes > 1)
            {
                BagManager.Instance.BagItemList.Clear();
                AwardManager.Instance.AwardList.Clear();

                // 显示加载画面
                PanelManager.LoadingPanel.Show(() =>
                {
                    // 关闭登录画面
                    // 之前这段没写，找了一天才找到原因是在这
                    if (PanelManager.loginPanel != null)
                    {
                        PanelManager.loginPanel.Close();
                    }
                });
                
            }
            else
            {
                //第一次登陆（排除重连）
                LoginNoticeDatas = data.SMMsgs;

                // 显示加载画面
                PanelManager.LoadingPanel.Show(() =>
                {
                    // 关闭登录界面
                    if (PanelManager.loginPanel != null)
                    {
                        PanelManager.loginPanel.Close();
                    }
                });
            }

            //进入游戏，初始化数据
            UserManager.Instance.UpdateMe(data.Character);
            BagManager.Instance.AddBagItems(data.Foods);
            BagManager.Instance.AddBagItems(data.Items);
            BagManager.Instance.AddBagItems(data.Furnis);
            AwardManager.Instance.AddAwards(data.Awards);
            HandbookManager.Instance.UpdateCats(data.Pics);
            RoomManager.Instance.UpdateRooms(data.Rooms);
            GuideManager.Instance.StartGuideByServer(data.Guide);
            ShopManager.Instance.UpdateDatas(BagManager.Instance.BagItemList);
            Debug.Log("===================Achieves " + data.Achieves.Count);
            TaskManager.Instance.UpdateTaskDatas(data.Achieves);


            PanelManager.InfoBar.UpdateAwardBtnActive();

            MailManager.Instance.NewMailCount = data.NRMCount;

            GameApplication.Instance.SDKTools.SetAccountName(data.Character.Name);
            Debug.Log(string.Format("{0}个食物，{1} 个道具，{2} 个玩具， {3} 个奖励, {4} 个场景", data.Foods.Count, data.Items.Count, data.Furnis.Count, data.Awards.Count, data.Rooms.Count));

            // 初始化签到面板数据
            //Debug.Log("今天是否已签到：" + !data.Sign.Has + "；已签到次数：" + data.Sign.Times);
            if (data.Sign == null)
            {
                TaskManager.Instance.IsSceneExtendPointAchievementGet = true;
                UserManager.Instance.Me.SignAllDays = true;
            }
            else
            {
                Debug.Log("签到数据：" + data.Sign);
                UserManager.Instance.UpdateMe(data.Sign);
            }
            if (true == data.Shield)
            {
                Debug.Log("true == data.Shield");
                // 用全局变量好了。。
                isHideButton = true;
            }
            else
            {
                Debug.Log("data.Shield == false || data.Shield == null");
            }
            Debug.Log("进入游戏=======================");
            Debug.Log("引导数据" + data.Guide);
            Debug.Log("所有数据" + data);
        }

        public static void EnterRoom(Proto.EnterRoomData data)
        {
            //IsReconnect = GameApplication.Instance.ConnectTimes > 1 && SceneManager.Instance.IsChangingScene == false;
            if (SceneManager.Instance.IsChangingScene)
            {
                PanelManager.LoadingPanel.Show();
            }
            
            if (GameMain.SceneRoot.transform.childCount>0)
            {
                GameObject.DestroyImmediate(GameMain.SceneRoot.transform.GetChild(0).gameObject);
            }
            SceneGameObjectManager.Instance.ClearData();
            CatManager.Instance.ClearData();

            SceneGameObjectManager.Instance.AddToyDatas(data.Furnis);
            CatManager.Instance.AddCatDatas(data.Cats);
            SceneGameObjectManager.Instance.AddFoodDatas(data.Foods);
            Debug.Log(string.Format("玩具有 {0} 个， 猫有 {1} 只, 猫粮 {2} 个", data.Furnis.Count, data.Cats.Count, data.Foods.Count));

            //数据接收完毕，进入场景
            SceneManager.Instance.EnterGameScene(data.Type);

            System.Collections.Generic.Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("furnis", data.Furnis.Count.ToString());
            dic.Add("cats", data.Cats.Count.ToString());
            dic.Add("foods", data.Foods.Count.ToString());
            TalkingDataGA.OnEvent("EnterScene", dic);
        }

        public static void LeaveRoom(Proto.IDMessage data)
        {

        }

        /// <summary>
        /// 放置玩具到房间
        /// </summary>
        /// <param name="data"></param>
        public static void PlaceObject(FurniData data)
        {
            ToyData toyData = SceneGameObjectManager.Instance.AddOneToyData(data);
            //SceneManager.Instance.CurrentSceneData.SetToyDatas(ToyManager.Instance.ToyDataList);//原先设置数据时就保持的引用，所以这边数据改了，不需要重新赋值

            BagManager.Instance.UpdateRoomID(data);

            //场景加上玩具物体
            SceneManager.Instance.CurrentScene.SceneGameObject.AddOneToyGameObject(toyData).ShowToy();
            SceneManager.Instance.CurrentScene.SceneGameObject.ClosePointMark();
            //PanelManager.ShopPanel.Show();

            if (GuideManager.Instance.TakeWaitForServerTag()== "PlaceObject")
            {
                GuideManager.Instance.ContinueGuide();
            }
        }
        
        /// <summary>
        /// 移除房间中的玩具，猫粮
        /// </summary>
        /// <param name="data"></param>
        public static void RemoveObject(TargetActionMessage data)
        {
            EnumConfig.BagItemType type = SceneGameObjectManager.Instance.DeleteOneSceneGameObject(data.TargetID);
            //SceneManager.Instance.CurrentSceneData.SetToyDatas(ToyManager.Instance.ToyDataList);//原先设置数据时就保持的引用，所以这边数据改了，不需要重新赋值

            BagManager.Instance.UpdateRoomID(new FurniData() { Id = data.TargetID, RoomID = 0 });

            if (type == EnumConfig.BagItemType.Furni)
            {
                SceneManager.Instance.CurrentScene.SceneGameObject.DeleteOneToyGameObject(data.TargetID);
            }
            else if (type == EnumConfig.BagItemType.Food)
            {
                SceneManager.Instance.CurrentScene.SceneGameObject.DeleteOneFoodGameObject(data.TargetID);
            }

            if (PanelManager.IsCurrentPanel(PanelManager.bagPanel))
            {
                PanelManager.bagPanel.UpdatePlaceState(data.TargetID);
            }
        }

        /// <summary>
        /// 放置猫粮到房间中
        /// </summary>
        /// <param name="data"></param>
        public static void PlaceFood(Proto.FoodData data)
        {
            Model.Data.FoodData foodData = SceneGameObjectManager.Instance.AddOneFoodData(data);

            BagManager.Instance.UpdateRoomID(data);

            //场景加上猫粮
            SceneManager.Instance.CurrentScene.SceneGameObject.AddOneFoodGameObjet(foodData).ShowFood();
            SceneManager.Instance.CurrentScene.SceneGameObject.ClosePointMark();
        }

        public static void UpdateRoomFood(Proto.FoodData data)
        {
            Debug.Log("更新食物");
        }

        /// <summary>
        /// 更新某个玩具
        /// </summary>
        public static void UpdateRoomFurni(FurniData data)
        {
            SceneGameObjectManager.Instance.UpdateToyData(data);
        }

        /// <summary>
        /// 猫进入房间
        /// </summary>
        /// <param name="data"></param>
        public static void CatEnterRoom(Proto.CatData data)
        {
            Model.Data.CatData catData = CatManager.Instance.AddOneCatData(data);
            SceneManager.Instance.CurrentScene.SceneGameObject.AddOneCatGameObject(catData).ShowCat();
        }

        /// <summary>
        /// 猫送珍宝(在猫进房间的同时)
        /// </summary>
        /// <param name="giftData"></param>
        public static void SendCatTreasure(PicData giftData)
        {
            //Debug.Log(string.Format("giftData:{0}", giftData));
            //var gift = HandbookManager.Instance.FindOneCatGiftByType(giftData.TreasureID);
            //CommandHandle.Data.Type = giftData.TreasureID;

            var gift = HandbookManager.Instance.FindOneCatGiftByType(giftData.TreasureID);

            //// 显示猫咪珍宝
            //PanelManager.CatTreasureDisplayPanel.Show(() =>
            //{
            //    PanelManager.catTreasureDisplayPanel.SetData(gift);
            //});

            //// 显示猫咪珍宝
            //PanelManager.CatTreasureDisplayPanel.Show(() =>
            //{
            //    PanelManager.catTreasureDisplayPanel.Conceal();
            //});

            //// 显示获得猫咪珍宝
            //PanelManager.CatTreasureGetPanel.Show(()=>
            //{
            //    PanelManager.catTreasureGetPanel.GetPicData(gift);
            //});

            Debug.Log("服务器正常发送礼物");
            Debug.Log(string.Format("礼物：{0}",gift));
            // 在猫咪礼物队列中增加一个礼物
            PanelManager.CatTreasureGetPanel.Show(()=> {
                Debug.Log("开始添加礼物");
                PanelManager.catTreasureGetPanel.AddOneGift(gift);
                Debug.Log("添加礼物完成");
            });
        }

        /// <summary>
        /// 房间中突然跑了一只猫
        /// </summary>
        /// <param name="data"></param>
        public static void CatLeaveRoom(TargetActionMessage data)
        {
            CatManager.Instance.DeleteOneCatData(data.TargetID);
            SceneManager.Instance.CurrentScene.SceneGameObject.DeleteOneCatGameObject(data.TargetID);
        }

        /// <summary>
        /// 删除包裹中的物品
        /// </summary>
        /// <param name="data"></param>
        public static void DeleteObject(DeleteData data)
        {
            BagManager.Instance.DeleteItem(data);

            //TOGO 更新UI
        }

        /// <summary>
        /// 背包中增加玩具
        /// </summary>
        /// <param name="data"></param>
        public static void NewFurni(FurniData data)
        {
            BagManager.Instance.AddOneBagItem(data);

            if (GuideManager.Instance.TakeWaitForServerTag() == "NewFurni")
            {
                GuideManager.Instance.ContinueGuide();
            }

            //TOGO 更新UI
        }

        public static void UpdateFurni(Proto.FurniData data)
        {
            BagManager.Instance.UpdateFurni(data);
        }

        /// <summary>
        /// 新获得一个奖励
        /// </summary>
        /// <param name="data"></param>
        public static void NewAward(Proto.AwardData data)
        {
            AwardManager.Instance.AddOneAward(data);

            PanelManager.InfoBar.UpdateAwardBtnActive();
        }

        /// <summary>
        /// 删除一条奖励
        /// </summary>
        /// <param name="data"></param>
        public static void DeleteAward(IDMessage data)
        {
            AwardManager.Instance.DeleteOneAward(data);

            PanelManager.InfoBar.UpdateAwardBtnActive();

            if (PanelManager.awardPanel != null && PanelManager.CurrentPanel==PanelManager.awardPanel)
            {
                PanelManager.awardPanel.Refresh();
            }
        }

        public static void DeleteAwards(IDListMessage data)
        {
            var currency = AwardManager.Instance.DeleteAwards(data);

            //if (PanelManager.awardPanel != null && PanelManager.CurrentPanel == PanelManager.awardPanel)
            //{
            //    PanelManager.awardPanel.Close();
            //}
            //// 获得奖励提示
            //PanelManager.TipPanel.Show(() =>
            //    PanelManager.tipPanel.SetText(string.Format("获得 银鱼干{0}  金鱼干{1}",
            //        currency.silver, currency.gold))
            //);
            // 领奖界面存在且为当前界面时
            if (PanelManager.awardPanel != null && PanelManager.CurrentPanel == PanelManager.awardPanel)
            {
                PanelManager.awardPanel.DisplayGainInterface(currency.gold, currency.silver);
            }
        }

        /// <summary>
        /// 更新玩家数据
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateCharacter(CharacterData data)
        {
            UserManager.Instance.Me.UpdateData(data);

            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.UpdateData();
            }

            // 如果充值面板已存在就刷新充值面板
            if (PanelManager.rechargePanel != null)
            {
                PanelManager.rechargePanel.Refresh();
            }
        }

        /// <summary>
        /// 更新商店数据
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateProssess(Proto.ShopData data)
        {
            //ShopItemData temp = ShopManager.Instance.UpdateData(data);

            //if (PanelManager.IsCurrentPanel(PanelManager.shopPanel))
            //{
            //    PanelManager.shopPanel.UpdateData(temp);
            //}
        }

        /// <summary>
        /// 商店中的物品立即使用
        /// </summary>
        /// <param name="data"></param>
        public static void Immediately(IDMessage data)
        {
            BagItemData itemData = BagManager.Instance.FindOneItemData(data.Id);

            BagManager.Instance.SelectOnePlacePoint(itemData);
        }

        /// <summary>
        /// 包裹中获得一个新物品
        /// </summary>
        /// <param name="data"></param>
        public static void NewFood(Proto.FoodData data)
        {
            BagManager.Instance.AddOneBagItem(data);
            BagManager.Instance.Sort();
        }

        /// <summary>
        /// 更新包裹中的物品
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateFood(Proto.FoodData data)
        {
            BagManager.Instance.AddOneBagItem(data);
        }

        public static void NewItem(Proto.ItemData data)
        {
            BagManager.Instance.AddOneBagItem(data);
            BagManager.Instance.Sort();

            if (data.Type == 20000)
            {
                PanelManager.CloseAll();
                //if (PanelManager.IsCurrentPanel(PanelManager.shopPanel))
                //{
                //    PanelManager.SystemBar.ShowLeftBtns(EnumConfig.SystemMuneBtn.Scene);
                //}
                PanelManager.ShowTipString("扩建成功，可以放置更多的玩具和猫粮", EnumConfig.PropPopupPanelBtnModde.OneBtn, middleCallback:() =>
                {
                    //PanelManager.shopPanel.Refresh();
                    GuideManager.Instance.StartGuideManul(3);
                });
            }
        }

        public static void UpdateItem(ItemData data){
            BagManager.Instance.AddOneBagItem(data);
        }

        public static void UpdatePic(Proto.PicData data)
        {
            //引导判断
            if (data.IsSentTreasure && GuideManager.Instance.HasGettedOneTreasure == false)
            {
                GuideManager.Instance.HasGettedOneTreasure = true;
                GuideManager.Instance.HandbookAppointCatType = (int)data.Type;

                PanelManager.CloseAll();
                GuideManager.Instance.StartGuideManul(2);
            }

            HandbookManager.Instance.UpdateData(data, true);

            if (PanelManager.IsCurrentPanel(PanelManager.handbookPanel))
            {
                PanelManager.SystemBar.ShowLeftBtns(EnumConfig.SystemMuneBtn.Treasure);
            }
        }

        public static void NewRoom(Proto.RoomData data)
        {
            RoomManager.Instance.UpdateRoom(data);

            if (PanelManager.IsCurrentPanel(PanelManager.sceneChangePanel))
            {
                PanelManager.sceneChangePanel.Refresh();

                PanelManager.ShowTipString("获得了新场景，立即更换？",
                    EnumConfig.PropPopupPanelBtnModde.TwoBtb,
                    "更换",
                    () => {
                        SceneManager.Instance.ChangeScene(data.Type);
                    }, 
                    rightCallback: () => {
                        //if (PanelManager.shopPanel != null)
                        //{
                        //    PanelManager.shopPanel.Refresh();
                        //}
                    }
                    );
            }
        }

        // 跳过引导后的服务器命令
        public static void UpdateGuide(Proto.GuideData data)
        {
            GuideManager.Instance.ContinueGuide();
        }

        public static void Mails(Proto.MailDatas datas)
        {
            MailManager.Instance.Initialize(datas.Data);
            PanelManager.MailPanel.Show();
        }

        public static void NewMail(Proto.MailData data)
        {
            MailManager.Instance.AddOneMail(data);
            PanelManager.infoBar.updateMailBtnActive();
        }

        public static void ReadMail(IDMessage data)
        {
            Model.Data.MailData d = MailManager.Instance.GetOneMail(data.Id);
            if (d.HasAwards() == false)
            {
                //MailManager.Instance.NewMailCount--;
                
                d.UpdateState(Proto.MailStatus.Read);
                MailManager.Instance.CaculateNewCount();

                PanelManager.infoBar.updateMailBtnActive();
                PanelManager.mailPanel.Refresh();
            }
        }

        public static void GetMailAward(IDMessage data)
        {
            Model.Data.MailData d = MailManager.Instance.GetOneMail(data.Id);

            // 显示领取奖励的提示
            PanelManager.TipPanel.Show(() =>
            {
                Debug.Log("=============11111111");
                Debug.Log(d.Awards.Count);
                string str = "";
                int silver = 0;
                int gold = 0;
                foreach (var award in d.Awards)
                {
                    //if ("Coin" == award.Name)
                    //{
                    //    str += " " + "银鱼干" + "x" + award.Value;
                    //}
                    //else if ("Dollar" == award.Name)
                    //{
                    //    str += " " + "金鱼干" + "x" + award.Value;
                    //}
                    //else 
                    //{
                    //    str += " " + award.Name + "x" + award.Value; 
                    //}
                    
                    if ("Coin" == award.Name)
                    {
                        silver += award.Value;
                    }
                    else if ("Dollar" == award.Name)
                    {
                        gold += award.Value;
                    }
                    else
                    {
                        str += " " + award.Name + "x" + award.Value;    // 鱼干之外的奖励
                    }
                }
                str += " " + "银鱼干" + "x" + silver;
                str += " " + "金鱼干" + "x" + gold;
                Debug.Log(string.Format("奖励属性：{0}", str));
                PanelManager.tipPanel.SetText(string.Format("已领取奖励：{0}", str));

                PanelManager.infoBar.updateMailBtnActive();

                Debug.Log("==========222222");
                d.ClearAwards();

                //MailManager.Instance.NewMailCount--;
                d.UpdateState(Proto.MailStatus.Read);
                MailManager.Instance.CaculateNewCount();

                if (PanelManager.mailPanel != null)
                {
                    PanelManager.mailPanel.Refresh();
                }
            });
        }

        public static void GetAllMailAward()
        {
            MailManager.Instance.GetAllMailAward();
            MailManager.Instance.CaculateNewCount();

            PanelManager.infoBar.updateMailBtnActive();
            if (PanelManager.IsCurrentPanel(PanelManager.mailPanel))
            {
                PanelManager.mailPanel.Refresh();
            }
        }

        public static void RemoveMail(IDMessage data)
        {
            MailManager.Instance.RemoveMail(data);
            // 删除邮件提示
            PanelManager.TipPanel.Show(() =>
                PanelManager.tipPanel.SetText("成功删除了邮件")
            );
            MailManager.Instance.CaculateNewCount();

            PanelManager.infoBar.updateMailBtnActive();

            //if (PanelManager.IsCurrentPanel(PanelManager.mailPanel))
            //{
            //    PanelManager.mailPanel.Refresh();
            //}
            // 删除一封邮件后刷新邮件面板
            PanelManager.mailPanel.Refresh();
        }

        public static void ShowMessage(StringMessage data)
        {
            PanelManager.ShowTipString(data.Msg, EnumConfig.PropPopupPanelBtnModde.OneBtn);
        }

        // 显示绑定账号 xxx 和角色名 xxx 已成功绑定
        public static void BindMessage(StringMessage message)
        {
            isDisplayBindMessage = true;
        }

        public static void ShowLoginNotice()
        {
            if (GuideManager.Instance.IsGuiding()==false && LoginNoticeDatas != null && LoginNoticeDatas.Count>0)
            {
                Model.Data.MailData temp = new Model.Data.MailData();
                temp.SetLoginNoticeData(LoginNoticeDatas[0]);

                PanelManager.MailContentPanel.Show(() => {
                    PanelManager.mailContentPanel.SetData(temp);
                });

                LoginNoticeDatas = null;
            }
        }

        public static void NewAchieve(Proto.AchievementData data)
        {
            TaskManager.Instance.UpdateTask(data);
        }

        public static void UpdateAchieve(Proto.AchievementData data)
        {
            TaskManager.Instance.UpdateTask(data);
        }

        // 更新签到面板数据
        public static void UpdateSignData(Proto.SignData data)
        {
            // 设置客户端的签到字段的值为服务器端传过来的值
            UserManager.Instance.Me.todayHasSigned = !data.Has;
            UserManager.Instance.Me.signDays = data.Times;

            if (data.Times == 7)
            {
                UserManager.Instance.Me.SignAllDays = true;
                TaskManager.Instance.IsSceneExtendPointAchievementGet = true;
            }

            if (PanelManager.signPanel != null) 
            {
                // 刷新签到面板
                PanelManager.signPanel.Refresh();
            }    
            else
            {
                Debug.Log("签到面板不存在");
            }

            // 显示签到奖励弹窗
            switch(data.Times)
            {
                case 1:
                    PanelManager.ShowTipString("今日签到成功，获得奖励：银鱼干100", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    break;
                case 2:
                    PanelManager.ShowTipString("今日签到成功，获得奖励：金鱼干20", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    break;
                case 3:
                    PanelManager.ShowTipString("今日签到成功，获得奖励：兔耳帽 x1", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    break;
                case 4:
                    PanelManager.ShowTipString("今日签到成功，获得奖励：金鱼干30", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    break;
                case 5:
                    PanelManager.ShowTipString("今日签到成功，获得奖励：鲨鱼猫屋 x1", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    break;
                case 6:
                    PanelManager.ShowTipString("今日签到成功，获得奖励：金鱼干50", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    break;
                case 7:
                    PanelManager.ShowTipString("今日签到成功，获得奖励：扩容令", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    break;
                default:
                    break;
            }
        }

        public static void InviteeResult(Proto.InviteeData data)
        {
            //// 显示奖励提示
            //PanelManager.TipPanel.Show(() =>
            //    PanelManager.tipPanel.SetText(string.Format("获得：银鱼干{0} 金鱼干数量{1}", data.Coin, data.Dollar))
            //);
            //不需要显示奖励提示，因为奖励已经发到邮件了

            if (PanelManager.exchangePanel != null)
            {
                PanelManager.exchangePanel.Close();
            }
        }

        // 前面的方法都没有引用，所以写成私有的了 报错了，只能写成公有的了
        // 话说为什么要写成静态的
        public static void LotteryResult(Proto.AwardSpecData awardSpecData)
        {
            if (PanelManager.lotteryPanel != null)
            {
                PanelManager.lotteryPanel.StartLottery(awardSpecData);
            }
            Debug.Log("奖励：" + awardSpecData + "-----------------------------------------");
        }

        // 获取今天已抽奖的次数
        public static void ShowLotteryResult(Proto.ShowLotteryData data)
        {
            if (PanelManager.lotteryPanel != null)
            {
                //PanelManager.lotteryPanel.costTip(data.Times);
                PanelManager.lotteryPanel.costTip(data);
            }
        }

        // 喊话
        public static void Speak(SpeakData speakData)
        {
            // 在相应位置生成一条聊天消息
            if (PanelManager.chatPanel != null)
            {
                PanelManager.chatPanel.ProduceOneMessage(speakData);
            }
        }

        // 得到8个频道的各频道人数
        // 只有5个
        public static void SpeakChannelDataResult(SpeakChannelDatas datas)
        {
            //Debug.Log("--------------------------" + datas);
            // 设置人数状态（空闲、良好和热闹）
            if (PanelManager.chatPanel != null)
            {
                PanelManager.chatPanel.SetState(datas);
            }
        }

        public static void SpeakChannelDataResult2(SpeakChannelDatas2 datas)
        {
            //Debug.Log("收到的聊天内容：" + datas);
            if (PanelManager.chatPanel != null)
            {
                PanelManager.chatPanel.SetPreviousContent(datas);
            }
        }

        // 得到充值列表的数据
        public static void GetShopItemDatasResult(PayItemDatas datas)
        {
            if (PanelManager.rechargePanel != null)
            {
                PanelManager.rechargePanel.ReceivePars(datas);
            }
        }

        public static void UpdateLottery(ShowLotteryData data)
        {
            Debug.Log("剩余次数" + data.LeftTimes);
            if (PanelManager.lotteryPanel != null)
            {
                PanelManager.lotteryPanel.costTip(data);
            }
        }

        // 对应Advert
        public static void AdvertResult(MoneyData data)
        {
            if (PanelManager.awardPanel != null)
            {
                PanelManager.awardPanel.PromptAward(data.Dollar,data.Coin);
            }
        }
    }
}
