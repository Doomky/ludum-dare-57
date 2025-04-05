using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.EditorGUILayout;
#endif

namespace Framework.Helpers
{
#if UNITY_EDITOR
    public static class GUILayoutHelpers
    {
        public static readonly GUIStyle DefaultLabelStyle = new(GUI.skin.label)
        {
                    richText = true,
                    wordWrap = true
        };

        private static readonly GUIContent toggleOnContent = EditorGUIUtility.IconContent("IN foldout on", "edit");
        private static readonly GUIContent toggleOffContent = EditorGUIUtility.IconContent("IN foldout", "edit");

        public static bool Toggle(bool state)
        {
            GUIContent editButtonContent = state ? toggleOnContent : toggleOffContent;
            return GUILayout.Toggle(state, editButtonContent, GUI.skin.button, GUILayout.Width(20), GUILayout.MaxWidth(20));
        }

        public static bool RichFoldout(string content, bool state)
        {
            using (new HorizontalScope(GUI.skin.box))
            {
                string[] lines = content.Split("\n");
                using (new VerticalScope())
                {
                    foreach (string line in lines)
                    {
                        GUILayout.Label(line, DefaultLabelStyle);
                    }
                }

                return Toggle(state);
            }
        }
    }
#endif
}