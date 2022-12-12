using System;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.PlayerPrefsUtils
{
    /// <summary>
    /// Wraps around a specific PlayerPrefs entry for convenience and strong typing.
    /// Do not use this base class directly, use the type-specialized classes instead.
    /// </summary>
    /// <remarks>
    /// If you are creating a custom derived type, also consider creating a custom drawer for it. Take a look at
    /// <c>PlayerPrefsEntryDrawerBase</c> in the framework's Editor assembly, which is a base class for PlayerPrefs
    /// entry drawers.
    /// </remarks>
    public abstract class PlayerPrefsEntryBase<TValue>
    {
        private readonly PlayerPrefsGetter<TValue> _getter;
        private readonly PlayerPrefsSetter<TValue> _setter;

        /// <summary>
        /// The key of this PlayerPrefs entry.
        /// </summary>
        [PublicAPI]
        public string Key { get; }

        /// <summary>
        /// Does this PlayerPrefs entry have a value?
        /// </summary>
        [PublicAPI]
        public bool HasValue => PlayerPrefs.HasKey(Key);

        /// <summary>
        /// The value of this PlayerPrefs entry.
        /// </summary>
        [PublicAPI]
        public TValue Value
        {
            get => _getter.Invoke(Key, _defaultValue);
            set
            {
                var oldValue = Value;
                _setter.Invoke(Key, value);
                OnValueSet?.Invoke(oldValue, value);
            }
        }

        public delegate void OnValueSetDelegate(TValue oldValue, TValue newValue);
        public OnValueSetDelegate OnValueSet;
        private readonly TValue _defaultValue;

        /// <inheritdoc cref="PlayerPrefsEntryBase{TValue}"/>
        /// <param name="key">The key for this PlayerPrefs entry.</param>
        /// <param name="getter">The function to use for getting the value of this PlayerPrefs entry.</param>
        /// <param name="setter">The function to use for setting the value of this PlayerPrefs entry.</param>
        private protected PlayerPrefsEntryBase(string key, PlayerPrefsGetter<TValue> getter, PlayerPrefsSetter<TValue> setter, TValue defaultValue = default(TValue))
        {
            Key = key;
            _getter = getter;
            _setter = setter;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Deletes the value of this PlayerPrefs entry.
        /// </summary>
        [PublicAPI]
        public void Delete()
        {
            PlayerPrefs.DeleteKey(Key);
        }
    }
}
