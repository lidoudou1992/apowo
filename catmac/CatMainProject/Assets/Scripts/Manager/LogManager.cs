using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    static string lastError = "";
    public static void Initialize()
    {
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            Rootpath = Application.persistentDataPath;
        }

        Log.LogFile.OnWriteAnyLine = (string msg) =>
        {
            if (msg.Length < 2000)
            {
                AddList(msg);
                //Debug.Log(msg);
            }
        };
        Logger.Error.OnWriteLine = (string msg) =>
        {
            Debug.LogError(msg);
            if (GameApplication.Instance.SocketClient != null && msg != lastError)
            {
                lastError = msg;
            }
            AddList(msg);
        };

        //Logger.Temp.OnWriteLine = (string msg) =>
        //{
        //    AddList(msg);
        //    Debug.LogWarning(msg);
        //};

    }

    static string rootpath = "";
    List<string> items = new List<string>();
    public static string Rootpath
    {
        get
        {
            return rootpath;
        }
        set
        {
            rootpath = value;
        }
    }

    static Queue<string> _cache = new Queue<string>();

    public static Queue<string> Cache
    {
        get { return LogManager._cache; }
    }
    public static bool lastAdd = false;
    public static void AddList(string msg)
    {
        _cache.Enqueue(msg);

        lastAdd = true;
    }

}
