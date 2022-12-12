using System;
using System.Collections;
using EasyClap.Seneca.Common.Core;
using EasyClap.Seneca.Common.LevelManagement.Databases;
using EasyClap.Seneca.Common.LevelManagement.Levels;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyClap.Seneca.Common.LevelManagement.Loaders
{
    /// <summary>
    /// Level loader for the scene-based level setup. Levels are scenes.
    /// </summary>
    [PublicAPI]
    [CreateAssetMenu(menuName = MenuName, fileName = TypeName)]
    public class SceneLevelLoader : LevelLoader<SceneLevelLoader, SceneLevelDatabase, SceneLevel, Scene>
    {
        private const string TypeName = nameof(SceneLevelLoader);
        private const string MenuName = Core.Constants.EasyClap + "/" + TypeName;

        /// <summary>
        /// Determines in which mode this loader should load scenes.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>Select <see cref="LoadSceneMode.Additive"/> if you have a central scene that always stays loaded and of which level scenes get loaded alongside.</item>
        ///     <item>Select <see cref="LoadSceneMode.Single"/> if you are going for standalone level scenes.</item>
        /// </list>
        /// </remarks>
        [field: SerializeField]
        public LoadSceneMode LevelSceneLoadMode { get; [UsedImplicitly] private set; } = LoadSceneMode.Additive;

        /// <inheritdoc/>
        [field: NonSerialized]
        public override Scene CurrentLevelRuntime { get; protected set; }

        /// <inheritdoc/>
        public override SceneLevelDatabase LevelDatabase => SceneLevelDatabase.Instance;

        protected override bool NeedUnloadBeforeLoad => LevelSceneLoadMode switch
        {
            LoadSceneMode.Single => false, // we don't need an unload on single mode, unity unloads the current scene on load
            LoadSceneMode.Additive => true, // we need an unload on additive mode, unity won't touch other scenes on load
            _ => throw new ArgumentOutOfRangeException()
        };

        protected override IEnumerator Unload(int levelIndexToLoad, SceneLevel levelInDatabaseToLoad)
        {
            Debug.Log($"{FormattedLoaderTypeName}: Unloading {CurrentLevelRuntime.path}");
            yield return SceneManager.UnloadSceneAsync(CurrentLevelRuntime, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

        protected override IEnumerator Load(int levelIndexToLoad, SceneLevel levelInDatabaseToLoad)
        {
            void LoadOperationOnCompleted(AsyncOperation obj)
            {
                CurrentLevelRuntime = SceneManager.GetSceneByPath(levelInDatabaseToLoad.SceneReference.ScenePath);
            }

            Debug.Log($"{FormattedLoaderTypeName}: Loading {levelInDatabaseToLoad.SceneReference.ScenePath}");
            var loadOp = SceneManager.LoadSceneAsync(levelInDatabaseToLoad.SceneReference.ScenePath, LevelSceneLoadMode);
            loadOp.completed += LoadOperationOnCompleted;
            yield return loadOp;
        }

        protected override IEnumerator Cleanup(int levelIndexToLoad, SceneLevel levelInDatabaseToLoad)
        {
            Debug.Log($"{FormattedLoaderTypeName}: Cleaning up");
            Resources.UnloadUnusedAssets(); // do not yield this, no need.
            yield break;
        }

        protected override bool IsRuntimeLevelValid(Scene runtimeLevel)
        {
            return runtimeLevel.IsValid();
        }
    }
}
