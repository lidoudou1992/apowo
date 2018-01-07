using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using FlyModel;

 
    internal class PropertySetter
    {
        private struct CacheKey
        {
            public PropertyInfo PropertyInfo;
            public Type PropertyValueType;

            public override bool Equals(object obj)
            {
                var that = (CacheKey)obj;
                return this.PropertyInfo == that.PropertyInfo && this.PropertyValueType == that.PropertyValueType;
            }

            public override int GetHashCode()
            {
                var hashCode = this.PropertyInfo.GetHashCode();
                if (this.PropertyValueType != null)
                {
                    hashCode ^= this.PropertyValueType.GetHashCode();
                }

                return hashCode;
            }
        }

        public class SetStructValue
        {
            PropertyInfo propertyInfo;

            public PropertyInfo PropertyInfo
            {
                get { return propertyInfo; }
                set { propertyInfo = value; }
            }
            public object SetValue(Object entity,object value)
            {
                return propertyInfo.SetValue(entity, value);
            }
        }

        private class SetterCache : ConcurrentCache<CacheKey, Action<object, object>>
        {
            protected override Action<object, object> Create(CacheKey key)
            {
                return Create(key.PropertyInfo, key.PropertyValueType);
            }

            public static Action<object, object> SetValue(PropertyInfo propertyInfo)
            {
                Action<object, object> result = (object entity,object value) =>
                    {
                        propertyInfo.SetValue(entity,value);
                    };
                return result;
            }

            private static Action<object, object> Create(PropertyInfo propertyInfo, Type propertyValueType)
            {             
                    return SetValue(propertyInfo);
			}
        }

        private static SetterCache s_setterCache = new SetterCache();

        public static void Set(object entity, PropertyInfo propertyInfo, object propertyValue)
        {
            var cacheKey = new CacheKey
            {
                PropertyInfo = propertyInfo,
                PropertyValueType = propertyValue == null ? null : propertyValue.GetType()
            };
            
            var setter = s_setterCache.Get(cacheKey);
            if (setter == null)
            {
                throw new ConversionException(propertyInfo, propertyValue, null);
            }

            try
            {
                 setter(entity, propertyValue);
            }
            catch (Exception ex)
            {
                Logger.Fatel.Write(ex.Message + ex.StackTrace);
              
            }
        }
   }
 
