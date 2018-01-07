using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Scene.ViewObject.SceneViewObject;
using UnityEngine;

namespace FlyModel.UI.Scene
{
    public class FoodGameObject:ToyGameObject
    {
        public FoodData FoodData;
        public string FoodName;

        public FoodGameObject(BaseProp data):base(data)
        {
            
        }

        public override void initData(BaseProp data)
        {
            FoodData = data as FoodData;
            PlaceIndexStruct = FoodData.ScenePointIndex;
            FoodName = FoodData.Name;
        }

        public void ShowFood()
        {
            loadRes();
        }

        public override void loadRes()
        {
            ResourceLoader.Instance.TryLoadClone(FoodName.ToLower(), FoodName, (go) =>
            {
                FoodPointGameObject foodGO = SceneManager.Instance.CurrentScene.SceneGameObject.FindFoodPointByIndex(PlaceIndexStruct);
                //Debug.Log(JsonMapper.ToJson(PlaceIndexStruct));
                GameObject toyPoint = foodGO.Root;
                foodGO.ClosePointMark();

                foodGO.AddFoodChild(go);

                //BaseIdentity identity = go.AddComponent<BaseIdentity>();
                //identity.Data = FoodData;

                go.SetActive(true);

                parseGameObject(go);
            });
        }

        protected override void parseGameObject(GameObject go)
        {
            Root = go;

            go.SetActive(false);

            updateProgress();
        }

        private void updateProgress()
        {
            GameObject temp;
            int count = Root.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                temp = Root.transform.Find(string.Format("Image_{0}", i + 1)).gameObject;
                temp.SetActive((int)FoodData.ProgressStep == (i + 1));
            }

            Root.SetActive(true);
        }
    }
}
