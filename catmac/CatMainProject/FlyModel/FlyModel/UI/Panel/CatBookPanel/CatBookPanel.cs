using FlyModel.Model;
using FlyModel.UI.Component;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.CatBookPanel
{
    public class CatBookPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "CatBookPanel";
            }
        }

        public TableView tableViewScript;
        private CatBookController contentController;
        public List<Hashtable> animations = new List<Hashtable>();

        public override void Load()
        {
            base.Load();

            PanelManager.LoadSystemBar(transform);
        }

        protected override void Initialize(GameObject go)
        {
            GameObject tableView = go.transform.Find("TableView").gameObject;
            tableViewScript = new TableView(); //tableView.AddComponent<TableView>();

            //contentController = tableView.AddComponent<CatBookController>();
            contentController = new CatBookController();
            contentController.tableViewScript = tableViewScript;

            //GameObject tableItemCellPrefab = go.transform.Find("BookCellItem").gameObject;
            //tableItemCellPrefab.AddComponent<BookCellItem>();
            BookCellItem tableItemCellPrefab = new BookCellItem(go.transform.Find("BookCellItem").gameObject);
            contentController.cellPrefab = tableItemCellPrefab.GameObject;

            parseConfig();

            PanelManager.BringSystemBarToTop();
        }

        private void parseConfig()
        {
            ResourceLoader.Instance.TryLoadTextAsset(ResPathConfig.CATS_ANIMATIONS_CONFIG, (textAssert) =>
            {
                JsonData config = JsonMapper.ToObject((textAssert as TextAsset).text);

                JsonData allCats = config["Cats"];
                JsonData appointCats = config["AppointCat"];
                List<JsonData> List = new List<JsonData>();
                
                JsonData tempCat;
                JsonData tempCatAnimations;
                for (int i = 0; i < allCats.Count; i++)
                {
                    tempCat = allCats[i];
                    for (int j = 0; j < appointCats.Count; j++)
                    {
                        if (tempCat["name"].ToString() == appointCats[j].ToString())
                        {
                            List.Add(tempCat);

                            tempCatAnimations = tempCat["animations"];
                            for (int k = 0; k < tempCatAnimations.Count; k++)
                            {
                                var tempAnimation = new Hashtable();
                                tempAnimation["spineName"] = tempCat["name"];
                                tempAnimation["actionName"] = tempCatAnimations[k].ToString();
                                animations.Add(tempAnimation);
                            }
                        }
                    }
                }
                contentController.numRows = animations.Count;
                contentController.animations = animations;
                contentController.Refresh();
            });
        }

        public override void Refresh()
        {
            base.Refresh();

            //contentController.Refresh();
        }

        public override void SetInfoBar()
        {
            base.SetInfoBar();

            PanelManager.InfoBar.SetMenuClickedHandler(() => { Close(); });
            PanelManager.InfoBar.SetBtnImage(EnumConfig.InfoBarBtnMode.Close);
        }
    }
}
