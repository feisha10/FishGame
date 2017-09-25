using UnityEngine;
using System.Collections;

public class UITweenLocalPosition : UITween
{
    public override void WorkFunction(float process)
    {
        Vector3 startValue = (Vector3)StartValue;
        Vector3 endValue = (Vector3)EndValue;
        float x = Mathf.SmoothStep(startValue.x, endValue.x, process);
        float y = Mathf.SmoothStep(startValue.y, endValue.y, process);
        float z = Mathf.SmoothStep(startValue.z, endValue.z, process);
        CachedTransform.localPosition = new Vector3(x, y, z);
    }

    public override void Reset()
    {
        CachedTransform.localPosition = (Vector3)_startValue;
    }

    private Transform _cachedTransform;
    public Transform CachedTransform
    {
        get
        {
            if (_cachedTransform == null)
                _cachedTransform = transform;
            return _cachedTransform;
        }
    }
}
