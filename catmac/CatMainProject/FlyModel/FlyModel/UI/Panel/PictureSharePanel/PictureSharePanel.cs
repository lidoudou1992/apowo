using Assets.Scripts.TouchController;
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
using UnityEngine.EventSystems;

namespace FlyModel.UI.Panel.PictureSharePanel
{
    public class PictureSharePanel : PanelBase
    {
        public override string AssetName
        {
            get
            {
                return "PictureSharePanel";
            }
        }

        private byte[] bytes;

        private string currentPicName;

        private string photoName;

        protected override void Initialize(GameObject go)
        {
            PointerSensor pointerSensor = go.AddComponent<PointerSensor>();
            pointerSensor.OnPointerClickHandler = closeSelf;

            new SoundButton(go.transform.Find("Image/sina").gameObject, () =>
            {
                //Debug.Log("新浪");
                CaptureImage(PlatformType.SinaWeibo);
            });

            new SoundButton(go.transform.Find("Image/qq").gameObject, () =>
            {
                //Debug.Log("qq好友");
                CaptureImage(PlatformType.QQ);
            });

            new SoundButton(go.transform.Find("Image/qqz").gameObject, () =>
            {
                //Debug.Log("qq空间");
                CaptureImage(PlatformType.QZone);
            });

            new SoundButton(go.transform.Find("Image/weixin").gameObject, () =>
            {
                //Debug.Log("微信好友");
                CaptureImage(PlatformType.WeChat);
            });

            new SoundButton(go.transform.Find("Image/friends").gameObject, () =>
            {
                //Debug.Log("朋友圈");
                CaptureImage(PlatformType.WeChatMoments);
            });
        }

        public override void Refresh()
        {
            base.Refresh();


        }

        public void SetData(byte[] bytes, string name)
        {
            this.bytes = bytes;

            photoName = name;
        }

        public override void SetInfoBar()
        {

        }

        public void closeSelf(PointerEventData eventData)
        {
            Close();
        }

        private void CaptureImage(PlatformType type)
        {
            if (uploadFTP(bytes))
            {
                ShareImage(type);
            }

            //var dir = System.IO.Path.GetDirectoryName(ResourceLoader.ScenePictureCacheRoot);
            //if (!System.IO.Directory.Exists(dir))
            //{
            //    System.IO.Directory.CreateDirectory(dir);
            //}
            //string fileName = string.Format("{0}{1}.jpg", ResourceLoader.ScenePictureCacheRoot, "test");
            //System.IO.FileStream cache = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            //cache.Write(bytes, 0, bytes.Length);
            //cache.Close();
        }

        private bool checkFile(string fileName)
        {
            FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(fileName);
            ftp.Credentials = new NetworkCredential("ftpcat", "6qLXVcW0weQ[$Pmb");
            ftp.Timeout = 5000;
            ftp.UsePassive = true;
            ftp.KeepAlive = false;
            ftp.UseBinary = true;
            ftp.Method = WebRequestMethods.Ftp.GetFileSize; //尝试获取文件大小

            try
            {
                FtpWebResponse ftpresponse = (FtpWebResponse)ftp.GetResponse();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool uploadFTP(byte[] bytes)
        {
            
            currentPicName = string.Format("{0}-{1}", UserManager.Instance.Me.ID, photoName);

            //string ftphost = "222.73.31.114:6420";
            // 使用域名和端口号登陆FTP服务器
            string ftphost = "catftp.apowogame.com:6420";

            string ftpfullpath = "ftp://" + ftphost + @"/" + currentPicName;
            Debug.Log("FTP完整路径ftpfullpath：" + ftpfullpath);

            if (checkFile(ftpfullpath))
            {
                return true;
            }

            try
            {
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential("ftpcat", "6qLXVcW0weQ[$Pmb");
                ftp.Timeout = 5000;
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

            if (type == PlatformType.QZone)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    content.SetImageUrl(string.Format("{0}{1}", SharePanel.SharePanel.SHARE_URL, currentPicName));
                    content.SetShareType(ContentType.Image);
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    content.SetTitle("萌猫大合集");
                    content.SetThumbImageUrl(string.Format("{0}{1}", SharePanel.SharePanel.SHARE_URL, "icon.png"));
                    //content.SetTitleUrl(string.Format("{0}{1}", SharePanel.SharePanel.SHARE_URL, currentPicName));
                    content.SetUrl(string.Format("{0}{1}", SharePanel.SharePanel.SHARE_URL, currentPicName));
                    content.SetImageUrl(string.Format("{0}{1}", SharePanel.SharePanel.SHARE_URL, currentPicName));

                    content.SetShareType(ContentType.Webpage);
                }
            } else if (Application.platform == RuntimePlatform.IPhonePlayer && type == PlatformType.SinaWeibo) {
                content.SetTitle("萌猫大合集");
                content.SetText("萌猫大合集");
                content.SetThumbImageUrl(string.Format("{0}{1}", SharePanel.SharePanel.SHARE_URL, "icon.png"));
                //content.SetTitleUrl(string.Format("{0}{1}", SharePanel.SharePanel.SHARE_URL, currentPicName));
                content.SetUrl(string.Format("{0}{1}", SharePanel.SharePanel.SHARE_URL, currentPicName));
                content.SetImageUrl(string.Format("{0}{1}", SharePanel.SharePanel.SHARE_URL, currentPicName));
                content.SetShareType(ContentType.Auto);

                PanelManager.ShowTipString("分享成功", EnumConfig.PropPopupPanelBtnModde.OneBtn);
            } else
            {
                content.SetImageUrl(string.Format("{0}{1}", SharePanel.SharePanel.SHARE_URL, currentPicName));
                content.SetShareType(ContentType.Image);
            }

            GameApplication.Instance.SSDK.ShareContent(type, content);
            Close();
        }
    }
}
