using System;
using System.Collections;
using EasyClap.Seneca.Common.Core;
using EasyClap.Seneca.Common.LevelManagement.Databases;
using EasyClap.Seneca.Common.LevelManagement.Levels;
using EasyClap.Seneca.Common.Utility;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.LevelManagement.Loaders
{
    /// <summary>
    /// Level loader for the prefab-based level setup. Levels are prefabs.
    /// </summary>
    [PublicAPI]
    [CreateAssetMenu(menuName = MenuName, fileName = TypeName)]
    public class PrefabLevelLoader : LevelLoader<PrefabLevelLoader, PrefabLevelDatabase, PrefabLevel, GameObject>
    {
        private const string TypeName = nameof(PrefabLevelLoader);
        private const string MenuName = Core.Constants.EasyClap + "/" + TypeName;

        /// <inheritdoc/>
        [field: NonSerialized]
        public override GameObject CurrentLevelRuntime { get; protected set; }

        /// <inheritdoc/>
        public override PrefabLevelDatabase LevelDatabase => PrefabLevelDatabase.Instance;

        // prefabs always need to be manually unloaded.
        protected override bool NeedUnloadBeforeLoad => true;

        protected override IEnumerator Unload(int levelIndexToLoad, PrefabLevel levelInDatabaseToLoad)
        {
            Debug.Log($"{FormattedLoaderTypeName}: Unloading {CurrentLevelRuntime.name}");
            Destroy(CurrentLevelRuntime);
            yield break;
        }

        protected override IEnumerator Load(int levelIndexToLoad, PrefabLevel levelInDatabaseToLoad)
        {
            Debug.Log($"{FormattedLoaderTypeName}: Loading {levelInDatabaseToLoad.Prefab.name}");
            CurrentLevelRuntime = Instantiate(levelInDatabaseToLoad.Prefab);
            CurrentLevelRuntime.transform.ResetLocalPosRot();
            yield break;
        }

        protected override IEnumerator Cleanup(int levelIndexToLoad, PrefabLevel levelInDatabaseToLoad)
        {
            Debug.Log($"{FormattedLoaderTypeName}: Cleaning up");
            Resources.UnloadUnusedAssets(); // do not yield this, no need.
            yield break;
        }

        protected override bool IsRuntimeLevelValid(GameObject runtimeLevel)
        {
            // implicit boolean operator on UnityEngine.Object
            return runtimeLevel;
        }
    }
}
