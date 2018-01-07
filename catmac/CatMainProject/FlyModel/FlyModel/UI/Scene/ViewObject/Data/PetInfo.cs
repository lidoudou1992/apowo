using FlyModel.UI.Scene.Data;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyModel.UI.Scene.ViewObject.Data
{
    public class PetInfo
    {
        public string Name;
        //public List<PetAnimation> animationList = new List<PetAnimation>();
        public Dictionary<string, PetAnimation> animationList = new Dictionary<string, PetAnimation>();

        private JsonData petInfoJsonData;

        public PetInfo(JsonData jsonData)
        {
            petInfoJsonData = jsonData;
            parseConfig();
        }

        private void parseConfig()
        {
            Name = petInfoJsonData["CatName"].ToString();
            string key = ToyGameObject.TOY_CONFIG_KEY_DEPTH[5];
            int animCount = petInfoJsonData[key].Count;
            JsonData tempJsonData;
            PetAnimation temp;
            for (int i = 0; i < animCount; i++)
            {
                tempJsonData = petInfoJsonData[key][i];
                temp = new PetAnimation(tempJsonData);
                //animationList.Add(temp);
                animationList.Add(temp.Name, temp);
            }
        }
    }
}
