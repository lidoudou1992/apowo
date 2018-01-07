using FlyModel.Model;
using FlyModel.Model.Data;
using FlyModel.UI.Scene.Data;
using FlyModel.UI.Scene.ViewObject.Data;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Scene
{
    public class CatGameObject
    {
        public string CatName;
        public bool IsInScene;

        private PetAnimation animationInfo;

        public ToyStructInfo CatPathInfo;
        public ToyGameObject ToyGameObject;

        public GameObject Root;
        public GameObject CatSpine;

        public CatData CatData;

        private GameObject shadow;

        private bool isNeedDeleteSelfAsync;

        public CatGameObject(CatData data)
        {
            CatData = data;
            CatName = data.CatSpineName;
            CatPathInfo = data.CatPath;
            Debug.Log(data + "44444444444444444444444");
            Debug.Log(data.CatPath + "555555555555555555");

            IsInScene = true;
        }

        public void ShowCat()
        {
            loadRes();
        }

        private void loadRes()
        {
            ResourceLoader.Instance.TryLoadClone(CatName.ToLower(), CatName, (go) =>
            {
                createCat(go);
            });
        }

        private void createCat(GameObject catPrefab)
        {
            Debug.Log(catPrefab + "222222222222222222222");
            Debug.Log(CatPathInfo + "3333333333333333333");
            if (isNeedDeleteSelfAsync)
            {
                GameObject.DestroyImmediate(catPrefab);
                return;
            }

            //Debug.Log(string.Format("=======猫所属玩具的ID {0}", CatData.ToyID));
            ToyGameObject = SceneManager.Instance.CurrentScene.SceneGameObject.FindToyGameObjectByID(CatData.ToyID);
            //Debug.Log(ToyGameObject);
            //Debug.Log(CatData.ToyID);
            //Debug.Log(CatPathInfo);
            GameObject playPoint = ToyGameObject.FindPlayPoint(CatPathInfo);
            //Debug.Log(ToyGameObject.Root.name + " over==========");

            GameObject container = new GameObject(string.Format("{0}_Container", CatName.ToLower()));
            container.transform.SetParent(playPoint.transform, false);
            Root = container;

            shadow = new GameObject("Shadow", typeof(Image));
            shadow.SetActive(false);
            shadow.transform.SetParent(Root.transform);

            catPrefab.SetActive(false);
            catPrefab.transform.SetParent(container.transform, false);
            CatSpine = catPrefab;

            ToyGameObject.UpdateToyState(this);
            updateCatState();
            catPrefab.SetActive(true);
        }

        //设置动画的状态(猫的动画会影响玩具的动画状态，比如玩具的空置状态 和 有猫的状态)
        private void updateCatState()
        {
            randomOneAnimation();
        }

        private void randomOneAnimation()
        {
            PetInfo petInfo = ToyGameObject.GetOnePetInfo(CatName, CatPathInfo);
            if (petInfo==null)
            {
                Debug.Log(string.Format("出现了一种 玩具({0}) 不支持的猫({1})！！！", ToyGameObject.ToyName, CatName));
            }
            else
            {
                animationInfo = petInfo.animationList[CatData.Animation];
            }

            playAnimation();
        }

        private void playAnimation()
        {
            if (animationInfo!=null)
            {
                SkeletonGraphic spineGraphic = CatSpine.GetComponent<SkeletonGraphic>();
                spineGraphic.AnimationState.SetAnimation(0, animationInfo.Name, true);

                CatSpine.transform.localPosition = new Vector3(animationInfo.X, animationInfo.Y, 0);
                CatSpine.transform.localScale = new Vector3(animationInfo.ScaleX, animationInfo.ScaleY, 1);
                CatSpine.transform.localRotation = Quaternion.Euler(0, 0, animationInfo.RotationZ);

                if (string.IsNullOrEmpty(animationInfo.ShadowName)==false)
                {
                    ResourceLoader.Instance.TryLoadPic(ResPathConfig.CAT_SHADOW_ASSETBUNDLE, animationInfo.ShadowName, (shadowPic)=> {
                        Image image = shadow.GetComponent<Image>();
                        image.sprite = shadowPic as Sprite;
                        image.SetNativeSize();

                        shadow.transform.localPosition = new Vector3(animationInfo.ShadowX, animationInfo.ShadowY, 0);
                        shadow.transform.localScale = new Vector3(animationInfo.ShadowScaleX, animationInfo.ShadowScaleY, 1);
                        shadow.SetActive(true);
                    });
                }
                else
                {
                    shadow.SetActive(false);
                }
            }
        }

        public void DeleteSelf()
        {
            if (Root == null || ToyGameObject == null)
            {
                isNeedDeleteSelfAsync = true;
                return;
            }

            delete();
        }

        private void delete()
        {
            Object.Destroy(Root);

            bool toyHasCat = false;

            foreach (var catData in CatManager.Instance.CatDataList)
            {
                if (catData.ToyID == CatData.ToyID)
                {
                    toyHasCat = true;
                    break;
                }
            }

            if (toyHasCat == false)
            {
                ToyGameObject.ShowEmptyState();
            }
        }
    }

    
}
