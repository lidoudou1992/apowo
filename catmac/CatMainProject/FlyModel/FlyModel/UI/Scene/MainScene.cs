using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Scene;
using FlyModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FlyModel.UI
{
    public class MainScene : BaseScene
    {
        protected override void InitializeOver()
        {
            base.InitializeOver();
            
        }

        public override void Show()
        {
            base.Show();

            if (CommandHandle.IsReconnect)
            {
                if (PanelManager.CurrentPanel != null)
                {
                    PanelManager.CurrentPanel.SetInfoBar();
                }
            }
            else
            {
                if (SceneManager.Instance.IsChangingScene == false)
                {
                    PanelManager.InfoBar.Show(() => { GuideManager.Instance.TakenStartByServer(); });
                    CommandHandle.ShowLoginNotice();

                    //测试
                    //PanelManager.InfoBar.Show(() => { GuideManager.Instance.StartGuideManul(2); });
                }
            }

            SceneManager.Instance.IsChangingScene = false;
        }

        public void ShowAvailable(BagItemData data)
        {
            SceneGameObject.ShowAvailable(data);
        }

        public override void OnUpHandler(PointerEventData eventData)
        {
            base.OnUpHandler(eventData);
        }

        public override void OnDownHandler(PointerEventData eventData)
        {
            base.OnDownHandler(eventData);
        }
    }
}
