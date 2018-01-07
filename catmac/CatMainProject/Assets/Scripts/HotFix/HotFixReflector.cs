using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;

public class HotFixReflector : MonoBehaviour,IHotFixMain {
    object commandInstance;
    Type commandType;
    Type opcodeType;

    void Start () {
        try
        {
            GameApplication.Instance.HotFix = this;

            var dab = ResourceLoader.GetAssetBundle("flymodel.dll").LoadAllAssets<TextAsset>();
            var dllAsset = dab[0];

#if !UNITY_EDITOR
                                                var assembly =AppDomain.CurrentDomain.Load(dllAsset.bytes);
                                                Logger.Temp.Write("LoadModule done.");
#else
            var pab = ResourceLoader.GetAssetBundle("flymodel.pdb").LoadAllAssets<TextAsset>();
            var pdbAsset = pab[0];
            var assembly = AppDomain.CurrentDomain.Load(dllAsset.bytes, pdbAsset.bytes);
            Logger.Temp.Write("LoadModule With pdb done");
#endif
            commandType = assembly.GetType("FlyModel.CommandHandle");

            commandInstance = Activator.CreateInstance(commandType);

            opcodeType = assembly.GetType("FlyModel.Proto.ClientMethod");

            var wt = assembly.GetType("FlyModel.GameMain");
            var methodctor = wt.GetMethod("Start");
            methodctor.Invoke(null, null);
        }
        catch (Exception e)
        {
            Logger.Temp.Write(e.Message + e.StackTrace);
        }
    }

    object[] paramArray = new object[1];
    Dictionary<int, MethodCache> methods = new Dictionary<int, MethodCache>();
    MethodCache GetMethod(int opcode)
    {
        if (methods.ContainsKey(opcode))
        {
            return methods[opcode];
        }
        else
        {
            string method = Enum.GetName(opcodeType, opcode);
            if (string.IsNullOrEmpty(method)==false)
            {
                var m = commandType.GetMethod(method);

                if (m != null)
                {
                    var mc = new MethodCache();
                    methods[opcode] = mc;
                    mc.Method = m;
                    var ps = m.GetParameters();

                    if (ps.Length == 1)
                    {
                        var po = Activator.CreateInstance(ps[0].ParameterType);
                        mc.ParameterType = ps[0].ParameterType;
                        var rm = ps[0].ParameterType.GetMethod("ReadFrom", new Type[] { typeof(byte[]), typeof(int), typeof(int) });
                        if (rm != null)
                        {
                            mc.ReadMethod = rm;
                        }
                        else
                        {
                            Debug.LogError("Command" + method + " 参数" + ps[0].ParameterType.FullName + "没有ReadFrom函数,无法执行");
                        }
                    }
                    else if (ps.Length >= 1)
                    {
                        Debug.LogError("Command" + method + " 参数个数大于1,无法执行");
                    }
                    return mc;

                }
                else
                {
                    Debug.LogError("CommandHandle: " + method + " not found");
                    return null;
                }
            }
            else
            {
                Debug.LogWarning(string.Format("收到未定义的ClientMethod opcode: {0}", opcode));
                return null;
            }
        }
    }

    public void InvokeScriptMethod(byte[] datas)
    {
        int opcode = BitConverter.ToInt32(datas, 0);

        var m = GetMethod(opcode);

        if (m != null)
        {
            m.Invoke(datas);
        }
    }
}

public class MethodCache
{
    object[] readParamList;
    object[] paramList;

    public MethodInfo Method
    {
        get;
        set;
    }

    public MethodInfo ReadMethod
    {
        get;
        set;
    }

    public Type ParameterType
    {
        get;
        set;
    }

    public void Invoke(byte[] datas)
    {
        if (ParameterType != null)
        {
            var po = Activator.CreateInstance(ParameterType);
            if (readParamList == null)
            {
                readParamList = new object[3];
                readParamList[1] = 4;
            }
            readParamList[0] = datas;
            readParamList[2] = datas.Length - 4;
            ReadMethod.Invoke(po, readParamList);
            readParamList[0] = "";
            if (paramList == null)
            {
                paramList = new object[1];
            }
            paramList[0] = po;
            Debug.Log("TryInvoke " + Method.Name + " " + po.ToString());
            Method.Invoke(null, paramList);
        }
        else
        {
            Debug.Log("TryInvoke " + Method.Name);
            Method.Invoke(null, null);
        }
    }
}
