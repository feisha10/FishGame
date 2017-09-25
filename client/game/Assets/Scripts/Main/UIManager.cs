using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UIManager {

    public static RectTransform SceneUiLayer;
    public static RectTransform TipLayer;
    public static RectTransform UiLayer;
    public static RectTransform GuideLayer;
    public static RectTransform EffectLayer;
    public static RectTransform m_rootUICanvas;
    public static RectTransform m_rootScenceCanvas;


    public static void Init(SceneName name)
    {
        CameraSetting.InitUICamera();
        if (m_rootUICanvas == null)
        {
            var rootCanvasGo = new GameObject("UGUIRootCanvas");
            rootCanvasGo.layer = GameSetting.LAYER_VALUE_UI;
            Canvas canvas = rootCanvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = CameraSetting.UICamera;
            canvas.pixelPerfect = false;
            canvas.sortingOrder = 0;
            canvas.planeDistance = 0;
            var scaler = rootCanvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1024f, 576f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            rootCanvasGo.AddComponent<GraphicRaycaster>();
            m_rootUICanvas = rootCanvasGo.GetRectTransform();
        }
        if (m_rootScenceCanvas == null)
        {
            var rootCanvasGo = new GameObject("UGUISceneUiRootCanvas");
            rootCanvasGo.layer = GameSetting.LAYER_VALUE_UI;
            Canvas canvas = rootCanvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = CameraSetting.UICamera;
            canvas.pixelPerfect = false;
            canvas.sortingOrder=-1;
            canvas.planeDistance =100;
            var scaler = rootCanvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1024f, 576f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            rootCanvasGo.AddComponent<GraphicRaycaster>();
            m_rootScenceCanvas = rootCanvasGo.GetRectTransform();
        }

        switch (name)
        {
            case SceneName.MainScene:
                if (SceneUiLayer != null)
                {
                    Object.DestroyImmediate(SceneUiLayer.gameObject);
                    SceneUiLayer=null;
                }
                if (TipLayer != null && !GmView.Active)
                {
                    Object.DestroyImmediate(TipLayer.gameObject);
                    TipLayer = null;
                }

                if (UiLayer == null)    
                   UiLayer = AddUiLayer("UILayer");
                if (TipLayer == null)
                {
                    TipLayer = AddUiLayer("TipLayer");
                    Util.SetGoFrontUiEffect(TipLayer.gameObject, true, 20);
                    TipLayer.localPosition = new UnityEngine.Vector3(0, 0, -1000);
                }
                if (GuideLayer == null)
                {
                    GuideLayer = AddUiLayer("GuideLayer");
                    Util.SetGoFrontUiEffect(GuideLayer.gameObject, true, 15);
                    GuideLayer.localPosition = new UnityEngine.Vector3(0, 0, -1000);
                }
                if (EffectLayer == null) 
                    EffectLayer = AddUiLayer("EffectLayer"); 
                break;

        }     
    }

    private static RectTransform AddUiLayer(string name)
    {
        var go = new GameObject(name);
        var rect = go.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.SetUILocation(m_rootUICanvas);
        go.layer = GameSetting.LAYER_VALUE_UI;
        return rect;
    }
    private static RectTransform AddSceneUiLayer(string name)
    {
        var go = new GameObject(name);
        var rect = go.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.SetUILocation(m_rootScenceCanvas);
        go.layer = GameSetting.LAYER_VALUE_UI;
        return rect;
    }
}
