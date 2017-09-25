using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    private int _type = 2;

    /// <summary>
    /// 设置连接类型,如果是网络中断，就不再给网络延迟覆盖
    /// type:1 网络连接中断 2 网络延迟过高
    /// </summary>
    public void SetConnectingType(int type=2)
    {
        if (_type == 1)
            return;

        _type = type;

        TimerManager.Instance.RemoveAction(OnTimeOut);

        switch (_type)
        {
            case 1:
                TimerManager.Instance.SetTimeOut(OnTimeOut, 15);
                break;
            case 2:
                TimerManager.Instance.SetTimeOut(OnTimeOut, 10);
                break;
        }
    }

    private void OnTimeOut()
    {
        switch (_type)
        {
            case 1:
                OnNetFail();
                break;
            case 2:
                OnDelayOut();
                break;
        }
    }

    //直接关闭网络，弹出提示
    private void OnNetFail()
    {
        //关闭网络
        LoginManager.Instance.LoginReset(false,false);

        NetReconnectManager.Instance.ShowReconnectFail();
    }

    // 直接关闭网络，进行重连
    private void OnDelayOut()
    {
        //关闭网络
        LoginManager.Instance.LoginReset(false,false);

        NetReconnectManager.Instance.RemoveSendingUI();
        NetReconnectManager.Instance.ShowReconnectFail();
    }

    void OnDestroy()
    {
        TimerManager.Instance.RemoveAction(OnTimeOut);
    }

}
