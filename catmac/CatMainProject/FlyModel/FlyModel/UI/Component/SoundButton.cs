using FlyModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FlyModel.UI.Component
{
    public class SoundButton
    {
        public GameObject GameObject;
        public Button btn;
        private Action callback;
        private string sound;

        public SoundButton(GameObject go, Action callback=null, string sound= "BGM001")
        {
            GameObject = go;
            this.sound = sound;
            this.callback = callback;

            btn = go.transform.GetComponent<Button>();
            btn.onClick.AddListener(onClickhandler);
        }

        public void onClickhandler()
        {
            SoundUtil.PlaySound(sound, callback);
        }

        public void SetSound(string sound)
        {
            this.sound = sound;
        }

        public void SetCallback(Action callback)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(onClickhandler);
            this.callback = callback;
        }

        public void SetActive(bool active)
        {
            GameObject.SetActive(active);
        }

        public void RemoveListener()
        {
            btn.onClick.RemoveAllListeners();
        }
    }
}
