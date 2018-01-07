using FlyModel.Model.Data;
using FlyModel.Proto;
using FlyModel.UI;
using FlyModel.UI.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyModel.Model
{
    class SceneManager
    {
        //场景点与房间关系的配置 0屋内 1屋外
        private static Dictionary<string, int[]> TOY_POINT_DISTRIBUTION = new Dictionary<string, int[]>();
        private static Dictionary<string, int[]> FOOD_POINT_DISTRIBUTION = new Dictionary<string, int[]>();
        public static int[] CURRENT_TOY_POINT_DISTRIBUTION;
        public static int[] CURRENT_FOOD_POINT_DISTRIBUTION;
        private static string[] sceneAssetBundles = new string[6] { "", "Scene001", "Scene002", "Scene003", "Scene004", "Scene005" };

        public static SceneManager Instance;

        //public SceneData CurrentSceneData;

        public bool IsOpenFullSize;

        public string CurrentSceneName;

        /// <summary>
        /// 是否是正在切换场景过程中
        /// </summary>
        public bool IsChangingScene;

        public int CurrentSceneType = 1;

        public static SceneManager Initialize()
        {
            if (Instance==null)
            {
                Instance = new SceneManager();

                TOY_POINT_DISTRIBUTION.Add("Scene001", new int[9] { -1, 1, 1, 1, 1, 0, 0, 0, 1});
                FOOD_POINT_DISTRIBUTION.Add("Scene001", new int[3] { -1, 1, 0 });
                TOY_POINT_DISTRIBUTION.Add("Scene002", new int[9] { -1, 0, 0, 0, 0, 1, 1, 1, 1});
                FOOD_POINT_DISTRIBUTION.Add("Scene002", new int[3] { -1, 1, 0 });
                TOY_POINT_DISTRIBUTION.Add("Scene003", new int[9] { -1, 0, 0, 0, 1, 1, 1, 1, 1});
                FOOD_POINT_DISTRIBUTION.Add("Scene003", new int[3] { -1, 1, 0 });
                TOY_POINT_DISTRIBUTION.Add("Scene004", new int[9] { -1, 0, 0, 0, 1, 1, 1, 1, 1});
                FOOD_POINT_DISTRIBUTION.Add("Scene004", new int[3] { -1, 1, 0 });
                TOY_POINT_DISTRIBUTION.Add("Scene005", new int[9] { -1, 0, 1, 1, 0, 1, 1, 1, 1 });
                FOOD_POINT_DISTRIBUTION.Add("Scene005", new int[3] { -1, 1, 0 });
            }
            return Instance;
        }

        public BaseScene CurrentScene;

        private MainScene mainScene;
        public MainScene MainScene
        {
            get
            {
                if (mainScene==null)
                {
                    mainScene = new MainScene();
                }
                CurrentScene = mainScene;
                return mainScene;
            }
        }

        public void EnterGameScene(long sceneType)
        {
            //显示主场景
            CurrentSceneType = (int)sceneType;
            CurrentSceneName = sceneAssetBundles[sceneType];
            CURRENT_TOY_POINT_DISTRIBUTION = TOY_POINT_DISTRIBUTION[CurrentSceneName];
            CURRENT_FOOD_POINT_DISTRIBUTION = FOOD_POINT_DISTRIBUTION[CurrentSceneName];
            Instance.MainScene.StartShow(CurrentSceneName);
        }

        private ArtTestScene artTestScene;
        public ArtTestScene ArtTestScene
        {
            get
            {
                if (artTestScene == null)
                {
                    artTestScene = new ArtTestScene();
                }
                CurrentScene = artTestScene;
                return artTestScene;
            }
        }

        public void EnterArtTestScene()
        {
            //显示主场景
            string sceneAssetName = "Scene";
            Instance.ArtTestScene.StartShow(sceneAssetName);
        }

        public void ChangeScene(long sceneType)
        {
            IsChangingScene = true;

            PanelManager.ShowTipString(
                "更换场景会回收所有的玩具哦！",
                EnumConfig.PropPopupPanelBtnModde.TwoBtb,
                "更换",
                () => {
                    PanelManager.CurrentPanel.Close();
                    PanelManager.CurrentPanel.Close();
                    CommandHandle.Send(ServerMethod.SelectDefaultRoom, new IDMessage() { Id = sceneType });
                }
                );

            //PanelManager.LoadingPanel.Show();

            //SceneGameObjectManager.Instance.ClearData();
            //CatManager.Instance.ClearData();

            ////数据接收完毕，进入场景
            //Instance.EnterGameScene("Scene002");
        }
        
    }
}
