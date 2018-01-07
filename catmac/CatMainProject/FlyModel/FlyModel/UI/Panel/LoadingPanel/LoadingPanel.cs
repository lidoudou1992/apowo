using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.LoadingPanel
{
    public class LoadingPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "LoadingPanel";
            }
        }

        private Action afterShowCallback;

        public LoadingQueue LoadingQueue;

        public LoadingPanel(RectTransform parent) : base(parent)
        {
            LoadingQueue = new LoadingQueue();
        }

        public DelayController delayController;
        private bool isOnTime;
        private bool isNeedForceClose;

        public override void Load()
        {
            
            ResourceLoader.Instance.TryLoadClone(BundleName, AssetName, (go) =>
            {
                PanelPrefab = go;

                delayController = PanelPrefab.AddComponent<DelayController>();
                
                go.transform.SetParent(transform, false);
                TryInitializeBaseUI();
                Initialize(go);
                InitializeOver();
                loaded = true;
                if (ShowOnLoaded)
                {
                    Show(afterShowCallback);
                }
            });
        }

        protected override void Initialize(GameObject go)
        {
            
        }

        public override bool Show(Action afterShowCallback)
        {
            isOnTime = false;
            isNeedForceClose = false;
            this.afterShowCallback = afterShowCallback;
            return base.Show(afterShowCallback);
        }

        public override bool Show()
        {
            if (loaded)
            {
                if (PanelManager.CurrentPanel == this)
                {
                    return false;
                }

                isOnTime = false;
                isNeedForceClose = false;

                transform.gameObject.SetActive(true);
                transform.SetAsLastSibling();

                delayController.DelayInvoke(() =>
                {
                    isOnTime = true;
                    if (isNeedForceClose)
                    {
                        Close();
                    }
                }, 2f);

                return true;
            }
            return false;
        }

        public override void Close(bool isCloseAllMode = false)
        {
            if (isOnTime)
            {
                Hide();
                Dispose();
            }
            else
            {
                isNeedForceClose = true;
            }

            //Hide();
            //Dispose();
        }
    }
}
