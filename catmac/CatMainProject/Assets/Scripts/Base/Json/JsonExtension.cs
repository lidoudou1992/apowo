using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using System.Diagnostics;
using System.Reflection;


public static class JsonExtension
{

    public static JsonValue ToJson(this IDictionary<string, object> pardic)
    {
        JsonObject jo = new JsonObject();
        foreach (var key in pardic.Keys)
        {
            var re =  JsonUtils.ToJson(pardic[key]);
            jo.Add(key, re);
        }
        return jo;
    }

    public static JsonValue ToJson(this IEnumerable pardic)
    {
        return  JsonUtils.ToJson(pardic);
    }
}
        
 
