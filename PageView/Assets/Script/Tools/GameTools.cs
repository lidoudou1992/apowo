//-------------------------------
//该Demo由风冻冰痕所写
//http://icemark.cn/blog
//转载请说明出处
//-------------------------------
using UnityEngine;

public static class GameTools
{
    /// <summary>
    /// 往父对象中添加指定的子对象，照搬自NGUI的NGUITools
    /// </summary>
    /// <param name="parent">父对象</param>
    /// <param name="prefab">子对象</param>
    /// <returns>新加的子对象</returns>
    public static GameObject AddChild(Transform parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;

        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.SetParent(parent, false);
            go.layer = parent.gameObject.layer;
        }
        return go;
    }

    /// <summary>
    /// 销毁对象，照搬自NGUI的NGUITools
    /// </summary>
    /// <param name="obj"></param>
    public static void Destroy(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isPlaying)
            {
                if (obj is GameObject)
                {
                    GameObject go = obj as GameObject;
                    go.transform.parent = null;
                }

                UnityEngine.Object.Destroy(obj);
            }
            else UnityEngine.Object.DestroyImmediate(obj);
        }
    }
}