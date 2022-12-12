using System.Collections.Generic;
using EasyClap.Seneca.Common.ModuleManagement;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace EasyClap.Seneca.Common.Configuration
{
    [CreateAssetMenu(fileName = TypeName, menuName = MenuName)]
    public sealed class StartupConfigurator : SingletonModule<StartupConfigurator>
    {
        private const string TypeName = nameof(StartupConfigurator);
        private const string MenuName = Core.Constants.EasyClap + "/" + TypeName;

        [SerializeField, Required, AssetsOnly, AssetSelector]
        private StartupConfigurationPreset editorPreset;

        [SerializeField, Required, AssetsOnly, AssetSelector]
        private StartupConfigurationPreset fallbackPreset;

        [OdinSerialize, Required, AssetsOnly]
        private Dictionary<RuntimePlatform, StartupConfigurationPreset> platformPresets;

        protected override void Init()
        {
            var preset = ChoosePreset();
            ApplyPreset(preset);
        }

        private StartupConfigurationPreset ChoosePreset()
        {
            if (platformPresets.TryGetValue(Application.platform, out var preset))
            {
                Debug.Log($"{TypeName}: using platform preset for {Application.platform}");
                return preset;
            }

            if (Application.isEditor && editorPreset)
            {
                Debug.Log($"{TypeName}: using {nameof(editorPreset)}");
                return editorPreset;
            }

            Debug.LogWarning($"{TypeName}: using {nameof(fallbackPreset)}");
            return fallbackPreset;
        }

        private void ApplyPreset(StartupConfigurationPreset config)
        {
            Application.targetFrameRate = config.TargetFrameRate;
        }
    }
}
