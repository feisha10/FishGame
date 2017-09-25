using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITweenAlpha : UITween
{
    public override void WorkFunction(float process)
    {
        CachedCanvasGroup.alpha = Mathf.Lerp((float)StartValue, (float)EndValue, process);
    }

    public override void Reset()
    {
        CachedCanvasGroup.alpha = (float)_startValue;
    }


    private CanvasGroup _canvasGroup;

    public CanvasGroup CachedCanvasGroup
    {
        get
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            return _canvasGroup;
        }
    }
}
