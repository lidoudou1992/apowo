
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Json;

public static class DBHelp
{
    public static object SetValue(this PropertyInfo PropertyInfo, object target, object value)
    {
        if (PropertyInfo.PropertyType.IsEnum && value != null)
        {
            PropertyInfo.SetValue(target, Enum.Parse(PropertyInfo.PropertyType, value.ToString()), null);
        }
        else
        {
            if (value != null)
            {
                Type type = value.GetType();
                if (PropertyInfo.PropertyType != type)
                {
                    if (!type.IsSubclassOf(PropertyInfo.PropertyType))
                    {
                        try
                        {
                            if (PropertyInfo.PropertyType == typeof(DateTime))
                            {
                                value = DateTime.Parse(value.ToString());
                            }
                            else if (PropertyInfo.PropertyType == typeof(TimeSpan))
                            {
                                value = TimeSpan.Parse(value.ToString());
                            }
                            else if (PropertyInfo.PropertyType == typeof(float))
                            {
                               
                                value = (float)(JsonPrimitive)value;
                            }
                            else
                            {
                                //									if(value is JsonPrimitive)
                                //									{
                                //										var t=value as JsonPrimitive;
                                //										if(PropertyInfo.PropertyType==typeof(bool))
                                //										{
                                //											value=t.ToBoolean(null);
                                //										}
                                //										else if(PropertyInfo.PropertyType==typeof(char))
                                //										{
                                //											value=t.ToChar(null);
                                //										}
                                //											else if(PropertyInfo.PropertyType==typeof(byte))
                                //										{
                                //											value=t.ToByte(null);
                                //										}
                                //											else if(PropertyInfo.PropertyType==typeof(short))
                                //										{
                                //											value=t.ToInt16(null);
                                //										}
                                //											else if(PropertyInfo.PropertyType==typeof(int))
                                //										{
                                //											value=t.ToInt32(null);
                                //										}
                                //											else if(PropertyInfo.PropertyType==typeof(long))
                                //										{
                                //											value=t.ToInt64(null);
                                //										}
                                //											else if(PropertyInfo.PropertyType==typeof(string))
                                //										{
                                //											value=t.ToString(null);
                                //										}
                                //										 
                                //										
                                //									}
                                //									else
                                {
                                    value = Convert.ChangeType(value, PropertyInfo.PropertyType);
                                }
                            }
                        }
                        catch (Exception ee)
                        {
                            string re = "尝试把[" + value.GetType().Name + "-" + value.ToString() + "]转换成[" + PropertyInfo.PropertyType + "]时出错" + ee.Message + ee.StackTrace;
                            Logger.Error.Write(re);
                        }
                    }
                }
            }
            try
            {
                PropertyInfo.SetValue(target, value, null);
            }
            catch
            {
            }
        }
        return target;
    }

    /// <summary>
    /// 获取数据表类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetTableType(Type type)
    {
        if (type == typeof(byte) || type == typeof(short) || type == typeof(uint)
                || type == typeof(int) || type == typeof(long) || type == typeof(float)
                || type == typeof(double) || type == typeof(bool) || type == typeof(Guid)
                || type == typeof(DateTime))
        {
            return type;
        }
        if (type.IsEnum)
        {
            return typeof(int);
        }
        return typeof(string);
    }

    static Dictionary<Type, List<JsonMemberAttribute>> JsonMemberCache = new Dictionary<Type, List<JsonMemberAttribute>>();

