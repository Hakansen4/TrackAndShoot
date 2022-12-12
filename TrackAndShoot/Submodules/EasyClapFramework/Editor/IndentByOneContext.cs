using System;
using JetBrains.Annotations;
using Sirenix.Utilities.Editor;

namespace EasyClap.Seneca.Common.Editor
{
    /// <summary>
    /// A <c>using</c> context that indents the content by 1 compared to where it should normally appear.
    /// </summary>
    /// <example><code>
    /// using (new IndentByOneContext())
    /// {
    ///     // GUI code here will be indented by 1
    /// }
    /// </code></example>
    /// <seealso cref="SenecaEditorUtility.PushIndentByOne"/>
    [PublicAPI]
    public class IndentByOneContext : IDisposable
    {
        public IndentByOneContext()
        {
            SenecaEditorUtility.PushIndentByOne();
        }

        public void Dispose()
        {
            GUIHelper.PopIndentLevel();
        }
    }
}
