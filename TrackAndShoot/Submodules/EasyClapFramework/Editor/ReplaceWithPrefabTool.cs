using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyClap.Seneca.Common.Editor
{
    public class ReplaceWithPrefabTool : EditorWindow
    {
        private enum NamingMethod
        {
            Preserve,
            PrefabEnumerate,
            PrefabExact
        }

        [MenuItem("Easy Clap/Utility/Replace With Prefab...")]
        private static void CreateEditorWindow()
        {
            GetWindow<ReplaceWithPrefabTool>("Replace With Prefab", true);
        }

        [SerializeField] private GameObject selectedPrefab = null;
        [SerializeField] private Vector2 replaceListScrollPosition = Vector2.zero;
        [SerializeField] private NamingMethod namingMethod = NamingMethod.PrefabEnumerate;

        void OnSelectionChange()
        {
            Repaint();
        }

        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("This tool will replace the selected objects in the hierarchy with the selected prefab.", MessageType.None);
            EditorGUILayout.HelpBox("If you select GameObjects in a parent-child relationship, only the selection on the topmost level will be recognized (because when we replace an object, all descendants are destroyed anyway).", MessageType.None);
            SenecaEditorUtility.DrawLine();

            namingMethod = SenecaEditorUtility.DrawEnumPopup("Naming method", namingMethod);
            SenecaEditorUtility.DrawLine();

            selectedPrefab = EditorGUILayout.ObjectField("Prefab", selectedPrefab, typeof(GameObject), false) as GameObject;
            EditorGUILayout.Space();
            if (!selectedPrefab)
            {
                EditorGUILayout.HelpBox("No prefab selected.", MessageType.Info);
                return;
            }

            var selection = GetSelectionNoAssets();
            if (selection.Length <= 0)
            {
                EditorGUILayout.HelpBox("No GameObjects are selected in the hierarchy.", MessageType.Info);
                return;
            }

            if (GUILayout.Button($"Replace {selection.Length} objects with {selectedPrefab.name}"))
            {
                var confirmation = SenecaEditorUtility.Confirm($"Do you want to replace {selection.Length} GameObjects from the hierarchy with {selectedPrefab.name} prefab?");
                
                if (confirmation)
                {
                    ReplaceAll(selection);
                }
            }

            SenecaEditorUtility.DrawLine();
            SenecaEditorUtility.DrawObjectList(ref replaceListScrollPosition, selection, "Following objects will get replaced:");
        }

        private void ReplaceAll(IReadOnlyList<GameObject> gameObjects)
        {
            for (var i = gameObjects.Count - 1; i >= 0; --i)
            {
                var itSelection = gameObjects[i];
                ReplaceSingle(itSelection, i);
            }
        }

        private GameObject[] GetSelectionNoAssets()
        {
            return Selection.GetFiltered<GameObject>(SelectionMode.Editable | SelectionMode.TopLevel)
                .Where(x => !AssetDatabase.Contains(x))
                .ToArray();
        }

        private void ReplaceSingle(GameObject gameObject, int index)
        {
            var prevName = gameObject.name;

            var newObject = PrefabUtility.InstantiatePrefab(selectedPrefab) as GameObject;

            if (!newObject)
            {
                Debug.LogError($"{nameof(ReplaceWithPrefabTool)}: Error while replacing {gameObject.name} with {selectedPrefab.name}: Could not instantiate prefab.");
                return;
            }

            Undo.RegisterCreatedObjectUndo(newObject, $"Replace {gameObject.name} With Prefab {selectedPrefab.name}");

            newObject.transform.parent = gameObject.transform.parent;
            newObject.transform.localPosition = gameObject.transform.localPosition;
            newObject.transform.localRotation = gameObject.transform.localRotation;
            newObject.transform.localScale = gameObject.transform.localScale;
            newObject.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex());

            newObject.name = namingMethod switch
            {
                NamingMethod.Preserve => prevName,
                NamingMethod.PrefabEnumerate => $"{selectedPrefab.name}{(index == 0 ? "" : $" {index}")}",
                NamingMethod.PrefabExact => $"{selectedPrefab.name}",
                _ => throw new ArgumentOutOfRangeException()
            };

            Undo.DestroyObjectImmediate(gameObject);
        }
    }
}
