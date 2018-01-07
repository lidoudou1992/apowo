using UnityEngine;
using System.Collections;
using System.Json;
 
using FlyModel;
using System;
using System.Collections.Generic;


public static class Extentions
{
    public static void SetColor(this MonoBehaviour mb ,Color color)
    {
        MeshRenderer mr =mb.gameObject.GetComponent<MeshRenderer>();
        if (mr == null)
        {
            mr = mb.gameObject.GetComponentInChildren<MeshRenderer>();
        }
        if (mr != null)
        {
            mr.material.color = color;
        }
    }

    public static void DeleteAllChildren(this GameObject go)
    {
        List<GameObject> todeleted = new List<GameObject>();
        for (int i = 0; i < go.transform.childCount;i++ )
        {
            var c = go.transform.GetChild(i);
            todeleted.Add(c.gameObject);
        }
        foreach(var g in todeleted)
        {
            GameObject.DestroyImmediate(g);
        }
    }

	public static void DestroyImmediateAllChildren(this GameObject go)
	{
		List<GameObject> todeleted = new List<GameObject>();
		for (int i = 0; i < go.transform.childCount;i++ )
		{
			var c = go.transform.GetChild(i);
			todeleted.Add(c.gameObject);
		}
		foreach(var g in todeleted)
		{
			GameObject.DestroyImmediate(g);
		}
	}

    public static void SetLayerRecursively(this GameObject obj, int layer)
    {
        if(!obj)
            return;

        obj.layer = layer;
        foreach(Transform child in obj.transform)
        {
            child.gameObject.SetLayerRecursively(layer);
        }
    }

    public static void SetTagRecursively(this GameObject obj, string tag)
    {
        if(!obj)
            return;

        obj.tag = tag;
        foreach(Transform child in obj.transform)
        {
            child.gameObject.SetTagRecursively(tag);
        }
    }

    public static GameObject FindChildByName(this GameObject parent, String name)
    {
        Transform result = parent.transform.Find(name);
        if(result!=null)
        {
            return result.gameObject;
        }
        else
        {
            foreach(Transform childTrans in parent.transform)
            {
                GameObject obj = FindChildByName(childTrans.gameObject, name);
                if (obj != null)
                    return obj;
            }
        }
        return null;
    }

    public static List<GameObject> FindChildrenWithTag(this GameObject go, string tag, bool recursion = false)
    {
        List<GameObject> result = new List<GameObject>();
        if(!go)
            return result;

        for(int i=0; i<go.transform.childCount; i++)
        {
            var c = go.transform.GetChild(i);
            if(recursion)
            {
                var cs = c.gameObject.FindChildrenWithTag(tag, recursion);
                result.AddRange(cs);
            }
        }

        if(go.tag == tag)
            result.Add(go);

        return result;
    }

    public static Color32 GetColor(this MonoBehaviour mb)
    {
        MeshRenderer mr = mb.gameObject.GetComponent<MeshRenderer>();
        if (mr == null)
        {
            mr = mb.gameObject.GetComponentInChildren<MeshRenderer>();
        }
        if (mr != null)
        {
            return mr.material.color;
        }
        return new Color32();
    }

    public static string LeapColor(int value,int CheckValue)
    {
        if(value<CheckValue)
        {
            return "FF0000";
        }
        else
        {
            return "00FF00";
        }
    }

    public static string HtmlColor(string color,object p)
    {
        return "<color=#" + color + ">" + p.ToString() + "</color>";
    }

	public static string LeapColorWithSymbol(object p,int value, int CheckValue, string min = "↓", string max = "↑")
    {
        if (value < CheckValue)
        {
            return "<color=#FF0000>(" + min +p+ ")</color>";
        }
        else
        {
            return "<color=#00FF00>(" + max +p+")</color>";
        }
    }

    public static T GetOrCreateCompont<T>(this GameObject go) where T : Component
    {
        var c = go.GetComponent<T>();
        if (c == null)
        {
            c = go.AddComponent<T>();
        }
        return c;
    }


    public static JsonObject ToJson(this Vector3 v)
    {
        JsonObject jo = new JsonObject();
        jo.Add("X", Math.Round(v.x, 6));
        jo.Add("Y", Math.Round(v.y, 6));
        jo.Add("Z", Math.Round(v.z, 6));
        return jo;
    }

