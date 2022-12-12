using System;
using JetBrains.Annotations;
using Sirenix.Utilities.Editor;

namespace EasyClap.Seneca.Common.Editor
{
    /// <summary>
    /// A <c>using</c> context that sets the indent level of its content to the given level.
    /// </summary>
    /// <example><code>
    /// using (new IndentContext(2))
    /// {
    ///     // GUI code here will have the indent level 2
    /// }
    /// </code></example>
    /// <seealso cref="GUIHelper.PushIndentLevel(int)"/>
    [PublicAPI]
    public class IndentContext : IDisposable
    {
        public IndentContext(int level)
        {
            GUIHelper.PushIndentLevel(level);
        }

        public void Dispose()
        {
            GUIHelper.PopIndentLevel();
        }
    }
}
