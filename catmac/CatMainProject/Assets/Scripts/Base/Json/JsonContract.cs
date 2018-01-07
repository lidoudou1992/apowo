using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
 
public class JsonContract
{
		static Dictionary<Type, JsonContract> JsonContractCache = new Dictionary<Type, JsonContract> ();

		public static JsonContract GetJsonContractByType (Type type)
		{
				JsonContract jct = null;
				if (JsonContractCache.ContainsKey (type)) {
						jct = JsonContractCache [type];
				} else {
                        //DateTime dt = DateTime.Now;
						jct = new JsonContract (type);
						jct.InitialDefault ();
						JsonContractCache [type] = jct;
                        //var cost = (int)(DateTime.Now - dt).TotalMilliseconds;
                        //Logger.GetLogFile ("性能").WriteLog ("GetJsonContractByType[{0}][{1}]耗时[{2}]", type.Name, jct.Properties.Count (), cost);
				}
				return jct;
		}

		public JsonContract (Type type)
		{
				this.Type = type;
		}

		public Type Type {
				get;
				set;
		}

		private Dictionary<PropertyInfo, IJsonProperty> m_properties = new Dictionary<PropertyInfo, IJsonProperty> ();

		public JsonSimpleProperty SimpleProperty (PropertyInfo pinfo)
		{
				var property = new JsonSimpleProperty (pinfo);
				this.m_properties.Add (pinfo, property);

				return property;
		}

		public JsonSimpleProperty SimpleProperty<T,TProperty> (Expression<Func<T, TProperty>> propertyExpr)
		{
				var propertyInfo = (PropertyInfo)(propertyExpr.Body as MemberExpression).Member;
				var property = new JsonSimpleProperty (propertyInfo);
				this.m_properties.Add (propertyInfo, property);

				return property;
		}

		public JsonComplexProperty<TProperty> ComplexProperty<TProperty> (PropertyInfo propertyInfo)
            where TProperty : class
		{
				var property = new JsonComplexProperty<TProperty> (propertyInfo);
				this.m_properties.Add (propertyInfo, property);

				return property;
		}

		public JsonArrayProperty<TElement> ArrayProperty<TElement> (PropertyInfo propertyInfo)
            where TElement : class
		{
				var property = new JsonArrayProperty<TElement> (propertyInfo);
				this.m_properties.Add (propertyInfo, property);

				return property;
		}

		public void InitialDefault ()
		{
				var tps = this.Type.GetJsonMembers ();
				foreach (var tp in tps) {
						if (!tp.PropertyInfo.CanWrite) {
								continue;
						}
						var tt = SimpleProperty (tp.PropertyInfo);
						if (tp.FormatType != null) {
								tt.Converter (tp.FormatType);
						}

						if (tp.PropertyInfo.PropertyType == typeof(DateTime)) {      
								tt.Converter (new DataTimeConverter ());
						}

				}
		}

		public void GetGenericType ()
		{
				typeof(JsonContract<>).MakeGenericType (Type);
		}

		internal IEnumerable<IJsonProperty> Properties {
				get {
						return this.m_properties.Values;
				}
		}
}

public class JsonContract<T>
{
		private Dictionary<PropertyInfo, IJsonProperty> m_properties = new Dictionary<PropertyInfo, IJsonProperty> ();

		public JsonSimpleProperty SimpleProperty<TProperty> (Expression<Func<T, TProperty>> propertyExpr)
		{
				var propertyInfo = (PropertyInfo)(propertyExpr.Body as MemberExpression).Member;
				var property = new JsonSimpleProperty (propertyInfo);
				this.m_properties.Add (propertyInfo, property);

				return property;
		}

		public JsonComplexProperty<TProperty> ComplexProperty<TProperty> (Expression<Func<T, TProperty>> propertyExpr)
            where TProperty : class
		{
				var propertyInfo = (PropertyInfo)(propertyExpr.Body as MemberExpression).Member;
				var property = new JsonComplexProperty<TProperty> (propertyInfo);
				this.m_properties.Add (propertyInfo, property);

				return property;
		}

		public JsonArrayProperty<TElement> ArrayProperty<TElement> (Expression<Func<T, IEnumerable<TElement>>> propertyExpr)
            where TElement : class
		{
				var propertyInfo = (PropertyInfo)(propertyExpr.Body as MemberExpression).Member;
				var property = new JsonArrayProperty<TElement> (propertyInfo);
				this.m_properties.Add (propertyInfo, property);

				return property;
		}

		internal IEnumerable<IJsonProperty> Properties {
				get {
						return this.m_properties.Values;
				}
		}
}
 
