using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.HotFixPanel
{
    public class HotFixPanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "HotFixPanel";
            }
        }

        private Text progressInfoTF;
        private Slider progressBar;

        protected override void Initialize(GameObject go)
        {
            progressInfoTF = go.transform.Find("Text").GetComponent<Text>();
            progressBar = go.transform.Find("Slider").GetComponent<Slider>();
        }

        public void UpdateInfo(string info, float progress)
        {
            progressInfoTF.text = info;
            progressBar.value = progress;
        }
    }
}
