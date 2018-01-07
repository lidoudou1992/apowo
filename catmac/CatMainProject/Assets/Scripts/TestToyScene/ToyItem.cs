using UnityEngine;
using System.Collections;
using Assets.Scripts.TestToyScene;
using UnityEngine.UI;
using LitJson;

public class ToyItem : MonoBehaviour {

    private GameObject toyContainer;
    private TestCatAnimationData catAnimationData;
    private GameObject toyGO;
    private GameObject catContainer;

    private Text pathTF;
    private Text actionTF;

    void Awake()
    {
        toyContainer = transform.Find("ToyContainer").gameObject;
        pathTF = transform.Find("Path").GetComponent<Text>();
        actionTF = transform.Find("Action").GetComponent<Text>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowToy(TestCatAnimationData animationData)
    {
        catAnimationData = animationData;

        string currentToyName = GameObject.Find("Canvas/Panel").GetComponent<ToyPreviewPanel>().CurrentToyName;
        ResourceLoader.Instance.TryLoadClone(currentToyName.ToLower(), currentToyName, (go) =>
        {
            toyGO = go;
            go.transform.SetParent(toyContainer.transform, false);

            showCat();
        });
    }

    private void showCat()
    {
        pathTF.text = catAnimationData.CatData.GetCatPath();
        actionTF.text = catAnimationData.CatData.Name + "/" + catAnimationData.Name + "/" + catAnimationData.ShadowName;

        ResourceLoader.Instance.TryLoadClone(catAnimationData.CatData.Name.ToLower(), catAnimationData.CatData.Name, (catGO) =>
        {
            Transform parent = toyGO.transform.Find(catAnimationData.CatData.GetCatPath());

            catContainer = new GameObject("CatContainer");
            catContainer.transform.SetParent(parent, false);

            catGO.transform.SetParent(catContainer.transform, false);

            catGO.transform.localPosition = new Vector3(catAnimationData.X, catAnimationData.Y, 0);
            catGO.transform.localScale = new Vector3(catAnimationData.ScaleX, catAnimationData.ScaleY, 1);
            catGO.transform.localRotation = Quaternion.Euler(0, 0, catAnimationData.RotationZ);

            setToyLayersActive();
            var sg = catGO.GetComponent<SkeletonGraphic>();
            sg.AnimationState.SetAnimation(0, catAnimationData.Name, true);

            showShadow();
        });
    }

    private void setToyLayersActive()
    {
        int count = toyGO.transform.childCount;
        GameObject tempToyLayer;
        JsonData hideLayers;
        GameObject tempToyPic = null;
        bool flag = true;
        bool useLog = false;
        for (int i = 0; i < count; i++)
        {
            flag = true;
            tempToyLayer = toyGO.transform.GetChild(i).gameObject;

            //if (actionTF.text == "catani019/action1302/shou_2051yz" && pathTF.text== "ToyLayer_1/PlayPointLayer_1/Point_1")
            //{
            //    useLog = true;
            //}

            hideLayers = catAnimationData.CatData.PlayPointData.HideToyLayers;

            for (int j = 0; j < hideLayers.Count; j++)
            {
                if (hideLayers[j].ToString() == tempToyLayer.name)
                {
                    flag = false;
                    break;
                }
            }

            tempToyLayer.SetActive(flag);

            if (flag)
            {
                int toyCount = tempToyLayer.transform.childCount;
                for (int k = 0; k < toyCount; k++)
                {
                    tempToyPic = tempToyLayer.transform.GetChild(k).gameObject;
                    if (tempToyPic.name.Contains("Toy"))
                    {
                        if (tempToyPic.GetComponent<SkeletonGraphic>() != null)
                        {
                            tempToyPic.GetComponent<SkeletonGraphic>().freeze = false;
                            tempToyPic.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "play", true);
                        }
                    }
                }
            }
        }

        Transform stopLayer = toyGO.transform.Find("ToyLayer_stop");
        if (stopLayer != null)
        {
            stopLayer.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError(string.Format("{0} 玩具没有stop层", toyGO.name));
        }
    }

    private void showShadow()
    {
        if (string.IsNullOrEmpty(catAnimationData.ShadowName) == false)
        {
            ResourceLoader.Instance.TryLoadPic("catshadowpics", catAnimationData.ShadowName, (shadowPic) => {

                GameObject shadow = new GameObject("Shadow", typeof(Image));
                shadow.transform.SetParent(catContainer.transform, false);

                shadow.transform.SetAsFirstSibling();
                Image image = shadow.GetComponent<Image>();
                image.sprite = shadowPic as Sprite;
                image.SetNativeSize();

                shadow.transform.localPosition = new Vector3(catAnimationData.ShadowX,catAnimationData.ShadowY, 0);
                shadow.transform.localScale = new Vector3(catAnimationData.ShadowScaleX, catAnimationData.ShadowScaleY, 1);
            });
        }
    }
}
