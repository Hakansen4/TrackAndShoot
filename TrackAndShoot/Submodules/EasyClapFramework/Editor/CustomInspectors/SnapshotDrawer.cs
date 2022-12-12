using EasyClap.Seneca.Common.Snapshotting;
using EasyClap.Seneca.Common.Utility;
using JetBrains.Annotations;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace EasyClap.Seneca.Common.Editor.CustomInspectors
{
    /// <summary>
    /// Property drawer for <see cref="Snapshot"/>.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Itself)]
    internal class SnapshotDrawer : FoldoutDrawer<Snapshot>
    {
        private Space _positionSpace = Space.Self;
        private Space _rotationSpace = Space.Self;
        private Space _scaleSpace = Space.Self;
        private bool _copyFromTransformFoldout;

        private Object OwnerObject => Property.Tree.UnitySerializedObject.targetObject;
        [CanBeNull] private Component OwnerComponentOrNull => OwnerObject as Component;
        [CanBeNull] private GameObject OwnerGameObjectOrNull => OwnerComponentOrNull.Maybe(x => x.gameObject);
        [CanBeNull] private Transform OwnerTransformOrNull => OwnerGameObjectOrNull.Maybe(x => x.transform);

        protected override NextDrawerOrder GetNextDrawerOrder()
        {
            return NextDrawerOrder.Before;
        }

        protected override void DrawCustomFoldoutContent()
        {
            if (!OwnerTransformOrNull)
            {
                EditorGUILayout.HelpBox("Copy from Transform tool is not available: Cannot detect an owner GameObject.", MessageType.Info);
                return;
            }

            _copyFromTransformFoldout = SirenixEditorGUI.Foldout(_copyFromTransformFoldout, "Copy from Transform");

            if (!_copyFromTransformFoldout)
            {
                return;
            }

            EditorGUILayout.HelpBox("Don't panic if the rotation does not turn out to exact same euler angles after copying. It will still be the same quaternion.", MessageType.Info);

            _positionSpace = SenecaEditorUtility.DrawEnumPopup(ObjectNames.NicifyVariableName(nameof(_positionSpace)), _positionSpace);
            _rotationSpace = SenecaEditorUtility.DrawEnumPopup(ObjectNames.NicifyVariableName(nameof(_rotationSpace)), _rotationSpace);
            _scaleSpace = SenecaEditorUtility.DrawEnumPopup(ObjectNames.NicifyVariableName(nameof(_scaleSpace)), _scaleSpace);

            if (_scaleSpace != Space.Self)
            {
                EditorGUILayout.HelpBox($"{ObjectNames.NicifyVariableName(nameof(_scaleSpace))} is currently set to {_scaleSpace}. Transform.lossyScale does not have a setter, therefore all Snapshot scale setters are designed to ONLY set to Transform.localScale. Make sure you know what you are doing.", MessageType.Warning);
            }

            if (GUILayout.Button("Copy from Transform"))
            {
                var transformSnapshot = OwnerTransformOrNull.TakeSnapshot(_positionSpace, _rotationSpace, _scaleSpace);
                ValueEntry.SmartValue = transformSnapshot;
                SenecaEditorUtility.ClearFocus();
            }
        }
    }
}
