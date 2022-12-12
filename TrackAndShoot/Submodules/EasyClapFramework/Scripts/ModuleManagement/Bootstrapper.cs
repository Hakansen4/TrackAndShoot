using EasyClap.Seneca.Common.Utility;
using UnityEngine;
using UnityEngine.Scripting;

namespace EasyClap.Seneca.Common.ModuleManagement
{
    /// <summary>
    /// Loads and initializes the single <see cref="ModuleRegistry"/> instance.
    /// </summary>
    internal static class Bootstrapper
    {
        [Preserve]
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            ModuleRegistry.Load().Bootstrap();
        }
    }
}
