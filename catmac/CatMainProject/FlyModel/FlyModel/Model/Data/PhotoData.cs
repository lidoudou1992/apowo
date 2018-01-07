using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyModel.Model.Data
{
    public class PhotoData:BaseProp
    {
        public string Owner;
        public string PhotoName;
        //public string DateString;

        public UI.EnumConfig.PictureMode Mode = UI.EnumConfig.PictureMode.Cat;
    }
}
