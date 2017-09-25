using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 注意： Timers 会在切换Scene时进行清理，如果实在需要执行，请不要使用TimerManager，应该使用全局Update，比如MainControl.Update
/// 使用Time.realtimeSinceStartup 必需要在应用程序启动第一个Start后使用才正确
/// </summary>
public class TimerManager : SingletonMonoBehaviour<TimerManager>
{
    private List<UTimer> Timers = new List<UTimer>();

    public TimerManager()
    {
    }

    /// <summary>
    /// 延迟执行函数
    /// ignoreTimeScale 是否忽略时间缩放
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delay"></param>
    /// <param name="ignoreTimeScale"></param>
    /// <param name="canRepeat">函数是否允许重复</param>
    /// <param name="args"></param>
    public void SetTimeOut(Action<object[]> action, float delay,bool ignoreTimeScale = true, bool canRepeat = false, params object[] args)
    {
        Add(delay, action, 1, ignoreTimeScale, canRepeat, args);
    }

    public void SetTimeOut(Action action, float delay, bool ignoreTimeScale = true, bool canRepeat = false,bool isClearWhenChangeMap=true)
    {
        Add(delay, action, 1, ignoreTimeScale, canRepeat,isClearWhenChangeMap);
    }

    /// <summary>
    /// 间隔执行函数，loop=0时代表循环执行，loop>0时代表执行的次数
    /// ignoreTimeScale 是否忽略时间缩放
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delay"></param>
    /// <param name="loop"></param>
    /// <param name="ignoreTimeScale"></param>
    /// <param name="args"></param>
    public void SetInterval(Action<object[]> action, float delay,int loop=0, bool ignoreTimeScale = true, params object[] args)
    {
        Add(delay, action, loop, ignoreTimeScale, false, args);
    }

    public void SetInterval(Action action, float delay, int loop = 0, bool ignoreTimeScale = true, bool isClearWhenChangeMap=true)
    {
        Add(delay, action, loop, ignoreTimeScale, false, isClearWhenChangeMap);
    }

