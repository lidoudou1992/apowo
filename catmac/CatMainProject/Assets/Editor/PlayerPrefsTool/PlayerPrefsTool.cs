using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.PlayerPrefsTool
{
    public class PlayerPrefsTool
    {
        [MenuItem("PlayerPrefs/Clear", false, 1)]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Clear PlayerPrefs");
        }
    }
}
