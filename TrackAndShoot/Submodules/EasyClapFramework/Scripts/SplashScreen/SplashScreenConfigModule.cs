using System.Linq;
using DG.Tweening;
using EasyClap.Seneca.Common.Core;
using EasyClap.Seneca.Common.ModuleManagement;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using DG.DemiEditor;
using UnityEditor;
#endif // UNITY_EDITOR

namespace EasyClap.Seneca.Common.SplashScreen
{
    [CreateAssetMenu(fileName = TypeName, menuName = MenuName)]
    internal sealed class SplashScreenConfigModule : SingletonModule<SplashScreenConfigModule>
    {
        // ReSharper disable InconsistentNaming
        private const string __Inspector_GroupName_Config = "Config";
        private const string __Inspector_GroupName_Actions = "Actions";
        private const string __Inspector_GroupName_SceneLoading = "Scene Loading";
        // ReSharper restore InconsistentNaming

        private const string TypeName = nameof(SplashScreenConfigModule);
        private const string MenuName = Core.Constants.EasyClap + "/" + TypeName;

        [field: TitleGroup(__Inspector_GroupName_Config)]
        [field: SerializeField, Min(0f)]
        internal float FadeInDuration { get; [UsedImplicitly] private set; } = 0.5f;

        [field: TitleGroup(__Inspector_GroupName_Config)]
        [field: SerializeField, Min(0f)]
        internal float WaitDuration { get; [UsedImplicitly] private set; } = 2f;

        [field: TitleGroup(__Inspector_GroupName_Config)]
        [field: SerializeField, Min(0f)]
        internal float FadeOutDuration { get; [UsedImplicitly] private set; } = 0.5f;

        [field: TitleGroup(__Inspector_GroupName_Config)]
        [field: SerializeField]
        internal Ease FadeInEase { get; [UsedImplicitly] private set; } = Ease.Linear;

        [field: TitleGroup(__Inspector_GroupName_Config)]
        [field: SerializeField]
        internal Ease FadeOutEase { get; [UsedImplicitly] private set; } = Ease.Linear;

        [field: TitleGroup(__Inspector_GroupName_Actions)]
        [field: SerializeField, AssetSelector, AssetsOnly]
        [field: InfoBox("Splash screen has just loaded. Logo will start to fade-in." +
                        "\nAnything done here will delay the logo fade-in. You can perform cheap initializations here, but nothing too expensive.")]
        internal GameObject[] PrefabsToInstantiateBeforeFadeIn { get; [UsedImplicitly] private set; }

        [field: TitleGroup(__Inspector_GroupName_Actions), Space]
        [field: SerializeField, AssetSelector, AssetsOnly]
        [field: InfoBox("Logo faded-in. Waiting." +
                        "\nNothing done here will cause a delay as long as it completes within the wait time limit. You can do expensive operations here, wait time will account for it.")]
        internal GameObject[] PrefabsToInstantiateAfterFadeIn { get; [UsedImplicitly] private set; }

        [field: TitleGroup(__Inspector_GroupName_Actions), Space]
        [field: SerializeField, AssetSelector, AssetsOnly]
        [field: InfoBox("Wait time is over. Will fade-out the logo." +
                        "\nAnything done here will delay the logo fade-out. Try not to do anything here at all.")]
        internal GameObject[] PrefabsToInstantiateBeforeFadeOut { get; [UsedImplicitly] private set; }

        [field: TitleGroup(__Inspector_GroupName_Actions), Space]
        [field: SerializeField, AssetSelector, AssetsOnly]
        [field: InfoBox("Logo faded-out completely. Will maybe load the next scene." +
                        "\nAnything done here will delay the next scene load. You can perform a custom scene load here.")]
        internal GameObject[] PrefabsToInstantiateAfterFadeOut { get; [UsedImplicitly] private set; }

