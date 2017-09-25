using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.UI
{

    public static class UGUITools
    {
        private const float kWidth = 160f;
        private const float kThickHeight = 40f;
        public static Vector2 s_ThickElementSize = new Vector2(kWidth, kThickHeight);
        private static Color s_TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);
        private static Vector2 s_ImageElementSize = new Vector2(100f, 100f);

        public static GameObject CreateUIElementRoot(string name, Vector2 size)
        {
            GameObject child = new GameObject(name);
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }

        public static Text CreateText(GameObject parent)
        {
            GameObject go = CreateUIElementRoot("Text", s_ThickElementSize);
            go.layer = GameSetting.LAYER_VALUE_UI;
            Text lbl = go.AddComponent<Text>();
            lbl.text = "New Text";
            lbl.raycastTarget = false;
            lbl.supportRichText = false;
            SetDefaultTextValues(lbl);

            lbl.rectTransform.SetParent(parent.transform);
            lbl.rectTransform.localScale = Vector3.one;
            lbl.rectTransform.localPosition = new Vector3(0, 0, 0);

            return lbl;
        }

        public static Image CreateImage(GameObject parent, Vector2 size)
        {
            GameObject go = CreateUIElementRoot("Image", size);
            Image image = go.AddComponent<Image>();
            image.rectTransform.SetParent(parent.transform);
            image.rectTransform.localScale = Vector3.one;
            image.rectTransform.localPosition = new Vector3(0, 0, 0);
            return image;
        }

        public static Image CreateImage(GameObject parent, GameObject prefab)
        {
            GameObject go = Object.Instantiate(prefab);
            Image image = go.GetComponent<Image>();
            image.rectTransform.SetParent(parent.transform);
            image.rectTransform.localScale = Vector3.one;
            image.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
            return image;
        }

        public static Image ResetImage(GameObject go)
        {
            Image image = go.GetComponent<Image>();
            image.rectTransform.localScale = Vector3.one;
            image.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
            return image;
        }

        public static void SetDefaultTextValues(Text lbl)
        {
#if UNITY_EDITOR
            var font = AssetDatabase.LoadAssetAtPath<Font>("Assets/ResourcesLib/TTFFont/GameFont.ttf");
            lbl.font = font;
#else
            lbl.font = Util.GameFont;
#endif
        }

        public static GameObject AddChild(GameObject parent)
        {
            GameObject go = new GameObject();
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
            if (parent != null)
            {
                //SetParentAndAlign(go, parent);
                Transform t = go.transform;
                t.SetParent(parent.transform);
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }
            return go;
        }

        /// <summary>
        /// 更换父类
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="go"></param>
        static public void AddChild(Transform parent, GameObject go)
        {
            if (go != null && parent != null)
            {
                //SetParentAndAlign(go, parent.gameObject);
                Transform t = go.transform;
                t.SetParent(parent);
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                if (go.layer != parent.gameObject.layer)
                {
                    SetLayer(t, parent.gameObject.layer);
                }
            }
        }

        /// <summary>
        /// Instantiate an object and add it to the specified parent.
        /// </summary>

        static public GameObject AddChild(GameObject parent, GameObject prefab)
        {
            if (prefab == null)
                return null;
            GameObject go = GameObject.Instantiate(prefab) as GameObject;

#if UNITY_EDITOR && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif

            if (go != null && parent != null)
            {
                //SetParentAndAlign(go, parent);
                Transform t = go.transform;
                t.SetParent(parent.transform);
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                if (go.layer != parent.layer)
                {
                    SetLayer(t, parent.layer);
                }
            }
            Util.RefreshPanelShaderInEditor(go);
            return go;
        }

        static public void SetLayer(Transform tran, int layer)
        {
            tran.gameObject.layer = layer;
            int childCount = tran.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                var tChild = tran.GetChild(i);
                SetLayer(tChild, layer);
            }
        }

        public static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform, false);
            SetLayerRecursively(child, parent.layer);
        }

        private static void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++)
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
        }

        /// <summary>
        /// 适配全屏
        /// </summary>
        static public void SetStretch(GameObject go)
        {
            RectTransform trn = go.GetRectTransform();
            if (trn != null)
            {
                trn.offsetMax = Vector2.zero;
                trn.offsetMin = Vector2.zero;
            }
            else
            {
                go.transform.localPosition = Vector3.one;
            }
        }

        /// <summary>
        /// Returns the hierarchy of the object in a human-readable format.
        /// </summary>

        static public string GetHierarchy(GameObject obj)
        {
            string path = obj.name;

            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = obj.name + "\\" + path;
            }
            return path;
        }

    }

   
}


