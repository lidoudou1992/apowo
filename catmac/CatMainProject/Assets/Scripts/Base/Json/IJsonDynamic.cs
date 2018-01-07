using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Json;

 
public interface IJsonDynamic
{
	string ClassName { get; }
}

public interface IJsonDynamicCollection
{
	void InitializeFromJsonValue(JsonValue jv, Type type, Type parentType);
	void InitializeFromString(string jv, Type type, Type parentType);
	Type BaseType { get; }
}

public interface IExtraCollection:IList
{
	Type ItemType { get; }
}
 
