using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Net;

namespace Client
{
    public delegate void ConnectDelegate(bool result);
    public delegate void ConnectDelegate1(bool result, SocketName name);
    public delegate void ConnectDelegate2(bool result,SocketName name,Action<object[]> back);

    public class SocketLayer
	{
		private Socket client;
        public string serverHost = "";
        public int serverPort = 0;
		private ConnectDelegate connectDelegate;
        private bool _sockeException;
        private bool connectback;
        private bool connectbackResult;
        private bool _connectState = true;//该标记可以在接口发送的时候马上知道网络是否断开

        public void Update ()
        {
            if (connectback)
            {
                connectback = false;
                if (connectDelegate != null)
                    connectDelegate(connectbackResult);
            }

            if (_sockeException)
            {
                _sockeException = false;
                Close();
            }
        }

        /// <summary>
        /// 增加连接回调
        /// </summary>
        /// <param name="callback"></param>
        public void SetConnectCallback(ConnectDelegate callback)
        {
            connectDelegate = callback;
        }

		/// <summary>
		/// 创建一个异步的连接
		/// </summary>
		public void Connect(string host, int port)
		{
			try
			{
			    Close();
                Log.Debug("Connect Socket host:" + host + ", port:" + port);
				serverHost = host;
				serverPort = port;

			    AddressFamily addressFamily = AddressFamily.InterNetworkV6;
			    IPEndPoint ipEndPoint = GetIPEndPointFromHostName(serverHost, serverPort, out addressFamily);

                if (ipEndPoint==null)
                {
                    connectback = true;
                    connectbackResult = false;

                    return;
                }

                if (client == null)//创建一个TCP Socket对象
                {
                    client = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
                    client.SendTimeout = client.ReceiveTimeout = 5000;

#if !UNITY_WEBPLAYER
                    const uint dummy = 0;
                    byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
                    BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
                    BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
                    BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);
                    client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, inOptionValues);
#endif
                }
                Log.Debug("Socket BeginConnect开始连接");
			    //开始一个异步建立连接，连接成功或异常后调用ConnectCallback方法   

                client.Connect(ipEndPoint); //苹果推行ipv6时出现连不上问题，尝试改成同步连接
                Log.Debug("Socket BeginConnect连接成功");
                connectback = true;
                connectbackResult = true;

            }
            catch (SocketException e)
            {
                Log.Exception(e);

                connectback = true;
                connectbackResult = false;
            }
			catch (Exception e)
			{
                Log.Exception(e);

                connectback = true;
                connectbackResult = false;
			}
		}

        public static IPEndPoint GetIPEndPointFromHostName(string hostName, int port, out AddressFamily addressFamily)
        {
            try
            {
                var ips = Dns.GetHostAddresses(hostName);

                if (ips.Length == 0)
                {
                    //RecordMsg.Error(RecordType.GetHostAddresses, "域名解析不出IP");
                }
                else
                {
                    addressFamily = ips[0].AddressFamily;
                    return new IPEndPoint(ips[0], port);
                }
            }
            catch (Exception e)
            {
                Log.Exception(e);

               // RecordMsg.Exception(RecordType.GetHostAddresses, e.StackTrace,e.Message);
            }

            try
            {
                IPHostEntry entry = Dns.GetHostEntry(hostName);
                if (entry.AddressList.Length == 0)
                {
                    //RecordMsg.Error(RecordType.GetHostAddresses, "域名解析不出IP");
                }
                else
                {
                    addressFamily = entry.AddressList[0].AddressFamily;
                    return new IPEndPoint(entry.AddressList[0], port);
                }
            }
            catch (Exception e)
            {
                Log.Exception(e);

               // RecordMsg.Exception(RecordType.GetHostAddresses, e.StackTrace, e.Message);
            }

            addressFamily = AddressFamily.InterNetworkV6;
            return null;
        }

        /// <summary>
        /// 连接回调(失败则抛出异常)
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                //从State对象中获取Socket
                Log.Debug("ConnectBack");
