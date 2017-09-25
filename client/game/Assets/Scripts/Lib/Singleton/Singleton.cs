using System.Collections.Generic;
using System.Diagnostics;

public class Singleton<T> where T: new()
{
    /// <summary>
    /// 保存单例列表，用于重置单例类的数据
    /// </summary>
    public static List<IResetData> SingtonList = new List<IResetData>();

    private static readonly object _lock = new object();
    private static T instance;

    protected Singleton()
    {
        Debug.Assert(instance == null);
    }

	public static bool Exists
	{
		get
		{
			return instance != null;
		}
	}
    
    public static T Instance
    {
        get {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                    if (instance is IResetData)
                    {
                        SingtonList.Add(instance as IResetData);
                    }
                }
            }
            return instance;
        }
    }
}
