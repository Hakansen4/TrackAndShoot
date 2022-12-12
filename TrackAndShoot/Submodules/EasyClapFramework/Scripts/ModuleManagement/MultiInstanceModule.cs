using EasyClap.Seneca.Common.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace EasyClap.Seneca.Common.ModuleManagement
{
    /// <summary>
    /// A multi-instance <see cref="Module"/>.
    /// </summary>
    /// <example>
    /// 1. Implement:
    /// <code>
    /// [CreateAssetMenu(...)]
    /// public class Foo : MultiInstanceModule&lt;Foo&gt;
    /// { ... }
    /// </code>
    /// 2. Create an instance in the project (it doesn't have to be under a Resources folder). Register it on the
    /// <see cref="ModuleRegistry"/> instance in the Editor.
    /// </example>
    public abstract class MultiInstanceModule<T> : Module
        where T : MultiInstanceModule<T>
    {
        private static readonly string FormattedTypeName = Utils.GetFormattedName<T>();

#if UNITY_EDITOR
        // ReSharper disable InconsistentNaming
        private const string __Inspector_GroupName_ToolboxFoldoutName = "Multi-Instance Module Toolbox";

        [Button("Full Bootstrap")]
        [FoldoutGroup(__Inspector_GroupName_ToolboxFoldoutName, -1, Expanded = false), PropertyOrder(1)]
        [PropertyTooltip("Runs the same Bootstrap method used by the runtime bootstrapper.")]
        [InfoBox("Tools here may have unintended side-effects. Proceed with caution.", InfoMessageType.Warning)]
        [InfoBox("CURRENTLY IN PLAYMODE\nModules are resolved and initialized automatically at the beginning of runtime, you don't need to manually do it.", InfoMessageType.Warning, "@UnityEngine.Application.isPlaying")]
        private void __Inspector_BootstrapButton()
        {
            var assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            Debug.Log($"{FormattedTypeName} ({assetPath}): Running full bootstrap.");
            Bootstrap();
        }
        // ReSharper restore InconsistentNaming
#endif // UNITY_EDITOR
    }
}
