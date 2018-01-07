using Assets.Scripts.TouchController;
using FlyModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.SystemBar
{
    
    public class SystemBar
    {
        public GameObject PanelPrefab;

        private string[] btnNames = new string[7] { "Shop", "Bag", "PicBook", "PHOTO", "Setting", "Treasure", "Scene"};

        private List<SystemMenuBtn> btns = new List<SystemMenuBtn>();

        public SystemBar()
        {

        }

        public void InitUI(GameObject prefab)
        {
            PanelPrefab = prefab;

            SystemMenuBtn temp;
            for (int i = 0; i < btnNames.Length; i++)
            {
                temp = new SystemMenuBtn(prefab.transform.Find(btnNames[i]).gameObject);
                temp.Index = i;
                temp.AddListener(SelectBtn);
                btns.Add(temp);
            }

            Refresh();
        }

        public void Refresh()
        {
            ShowLeftBtns(EnumConfig.SystemMuneBtn.Scene);
        }

        public void SelectBtn(int index)
        {
            //if (index != 4)
            //{
                for (int i = 0; i < btns.Count; i++)
                {

                    btns[i].ShowSelected(i == index);
                }
            //}
                

            switch (index)
            {
                case 0:
                    if (PanelManager.IsCurrentPanel(PanelManager.shopPanel) == false)
                    {
                        //if (PanelManager.shopPanel != null)
                        //{
                        //    //PanelManager.CurrentPanel.Close();
                        //}
                        PanelManager.ShopPanel.Show();
                    }

                    ShowLeftBtns(EnumConfig.SystemMuneBtn.Scene);

                    break;
                case 1:
                    if (PanelManager.IsCurrentPanel(PanelManager.bagPanel) == false)
                    {
                        //if (PanelManager.bagPanel != null)
                        //{
                        //    PanelManager.CurrentPanel.Close();
                        //}
                        PanelManager.BagPanel.Show(() =>
                        {
                            if (GuideManager.Instance.IsGestureTouchEffective("Bag"))
                            {
                                GuideManager.Instance.ContinueGuide();
                            }
                        });

                    }

                    ShowLeftBtns(EnumConfig.SystemMuneBtn.Scene);

                    break;
                case 2:
                    if (PanelManager.IsCurrentPanel(PanelManager.handbookPanel) == false)
                    {
                        //if (PanelManager.handbookPanel != null)
                        //{
                        //    PanelManager.CurrentPanel.Close();
                        //}
                        PanelManager.HandbookPanel.Show();
                        
                    }

                    ShowLeftBtns(EnumConfig.SystemMuneBtn.Treasure);

                    break;
                case 3:
                    if (PanelManager.IsCurrentPanel(PanelManager.picturesPanel) == false)
                    {
                        //if (PanelManager.picturesPanel != null)
                        //{
                        //    PanelManager.CurrentPanel.Close();
                        //}
                        PanelManager.PicturesPanel.Show();

                    }

                    ShowLeftBtns(EnumConfig.SystemMuneBtn.Null);

                    break;
                case 4:
                    //if (PanelManager.IsCurrentPanel(PanelManager.settingPanel) == false)
                    //{

                    //    PanelManager.SettingPanel.Show();
                    //    //PanelManager.ShowTipString("设置功能暂未开放", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                    //}


                    //ShowLeftBtns(EnumConfig.SystemMuneBtn.Null);

                    if (PanelManager.IsCurrentPanel(PanelManager.taskPanel) == false)
                    {

                        PanelManager.TaskPanel.Show();
                    }


                    ShowLeftBtns(EnumConfig.SystemMuneBtn.Null);
                    break;
                case 5:
                    if (PanelManager.IsCurrentPanel(PanelManager.treasurPanel) == false)
                    {
                        //if (PanelManager.treasurPanel != null)
                        //{
                        //    PanelManager.CurrentPanel.Close();
                        //}
                        PanelManager.TreasurePanel.Show();
                    }

                    break;
                case 6:
                    if (PanelManager.IsCurrentPanel(PanelManager.sceneChangePanel) == false)
                    {
                        //if (PanelManager.sceneChangePanel != null)
                        //{
                        //    PanelManager.CurrentPanel.Close();
                        //}
                        PanelManager.SceneChangePanel.Show();
                    }

                    break;
                default:
                    break;
            }
        }

        public void ShowLeftBtns(EnumConfig.SystemMuneBtn index)
        {
            if (index == EnumConfig.SystemMuneBtn.Scene)
            {
                btns[(int)EnumConfig.SystemMuneBtn.Treasure].ShowActive(false);
                //btns[(int)EnumConfig.SystemMuneBtn.Scene].ShowActive(true);
                btns[(int)EnumConfig.SystemMuneBtn.Scene].ShowActive(BagManager.Instance.HasHouseExtensionItem());
            }
            else if (index == EnumConfig.SystemMuneBtn.Treasure)
            {
                btns[(int)EnumConfig.SystemMuneBtn.Scene].ShowActive(false);
                btns[(int)EnumConfig.SystemMuneBtn.Treasure].ShowActive(GuideManager.Instance.HasGettedOneTreasure);
            }
            else
            {
                btns[(int)EnumConfig.SystemMuneBtn.Scene].ShowActive(false);
                btns[(int)EnumConfig.SystemMuneBtn.Treasure].ShowActive(false);
            }
        }
    }
}
