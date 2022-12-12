using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.PlayerPrefsUtils
{
    /// <summary>
    /// Wraps around a specific PlayerPrefs integer entry for convenience and strong typing.
    /// </summary>
    /// <remarks><inheritdoc cref="PlayerPrefsEntryFloat"/></remarks>
    [HideReferenceObjectPicker, HideDuplicateReferenceBox]
    public sealed class PlayerPrefsEntryInt : PlayerPrefsEntryBase<int>
    {
        /// <inheritdoc cref="PlayerPrefsEntryInt"/>
        /// <param name="key">The key of this PlayerPrefs integer entry.</param>
        [PublicAPI]
        public PlayerPrefsEntryInt(string key) : base(key, PlayerPrefs.GetInt, PlayerPrefs.SetInt)
        {
        }
        
        public PlayerPrefsEntryInt(string key, int defaultValue) : base(key, PlayerPrefs.GetInt, PlayerPrefs.SetInt, defaultValue)
        {
        }
    }
}
