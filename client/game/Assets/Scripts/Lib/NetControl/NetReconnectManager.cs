using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 网络重连管理类   //弹窗 的优先级 重登 > 重登、重连 > 连接中
/// </summary>
public class NetReconnectManager : Singleton<NetReconnectManager>
{
    public static readonly int CONNECT_SOCKET_COUNT = 20;
    public static readonly int LOGIN_CONNECT_COUNT = 10;
    public static readonly int FIGHT_CONNECT_COUNT = 1000;

    public bool IsReconnectiong = false;        //是否重连中
    public bool CanReconnect = true;            //是否可继续重连,已断开就不需要再重连

    public bool HaseConnected = false;          //是否有连上过服务器
    private GameObject _disConnectAlertGo = null;               //链接失败提示框
    private GameObject _connectingUI = null;                 //连接中提示框

    private int type = 0; // 1 连接中， 2 重登，重连， 3，重登


    /// <summary>
    /// 显示服务器重连中提示
    /// </summary>
	public void ShowReconnectiong()
	{
        if (type == 2 || type==3)
            return;

        IsReconnectiong = true;
        if (false == CanShowAlert())
        {
            return;
        }
        if (_connectingUI != null)
        {
            _connectingUI.GetComponent<ConnectingUI>().SetConnectingType(1);
            return;
        }
        type = 1;
        GameObject prefab = ResourceManager.Instance.LoadExistsAsset<GameObject>("Prefab/UI/Common/ConnectingUI");
        prefab.AddMissingComponent<ConnectingUI>();
        _connectingUI = UGUITools.AddChild(UIManager.TipLayer.gameObject,prefab);
        _connectingUI.GetComponent<ConnectingUI>().SetConnectingType(1);
	}

    /// <summary>
    /// 移除链接中提示框
    /// </summary>
    public void RemoveConnectiong()
    {
        type = 0;
        if (_connectingUI != null)
        {
            GameObject.Destroy(_connectingUI);
            _connectingUI = null;
        }
    }

    /// <summary>
    /// 显示消息发送中
    /// </summary>
    public void ShowSendingUI()
    {
        if (type == 2 || type == 3)
            return;

        if (false == CanShowAlert())
        {
            return;
        }

        if (_connectingUI != null)
        {
            _connectingUI.GetComponent<ConnectingUI>().SetConnectingType(2);
            return;
        }
        type = 1;
        GameObject prefab = ResourceManager.Instance.LoadExistsAsset<GameObject>("Prefab/UI/Common/ConnectingUI");
        prefab.AddMissingComponent<ConnectingUI>();
        _connectingUI = UGUITools.AddChild(UIManager.TipLayer.gameObject, prefab);
        _connectingUI.GetComponent<ConnectingUI>().SetConnectingType(2);
    }

    /// <summary>
    /// 移除发送中UI
    /// </summary>
    public void RemoveSendingUI()
    {
        type = 0;
        if (!IsReconnectiong)
            RemoveConnectiong();
        NetAppLayer.Instance.ResetNetCheck();
    }

    /// <summary>
    /// 显示网络重连接失败提示
    /// </summary>
    public void ShowReconnectFail(int triggerType=0)
    {
        if(type ==3)
            return;

        CanReconnect = false;

        RemoveConnectiong();
        NetAppLayer.Instance.ResetNetCheck();

        if (false == CanShowAlert())
        {
            return;
        }

        RemoveDicConnectAlert();

        type = 2;
        if (false == HaseConnected)
        {
           // _disConnectAlertGo = Alert.ShowSingleBtn("无法与服务器建立连接，请检查网络！", UIManager.TipLayer.gameObject, TurnToLoginScene, "重登", false);
        }
        else
        {
           /* if(ChangeSceneManager.Instance.currentSceneName==SceneName.FightScene)
            {
                ReConnect();
            }
            else
            {
                _disConnectAlertGo = Alert.Show("与服务器断开连接！", UIManager.TipLayer.gameObject, ReConnect, TurnToLoginSceneAndLoginPlatform, "重连", "重登");
            }  */
        }
    }
    private void OpenLoginHelp()
    {

    }
    void ShowReLoginAlert(string msg)
    {
        Log.Debug("ShowReLoginAlert");
        CanReconnect = false;

        LoginManager.Instance.LoginReset();

        RemoveConnectiong();
        NetAppLayer.Instance.ResetNetCheck();

        RemoveDicConnectAlert();

        type = 3;

       // _disConnectAlertGo = Alert.ShowSingleBtn(msg, UIManager.TipLayer.gameObject, TurnToLoginSceneAndLoginPlatform, "重登", false);
    }
    /// <summary>
    /// UDP链接异常，需重新登录
    /// </summary>
    public void ShowUDPError()
    {
        Log.Error("网络断开连接，请重新登录..");
        ShowReLoginAlert("网络断开连接，请重新登录..");
    }

