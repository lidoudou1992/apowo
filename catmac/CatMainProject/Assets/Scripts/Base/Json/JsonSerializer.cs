using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

using System.Collections;

public static class JsonSerializer
{
    public static JsonObject SerializeObject<T>(T entity, JsonContract<T> contract)
    {
        if (entity == null)
            return null;

        var jsonObject = new JsonObject();

        foreach (var property in contract.Properties)
        {
            var value = property.PropertyInfo.GetValue(entity, null);
            var jsonValue = JsonUtils.ToJsonValue(property, value);

            jsonObject.Add(property.Name, jsonValue);
        }

        return jsonObject;
    }

    public static JsonArray SerializeArray<T>(IEnumerable<T> entities, JsonContract<T> contract)
    {
        if (entities == null)
            return null;


        var jsonEntities = entities.Select(e => SerializeObject(e, contract));
        return new JsonArray(jsonEntities.Cast<JsonValue>());
    }

    public static T DeserializeObject<T>(string jsonString)
        where T : class, new()
    {
        var jsonObj = (JsonObject)Deserialize(jsonString);
        return DeserializeObject<T>(jsonObj);
    }

    public static T DeserializeObject<T>(JsonObject jsonObj)
    {
        if (jsonObj == null)
            return default(T);

        return (T)DeserializeObject(jsonObj, typeof(T));
    }
    public static object DeserializeObject(string jsonString, Type type)
    {
        var jsonObj = Deserialize(jsonString);
        return DeserializeObject(jsonObj, type);
    }

    public static object DeserializeObject(JsonValue jsonObj, Type type)
    {
        if (jsonObj is JsonArray)
        {
            return DeserializeArray(jsonObj as JsonArray, type);
        }
        else if (jsonObj is JsonObject)
        {
            return DeserializeObject(jsonObj as JsonObject, type);
        }
        else if (jsonObj is JsonPrimitive)
        {
            return (jsonObj as JsonPrimitive).Value;
        }
        else
        {
            return null;
        }
    }

    public static List<T> DeserializeArray<T>(string jsonString)
    {
        var jsonArray = (JsonArray)Deserialize(jsonString);
        return DeserializeArray<T>(jsonArray);
    }

    public static object DeserializeArray(string jsonString, Type type)
    {
        var jsonArray = (JsonArray)Deserialize(jsonString);
        return DeserializeArray(jsonArray, type);
    }

    public static void DeserializeObject(object entity, JsonObject jsonObj)
    {
        if (jsonObj == null)
            return;
        Type type = entity.GetType();
        JsonContract jcb = JsonContract.GetJsonContractByType(type);

        foreach (var property in jcb.Properties)
        {
            JsonValue jsonValue;
            if (!jsonObj.TryGetValue(property.Name, out jsonValue))
            {
                continue;
            }

            JsonUtils.SetProperty(entity, property, jsonValue);
        }
    }

    public static object DeserializeObject(JsonObject jsonObj, Type type)
    {
        if (jsonObj == null || type == null)
            return null;
        if (type == typeof(JsonObject))
        {
            return jsonObj;
        }

        object entity = Activator.CreateInstance(type);
        JsonContract jcb = JsonContract.GetJsonContractByType(type);
        if (type.IsValueType)
        {
            foreach (var property in jcb.Properties)
            {
                JsonValue jsonValue;
                if (!jsonObj.TryGetValue(property.Name, out jsonValue))
                {
                    continue;
                }

                JsonUtils.SetProperty(entity, property, jsonValue);
            }
            return entity;
        }
        else
        {
            foreach (var property in jcb.Properties)
            {
                JsonValue jsonValue;
                if (!jsonObj.TryGetValue(property.Name, out jsonValue))
                {
                    continue;
                }

                JsonUtils.SetProperty(entity, property, jsonValue);
            }
            return entity;
        }


    }

    public static List<T> DeserializeArray<T>(JsonArray jsonArray)
    {
        var templist = jsonArray.Select(e =>
        {
            if (e is JsonPrimitive)
            {
                var ss = (e as JsonPrimitive).Value;
                return (T)Convert.ChangeType(ss, typeof(T));
            }
            else if (e is JsonObject)
            {
                JsonObject jo = (JsonObject)e;
                return DeserializeObject<T>(jo);
            }
            else
            {
                return default(T);
            }
        }).ToList();
        return templist.ToList();
    }

    public static object DeserializeArray(JsonArray jsonArray, Type type)
    {
        if (type == typeof(JsonArray))
        {
            return jsonArray;
        }
        if (jsonArray == null)
        {
            return null;
        }
        var genericType = typeof(List<>).MakeGenericType(type);
        var result = Activator.CreateInstance(genericType) as IList;
        object temp = null;
        foreach (JsonValue v in jsonArray)
        {
            if (v is JsonPrimitive)
            {
                temp = (v as JsonPrimitive).Value;
                if (temp.ToString() == "null")
                {
                    temp = null;
                }
                else if (temp.ToString().Equals("null"))
                {
                    temp = null;
                }
                else if (!type.IsEnum && !temp.GetType().IsSubclassOf(type))
                {
                    temp = Convert.ChangeType(temp, type);
                }

                result.Add(temp);
            }
            else if (v is JsonObject)
            {
                JsonObject tempjo = (JsonObject)v;
                temp = DeserializeObject(tempjo, type);
                result.Add(temp);
            }
            else
            {
                if (typeof(IJsonDynamicCollection).IsAssignableFrom(type))
                {
                    var re = Activator.CreateInstance(type) as IJsonDynamicCollection;
                    re.InitializeFromJsonValue(v, type, type);
                    result.Add(re);
                }
                else if (typeof(IExtraCollection).IsAssignableFrom(type))
                {
                    var re = Activator.CreateInstance(type) as IExtraCollection;
                    JsonArray ja = v as JsonArray;
                    foreach (JsonValue jv in ja)
                    {
                        re.Add(JsonSerializer.DeserializeObject(jv, re.ItemType));
                    }
                    result.Add(re);
                }
            }

        }
        return result;
    }

    public static object DeserializeJsonDynamicArray(JsonArray ja, Type type)
    {
        return null;
    }

    public static JsonValue Deserialize(string jsonString)
    {
        try
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                return new JsonObject();
            }
            return JsonValue.Parse(jsonString);
        }
        catch (Exception ex)
        {
            Logger.Error.Write("{0}{1}", jsonString, ex);
            throw new JsonFormatException(jsonString, ex);
        }
    }

    public static JsonValue Serialize(object entity)
    {
        return JsonUtils.ToJson(entity);
    }
}
 
