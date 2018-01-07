using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using System.Runtime.Serialization.Json;
using System.Text;
 
using System.Collections;

using System.Runtime.Serialization;

[Serializable]
public class JsonFormatObject
{
    public virtual long ID { get; set; }

    public virtual JsonValue ToJson(Type structureType = null)
    {
        JsonObject jo = new JsonObject();
        if (structureType == null)
        {
            structureType = this.GetType();
        }
        var ts = structureType.GetJsonMembers();
        for (int i = 0; i < ts.Count; i++)
        {
            var dp = ts[i];

            var child = dp.GetValue(this);
            if (child is JsonFormatObject)
            {
                jo.Add(dp.Name, (child as JsonFormatObject).ToJson());
            }
            else
            {
                jo.Add(dp.Name, JsonSerializer.Serialize(child));
            }
        }
        return jo;
    }
    public virtual JsonObject ToJson<T>(params Expression<Func<T, object>>[] propertyExprs)
    {
        var result = GetPropertyInfos(propertyExprs);

        return GetJsonValue(result);

    }

    public virtual JsonObject ToJsonByArray<T>(Expression<Func<T, object[]>> propertyExpr)
    {
        var result = GetPropertyInfos(propertyExpr);

        return GetJsonValue(result);
    }

    public JsonObject GetJsonValue(IEnumerable<PropertyInfo> result)
    {
        JsonObject updates = new JsonObject();
        foreach (var p in result)
        {
            try
            {
                var child = p.GetValue(this, null);
                if (child is JsonFormatObject)
                {
                    updates[p.Name] = (child as JsonFormatObject).ToJson();
                }
                else
                {
                    updates[p.Name] = JsonSerializer.Serialize(child);
                }
            }
            catch (Exception ee)
            {
                Logger.Error.Write(ee.Message + ee.StackTrace);
                throw (ee);
            }
        }
        return updates;
    }

    public static IEnumerable<PropertyInfo> GetPropertyInfos<T>(params Expression<Func<T, object>>[] propertyExprs)
    {
        List<PropertyInfo> result = new List<PropertyInfo>();
        foreach (Expression<Func<T, object>> propertyExpr in propertyExprs)
        {
            if (propertyExpr.Body is UnaryExpression)
            {
                var propertyInfo = (PropertyInfo)((propertyExpr.Body as UnaryExpression).Operand as MemberExpression).Member;
                result.Add(propertyInfo);
            }
            else if (propertyExpr.Body is MemberExpression)
            {
                var propertyInfo = (PropertyInfo)(propertyExpr.Body as MemberExpression).Member;
                result.Add(propertyInfo);
            }
        }
        return result;
    }

    public static IEnumerable<PropertyInfo> GetPropertyInfos<T>(Expression<Func<T, object[]>> propertyExpr)
    {
        List<PropertyInfo> result = new List<PropertyInfo>();
        if (propertyExpr.Body is NewArrayExpression)
        {
            var exps = (propertyExpr.Body as NewArrayExpression).Expressions;
            foreach (var exp in exps)
            {
                if (exp is UnaryExpression)
                {
                    var propertyInfo = (PropertyInfo)((exp as UnaryExpression).Operand as MemberExpression).Member;
                    result.Add(propertyInfo);
                }
                else if (exp is MemberExpression)
                {
                    var propertyInfo = (PropertyInfo)(exp as MemberExpression).Member;
                    result.Add(propertyInfo);
                }
            }
        }
        return result;
    }

    public struct PropertyRequirement
    {
        public string Name
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }
    }

    public JsonFormatObject Clone()
    {
        var jv = ToJson();
        return JsonSerializer.DeserializeObject(jv, this.GetType()) as JsonFormatObject;
    }

    public T Clone<T>() where T:JsonFormatObject
    {
        return Clone() as T;
    }
}
 