using FlyModel.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.CatBookPanel
{
    public class BookCellItem: TableViewCell
    {

        private string catName;
        private string actionName;
        private Text nameTF;
        private Text actionTF;

        private GameObject catSpine;

        public BookCellItem(GameObject go):base(go)
        {
            nameTF = go.transform.Find("CatName").GetComponent<Text>();
            actionTF = go.transform.Find("ActionName").GetComponent<Text>();
        }

        public void updateData(Hashtable data)
        {
            catName = data["spineName"].ToString();
            actionName = data["actionName"].ToString();
            nameTF.text = catName;
            actionTF.text = actionName;

            createSpine();
        }

        private void createSpine()
        {
            if (catSpine != null)
            {
                catSpine.SetActive(false);
            }

            ResourceLoader.Instance.TryLoadClone(catName.ToLower(), catName, (go) =>
            {
                if (go.name == catName+"(Clone)")
                {
                    if (catSpine != null)
                    {
                        GameObject.DestroyImmediate(catSpine);
                        catSpine = null;
                    }

                    catSpine = go;
                    go.SetActive(false);
                    go.transform.SetParent(GameObject.transform, false);
                    go.transform.localPosition = new Vector3(110, -70, 0);

                    SkeletonGraphic skeletonGrapic = catSpine.GetComponent<SkeletonGraphic>();
                    skeletonGrapic.AnimationState.SetAnimation(0, actionName, true);

                    go.SetActive(true);
                }
            });
        }
    }
}
