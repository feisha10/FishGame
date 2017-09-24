using UnityEngine;
using System;

/**
 * 该单例不绑定某个对象，做全局脚本用，切换场景时不会自动销毁
 * */
public class SingletonSpawningMonoBehaviour<T> : MonoBehaviour where T : SingletonSpawningMonoBehaviour<T>
{
    private static T uniqueInstance;
    protected static bool applicationQuitting;

    public static T Instance
    {
        get
        {
            if (!Exists)
            {
                if (applicationQuitting || !Application.isPlaying)
                {
                    return null;
                }
                Type[] components = new Type[] { typeof(T) };
                GameObject target = new GameObject("Singleton " + typeof(T).ToString(), components);
                uniqueInstance = target.GetComponent<T>();
                UnityEngine.Object.DontDestroyOnLoad(target);
                Exists = true;
            }
            return uniqueInstance;
        }
    }

    public static bool Exists
    {
        get;
        private set;
    }

    protected virtual void Awake()
    {
        if (uniqueInstance == null)
        {
            uniqueInstance = (T)this;
        }
        else if (uniqueInstance != this)
        {
            throw new InvalidOperationException("Cannot have two instances of a SingletonMonoBehaviour : " + typeof(T).ToString() + ".");
        }
    }
    protected virtual void OnApplicationQuit()
    {
        applicationQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (uniqueInstance == this)
        {
            Exists = false;
            uniqueInstance = null;
        }
    }

}
