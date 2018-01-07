using UnityEngine;
using System.Collections;
using Together;

public class YoMobController : MonoBehaviour
{

    void Awake()
    {
        // 开启Debug模式
        TGSDK.SetDebugModel(true);

        // 宏判断里初始化TGSDK
#if UNITY_IOS && !UNITY_EDITOR
        // 猫宅日记苹果版AppID
        TGSDK.Initialize("1z525lf2gLH88790fQNZ");
#elif UNITY_ANDROID && !UNITY_EDITOR
        // 猫宅日记安卓版AppID
        TGSDK.Initialize("Hg6513A4774EnEf53KTy");
#endif
        // 预加载广告
        TGSDK.PreloadAd();
    }
}
