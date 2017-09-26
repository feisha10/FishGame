using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    // TODO REVIEW
    // Have material live under text
    // move stencil mask into effects *make an efects top level element like there is
    // paragraph and character

    /// <summary>
    /// Editor class used to edit UI Labels.
    /// </summary>

    [CustomEditor(typeof(Text), true)]
    [CanEditMultipleObjects]
    public class TextEditor : GraphicEditor
    {
        SerializedProperty m_Text;
        SerializedProperty m_FontData;
        Text mText;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Text = serializedObject.FindProperty("m_Text");
            m_FontData = serializedObject.FindProperty("m_FontData");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            mText = target as Text;

            EditorGUILayout.PropertyField(m_Text);

            DrawColorEnum();

            EditorGUILayout.PropertyField(m_FontData);
            AppearanceControlsGUI();
            RaycastControlsGUI();
            serializedObject.ApplyModifiedProperties();
        }

        void DrawColorEnum()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Choose Color:", GUILayout.Width(100));
            GUILayout.EndHorizontal();

            GUILayout.Space(2);

            GUILayout.BeginHorizontal();

            CreateColorBtn("White",ColorEnum.White);
            CreateColorBtn("Green", ColorEnum.Black);

            CreateColorBtn("RoleLev", ColorEnum.RoleLev);
            CreateColorBtn("Blue1", ColorEnum.Blue1);
            CreateColorBtn("Blue2", ColorEnum.Blue2);
            CreateColorBtn("Yellow", ColorEnum.Yellow);

            GUILayout.EndHorizontal();
            GUILayout.Space(2);
        }

        void CreateColorBtn(string tooltip, Color color)
        {
            if (IsPressColorBtn(tooltip, color))
            {
                mText.color = color;
                EditorUtility.SetDirty(mText);
            }
        }

        bool IsPressColorBtn(string tooltip, Color color)
        {
            var clr = color;
            var texture = new Texture2D(16, 16, TextureFormat.RGB24, false, true);
            texture.wrapMode = TextureWrapMode.Repeat;
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    texture.SetPixel(i, j, clr);
                }
            }
            texture.Apply();
            var result = GUILayout.Button(new GUIContent(texture, tooltip), GUILayout.Width(24), GUILayout.Height(24));
            DestroyImmediate(texture);
            return result;
        }
    }
}
