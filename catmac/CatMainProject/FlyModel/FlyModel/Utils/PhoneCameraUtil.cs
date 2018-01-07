using FlyModel.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Utils
{
    public class PhoneCameraUtil
    {
        public static string deviceName;
        //摄像头
        public static WebCamTexture tex;
        //保存路径
        public static string path = "/Users/hanxu/Desktop/" + "test.png";

        //展示摄像头内容
        public static IEnumerator ShowWebCam(DelayController delayController)
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                WebCamDevice[] devices = WebCamTexture.devices;
                deviceName = devices[0].name;
                tex = new WebCamTexture(deviceName);
                delayController.GetComponent<Renderer>().material.mainTexture = tex;
                yield return new WaitForEndOfFrame();
                tex.Play();

            }
        }

        //保存为图片
        public static void SaveWebCam()
        {

            Texture2D tempTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
            Color32[] data = new Color32[tempTex.width * tempTex.height];
            tempTex.SetPixels32(tex.GetPixels32(data));
            byte[] imagebytes = tempTex.EncodeToPNG();
            FileStream cache = new FileStream(path, FileMode.Create);
            cache.Write(imagebytes, 0, imagebytes.Length);
            cache.Close();

            File.WriteAllBytes(path, imagebytes);
        }
    }
}
