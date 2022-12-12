using EasyClap.Seneca.Common.PlayerPrefsUtils;
using JetBrains.Annotations;
using UnityEditor;

namespace EasyClap.Seneca.Common.Editor.CustomInspectors.PlayerPrefsEntries
{
    /// <summary>
    /// Property drawer for <see cref="PlayerPrefsEntryFloat"/>.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Itself)]
    internal class PlayerPrefsEntryFloatDrawer : PlayerPrefsEntryDrawerBase<PlayerPrefsEntryFloat, float>
    {
        protected override float DrawValue(string name, float value) => EditorGUILayout.FloatField(name, value);
    }
}
