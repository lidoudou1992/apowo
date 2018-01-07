using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;
using System.Collections.Generic;
using Assets.Scripts.TestToyScene;

public class ToyPreviewPanel : MonoBehaviour {

    private Dropdown dropDown;
    public string CurrentToyName;
    private JsonData currentToyConfig;
    private List<TestToyLayerData> toyLayerDatas = new List<TestToyLayerData>();

    private List<TestCatAnimationData> animationDatasList = new List<TestCatAnimationData>();

    private GameObject itemPrefab;
    private GameObject scrollerContent;

    void Awake()
    {
        dropDown = transform.Find("Dropdown").GetComponent<Dropdown>();
        scrollerContent = transform.Find("Scroll View/Content").gameObject;
        itemPrefab = transform.Find("ToyItem").gameObject;
    }

	// Use this for initialization
	void Start () {
        initToys();
    }
	
    private void initToys()
    {
        string toysConfig = "FurnisConfig";
        ResourceLoader.Instance.TryLoadTextAsset(toysConfig, (textAssert) =>
        {
            JsonData config = JsonMapper.ToObject((textAssert as TextAsset).text);

            List<JsonData> furniConfig = new List<JsonData>();
            for (int i = 0; i < config.Count; i++)
            {
                furniConfig.Add(config[i]);
            }
            furniConfig.Sort((JsonData a, JsonData b) =>
            {
                if ((int)a["ID"]<(int)b["ID"])
                {
                    return -1;
                }
                else if((int)a["ID"] > (int)b["ID"])
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });


            dropDown.ClearOptions();
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

            JsonData temp;
            for (int i = 0; i < furniConfig.Count; i++)
            {
                temp = furniConfig[i];

                if (temp["Code"] == null)
                {
                    options.Add(new Dropdown.OptionData() { text = string.Format("ToyRoot_Toy{0}", temp["ID"].ToString()) });
                    Debug.LogWarning(string.Format("ID为 {0} 的玩具 Code字段未设置！！！", temp["ID"].ToString()));
                }
                else
                {
                    options.Add(new Dropdown.OptionData() { text = string.Format("ToyRoot_{0}", temp["Code"].ToString()) });
                }
            }

            dropDown.AddOptions(options);

            SetCurrentToy();
        });
    }

    private void clear()
    {
        toyLayerDatas.Clear();
        animationDatasList.Clear();
        int count = scrollerContent.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            DestroyImmediate(scrollerContent.transform.GetChild(0).gameObject);
        }
    }

    public void SetCurrentToy()
    {
        clear();

        CurrentToyName = dropDown.options[dropDown.value].text;

        ResourceLoader.Instance.TryLoadTextAsset(string.Format("{0}Config", CurrentToyName).ToLower(), (textAssert) =>
        {
            currentToyConfig = JsonMapper.ToObject((textAssert as TextAsset).text);

            parseToyConfig();
            getToyAllAnimations();
            showToy();
        });
    }

    private void showToy()
    {
        GameObject temp;
        for (int i = 0; i < animationDatasList.Count; i++)
        {
            temp = Instantiate(itemPrefab);
            temp.GetComponent<ToyItem>().ShowToy(animationDatasList[i]);
            temp.SetActive(true);
            temp.transform.SetParent(scrollerContent.transform, false);
        }
    }

    private void parseToyConfig()
    {
        JsonData toyLayers = currentToyConfig["ToyLayers"];
        for (int i = 0; i < toyLayers.Count; i++)
        {
            toyLayerDatas.Add(new TestToyLayerData(toyLayers[i]));
        }
    }

    private void getToyAllAnimations()
    {
        TestToyLayerData tempToyLayer;
        TestPlayPointLayerData tempPlayPointLayer;
        TestPointData tempPlayPoint;
        TestCatData tempCat;
        TestCatAnimationData tempAnimation;
        for (int i = 0; i < toyLayerDatas.Count; i++)
        {
            tempToyLayer = toyLayerDatas[i];
            
            for (int j = 0; j < tempToyLayer.playPointLayersData.Count; j++)
            {
                tempPlayPointLayer = tempToyLayer.playPointLayersData[j];

                for (int k = 0; k < tempPlayPointLayer.pointDatas.Count; k++)
                {
                    tempPlayPoint = tempPlayPointLayer.pointDatas[k];

                    for (int l = 0; l < tempPlayPoint.catDatas.Count; l++)
                    {
                        tempCat = tempPlayPoint.catDatas[l];

                        for (int m = 0; m < tempCat.animationDatas.Count; m++)
                        {
                            tempAnimation = tempCat.animationDatas[m];

                            animationDatasList.Add(tempAnimation);
                        }
                    }
                }
            }
        }
    }
}
