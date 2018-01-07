using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Log;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public static class Logger
{
    static Dictionary<string, ILogFile> ILogFiles = new Dictionary<string, ILogFile>();

    private static List<ILogFile> logList = new List<ILogFile>();

    public static List<ILogFile> LogList
    {
        get { return logList; }
        set { logList = value; }
    }

    public static string RootPath = "Log";

    public static ILogFile GetLogFile(string name)
    {
        if (!ILogFiles.ContainsKey(name))
        {
            var result = new LogFile(Path.Combine(LogManager.Rootpath, RootPath), name);
            ILogFiles[name] = result;
            LogList.Add(result);
            result.Name = name;
            return result;
        }
        else
        {
            return ILogFiles[name];
        }
    }
    public static ILogFile Login
    {
        get
        {
            return GetLogFile("Login");
        }
    }

    public static ILogFile Temp
    {
        get
        {
            return GetLogFile("Temp");
        }
    }

    public static ILogFile Debug
    {
        get
        {
            return GetLogFile("Debug");
        }
    }

    public static ILogFile Fatel
    {
        get
        {
            return GetLogFile("Fatel");
        }
    }

    public static ILogFile Server
    {
        get
        {
            return GetLogFile("Server");
        }
    }

    public static ILogFile Web
    {
        get
        {
            return GetLogFile("Web");
        }
    }

 

    public static ILogFile Error
    {
        get
        {
            return GetLogFile("Error");
        }
    }

    public static ILogFile ConnectionError
    {
        get
        {
            return GetLogFile("ConnectionError");
        }
    }

    public static ILogFile Warning
    {
        get
        {
            return GetLogFile("Warning");
        }
    }



    public static ILogFile OnRecv
    {
        get
        {
            return GetLogFile("Recv");
        }
    }

    public static ILogFile OnSend
    {
        get
        {
            return GetLogFile("Send");
        }
    }

    public static ILogFile Connection
    {
        get
        {
            return GetLogFile("Connection");
        }
    }
     
   
}