    /// <summary>
    /// 获取需要保存到数据库的属性
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static List<JsonMemberAttribute> GetJsonMembers(this Type type)
    {
        lock (JsonMemberCache)
        {
            if (JsonMemberCache.ContainsKey(type))
            {
                return JsonMemberCache[type];
            }
            else
            {
                var tpinfos = type.GetProperties();
                List<JsonMemberAttribute> rpinfos = new List<JsonMemberAttribute>();
                for (int i = tpinfos.Length - 1; i >= 0; i--)
                {
                    PropertyInfo pinfo = tpinfos[i];
                    var attr = pinfo.GetCustomAttributes(typeof(JsonMemberAttribute), true);
                    if (attr != null && attr.Length >= 1)
                    {
                        JsonMemberAttribute dpa = attr[0] as JsonMemberAttribute;
                        dpa.PropertyType = pinfo.PropertyType;
                        dpa.PropertyInfo = pinfo;
                        rpinfos.Add(dpa);
                    }
                }
                rpinfos = rpinfos.OrderBy(p => p.ShowIndex).ToList();
                JsonMemberCache[type] = rpinfos;
                return rpinfos;
            }
        }
    }

    /// <summary>
    /// 获取需要保存到数据库的属性
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static JsonMemberAttribute GetJsonMember(this Type type, string propertyName)
    {
        lock (JsonMemberCache)
        {
            if (JsonMemberCache.ContainsKey(type))
            {
                return JsonMemberCache[type].First(p => p.Name == propertyName);
            }
            else
            {
                var tpinfos = type.GetProperties();
                List<JsonMemberAttribute> rpinfos = new List<JsonMemberAttribute>();
                for (int i = tpinfos.Length - 1; i >= 0; i--)
                {
                    PropertyInfo pinfo = tpinfos[i];
                    var attr = pinfo.GetCustomAttributes(typeof(JsonMemberAttribute), true);
                    if (attr != null && attr.Length >= 1)
                    {
                        JsonMemberAttribute dpa = attr[0] as JsonMemberAttribute;
                        dpa.PropertyType = pinfo.PropertyType;
                        dpa.PropertyInfo = pinfo;
                        rpinfos.Add(dpa);
                    }
                }
                rpinfos = rpinfos.OrderBy(p => p.ShowIndex).ToList();
                JsonMemberCache[type] = rpinfos;
                return rpinfos.First(p => p.Name == propertyName);
            }
        }
    }

    public static object GetValueByPropertyName(this object instance, string propertyName)
    {
        string[] ts = propertyName.Split('.');
        PropertyInfo result = null;
        object tempValue = instance;
        for (int i = 0; i < ts.Length; i++)
        {
            string tpn = ts[i];
            result = tempValue.GetType().GetProperty(tpn);
            tempValue = result.GetValue(tempValue, null);
        }
        return tempValue;
    }

    public static object GetValueByJsonPropertyName(this object instance, string propertyName)
    {
        string[] ts = propertyName.Split('.');
        JsonMemberAttribute result = null;
        object tempValue = instance;
        for (int i = 0; i < ts.Length; i++)
        {
            string tpn = ts[i];
            result = tempValue.GetType().GetJsonMember(tpn);
            tempValue = result.GetValue(tempValue, false);
        }
        return tempValue;
    }

    public static void SetValueByPropertyName(this object instance, string propertyName, object value)
    {
        string[] ts = propertyName.Split('.');
        PropertyInfo resultPinfo = null;
        object tempValue = instance;
        object parentObject = null;
        for (int i = 0; i < ts.Length; i++)
        {
            string tpn = ts[i];
            resultPinfo = tempValue.GetType().GetProperty(tpn);
            parentObject = tempValue;
            tempValue = resultPinfo.GetValue(tempValue, null);
        }
        resultPinfo.SetValue(parentObject, value, null);
    }

    public static void SetValueByJsonPropertyName(this object instance, string propertyName, object value)
    {
        string[] ts = propertyName.Split('.');
        JsonMemberAttribute resultPinfo = null;
        object tempValue = instance;
        object parentObject = null;
        for (int i = 0; i < ts.Length; i++)
        {
            string tpn = ts[i];
            resultPinfo = tempValue.GetType().GetJsonMember(tpn);
            parentObject = tempValue;
            tempValue = resultPinfo.GetValue(tempValue, false);
        }
        resultPinfo.SetValue(parentObject, value);
    }
}
 
