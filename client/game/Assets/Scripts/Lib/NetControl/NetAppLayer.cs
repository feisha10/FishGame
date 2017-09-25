using UnityEngine;
using Client;
using System;
using System.Collections.Generic;

public delegate void TocHandle(object toc);

/// <summary>
/// 网络连接管理类
/// </summary>
public partial class NetAppLayer : SingletonMonoBehaviour<NetAppLayer>
{
    public bool isConnectSuccess = false; //是否登陆成功
    public bool isConnectUdp = false;
    public Action<string> LoginErroAction; //登陆出错回调

    private ProtoInfo[] TcpPros = new ProtoInfo[2]; //Tcp目前只有两个有新的再加

    private Dictionary<Type, TocHandle> handleDic; //模块manager初始化添加的函数映射

    private List<object> waiteInvokToc = new List<object>();
    private bool CanInvokeToc = true; //收到Toc立刻执行

    private int _CheckingNet = 0; //网络连接检测状态 0:初始状态 1:检测中 3:弹出提示中
    private float _checkNetStartTime; //开始检查网络时间
    private float _netAlertTime; //网络延迟提示时间     

    protected override void Awake()
    {
        base.Awake();
        handleDic = new Dictionary<Type, TocHandle>();
        InitNoCheckId();
    }


    public void SetNotCheckId(int protoId, bool isAdd)
    {
        if (!_notCheckIdDic.ContainsKey(protoId) && isAdd)
            _notCheckIdDic.Add(protoId, protoId);
        else if (_notCheckIdDic.ContainsKey(protoId) && !isAdd)
        {
            _notCheckIdDic.Remove(protoId);
        }
    }

    private int _fram = 0; //计数

