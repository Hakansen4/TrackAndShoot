using JetBrains.Annotations;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace EasyClap.Seneca.Common.Editor.CustomInspectors
{
    /// <summary>
    /// Indicates where <see cref="OdinValueDrawer{T}.CallNextDrawer(GUIContent)"/> should be called in a custom drawer.
    /// </summary>
    [PublicAPI]
    public enum NextDrawerOrder
    {
        /// <summary>
        /// Do not invoke <see cref="OdinValueDrawer{T}.CallNextDrawer(GUIContent)"/>.
        /// </summary>
        Never,

        /// <summary>
        /// Invoke <see cref="OdinValueDrawer{T}.CallNextDrawer(GUIContent)"/> before our own drawer.
        /// </summary>
        Before,

        /// <summary>
        /// Invoke <see cref="OdinValueDrawer{T}.CallNextDrawer(GUIContent)"/> after our own drawer.
        /// </summary>
        After
    }
}
