using EasyClap.Seneca.Common.PlayerPrefsUtils;
using EasyClap.Seneca.Common.Utility;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace EasyClap.Seneca.Common.Editor.CustomInspectors.PlayerPrefsEntries
{
    /// <summary>
    /// Base of property drawers for <see cref="PlayerPrefsEntryBase{TValue}"/>.
    /// </summary>
    internal abstract class PlayerPrefsEntryDrawerBase<TEntry, TVal> : FoldoutDrawer<TEntry>
        where TEntry : PlayerPrefsEntryBase<TVal>
    {
        private bool _editFoldout = false;
        private TVal _newValue = default;

        protected override void Initialize()
        {
            base.Initialize();
            _newValue = ValueEntry.SmartValue.Value;
        }

        protected override GUIContent GetFoldoutTitle(GUIContent label)
        {
            var newVal = new GUIContent(label);
            newVal.text += $" ({typeof(TEntry).GetFormattedName().Nicify()})";
            return newVal;
        }

        // protected override void DrawCustomHeaderContentAfterTitle()
        // {
        //     if (IsFoldoutExpanded)
        //     {
        //         GUILayout.FlexibleSpace();
        //         EditorGUILayout.LabelField("Edit Mode", GUILayoutOptions.Width(60));
        //         _editMode = EditorGUILayout.Toggle(_editMode, GUILayoutOptions.Width(20));
        //     }
        // }

        protected override void DrawCustomFoldoutContent()
        {
            var val = ValueEntry.SmartValue;

            using (new GuiEnabledMementoContext())
            using (new LabelWidthMementoContext())
            {
                EditorGUIUtility.labelWidth = 75;

                GUI.enabled = false;
                EditorGUILayout.TextField(ObjectNames.NicifyVariableName(nameof(val.Key)), val.Key);
                EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(nameof(val.HasValue)), val.HasValue);
                DrawValue(ObjectNames.NicifyVariableName(nameof(val.Value)), val.Value);

                GUI.enabled = true;
                DrawEditTools(val);
            }
        }

        private void DrawEditTools(TEntry val)
        {
            SirenixEditorGUI.BeginBox();
            _editFoldout = SirenixEditorGUI.Foldout(_editFoldout, "Edit Tools");
            if (_editFoldout)
            {
                _newValue = DrawValue(ObjectNames.NicifyVariableName(nameof(_newValue)), _newValue);

                if (GUILayout.Button("Save"))
                {
                    if (SenecaEditorUtility.Confirm($"You are about to write to a PlayerPrefs entry:\n\nKey: {val.Key}\nProposed Value: {_newValue}\n\nThere is no undo."))
                    {
                        val.Value = _newValue;
                    }
                }

                if (GUILayout.Button(ObjectNames.NicifyVariableName(nameof(val.Delete))))
                {
                    if (SenecaEditorUtility.Confirm($"You are about to delete a PlayerPrefs entry:\n\nKey: {val.Key}\n\nThere is no undo."))
                    {
                        val.Delete();
                    }
                }
            }
            SirenixEditorGUI.EndBox();
        }

        protected override NextDrawerOrder GetNextDrawerOrder()
        {
            return NextDrawerOrder.Never;
        }

        protected abstract TVal DrawValue(string name, TVal value);
    }
}
