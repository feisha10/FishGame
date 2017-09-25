using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITweenPosition : UITween
{
    public override void WorkFunction(float process)
    {
        Vector2 startValue = (Vector2)StartValue;
        Vector2 endValue = (Vector2)EndValue;
        float x = Mathf.SmoothStep(startValue.x, endValue.x, process);
        float y = Mathf.SmoothStep(startValue.y, endValue.y, process);
        CachedRectTransform.anchoredPosition = new Vector2(x, y);
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
