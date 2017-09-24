using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public static class CameraSetting {

    static public int CAMERA_DEEPTH_SCENECAMERA = 0;
    static public int CAMERA_DEEPTH_UICAMERA = 10;
    static public int CAMERA_DEPTH_HEROCAMERA = 5;

    public static Camera UICamera;
    public static Camera MainCamera; //also Scene camera
    public static Camera HeroCamera; //专注于观察英雄

    public static void InitUICamera()
    {
        if (UICamera == null)
        {
            //GL.Clear(true, true, Color.black);
            UICamera = new GameObject("UICamera").AddComponent<Camera>();
            UICamera.cullingMask = GameSetting.LAYER_MASK_UI;
            UICamera.orthographic = true;
            initUICameraSize(UICamera);
            UICamera.nearClipPlane = -600;
            UICamera.farClipPlane = 200;
            UICamera.depth = CAMERA_DEEPTH_UICAMERA;
            UICamera.useOcclusionCulling = false; //关掉遮挡剔除
            UICamera.backgroundColor = Color.black;
            UICamera.clearFlags = CameraClearFlags.Depth;
            Object.DontDestroyOnLoad(UICamera.gameObject);
            UICamera.gameObject.AddMissingComponent<AudioListener>();
            UICamera.gameObject.AddMissingComponent<GUILayer>();
        }
    }

    static void initUICameraSize(Camera m_camera)
    {
        var result = System.Convert.ToSingle(Screen.width) / System.Convert.ToSingle(Screen.height);
        if (result < 1.5f)
            m_camera.orthographicSize = 15.0f / result;
        else
            m_camera.orthographicSize = 10;
    }

    public static void InitSceneCamera()
    {
        //GL.Clear(true, true, Color.black);
        if (MainCamera == null)
        {
            GameObject go = new GameObject("SceneCamera");
            go.tag = "MainCamera";
            go.transform.localPosition = new Vector3(0, 0, -100);

            MainCamera = go.AddComponent<Camera>();
            MainCamera.orthographic = true;
            MainCamera.orthographicSize = 2.35f;
            MainCamera.cullingMask = GameSetting.LAYER_MASK_DEFAULT;
            MainCamera.depth = CAMERA_DEEPTH_SCENECAMERA;
            MainCamera.renderingPath = RenderingPath.VertexLit;
            MainCamera.gameObject.AddMissingComponent<GUILayer>();
            InitScreenOffset();
        }  
    }

    /// <summary>
    /// 世界坐标到视口坐标的射线投影坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="uiCamera"></param>
    /// <returns></returns>
    public static Vector3 WorldPointToViewportProjection(Vector3 pos)
    {
        pos = MainCamera.WorldToViewportPoint(pos);
        pos.z = 0;

        pos = UICamera.ViewportToWorldPoint(pos);
        return pos;
    }

    static int ScreenOffsetMinX;
    static int ScreenOffsetMaxX;
    static int ScreenOffsetMinY;
    static int ScreenOffsetMaxY;

    //1024 * 676
    public static void InitScreenOffset()
    {
        int screenHeight = Screen.height;
        int screenWidth = Screen.width;

        ScreenOffsetMinX = -(int)(200 * (screenWidth / 1024f));
        ScreenOffsetMaxX = screenWidth + (int)(200 * (screenWidth / 1024f));

        ScreenOffsetMinY = -(int)(40 * (screenHeight / 576f));
        ScreenOffsetMaxY = screenHeight + (int)(40 * (screenHeight / 576f));
    }

    public static bool IsWorldPosInScreenOffset(Vector3 worldPos)
    {
        Vector3 screenPos = MainCamera.WorldToScreenPoint(worldPos);
        if (screenPos.x >= ScreenOffsetMinX && screenPos.x <= ScreenOffsetMaxX && screenPos.y >= ScreenOffsetMinY && screenPos.y <= ScreenOffsetMaxY)
            return true;

        return false;
    }

    public static bool IsWorldPosInScreenOffsetUI(Vector3 worldPos)
    {
        Vector3 screenPos = UICamera.WorldToScreenPoint(worldPos);
        if (screenPos.x >= ScreenOffsetMinX && screenPos.x <= ScreenOffsetMaxX && screenPos.y >= ScreenOffsetMinY && screenPos.y <= ScreenOffsetMaxY)
            return true;

        return false;
    }

    public static bool IsWorldPosInScreen(Vector3 worldPos)
    {
        Vector3 screenPos = MainCamera.WorldToScreenPoint(worldPos);
        int screenHeight = Screen.height;
        int screenWidth = Screen.width;
        if (screenPos.x >= 0 && screenPos.x <= screenWidth && screenPos.y >= 0 && screenPos.y <= screenHeight)
            return true;

        return false;
    }
}
