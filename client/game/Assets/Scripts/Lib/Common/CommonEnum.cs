public enum SceneName
{
    MainScene,
    Fight,
}

public enum Direction
{
    none,//设置不可拖动
    Vertical,//竖直
    Horizontal,//水平
}

public enum SocketName
{
    None,
    MainSocket,//主socket连接
    CrossSocket,//跨服连接
    UdpSocket,//Udp连接
}

public enum ConnectResultType
{
    None = 0,
    SocketConnectSuccess,//socket连接成功
    SocketConnecting,// socket 连接中
    SocketConnectFail,// 连接socket失败

}
