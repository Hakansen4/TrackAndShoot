using JetBrains.Annotations;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace EasyClap.Seneca.Common.Editor.CustomInspectors
{
    /// <summary>
    /// Base class for a property drawer that, when derived from, wraps the property in a foldout.
    /// </summary>
    /// <typeparam name="T">Type of the property we are making a drawer for.</typeparam>
    /// <example>
    /// This is how you would use this base class to make a property drawer for a type named <c>Foo</c>:
    /// <code>
    /// class FooDrawer : FoldoutDrawer&lt;Foo&gt;
    /// {
    ///     // You can leave the class body empty. Base class handles everything.
    ///     // However, you also have a couple of customization options:
    ///
    ///     protected override NextDrawerOrder GetNextDrawerOrder()
    ///     {
    ///         // Override to customize when the next drawer should be called. Defaults to After.
    ///     }
    ///
    ///     protected override GUIContent GetFoldoutTitle(GUIContent label)
    ///     {
    ///         // Override to customize the foldout title. Defaults to label.
    ///     }
    ///
    ///     protected override void DrawCustomFoldoutContent()
    ///     {
    ///         // Override to draw custom foldout content. Defaults to nothing.
    ///     }
    /// }
    /// </code>
    /// </example>
    [PublicAPI]
    public abstract class FoldoutDrawer<T> : OdinValueDrawer<T>
    {
        public bool IsFoldoutExpanded { get; private set; }

        /// <summary>
        /// DO NOT CALL THIS. This is for <see cref="OdinValueDrawer{T}"/> to call.
        /// </summary>
        protected sealed override void DrawPropertyLayout(GUIContent label)
        {
            label ??= GUIContent.none;

            SirenixEditorGUI.BeginBox();

            SirenixEditorGUI.BeginBoxHeader();
            DrawCustomHeaderContentBeforeTitle();
            IsFoldoutExpanded = SirenixEditorGUI.Foldout(IsFoldoutExpanded, GetFoldoutTitle(label));
            DrawCustomHeaderContentAfterTitle();
            SirenixEditorGUI.EndBoxHeader();

            if (IsFoldoutExpanded)
            {
                var nextDrawerOrder = GetNextDrawerOrder();

                if (nextDrawerOrder == NextDrawerOrder.Before)
                {
                    CallNextDrawer(null);
                }

                DrawCustomFoldoutContent();

                if (nextDrawerOrder == NextDrawerOrder.After)
                {
                    CallNextDrawer(null);
                }
            }

            SirenixEditorGUI.EndBox();
        }

        /// <summary>
        /// Override to draw extra content to the right of the box header.
        /// </summary>
        protected virtual void DrawCustomHeaderContentBeforeTitle()
        {
        }

        /// <summary>
        /// Override to draw extra content to the left of the box header.
        /// </summary>
        protected virtual void DrawCustomHeaderContentAfterTitle()
        {
        }

        /// <summary>
        /// Override to customize when the next drawer should be called. Defaults to <see cref="NextDrawerOrder.After"/>.
        /// </summary>
        /// <seealso cref="OdinValueDrawer{T}.CallNextDrawer(GUIContent)"/>
        protected virtual NextDrawerOrder GetNextDrawerOrder()
        {
            return NextDrawerOrder.After;
        }

        /// <summary>
        /// Override to customize the foldout title. Defaults to <paramref name="label"/>.
        /// </summary>
        /// <param name="label">The raw label supplied by <see cref="OdinValueDrawer{T}"/>.</param>
        protected virtual GUIContent GetFoldoutTitle(GUIContent label)
        {
            return label;
        }

        /// <summary>
        /// Override to draw custom foldout content. Defaults to nothing.
        /// </summary>
        protected virtual void DrawCustomFoldoutContent()
        {
        }
    }
}
