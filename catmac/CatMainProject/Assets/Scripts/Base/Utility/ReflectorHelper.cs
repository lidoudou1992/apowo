using System;
using System.Reflection;
using System.Json;

namespace FlyModel
{
    public static class ReflectorHelper
    {
   
        public static object ChangeType2(Type type, object value)
        {
            if ((value == null) && type.IsGenericType)
            {
                return Activator.CreateInstance(type);
            }
            if (value == null)
            {
                return null;
            }
            if (type == value.GetType())
            {
                return value;
            }
            if (type.IsEnum)
            {
                if (value is string)
                {
                    return Enum.Parse(type, value as string, true);
                }
                return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type type1 = type.GetGenericArguments()[0];
                object obj1 = ChangeType2(type1, value);
                return Activator.CreateInstance(type, new object[] { obj1 });
            }
            if ((value is string) && (type == typeof(Guid)))
            {
                return new Guid(value as string);
            }
            if ((value is string) && (type == typeof(Version)))
            {
                return new Version(value as string);
            }
            if (!(value is IConvertible))
            {
                return value;
            }
            return Convert.ChangeType(value, type, null);
        }

        static void ConvertParametersType(ParameterInfo[] ParameterInfos, object[] parameters)
        {
            if (parameters.Length != ParameterInfos.Length)
            {
                throw new Exception("参数个数不正确,需要" + ParameterInfos.Length + ",发送了" + parameters.Length);
            }
            for (int i = 0; i < ParameterInfos.Length; i++)
            {
				try{
                var ParameterInfo = ParameterInfos[i];
                object parameter=parameters[i];
                if (parameters[i] != null && !parameters[i].GetType().IsSubclassOf(ParameterInfo.ParameterType))
                {
                    if (ParameterInfo.ParameterType == parameter.GetType())
                    {

                    }
                    else if (parameter is JsonObject)
                    {
                        parameter = JsonSerializer.DeserializeObject((parameter as JsonObject), ParameterInfo.ParameterType);
                        parameters[i] = parameter;
                    }
                    else if (parameter is JsonArray)
                    {                       
                        parameter = JsonSerializer.DeserializeArray((parameter as JsonArray), ParameterInfo.ParameterType.GetGenericArguments()[0]);
                        parameters[i] = parameter;
                    }
                    else if (ParameterInfo.ParameterType.IsPrimitive)
                    {
                        var pType=parameter.GetType();
                        if (pType != ParameterInfo.ParameterType)
                        {
                            parameters[i] = Convert.ChangeType(parameter, ParameterInfo.ParameterType);
                        }
                    }
                    else if (ParameterInfo.ParameterType.IsEnum)
                    {
                        parameters[i] = Enum.Parse(ParameterInfo.ParameterType, parameters[i].ToString());
                    }
                }
				}catch(Exception e)
				{
					Logger.Error.Write("[{0}][{1}]",e.Message,e.StackTrace);
				}
            }
        }
		
		public static object CallMethod(object obj, string methodName, object[] parameters)
        {
            Type type = obj.GetType();
           
            MethodInfo methodInfo = type.GetMethod(methodName);
            if (methodInfo == null)
            {
                Logger.Error.Write("方法[{0}]未解析", methodName);
                throw new MissingMethodException();
            }
            else
            {
                ConvertParametersType(methodInfo.GetParameters(), parameters);

                return methodInfo.Invoke(obj, parameters);

            }
        }

        

        private static object ChangeType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {

                if (value == null)
                    return null;
                throw new NotSupportedException();
                //System.ComponentModel.NullableConverter nullableConverter
                //    = new System.ComponentModel.NullableConverter(conversionType);

                //conversionType = nullableConverter.UnderlyingType;
            }
            if (conversionType.GetInterface("IConvertible", true) != null)
            {
                return Convert.ChangeType(value, conversionType, null);
            }
            return value;
            //return Convert.ChangeType(value, conversionType,null);
        }
    } 

}

