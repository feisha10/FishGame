using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class ExtendFuncHelper
{
    /// <summary>
    /// 【扩展】设置UI组件的父对象、坐标
    /// </summary>
    /// <param name="source"></param>
    /// <param name="parent"></param>
    /// <param name="x">如果为float.NaN，则不更改</param>
    /// <param name="y">如果为float.NaN，则不更改</param>
    public static void SetUILocation(this RectTransform source, Transform parent, float x = float.NaN, float y = float.NaN)
    {
        source.SetParent(parent, false);
        var pos = source.anchoredPosition;
        if (!float.IsNaN(x))
            pos.x = x;
        if (!float.IsNaN(y))
            pos.y = y;
        source.anchoredPosition = pos;
    }

    static public T AddMissingComponent<T>(this GameObject go) where T : Component
    {
        if (go == null)
            return null;

        T comp = go.GetComponent<T>();
        if (comp == null)
            comp = go.AddComponent<T>();
        return comp;
    }
    
    public static RectTransform GetRectTransform(this GameObject source)
    {
        return source.GetComponent<RectTransform>();
    }

    public static void ScrollToTop(this ScrollRect scrollRect)
    {
        scrollRect.verticalNormalizedPosition = 1f;
    }

    public static void ScrollToBottom(this ScrollRect scrollRect)
    {
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public static void ScrollToCenterVertical(this ScrollRect scrollRect)
    {
        scrollRect.verticalNormalizedPosition = 0.5f;
    }

    public static void ScrollToLeft(this ScrollRect scrollRect)
    {
        scrollRect.horizontalNormalizedPosition = 1f;
    }

    public static void ScrollToRight(this ScrollRect scrollRect)
    {
        scrollRect.horizontalNormalizedPosition = 0f;
    }

    public static void ScrollToCenterHorizontal(this ScrollRect scrollRect)
    {
        scrollRect.horizontalNormalizedPosition = 0.5f;
    }

}
