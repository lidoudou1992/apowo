using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
 
public class JsonMemberAttribute:Attribute
{
		public JsonMemberAttribute ()
		{
				ShowIndex = 1000;
				Editable = true;
		}
		/// <summary>
		/// 字段名字
		/// </summary>
		public string Name {
				get {
						return PropertyInfo.Name;
				}
		}

		/// <summary>
		/// 给编辑器看的名字
		/// </summary>
		private string showName = "";

		public string ShowName {
				get {
						if (string.IsNullOrEmpty (showName)) {
								return Name;
						}
						return showName; 
				}
				set { showName = value; }
		}
         
		/// <summary>
		/// 字段的描述
		/// </summary>
		public string Description {
				get;
				set;
		}


		/// <summary>
		/// 表示显示排序
		/// </summary>
		public int ShowIndex {
				get;
				set;
		}

		/// <summary>
		/// 可选值 一般为枚举
		/// </summary>
		public Type SelectedValue {
				get;
				set;
		}
		/// <summary>
		/// 是否可编辑修改
		/// </summary>
		public bool Editable {
				get;
				set;
		}
		/// <summary>
		/// 如果编辑类型是Command 则记录命令字符
		/// </summary>
		public string Command {
				get;
				set;
		}

		public Type FormatType {
				get;
				set;
		}

		public Type PropertyType {
				get;
				set;
		}

		public PropertyInfo PropertyInfo {
				get;
				set;
		}

		object defaultValue;

		public object DefaultValue {
				get {
						if (defaultValue == null) {
								if (PropertyType == typeof(string)) {
										return "";
								} else if (PropertyType == typeof(DateTime)) {
										return DateTime.Now;
								} else if (PropertyType.IsValueType) {
										return 0;
								} else {
										return null;
								}
						} else {
								return defaultValue;
						}
				}
				set {
						defaultValue = value;
				}
		}

		public object GetValue (object obj, bool checkFormat=true)
		{
				try {
						object result = PropertyInfo.GetValue (obj, null);

						if (result == null) {
								result = DefaultValue;
						}
						if (FormatType != null && checkFormat) {
								if (typeof(IDataFormat).IsAssignableFrom (FormatType)) {
										var dataFormat = Activator.CreateInstance (FormatType) as IDataFormat;
										result = dataFormat.ToJsonValue (this.PropertyType, result);
								}
						}
						if (result == null) {
								return null;
						}
						if (result.GetType () == typeof(uint)) {
								result = (int)(uint)result;
						} else if (result.GetType ().IsEnum) {
								result = (int)result;
						}
						return result;
				} catch (Exception ee) {
						string errMsg = "获取" + obj.GetType ().Name + "属性" + PropertyInfo.Name + "时出错" + ee.Message + ee.StackTrace;
						Logger.Error.Write (errMsg);
						throw (ee);
				}
		}

		public virtual object SetValue (object obj, object value, bool checkFormart=true)
		{
				if (checkFormart && FormatType != null) {
						var dataFormat = FormatCache.Instance.Get (FormatType);
						if (value != null) {
								value = dataFormat.SetValue (value.ToString (), PropertyInfo.DeclaringType);
						}
				}
				if (PropertyInfo.CanWrite) {
						PropertyInfo.SetValue (obj, value, null);
				}
				return value;
		}
}
 