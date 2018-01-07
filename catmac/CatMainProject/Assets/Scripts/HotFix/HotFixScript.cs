using UnityEngine;
using System.Collections;
using CLRSharp;
using System;
using System.Collections.Generic;
using LitJson;

public class HotFixScript : MonoBehaviour, IHotFixMain{

    public static CLRSharp_Environment env;
    ThreadContext context;
    CLRSharp_Instance commandInstance;
    ICLRType commandType;

    void Initialize()
    {
        env = new CLRSharp.CLRSharp_Environment(new Logger());

        ////for aot
        //env.GetType(typeof(Dictionary<int, string>));
        //env.GetType(typeof(Dictionary<int, object>));
        //env.GetType(typeof(Dictionary<int, CLRSharp.CLRSharp_Instance>));
        //env.GetType(typeof(Dictionary<int, Action>));

        //env.GetType(typeof(Dictionary<Int16, Action>));
        //env.GetType(typeof(LinkedList<int>));
        //env.GetType(typeof(int[,]));

        //env.GetType(typeof(List<Vector3>));
        //env.GetType(typeof(List<int>[]));
        //env.GetType(typeof(List<List<int>>));
        //env.GetType(typeof(List<List<List<int>>>));
        //env.GetType(typeof(Vector3[]));
        //env.GetType(typeof(System.Collections.Generic.IEnumerable<int>));

        ////for aot dele
        //CLRSharp.Delegate_Binder.RegBind(typeof(Action<int>), new CLRSharp.Delegate_BindTool<int>());
        //CLRSharp.Delegate_Binder.RegBind(typeof(Action<int, int>), new CLRSharp.Delegate_BindTool<int, int>());
        //CLRSharp.Delegate_Binder.RegBind(typeof(Action<int, int, int>), new CLRSharp.Delegate_BindTool<int, int, int>());
        //CLRSharp.Delegate_Binder.RegBind(typeof(Func<int, int, int>), new CLRSharp.Delegate_BindTool_Ret<int, int, int>());
        //CLRSharp.Delegate_Binder.RegBind(typeof(Action<int, string>), new CLRSharp.Delegate_BindTool<int, string>());
        //CLRSharp.Delegate_Binder.RegBind(typeof(Action<string>), new CLRSharp.Delegate_BindTool<string>());

        //CLRSharp.Delegate_Binder.RegBind(typeof(Action<bool>), new CLRSharp.Delegate_BindTool<bool>());

        //for aot
        env.GetType(typeof(Dictionary<int, string>));
        env.GetType(typeof(Dictionary<int, object>));
        env.GetType(typeof(Dictionary<int, int>));
        env.GetType(typeof(Dictionary<int, CLRSharp.CLRSharp_Instance>));
        env.GetType(typeof(Dictionary<long, CLRSharp.CLRSharp_Instance>));
        env.GetType(typeof(Dictionary<int, Action>));
        env.GetType(typeof(Queue<Vector3>));

        env.GetType(typeof(Dictionary<Int16, Action>));
        env.GetType(typeof(LinkedList<int>));
        env.GetType(typeof(int[,]));
        env.GetType(typeof(int[]));
        env.GetType(typeof(int));
        env.GetType(typeof(List<long>));

        env.GetType(typeof(List<Vector3>));
        env.GetType(typeof(List<int>[]));
        env.GetType(typeof(List<long>[]));
        env.GetType(typeof(List<List<int>>));
        env.GetType(typeof(List<List<List<int>>>));
        env.GetType(typeof(List<List<List<KeyCode>>>));
        env.GetType(typeof(Vector3[]));
        env.GetType(typeof(System.Collections.Generic.IEnumerable<int>));
        env.GetType(typeof(System.Collections.Generic.IEnumerable<CLRSharp.CLRSharp_Instance>));
        env.GetType(typeof(System.IO.Stream));
        env.GetType(typeof(CLRSharp.Delegate_BindTool_Ret<long, CLRSharp.CLRSharp_Instance>));
        env.GetType(typeof(UnityEngine.Events.UnityAction<UnityEngine.Vector2>));
        env.GetType(typeof(UnityEngine.Events.UnityAction<CLRSharp.CLRSharp_Instance>));
        
        //env.GetType(typeof(LitJson.JsonData));
        //env.GetType(typeof(LitJson.JsonException));
        //env.GetType(typeof(LitJson.JsonMapper));
        //env.GetType(typeof(LitJson.JsonReader));
        //env.GetType(typeof(LitJson.JsonToken));
        //env.GetType(typeof(LitJson.JsonType));
        //env.GetType(typeof(LitJson.JsonWriter));

        //for aot dele
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<int>), new CLRSharp.Delegate_BindTool<int>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<long>), new CLRSharp.Delegate_BindTool<long>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<float>), new CLRSharp.Delegate_BindTool<float>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<int, int>), new CLRSharp.Delegate_BindTool<int, int>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<int, int, int>), new CLRSharp.Delegate_BindTool<int, int, int>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Func<int, int, int>), new CLRSharp.Delegate_BindTool_Ret<int, int, int>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Func<CLRSharp.CLRSharp_Instance, long>), new CLRSharp.Delegate_BindTool_Ret<long, CLRSharp.CLRSharp_Instance>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<int, string>), new CLRSharp.Delegate_BindTool<int, string>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<string>), new CLRSharp.Delegate_BindTool<string>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<string, float>), new CLRSharp.Delegate_BindTool<string, float>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<bool>), new CLRSharp.Delegate_BindTool<bool>());
        CLRSharp.Delegate_Binder.RegBind(typeof(UnityEngine.Events.UnityAction<UnityEngine.Vector2>), new CLRSharp.Delegate_BindTool<UnityEngine.Vector2>());

        CLRSharp.Delegate_Binder.RegBind(typeof(Action<CLRSharp.CLRSharp_Instance, CLRSharp.CLRSharp_Instance, int>), new CLRSharp.Delegate_BindTool<CLRSharp.CLRSharp_Instance, CLRSharp.CLRSharp_Instance, int>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<UnityEngine.EventSystems.PointerEventData>), new CLRSharp.Delegate_BindTool<UnityEngine.EventSystems.PointerEventData>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Func<CLRSharp.CLRSharp_Instance, int, CLRSharp.CLRSharp_Instance>), new CLRSharp.Delegate_BindTool_Ret<CLRSharp.CLRSharp_Instance, CLRSharp.CLRSharp_Instance, int>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<CLRSharp.CLRSharp_Instance>), new CLRSharp.Delegate_BindTool<CLRSharp.CLRSharp_Instance>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<GSDKUnityLib.Pay.PayResultInfo>), new CLRSharp.Delegate_BindTool<GSDKUnityLib.Pay.PayResultInfo>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<UnityEngine.AudioClip>), new CLRSharp.Delegate_BindTool<UnityEngine.AudioClip>());
        CLRSharp.Delegate_Binder.RegBind(typeof(Action<UnityEngine.Object>), new CLRSharp.Delegate_BindTool<UnityEngine.Object>());
    }

    void Start()
    {
        GameApplication.Instance.HotFix = this;
        Initialize();

        var dab = ResourceLoader.GetAssetBundle("flymodel.dll").LoadAllAssets<TextAsset>();
        var dllAsset = dab[0];
        var msDll = new System.IO.MemoryStream(dllAsset.bytes);

        //var pab = ResourceLoader.GetAssetBundle("flymodel.pdb").LoadAllAssets<TextAsset>(); ;
        //var pdbAsset = pab[0];
        //var msPdb = new System.IO.MemoryStream(pdbAsset.bytes);

        //env.LoadModule(msDll, msPdb, new Mono.Cecil.Pdb.PdbReaderProvider());
        env.LoadModule(msDll);
        Debug.Log("LoadModule HotFixCode.dll done.");


        context = new CLRSharp.ThreadContext(env);

        Debug.Log("Create ThreadContext for L#.");


        commandType = env.GetType("FlyModel.CommandHandle");

        commandInstance = new CLRSharp_Instance(commandType as CLRSharp.ICLRType_Sharp);
        IMethod methodctor = commandType.GetMethod(".ctor", CLRSharp.MethodParamList.constEmpty());
        if (methodctor != null)
        {
            methodctor.Invoke(context, commandInstance, null);
        }

        DoStaticMethod(context, "FlyModel.GameMain", "Start");
    }

    object[] paramArray = new object[1];

    public void InvokeScriptMethod(byte[] datas)
    {
        var ot = env.GetType("FlyModel.Proto.ClientMethod") as Type_Common_CLRSharp;
        int opcode = BitConverter.ToInt32(datas, 0);
        string method = "";
        foreach (var f in ot.type_CLRSharp.Fields)
        {
            if (f.HasConstant && (int)f.Constant == opcode)
            {
                method = f.Name;
                break;
            }
        }
        Debug.Log("try invoke method " + method);
        var ms = commandType.GetMethods(method);
        if (ms.Length > 0)
        {
            var m = ms[0];
            if (m.Name == method)
            {
                if (m.ParamList.Count > 0)
                {
                    var pt = m.ParamList[0];
                    var po = new CLRSharp_Instance(pt as ICLRType_Sharp);
                    var ptctor = pt.GetMethod(".ctor", MethodParamList.constEmpty());//取得构造函数
                    ptctor.Invoke(context, po, null);//执行构造函数

                    paramArray[0] = po;
                    var ts = pt.GetMethod("ReadFrom", MethodParamList.Make(
                         HotFixScript.env.GetType(typeof(byte[])),
                     HotFixScript.env.GetType(typeof(int)),
                     HotFixScript.env.GetType(typeof(int))
                     ));
                    ts.Invoke(context, po, new object[] { datas, 4, datas.Length - 4 });

                    m.Invoke(context, null, paramArray);
                }
            }
        }
        else
        {
            Debug.LogError("Command " + method + " not found");
        }

    }

    void InvokeMethod(CLRSharp.ThreadContext context, string type, string method)
    {
        CLRSharp.ICLRType wt = env.GetType(type);//用全名称，包括命名空间
        CLRSharp.CLRSharp_Instance to = new CLRSharp.CLRSharp_Instance(wt as CLRSharp.ICLRType_Sharp);//创建实例
        CLRSharp.IMethod methodctor = wt.GetMethod(".ctor", CLRSharp.MethodParamList.constEmpty());//取得构造函数
        methodctor.Invoke(context, to, null);//执行构造函数
        CLRSharp.IMethod mt = wt.GetMethod(method, CLRSharp.MethodParamList.constEmpty());
        mt.Invoke(context, to, null);
    }

    object DoStaticMethod(CLRSharp.ThreadContext context, string type, string method)
    {
        CLRSharp.ICLRType wt = env.GetType(type);

        CLRSharp.IMethod methodctor = wt.GetMethod(method, CLRSharp.MethodParamList.constEmpty());//取得构造函数
        return methodctor.Invoke(context, null, null);
    }

    public class Logger : CLRSharp.ICLRSharp_Logger//实现L#的LOG接口
    {
        public void Log(string str)
        {
            Debug.Log(str);
        }

        public void Log_Error(string str)
        {
            Debug.LogError(str);
        }

        public void Log_Warning(string str)
        {
            Debug.LogWarning(str);
        }
    }
}

public static class ReflectorWrap
{
    public static object CreateObject(ThreadContext context, string name)
    {
        var wt = HotFixScript.env.GetType(name);

        CLRSharp.CLRSharp_Instance to = new CLRSharp.CLRSharp_Instance(wt as CLRSharp.ICLRType_Sharp);//创建实例
        CLRSharp.IMethod methodctor = wt.GetMethod(".ctor", CLRSharp.MethodParamList.constEmpty());//取得构造函数
        methodctor.Invoke(context, to, null);//执行构造函数
        return to;
    }

    public static object Get(CLRSharp_Instance o, string name, string fieldName)
    {
        var wt = HotFixScript.env.GetType(name);
        var f = wt.GetField(fieldName);
        return f.Get(o);
    }

    public static void Set(CLRSharp_Instance o, string name, string fieldName, object value)
    {
        var wt = HotFixScript.env.GetType(name);
        var f = wt.GetField(fieldName);
        f.Set(o, value);
    }
}

