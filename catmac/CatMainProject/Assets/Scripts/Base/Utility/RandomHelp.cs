using FlyModel;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RandomEvent
{
    static int seed = 0;

    public static int Seed
    {
        get { return ++seed; }
    }
    static int GetRandomSeed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }
 
    public static int Random(int min, int max)
    {
		if(min>=max)
		{
            //UnityEngine.Debug.LogError(min+">="+max);
			return min;
		}
        int result = new Random(Seed * ((int)DateTime.Now.Ticks)).Next(min, max);
        return result;
    }

	public static int Random(long min, long max)
	{
		int result = new Random(Seed * ((int)DateTime.Now.Ticks)).Next((int)min, (int)max);
		return result;
	}

    public static float Random(float min, float max)
    {
        //double result = new Random(Seed * ((int)DateTime.Now.Ticks)).NextDouble();
        //var dig = max - min;
        //if (dig >1)
        //{
        //    result += Random((int)dig);
        //}
        //return (float)result+min;
        return Random((int)(min * 10000), (int)(max * 10000)) / 10000f;
    }

	public static List<int> GetRandomIndex(int max)
	{
		List<int> re=new List<int>();
		for(int i=0;i<max;i++)
		{
			re.Add(i);
		}
		return re;
	}

    public static int Random(int max)
    {
        int result = new Random(Seed * ((int)DateTime.Now.Ticks)).Next(0, max);
        return result;
    }

    public static UnityEngine.Color32 RandomColor(bool randomA=false)
    {
        byte r = (byte)Random(0, 256);
        byte g = (byte)Random(0, 256);
        byte b = (byte)Random(0, 256);
        byte a = 255;
        if (randomA)
        {
            a=(byte)Random(0, 256);
        }
        return new UnityEngine.Color32(r,g,b,a);
    }

    public static short Random(short max)
    {
        int result = new Random(Seed * ((int)DateTime.Now.Ticks)).Next(0, max);
        return (short)result;
    }

    public static int GetRandomNumByString(string ss,out int min,out int max)
    {
        min=0;
        max=0;
        string [] ts=ss.Split('-');
        if (ts.Length == 2)
        {
            if (int.TryParse(ts[0], out min) && int.TryParse(ts[1], out max))
            {
                if (min <= max)
                {
                    return Random(min,max);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
        
    }
}


/// 随机计算的缓存
/// </summary>
[Serializable]
public class RandomGroup : JsonDynamicList<RandomItem>
{ 
    public RandomGroup()
    {

    }

    public int TotalValue
    {
        get;
        set;
    }

    string name = "";
    [JsonMember]
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
            if (!string.IsNullOrEmpty(name))
            {
                foreach (var ri in this)
                {
                    ri.Name = name;
                }
            }
        }
    }

    string spec = "";
    [JsonMember]
    public string Spec
    {
        get { return spec; }
        set 
        { 
            spec = value;
            if (!string.IsNullOrEmpty(spec))
            {
                foreach (var ri in this)
                {
                    ri.Spec = spec;
                }
            }
        }
    }

    public RandomItem GetRandomItem()
    {
        RandomItem result = null;

        if (TotalValue == 0)
        {
            TotalValue = this.Sum(p => p.Probability);
        }
        int tempvalue = 0;
        int thisvalue = RandomEvent.Random(0, TotalValue);
        foreach (var tt in this)
        {
            tempvalue += tt.Probability;
            if (thisvalue < tempvalue)
            {
                result = tt;
                break;
            }
        }

        return result;
    }

    public string SimpleDescription
    {
        get
        {
            string result = "";
            foreach (var award in this)
            {
                result += award.ToString() + "|";
            }
            return result.TrimEnd('|');
        }
    }

    public override string ToString()
    {
        return SimpleDescription;
    }

}

[Serializable]
public class RandomList<T>:List<T> where T:class,IProbabilityItem
{
    public RandomList()
        : base()
    {
    }

    public RandomList(IEnumerable<T> collection)
        : base(collection)
    {
    }

    public int TotalValue
    {
        get;
        set;
    }

    public T GetRandomItem()
    {
        if (TotalValue == 0)
        {
            TotalValue = this.Sum(p => p.Probability);
        }
        int tempvalue = 0;
        int thisvalue = RandomEvent.Random(0, TotalValue);
        foreach (var tt in this)
        {
            tempvalue += tt.Probability;
            if (thisvalue < tempvalue)
            {
                return tt;
            }
        }
        return null;
    }

    public List<T> GetRandomItems(int count)
    {
        List<T> result = new List<T>();
        var tl = new RandomList<T>(this);
        for (int i = 0; i < count; i++)
        {
            if (tl.Count <= 0)
            {
                break;
            }
            var t = tl.GetRandomItem();
            result.Add(t);
            tl.Remove(t);
            tl.TotalValue = 0;
        }
        return result;
    }
}

public interface IProbabilityItem
{
    int Probability
    {
        get;
        set;
    }
}
[Serializable]
public abstract class RandomItem : JsonFormatObject, IJsonDynamic, IProbabilityItem
{
    private int probability = 1;

    [JsonMember]
    public int Probability
    {
        get 
        { 
            return probability;
        }
        set
        { 
            probability = value;
            if (probability == 0)
            {
                probability = 1;
            }
        }
    }

    [JsonMember]
    public string ClassName
    {
        get
        {
            return this.GetType().Name;
        }
    }

    public abstract object Value
    {
        get;
    }

    [JsonMember]
    public string Name
    {
        get;
        set;
    }
    string spec = "";
    [JsonMember]
    public string Spec
    {
        get
        {
            return spec;
        }
        set
        {
            spec = value;
        }
    }
}

[Serializable]
public class ObjectWrap<T>:RandomItem
{
    public T UnWrapedValue
    {
        get;
        set;
    }

    public override object Value
    {
        get { return UnWrapedValue; }
    } 
}

/// <summary>
/// 作为随机计算待选项 的线性Int值随机区间 
/// </summary>
[Serializable]
public class IntRegion: RandomItem
{
    [JsonMember]
    public int Min
    {
        get;
        set;
    }

    [JsonMember]
    public int Max
    {
        get;
        set;
    }

    public override object Value
    {
        get
        {
            return RandomEvent.Random(Min, Max);
        }
    }
}

/// <summary>
/// 作为随机计算待选项 嵌套随机计算待选项
/// </summary>
[Serializable]
public class RandomGroupItem : RandomItem
{
    [JsonMember(FormatType = typeof(DynamicCollectionFormat<RandomItem, RandomGroup>))]
    public RandomGroup RandomGroup
    {
        get;
        set;
    }

    public override object Value
    {
        get { return RandomGroup; }
    }
}

