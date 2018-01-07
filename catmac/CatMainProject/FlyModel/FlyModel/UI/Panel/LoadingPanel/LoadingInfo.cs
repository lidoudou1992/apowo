using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.LoadingPanel
{
    public class LoadingInfo
    {
        public EnumConfig.LoadingType Type;
        public Type AssetType;
        public string AssetName;
        public Action<UnityEngine.Object> LoadOverCallback;
    }
}
