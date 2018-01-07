using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;

public class advertisementcover : MonoBehaviour
{
    private Button AdvertisButton001_1;
    private Button AdverTisButton001_2;
    private Image AdvertisImage001;
    private Transform Advertising001;
    private Image AdvertisButton001_1_Image;
    //处于等待界面的一系列动作
    private Text times;
    private float waitTime = 5f;
    private Button SkipButton;
    //控制广告的显示
    //private bool _bAdvertising001 = true;
    //private bool _bAdvertising002 = false;
    // private bool _bAdvertising003 = false;
    //当下的图片的一些信息
    private string nowName = " ";
    private string nowIcon = " ";//图标
    private string nowBanner = " ";//横幅图片的广告
    private string nowCover = " ";//处在封面
    private string nowAppleID = " ";
    private string nowAndroidName = " ";
    //盛放json数据的一些容器
    private List<string> nameList = new List<string>();//产品名字
    private List<string> iconList = new List<string>();//icon图片的地址
    private List<string> bannerList = new List<string>();//banner图片的地址
    private List<string> coverList = new List<string>();//封面图片的地址
    private List<string> appleIDList = new List<string>();//苹果的应用ID
    private List<string> androidNameList = new List<string>();//安卓的应用ID
    //用于上传的数据
    Dictionary<string, string> adsbanner = new Dictionary<string, string>();
    Dictionary<string, string> adscover = new Dictionary<string, string>();
    Dictionary<string, string> adsicon1 = new Dictionary<string, string>();
    Dictionary<string, string> adsicon2 = new Dictionary<string, string>();
    Dictionary<string, string> adsicon3 = new Dictionary<string, string>();

    private List<string> ID = new List<string>();
    private List<string> text = new List<string>();
    //盛放图片的临时图片
    private Texture2D texturePhoto = null;
    // 要访问的广告数据的网址的前缀
    private string websitePrefix = "";
    private int adId = 0;
    private string selfPlatform = "";
    private string adWebsite = "";
    //对按钮显示进行判断
    // private bool IsXianshi=false;
    public bool isKong = false;
    //用于判断当前设备是安卓或者IOS
    private string systemname = "安卓";
    //统计数据时候的两个url
    private string adsShow = "https://tuqing.apowo.com/server/ads/adsShow";
    private string adsPress = "https://tuqing.apowo.com/server/ads/adsPress";
    //获取当前的设备ID
    private string devicelID = "66666666666";
    public bool isicon;
    // Use this for initialization
    void Start()
    {
        GetTime();
        GetdeviceID();
        SystemName();
        StartCoroutine(GetWebsiteData());

        //用来使用户关闭广告的显示       
        times = transform.FindChild("Advertising001/TimeText").GetComponent<Text>();
        SkipButton = transform.FindChild("Advertising001/Button3").GetComponent<Button>();
        SkipButton.onClick.AddListener(OnClickButton3);

        AdverTisButton001_2 = transform.FindChild("Advertising001/Button2").GetComponent<Button>();
        AdverTisButton001_2.onClick.AddListener(OnClickAdvertisButton001_1);
        Advertising001 = transform.FindChild("Advertising001").GetComponent<Transform>();
        //需要改变的一系列button图片
        AdvertisButton001_1_Image = transform.FindChild("Advertising001/Button2").GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        Wait();
        IsClose();
    }

    //获取当前的设备ID
    private void GetdeviceID()
    {

        devicelID = TalkingDataGA.GetDeviceId();
        Debug.Log(devicelID);
        if (devicelID == null)
        {
            devicelID = "Eidor";
        }
    }

    //随时对时间进行刷新
    private void GetTime()
    {
        string t = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        int M = System.DateTime.Now.Millisecond;
        // systemTime = t + M.ToString();
    }

