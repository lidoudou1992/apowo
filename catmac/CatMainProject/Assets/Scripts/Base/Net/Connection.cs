using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;

public enum SerializeType
{
    Normal = 0,
    String = 9999999,
    /// <summary>
    /// 二进制序列化
    /// </summary>
    Binary = 1000000,
}
 
/// <summary>
/// SocketConnection连接类
/// </summary>
public class Connection
{  
    #region Member

    /// <summary>
    /// 全局发送次数
    /// </summary>
    public static long sendtime = 0;
    /// <summary>
    /// 全局发送数据大小(byte)
    /// </summary>
    public static long senddata = 0;
    /// <summary>
    /// 全局接收次数
    /// </summary>
    public static long recievetime = 0;
    /// <summary>
    /// 全局接收数据大小(byte)
    /// </summary>
    public static long receivedata = 0;
	public static DateTime lastSendTime=DateTime.Now;

    #region 数据接收分流处理
    List<byte> TempBytes = new List<byte>();
    int thislength = 0;
    SerializeType ThisReceiveSerializeType = SerializeType.Normal;
    #endregion

    /// <summary>
    /// 连接时间
    /// </summary>
    public DateTime ConnectedTime;

    /// <summary>
    /// 断开连接时候的事件
    /// </summary>
    public Action DisconnectedCallback;

    /// <summary>
    /// 发送缓存
    /// </summary>
	SendBuff sendbuf=new SendBuff();

	public SendBuff Sendbuf {
		get {
			return sendbuf;
		}
	}

    /// <summary>
    /// 该连接的远程终结点
    /// </summary>
    public IPEndPoint TargetIP
    {
        get;
        set;
    }

    /// <summary>
    /// 远程IP
    /// </summary>
    public string TargetIPString
    {
        get
        {
            if (TargetIP != null)
                return TargetIP.ToString();
            else
                return "空连接";
        }
    }
	
    public Socket Socket;

    public bool IsConnected
    {
        get
        {
            if (Socket == null || !Socket.Connected)
                return false;
            return true;
        }
    }

    #endregion
    
    #region 构造初始化

    public Connection()
    { 
        
    }

    public bool IsReconnecting;

	public void CheckSend()
	{
		var passed=(DateTime.Now-lastSendTime).TotalSeconds;
        //if (passed > 5 && sendbuf.Count > 0)
        //{
        //    Send(null, 0, 0);
        //}

        if (passed > 300)
        {
            if (IsConnected==false && IsReconnecting==false)
            {
                IsReconnecting = true;
                Reconnecting();
            }
        }
    }

    public virtual void Reconnecting() { }

    public void Initialize(Socket socket)
    {
        Socket = socket;
        //Socket.Blocking = false;

        Socket.ReceiveTimeout = 0;
        Socket.SendTimeout = 0;
        //Socket.Blocking = false;

        ConnectedTime = DateTime.Now;

        Receive();
    }

	public void Close()
	{
		if(Socket!=null)
		{
			Socket.Close();
			Socket=null;
		}
	}
	
    #endregion

    #region Public Methods
    private void SendData(byte[] data, Action<int> onSendDone)
    {
        //var s = string.Format("Send[{0}][{1}][{2}]", IsConnected, TargetIP, data.Length);
        //UnityEngine.Debug.Log(s);

        try
        {

            SocketError se = SocketError.Success;
            int re = Socket.Send(data, 0, data.Length, SocketFlags.None);
            onSendDone(re);
            //var re = Socket.BeginSend(data, offset, length, SocketFlags.None, out se, SendDone, onSendDone);
            //Logger.OnSend.WriteLog("SendData[{0}]", data.Length);
            //UnityEngine.Debug.Log("===== GameMainProject Send Over=====");
            if (se != SocketError.Success)
            {
                UnityEngine.Debug.Log(se.ToString());
            }

        }
        catch (SocketException e)
        {
            if (e.SocketErrorCode != SocketError.NoBufferSpaceAvailable && e.SocketErrorCode != SocketError.WouldBlock &&
                e.SocketErrorCode != SocketError.ConnectionReset && e.SocketErrorCode != SocketError.ConnectionAborted)
            {
                Logger.Error.Write("[" + TargetIPString + "],data.count:" + data.Length + ",offset:" + 0 + "\r\n" +
                    e.Message + e.StackTrace + "\r\nSocketErrorCode:" + e.SocketErrorCode);

                UnityEngine.Debug.Log("[" + TargetIPString + "],data.count:" + data.Length + ",offset:" + 0 + "\r\n" +
                    e.Message + e.StackTrace + "\r\nSocketErrorCode:" + e.SocketErrorCode);
            }
        }
        catch (Exception e)
        {
            Logger.Error.Write("[" + TargetIPString + "],data.count:" + data.Length + ",offset:" + 0 + ",sendLength:" + data.Length + "\r\n" + e.Message + e.StackTrace);
            UnityEngine.Debug.Log("[" + TargetIPString + "],data.count:" + data.Length + ",offset:" + 0 + ",sendLength:" + data.Length + "\r\n" + e.Message + e.StackTrace);
        }
    }
    void SendDone(IAsyncResult result)
    {
        var message = result.AsyncState as Action<int>;
        
        try
        {
            var bytesTransferred = Socket.EndSend(result);
            message(bytesTransferred);
        }
        catch (Exception e)
        {
            Logger.Error.Write(e.Message + e.StackTrace);
        }
    }

