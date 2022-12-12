using JetBrains.Annotations;
using UnityEditor;

namespace EasyClap.Seneca.Common.Editor
{
    [PublicAPI]
    public static class Extensions
    {
        /// <summary>
        /// Alias for <see cref="ObjectNames.NicifyVariableName"/>.
        /// </summary>
        public static string Nicify(this string name)
        {
            return ObjectNames.NicifyVariableName(name);
        }
    }
}
