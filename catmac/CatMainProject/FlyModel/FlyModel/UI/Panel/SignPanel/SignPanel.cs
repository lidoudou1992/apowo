using FlyModel.Model;
using FlyModel.UI.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.SignPanel
{
    public class SignPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "SignInPanel";
            }
        }

        public override bool IsRoot
        {
            get
            {
                return true;
            }
        }

        private Text signDaysTF;
        private Text signButtonTF;

        private List<SignCell> cellGOList = new List<SignCell>();

        protected override void Initialize(GameObject go)
        {
            GameObject cellContainer = go.transform.Find("Imagebg/CellItem").gameObject;

            signDaysTF = go.transform.Find("Imagebg/Text/Text").GetComponent<Text>();
            signButtonTF = go.transform.Find("Imagebg/Button/Text").GetComponent<Text>();

            new SoundButton(go.transform.Find("Imagebg/Button").gameObject, () =>
            {
                signDirectly();
            });

            Transform tempCell = null;
            SignCell tempSignCell = null;
            for (int i = 0; i < 7; i++)
            {
                tempCell = cellContainer.transform.GetChild(i);
                tempSignCell = new SignCell(tempCell.gameObject);
                tempSignCell.Index = i + 1;
                cellGOList.Add(tempSignCell);
            }
        }

        public override void Refresh()
        {
            base.Refresh();

            signDaysTF.text = UserManager.Instance.Me.signDays.ToString();

            signButtonTF.text = UserManager.Instance.Me.todayHasSigned ? "今日已签到" : "领取奖励";

            SignCell tempCell;
            for (int i = 0; i < 7; i++)
            {
                tempCell = cellGOList[i];
                tempCell.UpdateState();
            }
        }

        public override void SetInfoBar()
        {
            
        }

        private void signDirectly() {
            if (UserManager.Instance.Me.todayHasSigned==false)
            {
                foreach (var cell in cellGOList)
                {
                    if (cell.State == EnumConfig.SignState.canSign)
                    {
                        cell.StartSign();
                        return;
                    }
                }
            }
        }
    }
}
