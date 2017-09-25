using UnityEngine;
using System.Collections;

public class UITweenPositionAccLine : UITween
{
    private int _type = 0;  //0：初速度为0，1：末速度为0

    public void Init(Vector2 startValue, Vector2 endValue, float startTime, float lastTime, int type)
    {
        base.Init(startValue, endValue, startTime, lastTime);
        _type = type;
    }

    public override void WorkFunction(float process)
    {
        if (_type == 0)
            CachedRectTransform.anchoredPosition = Vector2.Lerp((Vector2)StartValue, (Vector2)EndValue, process * process);
        else if (_type == 1)
            CachedRectTransform.anchoredPosition = Vector2.Lerp((Vector2)StartValue, (Vector2)EndValue, (2 - process) * process);
    }

    public override void Reset()
    {
        CachedRectTransform.anchoredPosition = (Vector2)_startValue;
    }

    private RectTransform _cachedRectTransform;
    public RectTransform CachedRectTransform
    {
        get
        {
            if (_cachedRectTransform == null)
                _cachedRectTransform = transform as RectTransform;
            return _cachedRectTransform;
        }
    }
}
