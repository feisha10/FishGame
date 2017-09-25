using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System;

/// <summary>
/// 进入游戏登录流程的情况：新进入游戏、切换账号、防沉迷、重新登录（断网、切后台、后端主动断）,退出登录
/// </summary>
public class LoginManager : Singleton<LoginManager>, IResetData
{
    public const int TokenTimeout = 240;
    public float LoginTime = 0;             //SDK首次登陆时间
    public ConnectResultType connectResult = ConnectResultType.None;// 连接结果

    public bool IsLoginBeforLogDone;

    private float _refreshListTime;

    int _connectCount = 0;
    private void ConnectToServer()
    {
        string ip="";
        int port=0;
        string bgpip = "";
        int bgpport = 0;
        bool isBgp = false;
        NetAppLayer.Instance.ConnectTcpServer(ip, port,bgpip,bgpport, SocketName.MainSocket, ConnectCallback,isBgp);
    }

    /// <summary>
    /// socket链接回调函数
    /// </summary>
    /// <param name="result">true 链接成功，false 链接失败</param>
    private void ConnectCallback(bool result)
    {
        Log.Debug("LoginModel ConnectCallback result:" + result);

        if (result)
        {
            connectResult = ConnectResultType.SocketConnectSuccess;
            NetReconnectManager.Instance.HaseConnected = true;
            NetReconnectManager.Instance.RemoveConnectiong();
            //LoginModel.Instance.TosClientLogin(Global.Instance.userplatformname, Global.Instance.localToken);
        }
        else
        {
            if (NetReconnectManager.Instance.CanReconnect == false)
            {
                _connectCount = 0;
                return;
            }

            if (_connectCount >= 2)
            {
                //RecordMsg.Error(RecordType.ConnectBgpFail, _connectCount.ToString());
            }

            _connectCount++;
            //按场景设置重连次数
            float delay = 2.0f;
            int MaxCount = NetReconnectManager.CONNECT_SOCKET_COUNT;

    /*        if (ChangeSceneManager.Instance.currentSceneName  == SceneName.FightScene)
            {
                MaxCount = NetReconnectManager.LOGIN_CONNECT_COUNT;
                delay = 1.0f;
            }
            else if (ChangeSceneManager.Instance.currentSceneName == SceneName.FightScene)
            {
                MaxCount = NetReconnectManager.FIGHT_CONNECT_COUNT;
            }*/

            if (_connectCount < MaxCount)
            {
                //TimerManager.Instance.SetTimeOut(ToReconnect, delay);
            }
            else
            {
                Log.Debug("LoginModel ConnectCallback " + _connectCount + "次连接失败后返回");
                _connectCount = 0;
                connectResult = ConnectResultType.SocketConnectFail;
            }
        }
    }

    /// <summary>
    /// 网络重连
    /// </summary>
    private void ToReconnect()
    {
        if (NetReconnectManager.Instance.CanReconnect == false)
            return;

        //如果连接上了就不重连
        if (connectResult == ConnectResultType.SocketConnectSuccess)
            return;

        Log.Debug("LoginModel ConnectCallback 第" + _connectCount + "次重连开始");

        ConnectToServer();
    }

    /// <summary>
    /// 断线15秒提示，点击重新连接
    /// </summary>
    public void StartReconnect()
    {
        _connectCount = 1;
        ToReconnect();
    }

    public bool IsReconnect()
    {
        return NetAppLayer.Instance.IsReconnect();
    }
    

    /// <summary>
    /// 发送LoginTOS登录失败处理
    /// </summary>
    public void OnLoginError()
    {

    }

    //重新登录： bol 是否清理数据
    public void LoginReset(bool bol = true,bool isRectReconnnect=true)
    {
        //关闭网络
        NetAppLayer.Instance.CloseServer(isRectReconnnect);
        NetAppLayer.Instance.CloseServer(isRectReconnnect, SocketName.CrossSocket);
        if (isRectReconnnect)
        {
            NetAppLayer.Instance.CloseServer(isRectReconnnect, SocketName.UdpSocket);
        }
        if (bol)
            DataReset();
    }

    /// <summary>
    /// 本地缓存数据清理
    /// </summary>
    private void DataReset()
    {
        //Global.Instance.ResetData();                //全局变量清理
        //MainControl.Instance.SingletonReset();
    }

    public void ResetData()
    {
        
    }
}
