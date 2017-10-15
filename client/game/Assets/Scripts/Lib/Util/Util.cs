using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public static class Util
{
    public static void SetGoFrontUiEffect(GameObject go, bool isCanClick, int order = 10)
    {
        bool isActive = true;
        if (!go.activeSelf)
        {
            isActive = false;
            go.SetActive(true);
        }
        Canvas m_canvas = go.AddMissingComponent<Canvas>();
        m_canvas.overrideSorting = true;
        if (isCanClick)
            go.AddMissingComponent<GraphicRaycaster>();
        if (!m_canvas.overrideSorting)
            m_canvas.overrideSorting = true;
        m_canvas.sortingOrder = order;
        if (!isActive)
            go.SetActive(false);
    }

    public static string OutDir
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "out/");
        }
    }

    private static string AssetPath = null;
    private static string AssetPathPathNoTarget = null;
    public static string GetStreamingAssetsPath()
    {
        if (AssetPath != null)
            return AssetPath;

#if UNITY_EDITOR
        AssetPath = string.Format("file://{0}/StreamingAssets/", Application.dataPath);
#elif UNITY_IOS  
        AssetPath = string.Format("file://{0}/Raw/", Application.dataPath);
#elif UNITY_ANDROID
        AssetPath = string.Format("jar:file://{0}!/assets/", Application.dataPath);
#else
        AssetPath = string.Format("file://{0}/StreamingAssets/", Application.dataPath);
#endif

        AssetPath = string.Format("{0}{1}/", AssetPath, GetTargetPath());
        return AssetPath;
    }

    public static string GetStreamingAssetsPathNoTarget()
    {
        if (AssetPathPathNoTarget != null)
            return AssetPathPathNoTarget;

#if UNITY_EDITOR
        AssetPathPathNoTarget = string.Format("file://{0}/StreamingAssets/", Application.dataPath);
#elif UNITY_IOS  
        AssetPathPathNoTarget = string.Format("file://{0}/Raw/", Application.dataPath);
#elif UNITY_ANDROID
        AssetPathPathNoTarget = string.Format("jar:file://{0}!/assets/", Application.dataPath);
#else
        AssetPathPathNoTarget = string.Format("file://{0}/StreamingAssets/", Application.dataPath);
#endif

        return AssetPathPathNoTarget;
    }

    public static string GetTargetPath()
    {
#if UNITY_ANDROID
       return "Android";
#elif UNITY_IOS
       return "IOS";
#elif UNITY_STANDALONE
        return "PC";
#elif UNITY_WEBPLAYER
       return "WEB";
#endif
        return "Android";
    }

    public static void _MaskData(byte[] data)
    {
        uint mask = BitConverter.ToUInt32(data, 0);
        for (int i = 4; i < data.Length; i++)
        {
            int m = i % 4;
            if (m == 0)
                mask = mask * 1103515245 + 12345;
            data[i] ^= (byte)(mask >> (m * 8));
        }
    }

    public static void RefreshPanelShaderInEditor(GameObject go)
    {
#if UNITY_EDITOR
        if (go != null)
        {
            var renders = go.GetComponentsInChildren<Image>(true);
            for (int i = 0; i < renders.Length; i++)
            {
                var mat = renders[i].material;
                if (mat != null)
                {
                    mat.shader = Shader.Find(mat.shader.name);
                }
            }
        }
#endif
    }

    public static void CreateFileDirectory(string filePath)
    {
        string dir = Path.GetDirectoryName(filePath);
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
    }

    public static void SetLayer(Transform tran, int layer)
    {
        tran.gameObject.layer = layer;
        int childCount = tran.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            var tChild = tran.GetChild(i);
            SetLayer(tChild, layer);
        }
    }

    public static void SetLayer(GameObject go, int layer)
    {
        var tran = go.transform;
        go.layer = layer;
        int childCount = tran.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            var tChild = tran.GetChild(i);
            SetLayer(tChild, layer);
        }
    }
}