    /// <summary>
    /// 数据异常异常，需重新登录
    /// </summary>
    public void ShowDataError()
    {
        Log.Error("ShowDataError");
        ShowReLoginAlert("登录数据异常重新登录");
    }

    /// <summary>
    /// 登录异常重新登录(当在选择角色界面或是创建角色界面太久，而没有进入游戏的话，后端会主动断开连接)
    /// </summary>
    public void ShowLoginError()
    {
        Log.Error("ShowLoginError");
        ShowReLoginAlert("登录异常重新登录");
    }

    /// <summary>
    /// 角色被服务器踢下线
    /// </summary>
    public void OnClientClose()
    {
        ShowReLoginAlert("您的账号已在其他设备登陆");
    }

    /// <summary>
    /// 后台踢人
    /// </summary>
    public void GMKickOff()
    {
        ShowReLoginAlert("您已被强制离线");
    }

    /// <summary>
    /// 已链接上服务器
    /// </summary>
    public void OnConnected()
    {
        RemoveConnectiong();
        ResetStatus();
        HaseConnected = true;
        IsReconnectiong = false;
    }

    /// <summary>
    /// 连接状态重置
    /// </summary>
    public void ResetStatus()
    {
        type = 0;
        IsReconnectiong = false;
        HaseConnected = false;
        
        LoginManager.Instance.connectResult = ConnectResultType.None;
        CanReconnect = true;
        RemoveDicConnectAlert();
        RemoveConnectiong();
    }

    void RemoveDicConnectAlert()
    {
        type = 0;
        if (null != _disConnectAlertGo)
        {
            GameObject.Destroy(_disConnectAlertGo);
            _disConnectAlertGo = null;
        }
    }

    /// <summary>
    /// 尝试重连
    /// </summary>
    private void ReConnect()
    {
        CanReconnect = true;
        LoginManager.Instance.connectResult = ConnectResultType.None;
        ShowReconnectiong();
        RemoveDicConnectAlert();
      /*  if (ChangeSceneManager.Instance.currentSceneName==SceneName.FightScene)
            NetAppLayer.Instance.ReConnectSocket();
        else
            NetAppLayer.Instance.ReConnectSocket(SocketName.MainSocket);*/
    }

    /// <summary>
    /// 是否可显示网络提示
    /// </summary>
    /// <returns></returns>
    private bool CanShowAlert()
    {
        bool bol = true;

        return bol;
    }

    /// <summary>
    /// 战斗结束，显示网络掉线提示
    /// </summary>
    public void OnFightOverAlert()
    {
        if (CanReconnect==false)
        {
            ShowReconnectFail();
        }
        else if (IsReconnectiong)
        {
            ShowReconnectiong();
        }
    }

    //防沉迷返回登陆
    public void FatigueTurnToLogin()
    {
        TurnToLoginScene();
    }

    private void TurnToLoginScene()
    {
        ResetStatus();
        LoginManager.Instance.LoginReset();
       // FightManager.Instance.StopGame();
        //ChangeSceneManager.Instance.ChangeScene(SceneName.LoginScene);
    }

    private void TurnToLoginSceneAndLoginPlatform()
    {
        TurnToLoginScene();

        //PlatformPlugin.Instance.platformLogin();
    }
	
}
