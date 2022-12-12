using EasyClap.Seneca.Common.PlayerPrefsUtils;
using JetBrains.Annotations;
using UnityEditor;

namespace EasyClap.Seneca.Common.Editor.CustomInspectors.PlayerPrefsEntries
{
    /// <summary>
    /// Property drawer for <see cref="PlayerPrefsEntryString"/>.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Itself)]
    internal class PlayerPrefsEntryStringDrawer : PlayerPrefsEntryDrawerBase<PlayerPrefsEntryString, string>
    {
        protected override string DrawValue(string name, string value) => EditorGUILayout.TextField(name, value);
    }
}
