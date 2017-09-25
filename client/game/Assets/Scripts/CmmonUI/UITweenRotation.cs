using UnityEngine;
using System.Collections;

public class UITweenRotation : UITween
{
    public override void WorkFunction(float process)
    {
        CachedRectTransform.localEulerAngles = Vector3.Lerp((Vector3)StartValue, (Vector3)EndValue, process);
    }

    public override void Reset()
    {
        CachedRectTransform.localEulerAngles = (Vector3)_startValue;
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
