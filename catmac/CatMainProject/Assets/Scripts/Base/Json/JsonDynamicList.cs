using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Json;
using System.Linq;
using System.Text;

 
    [Serializable]
    public class JsonDynamicList<T> : List<T>, IJsonDynamicCollection where T :class, IJsonDynamic
    {
        public JsonDynamicList()
        {

        }

        public void InitializeFromString(string valueString, Type type, Type parentType)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                return;
            }
            if (this.Count == 0)
            {
                var jv = JsonSerializer.Deserialize(valueString);
                InitializeFromJsonValue(jv, type,parentType);
            }
        }

        public void InitializeFromJsonValue(JsonValue jv, Type type, Type parentType)
        {    
            if (jv != null)
            {
                if (jv is JsonArray)
                {
                    JsonArray ja = jv as JsonArray;
                    foreach (JsonValue el in ja)
                    {
                        if (el.ContainsKey("ClassName"))
                        {
                            var functionname =(string)(el["ClassName"] as JsonPrimitive).Value;
                            Type btype = TypeCache.GetType(functionname, parentType);

                            var result = JsonSerializer.DeserializeObject(el as JsonObject, btype) as T;
                            Add(result);
                        }
                    }
                }
                else
                {
                }
            }
        }

        public Type BaseType
        {
            get { return typeof(T); }
        }
    }

    [Serializable]
    public class TypeCache
    {
        internal static Dictionary<string, Type> Typecache = new Dictionary<string, Type>();
        public static Type GetType(string typeName, System.Type parentType)
        {
            Type btype;
            if (Typecache.ContainsKey(typeName))
            {
                btype = Typecache[typeName];
            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var ns = parentType.Namespace;
                string fullName = string.IsNullOrEmpty(ns) ? typeName : ns + "." + typeName;
                btype = parentType.Assembly.GetType(fullName);

                if (btype == null)
                {
                    btype = parentType.Assembly.GetType(typeName);
                    if (btype == null)
                    {
                        var tps = parentType.Assembly.GetTypes();
                        foreach (var t in tps)
                        {
                            ///可能造成死循环
                            //var tb = System.Text.Encoding.UTF8.GetBytes(t.Name);
                            //var tb1 = System.Text.Encoding.UTF8.GetBytes(typeName);
                            //Logger.Error.WriteLog("[{0}][{1}][{2}][{3}]", t.Name,tb.ToJson(),typeName,tb1.ToJson());
                            if (t.Name.Equals(typeName))
                            {
                                btype = t;
                                break;
                            }
                        }
                        if (btype == null)
                        {
                            Logger.Error.Write("反序列化类[{0}][{1}][{2}]时未找到", typeName, fullName, parentType.Assembly.FullName);
                        }

                    }
                }

                Typecache[typeName] = btype;
                sw.Stop();
                if (sw.ElapsedMilliseconds > 100)
                {
                    Logger.Temp.Write("获取类信息[{0}]耗时[{1}]毫秒[{2}]", typeName, sw.ElapsedMilliseconds, fullName);
                }
            }
            return btype;
        }
    }

    [Serializable]
    public class JsonList<T> : List<T>,IExtraCollection
    {
        public Type ItemType
        {
            get { return typeof(T); }
        }
    }
 
