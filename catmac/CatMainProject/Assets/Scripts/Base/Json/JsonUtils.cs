using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using System.Collections;
using System.Linq.Expressions;
 
 
    internal static class JsonUtils
    {
        private static Dictionary<Type, JsonPrimitiveProvider> s_primitiveProviders =
            new Dictionary<Type, JsonPrimitiveProvider>
            {
                 { 
                    typeof(byte),
                    new JsonPrimitiveProvider(p => (int)p, o => new JsonPrimitive((byte)o))
                },
                 { 
                    typeof(short),
                    new JsonPrimitiveProvider(p => (int)p, o => new JsonPrimitive((short)o))
                },
                { 
                    typeof(int),
                    new JsonPrimitiveProvider(p => (int)p, o => new JsonPrimitive((int)o))
                },
                 { 
                    typeof(uint),
                    new JsonPrimitiveProvider(p => (uint)p, o => new JsonPrimitive((uint)o))
                },
                { 
                    typeof(long),
                    new JsonPrimitiveProvider(p => (long)p, o => new JsonPrimitive((long)o))
                },
                { 
                    typeof(float),
                    new JsonPrimitiveProvider(p => (float)p, o => new JsonPrimitive((float)o))
                },
                { 
                    typeof(double),
                    new JsonPrimitiveProvider(p => (double)p, o => new JsonPrimitive((double)o))
                },
                { 
                    typeof(string),
                    new JsonPrimitiveProvider(p => (string)p, o => new JsonPrimitive((string)o))
                },
                { 
                    typeof(bool),
                    new JsonPrimitiveProvider(p => (bool)p, 
                        o =>{
                                var result= new JsonPrimitive((bool)o);
                                return result;
                            })
                },
                {
                    typeof(DateTime),
                    new JsonPrimitiveProvider(p =>
                    {
                        if(p==null)
                        {
                            return DateTime.Now;
                        }
                        else
                        {
                            return (DateTime)p;
                        }
                    }
                    , o =>       
                    {
                        if (o != null)
                        {
                            DateTime dt = DateTime.Parse(o.ToString());
                            return new JsonPrimitive(dt);
                        }
                        else
                        { 
                            return new JsonPrimitive(DateTime .Now);
                        }
                    })
                },
                {
                    typeof(TimeSpan),
                    new JsonPrimitiveProvider(p =>
                    {
                        if(p==null)
                        {
                            return TimeSpan.Zero;
                        }
                        else
                        {
                            return (TimeSpan)p;
                        }
                    }
                    , o =>       
                    {
                        if (o != null)
                        {
                            TimeSpan dt = TimeSpan.Parse(o.ToString());
                            return new JsonPrimitive(dt);
                        }
                        else
                        {
                            return new JsonPrimitive(TimeSpan.Zero);
                        }
                    })
                },
                 {
                    typeof(Guid),
                    new JsonPrimitiveProvider(p =>
                    {
                        if(p==null)
                        {
                            return Guid.NewGuid();
                        }
                        else
                        {
                            return (Guid)p;
                        }
                    }
                    , o =>       
                    {
                        if (o != null)
                        {
                            Guid dt = (Guid)o;
                            return new JsonPrimitive(dt);
                        }
                        else
                        {
                            return new JsonPrimitive(Guid.NewGuid());
                        }
                    })
                }
            };

        public static JsonValue ToJson(object value)
        {
            if (value == null)
                return null;
            //if (value is DateTime)
            //{
            //    return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            //}
            if (value is JsonFormatObject)
            {
                return (value as JsonFormatObject).ToJson();
            }
            if (value is Enum)
            {
                return (int)value;
            }
            var jsonValue = value as JsonValue;
            if (jsonValue != null) 
                return jsonValue;

            JsonPrimitiveProvider provider;
            if (s_primitiveProviders.TryGetValue(value.GetType(), out provider))
            {
                var re= provider.ToPrimitive(value);
                return re;
            }

            var dict = value as Dictionary<string, object>;
            if (dict != null)
            {
                return new JsonObject(dict.ToDictionary(p => p.Key, p => ToJson(p.Value)));
            }
            
            var array = value as IEnumerable;
            if (array != null)
            {
                return new JsonArray(array.Cast<object>().Select(o => ToJson(o)));
            }
            if (value.GetType().IsPrimitive)
            {
                return (JsonPrimitive)value;
            }
            var jsonObject = new JsonObject();
            var valueProperties=value.GetType().GetProperties().Where(p => !p.IsSpecialName);
            if (valueProperties.Count() == 0)
            {
                return value.ToString();
            }
            else
            {
                foreach (var property in valueProperties)
                {
                    jsonObject.Add(property.Name, ToJson(property.GetValue(value, null)));
                }
            }
            return jsonObject;
        }

        public static JsonValue ToJsonValue(IJsonProperty property, object value)
        {
            try
            {
                if (property.Converter == null)
                {
                    return JsonUtils.ToJson(value);
                }

                return property.Converter.ToJsonValue(property.PropertyInfo.PropertyType, value);
            }
            catch (Exception ex)
            {
                throw new ConversionException(property.PropertyInfo, value, ex);
            }
        }

        public static void SetProperty(object entity, IJsonProperty property, JsonValue value)
        {
            var propertyType = property.PropertyInfo.PropertyType;

            object propertyValue;
            try
            {
                if (property.Converter == null)
                {
                    propertyValue = value;
                    if (property.PropertyInfo.PropertyType.IsEnum)
                    {
                        object tempobject=(value as JsonPrimitive).Value;
                        if (tempobject is int)
                        {
                            propertyValue = tempobject;
                        }
                        else
                        {
                            propertyValue = Enum.Parse(property.PropertyInfo.PropertyType, (string)tempobject);
                        }
                       
                    }
                    else if (property.PropertyInfo.PropertyType == typeof(Guid))
                    {
                        propertyValue =new Guid(value.ToString());
                    }
                }
                else
                {
                    propertyValue = property.Converter.FromJsonValue(propertyType, value, entity.GetType());
                }
            }
            catch (Exception ex)
            {
                throw new ConversionException(property.PropertyInfo, value, ex);
            }

            PropertySetter.Set(entity, property.PropertyInfo, propertyValue);
        }
    }

    internal static class JsonUtils<T>
    {
        static JsonUtils()
        {
            var jsonValueExpr = Expression.Parameter(typeof(JsonValue), "jsonValue");
            var convertExpr = GetConvertExpression(jsonValueExpr, typeof(T));
            var lambdaExpr = Expression.Lambda<Func<JsonValue, T>>(convertExpr, jsonValueExpr);
            s_fromJsonValue = lambdaExpr.Compile();
        }

        private static Func<JsonValue, T> s_fromJsonValue;

        public static T FromJsonValue(JsonValue jsonValue)
        {
            return s_fromJsonValue(jsonValue);
        }

        private static Expression GetConvertExpression(Expression instanceExpr, Type targetType)
        {
            var mediateType = instanceExpr.Type;

            if (mediateType == typeof(object))
            {
                // (TargetType)instance
                return Expression.Convert(instanceExpr, targetType);
            }

            while (mediateType != typeof(object))
            {
                try
                {
                    // (MediateType)instace
                    var mediateExpr = Expression.Convert(instanceExpr, mediateType);
                    // (TargetType)(MediateType)instance
                    return Expression.Convert(mediateExpr, targetType);
                }
                catch
                {
                    mediateType = mediateType.BaseType;
                }
            }

            throw new Exception();
        }
    }
 
