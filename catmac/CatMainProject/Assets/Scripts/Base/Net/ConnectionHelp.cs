using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

 
public static class ConnectionHelp
{
    #region 静态方法
    /// <summary>
    /// 接收解码器
    /// </summary>
    public static Encoding Encoding = Encoding.UTF8;
 

    public delegate bool SocketAsyncMethod(SocketAsyncEventArgs args);
    public static void InvokeAsyncMethod(this Socket socket, SocketAsyncMethod method, EventHandler<SocketAsyncEventArgs> callback, SocketAsyncEventArgs args)
    {
        if (callback == null)
            return;
        if (method == null)
            return;
        if (!method(args))
            callback(socket, args);
    }

    static public IList<T> RemoveToList<T>(this IList<T> tlist, int startindex, int count)
    {
        var nlist = new List<T>();
        for (int i = 0; i < count; i++)
        {
            nlist.Add(tlist[startindex]);
            tlist.RemoveAt(startindex);
        }
        return nlist;
    }
 

    public static IPAddress GetMyIP()   //获取本地IP   
    {
        IPAddress thisip = null;
        string cname = Dns.GetHostName();
        IPAddress[] ips = Dns.GetHostAddresses(cname);

        thisip = ips.First(p => p.AddressFamily == AddressFamily.InterNetwork);
        return thisip;
    }

    #endregion

}
