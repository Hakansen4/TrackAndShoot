using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.PlayerPrefsUtils
{
    /// <summary>
    /// Wraps around a specific PlayerPrefs boolean entry for convenience and strong typing.
    /// </summary>
    /// <remarks>
    /// PlayerPrefs doesn't have support for booleans. We are wrapping around an integer internally.
    /// <para/>
    /// <inheritdoc cref="PlayerPrefsEntryFloat"/>
    /// </remarks>
    [HideReferenceObjectPicker, HideDuplicateReferenceBox]
    public sealed class PlayerPrefsEntryBool : PlayerPrefsEntryBase<bool>
    {
        /// <inheritdoc cref="PlayerPrefsEntryBool"/>
        /// <param name="key">The key of this PlayerPrefs boolean entry.</param>
        [PublicAPI]
        public PlayerPrefsEntryBool(string key) : base(key, GetBool, SetBool)
        {
        }
        
        public PlayerPrefsEntryBool(string key, bool defaultValue) : base(key, GetBool, SetBool, defaultValue)
        {
        }

        private static bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;
        }

        private static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
    }
}
