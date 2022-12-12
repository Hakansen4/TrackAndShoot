using EasyClap.Seneca.Common.PlayerPrefsUtils;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;

namespace EasyClap.Seneca.Common.Editor.CustomInspectors.PlayerPrefsEntries
{
    /// <summary>
    /// Property drawer for <see cref="PlayerPrefsEntryBool"/>.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Itself)]
    internal class PlayerPrefsEntryBoolDrawer : PlayerPrefsEntryDrawerBase<PlayerPrefsEntryBool, bool>
    {
        protected override bool DrawValue(string name, bool value) => EditorGUILayout.Toggle(name, value);
    }
}
