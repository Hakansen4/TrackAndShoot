using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyClap.Seneca.Common.Editor
{
    public class LookAtTool : EditorWindow
    {
        [MenuItem("Easy Clap/Utility/Look At...")]
        private static void CreateEditorWindow()
        {
            GetWindow<LookAtTool>("Look At", true);
        }

        [SerializeField] private GameObject target = null;
        [SerializeField] private Vector3 worldUp = Vector3.up;
        [SerializeField] private Vector2 lookersListScrollPosition = Vector2.zero;

        void OnSelectionChange()
        {
            Repaint();
        }

        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("This tool will rotate a set of GameObjects such that they all will look at another GameObject.", MessageType.None);
            EditorGUILayout.HelpBox("If you select GameObjects in a parent-child relationship, only the selection on the topmost level will be recognized.", MessageType.None);
            SenecaEditorUtility.DrawLine();

            target = EditorGUILayout.ObjectField("Target", target, typeof(GameObject), true) as GameObject;
            worldUp = EditorGUILayout.Vector3Field("World Up", worldUp);

            EditorGUILayout.Space();

            if (!target)
            {
                EditorGUILayout.HelpBox("No target GameObject selected.", MessageType.Info);
                return;
            }

            if (worldUp.magnitude == 0)
            {
                EditorGUILayout.HelpBox("World Up vector has to be non-zero.", MessageType.Error);
                return;
            }

            var selection = GetSelectionNoAssets();
            if (selection.Length <= 0)
            {
                EditorGUILayout.HelpBox("No GameObjects are selected in the hierarchy.", MessageType.Info);
                return;
            }

            if (GUILayout.Button($"Rotate {selection.Length} objects to look at {target.name}"))
            {
                var confirmation = SenecaEditorUtility.Confirm($"Do you want to rotate {selection.Length} GameObjects from the hierarchy so that they all look at {target.name} GameObject?");

                if (confirmation)
                {
                    var lookerTransforms = selection.Select(x => x.transform).ToArray();
                    RotateAll(lookerTransforms);
                }
            }

            SenecaEditorUtility.DrawLine();
            SenecaEditorUtility.DrawObjectList(ref lookersListScrollPosition, selection, "Following objects will be rotated:");
        }

        private void RotateAll(Transform[] lookers)
        {
            Undo.RecordObjects(lookers, $"Rotate {lookers.Length} transforms to look at {target.name}");

            foreach (var looker in lookers)
            {
                looker.LookAt(target.transform, worldUp);
                PrefabUtility.RecordPrefabInstancePropertyModifications(looker);
            }
        }

        private GameObject[] GetSelectionNoAssets()
        {
            return Selection.GetFiltered<GameObject>(SelectionMode.Editable | SelectionMode.TopLevel)
                .Where(x => !AssetDatabase.Contains(x))
                .ToArray();
        }
    }
}