        [field: TitleGroup(__Inspector_GroupName_SceneLoading)]
        [field: SerializeField]
        [field: InfoBox("This section determines which scene should be loaded after splash fade-out." +
                        "\nDisabled: Do not perform any scene load operations in the splash screen." +
                        "\nScene With Build Index One: Load the scene that has the build index 1." +
                        "\nCustom Scene: Load a custom scene. Use " + nameof(SceneToLoad) + "field to select the scene.")]
        internal SplashSceneLoadBehaviour SceneLoadBehaviour { get; [UsedImplicitly] private set; } = SplashSceneLoadBehaviour.SceneWithBuildIndexOne;

        [field: TitleGroup(__Inspector_GroupName_SceneLoading)]
        [field: SerializeField, Required, ShowIf(nameof(SceneLoadBehaviour), SplashSceneLoadBehaviour.CustomScene)]
        internal SceneReference SceneToLoad { get; [UsedImplicitly] private set; }

        internal GameObject[] GetPrefabsToInstantiate(SplashScreenLifetimeStage lifetimeStage)
        {
            return lifetimeStage switch
            {
                SplashScreenLifetimeStage.BeforeFadeIn => PrefabsToInstantiateBeforeFadeIn,
                SplashScreenLifetimeStage.AfterFadeIn => PrefabsToInstantiateAfterFadeIn,
                SplashScreenLifetimeStage.BeforeFadeOut => PrefabsToInstantiateBeforeFadeOut,
                SplashScreenLifetimeStage.AfterFadeOut => PrefabsToInstantiateAfterFadeOut,
                _ => throw SenecaCommonException.FromArgumentOutOfRange(nameof(lifetimeStage), lifetimeStage)
            };
        }

#if UNITY_EDITOR
// ReSharper disable InconsistentNaming
        [ShowInInspector, CustomValueDrawer(nameof(__Inspector_GuiHookDrawer))]
        private string __Inspector_GuiHookDummy => "";
        private string __Inspector_GuiHookDrawer(string value)
        {
            var guiEnabledMemento = GUI.enabled;
            GUI.enabled = true;

            switch (SceneLoadBehaviour)
            {
                case SplashSceneLoadBehaviour.Disabled:
                {
                    EditorGUILayout.HelpBox($"{nameof(SceneLoadBehaviour).Nicify()} is {SceneLoadBehaviour.ToString().Nicify()}." +
                                            "\nSplash Screen will not perform any scene load operations.",
                            MessageType.Warning);
                    break;
                }

                case SplashSceneLoadBehaviour.SceneWithBuildIndexOne:
                {
                    var status = $"{nameof(SceneLoadBehaviour).Nicify()} is {SceneLoadBehaviour.ToString().Nicify()}, scene with build index 1 will be used:";
                    var enabledBuildSceneCount = EditorBuildSettings.scenes.Count(x => x.enabled);
                    if (enabledBuildSceneCount < 2)
                    {
                        EditorGUILayout.HelpBox($"{status}\nBuild settings has {enabledBuildSceneCount} enabled scene(s)! Must be at least 2.",
                            MessageType.Error);
                    }
                    else
                    {
                        var secondEnabledBuildScenePath = EditorBuildSettings.scenes.Where(x => x.enabled).Skip(1).First().path;
                        EditorGUILayout.HelpBox($"{status}\n{secondEnabledBuildScenePath}",
                            MessageType.Info);
                    }
                    break;
                }

                case SplashSceneLoadBehaviour.CustomScene:
                {
                    var status = $"{nameof(SceneLoadBehaviour).Nicify()} is {SceneLoadBehaviour.ToString().Nicify()}, {nameof(SceneToLoad).Nicify()} will be used:";
                    if (SceneUtility.GetBuildIndexByScenePath(SceneToLoad.ScenePath) < 0)
                    {
                        EditorGUILayout.HelpBox($"{status}\n{nameof(SceneToLoad).Nicify()} is invalid or not included in the build!",
                            MessageType.Error);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox($"{status}\n{SceneToLoad.ScenePath}",
                            MessageType.Info);
                    }
                    break;
                }

                default:
                    throw SenecaCommonException.FromArgumentOutOfRange(nameof(SceneLoadBehaviour), SceneLoadBehaviour);
            }

            GUI.enabled = guiEnabledMemento;
            return value;
        }
// ReSharper restore InconsistentNaming
#endif // UNITY_EDITOR
    }
}
