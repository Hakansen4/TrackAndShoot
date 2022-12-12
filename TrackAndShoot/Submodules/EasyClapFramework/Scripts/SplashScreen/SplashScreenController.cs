using System.Collections;
using DG.Tweening;
using EasyClap.Seneca.Common.Core;
using EasyClap.Seneca.Common.EventBus;
using EasyClap.Seneca.Common.SplashScreen.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyClap.Seneca.Common.SplashScreen
{
    internal sealed class SplashScreenController : MonoBehaviour
    {
        [SceneObjectsOnly]
        [SerializeField] private CanvasGroup logo;

        private static SplashScreenConfigModule Config => SplashScreenConfigModule.Instance;

        IEnumerator Start()
        {
            HandleLifetimeStage(SplashScreenLifetimeStage.BeforeFadeIn);

            // Logo fade in
            logo.alpha = 0f;
            yield return DOTween.To(() => logo.alpha, value => logo.alpha = value, 1f, Config.FadeInDuration)
                .SetEase(Config.FadeInEase)
                .WaitForCompletion();

            // Calculate minimum wait timing
            var processingStartTime = Time.realtimeSinceStartup;
            var timeToMoveOn = processingStartTime + Config.WaitDuration;

            HandleLifetimeStage(SplashScreenLifetimeStage.AfterFadeIn);

            // Maybe wait
            yield return new WaitUntil(() => Time.realtimeSinceStartup > timeToMoveOn);

            HandleLifetimeStage(SplashScreenLifetimeStage.BeforeFadeOut);

            // Logo fade out
            yield return DOTween.To(() => logo.alpha, value => logo.alpha = value, 0f, Config.FadeOutDuration)
                .SetEase(Config.FadeOutEase)
                .WaitForCompletion();

            HandleLifetimeStage(SplashScreenLifetimeStage.AfterFadeOut);

            // Scene loading
            switch (Config.SceneLoadBehaviour)
            {
                case SplashSceneLoadBehaviour.Disabled:
                {
                    Debug.Log($"{nameof(SplashScreenController)}: {nameof(Config.SceneLoadBehaviour)} is {Config.SceneLoadBehaviour}. Will not perform any scene load operations.");
                    break;
                }

                case SplashSceneLoadBehaviour.SceneWithBuildIndexOne:
                {
                    var scenePathToLoad = SceneUtility.GetScenePathByBuildIndex(1);
                    Debug.Log($"{nameof(SplashScreenController)}: {nameof(Config.SceneLoadBehaviour)} is {Config.SceneLoadBehaviour}. Will load scene at {scenePathToLoad}.");
                    SceneManager.LoadScene(scenePathToLoad);
                    break;
                }

                case SplashSceneLoadBehaviour.CustomScene:
                {
                    var scenePathToLoad = Config.SceneToLoad;
                    Debug.Log($"{nameof(SplashScreenController)}: {nameof(Config.SceneLoadBehaviour)} is {Config.SceneLoadBehaviour}. Will load scene at {scenePathToLoad}.");
                    SceneManager.LoadScene(scenePathToLoad);
                    break;
                }

                default:
                    throw SenecaCommonException.FromArgumentOutOfRange(nameof(Config.SceneLoadBehaviour), Config.SceneLoadBehaviour);
            }
        }

        private void HandleLifetimeStage(SplashScreenLifetimeStage lifetimeStage)
        {
            EventBus<SplashScreenLifetimeEvent>.Emit(this, new SplashScreenLifetimeEvent(lifetimeStage));

            var prefabsToInstantiate = Config.GetPrefabsToInstantiate(lifetimeStage);
            foreach (var prefab in prefabsToInstantiate)
            {
                Instantiate(prefab);
            }
        }
    }
}
