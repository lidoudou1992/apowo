using UnityEngine;
using System.Collections;
using UnityEditor;

public class CustomFont : Editor
{
    [MenuItem("MakeFont/Make")]
    static void Make()
    {
        Object[] objs = Selection.objects;
        if (objs.Length != 2)
        {
            Debug.LogError("请选中Custom Font文件与BMFont导出的fnt数据文件");
            return;
        }
        Font m_myFont;
        TextAsset m_data;
        if (objs[0].GetType() == typeof(TextAsset) && objs[1].GetType() == typeof(Font))
        {
            Debug.Log("text");
            m_data = (TextAsset)objs[0];
            m_myFont = (Font)objs[1];
            Debug.Log("FontName:" + m_myFont.name + "\nData:" + m_data.name);
        }
        else if (objs[1].GetType() == typeof(TextAsset) && objs[0].GetType() == typeof(Font))
        {
            m_data = (TextAsset)objs[1];
            m_myFont = (Font)objs[0];
            Debug.Log("FontName:" + m_myFont.name + "\nData:" + m_data.name);
        }
        else
        {
            Debug.LogError("请选中Custom Font文件与BMFont导出的fnt数据文件");
            Debug.Log("FontName:null" + "\nData:null");
            return;
        }

        BMFont mbFont = new BMFont();
        BMFontReader.Load(mbFont, m_data.name, m_data.bytes);  // 借用NGUI封装的读取类
        CharacterInfo[] characterInfo = new CharacterInfo[mbFont.glyphs.Count];
        for (int i = 0; i < mbFont.glyphs.Count; i++)
        {
            BMGlyph bmInfo = mbFont.glyphs[i];
            CharacterInfo info = new CharacterInfo();
            info.index = bmInfo.index;
            info.uv.x = (float)bmInfo.x / (float)mbFont.texWidth;
            info.uv.y = 1 - (float)bmInfo.y / (float)mbFont.texHeight;
            info.uv.width = (float)bmInfo.width / (float)mbFont.texWidth;
            info.uv.height = -(float)bmInfo.height / (float)mbFont.texHeight;
            info.vert.x = (float)bmInfo.offsetX;
            info.vert.y = (float)bmInfo.offsetY;
            info.vert.width = (float)bmInfo.width;
            info.vert.height = (float)bmInfo.height;
            info.width = (float)bmInfo.advance;
            characterInfo[i] = info;
        }
        m_myFont.characterInfo = characterInfo;

        Debug.Log("OK");
    }
}