    internal void Receive()
    {
        try
        {
            byte[] buff = new byte[4096];
            Socket.BeginReceive(
                 buff,
                 0,
                 buff.Length,
                 System.Net.Sockets.SocketFlags.None,
                 ReceiveDone,
                 buff
             );
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message + e.StackTrace);
            Logger.Error.Write(e.Message + e.StackTrace);
        }
    }

    void ReceiveDone(IAsyncResult result)
    {
        byte[] buff = result.AsyncState as byte[];
        var bytesTransferred = Socket.EndReceive(result);
        if (bytesTransferred > 0)
        {
            Byte[] data = new Byte[bytesTransferred];
            Array.Copy(buff, 0, data, 0, data.Length);
            recievetime++;
            receivedata += data.Length;
            OnDataReceived(data);
            Receive();
        }
        else
        {
            OnDisconnected();
        }
      
    }

    public bool Send(byte[] content,int offset,int length)
    {
        try
        {
            if (Socket != null && Socket.Connected)
            {
                lastSendTime = DateTime.Now;
                if (sendbuf == null)
                {
                    return false;
                }
                if (content != null)
                {
                    sendbuf.AddRange(content,offset,length);
                }

                if (sendbuf.Count > 0)
                {
                    SendData(sendbuf.Data, onSendDone: (int n) =>
                        {
                            if (n > 0)
                            {
                                sendbuf.RemoveRange(0, n);
                                sendtime++;
                                senddata += n;

                            }
                        });
                }
            }
            else
            {
                if (Socket != null)
                {
                    UnityEngine.Debug.Log(Socket.Connected);
                }

                OnDisconnected();
            }
            return sendbuf.Count == 0;
        }
        catch (Exception e)
        {
            Logger.Error.Write(e.Message + e.StackTrace);

            sendbuf.Clear();
            return false;
        }
    }

 
    public bool SendJson(string cmd, object param)
    {
        //DateTime begin = DateTime.Now;
        bool sendok = false;
        Dictionary<string, object> argv = new Dictionary<string, object>();

        argv.Add("Cmd", cmd);
        argv.Add("Par", param);

        var result =JsonSerializer.Serialize(argv);

        sendok = SendUnMarshal(result.ToString());
    
        return sendok;
    }

    public bool SendUnMarshal(string msg)
    {
        byte[] tt = ConnectionHelp.Encoding.GetBytes(msg);
		Logger.OnSend.Write("Send[{0}]", msg);
        return SendObject(tt, SerializeType.String);
    }
    byte[] outputBuffer = new byte[4096];
    public void SendOpcodeData(int opcode, byte[] data)
    {
        int len = data == null ? 0 : data.Length;
        if(outputBuffer.Length<12+len)
        {
            outputBuffer = new byte[12 + len];
        }
        Buffer.BlockCopy(BitConverter.GetBytes((int)SerializeType.Binary), 0, outputBuffer, 0, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(len + 4), 0, outputBuffer, 4, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(opcode), 0, outputBuffer, 8, 4);
        if (data != null)
        {
            Buffer.BlockCopy(data, 0, outputBuffer, 12, len);
        }
        Send(outputBuffer,0,len+12);
    }
   

    public bool SendObject(byte[] data, SerializeType SerializeType)
    {
        try
        {
            if (outputBuffer.Length < 12 + data.Length)
            {
                outputBuffer = new byte[8 + data.Length];
            }
          
            Buffer.BlockCopy(BitConverter.GetBytes((int)SerializeType), 0, outputBuffer, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(data.Length), 0, outputBuffer, 4, 4);
            Buffer.BlockCopy(data, 0, outputBuffer, 8, data.Length);
            Send(outputBuffer, 0, 8 + data.Length);
            return true;
        }
        catch (Exception e)
        {
            Logger.Error.Write(e.Message + e.StackTrace);
            return false;
        }
    }

    #endregion

    #region Events

    private void OnDataReceived(byte[] data)
    {
        try
        {
            //UnityEngine.Debug.LogWarning("接收到数据" + data.Length);
            TempBytes.AddRange(data);
            if (ThisReceiveSerializeType == SerializeType.Normal)
            {
                if (TempBytes.Count >= 8)
                {
                    var ts = TempBytes.ToArray();
                    int result = BitConverter.ToInt32(ts, 0);
                    if (Enum.IsDefined(typeof(SerializeType), result))
                    {
                        ThisReceiveSerializeType = (SerializeType)result;
                        if (this.ThisReceiveSerializeType != SerializeType.Normal)
                        {
                            thislength = BitConverter.ToInt32(ts, 4);
                            TempBytes.RemoveRange(0, 8);
                        }
                        else
                        {
                            PutOutString(new byte[0]);
                            return;
                        }
                    }
                    else
                    {
                        PutOutString(new byte[0]);
                        return;
                    }
                }
            }
            if (ThisReceiveSerializeType != SerializeType.Normal && TempBytes.Count >= thislength)
            {
                var newlist = TempBytes.RemoveToList(0, thislength);
                PutOutReceivedData(ThisReceiveSerializeType, newlist.ToArray());
                if (TempBytes.Count > 0)
                {
                    OnDataReceived(new byte[0]);
                }
            }

        }
        catch (Exception e)
        {
            Logger.Fatel.Write(e.Message + e.StackTrace);
            TempBytes.Clear();
            ThisReceiveSerializeType = SerializeType.Normal;
        }
    }
   
    public void PutOutString(byte[] data)
    {
        int index = TempBytes.FindIndex(p => p == 10);
        IList<byte> newlist;
        if (index == -1)
        {
            newlist = TempBytes.RemoveToList(0, TempBytes.Count);
        }
        else
        {
            newlist = TempBytes.RemoveToList(0, index + 1);
        }
        DataReceivedCallback(SerializeType.String,newlist.ToArray());
        if (TempBytes.Count > 0)
        {
            OnDataReceived(new byte[0]);
        }
    }

    protected virtual void DataReceivedCallback(SerializeType st,byte[] data)
    {

    }
    /// <summary>
    /// 输出反序列化后的数据
    /// </summary>
    /// <param name="remoteEndPoint"></param>
    /// <param name="callback"></param>
    void PutOutReceivedData(SerializeType serializeType, byte[] data)
    {
        try
        {
            DataReceivedCallback(serializeType, data);
        }
        catch (Exception ee)
        {
            Logger.Fatel.Write(ee.Message + ee.StackTrace);
        }
        finally
        {
            ThisReceiveSerializeType = SerializeType.Normal;
        }
    }

    private void OnDisconnected()
    {
        try
        {
            if (DisconnectedCallback != null)
            {
                DisconnectedCallback();
            }
        }
        catch (Exception e)
        {
            Logger.Fatel.Write(e.Message + e.StackTrace);
        }
    }
    #endregion

}


public class SendBuff
{
    List<byte> data;

    public byte[] Data
    {
        get
        {
            lock (data)
            {
                return data.ToArray();
            }
        }
    }

    public SendBuff()
    {
        data = new List<byte>();
    }
 
    public void AddRange(byte[] content, int offset, int length)
    {
        try
        {

            int tlength = length;
            //if (offset + length > content.Length)
            //{
            //    tlength = content.Length - offset;
            //}

            for (int i = offset; i < offset+tlength; i++)
            {
                data.Add(content[i]);
            }

        }
        catch (Exception e)
        {
            Logger.Error.Write("往发送缓存中增加数据的时候出错,Datalength:[{0}]contentlength[{1}]offset[{2}][{3}]\r\n[{4}]", data.Count, content.Count(),offset, e.Message, e.StackTrace);
        }
    }

    public void RemoveRange(int offset, int count)
    {
        lock (data)
        {
            if (data.Count > count)
                data.RemoveRange(offset, count);
            else
                data.Clear();
        }
    }

    public int Count
    {
        get
        {
            return data.Count;
        }
    }

    public void Clear()
    {
        lock (data)
        {
            data.Clear();
        }
    }

}




