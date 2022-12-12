using EasyClap.Seneca.Common.Core;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.Configuration
{
    [CreateAssetMenu(fileName = TypeName, menuName = MenuName)]
    public sealed class StartupConfigurationPreset : ScriptableObject
    {
        private const string TypeName = nameof(StartupConfigurationPreset);
        private const string MenuName = Constants.EasyClap + "/" + TypeName;

        [field: SerializeField, InfoBox("Recommended: 60 for Android, -1 for Editor")]
        public int TargetFrameRate { get; [UsedImplicitly] private set; }
    }
}
