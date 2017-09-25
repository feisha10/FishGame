using System.Collections.Generic;
partial class NetAppLayer
{
	private Dictionary<int,int> _notCheckIdDic; //不进行网络延迟检查的接口
	private void InitNoCheckId() //初始化不进行网络检测的vo
	{
		_notCheckIdDic = new Dictionary<int, int>(8);
		_notCheckIdDic[1010] = 1010;
		_notCheckIdDic[1105] = 1105;
		_notCheckIdDic[1414] = 1414;
		_notCheckIdDic[1419] = 1419;
		_notCheckIdDic[1606] = 1606;
		_notCheckIdDic[1923] = 1923;
	}
}
