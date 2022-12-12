using System;
using JetBrains.Annotations;
using UnityEditor;

namespace EasyClap.Seneca.Common.Editor
{
    /// <summary>
    /// A <c>using</c> context that remembers <see cref="EditorGUIUtility.labelWidth">EditorGUIUtility.labelWidth</see> on enter <i>(construction)</i>, and restores it on exit <i>(disposal)</i>.
    /// </summary>
    /// <example><code>
    /// using (new LabelWidthMementoContext())
    /// {
    ///     // your code can set EditorGUIUtility.labelWidth here
    /// }
    /// // EditorGUIUtility.labelWidth here will be back to what it was before the using statement
    /// </code></example>
    [PublicAPI]
    public class LabelWidthMementoContext : IDisposable
    {
        private readonly float _labelWidth;

        public LabelWidthMementoContext()
        {
            _labelWidth = EditorGUIUtility.labelWidth;
        }

        public void Dispose()
        {
            EditorGUIUtility.labelWidth = _labelWidth;
        }
    }
}
