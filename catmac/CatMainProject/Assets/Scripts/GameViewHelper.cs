using UnityEngine;
using System.Collections;
using System.Linq;

public static class GameViewHelper
{
#if UNITY_EDITOR
    private static System.Reflection.FieldInfo s_MainGameViewRect = null;
    public static void Init()
    {
        var gameView = typeof(UnityEditor.EditorWindow).Assembly.GetTypes().Where(a => a.Name == "GameView").FirstOrDefault();
        s_MainGameViewRect = gameView.GetField("s_MainGameViewRect", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
    }
    public static Rect GetRect()
    {
        if (s_MainGameViewRect == null)
            Init();
        Rect r = (Rect)s_MainGameViewRect.GetValue(null);
        r.y -= 17;
        return r;
    }
#else
     public static Rect GetRect()
     {
         return new Rect(0, 0, Screen.width, Screen.height);
     }
 
#endif
}
