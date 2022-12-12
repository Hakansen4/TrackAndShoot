using Eflatun.CodePatterns;
using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.Utility
{
    /// <summary>
    /// A no-op MonoBehaviour singleton that stays alive within the lifetime of a scene.
    /// </summary>
    /// <remarks>
    /// You can use this class as a coroutine host.
    /// </remarks>
    /// <seealso cref="GlobalMonoBehaviourSurrogate"/>
    [PublicAPI]
    public class SceneMonoBehaviourSurrogate: SceneSingleton<SceneMonoBehaviourSurrogate>
    {
    }
}
