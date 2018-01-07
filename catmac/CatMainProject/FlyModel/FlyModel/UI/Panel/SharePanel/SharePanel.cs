using cn.sharesdk.unity3d;
using FlyModel.Model;
using FlyModel.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;

namespace FlyModel.UI.Panel.SharePanel
{
    public class SharePanel : PanelBase
    {
        public static string SHARE_URL = "http://resource.apowogame.com/cat/upload/";

        public override string AssetName
        {
            get
            {
                return "SharePanel";
            }
        }

        public override bool IsRoot
        {
            get
            {
                return true;
            }
        }

        private DelayController delayController;

        private Text UIDTF;

        private RectTransform rectImage;

        private float rate = Screen.height / 1136f;

        private string currentPicName;

        // 用来查看FTP服务器的IPv6地址
        private SocketClient socketClient;
        private AddressFamily newAddressFamily;
        private string newServerIp;

        protected override void Initialize(GameObject go)
        {
            UIDTF = go.transform.Find("Image/Image/Text").GetComponent<Text>();
            rectImage = go.transform.Find("Image/Image").GetComponent<RectTransform>();

            delayController = go.AddComponent<DelayController>();

            new SoundButton(go.transform.Find("Image/QQ").gameObject, () =>
            {
                delayController.StartCoroutine(CaptureImage(PlatformType.QQ));
            });

            new SoundButton(go.transform.Find("Image/weix").gameObject, () =>
            {
                delayController.StartCoroutine(CaptureImage(PlatformType.WeChat));
            });
        }

        public override void Refresh()
        {
            base.Refresh();

            currentPicName = "";
            UIDTF.text = UserManager.Instance.Me.ID.ToString();
        }

        public override void SetInfoBar()
        {
            
        }

        private IEnumerator CaptureImage(PlatformType type)
        {
            yield return new WaitForEndOfFrame();


            int relativeWidth = (int)(rectImage.sizeDelta.x * rate);
            int relativeHeight = (int)(rectImage.sizeDelta.y * rate);
            //图片大小
            Texture2D cutImage = new Texture2D((int)relativeWidth, (int)relativeHeight, TextureFormat.RGB24, false);

            //坐标左下角为0  
            Rect rect = new Rect(rectImage.position.x - relativeWidth/2f, rectImage.position.y - relativeHeight/2f, relativeWidth, relativeHeight);
            cutImage.ReadPixels(rect, 0, 0, false);

            cutImage.Apply();
            byte[] byt = cutImage.EncodeToJPG(75);

            if (uploadFTP(byt))
            {
                ShareImage(type);
            }
            //Debug.Log("平台类型type：" + type);
            //// 测试不上传到FTP，直接分享
            //ShareImage(type);
            //Debug.Log("分享成功");

            //var dir = System.IO.Path.GetDirectoryName(ResourceLoader.ScenePictureCacheRoot);
            //if (!System.IO.Directory.Exists(dir))
            //{
            //    System.IO.Directory.CreateDirectory(dir);
            //}
            //string fileName = string.Format("{0}{1}.jpg", ResourceLoader.ScenePictureCacheRoot, "test");
            //System.IO.FileStream cache = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            //cache.Write(byt, 0, byt.Length);
            //cache.Close();

            UnityEngine.Object.Destroy(cutImage);
        }

        private bool checkFile(string fileName)
        {
            FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(fileName);
            Debug.Log("ftp:" + ftp);
            ftp.Credentials = new NetworkCredential("ftpcat", "6qLXVcW0weQ[$Pmb");
            ftp.Timeout = 5000;
            ftp.UsePassive = true;
            ftp.KeepAlive = false;
            ftp.UseBinary = true;
            ftp.Method = WebRequestMethods.Ftp.GetFileSize; //尝试获取文件大小

            try
            {
                FtpWebResponse ftpresponse = (FtpWebResponse)ftp.GetResponse();
                Debug.Log("ftpresponse:" + ftpresponse);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("检查文件异常");
                return false;
            }
        }

        public bool uploadFTP(byte[] bytes)
        {
           
            currentPicName = string.Format("{0}.jpg", UserManager.Instance.Me.ID);

            //string ftphost = "222.73.31.114:6420";
            // 使用域名登陆服务器
            string ftphost = "catftp.apowogame.com:6420";

            //// iOS环境下把FTP服务器域名转换成IPv6类型的IP地址
            //string ip = "catftp.apowogame.com";
            ////SocketClient temp = new SocketClient(ip,6420);
            ////ftphost = ip + ":6420";
            //Debug.Log("ip:" + ip);
            // 用SocketClient来查看Ipv6地址
            //socketClient = new SocketClient("catftp.apowogame.com", 6420);
            //// 取得解析出的IPv6地址：newServerIp
            //socketClient.getIPType("catftp.apowogame.com", 6420.ToString(), out newServerIp, out newAddressFamily);

            string ftpfullpath = "ftp://" + ftphost + @"/" + currentPicName;
            //string ftpfullpath = "ftp://" + "64:ff9b::de49:1f72" + @"/" + currentPicName;
            //string ftpfullpath = "ftp://" + "64:ff9b::de49:1f72:6420" + @"/" + currentPicName;
            //string ftpfullpath = "ftp://" + "222.73.31.114:6420" + @"/" + currentPicName;
            //Debug.Log("ftpfullpath:" + ftpfullpath);
            //Debug.Log("64:ff9b::de49:1f72:6420");
            //socketClient = new SocketClient(ftpfullpath, 6420);
            //string ftpfullpath = "ftp://" + newServerIp + ":6420" + @"/" + currentPicName;
            Debug.Log("ftpfullpath:" + ftpfullpath);

            if (checkFile(ftpfullpath))
            {
                return true;
            }

            try
            {
                FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(ftpfullpath);
                Debug.Log("ftp:" + ftp);
                ftp.Credentials = new NetworkCredential("ftpcat", "6qLXVcW0weQ[$Pmb");
                ftp.Timeout = 5000;
                //// 测试超时为10秒时
                //ftp.Timeout = 10000;
                ftp.UsePassive = true;
                ftp.KeepAlive = false;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                //FileStream fs = File.OpenRead(inputfilepath);
                byte[] buffer = bytes;
                //fs.Read(buffer, 0, buffer.Length);
                //fs.Close();
                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                Debug.Log("Write成功");
                ftpstream.Close();
                return true;
            }
            catch (Exception e)
            {
                PanelManager.ShowTipString("分享失败", EnumConfig.PropPopupPanelBtnModde.OneBtn);
                Debug.Log(e);
            }

            return false;
        }

        private void ShareImage(PlatformType type)
        {
            ShareContent content = new ShareContent();
            content.SetImageUrl(string.Format("{0}{1}", SHARE_URL, currentPicName));
            content.SetShareType(ContentType.Image);

            GameApplication.Instance.SSDK.ShareContent(type, content);
        }
    }
}
