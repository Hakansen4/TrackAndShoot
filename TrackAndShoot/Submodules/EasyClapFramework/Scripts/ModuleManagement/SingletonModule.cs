// ReSharper disable StaticMemberInGenericType

using EasyClap.Seneca.Common.Core;
using EasyClap.Seneca.Common.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.ModuleManagement
{
    /// <summary>
    /// A singleton <see cref="Module"/>.
    /// </summary>
    /// <typeparam name="T">The deriving class itself.</typeparam>
    /// <example>
    /// 1. Implement:
    /// <code>
    /// [CreateAssetMenu(...)]
    /// public class Foo : SingletonScriptableObjectModule&lt;Foo&gt;
    /// { ... }
    /// </code>
    /// 2. Create an instance in the project (it doesn't have to be under a Resources folder). Register it on the
    /// <see cref="ModuleRegistry"/> instance in the Editor.
    /// <para/>
    /// 3. Singleton access:
    /// <code>Foo.Instance</code>
    /// </example>
    public abstract class SingletonModule<T> : Module
        where T : SingletonModule<T>
    {
        private static readonly string FormattedTypeName = Utils.GetFormattedName<T>();
        private static readonly string NoInstanceExceptionMessage = $"No instances of {FormattedTypeName} is registered! Make sure you have an instance registered on the {nameof(ModuleRegistry)}.";
        private static readonly string MultipleInstanceExceptionMessage = $"An instance of {FormattedTypeName} was already registered! Make sure you have only one of it registered on the {nameof(ModuleRegistry)}.";
        private static readonly string EditorNoInstanceExceptionMessage = $"No instances of {FormattedTypeName} is registered! Make sure you have an instance registered on the {nameof(ModuleRegistry)}. If you do, you can use the toolbox on the module to resolve that module only in editor-time. You can also use the toolbox of the {nameof(ModuleRegistry)} and resolve all the modules registered on it in editor-time.";
        private static readonly string EditorUsageWarningMessage = $"{FormattedTypeName}: Returning the already-resolved instance in the Editor. Be aware that this may have unintended side-effects.";

        private static T _instance;

        /// <summary>
        /// The singleton instance of <typeparamref name="T"/>.
        /// </summary>
        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    if (!_instance)
                    {
                        throw new SenecaCommonException(EditorNoInstanceExceptionMessage);
                    }

                    Debug.LogWarning(EditorUsageWarningMessage);
                    return _instance;
                }
#endif // UNITY_EDITOR

                return _instance ? _instance : throw new SenecaCommonException(NoInstanceExceptionMessage);
            }
        }

        internal sealed override void Bootstrap()
        {
            if (_instance)
            {
                throw new SenecaCommonException(MultipleInstanceExceptionMessage);
            }

            _instance = (T)this;
            Init();
        }

#if UNITY_EDITOR
        // ReSharper disable InconsistentNaming
        private const string __Inspector_GroupName_Toolbox = "Sngleton Module Toolbox";

        [ShowInInspector, DisplayAsString, HideLabel, EnableGUI]
        [FoldoutGroup(__Inspector_GroupName_Toolbox, -1, Expanded = false), PropertyOrder(0)]
        [GUIColor("@!_instance ? Color.red : _instance == this ? Color.green : Color.yellow")]
        private string __Inspector_SingletonInstanceState => $"Current singleton instance: {(!_instance ? "Not resolved" : _instance == this ? "This instance" : "Another instance")}";

        [InfoBox("Tools here may have unintended side-effects. Proceed with caution.", InfoMessageType.Warning)]
        [InfoBox("CURRENTLY IN PLAYMODE\nModules are resolved and initialized automatically at the beginning of runtime, you don't need to manually do it.", InfoMessageType.Warning, "@UnityEngine.Application.isPlaying")]

        [Button("Full Bootstrap")]
        [FoldoutGroup(__Inspector_GroupName_Toolbox), PropertyOrder(1)]
        [PropertyTooltip("Runs the same Bootstrap method used by the runtime bootstrapper.")]
        private void __Inspector_BootstrapButton()
        {
            Debug.Log($"{FormattedTypeName}: Running full bootstrap.");
            Bootstrap();
        }

        [Button("Force Resolve (No Init)")]
        [FoldoutGroup(__Inspector_GroupName_Toolbox), PropertyOrder(2)]
        [PropertyTooltip("Forcefully replaces the singleton instance with self, but doesn't run the initialization.")]
        private void __Inspector_ForceResolveSelfButton()
        {
            Debug.Log($"{FormattedTypeName}: Force resolving (no init).");
            _instance = (T)this;
        }

        [Button("Init Only (No Resolve)")]
        [FoldoutGroup(__Inspector_GroupName_Toolbox), PropertyOrder(3)]
        [PropertyTooltip("Runs the initialization, but doesn't change the singleton instance.")]
        private void __Inspector_InitOnlyButton()
        {
            Debug.Log($"{FormattedTypeName}: Initializing (no resolve).");
            Init();
        }

        [Button("Forget Singleton")]
        [FoldoutGroup(__Inspector_GroupName_Toolbox), PropertyOrder(4)]
        [PropertyTooltip("Forgets the current singleton instance.")]
        private void __Inspector_ForgetButton()
        {
            Debug.Log($"{FormattedTypeName}: Forgetting the singleton instance.");
            _instance = null;
        }
        // ReSharper restore InconsistentNaming
#endif // UNITY_EDITOR
    }
}
