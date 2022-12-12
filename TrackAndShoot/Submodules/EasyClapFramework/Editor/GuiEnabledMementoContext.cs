using System;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.Editor
{
    /// <summary>
    /// A <c>using</c> context that remembers <see cref="GUI.enabled">GUI.enabled</see> on enter <i>(construction)</i>, and restores it on exit <i>(disposal)</i>.
    /// </summary>
    /// <example><code>
    /// using (new GuiEnabledMementoContext())
    /// {
    ///     // your code can set GUI.enabled here
    /// }
    /// // GUI.enabled here will be back to what it was before the using statement
    /// </code></example>
    [PublicAPI]
    public class GuiEnabledMementoContext : IDisposable
    {
        private readonly bool _prevState;

        public GuiEnabledMementoContext()
        {
            _prevState = GUI.enabled;
        }

        public void Dispose()
        {
            GUI.enabled = _prevState;
        }
    }
}
