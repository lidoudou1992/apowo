using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
 
    internal interface IJsonPrimitiveProvider
    {
        JsonPrimitive ToPrimitive(object value);
        object FromPrimitive(JsonPrimitive value);
    }
    public class DataTimeConverter : IJsonConverter
    {
        public JsonValue ToJsonValue(Type type, object value)
        {
            //return ((DateTime)value).ToString("R");
            return value.ToString();
        }

        public object FromJsonValue(Type type, JsonValue value, Type parentType)
        {
            if (value is JsonPrimitive)
            {
                var tv = value as JsonPrimitive;
                if (tv.Value is DateTime)
                {
                    return (DateTime)tv;
                }
                else
                {
                    DateTime result = new DateTime();
                    if (!DateTime.TryParse((string)tv, out result))
                    {
                    }
                    return result;
                }
            }
            else
            {
                return new DateTime();
            }
        }
    }
    internal class JsonPrimitiveProvider : IJsonPrimitiveProvider
    {
        private Func<object, JsonPrimitive> m_to;
        private Func<JsonPrimitive, object> m_from;

        public JsonPrimitiveProvider(Func<JsonPrimitive, object> from, Func<object, JsonPrimitive> to)
        {
            this.m_from = from;
            this.m_to = to;
        }

        public JsonPrimitive ToPrimitive(object value)
        {
            var temp= this.m_to(value);
            return temp;
        }

        public object FromPrimitive(JsonPrimitive value)
        {
            return this.m_from(value);
        }
    }
 
