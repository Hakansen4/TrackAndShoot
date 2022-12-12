using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.PlayerPrefsUtils
{
    /// <summary>
    /// Wraps around a specific PlayerPrefs float entry for convenience and strong typing.
    /// </summary>
    /// <remarks>
    /// PlayerPrefs entries are not serializable, but they have custom drawers. Decorate a declaration with
    /// <c>[ShowInInspector, NonSerialized]</c> attributes in order to show it on the inspector.
    /// </remarks>
    [HideReferenceObjectPicker, HideDuplicateReferenceBox]
    public sealed class PlayerPrefsEntryFloat : PlayerPrefsEntryBase<float>
    {
        /// <inheritdoc cref="PlayerPrefsEntryFloat"/>
        /// <param name="key">The key of this PlayerPrefs float entry.</param>
        [PublicAPI]
        public PlayerPrefsEntryFloat(string key) : base(key, PlayerPrefs.GetFloat, PlayerPrefs.SetFloat)
        {
        }
        
        public PlayerPrefsEntryFloat(string key, float defaultValue) : base(key, PlayerPrefs.GetFloat, PlayerPrefs.SetFloat, defaultValue)
        {
        }
    }
}
