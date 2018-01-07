using FlyModel.Model.Data;
using FlyModel.UI.Component;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Panel.ScenePhotoPanel
{
    public class ScenePhotoCell
    {
        public GameObject GameObject;
        public PhotoData Data;

        private Texture2D texture;

        private Image sceneImage;

        private Text photoNameTF;

        public ScenePhotoCell(GameObject go)
        {
            GameObject = go;

            sceneImage = go.transform.Find("Photo").GetComponent<Image>();

            photoNameTF = go.transform.Find("Name").GetComponent<Text>();

            new SoundButton(go, () =>
            {
                PanelManager.ScenePhotoPreviewPanel.Show(()=> { PanelManager.scenePhotoPreviewPanel.SetData(Data, texture, photoNameTF.text); });
            });
        }

        public void UpdateData(Model.Data.PhotoData data, int index)
        {
            Data = data;

            GameObject.transform.localRotation = Quaternion.Euler(0, 0, ScenePhotoPanel.ROTATIN[index % 4]);

            string url = string.Format("{0}{1}", ResourceLoader.ScenePictureCacheUrl, Data.PhotoName);
            if (PanelManager.scenePhotoPanel.TextureDic.ContainsKey(url))
            {
                Sprite sprite = PanelManager.scenePhotoPanel.TextureDic[url];
                this.texture = sprite.texture;
                sceneImage.sprite = sprite;
            }
            else
            {
                PanelManager.scenePhotoPanel.LoadPhoto(url, (sprite) => {
                    this.texture = sprite.texture;
                    sceneImage.sprite = sprite;
                });
            }

            photoNameTF.text = formatDate(data.PhotoName);
        }

        public void Clear()
        {
            //GameObject.Destroy(texture);
        }

        private string formatDate(string name)
        {
            var names = name.Split('-');
            return string.Format("{0}/{1}/{2} {3}:{4}", names[1], names[2], names[3], names[4], names[5]);
        }
    }
}
