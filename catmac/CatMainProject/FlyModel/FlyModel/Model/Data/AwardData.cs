using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model.Data
{
    public class AwardData:BaseProp
    {
        public Proto.AwardType AwardType;
        public string CatCode;
        public string ToyCode;

        public void UpdateData(Proto.AwardData data)
        {
            ID = data.Id;
            AwardType = data.Type;
            Count = data.Count;
            Name = data.CatName;
            CatCode = data.CatCode;
            ToyCode = data.FurniCode;
        }
    }
}
