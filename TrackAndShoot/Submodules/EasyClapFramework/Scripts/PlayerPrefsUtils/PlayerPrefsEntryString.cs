using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.PlayerPrefsUtils
{
    /// <summary>
    /// Wraps around a specific PlayerPrefs string entry for convenience and strong typing.
    /// </summary>
    /// <remarks><inheritdoc cref="PlayerPrefsEntryFloat"/></remarks>
    [HideReferenceObjectPicker, HideDuplicateReferenceBox]
    public sealed class PlayerPrefsEntryString : PlayerPrefsEntryBase<string>
    {
        /// <inheritdoc cref="PlayerPrefsEntryString"/>
        /// <param name="key">The key of this PlayerPrefs string entry.</param>
        [PublicAPI]
        public PlayerPrefsEntryString(string key) : base(key, PlayerPrefs.GetString, PlayerPrefs.SetString)
        {
        }
        
        public PlayerPrefsEntryString(string key, string defaultValue) : base(key, PlayerPrefs.GetString, PlayerPrefs.SetString, defaultValue)
        {
        }
    }
}