//                Socket temp = (Socket)ar.AsyncState;
                //完成连接
                client.EndConnect(ar);
                //输出信息
                Log.Debug("Socket sucessfully connected to：" + client.RemoteEndPoint);
                connectback = true;
                connectbackResult = true;
            }
            catch (SocketException e)
            {
                connectback = true;
                connectbackResult = false;
                //RecordMsg.Exception(RecordType.SocketConnectException, e.ErrorCode + e.StackTrace, e.Message);
                Log.Exception(e);

                
            }
            catch (Exception e)
            {
                connectback = true;
                connectbackResult = false;
                //RecordMsg.Exception(RecordType.SocketConnectException,e.StackTrace, e.Message);
                Log.Exception(e);
                
            }
        }

        /// <summary>
        /// 设置是否关闭nagle算法，当组队副本这种实时性比较高的副本，进行关闭
        /// </summary>
        /// <param name="noDelay"></param>
        public void SetSocketDelay(bool noDelay=false)
        {
            if (Connected && client.NoDelay != noDelay)
            {
                client.NoDelay = noDelay;
            }
        }

		/// <summary>
		/// 同步方式的接收数据
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public int Receive(byte[] buffer)
		{
			if (ConnectedSimple)
			{
			    try
			    {
                    if (!client.Poll(0, SelectMode.SelectRead))
                        return 0;

                    if (client.Available > 0)
                    {
                        int len = client.Receive(buffer);
                        return len;
                    }

                    _sockeException = true; //关闭socket 重连
			    }
			    catch (SocketException e)
			    {
                   // RecordMsg.Exception(RecordType.SocketReceiveException, e.ErrorCode+e.StackTrace,e.Message);
                    Log.Exception(e);

                    _sockeException = true; //关闭socket 重连

			        return 0;
			    }
			}
			return 0;
		}

        /// <summary>
        /// 异步发送字节数据
        /// </summary>
        /// <param name="buffer"></param>
        public void SendBytesAsync(byte[] buffer)
        {
            try
            {
                if (Connected)
                {
                    client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), client);
                }
            }
            catch (SocketException e)
            {
                //RecordMsg.Exception(RecordType.SocketSendException, e.ErrorCode + e.StackTrace, e.Message);
                Log.Exception(e);

                _sockeException = true; //关闭socket 重连
            }
        }

		/// <summary>
		/// 发送数据回调
		/// </summary>
		/// <param name="ar"></param>
		private void SendCallback(IAsyncResult ar)
		{
			try
			{
				Socket temp = (Socket)ar.AsyncState;
                temp.EndSend(ar);
			}
            catch (SocketException e)
            {
                //RecordMsg.Exception(RecordType.SocketSendException, e.ErrorCode + e.StackTrace, e.Message);
                Log.Exception(e);

                _sockeException = true; //关闭socket 重连
            }
		}

		/// <summary>
		/// 同步的方式发送数据
		/// </summary>
		/// <param name="buffer"></param>
		public void SendBytes(byte[] buffer)
		{
            try
            {
                if (Connected)
                {
                    client.Send(buffer);
                }
            }
            catch (SocketException e)
            {
               // RecordMsg.Exception(RecordType.SocketSendException, e.ErrorCode + e.StackTrace, e.Message);
                Log.Exception(e);

                _sockeException = true; //关闭socket 重连
            }
		}

		/// <summary>
		/// Socket是否已连接
		/// </summary>
		/// <returns></returns>
		public bool Connected
		{
            get
            {
                if (client != null && client.Connected)
                {
                    if (!(client.Poll(0, SelectMode.SelectRead) && (client.Available == 0)))
                    {
                        return true;
                    }
                }

                return false;
            }
		}

        /// <summary>
        /// Socket是否已连接,不使用poll
        /// </summary>
        /// <returns></returns>
        public bool ConnectedSimple
        {
            get
            {
                if (client != null && client.Connected)
                    return true;
                return false;
            }
        }

		/// <summary>
		/// 关闭Socket
		/// </summary>
		public void Close()
		{
            if (client != null)
            {
                Log.Debug("Socket Close");
				try
                {
                    if (client.Connected)
                        client.Shutdown(SocketShutdown.Both);
				}
                catch (Exception e)
                {
                    Log.Exception(e);
                    //RecordMsg.Exception(RecordType.SocketCloseException, e.StackTrace,e.Message);
                }

			    try
                {
                    client.Disconnect(false);
                }

                catch (Exception e)
                {
                    Log.Exception(e);
                   // RecordMsg.Exception(RecordType.SocketCloseException, e.StackTrace, e.Message);
                }

				try
                {
                    client.Close();
                }

                catch (Exception e)
                {
                    Log.Exception(e);
                   // RecordMsg.Exception(RecordType.SocketCloseException, e.StackTrace, e.Message);
                }

                client = null;
			}
		}
	}

}