    /// <summary>
    /// 删除某个执行函数
    /// 因为 canRepeat ，去掉 break
    /// </summary>
    /// <param name="action"></param>
    public void RemoveAction(Action<object[]> action)
    {
        if (Timers.Count <= 0)
            return;
        for (int i = Timers.Count - 1; i >= 0; i--)
        {
            if (Timers[i].action == action)
            {
                Timers.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 删除某个执行函数
    /// 因为 canRepeat ，去掉 break 
    /// </summary>
    /// <param name="action"></param>
    public void RemoveAction(Action action)
    {
        if (Timers.Count <= 0)
            return;
        for (int i = Timers.Count - 1; i >= 0; i--)
        {
            if (Timers[i].simpleAction == action)
            {
                Timers.RemoveAt(i);
            }
        }
    }


    /// <summary>
    /// 是否存在某个Action如果存在就删除之
    /// </summary>
    /// <param name="action"></param>
    public bool IsHaveActionAddRemove(Action action)
    {
        if (Timers.Count <= 0)
            return false;
        for (int i = Timers.Count - 1; i >= 0; i--)
        {
            if (Timers[i].simpleAction == action)
            {
                Timers.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public bool IsHaveAction(Action action)
    {
        if (Timers.Count <= 0)
            return false;
        for (int i = Timers.Count - 1; i >= 0; i--)
        {
            if (Timers[i].simpleAction == action)
            {
                return true;
            }
        }
        return false;
    }

    public void ClearWhenChangeMap()
    {
        for (var i = Timers.Count - 1; i >= 0;i--)
        {
            if(Timers[i].isClearWhenChangeMap)
            {
                Timers.RemoveAt(i);
            }
        }
        _timerMap.Clear();
    }
    /// <summary>
    /// 清理所有Action
    /// </summary>
    public void ClearAllAction()
    {
        Timers.Clear();
        _timerMap.Clear();
        //_timerKeyList.Clear();
    }

    void Add(float delay, Action<object[]> action, int loop,bool ignoreTimeScale, bool canRepeat, params object[] args)
    {
        if (delay > 0 && action != null)
        {
            AddTimerVo(delay, action, loop, ignoreTimeScale, canRepeat, args);
        }
        else if (Mathf.Approximately(delay, 0f) && action != null)
        {
            action(args);
        }
        else
        {
            throw new Exception("注册时间管理函数参数有误！");
        }
    }

    void Add(float delay, Action action, int loop, bool ignoreTimeScale, bool canRepeat,bool isClearWhenChangeMap)
    {
        if (delay > 0 && action != null)
        {
            AddTimerVo(delay, action, loop, ignoreTimeScale, canRepeat,isClearWhenChangeMap);
        }
        else if (Mathf.Approximately(delay, 0f) && action != null)
        {
            action();
        }
        else
        {
            throw new Exception("注册时间管理函数参数有误！");
        }
    }

    void AddTimerVo(float delay, Action<object[]> action, int loop, bool ignoreTimeScale, bool canRepeat, params object[] args)
    {
        for (int i = 0; i < Timers.Count; i++)
        {
            if (Timers[i].action == action && !canRepeat)
            {
                Timers[i] = CreateTimerVo(delay, action, loop, ignoreTimeScale, args);
                return;
            }
        }

        Timers.Add(CreateTimerVo(delay, action, loop, ignoreTimeScale, args));
    }

    void AddTimerVo(float delay, Action action, int loop, bool ignoreTimeScale, bool canRepeat, bool isClearWhenChangeMap)
    {
        for (int i = 0; i < Timers.Count; i++)
        {
            if (Timers[i].simpleAction == action && !canRepeat)
            {
                Timers[i] = CreateTimerVo(delay, action, loop, ignoreTimeScale, isClearWhenChangeMap);
                return;
            }
        }

        Timers.Add(CreateTimerVo(delay, action, loop, ignoreTimeScale, isClearWhenChangeMap));
    }

    UTimer CreateTimerVo(float delay, Action<object[]> action, int loop, bool ignoreTimeScale, params object[] args)
    {
        UTimer actionVo = new UTimer();
        actionVo.action = action;
        actionVo.args = args;
        actionVo.ignoreTimeScale = ignoreTimeScale;
        actionVo.delay = delay;
        actionVo.loop = loop;
        if (ignoreTimeScale)
        {
            actionVo.time = Time.realtimeSinceStartup;
        }
        else
        {
            actionVo.time = Time.time;
        }
        return actionVo;
    }

    UTimer CreateTimerVo(float delay, Action action, int loop, bool ignoreTimeScale, bool isClearWhenChangeMap)
    {
        UTimer actionVo = new UTimer();
        actionVo.simpleAction = action;
        actionVo.args = null;
        actionVo.ignoreTimeScale = ignoreTimeScale;
        actionVo.delay = delay;
        actionVo.loop = loop;
        actionVo.isClearWhenChangeMap = isClearWhenChangeMap;
        if (ignoreTimeScale)
        {
            actionVo.time = Time.realtimeSinceStartup;
        }
        else
        {
            actionVo.time = Time.time;
        }
        return actionVo;
    }

    void Update()
    {
        #region 0's
        float t = Time.time;
        float rt = Time.realtimeSinceStartup;
        if(Timers.Count>0)
        {
            float compareTime = 0;
            bool ok = false;
            for (int i = Timers.Count - 1; i >= 0; i--)
            {
                if (i >= Timers.Count)
                    continue;
                UTimer timer = Timers[i];
                compareTime = timer.ignoreTimeScale ? rt : t;
                ok = (compareTime - timer.time) >= timer.delay;
                if (ok && (timer.loop == 0 || timer.count < timer.loop))
                {
                    timer.count++;
                    timer.time += timer.delay;

                    if (timer.loop != 0 && timer.loop <= timer.count)
                    {
                        Timers.RemoveAt(i);
                    }

                    try
                    {
                        if (timer.args == null && timer.simpleAction != null)
                        {
                            timer.simpleAction();
                        }else
                        {
                            timer.action(timer.args);
                        }
                    }catch (Exception ex)
                    {
                        Log.Exception(ex);
                    }
                }
            }
        }
        
        #endregion

        #region 1's
        if (0 == _timerMap.Count)
            return;

        if(0 != _cachedKeyList.Count)
            _cachedKeyList.Clear();

        var iter = _timerMap.GetEnumerator();
        while (iter.MoveNext())
            _cachedKeyList.Add(iter.Current.Key);
        //for (int i = 0; i < _timerKeyList.Count; ++i)   // 缓存键名处理，为了防止在回调中做了定时器的增减操作
        //    _cachedKeyList.Add(_timerKeyList[i]);

        for (int i = 0; i < _cachedKeyList.Count; ++i)
        {
            string keyItem = _cachedKeyList[i];

            if (!_timerMap.ContainsKey(keyItem))
                continue;

            TimerInfo timerInfo = _timerMap[keyItem];
            timerInfo._time -= Time.deltaTime;
            if (timerInfo._time > 0)
                continue;

            _timerMap.Remove(keyItem);  // 必须前置，后置会导致嵌套调用tierm重复删除相关键名

            if (null != timerInfo._callback)
                timerInfo._callback(timerInfo._param);

        }
        #endregion
    }

    #region 1
    List<string> _cachedKeyList = new List<string>();
    class TimerInfo
    {
        public float _time;
        public Action<object> _callback;
        public object _param;
    }

    public bool HasTimer(string key)
    {
        return _timerMap.ContainsKey(key);
        //for (int i = 0; i < _timerKeyList.Count; i++)
        //{
        //    if (_timerKeyList[i] == key)
        //        return true;
        //}
        //return false;
    }

    public bool RemoveTimer(string key)
    {
        //_timerKeyList.Remove(key);
        return _timerMap.Remove(key);
    }

    public bool SetTimer(string key, float time)
    {
        return SetTimer(key, time, null);
    }

    public bool SetTimer(string key, float time, Action<object> callback)
    {
        return SetTimer(key, time, callback, null);
    }

    Dictionary<string, TimerInfo> _timerMap = new Dictionary<string, TimerInfo>();
    //List<string> _timerKeyList = new List<string>();    // 键名列表，跟_timerMap同步增减，为的是在做定时回调缓存处理的时候减少gc alloc
    public bool SetTimer(string key, float time, Action<object> callback, object param)
    {
        if (_timerMap.ContainsKey(key))
            return false;

        TimerInfo timerInfo = new TimerInfo();
        timerInfo._time = time;
        timerInfo._callback = callback;
        timerInfo._param = param;
        _timerMap.Add(key, timerInfo);
        //_timerKeyList.Add(key);

        return true;
    }

    public void ClearDramaTimer()
    {
        List<string> dramaKeyList = new List<string>();
        foreach (var key in _timerMap.Keys)
        {
//            if (key.Contains(DramaControl.DramaTimerKey) && !dramaKeyList.Contains(key))
//                dramaKeyList.Add(key);
        }
        for(var i=0;i<dramaKeyList.Count;i++)
        {
            RemoveTimer(dramaKeyList[i]);
        }
    }
    #endregion
}
public class UTimer
{
    public float delay;
    public bool ignoreTimeScale;
    public int loop;
    public int count;
    public float time;
    public Action<object[]> action;
    public Action simpleAction;
    public object[] args;
    public bool isClearWhenChangeMap=true;//跳地图时是否清理
}
