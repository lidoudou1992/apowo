using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Json;
using System.Linq;
using System.Text;
using UnityEngine;
using FlyModel;
 
internal class FormatCache : ConcurrentCache<Type, IDataFormat>
{
		internal static FormatCache Instance = new FormatCache ();

		protected override IDataFormat Create (Type key)
		{
				var dataFormat = Activator.CreateInstance (key) as IDataFormat;
				return dataFormat;
		}
}

public interface IDataFormat : IJsonConverter
{
		object SetValue (string value, Type type = null);
}

public class TimeSpanFormat : IDataFormat
{
        #region IDataFormat 成员

		public object SetValue (string value, Type type = null)
		{
				if (string.IsNullOrEmpty (value)) {
						return TimeSpan.Zero;
				} else {
						return TimeSpan.Parse (value);
				}
		}

        #endregion

        #region IJsonConverter 成员

		public JsonValue ToJsonValue (Type type, object value)
		{
				return value.ToString ();
		}

		public object FromJsonValue (Type type, JsonValue value, Type parentType)
		{
				if (string.IsNullOrEmpty (value)) {
						return TimeSpan.Zero;
				} else {
						return TimeSpan.Parse (value);
				}
		}

        #endregion
}

public class JsonDataFormat<T> : IDataFormat
{
    public virtual JsonValue ToJsonValue(Type type, object value)
    {
        JsonValue jv = JsonSerializer.Serialize(value);
        return jv;
    }

    public virtual object FromJsonValue(Type type, JsonValue value, Type parentType)
    {
        var rr = JsonSerializer.DeserializeObject(value, typeof(T));
        return rr;
    }

    public object SetValue(string value, Type type = null)
    {
        var rr = JsonSerializer.DeserializeObject(value, typeof(T));
        return rr;
    }
}

public class JsonObjectFormat : IDataFormat
{
    public virtual JsonValue ToJsonValue(Type type, object value)
    {
        if (value is JsonObject)
        {
            return value as JsonValue;
        }
        else
        {
            JsonValue jv = JsonSerializer.Serialize(value);
            return jv;
        }
    }

    public virtual object FromJsonValue(Type type, JsonValue value, Type parentType)
    {
        var rr = value as JsonObject;
        return rr;
    }

    public object SetValue(string value, Type type = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            return new JsonObject();
        }
        else
        {
            var jsonObj = JsonSerializer.Deserialize(value) as JsonObject;
            return jsonObj;
        }
    }
}

public class Vector3Format : IDataFormat
{
    public virtual JsonValue ToJsonValue(Type type, object value)
    {
        JsonValue jv = Extentions.ToJson((Vector3)value);
        return jv;
    }

    public virtual object FromJsonValue(Type type, JsonValue value, Type parentType)
    {
        var rr = Extentions.ToVector3(value as JsonObject);
        return rr;
    }

    public object SetValue(string value, Type type = null)
    {
        var re = JsonSerializer.Deserialize(value);
        return Extentions.ToVector3(re as JsonObject);
    }
}

public class QuaternionFormat : IDataFormat
{
    public virtual JsonValue ToJsonValue(Type type, object value)
    {
        JsonValue jv = Extentions.ToJson((Quaternion)value);
        return jv;
    }

    public virtual object FromJsonValue(Type type, JsonValue value, Type parentType)
    {
        var rr = Extentions.ToQuaternion(value as JsonObject);
        return rr;
    }

    public object SetValue(string value, Type type = null)
    {
        var re = JsonSerializer.Deserialize(value);
        return Extentions.ToQuaternion(re as JsonObject);
    }
}
/// <summary>
/// JsonArray到字符串的格式化转换
/// </summary>
public class JsonArrayFormat : IDataFormat
{
    public virtual JsonValue ToJsonValue(Type type, object value)
    {
        JsonValue jv = value as JsonArray;
        return jv;
    }

    public virtual object FromJsonValue(Type type, JsonValue value, Type parentType)
    {
        var rr = value as JsonArray;
        return rr;
    }

    public object SetValue(string value, Type type = null)
    {
        var rr = JsonSerializer.Deserialize(value) as JsonArray;
        return rr;
    }
}

public class FunctionFormat<T> : IDataFormat where T : class
{
    #region IDataFormat 成员

    public virtual JsonValue ToJsonValue(Type type, object value)
    {
        JsonValue jv = JsonSerializer.Serialize(value);
        return jv;
    }

    public virtual object FromJsonValue(Type type, JsonValue value, Type parentType)
    {
        if ((value is JsonObject) && value.ContainsKey("ClassName"))
        {
            var className = value["ClassName"];
            Type btype = TypeCache.GetType(className, parentType);

            var result = JsonSerializer.DeserializeObject(value as JsonObject, btype);
            return result as T;

        }
        else
        {
            return null;
        }
    }

    public virtual object SetValue(string value, Type type = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        JsonValue jv = JsonSerializer.Deserialize(value);
        return FromJsonValue(type, jv, type);
    }

    #endregion
}

public class JsonListFormat<T> : IDataFormat
{
		public JsonListFormat()
		{
		}
        #region IDataFormat 成员

		public JsonValue ToJsonValue (Type type, object value)
		{
				JsonValue rr = JsonSerializer.Serialize (value);
				return rr;
		}

		public virtual object FromJsonValue (Type type, JsonValue value, Type parentType)
		{
				if ((value is JsonPrimitive)) {
						var result = value.ToString ();
						if (string.IsNullOrEmpty (result)) {
								return new List<T> ();
						}
						return new List<T> ();
				}
				var rr = JsonSerializer.DeserializeArray (value as JsonArray, typeof(T));
				return rr;
		}

