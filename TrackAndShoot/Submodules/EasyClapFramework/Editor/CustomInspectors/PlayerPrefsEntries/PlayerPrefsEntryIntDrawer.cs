using EasyClap.Seneca.Common.PlayerPrefsUtils;
using JetBrains.Annotations;
using UnityEditor;

namespace EasyClap.Seneca.Common.Editor.CustomInspectors.PlayerPrefsEntries
{
    /// <summary>
    /// Property drawer for <see cref="PlayerPrefsEntryInt"/>.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Itself)]
    internal class PlayerPrefsEntryIntDrawer : PlayerPrefsEntryDrawerBase<PlayerPrefsEntryInt, int>
    {
        protected override int DrawValue(string name, int value) => EditorGUILayout.IntField(name, value);
    }
}
