using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Utils
{
    public class SoundUtil
    {
        public static void PlaySound(string fileAssetBundle, Action  callback=null)
        {
            Debug.Log("11111111111");
            ResourceLoader.Instance.TryLoadSound(fileAssetBundle.ToLower(), (sound) =>
            {
                PlaySound((AudioClip)sound);
                if (callback != null)
                {
                    Debug.Log("22222222222");
                    callback();
                    Debug.Log("66666666666");
                }
            });
        }

        public static void PlaySound(AudioClip clip)
        {
            GameApplication.SoundEffectController.clip = clip;
            GameApplication.SoundEffectController.Play();
        }

        public static void PlayMusic(AudioClip clip, bool loop = false)
        {
            GameApplication.MusicController.clip = clip;
            GameApplication.MusicController.Play();
        }
    }
}
