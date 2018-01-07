using UnityEngine.UI;
using UnityEngine;
using System;
using FlyModel.UI.Component;

namespace FlyModel.UI
{
    public abstract class PanelBase : DisposeObject
    {
       

        public virtual string BundleName
        {
            get
            {
                return AssetName.ToLower();
            }
        }

        public abstract string AssetName { get; }

        public virtual bool ShowOnLoaded
        {
            get
            {
                return true;
            }
        }

        public GameObject PanelPrefab;

        public PanelBase()
        {
            GameObject go = new GameObject(AssetName+"Root");
            go.transform.SetParent(PanelManager.PanelRectTransform, false);
            transform = go.AddComponent<RectTransform>();
          
            transform.anchorMin = new Vector2(0,0);
            transform.anchorMax = new Vector2(1, 1);
            transform.sizeDelta = Vector2.zero;

            ConstructorCallback();

            Load();
        }

        public PanelBase(RectTransform parent)
        {
            GameObject go = new GameObject(AssetName + "Root");
            go.transform.SetParent(parent, false);
            transform = go.AddComponent<RectTransform>();

            transform.anchorMin = new Vector2(0, 0);
            transform.anchorMax = new Vector2(1, 1);
            transform.sizeDelta = Vector2.zero;

            Load();
        }

        //transform xxxRoot(LoginPanelRoot)
        public RectTransform transform;

        protected Button closeButton;

        /// <summary>
        /// 是否是根
        /// </summary>
        public virtual bool IsRoot
        {
            get
            {
                return false;
            }
        }

        public bool IsActive
        {
            get;
            set;
        }
        /// <summary>
        /// 是否遮挡场景
        /// </summary>
        public virtual bool MaskScene
        {
            get { return true; }
        }

        protected bool loaded = false;

        public Action afterShowCallbackAsync;

        public bool IsNeedPushToPanelStack = true;

        public EnumConfig.InfoLayoutMode HeadLayoutMode = EnumConfig.InfoLayoutMode.Currency;

        public virtual void Load()
        {
            ResourceLoader.Instance.TryLoadClone(BundleName, AssetName, (go) =>
            {
                PanelPrefab = go;

                go.transform.SetParent(transform,false);
                TryInitializeBaseUI();
                Initialize(go);
                InitializeOver();
                loaded = true;
                if (ShowOnLoaded)
                {
                    Show();
                }
            });

        }

        public virtual bool Show()
        {
            if (loaded)
            {
                if (PanelManager.CurrentPanel==this)
                {
                    return false;
                }
              
                if (PanelManager.CurrentPanel != null)
                {
                    if (IsRoot)
                    {
                        PanelManager.CloseAll();
                    }
                    else
                    {
                        PanelManager.CurrentPanel.LostFocus();
                    }
                }

                if (IsNeedPushToPanelStack)
                {
                    PanelManager.OpenPanel(this);
                }
                transform.gameObject.SetActive(true);
                transform.SetAsLastSibling();
                Focus();

                Refresh();

                updateAwardBtnActive();
                UpdateCamareBtnActive();
                UpdateMailBtnActive();
                //UpdateSettingBtnActive();
                UpdateSignBtnActive();
                UpdateShareBtnActive();
                UpdateExchangeBtnActive();
                UpdateTeachBtnActive();
                UpdateLotterBtnActive();  // 抽奖
                UpdateChitchatBtnActive();  // 聊天

                SetInfoBar();

                if (afterShowCallbackAsync != null)
                {
                    afterShowCallbackAsync();
                    afterShowCallbackAsync = null;
                }
              
                return true;
            }
            return false;
        
        }

