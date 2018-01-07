using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Panel.GuidePanel;
using FlyModel.UI.Scene.ViewObject.Data;
using UnityEngine;

namespace FlyModel.UI.Scene.ViewObject.SceneViewObject
{
    public class ToyMarkPointGameObject : InteractivePoint
    {
        public ScenePointStruct PointStruct;
        private GameObject availableGO;
        private GameObject forbidGO;

        //private GameObject guideGesture;

        public ToyMarkPointGameObject(GameObject point, bool isSmallType, int scenePointIndex, int subPointIndex) : base(point)
        {
            PointStruct = new ScenePointStruct();
            PointStruct.ScenePointIndex = scenePointIndex;
            PointStruct.SubPointIndex = subPointIndex;
            PointStruct.PointType = isSmallType ? EnumConfig.SubPointType.small : EnumConfig.SubPointType.large;
            PointStruct.Distribution = (EnumConfig.InteractivePointDistibution)SceneManager.CURRENT_TOY_POINT_DISTRIBUTION[scenePointIndex];

            Root = point;

            //MarkPointIdentity markPointIdentity = Root.AddComponent<MarkPointIdentity>();
            //markPointIdentity.PointStruct = pointStruct;

            initPoint(point);
        }

        public void initPoint(GameObject point)
        {
            availableGO = point.transform.Find("Available").gameObject;
            if (PointStruct.PointType == EnumConfig.SubPointType.small)
            {
                forbidGO = point.transform.Find("Forbid").gameObject;
            }
            else if (PointStruct.PointType == EnumConfig.SubPointType.large)
            {

            }
        }

        public override void ShowAvailable(BagItemData data)
        {
            bool availableGOShowable = IsSmallPoint == data.IsSmallType;

            //availableGO.SetActive(IsSmallPoint == data.IsSmallType);
            availableGO.SetActive(availableGOShowable);

            if (forbidGO)
            {
                forbidGO.SetActive(isSmallGroup() && IsSmallPoint && !data.IsSmallType);
            }

            if (PointStruct.Distribution == EnumConfig.InteractivePointDistibution.In)
            {
                availableGOShowable = IsSmallPoint == data.IsSmallType && BagManager.Instance.HasHouseExtensionItem();
                availableGO.SetActive(availableGOShowable);
                //availableGO.SetActive(IsSmallPoint == data.IsSmallType && BagManager.Instance.HasHouseExtensionItem());
            }

            if (availableGOShowable && GuideManager.Instance.IsGuiding())
            {
                //if (guideGesture == null)
                //{
                //    guideGesture = GameObject.Instantiate(GuidePanel.GestureGO);
                //}
                //guideGesture.SetActive(true);
                //guideGesture.transform.SetParent(availableGO.transform, false);
                //guideGesture.transform.localPosition = new Vector3(26, 57, 0);
                //guideGesture.transform.localScale = new Vector3(-1, 1, 1);
                //guideGesture.transform.localRotation = Quaternion.Euler(0, 0, 148);
            }


            if (PointStruct.ScenePointIndex == 8)
            {
                Root.SetActive(TaskManager.Instance.IsSceneExtendPointAchievementGet);
            }
            else
            {
                Root.SetActive(true);
            }
        }

        private bool isSmallGroup()
        {
            Transform toyPoint = availableGO.transform.parent.parent;
            int count = toyPoint.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                if (toyPoint.GetChild(i).name.Contains("Large_"))
                {
                    return false;
                }
            }

            return true;
        }

        public override void ClosePointMark()
        {
            if (availableGO)
            {
                availableGO.SetActive(false);
            }
            if (forbidGO)
            {
                forbidGO.SetActive(false);
            }

            //if (guideGesture != null)
            //{
            //    GameObject.DestroyImmediate(guideGesture);
            //    guideGesture = null;
            //}
        }

        public void AddToyChild(GameObject toy)
        {
            toy.transform.SetParent(Root.transform, false);
            Root.SetActive(true);
            ClosePointMark();

            toy.transform.localPosition = availableGO.transform.localPosition;

            toy.transform.SetSiblingIndex(0);
        }
    }
}
