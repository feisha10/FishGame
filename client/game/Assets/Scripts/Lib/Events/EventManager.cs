using System;
using System.Collections.Generic;

public class EventManager {

    static Dictionary<int, List<Delegate>> _eventMap = new Dictionary<int, List<Delegate>>();

    public static void Regist(int eventIdx, EventCallback callback)
    {
        RegistLocal(eventIdx, callback);
    }
    public static void Regist<T>(int eventIdx, EventCallback<T> callback)
    {
        RegistLocal(eventIdx, callback);
    }
    public static void Regist<T, T1>(int eventIdx, EventCallback<T, T1> callback)
    {
        RegistLocal(eventIdx, callback);
    }
    public static void Regist<T, T1, T2>(int eventIdx, EventCallback<T, T1, T2> callback)
    {
        RegistLocal(eventIdx, callback);
    }
    public static void Regist<T, T1, T2, T3>(int eventIdx, EventCallback<T, T1, T2, T3> callback)
    {
        RegistLocal(eventIdx, callback);
    }

    private static void RegistLocal(int eventIdx, Delegate callback)
    {
        int idx = eventIdx;

        List<Delegate> callbackList = null;
        if (_eventMap.ContainsKey(idx))
        {
            callbackList = _eventMap[idx];
        }
        else
        {
            callbackList = new List<Delegate>();
            _eventMap.Add(idx, callbackList);
        }

        if (callbackList.Contains(callback))
        {
            Log.Warn(string.Format("已存在该回调!eventIdx: {0},callback: {1}", eventIdx, callback));
            return;
        }

        callbackList.Add(callback);
    }

    public static void UnRegist(int eventIdx, EventCallback callback)
    {
        UnRegistLocal(eventIdx, callback);
    }
    public static void UnRegist<T>(int eventIdx, EventCallback<T> callback)
    {
        UnRegistLocal(eventIdx, callback);
    }
    public static void UnRegist<T, T1>(int eventIdx, EventCallback<T, T1> callback)
    {
        UnRegistLocal(eventIdx, callback);
    }
    public static void UnRegist<T, T1, T2>(int eventIdx, EventCallback<T, T1, T2> callback)
    {
        UnRegistLocal(eventIdx, callback);
    }

    private static bool UnRegistLocal(int eventIdx, Delegate callback)
    {
        int idx = eventIdx;
        if (!_eventMap.ContainsKey(idx))
            return false;

        var callbackList = _eventMap[idx];
        bool removeSucceed = callbackList.Remove(callback);

        return removeSucceed;
    }

    public static void Send(int eventIdx)
    {
        int idx = eventIdx;
        if (_eventMap.ContainsKey(idx))
        {
            var callbackList = _eventMap[idx];
            for (int i = 0; i < callbackList.Count; ++i)
            {
                try
                {
                    EventCallback listener = callbackList[i] as EventCallback;
                    if (listener != null)
                        listener();
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }
        }
    }

    public static void Send<T>(int eventIdx, T param)
    {
        int idx = eventIdx;
        if (_eventMap.ContainsKey(idx))
        {
            var callbackList = _eventMap[idx];
            for (int i = 0; i < callbackList.Count; ++i)
            {
                try
                {
                    EventCallback<T> listener = callbackList[i] as EventCallback<T>;
                    if (listener != null)
                        listener(param);
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }
        }
    }

    public static void Send<T, T1>(int eventIdx, T param, T1 param1)
    {
        int idx = eventIdx;
        if (_eventMap.ContainsKey(idx))
        {
            var callbackList = _eventMap[idx];
            for (int i = 0; i < callbackList.Count; ++i)
            {
                try
                {
                    EventCallback<T, T1> listener = callbackList[i] as EventCallback<T, T1>;
                    if (listener != null)
                        listener(param, param1);
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }
        }
    }

    public static void Send<T, T1, T2>(int eventIdx, T param, T1 param1, T2 param2)
    {
        int idx = eventIdx;
        if (_eventMap.ContainsKey(idx))
        {
            var callbackList = _eventMap[idx];
            for (int i = 0; i < callbackList.Count; ++i)
            {
                try
                {
                    EventCallback<T, T1, T2> listener = callbackList[i] as EventCallback<T, T1, T2>;
                    if (listener != null)
                        listener(param, param1, param2);
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }
        }
    }

    public static void Send<T, T1, T2, T3>(int eventIdx, T param, T1 param1, T2 param2, T3 param3)
    {
        int idx = eventIdx;
        if (_eventMap.ContainsKey(idx))
        {
            var callbackList = _eventMap[idx];
            for (int i = 0; i < callbackList.Count; ++i)
            {
                try
                {
                    EventCallback<T, T1, T2, T3> listener = callbackList[i] as EventCallback<T, T1, T2, T3>;
                    if (listener != null)
                        listener(param, param1, param2, param3);
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }
        }
    }
}

public delegate void EventCallback();
public delegate void EventCallback<T>(T arg1);
public delegate void EventCallback<T1, T2>(T1 arg1, T2 arg2);
public delegate void EventCallback<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
public delegate void EventCallback<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
