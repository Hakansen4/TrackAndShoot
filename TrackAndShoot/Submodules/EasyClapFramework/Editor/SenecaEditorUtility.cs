using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EasyClap.Seneca.Common.Editor
{
    [PublicAPI]
    public static class SenecaEditorUtility
    {
        public const string BulletPoint = "•";

        /// <summary>
        /// Pushes an indent level to the stack which is the current indent level increased by one.
        /// Remember to pop with <see cref="GUIHelper.PopIndentLevel"/>.
        /// </summary>
        /// <seealso cref="IndentByOneContext"/>
        public static void PushIndentByOne()
        {
            GUIHelper.PushIndentLevel(Mathf.RoundToInt(GUIHelper.CurrentIndentAmount + 1));
        }

        /// <summary>
        /// Alias for <see cref="GUIHelper.PopIndentLevel">GUIHelper.PopIndentLevel()</see>.
        /// </summary>
        public static void PopIndentLevel()
        {
            GUIHelper.PopIndentLevel();
        }

        public static void DrawLine(Color? color = null, int thickness = 1, int padding = 10)
        {
            var r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2f;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color ?? Color.black);
        }

        public static void DrawPingButton(UnityEngine.Object obj)
        {
            if (GUILayout.Button("Ping", EditorStyles.miniButton))
            {
                EditorGUIUtility.PingObject(obj);
            }
        }

        public static T DrawEnumPopup<T>(string label, T currentValue) where T : Enum
        {
            return (T)EditorGUILayout.EnumPopup(label, currentValue);
        }

        public static bool Confirm(string message)
        {
            return EditorUtility.DisplayDialog("Are you sure?", message, "Yes", "No");
        }

        public static void DrawObjectList(ref Vector2 scrollPosition, IEnumerable<Object> objects, string title)
        {
            EditorGUILayout.LabelField(title);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (var obj in objects)
            {
                if (obj)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{BulletPoint} {obj.name}");
                    DrawPingButton(obj);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();
        }

        public static void ClearFocus()
        {
            GUI.FocusControl(null);
        }

        public static int Buttonbar(string[] texts) => Buttonbar(texts, null);
        public static int Buttonbar(string[] texts, Action onAnyClick)
        {
            var selectedIndex = GUILayout.Toolbar(-1, texts);

            if (selectedIndex >= 0)
            {
                ClearFocus(); // This just makes sense. It should have been the default behaviour of Toolbar.
                onAnyClick?.Invoke();
            }

            return selectedIndex;
        }
    }
}
