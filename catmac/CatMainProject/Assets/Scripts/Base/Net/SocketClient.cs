using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using FlyModel;
using System.Json;
using System.Timers;
using System.Runtime.InteropServices;

public class SocketClient : Connection
{
    public Action ConnectedCallback;
    //string targetIpString = "localhost";
    int targetPort = 2234;
    IPAddress TargetIPAddress = null;
    public string token = "111111111111111111111111111111";

    #region ipv6
    [DllImport("__Internal")]
    private static extern string getIPv6(string mHost, string mPort);

    //"192.168.1.1&&ipv4"
    public static string GetIPv6(string mHost, string mPort)
    {
#if UNITY_IPHONE && !UNITY_EDITOR   
        		string mIPv6 = getIPv6(mHost, mPort);
        		return mIPv6;
#else
        return mHost + "&&ipv4";
#endif
    }

    public void getIPType(string serverIp, string serverPorts, out String newServerIp, out AddressFamily mIPType)
    {
        mIPType = AddressFamily.InterNetwork;
        newServerIp = serverIp;
        try
        {
            string mIPv6 = GetIPv6(serverIp, serverPorts);
            if (!string.IsNullOrEmpty(mIPv6))
            {
                string[] m_StrTemp = System.Text.RegularExpressions.Regex.Split(mIPv6, "&&");
                if (m_StrTemp != null && m_StrTemp.Length >= 2)
                {
                    string IPType = m_StrTemp[1];
                    if (IPType == "ipv6")
                    {
                        newServerIp = m_StrTemp[0];
                        mIPType = AddressFamily.InterNetworkV6;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("GetIPv6 error:" + e);
        }

    }
    #endregion




    void OnConnected()
    {
        if (ConnectedCallback != null)
        {
            ConnectedCallback();
        }
    }

    private AddressFamily newAddressFamily = AddressFamily.InterNetwork;
    public SocketClient(string ip, int port)
    {
        //targetIpString = ip;
        targetPort = port;

        #region ipv6
        String newServerIp = "";
        
        getIPType(ip, port.ToString(), out newServerIp, out newAddressFamily);  // 这里newServerIp已经是IPv6类型的IP地址了
        if (!string.IsNullOrEmpty(newServerIp)) { ip = newServerIp; }
        //socketClient = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
        Debug.Log("Socket AddressFamily :" + newAddressFamily.ToString() + " ServerIp:" + ip + " newServerIp:" + newServerIp);
        #endregion

        if (ip.ToLower() == "local" || ip.ToLower() == "localhost")
        {
            TargetIPAddress = ConnectionHelp.GetMyIP();
        }
        else
        {
            //TargetIPAddress = Dns.GetHostAddresses(ip).First(p => p.AddressFamily == AddressFamily.InterNetwork);
            TargetIPAddress = Dns.GetHostAddresses(ip).First(p => p.AddressFamily == newAddressFamily);
            Debug.Log("TargetIPAddress :" + TargetIPAddress);   // 这里的TargetIPAddress已经是IPv6类型的IP地址了
        }
    }

    /// <summary>
    /// 返回IPv6地址
    /// </summary>
    /// <returns></returns>
    public void xixi()
    {
    }

    public bool IsConnecting = false;
	Timer timer_connection;
	Socket socket=null;
    public void TryConnect()
    {
        if (IsConnecting)
        {
            return;
        }
        if (socket != null)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        if (timer_connection != null)
        {
            timer_connection.Dispose();
        }
        else
        {
            timer_connection = new Timer();
        }
        timer_connection.Interval = 12000;

        timer_connection.Elapsed += timerConnectionTick;
        timer_connection.Start();

        //Logger.Server.Write("准备连接" + targetIpString + ":" + targetPort);
        IsConnecting = true;
        IPEndPoint endPoint = new IPEndPoint(TargetIPAddress, targetPort);
        Debug.Log(string.Format("实际连接的服务器地址和端口：{0}，服务器地址：{1}，端口：{2}",endPoint,TargetIPAddress,targetPort));

        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.RemoteEndPoint = endPoint;

        args.Completed += args_Completed;

        //socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.ReceiveTimeout = 30;
        args.UserToken = socket;
        socket.InvokeAsyncMethod(socket.ConnectAsync, args_Completed, args);
    }

    public Action ReconnectingCallback;
    public override void Reconnecting()
    {
        base.Reconnecting();

        if (ReconnectingCallback != null)
        {
            ReconnectingCallback();
        }
    }

    public void timerConnectionTick(object sender,ElapsedEventArgs e)
	{
		if (!socket.Connected)
		{
            Debug.Log("连接超时");
            IsConnecting = false;
            IsReconnecting = false;
            timer_connection.Stop();

            if (DisconnectedCallback != null)
            {
                DisconnectedCallback();
            }
        }
	}
    private void args_Completed(object sender, SocketAsyncEventArgs e)
    {
        try
        {
            Logger.Connection.Write("args_Completed "+e.SocketError);
			timer_connection.Stop();
            if (e.SocketError != SocketError.Success)
            {
                Logger.Error.Write("连接出错"+e.SocketError.ToString());
                if(e.SocketError==SocketError.ConnectionRefused)
                {
                    Logger.Error.Write("没有可用的网络连接");
                }
                else
                {
                    Logger.Error.Write("网络出错:" + e.SocketError.ToString());
                }
                return;
            }
            if (e.LastOperation == SocketAsyncOperation.Connect)
            {
                Logger.Connection.Write("连接成功");
                Debug.Log("连接成功");
                Byte[] RecData = new Byte[4096];
                e.SetBuffer(RecData, 0, RecData.Length);

                e.Completed -= args_Completed;

                var socket = sender as Socket;
             
                Initialize(socket);
                OnConnected();
            }
            else
            {
                Logger.Error.Write("网络出错 LastOperation :" + e.LastOperation);
            }
        }
        catch (Exception ee)
        {
            Debug.LogError(ee.Message + ee.StackTrace);
        }
        finally
        {
            IsConnecting = false;
            IsReconnecting = false;
        }
    }

    #region   User
    public Queue<byte[]> ByteCommands = new Queue<byte[]>();

    public void AnalysisBytes(byte[] bytes)
    {
        ByteCommands.Enqueue(bytes);
    }

 
    protected override void DataReceivedCallback(SerializeType st, byte[] data)
    {
        try
        {    
            if (st == SerializeType.Binary)
            {
                AnalysisBytes(data);
            }
            else
            {
                UnityEngine.Debug.LogWarning("收到未解析的数据 " + st + " " + ConnectionHelp.Encoding.GetString(data));
            }
        }
        catch (Exception ee)
        {
            Logger.Error.Write(ee.Message + ee.StackTrace);
        }
    }


    public bool Send(string msg)
    {
        return SendUnMarshal(msg);
    }
    #endregion
}