    private void Update()
    {
        _fram++;
        if (_fram == 3)
        {
            _fram = 0;
            if (_CheckingNet == 1)
            {
                if (Time.realtimeSinceStartup >= _netAlertTime)
                {
                    _CheckingNet = 2;
                    if (LoginErroAction != null)
                    {
                        LoginErroAction("ShowSendingUI");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 判断发送的消息
    /// </summary>
    /// <param name="protoId"></param>
    public void StartCheckSendTos(int protoId)
    {
        if (_CheckingNet != 0)
            return;
        if (_notCheckIdDic.ContainsKey(protoId) || !isConnectSuccess)
            return;

        _checkNetStartTime = Time.realtimeSinceStartup;
        _netAlertTime = _checkNetStartTime + 4;
        _CheckingNet = 1;
    }

    /// <summary>
    /// 有包返回
    /// </summary>
    public void OnTocBackCheck()
    {
        if (_CheckingNet < 1) //0
            return;

        if (_CheckingNet == 1)
            ResetNetCheck();
        else if (_CheckingNet == 2)
        {
            if (LoginErroAction != null)
            {
                LoginErroAction("RemoveSendingUI");
            }
        }
    }

    /// <summary>
    /// 重置检查状态
    /// </summary>
    public void ResetNetCheck()
    {
        _CheckingNet = 0;
    }




    /// <summary>
    /// 设置连接出错回调   连接之前需要有设置这个，会在其他地方设置此处加次回调为和UI分离
    /// </summary>
    /// <param name="callBack"></param>
    public void SetConnectErrorBack(Action<string> callBack)
    {
        LoginErroAction = callBack;
    }


   
    /// <summary>
	/// 连接Tcp服务器
	/// </summary>
	/// <param name="host"></param>
	/// <param name="port"></param>
	public void ConnectTcpServer(string host, int port, string bgphost, int bgpport, SocketName _socketName,ConnectDelegate callBack,bool bgpConnect)
    {
        for (int i = 0; i < TcpPros.Length; i++)
        {
            if(TcpPros[i]==null)
            {
                TcpPros[i]=new ProtoInfo();
                TcpPros[i].socketName = _socketName;
                TcpPros[i].host = host;
                TcpPros[i].port = port;
                TcpPros[i].bdphost = bgphost;
                TcpPros[i].bdpport = bgpport;
                TcpPros[i].outSetWork = true;
                TcpPros[i].pro = gameObject.AddComponent<ProtoLayer>();
            }
            if (TcpPros[i].socketName==_socketName)
            {
                TcpPros[i].host = host;
                TcpPros[i].port = port;
                TcpPros[i].bdphost = bgphost;
                TcpPros[i].bdpport = bgpport;
                TcpPros[i].outSetWork = true;
                if(bgpConnect)
                    TcpPros[i].pro.StartTcpSocket(bgphost, bgpport, _socketName, callBack);
                else
                    TcpPros[i].pro.StartTcpSocket(host, port, _socketName, callBack);
                break;
            }
        }
	}
    int[] _connectCount = new int[2];
    public void ReConnectSocket(SocketName _socketName = SocketName.None)
    {
        for (int i = 0; i < TcpPros.Length; i++)
        {
            if (TcpPros[i] != null && TcpPros[i].socketName == _socketName || _socketName == SocketName.None)
            {
                TcpPros[i].outSetWork = true;
                TcpPros[i].pro.ReStartTcpSocket(ReconnectConnectCallback);
            }
        }
    }


    /// <summary>
    /// socket链接回调函数
    /// </summary>
    /// <param name="result">true 链接成功，false 链接失败</param>
    private void ReconnectConnectCallback(bool result, SocketName socketName)
    {
        Log.Debug("ReConnectSocket ConnectCallback result:" + result);
        SocketName _socketName = socketName;
        if (result)
        {
            if (socketName == SocketName.MainSocket)
                _connectCount[0] = 0;
            else
                _connectCount[1] = 0;
            LoginManager.Instance.connectResult = ConnectResultType.SocketConnectSuccess;
            NetReconnectManager.Instance.HaseConnected = true;
            NetReconnectManager.Instance.RemoveConnectiong();
        }
        else
        {
            if (NetReconnectManager.Instance.CanReconnect == false)
            {
                if (socketName == SocketName.MainSocket)
                    _connectCount[0] = 0;
                else
                    _connectCount[1] = 0;
                return;
            }
            if (socketName == SocketName.MainSocket)
                _connectCount[0] ++;
            else
                _connectCount[1] ++;
            //按场景设置重连次数
            float delay = 2.0f;
            int MaxCount = NetReconnectManager.CONNECT_SOCKET_COUNT;

         /*   if (ChangeSceneManager.Instance.currentSceneName == SceneName.FightScene)
            {
                MaxCount = NetReconnectManager.LOGIN_CONNECT_COUNT;
                delay = 1.0f;
            }
            else if (ChangeSceneManager.Instance.currentSceneName == SceneName.FightScene)
            {
                MaxCount = NetReconnectManager.FIGHT_CONNECT_COUNT;
            }*/

            if ((socketName == SocketName.MainSocket)?_connectCount[0] < MaxCount: _connectCount[1] < MaxCount)
            {
                //TimerManager.Instance.SetTimeOut(ToReconnect, delay, true, false, true, _socketName);
            }
            else
            {
                Log.Debug("LoginModel ConnectCallback " + _connectCount + "次连接失败后返回");
                if (socketName == SocketName.MainSocket)
                    _connectCount[0] = 0;
                else
                    _connectCount[1] = 0;
                LoginManager.Instance.connectResult = ConnectResultType.SocketConnectFail;
                NetReconnectManager.Instance.ShowUDPError();
            }
        }
    }

    /// <summary>
    /// 网络重连
    /// </summary>
    private void ToReconnect(object[] _socketName)
    {
        if (NetReconnectManager.Instance.CanReconnect == false)
            return;

        //如果连接上了就不重连
        if (LoginManager.Instance.connectResult == ConnectResultType.SocketConnectSuccess)
            return;

        Log.Debug("LoginModel ConnectCallback 第" + _connectCount + "次重连开始");
        string socket = _socketName[1].ToString();
        SocketName socketName = (SocketName)Enum.Parse(typeof(SocketName), socket, true);
        Log.Debug("socketName" + socketName);
        ReConnectSocket(socketName);
    }
    /// <summary>
    /// 是否连接中
    /// </summary>
    /// <returns></returns>
    public bool IsTcpConnected(SocketName _socketName = SocketName.MainSocket)
	{
	    for (int i = 0; i < TcpPros.Length; i++)
	    {
            if (TcpPros[i] != null && TcpPros[i].socketName == _socketName)
                return TcpPros[i].pro.IsConnected();
	    }
		return false;
	}

	/// <summary>
	/// 是否是一个重练接, false 为 未登陆状态
	/// </summary>
	/// <returns></returns>
    public bool IsTcpReconnect(SocketName _socketName = SocketName.MainSocket)
    {
        for (int i = 0; i < TcpPros.Length; i++)
        {
            if (TcpPros[i] != null && TcpPros[i].socketName == _socketName)
                return TcpPros[i].pro.IsReconnect();
        }
        return false;
    }

    public bool IsTcpConnectServer()
    {
        for (int i = 0; i < TcpPros.Length; i++)
        {
            if (TcpPros[i] != null && TcpPros[i].socketName == SocketName.MainSocket)
                return TcpPros[i].pro.IsServerConnet;
        }
        return false;
    }

    /// <summary>
    /// 设置是否关闭nagle算法，当组队副本这种实时性比较高的副本，进行关闭
    /// </summary>
    /// <param name="noDelay"></param>
    public void SetSocketDelay(bool noDelay = false,SocketName _socketName = SocketName.MainSocket)
    {
        for (int i = 0; i < TcpPros.Length; i++)
        {
            if (TcpPros[i] != null && TcpPros[i].socketName == _socketName)
                TcpPros[i].pro.SetSocketDelay(noDelay);
        }
    }


	/// <summary>
	/// 关闭网络Socket
	/// </summary>
	/// <param name="resetConnect">完全重置连接，如果重练也连接不上后可以重置</param>
    public void CloseServer(bool resetConnect = false, SocketName _socketName = SocketName.MainSocket)
	{
        for (int i = 0; i < TcpPros.Length; i++)
        {
            if (TcpPros[i] != null && TcpPros[i].socketName == _socketName)
            {
                TcpPros[i].pro.Close(resetConnect);
                TcpPros[i].outSetWork = false;
            }
        }
	}

    public bool IsReconnect( SocketName _socketName = SocketName.MainSocket)
    {
        for (int i = 0; i < TcpPros.Length; i++)
        {
            if (TcpPros[i] != null && TcpPros[i].socketName == _socketName)
            {
                return TcpPros[i].pro.IsReconnect();
            }
        }
        return false;

    }

	/// <summary>
	/// 增加一个TOC类型的处理方法，可以在一个单独的Model脚本中设置好回调，来统一管理一组的Toc处理
	/// </summary>
	/// <param name="type"></param>
	/// <param name="handler"></param>
	public void AddTocHandle(Type type, TocHandle handler)
	{
		if (handleDic.ContainsKey(type))
			handleDic[type] = handler;
		else
			handleDic.Add(type, handler);
	}

	/// <summary>
	/// 发送tos消息
	/// </summary>
	/// <param name="tos"></param>
    public void SendTos(object tos, SocketName _socketName = SocketName.MainSocket)
    {
        for (int i = 0; i < TcpPros.Length; i++)
        {
            if (TcpPros[i] != null && TcpPros[i].socketName == _socketName)
            {
                Log.InfoVO(1, tos);
                TcpPros[i].pro.Send(tos);
            }
        }
    }


    #region  因为后端协议发送顺序不一定，有些协议需要提前执行然后才能执行其他协议的处理
    private Type stopToc;
    private Type[] canInvokeLsit;
    /// <summary>
    /// 设置是否收到toc之后立刻执行（针对登陆流程设计）
    /// </summary>
    /// <param name="bol">是否立即执行</param>
    /// <param name="_stopToc">需要等待的先执行的协议此协议之后执行之前已经收到的消息，并直接执行</param>
    /// <param name="_canInvokeLsit">在等待需要先执行的消息的时可以继续执行的消息列表</param>
    public void SetInvokeAble(bool bol, Type _stopToc = null,Type[] _canInvokeLsit=null)
    {
        stopToc = _stopToc;
        canInvokeLsit = _canInvokeLsit;
        CanInvokeToc = bol;
    }

    #endregion
    /// <summary>
	/// 每一帧Update中进行接收和处理后端返回的接口
	/// </summary>
	/// <returns></returns>
	public void ProcessToc()
	{
	    for (int i = 0; i < TcpPros.Length; i++)
	    {
	        if(TcpPros[i]!=null&&TcpPros[i].outSetWork&&TcpPros[i].pro.IsConnected())
	        {
                ProcessToc(TcpPros[i].pro.RecvAll());
	        }
	    }
	}

    void ProcessToc(List<object> tocs)
    {
        if (tocs != null)
        {
            for (int i = 0; i < tocs.Count; i++)
            {
                object toc = tocs[i];

                Type type = toc.GetType();
                Log.InfoVO(2, toc);

                if (CanInvokeToc == false)
                {
                    if (type == stopToc)
                    {
                        InVokeToc(type, toc);

                        CanInvokeToc = true;

                        int leng = waiteInvokToc.Count;
                        for (int j = 0; j < leng; j++)
                        {
                            object ivkToc = waiteInvokToc[j];
                            InVokeToc(ivkToc.GetType(), ivkToc);
                        }

                        waiteInvokToc.Clear();
                    }
                    else
                    {
                        bool isAdd = true;
                        for (int j = 0; j < canInvokeLsit.Length; j++)
                        {
                            if (type == canInvokeLsit[i])
                            {
                                InVokeToc(type, toc);
                                isAdd = false;
                                break;
                            }
                        }
                        if (isAdd)
                            waiteInvokToc.Add(toc);
                    }
                }
                else
                {
                    InVokeToc(type, toc);
                }
            }
        }
    }

    /// <summary>
    /// 响应Toc
    /// </summary>
    private void InVokeToc(Type type,object toc)
    {
        try
        {
            if (handleDic.ContainsKey(type))
            {
                handleDic[type](toc);
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }

    public float GetPingTime(SocketName _socketName = SocketName.MainSocket)
    {
        for (int i = 0; i < TcpPros.Length; i++)
        {
            ProtoInfo proto = TcpPros[i];
            if (proto != null && proto.socketName == _socketName)
            {
                proto.pro.HeartBeatInterval = 1;
                return proto.pro.PingTime;
            }
        }
        return 0;
    }

   


    public class ProtoInfo
    {
        public ProtoLayer pro;
        public SocketName socketName;
        public string host;
        public int port;
        public string bdphost;
        public int bdpport;
        public bool outSetWork;  //外部去强制设置时候工作
    }
}
