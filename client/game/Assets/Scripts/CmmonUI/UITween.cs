using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class UITween : MonoBehaviour
{
    public class KeyFrameInfo
    {
        public float startTime;
        public float lastTime;
        public object targetValue;
    }

    public Action OnFinish; 
    
    public float StartTime;
    public float LastTime;
    protected object _startValue;
    public object StartValue;
    public object EndValue;
    private float _time = -1;
    private List<KeyFrameInfo> _keyFrameList;
    private int _idx;

    public UITween Init(object startValue, object endValue, float startTime, float lastTime)
    {
        OnFinish = null;
        _idx = -1;
        _keyFrameList = null;
        StartTime = startTime;
        LastTime = lastTime;
        _startValue = startValue;
        StartValue = startValue;
        EndValue = endValue;
        _time = 0;
        Reset();
        return this;
    }

    public UITween Init(object startValue, List<KeyFrameInfo> keyFrameList)
    {
        OnFinish = null;
        _idx = 0;
        _keyFrameList = keyFrameList;
        StartTime = _keyFrameList[_idx].startTime;
        LastTime = _keyFrameList[_idx].lastTime;
        _startValue = startValue;
        StartValue = startValue;
        EndValue = _keyFrameList[_idx].targetValue;
        _time = 0;
        Reset();
        return this;
    }

    public void AddOnFinish(Action onFinish)
    {
        OnFinish = onFinish;
    }
    
    public abstract void WorkFunction(float process);

    public abstract void Reset();

    public void Pause()
    {
        _time = -1;
    }

    public void Resume()
    {
        _time = 0;
    }

    void OnDisable()
    {
        _time = -1;
    }

    void Update()
    {
        if (_time >= 0)
        {
            _time += Time.deltaTime;
            if (_time > StartTime)
            {
                float t;
                if (LastTime <= 0)
                    t = 1;
                else
                    t = (_time - StartTime) / LastTime;

                WorkFunction(t);

                if (t >= 1)
                {
                    if (_idx >= 0)
                    {
                        if (_idx < _keyFrameList.Count - 1)
                        {
                            _idx++;
                            StartTime = _keyFrameList[_idx].startTime;
                            LastTime = _keyFrameList[_idx].lastTime;
                            StartValue = EndValue;
                            EndValue = _keyFrameList[_idx].targetValue;
                        }
                        else
                        {
                            _idx = -1;
                            _time = -1;
                            if (OnFinish != null)
                                OnFinish.Invoke();
                        }
                    }
                    else
                    {
                        _time = -1;
                        if (OnFinish != null)
                            OnFinish.Invoke();
                    }
                }
            }
        }
    }
}