		public object SetValue (string value, Type type = null)
		{
				if (string.IsNullOrEmpty (value)) {
						return new List<T> ();
				}
				var rr = JsonSerializer.DeserializeArray (value, typeof(T));
				return rr;
		}

        #endregion
}

public class DynamicCollectionFormat<T, TCollection> : IDataFormat
        where T : class, IJsonDynamic
        where TCollection : IJsonDynamicCollection, new()
{
        #region IDataFormat 成员
		public JsonValue ToJsonValue (Type type, object value)
		{
				JsonValue jv = JsonSerializer.Serialize (value);
				return jv;
		}

		public virtual object FromJsonValue (Type type, JsonValue value, Type parentType)
		{
				TCollection fm = new TCollection ();
				fm.InitializeFromJsonValue (value, type, parentType);
				return fm;
		}

		public object SetValue (string value, Type type = null)
		{
				TCollection fm = new TCollection ();
				fm.InitializeFromString (value, type, type);         
				return fm;
		}

        #endregion
}

public class ExtraCollectionFormat<T, TCollection> : IDataFormat
        where T : class
        where TCollection : IList<T>, new()
{
        #region IDataFormat 成员
		public JsonValue ToJsonValue (Type type, object value)
		{
				JsonValue jv = JsonSerializer.Serialize (value);
				return jv;
		}

		public virtual object FromJsonValue (Type type, JsonValue value, Type parentType)
		{
				TCollection tc = new TCollection ();
				InitializeCollection (tc, value, typeof(T));
				return tc;
		}

		public object SetValue (string value, Type type = null)
		{
				TCollection tc = new TCollection ();
				var jv = JsonSerializer.Deserialize (value);
				InitializeCollection (tc, jv, typeof(T));
				return tc;
		}

		public void InitializeCollection (TCollection tc, JsonValue jv, Type type)
		{
				if (jv != null && jv is JsonArray) {
						JsonArray ja = jv as JsonArray;
						foreach (var el in ja) {
								var result = JsonSerializer.DeserializeObject (el as JsonObject, type) as T;
								tc.Add (result);
						}
				}
		}

        #endregion
}

public class DynamicDataFormat<T> : IDataFormat
        where T : class, IJsonDynamic
{
        #region IDataFormat 成员
		public JsonValue ToJsonValue (Type type, object value)
		{
				JsonValue jv = JsonSerializer.Serialize (value);
				return jv;
		}

		public virtual object FromJsonValue (Type type, JsonValue value, Type parentType)
		{
				if (value.ContainsKey ("ClassName")) {
						var ttype = type as Type;
						var functionname = value ["ClassName"];
						Type btype = TypeCache.GetType (functionname, ttype);

						var result = JsonSerializer.DeserializeObject (value as JsonObject, btype) as T;
						return result;
				}
				return null;
		}

		public object SetValue (string value, Type type = null)
		{
				var jsonvalue = JsonSerializer.Deserialize (value);
				return FromJsonValue (type, jsonvalue, type);
		}

        #endregion
}

public class CommonDictionayFormat<TKey, TValue> : IDataFormat
{
        #region IDataFormat 成员
		public JsonValue ToJsonValue (Type type, object value)
		{
				if (value == null) {
						value = new Dictionary<TKey, TValue> ();
				}
				return (value as Dictionary<TKey, TValue>).ToJson ();
		}

		public virtual object FromJsonValue (Type type, JsonValue jsonValue, Type parentType)
		{
				Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue> ();
				if (jsonValue is JsonArray) {
						JsonArray ja = (JsonArray)jsonValue;
						foreach (JsonValue jv in ja) {
								TKey key = default(TKey);
								TValue value = default(TValue);
								Type valueType = typeof(TValue);
								if (jv.ContainsKey ("Key")) {
										JsonValue keyvalue = jv ["Key"];
										var keyType = typeof(TKey);
										if (keyType == typeof(Guid)) {
												key = (TKey)(object)new Guid (keyvalue);
										} else {
												var sv = JsonSerializer.DeserializeObject (keyvalue, typeof(TKey));
												key = (TKey)Convert.ChangeType (sv, typeof(TKey));

										}
								}

								if (jv.ContainsKey ("Value")) {
										JsonValue valueResult = jv ["Value"];
										if (typeof(IJsonDynamicCollection).IsAssignableFrom (valueType)) {
												var temp = Activator.CreateInstance (valueType) as IJsonDynamicCollection;
												temp.InitializeFromJsonValue (valueResult, valueType, parentType);
												value = (TValue)temp;
										} else {
												value = (TValue)JsonSerializer.DeserializeObject (valueResult, valueType);
										}
								}

								result [key] = value;
						}
				} else if (jsonValue is JsonObject) {
						JsonObject ja = (JsonObject)jsonValue;
						foreach (var jv in ja) {
								try {
										Type keyType = typeof(TKey);
										TKey key = default(TKey);
										if (keyType == typeof(Guid)) {
												key = (TKey)(object)new Guid (jv.Key);
										} else if (keyType.IsValueType) {
												key = (TKey)Convert.ChangeType (jv.Key, keyType);
										} else {
												var keystring = JsonSerializer.DeserializeObject (jv.Key, keyType);
												key = (TKey)keystring;
										}
										TValue value = (TValue)JsonSerializer.DeserializeObject (jv.Value, typeof(TValue));

										result [key] = value;
								} catch (Exception ee) {
										Logger.Error.Write (ee.Message + ee.StackTrace);
										throw (ee);
								}
						}
				}
				return result;
		}

		public object SetValue (string value, Type type = null)
		{
				var jv = JsonSerializer.Deserialize (value);
				return FromJsonValue (typeof(Dictionary<TKey, TValue>), jv, type);
		}

        #endregion
}
 
