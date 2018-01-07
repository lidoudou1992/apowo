using FlyModel.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Component.PageView
{
    public interface IItem
    {
        void Init(BaseProp data, int pageIndex, int itemIndex);
    }
}
