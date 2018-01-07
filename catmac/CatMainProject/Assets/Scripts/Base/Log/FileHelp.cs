using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Json;
using UnityEngine;

public class FileHelp
{
	public static List<string> LoadText(string fileName)
	{
		string root="";
		if(Application.platform==RuntimePlatform.IPhonePlayer)
		{
			root=UnityEngine.Application.persistentDataPath;
		}
		fileName=Path.Combine(root,fileName);
		if(!File.Exists(fileName))
		{
			File.Create(fileName);
		}
	    List<string> lines=new List<string>();
	    using (StreamReader sr = File.OpenText(fileName))
	    {
	        string linestring=sr.ReadLine();
	       
	        while (!string.IsNullOrEmpty(linestring))
	        {
	            lines.Add(linestring);
	            linestring = sr.ReadLine();
	        }
	    }
	    return lines;
	}

    public static JsonObject LoadJson(string fileName)
    {
        string root = "";
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            root = UnityEngine.Application.persistentDataPath;
        }
        fileName = Path.Combine(root, fileName);
        if (!File.Exists(fileName))
        {
            File.Create(fileName);
        }
      
        using (StreamReader sr = File.OpenText(fileName))
        {
            string str = sr.ReadToEnd();
            if(string.IsNullOrEmpty(str))
            {
                return new JsonObject();
            }
            return JsonSerializer.Deserialize(str) as JsonObject;
        }
        
    }
 

	public static T LoadConfig<T>()  where T : class, new()
	{
		T re=new T();
		var ps=typeof(T).GetJsonMembers();
	    foreach(var p in ps)
		{
			if(PlayerPrefs.HasKey(p.Name))
			{
				var va=PlayerPrefs.GetString(p.Name);
				p.SetValue(re,va);
			}
		}
		return re;
	}
	
	public static void SaveConfig(JsonFormatObject obj) 
	{
		var ps=obj.GetType().GetJsonMembers();
		foreach(var p in ps)
		{
			PlayerPrefs.SetString(p.Name,p.GetValue(obj).ToString());	
		}
		PlayerPrefs.Save();
	}

    public static void SaveByte(string fileName,byte[] datas)
    {
        string root = ResourceLoader.AssetBundleCacheRoot;
        
        fileName = Path.Combine(root, fileName);
       
        using (var sr =File.Open(fileName,FileMode.Create))
        {
            sr.Write(datas, 0, (int)datas.Length);
        }
    }

    public static byte[] Read(string fileName)
    {
        string root = ResourceLoader.AssetBundleCacheRoot;

        fileName = Path.Combine(root, fileName);
        if (!File.Exists(fileName))
        {
           return null;
        }
        
        using(var sr=File.OpenRead(fileName))
        {
            byte[] re =new byte[sr.Length];
            sr.Read(re,0,(int)sr.Length);
            return re;
        }
    }
}