    public static List<byte> ToBytes(this Vector3 v)
    {
        List<byte> re =new List<byte>();
        re.AddRange(BitConverter.GetBytes(v.x));
        re.AddRange(BitConverter.GetBytes(v.y));
        re.AddRange(BitConverter.GetBytes(v.z));
        return re;
    }

    public static Vector3 ToVector3(this Vector2 v)
    {
        return new Vector3(v.x,0,v.y);
    }

    public static JsonObject ToJson(this Quaternion v)
    {
        JsonObject jo = new JsonObject();
        jo.Add("X", Math.Round(v.x,6));
        jo.Add("Y", Math.Round(v.y, 6));
        jo.Add("Z", Math.Round(v.z, 6));
        jo.Add("W", Math.Round(v.w, 6));
        return jo;
    }

    public static JsonObject ToJson(this Color c)
    {
        JsonObject jo = new JsonObject();
        jo.Add("R", c.r);
        jo.Add("G", c.g);
        jo.Add("B", c.b);
        jo.Add("A", c.a);
        return jo;
    }

    public static JsonObject ToJson(this Color32 c)
    {
        JsonObject jo = new JsonObject();
        jo.Add("R", c.r);
        jo.Add("G", c.g);
        jo.Add("B", c.b);
        jo.Add("A", c.a);
        return jo;
    }

    public static Color32 ToColor32(JsonObject jo)
    {
        Color32 v = new Color32();
        v.r = (byte)jo["R"];
        v.g = (byte)jo["G"];
        v.b = (byte)jo["B"];
        v.a = (byte)jo["A"];
        return v;
    }

    public static Vector3 ToVector3(JsonObject jo)
    {
        Vector3 v = new Vector3();
        v.x = (float)jo["X"];
        v.y = (float)jo["Y"];

        v.z = (float)jo["Z"];
        return v;
    }

    public static Vector2 ToVector2(JsonObject jo)
    {
        Vector2 v = new Vector2();
        v.x = (float)jo["X"];
        v.y = (float)jo["Y"];
        return v;
    }

    public static Quaternion ToQuaternion(JsonObject jo)
    {
        Quaternion v = new Quaternion();
        v.x = (float)jo["X"];
        v.y = (float)jo["Y"];
        v.z = (float)jo["Z"];
        v.w = (float)jo["W"];
        return v;
    }

    public static TransformWrap ToTransform(JsonObject jo)
    {
        TransformWrap v = new TransformWrap();
        v.Position = ToVector3(jo["Position"] as JsonObject);
        v.Rotation = ToQuaternion(jo["Rotation"] as JsonObject);
        v.LocalScale = ToVector3(jo["LocalScale"] as JsonObject);
        return v;
    }

    public static void Reset(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }

    public static void AddRange<T>(this Dictionary<long, T> d, Dictionary<long, T> targets)
    {
        foreach(var k in targets.Keys)
        {
            d[k] = targets[k];
        }
    }

    public static Color GetDissolveColor(int quality)
    {

        Color c = Color.gray;

        switch (quality)
        {
            case 1:
                c = Color.black;
                break;
            case 2:
                c = Color.white;
                break;
            case 3:
                c = Color.yellow;
                break;
            case 4:
                c = Color.blue;
                break;
            default:
                c = Color.gray;
                break;
        }
        return c;
    }
}

public class TransformWrap:JsonFormatObject
{
    Vector3 position = Vector3.zero;
    [JsonMember(ShowIndex = 1, FormatType = typeof(Vector3Format))]
    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    Quaternion rotation = Quaternion.identity;
    [JsonMember(ShowIndex = 1, FormatType = typeof(QuaternionFormat))]
    public Quaternion Rotation
    {
        get { return rotation; }
        set { rotation = value; }
    }

    Vector3 localScale = Vector3.zero;
    [JsonMember(ShowIndex = 1, FormatType = typeof(Vector3Format))]
    public Vector3 LocalScale
    {
        get { return localScale; }
        set { localScale = value; }
    }

    public void CopyToTransform(Transform tf)
    {
        tf.position = this.position;
        tf.rotation = this.rotation;
        tf.localScale = this.LocalScale;

    }

}
