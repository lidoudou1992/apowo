using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Together;

public class ExampleScript : MonoBehaviour {
    public InputField username;
    public Text logField;

	void Awake (){ 
		TGSDK.SDKInitFinishedCallback = (string msg) => {
			Log ("TGSDK finished : " + msg);
		};
#if UNITY_IOS && !UNITY_EDITOR
		TGSDK.Initialize ("F621ZmgT2K81b864U7M7");
#elif UNITY_ANDROID && !UNITY_EDITOR
		TGSDK.Initialize ("F621ZmgT2K81b864U7M7");
#endif
	}

    public void Log(string message)
    {
        Debug.Log(message);
        if(logField != null)
        {
			if (logField.text.Length > 100) {
				logField.text = message;
			} else {
            	logField.text = logField.text + "\n" + message;
			}
        }
    }

    public void PreloadAd()
    {
		TGSDK.PreloadAdSuccessCallback = (string msg) => {
			username.text = msg;
			Log ("PreloadAdSuccessCallback : " + msg);
		};
		TGSDK.PreloadAdFailedCallback = (string msg) => {
			Log ("PreloadAdFailedCallback : " + msg);
		};
		TGSDK.CPAdLoadedCallback = (string msg) => {
			Log ("CPAdLoadedCallback : " + msg);
		};
		TGSDK.VideoAdLoadedCallback = (string msg) => {
			Log ("VideoAdLoadedCallback : " + msg);
		};
		TGSDK.AdShowSuccessCallback = (string msg) => {
			Log ("AdShowSuccessCallback : " + msg);
		};
		TGSDK.AdShowFailedCallback = (string msg) => {
			Log ("AdShowFailedCallback : " + msg);
		};
		TGSDK.AdCompleteCallback = (string msg) => {
			Log ("AdCompleteCallback : " + msg);
		};
		TGSDK.AdCloseCallback = (string msg) => {
			Log ("AdCloseCallback : " + msg);
		};
		TGSDK.AdClickCallback = (string msg) => {
			Log ("AdClickCallback : " + msg);
		};
		TGSDK.AdRewardSuccessCallback = (string msg) => {
			Log ("AdRewardSuccessCallback : " + msg);
		};
		TGSDK.AdRewardFailedCallback = (string msg) => {
			Log ("AdRewardFailedCallback : " + msg);
		};
        TGSDK.PreloadAd();
    }

    public void ShowAd()
    {
		string sceneid = username.text;
		if (TGSDK.CouldShowAd (sceneid)) {
			string cpImagePath = TGSDK.GetCPImagePath(sceneid);
			if (null != cpImagePath) {
				Log("cpImagePath : " + cpImagePath);
				TGSDK.ShowCPView(sceneid);
				TGSDK.ReportCPClose (sceneid);
			}
			TGSDK.ShowAd(sceneid);
		} else {
			Log("Scene "+sceneid+" could not to show");
		}
	}

}
