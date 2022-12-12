using JetBrains.Annotations;
using Sirenix.OdinInspector;

namespace EasyClap.Seneca.Common.ModuleManagement
{
    /// <summary>
    /// Base class for Module implementations. You probably don't want to inherit from this directly.
    /// </summary>
    [PublicAPI]
    public abstract class Module : SerializedScriptableObject
    {
        /// <summary>
        /// Don't forget to call <see cref="Init"/> if you override.
        /// </summary>
        /// <seealso cref="BootstrapSecondPass"/>
        internal virtual void Bootstrap()
        {
            Init();
        }

        /// <summary>
        /// Don't forget to call <see cref="InitSecondPass"/> if you override.
        /// </summary>
        /// <seealso cref="Bootstrap"/>
        internal virtual void BootstrapSecondPass()
        {
            InitSecondPass();
        }

        /// <summary>
        /// Override this method if you need to perform any custom runtime initialization.
        /// </summary>
        /// <remarks>
        /// The documentation for <c>ScriptableObject.Awake()</c> is wrong. It is NOT called in runtime, it is called
        /// at some stage in editor-time. This method is the approximate equivalent of the behaviour of
        /// <c>MonoBehaviour.Awake()</c>, in other words: this method is what <c>ScriptableObject.Awake()</c> should
        /// have been according to the docs. See more info:
        /// <list type="number">
        ///     <item>https://forum.unity.com/threads/solved-but-unhappy-scriptableobject-awake-never-execute.488468</item>
        ///     <item>https://docs.unity3d.com/ScriptReference/ScriptableObject.Awake.html</item>
        /// </list>
        /// </remarks>
        /// <seealso cref="InitSecondPass"/>
        protected virtual void Init() {}

        /// <summary>
        /// This is a second pass for <see cref="Init"/>.
        /// All <see cref="Init"/> methods will be invoked before the second pass.
        /// </summary>
        protected virtual void InitSecondPass() {}
    }
}
