using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Scene.ViewObject.Data;
using UnityEngine;

namespace FlyModel.UI.Scene.ViewObject.SceneViewObject
{
    public class FoodPointGameObject : InteractivePoint
    {
        public ScenePointStruct pointStruct;

        public FoodPointGameObject(GameObject point, int foodPointIndex) : base(point)
        {
            pointStruct = new ScenePointStruct();
            pointStruct.ScenePointIndex = foodPointIndex;
            pointStruct.SubPointIndex = 1;
            pointStruct.PointType = EnumConfig.SubPointType.small;
            pointStruct.Distribution = (EnumConfig.InteractivePointDistibution)SceneManager.CURRENT_FOOD_POINT_DISTRIBUTION[foodPointIndex];

            //MarkPointIdentity markPointIdentity = Root.AddComponent<MarkPointIdentity>();
            //markPointIdentity.PointStruct = pointStruct;

            initPointMarks();
        }

        private void initPointMarks()
        {
            ClosePointMark();
        }

        public override void ShowAvailable(BagItemData data)
        {
            base.ShowAvailable(data);

            Root.transform.Find("Food").gameObject.SetActive(true);
        }

        public override void ClosePointMark()
        {
            base.ClosePointMark();

            Root.transform.Find("Food").gameObject.SetActive(false);
        }

        public void AddFoodChild(GameObject go)
        {
            go.transform.SetParent(Root.transform, false);
            Root.SetActive(true);
            ClosePointMark();

            go.transform.SetSiblingIndex(0);
        }
    }
}
