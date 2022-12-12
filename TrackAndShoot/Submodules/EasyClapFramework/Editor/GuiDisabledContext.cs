using System;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.Editor
{
    /// <summary>
    /// A <c>using</c> context that sets the <see cref="GUI.enabled">GUI.enabled</see> to <c>false</c> on enter <i>(construction)</i>, and restores it on exit <i>(disposal)</i> to what it was before.
    /// </summary>
    /// <example><code>
    /// using (new GuiDisabledContext())
    /// {
    ///     // GUI code here will be rendered as disabled
    /// }
    /// </code></example>
    [PublicAPI]
    public class GuiDisabledContext : IDisposable
    {
        private readonly bool _prevState;

        public GuiDisabledContext()
        {
            _prevState = GUI.enabled;
            GUI.enabled = false;
        }

        public void Dispose()
        {
            GUI.enabled = _prevState;
        }
    }
}