    //用于检测当前设备
    private void SystemName()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            systemname = "android";
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            systemname = "ios";
        }
        else
        {
            systemname = "android";
        }
    }

    //用来跳转到应用商店
    private void TiaoZhuan(string nameself1)
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string[] sArray = nameself1.Split('.');
            Debug.Log(sArray[0] + "))))))))))))))");
            if (sArray[0] == "com")
            {

                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("isAppInstalled", nameself1);
            }
            else
            {
                Application.OpenURL(nameself1);
            }
        }
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            Debug.Log(nameself1);
            string url = "itms-apps://" + nameself1;
            //IOSadvertise(nameself1);
            Application.OpenURL(url);
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log(nameself1);
            string url = "itms-apps://" + nameself1;
            //IOSadvertise(nameself1);
            Application.OpenURL(url);

        }
    }

    //点击跳过时
    private void OnClickButton3()
    {
        Advertising001.gameObject.SetActive(false);
    }
    //点击封面时
    private void OnClickAdvertisButton001_1()
    {
        StartCoroutine(Post(adsPress, adscover));
        Advertising001.gameObject.SetActive(false);
        TiaoZhuan(nowName);

    }
    // 得到要访问的广告的网址的前缀
    private IEnumerator GetWebsiteData()
    {
        WWW www = new WWW("http://snpole.com/ads.json");
        yield return www;
        if (www.isDone && www.error == null)
        {
            JsonData jsonData = JsonMapper.ToObject(www.text);
            websitePrefix = jsonData["api"].ToString();
            GetAdWebsite();
        }
    }

    // 得到要访问的广告地址
    private void GetAdWebsite()
    {
        if (websitePrefix != "")
        {
            adId = 5;
            if (Application.platform == RuntimePlatform.Android)
            {
                selfPlatform = "1";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                selfPlatform = "0";
            }
            else
            {
                selfPlatform = "1";  // 编辑器里时模拟安卓
            }
            adWebsite = websitePrefix + "/" + adId + "/" + selfPlatform;
            StartCoroutine(GetAdData(adWebsite));
        }
        else
        {
            Debug.Log("没有得到广告网址前缀");
        }
    }

    // 取得广告数据
    private IEnumerator GetAdData(string website)
    {
        WWW www = new WWW(website);
        yield return www;
        if (www.isDone && www.error == null)
        {
            JsonData jsonData = JsonMapper.ToObject(www.text);
            JsonData dataJsonData = jsonData["data"];
            SaveData(dataJsonData);
        }
    }

    //用来将json文件存储起来
    private void SaveData(JsonData jd)
    {
        foreach (JsonData temp in jd as IList)
        {
            nameList.Add(temp["name"].ToString());
            iconList.Add(temp["icon"].ToString());
            bannerList.Add(temp["banner"].ToString());
            coverList.Add(temp["cover"].ToString());
            appleIDList.Add(temp["appleID"].ToString());
            androidNameList.Add(temp["androidName"].ToString());
            JsonData modeJsonData = temp["modes"];
            foreach (JsonData modeTemp in modeJsonData as IList)
            {
                Debug.Log(modeTemp["text"].ToString());
                ID.Add(modeTemp["id"].ToString());
                text.Add(modeTemp["text"].ToString());
            }
        }
        if (nameList.Count == 0)
        {
            isKong = true;
        }
        else
        {
            isKong = false;
        }
        //当数据为空时，关闭所有广告   
        RePhoto();
        //上传Show的数据
        foreach (var item in adsbanner)
        {
            Debug.Log(item.Key + item.Value);
        }
        if (isKong == false)
        {
            // StartCoroutine(Post(adsPress,adsbanner));
            //StartCoroutine(Post(adsPress,adsbanner));
            StartCoroutine(Post(adsShow, adscover));
            Debug.Log("封面上的button已经上传完毕’‘’‘’‘’‘’‘’‘’");
            if (isicon == true)
            {
                // StartCoroutine(adsPress,adscover);
                StartCoroutine(Post(adsShow, adsbanner));
            }
           
            Debug.Log("数据上传完毕！！！！！！！！！！！！！！！");
        }
    }

    //开始加载图片
    private void RePhoto()
    {
        JudgeName();
        ReplacePhoto();
    }

    //通过判断名字来进行显示
    public void JudgeName()
    {
        int i = Random.Range(0, nameList.Count);
        //选择平台
        if (nameList.Count != 0)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                nowName = androidNameList[i];
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {

                nowName = appleIDList[i];

            }
            else
            {
                nowName = androidNameList[i];  // 编辑器里时模拟安卓
            }
            nowIcon = iconList[i];
            nowBanner = bannerList[i];
            nowCover = coverList[i];
            adsbanner.Add("productID", "1");
            // adsbanner.Add("time", systemTime);
            adsbanner.Add("system", systemname);
            adsbanner.Add("deviceID", devicelID);
            adsbanner.Add("adsName", nameList[i]);
            adsbanner.Add("adsType", "2");
            adscover.Add("productID", "1");
            // adscover.Add("time", systemTime);
            adscover.Add("system", systemname);
            adscover.Add("deviceID", devicelID);
            adscover.Add("adsName", nameList[i]);
            adscover.Add("adsType", "1");
            foreach (var item in adsbanner)
            {
                Debug.Log(item);
            }
        }
    }

    //更换广告的图片
    private void ReplacePhoto()
    {
        if (isKong == false)
        {
            StartCoroutine(WaitLoad(nowCover, AdvertisButton001_1_Image));
        }
    }

    //加载图片
    IEnumerator WaitLoad(string fileName, Image image)
    {
        double startTime = (double)Time.time;
        string path = " ";
        if (Application.platform == RuntimePlatform.Android)
        {
            path = fileName;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            path = fileName;
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            path = fileName;
        }
        WWW wwwTexture = new WWW(path);
        yield return wwwTexture;
        texturePhoto = wwwTexture.texture;
        Sprite sprite = Sprite.Create(texturePhoto, new Rect(0, 0, texturePhoto.width, texturePhoto.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
        startTime = (double)Time.time - startTime;
    }

    //判断有无数据并关闭
    public void IsClose()
    {
        if (isKong == true)
        {
            Advertising001.gameObject.SetActive(false);
        }
    }

    //开机前等待的五秒钟
    private void Wait()
    {
        waitTime -= Time.deltaTime;
        int a = (int)waitTime;
        times.text = a.ToString();
        if (waitTime <= 0.5)
        {
            Advertising001.gameObject.SetActive(false);
        }
    }


    /// <summary>  
    /// 指定Post地址使用Get 方式获取全部字符串  
    /// </summary>  
    /// <param name="url">请求后台地址</param>  
    /// <returns></returns>  
    public static IEnumerator Post(string url, Dictionary<string, string> dic)
    {

        WWWForm form = new WWWForm();
        //从集合中取出所有参数，设置表单参数（AddField()).  
        foreach (KeyValuePair<string, string> post_arg in dic)
        {
            form.AddField(post_arg.Key, post_arg.Value);
            Debug.Log(post_arg.Key + "=" + post_arg.Value);
        }
        WWW www = new WWW(url, form);
        yield return www;
        if (www.error != null)
        {
            //POST请求失败  
            //mContent = "error :" + www.error;
            Debug.Log("error : " + www.error);
        }
        else
        {
            //POST请求成功  
            Debug.Log(www.text);
        }
    }

}