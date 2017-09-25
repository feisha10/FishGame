using System.Collections;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Client
{
    /// <summary>
    /// 网络连接数据发送管理类，位于NetAppLayer和ScoketLayer中间
    /// </summary>
    public class ProtoLayer : MonoBehaviour
    {
        public SocketName socketName = SocketName.None;
        public bool IsServerConnet = false;
        private SocketLayer client;
        private CProto proto;
        private byte[] recvBuf;
        private int recvLen;                    //当前收到包的字节数长度的累积总和,当和服务器发的头两个字节长度一致的时候进行包解析
        private byte[] m_guid = null;
        private ConnectDelegate connectBack;
        private ConnectDelegate1 connectBack1;
        private int m_sendPackID = 0;
        private int m_recvPackID = 0;
        private Queue<byte[]> m_sendPackQueue = new Queue<byte[]>();
        const int MAX_QUEUE_LEN = 100;
        const int BUFFER_SIZE = 1024;
        private byte[] _bufferCache = new byte[BUFFER_SIZE];
        private float m_heartBeatInterval = 10f;       //心跳包的发送间隔
        const int HeartBeat_TimeOut = 20;        //心跳包的接受超时（单位:秒）
        const int ProtoConnect_TimeOut = 4;     //执行DoConnect和DoReconnect的连接时间

        private float protoConnectTime;
        private float lastPingStartTime = 0;
        private float lastPingPongTime = 0;
        private bool waitingPong = false;
        private byte[] pingByte;

        private string _host;
        private int _port;

        public void ReStartTcpSocket(ConnectDelegate1 _connectback)
        {
            Log.Debug("打开一个tcp连接StartTcpSocket");
            if (client == null)
            {
                client = new SocketLayer();
            }
            else
            {
                Close(false);
            }
            connectBack = null;
            connectBack1 = _connectback;
            client.SetConnectCallback(ConnectBack);
            proto = new CProto();
            recvBuf = new byte[BUFFER_SIZE * 64];
            recvLen = 0;
            pingByte = new byte[] { (byte)CProto.PType.ping };
            client.Connect(_host, _port);
        }

        public void StartTcpSocket(string host, int port, SocketName name, ConnectDelegate _connectback)
        {
            Log.Debug("打开一个tcp连接StartTcpSocket");
            _host = host;
            _port = port;
            if (client == null)
            {
                client = new SocketLayer();
            }
            else
            {
                Close(true);
            }
            socketName = name;
            connectBack = _connectback;
            connectBack1 = null;
            client.SetConnectCallback(ConnectBack);
            proto = new CProto();
            recvBuf = new byte[BUFFER_SIZE * 64];
            recvLen = 0;
            pingByte = new byte[] { (byte)CProto.PType.ping };
            client.Connect(host, port);
        }


        void Update()
        {
            if (NetAppLayer.Instance.isConnectSuccess && IsServerConnet)
            {
                if (waitingPong)
                {
                    if (Time.realtimeSinceStartup - lastPingStartTime > 5)
                    {
                        if(LoginView.Active)
                        {
                            NetAppLayer.Instance.LoginErroAction("ShowLoginErro");
                        }
                        else
                        {
                            Log.Debug("网络延时过大" + socketName);
                            NetAppLayer.Instance.ReConnectSocket(socketName);
                        }
                    }
                }
                else
                {
                    if ((Time.realtimeSinceStartup - lastPingStartTime + lastPingPongTime) > m_heartBeatInterval)
                    {
                        SendHeartbeat();
                    }
                }
            }

            if (client != null)
                client.Update();
        }

        public void SetSocketDelay(bool noDelay = false)
        {
            client.SetSocketDelay(noDelay);
        }

        #region 收发心跳
        byte[] _heartPack = null;
        private void SendHeartbeat()
        {
            lastPingStartTime = Time.realtimeSinceStartup;
            if (_heartPack == null)
            {
                int len = pingByte.Length;
                _heartPack = new byte[len + CProto.PackHeadSize];
                for (int i = CProto.PackHeadSize - 1; i >= 0; i--)
                {
                    _heartPack[i] = (byte)len;
                    len >>= 8;
                }
                pingByte.CopyTo(_heartPack, CProto.PackHeadSize);
            }
            client.SendBytes(_heartPack);
            waitingPong = true;
        }

        private void ReceiveHeartbeat()
        {
            lastPingPongTime = Time.realtimeSinceStartup - lastPingStartTime;
            waitingPong = false;
        }

        public float PingTime
        {
            get
            {
                if (!waitingPong)
                    return lastPingPongTime * 1000;
                return Mathf.Max(Time.realtimeSinceStartup - lastPingStartTime, lastPingPongTime) * 1000;

            }
        }

        public float HeartBeatInterval
        {
            get { return m_heartBeatInterval; }
            set { m_heartBeatInterval = value; }
        }

        #endregion


        private void ConnectBack(bool result)
        {
            if (result == false)
            {
                IsServerConnet = false;
                ConnectBackHandle(false);
                return;
            }

            protoConnectTime = Time.realtimeSinceStartup;
            Log.Debug("(m_guid == null)" + (m_guid == null));
            if (m_guid == null)
                StartCoroutine(DoConnect()); //连接
            else
                StartCoroutine(DoReconnect());//重连
        }

        private IEnumerator DoConnect()
        {
            Log.Debug("Proto DoConnect");
            recvLen = 0;
            client.SendBytes(proto.WrapperBytes(new byte[] { (byte)CProto.PType.connect }));
            Log.Debug("Proto 发送 connect数据帧");
            int size;
            while (true)
            {
                Log.Debug("Proto DoConnect 接收字节");
                RecvBytes();
                size = proto.UnWrapperBytes(recvBuf, recvLen);
                if (size >= 0)
                    break;

                if (Time.realtimeSinceStartup - protoConnectTime < ProtoConnect_TimeOut)
                {
                    yield return 2;
                }
                else
                {
                    Log.Debug("Proto DoConnect超时");
                    IsServerConnet = false;
                    ConnectBackHandle(false);
                    yield break;
                }

                if (!client.Connected)
                {
                    Log.Debug("Proto DoConnect时Socket断开");
                    IsServerConnet = false;
                    ConnectBackHandle(false);
                    yield break;
                }
            }

            if (size < 1 || recvBuf[CProto.PackHeadSize] != (byte)CProto.PType.connect)
            {
                Log.Debug("Proto DoConnect时接收到非法字节");
                IsServerConnet = false;
                ConnectBackHandle(false);
                yield break;
            }

            int headlen = CProto.PackHeadSize + 1;
            m_guid = new byte[16];
            Array.Copy(recvBuf, headlen, m_guid, 0, m_guid.Length);

            byte[] bufSendID = new byte[4];
            Array.Copy(recvBuf, headlen + m_guid.Length, bufSendID, 0, bufSendID.Length);
            m_sendPackID = BitConverter.ToInt32(bufSendID.Reverse().ToArray(), 0);

            //m_sendPackID = 0;
            m_recvPackID = 0;
            m_sendPackQueue.Clear();
            recvLen -= (CProto.PackHeadSize + size);
            for (int i = 0; i < recvLen; i++)
                recvBuf[i] = recvBuf[i + CProto.PackHeadSize + size];

            Log.Debug("Proto DoConnect时成功连接");

            IsServerConnet = true;
            ConnectBackHandle(true);
        }

        private IEnumerator DoReconnect()
        {
            Log.Debug("Proto DoReconnect");
            recvLen = 0;
            MemoryStream msm = new MemoryStream();
            msm.WriteByte((byte)CProto.PType.connect);
            proto.Encoder<Int32>(msm, m_sendPackID - m_sendPackQueue.Count);
            proto.Encoder<Int32>(msm, m_sendPackID);
            proto.Encoder<Int32>(msm, m_recvPackID);
            msm.Write(m_guid, 0, m_guid.Length);
            client.SendBytes(proto.WrapperBytes(msm.ToArray()));
            Log.Debug("Proto 发送 connect数据帧 m_guid长度：" + m_guid.Length + " 字节：" + BitConverter.ToString(m_guid));

            int size;
            while (true)
            {
                Log.Debug("Proto DoReconnect 接收字节");
                RecvBytes();
                size = proto.UnWrapperBytes(recvBuf, recvLen);
                if (size >= 0)
                    break;

                if (Time.realtimeSinceStartup - protoConnectTime < ProtoConnect_TimeOut)
                {
                    yield return 2;
                }
                else
                {
                    Log.Debug("Proto DoReconnect超时");
                    IsServerConnet = false;
                    ConnectBackHandle(false);
                    yield break;
                }

                if (!client.Connected)
                {
                    Log.Debug("Proto DoReconnect时接收到非法字节");
                    IsServerConnet = false;
                    ConnectBackHandle(false);
                    yield break;
                }
            }

            if (size != 5 + CProto.PackVerifySize || recvBuf[CProto.PackHeadSize] != (byte)CProto.PType.connect)
            {
                // 服务器拒绝重连，可以通知应用层放弃了
                Log.Error("Proto DoReconnect时接收到非法字节或者服务器已拒绝重连 字节：" + BitConverter.ToString(recvBuf, 0, recvLen));
                if (NetAppLayer.Instance != null && NetAppLayer.Instance.LoginErroAction != null)
                {
                    NetAppLayer.Instance.LoginErroAction("ShowDataError");
                }

                yield break;
            }

            int scpId = BitConverter.ToInt32(new byte[] { recvBuf[6], recvBuf[5], recvBuf[4], recvBuf[3] }, 0);
            int startId = m_sendPackID - m_sendPackQueue.Count + 1;
            byte[][] queue = m_sendPackQueue.ToArray();
            for (int packId = scpId + 1; packId <= m_sendPackID; packId++)
            {
                if(queue.Length> (packId - startId))
                   client.SendBytesAsync(queue[packId - startId]);
            }

            recvLen -= (CProto.PackHeadSize + size);
            for (int i = 0; i < recvLen; i++)
                recvBuf[i] = recvBuf[i + CProto.PackHeadSize + size];

            Log.Debug("Proto DoReconnect时成功连接");
            IsServerConnet = true;
            ConnectBackHandle(true);
        }

        private void ConnectBackHandle(bool result)
        {
            if (connectBack != null)
                connectBack(result);
            if (connectBack1 != null)
                connectBack1(result, socketName);

            lastPingStartTime = 0f;
            waitingPong = false;
        }

        public void ResetConnectType()
        {
            Log.Debug("重置Proto重连状态");

            m_guid = null;
            m_sendPackID = m_recvPackID = 0;
            IsServerConnet = false;
            m_sendPackQueue.Clear();
        }

        public bool IsReconnect()
        {
            return m_guid != null;
        }

        /// <summary>
        /// 发送proto tos消息
        /// </summary>
        /// <param name="o"></param>
        public void Send(object o)
        {
            if (client != null)
            {
                byte[] bytes = proto.ProtoToBytes((BaseToSC)o, m_sendPackID + 1);
                if (bytes != null)
                {
                    m_sendPackID++;
                    m_sendPackQueue.Enqueue(bytes);
                    while (m_sendPackQueue.Count > MAX_QUEUE_LEN)
                        m_sendPackQueue.Dequeue();

                    client.SendBytesAsync(bytes);
                }
            }

        }

        /// <summary>
        /// 接收网络字节，并保存在程序缓冲区中
        /// </summary>
        /// <returns>接收字节数，没有收到则返回0</returns>
        private int RecvBytes()
        {
            //待优化，执行缓冲区清理。不使用new
            //byte[] buffer = new byte[BUFFER_SIZE];
            //_bufferCache.Initialize();
            Array.Clear(_bufferCache, 0, BUFFER_SIZE);
            int len = client.Receive(_bufferCache);
            if (len > 0)
            {
                //Log.Debug("Proto 接收字节数：" + len);
                Array.Copy(_bufferCache, 0, recvBuf, recvLen, len);
                recvLen += len;
            }
            return len;
        }


        /// <summary>
        /// 尝试从缓冲区中解析出一个消息
        /// </summary>
        /// <param name="type">输出解析出的消息的类型</param>
        /// <returns></returns>
        private CProto.PType TryRecv(out object toc)
        {
            if (recvLen > 0)
                return proto.BytesToProto(recvBuf, ref recvLen, out toc, m_recvPackID + 1);
            toc = null;
            return CProto.PType.none;
        }

        private List<object> _resultList = new List<object>();
        /// <summary>
        /// 接收解析网络和程序缓冲区中所有的消息  只有Tcp这样处理Udp直接底层异步接受信息
        /// </summary>
        /// <returns></returns>
        public List<object> RecvAll()
        {
            int len = 0;
            bool isHeartBeat = false;
            _resultList.Clear();

            do
            {
                len = RecvBytes();
                object toc;
                CProto.PType type;

                while ((type = TryRecv(out toc)) != CProto.PType.none)
                {
                    //客户端服务器包同步计数，心跳包和ping包不计算
                    if (type == CProto.PType.pong)
                    {
                        ReceiveHeartbeat();
                        isHeartBeat = true;
                    }
                    else if (type != CProto.PType.ping)
                        m_recvPackID++;

                    if (toc != null)
                    {
                        _resultList.Add(toc);
                    }
                }
            }
            while (len > 0);

            if (_resultList.Count > 0 || isHeartBeat)
            {
                NetAppLayer.Instance.OnTocBackCheck();
            }

            return _resultList;
        }

        public bool IsConnected()
        {
            if (client != null && IsServerConnet)
            {
                return client.Connected;
            }
            return false;
        }

        public void Close(bool resetConnect)
        {
            Log.Debug("Proto Close");
            if (client != null)
            {
                waitingPong = false;
                lastPingStartTime = 0;
                connectBack = null;
                connectBack1 = null;
                StopAllCoroutines();
                client.Close();
                recvLen = 0;
                IsServerConnet = false;
                if (resetConnect)
                    ResetConnectType();
            }

        }



    }
}
