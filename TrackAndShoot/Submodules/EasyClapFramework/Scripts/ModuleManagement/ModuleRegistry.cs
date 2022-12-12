using System;
using System.Collections.Generic;
using System.Reflection;
using EasyClap.Seneca.Common.Core;
using EasyClap.Seneca.Common.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace EasyClap.Seneca.Common.ModuleManagement
{
    /// <summary>
    /// Responsible for initializing <see cref="Module"/> instances assigned to it. The single instance needs to be
    /// placed under the root of a Resources folder.
    /// </summary>
    /// <remarks>
    /// Why do we need a system like this?
    /// The naive approach to implementing singleton ScriptableObjects does not work. I first tried to implement it in
    /// the same way I did with runtime singletons: A generic base class that handles all the boilerplate. This however
    /// did not work, because apparently <see cref="RuntimeInitializeOnLoadMethodAttribute"/> attribute does not work
    /// with generic base classes. It is impossible to make a type-safe implementation without a generic base class,
    /// because we need the static singleton instance reference to have the exact type of the implementor. The only
    /// other solution except from this one is to offload the singleton logic to individual implementations, which
    /// creates a lot of bloat and repeated code. Therefore this solution came about.
    /// </remarks>
    [CreateAssetMenu(fileName = TypeName, menuName = MenuName)]
    public sealed class ModuleRegistry : ScriptableObject
    {
        private const string TypeName = nameof(ModuleRegistry);
        private const string MenuName = Constants.EasyClap + "/" + TypeName;

        [AssetSelector, AssetsOnly]
        [SerializeField] private Module[] modules;

        internal void Bootstrap()
        {
            foreach (var module in modules)
            {
                module.Bootstrap();
            }

            foreach (var module in modules)
            {
                module.BootstrapSecondPass();
            }
        }

        public T GetModules<T>() where T : SingletonModule<T>
        {
            foreach (var module in modules)
            {
                if (module is T t)
                {
                    return t;
                }
            }

            return default;
        }

        public static ModuleRegistry Load()
        {
            return Utils.LoadSingleResource<ModuleRegistry>(nameof(ModuleRegistry));
        }
        
        public static T LoadModule<T>() where T : SingletonModule<T>
        {
            return Load().GetModules<T>();
        }

#if UNITY_EDITOR
        // ReSharper disable InconsistentNaming

        // TODO: Eliminate magic strings used in this section when getting the tool method definitions from modules.

        private const string __Inspector_GroupName_Toolbox = "Module Registry Toolbox";

        [Button("(Multi-Instance Modules) Full Bootstrap")]
        [FoldoutGroup(__Inspector_GroupName_Toolbox, -1), PropertyOrder(1)]
        [PropertyTooltip("For all multi-instance modules registered here: Runs the same Bootstrap method used by the runtime bootstrapper.")]
        [InfoBox("Tools here may have unintended side-effects. Proceed with caution.", InfoMessageType.Warning)]
        [InfoBox("CURRENTLY IN PLAYMODE\nModules are resolved and initialized automatically at the beginning of runtime, you don't need to manually do it.", InfoMessageType.Warning, "@UnityEngine.Application.isPlaying")]
        private void __Inspector_MultiInstanceBootstrapButton()
        {
            Debug.Log($"{TypeName}: (For all registered multi-instance modules) Running full bootstrap.");
            __Inspector_ToolDynamicInvoke(modules, typeof(MultiInstanceModule<>), "__Inspector_BootstrapButton");
        }

        [Button("(Singleton Modules) Full Bootstrap")]
        [FoldoutGroup(__Inspector_GroupName_Toolbox, -1), PropertyOrder(1)]
        [PropertyTooltip("For all singleton modules registered here: Runs the same Bootstrap method used by the runtime bootstrapper.")]
        private void __Inspector_SingletonBootstrapButton()
        {
            Debug.Log($"{TypeName}: (For all registered singleton modules) Running full bootstrap.");
            __Inspector_ToolDynamicInvoke(modules, typeof(SingletonModule<>), "__Inspector_BootstrapButton");
        }

        [Button("(Singleton Modules) Force Resolve (No Init)")]
        [FoldoutGroup(__Inspector_GroupName_Toolbox), PropertyOrder(2)]
        [PropertyTooltip("For all singleton modules registered here: Forcefully replaces the singleton instance with self, but doesn't run the initialization.")]
        private void __Inspector_SingletonForceResolveSelfButton()
        {
            Debug.Log($"{TypeName}: (For all registered singleton modules) Force resolving (no init).");
            __Inspector_ToolDynamicInvoke(modules, typeof(SingletonModule<>), "__Inspector_ForceResolveSelfButton");
        }

        [Button("(Singleton Modules) Init Only (No Resolve)")]
        [FoldoutGroup(__Inspector_GroupName_Toolbox), PropertyOrder(3)]
        [PropertyTooltip("For all singleton modules registered here: Runs the initialization, but doesn't change the singleton instance.")]
        private void __Inspector_SingletonInitOnlyButton()
        {
            Debug.Log($"{TypeName}: (For all registered singleton modules) Initializing (no resolve).");
            __Inspector_ToolDynamicInvoke(modules, typeof(SingletonModule<>), "__Inspector_InitOnlyButton");
        }

        [Button("(Singleton Modules) Forget Singleton")]
        [FoldoutGroup(__Inspector_GroupName_Toolbox), PropertyOrder(4)]
        [PropertyTooltip("For all singleton modules registered here: Forgets the current singleton instance.")]
        private void __Inspector_SingletonForgetButton()
        {
            Debug.Log($"{TypeName}: (For all registered singleton modules) Forgetting the singleton instance.");
            __Inspector_ToolDynamicInvoke(modules, typeof(SingletonModule<>), "__Inspector_ForgetButton");
        }

        private static void __Inspector_ToolDynamicInvoke(IEnumerable<Module> modules, Type targetOpenGenericType, string methodName)
        {
            var emptyObjectArray = Array.Empty<object>();
            foreach (var module in modules)
            {
                try
                {
                    if (module.GetType().TryGetClosedGenericAncestorType(targetOpenGenericType, out var foundClosedGenericType))
                    {
                        var methodInfo = foundClosedGenericType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
                        Assert.IsNotNull(methodInfo, Constants.Errors.InternalFramework);

                        methodInfo.Invoke(module, emptyObjectArray);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        // ReSharper restore InconsistentNaming
#endif // UNITY_EDITOR
    }
}
