using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HotFixPanel : MonoBehaviour {

    private GameObject ProgressGo;
    private Slider ProgressBar;
    private Text ProgressInfo;

    private GameObject Alert;
    private Text AlertText;
    private Button OKBtn;
    private Button CancleBtn;

    private Text titleTF;
    private Text OKBtnTF;
    private Text CancleBtnTF;

    void Awake()
    {
        ProgressGo = transform.Find("ProgressInfo").gameObject;
        ProgressBar = ProgressGo.transform.Find("Slider").GetComponent<Slider>();
        ProgressInfo = ProgressGo.transform.Find("Text").GetComponent<Text>();

        Alert = transform.Find("Alert").gameObject;
        AlertText = Alert.transform.Find("Text").GetComponent<Text>();
        OKBtn = Alert.transform.Find("OK").GetComponent<Button>();
        OKBtnTF = Alert.transform.Find("OK/Text").GetComponent<Text>();
        CancleBtn = Alert.transform.Find("Cancle").GetComponent<Button>();
        CancleBtnTF = Alert.transform.Find("Cancle/Text").GetComponent<Text>();

        titleTF = transform.Find("Alert/Image/Text").GetComponent<Text>();
    }

    public void SetOneBtnMode(string title, string str, string btnStr, Action OKCallback)
    {
        CancleBtn.gameObject.SetActive(false);
        titleTF.text = title;
        AlertText.text = str;
        OKBtn.transform.localPosition = new Vector3(0, -140, 0);

        OKBtnTF.text = btnStr;
        Alert.SetActive(true);
        OKBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(OKCallback));
    }

    public void SetTwoBtnMode(string title, string str, string leftBtnStr, Action leftCallback, string rightBtnStr, Action rightCallback)
    {
        titleTF.text = title;
        AlertText.text = str;

        Alert.SetActive(true);

        OKBtnTF.text = leftBtnStr;
        OKBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(leftCallback));

        CancleBtnTF.text = rightBtnStr;
        CancleBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(rightCallback));
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetAlert(string alertString, Action OKCallback, Action cancleCallback)
    {
        Alert.SetActive(true);
        AlertText.text = alertString;
        OKBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(OKCallback));
        CancleBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(cancleCallback));
    }

    public void CloseAlert()
    {
        Alert.SetActive(false);
    }


    public void SetActive()
    {
        if (transform.gameObject.activeSelf == false)
        {
            transform.gameObject.SetActive(true);
        }
    }

    public void SetProgressInfoActive()
    {
        ProgressGo.SetActive(true);
    }

    public void UpdateInfo(string info, float progressVal)
    {
        if (ProgressInfo)
        {
            ProgressInfo.text = info;
            ProgressBar.value = progressVal;
        }
    }
}
