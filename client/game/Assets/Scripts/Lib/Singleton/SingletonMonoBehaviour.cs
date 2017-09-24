using UnityEngine;
using System;

/**
 * 该单例需要绑定某个对象，不确定该脚本是否存在的情况下，可用Exists 判断
 * */
public  class  SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T uniqueInstance;

    public static T Instance
    {
        get
        {
            return uniqueInstance;
        }
    }

    protected virtual void Awake()
    {
        if (uniqueInstance == null)
        {
            uniqueInstance = (T)this;
            Exists = true;
        }
        else if (uniqueInstance != this)
        {
            throw new InvalidOperationException("Cannot have two instances of a SingletonMonoBehaviour : " + typeof(T).ToString() + ".");
        }
        string tempName = CheckViewName();
        Log.Debug("CreateView :" + tempName);
        EventManager.Send<string, GameObject>(EventIdx.OnSingletenPanelCreate, tempName, gameObject);
    }

    string CheckViewName()
    {
        if (this.name.Contains("(Clone)"))
        {
            return this.name.Replace("(Clone)", "");
        }
        else
        {
            return this.name;
        }
    }

    public void DestroyImmediate()
    {
        if (uniqueInstance == this)
        {
            Exists = false;
            uniqueInstance = null;
        }

        Destroy(this.gameObject);
    }

    protected S GetComponent<S>(string path) where S : Component
    {
        var t = CachedTransform.Find(path);

        if (null == t)
            return default(S);

        var component = t.GetComponent<S>();

        return component;
    }

    protected virtual void OnDestroy()
    {
        if (uniqueInstance == this)
        {
            Exists = false;
            uniqueInstance = null;
        }
        string tempName = CheckViewName();
        Log.Debug("DestroyView:" + tempName);
        EventManager.Send<string>(EventIdx.OnSingletenPanelDestory, tempName);
    }

    protected S AddComponent<S>() where S : Component
    {
        S component = GetComponent<S>();
        if (component == null)
			component = gameObject.AddComponent<S>();
		return component;
    }

    public static bool Exists
    {
        get;
        private set;
    }

    /// <summary>
    /// 是否激活状态
    /// </summary>
    public static bool Active
    {
        get { return Exists && uniqueInstance.gameObject && uniqueInstance.gameObject.activeInHierarchy; }
    }

    Transform _t;
    public Transform CachedTransform
    {
        get
        {
            if (null == _t)
                _t = transform;

            return _t;
        }
    }
}
