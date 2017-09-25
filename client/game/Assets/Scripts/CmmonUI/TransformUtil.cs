using UnityEngine;
using System.Collections;

/// <summary>
/// 都是基于UI组件的Transform Local x y相关的计算，包括LocalPosition、LocalScale等
/// </summary>
public class TransformUtil 
{
    public static bool HasParent(Transform src, Transform parent)
    {
        if (null == src || null == parent)
            return false;

        var t = src;
        while (null != t.parent)
        {
            var parentItem = t.parent;
            if (parentItem == parent)
            {
                return true;
            }

            t = parentItem;
        }

        return false;
    }

    public static void SetX(Transform source, float newX)
    {
        if (Mathf.Approximately(source.localPosition.x,newX))
            return;
        Vector3 localPos = source.localPosition;
        localPos.x = newX;
        source.localPosition = localPos;
    }

    public static void SetY(Transform source, float newY)
    {
        if (Mathf.Approximately(source.localPosition.y, newY))
            return;
        Vector3 localPos = source.localPosition;
        localPos.y = newY;
        source.localPosition = localPos;
    }

    public static void SetZ(Transform source, float newZ)
    {
        if (Mathf.Approximately(source.localPosition.z, newZ))
            return;
        Vector3 localPos = source.localPosition;
        localPos.z = newZ;
        source.localPosition = localPos;
    }

    public static void SetRotationX(Transform source, float newX)
    {
        if (Mathf.Approximately(source.localEulerAngles.x, newX))
            return;
        Vector3 localPos = source.localEulerAngles;
        localPos.x = newX;
        source.localEulerAngles = localPos;
    }

    public static void SetRotationY(Transform source, float newY)
    {
        if (Mathf.Approximately(source.localEulerAngles.y, newY))
            return;
        Vector3 localPos = source.localEulerAngles;
        localPos.y = newY;
        source.localEulerAngles = localPos;
    }  
    
    public static void SetRotationZ(Transform source, float newZ)
    {
        if (Mathf.Approximately(source.localEulerAngles.z, newZ))
            return;
        Vector3 localPos = source.localEulerAngles;
        localPos.z = newZ;
        source.localEulerAngles = localPos;
    }

    public static void SetScaleX(Transform source, float newX)
    {
        if (Mathf.Approximately(source.localScale.x, newX))
            return;
        Vector3 localPos = source.localScale;
        localPos.x = newX;
        source.localScale = localPos;
    }

    public static void SetScaleY(Transform source, float newY)
    {
        if (Mathf.Approximately(source.localScale.y, newY))
            return;
        Vector3 localPos = source.localScale;
        localPos.y = newY;
        source.localScale = localPos;
    }

    public static void SetScaleZ(Transform source, float newZ)
    {
        if (Mathf.Approximately(source.localScale.z, newZ))
            return;
        Vector3 localPos = source.localScale;
        localPos.z = newZ;
        source.localScale = localPos;
    }


    public static void MoveX(Transform source, float moveX)
    {
        Vector3 localPos = source.localPosition;
        localPos.x += moveX;
        source.localPosition = localPos;
    }

    public static void MoveY(Transform source, float moveY)
    {
        Vector3 localPos = source.localPosition;
        localPos.y += moveY;
        source.localPosition = localPos;
    }

    public static void MoveZ(Transform source, float moveZ)
    {
        Vector3 localPos = source.localPosition;
        localPos.z += moveZ;
        source.localPosition = localPos;
    }

    /// <summary>
    /// 浮点向量比较
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static bool IsVectorEqual(Vector3 v1, Vector3 v2)
    {
        return Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y) && Mathf.Approximately(v1.z, v2.z);
    }

    /// <summary>
    /// 点击坐标转换为本地坐标,限制边框 原始点在中心点
    /// </summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    /// <param name="transform"></param>
    /// <param name="uiCamera"></param>
    /// <returns></returns>
    public static Vector3 MousePositionToLoaclPostion(float w, float h, Transform transform, Camera uiCamera)
    {
        float halfH = h/2;
        float halfW = w/2;
        Vector3 pos1 = uiCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 pos2 = transform.InverseTransformPoint(pos1);

        float tempy = pos2.y;

        if (pos2.y - halfH < -286f)
        {
            //超下
            tempy = halfH - 286f;
        }
        else if (pos2.y + halfH > 286f)
        {
            //超上
            tempy = 286f - halfH;
        }

        float tempx = pos2.x;

        if (pos2.x - halfW < -512f)
        {
            //超左
            tempx = halfW - 512f;
        }
        else if (pos2.x + halfW > 512f)
        {
            //超右
            tempx = 480f - halfW;
        }

        return new Vector3(tempx, tempy,0);
    }

	public static Vector3 TranslateForward(Transform source, float length)
	{
		var rY = source.rotation.eulerAngles.y;
		var dir = 1;
		if (rY >= 180)
			dir = -1;

		var resultVector = source.position;
		resultVector.x += length*dir;
		resultVector.x = Mathf.Clamp(resultVector.x, 19, 2);  //需要配置再处理 by he
		return resultVector;
	}

    public static Transform FindChildByName(Transform parent,string childName)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == childName)
            {
                return child;
            }
            if(child.childCount > 0)
            {
                Transform trs = FindChildByName(child, childName);
                if (trs != null) return trs;
            }
        }
        return null;
    }

}