        public void updateAwardBtnActive()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.UpdateAwardBtnActive();
            }
        }

        public void UpdateCamareBtnActive()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.updateCamareBtnActive();
            }
        }

        public void UpdateShareBtnActive()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.updateShareBtnActive();
            }
        }

        public void UpdateExchangeBtnActive()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.updateExchangeBtnActive();
            }
        }

        public void UpdateTeachBtnActive()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.updateTeachBtnActive();
            }
        }

        //public void UpdateSettingBtnActive()
        //{
        //    if (PanelManager.infoBar != null)
        //    {
        //        PanelManager.infoBar.updateSettingBtnActive();
        //    }
        //}

        public void UpdateMailBtnActive()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.updateMailBtnActive();
            }
        }

        public void UpdateSignBtnActive()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.updateSignBtnActive();
            }
        }

        // 更新抽奖按钮是否启用
        public void UpdateLotterBtnActive()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.UpdateLotteryBtnActive();
            }
        }

        // 更新聊天按钮是否启用
        public void UpdateChitchatBtnActive()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.UpdateChatBtnActive();
            }
        }

        public virtual bool Show(Action afterShowCallback)
        {
            if (Show())
            {
                if (afterShowCallback != null)
                {
                    afterShowCallback();
                }
                return true;
            }
            else
            {
                afterShowCallbackAsync = afterShowCallback;
                return false;
            }
        }

        public virtual void Refresh() { }

        public virtual void RefreshWhenBack() { }

        public virtual void Hide()
        {
            transform.gameObject.SetActive(false);
            LostFocus();
        }

        /// <summary>
        /// 单纯关闭面板 显示上一个面板
        /// </summary>
        public virtual void Close(bool isCloseAllMode=false)
        {
            Hide();
            Dispose();

            if (isCloseAllMode == false)
            {
                PanelManager.CloseCurrent();
            }

            updateAwardBtnActive();
            UpdateCamareBtnActive();
            UpdateMailBtnActive();
            //UpdateSettingBtnActive();
            UpdateSignBtnActive();
            UpdateShareBtnActive();
            UpdateExchangeBtnActive();
            UpdateTeachBtnActive();
            UpdateLotterBtnActive();  // 抽奖
            UpdateChitchatBtnActive();  // 聊天
        }

        public virtual void Focus()
        {
            IsActive = true;
        }

        public virtual void LostFocus()
        {
            //Debug.LogWarning(AssetName + " LostFocus");
            IsActive = false;
        }

        /// <summary>
        /// 尝试初始化一些面板通用资源
        /// </summary>
        protected void TryInitializeBaseUI()
        {
            var cbgo=    transform.gameObject.FindChildByName("CloseButton");
            if (cbgo != null)
            {
                //closeButton = cbgo.GetComponent<Button>();
                //closeButton.onClick.AddListener(OnCloseButtonClick);

                new SoundButton(cbgo, OnCloseButtonClick, ResPathConfig.CLOSE_PANEL);
            }
        }

        /// <summary>
        /// 如果面板里面有 closeButton 重载才会生效
        /// </summary>
        public virtual void OnCloseButtonClick()
        {
            Close();
        }

        protected abstract void Initialize(GameObject go);

        /// <summary>
        /// 设置面顶挂的层次
        /// 在子类中覆盖此方法，为infoBar设置子类面板对应的菜单回掉
        /// </summary>
        public virtual void SetInfoBar()
        {
            if (PanelManager.infoBar != null)
            {
                PanelManager.infoBar.PanelPrefab.SetActive(true);

                Transform parent = PanelManager.infoBar.transform.parent;
                parent.SetParent(transform);
                PanelManager.infoBar.transform.SetAsLastSibling();

                PanelManager.infoBar.UpdateInfoLayout(HeadLayoutMode);
            }
        }

        protected virtual void InitializeOver()
        {
            InitializeWrap();
        }
  

        public override void Dispose()
        {
             
        }

        public virtual void ConstructorCallback()
        {

        }
        #region Wraps
        public virtual void InitializeWrap()
        {
           
        }

    

        #region static
         
        Image AddImage(float x, float y, float w = 100, float h = 100)
        {
            var ti = new GameObject("x");
            ti.transform.SetParent(transform, false);
            var image = ti.AddComponent<Image>();
            image.rectTransform.localPosition = new Vector3(x, y);
            image.rectTransform.sizeDelta = new Vector2(w, h);
            return image;
        }

       

        static void GetRowCell(GridLayoutGroup glg,out int row,out int cell)
        {
            var cc = glg.transform.childCount;
            if (glg.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
            {
                cell = glg.constraintCount;
                row = Mathf.CeilToInt(cc / (float)cell);
            }
            else if (glg.constraint == GridLayoutGroup.Constraint.FixedRowCount)
            {
                row = glg.constraintCount;
                cell = Mathf.CeilToInt(cc / (float)row);
            }
            else
            {
                row = 1;
                cell = cc;
            }
        }

      
        #endregion
        #endregion
    }
}
