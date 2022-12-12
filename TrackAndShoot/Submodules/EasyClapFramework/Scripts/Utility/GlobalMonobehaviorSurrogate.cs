using Eflatun.CodePatterns;
using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.Utility
{
    /// <summary>
    /// A no-op MonoBehaviour singleton that stays alive for the whole application.
    /// </summary>
    /// <remarks>
    /// You can use this class as a coroutine host.
    /// </remarks>
    /// <seealso cref="SceneMonoBehaviourSurrogate"/>
    [PublicAPI]
    public class GlobalMonoBehaviourSurrogate: GlobalSingleton<GlobalMonoBehaviourSurrogate>
    {
    }
}
