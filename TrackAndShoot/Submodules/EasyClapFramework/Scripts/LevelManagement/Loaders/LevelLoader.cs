using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EasyClap.Seneca.Common.Core;
using EasyClap.Seneca.Common.EventBus;
using EasyClap.Seneca.Common.LevelManagement.Databases;
using EasyClap.Seneca.Common.LevelManagement.Events;
using EasyClap.Seneca.Common.LevelManagement.Levels;
using EasyClap.Seneca.Common.ModuleManagement;
using EasyClap.Seneca.Common.PlayerPrefsUtils;
using EasyClap.Seneca.Common.Utility;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EasyClap.Seneca.Common.LevelManagement.Loaders
{
    /// <summary>
    /// Base class for a level loader.
    /// </summary>
    /// <typeparam name="TLoader">The deriving type itself, ie. the loader itself.</typeparam>
    /// <typeparam name="TDatabase">Type of the database used by this loader.</typeparam>
    /// <typeparam name="TLevelInDatabase">Database type of the levels. Levels are serialized as this type in the database.</typeparam>
    /// <typeparam name="TLevelRuntime">Runtime type of the levels. This is what the levels become in the game after we load them.</typeparam>
    /// <example>
    /// <!-- TODO: Code example -->
    /// See also <see cref="SceneLevelLoader"/> for a real level loader example.
    /// </example>
    [PublicAPI]
    public abstract class LevelLoader<TLoader, TDatabase, TLevelInDatabase, TLevelRuntime> : SingletonModule<TLoader>, ILevelLoader
        where TLoader : LevelLoader<TLoader, TDatabase, TLevelInDatabase, TLevelRuntime>
        where TDatabase : LevelDatabase<TDatabase, TLevelInDatabase>
        where TLevelInDatabase : ILevel
    {
        // ReSharper disable InconsistentNaming
        private const string __InspectorGroupName_DevTools = "Level Loader Development Tools";
        // ReSharper restore InconsistentNaming

        protected static readonly string FormattedLoaderTypeName = Utils.GetFormattedName<TLoader>();
        private static readonly string FormattedBaseLogPrefix = $"{FormattedLoaderTypeName} (base)";
        
        /// <inheritdoc/>
        [field: SerializeField]
        public LevelLoadMethodAfterCompletion LevelLoadMethodAfterCompletion { get; [UsedImplicitly] private set; } =
            LevelLoadMethodAfterCompletion.Randomized;
        
        [ShowIf(nameof(LevelLoadMethodAfterCompletion), LevelLoadMethodAfterCompletion.Randomized)] 
        public int[] RandomIndexArray;
        
        /// <inheritdoc/>
        [field: SerializeField]
        public VirtualLevelIndexPersistence VirtualLevelIndexPersistence { get; [UsedImplicitly] private set; } =
            VirtualLevelIndexPersistence.Persistent;

        /// <summary>
        /// Level database used by this loader.
        /// </summary>
        public abstract TDatabase LevelDatabase { get; }

        /// <inheritdoc/>
        [field: NonSerialized]
        public bool IsLoading { get; private set; } = false;

        /// <inheritdoc/>
        public bool IsInRandomLevelMode => LevelLoadMethodAfterCompletion == LevelLoadMethodAfterCompletion.Randomized
                                           && DidCompleteAtLeastOnce.HasValue
                                           && DidCompleteAtLeastOnce.Value;

        /// <inheritdoc/>
        // Starts from -1 because we want first level to have index 0
        [field: NonSerialized]
        public int VirtualCurrentLevelIndex { get; private set; } = -1;

        /// <inheritdoc/>
        [field: NonSerialized]
        public int CurrentLevelIndex { get; private set; } = -1;

        /// <inheritdoc/>
        [field: NonSerialized]
        public bool HasFirstLevelOfTheSessionLoaded { get; private set; }

        /// <summary>
        /// Database representation of the level currently active.
        /// </summary>
        /// <remarks>This is the level in its database form, i.e. before it was loaded into the game.</remarks>
        public TLevelInDatabase CurrentLevelInDatabase => LevelDatabase.GetLevelWithIndex(CurrentLevelIndex);

        /// <summary>
        /// Runtime representation of the level currently active.
        /// </summary>
        /// <remarks>This the level in its runtime form, i.e. after it was loaded into the game.</remarks>
        public abstract TLevelRuntime CurrentLevelRuntime { get; protected set; }

        /// <summary>
        /// Return whether the loader needs an <see cref="Unload"/> call before a <see cref="Load"/> call.
        /// If false, <see cref="Unload"/> will not be called.
        /// </summary>
        protected abstract bool NeedUnloadBeforeLoad { get; }

        private static PlayerPrefsEntryInt LastSuccessfulLevelIndex => PlayerPrefsGateway.LevelManagement.LastSuccessfulLevelIndex;
        private static PlayerPrefsEntryBool DidCompleteAtLeastOnce => PlayerPrefsGateway.LevelManagement.DidCompleteAtLeastOnce;
        private static PlayerPrefsEntryInt LastSuccessfulVirtualLevelIndex => PlayerPrefsGateway.LevelManagement.LastSuccessfulVirtualLevelIndex;

        protected override void Init()
        {
            base.Init();
            LevelServiceLocator.Instance.RegisterActiveLoader(this);
            InitializeVirtualLevelIndex();
        }

        /// <inheritdoc/>
        public void SmartLoad()
        {
            var progressionManager = LevelServiceLocator.Instance.ActiveProgressionManager;

            if (false == HasFirstLevelOfTheSessionLoaded)
            {
                Debug.Log($"{FormattedBaseLogPrefix}.{nameof(SmartLoad)}: Calling {nameof(LoadFirstLevel)}." +
                          $"\nBecause the first level of the session is not loaded yet.");
                LoadFirstLevel();
            }
            else if (false == progressionManager.DidHandleCompleteForCurrentLevel)
            {
                throw new SenecaCommonException($"{FormattedBaseLogPrefix}.{nameof(SmartLoad)}: The first level of the session is already loaded, and the current level is not completed yet. Cannot decide what to do.\nWhen a level is presently loaded, you need to call {nameof(ProgressionManager.HandleLevelComplete)} on the active {nameof(ProgressionManager)} to declare the current level as completed before trying to load a new level.");
            }
            else if (progressionManager.WasLatestCompleteASuccess)
            {
                Debug.Log($"{FormattedBaseLogPrefix}.{nameof(SmartLoad)}: Calling {nameof(LoadNextLevel)}." +
                          $"\nBecause the first level of the session is already loaded before, and the current level was completed with a success.");
                LoadNextLevel();
            }
            else
            {
                Debug.Log($"{FormattedBaseLogPrefix}.{nameof(SmartLoad)}: Calling {nameof(ReloadCurrentLevel)}." +
                          $"\nBecause the first level of the session is already loaded before, and the current level was completed with a fail.");
                ReloadCurrentLevel();
            }
        }

        /// <inheritdoc/>
        [Button, FoldoutGroup(__InspectorGroupName_DevTools)]
        public void LoadFirstLevel()
        {
            if (HasFirstLevelOfTheSessionLoaded)
            {
                Debug.LogWarning($"{FormattedBaseLogPrefix}: {nameof(LoadFirstLevel)} is called, but first level has already loaded. Continuing anyway.");
            }

            HasFirstLevelOfTheSessionLoaded = true;

            var firstLevelIndex = IsInRandomLevelMode
                ? GetRandomLevelIndexExceptCurrent()
                : LastSuccessfulLevelIndex.HasValue
                    ? GetLinearNextLevelIndex(LastSuccessfulLevelIndex.Value)
                    : 0;

            LoadLevelWithIndex(firstLevelIndex, false);
        }

        /// <inheritdoc/>
        [Button, FoldoutGroup(__InspectorGroupName_DevTools)]
        public void ReloadCurrentLevel()
        {
            LoadLevelWithIndex(CurrentLevelIndex, true);
        }

        /// <inheritdoc/>
        [Button, FoldoutGroup(__InspectorGroupName_DevTools)]
        public void LoadNextLevel()
        {
            int nextLevelIndex;
            if (IsInRandomLevelMode && LevelDatabase.LevelCount > 1)
            {
                nextLevelIndex = GetRandomLevelIndexExceptCurrent();
            }
            else
            {
                nextLevelIndex = GetLinearNextLevelIndex(CurrentLevelIndex);
            }

            LoadLevelWithIndex(nextLevelIndex, false);
        }

        /// <inheritdoc/>
        [Button, FoldoutGroup(__InspectorGroupName_DevTools)]
        public void LoadLevelWithIndex(int levelIndex, bool? isReload = null)
        {
            if (IsLoading)
            {
                Debug.LogError($"{FormattedBaseLogPrefix}: Already loading a level! Ignoring load request.");
                return;
            }

            if (levelIndex < 0)
            {
                Debug.LogWarning($"{FormattedBaseLogPrefix}: Level index negative. Given: {levelIndex}. Assuming 0.");
                levelIndex = 0;
            }
            else if (levelIndex > LevelDatabase.LevelCount - 1)
            {
                Debug.LogWarning($"{FormattedBaseLogPrefix}: Level index too high. Given: {levelIndex} LevelCount: {LevelDatabase.LevelCount}. Applying modulo operator.");
                levelIndex %= LevelDatabase.LevelCount;
            }

            // if the caller did not specify if this is a reload or not, infer that information from the level indices
            isReload ??= CurrentLevelIndex == levelIndex;

            GlobalMonoBehaviourSurrogate.Instance.StartCoroutine(LoadRoutine(levelIndex, isReload.Value));
        }

        private IEnumerator LoadRoutine(int levelIndexToLoad, bool isReload)
        {
            IsLoading = true;
            var previousLevelRuntime = CurrentLevelRuntime;
            var previousLevelIndex = CurrentLevelIndex;
            var levelInDatabaseToLoad = LevelDatabase.Levels[levelIndexToLoad];
            var virtualLevelIndexBeforeLoad = VirtualCurrentLevelIndex;
            var virtualLevelIndexAfterLoad = isReload ? VirtualCurrentLevelIndex : VirtualCurrentLevelIndex + 1;
            EventBus<LevelLoadStartedEvent>.Emit(this, new LevelLoadStartedEvent(previousLevelIndex, levelIndexToLoad, isReload, virtualLevelIndexBeforeLoad, virtualLevelIndexAfterLoad));

            yield return Prepare(levelIndexToLoad, levelInDatabaseToLoad);

            if (!NeedUnloadBeforeLoad)
            {
                Debug.Log($"{FormattedBaseLogPrefix}: Skipping unload because the loader specified it doesn't need an unload call.");
            }
            else if (previousLevelIndex < 0)
            {
                Debug.Log($"{FormattedBaseLogPrefix}: Skipping unload because {nameof(previousLevelIndex)} is {previousLevelIndex}.");
            }
            else if (false == IsRuntimeLevelValid(previousLevelRuntime))
            {
                Debug.LogError($"{FormattedBaseLogPrefix}: Skipping unload because {nameof(previousLevelIndex)} is {previousLevelIndex}, but {nameof(previousLevelRuntime)} is invalid!");
            }
            else
            {
                yield return Unload(levelIndexToLoad, levelInDatabaseToLoad);
            }

            yield return Load(levelIndexToLoad, levelInDatabaseToLoad);
            CurrentLevelIndex = levelIndexToLoad;
            VirtualCurrentLevelIndex = virtualLevelIndexAfterLoad;
            
            yield return Cleanup(levelIndexToLoad, levelInDatabaseToLoad);

            EventBus<LevelLoadCompletedEvent>.Emit(this, new LevelLoadCompletedEvent(previousLevelIndex, levelIndexToLoad, isReload, virtualLevelIndexBeforeLoad, virtualLevelIndexAfterLoad));
            IsLoading = false;
        }

        private int GetLinearNextLevelIndex(int currLevelIndex)
        {
            do
            {
                currLevelIndex = (currLevelIndex + 1) % LevelDatabase.LevelCount;
            } while (DidCompleteAtLeastOnce.Value && LevelDatabase.GetLevelWithIndex(currLevelIndex).LevelType == LevelType.Tutorial);

            return currLevelIndex;
        }

        private int GetRandomLevelIndexExceptCurrent()
        {
            var i = VirtualCurrentLevelIndex;
            int li;
            var limit = 500;
            do
            {
                if (limit-- == 0)
                {
                    throw new Exception("Possible infinite loop!.");
                }
                li = RandomIndexArray[i++ % RandomIndexArray.Length] % LevelDatabase.LevelCount;
            } while (li == CurrentLevelIndex || LevelDatabase.GetLevelWithIndex(li).LevelType == LevelType.Tutorial);

            return li;
        }

        private void InitializeVirtualLevelIndex()
        {
            if (VirtualLevelIndexPersistence != VirtualLevelIndexPersistence.Persistent)
            {
                Debug.Log($"{FormattedBaseLogPrefix}: Skipping virtual level index recovery because {nameof(VirtualLevelIndexPersistence)} is {VirtualLevelIndexPersistence}.");
                VirtualCurrentLevelIndex = -1;
            }
            else if (!LastSuccessfulVirtualLevelIndex.HasValue)
            {
                Debug.Log($"{FormattedBaseLogPrefix}: Skipping virtual level index recovery because {nameof(VirtualLevelIndexPersistence)} is {VirtualLevelIndexPersistence}, but {nameof(LastSuccessfulVirtualLevelIndex)} does not have a value.");
                VirtualCurrentLevelIndex = -1;
            }
            else
            {
                Debug.Log($"{FormattedBaseLogPrefix}: Recovering virtual level index because {nameof(VirtualLevelIndexPersistence)} is {VirtualLevelIndexPersistence} and found {nameof(LastSuccessfulVirtualLevelIndex)} of {LastSuccessfulVirtualLevelIndex.Value}.");
                VirtualCurrentLevelIndex = LastSuccessfulVirtualLevelIndex.Value;
            }
        }

        /// <summary>
        /// Override to implement a custom preparation behaviour that will be called at the beginning of any unload-load routine.
        /// </summary>
        /// <param name="levelIndexToLoad">Level index of the level we are intending to load.</param>
        /// <param name="levelInDatabaseToLoad">Database representation of the level we are intending to load.</param>
        /// <remarks>
        /// <para>This is a coroutine. You can use yield instructions.</para>
        /// <para>Level index is NOT the same as scene index.</para>
        /// </remarks>
        protected virtual IEnumerator Prepare(int levelIndexToLoad, TLevelInDatabase levelInDatabaseToLoad)
        {
            yield break;
        }

        /// <summary>
        /// Implement the unload logic for the current level by overriding this.
        /// </summary>
        /// <inheritdoc cref="Prepare"/>
        protected virtual IEnumerator Unload(int levelIndexToLoad, TLevelInDatabase levelInDatabaseToLoad)
        {
            yield break;
        }

        /// <summary>
        /// Implement the loading logic for the level with given index by overriding this.
        /// Don't forget to set <see cref="CurrentLevelRuntime"/>.
        /// </summary>
        /// <inheritdoc cref="Prepare"/>
        protected virtual IEnumerator Load(int levelIndexToLoad, TLevelInDatabase levelInDatabaseToLoad)
        {
            yield break;
        }

        /// <summary>
        /// Implement a custom cleanup logic after load and unload by overriding this.
        /// </summary>
        /// <inheritdoc cref="Prepare"/>
        protected virtual IEnumerator Cleanup(int levelIndexToLoad, TLevelInDatabase levelInDatabaseToLoad)
        {
            yield break;
        }

        /// <summary>
        /// Return whether the given <paramref name="runtimeLevel"/> is valid.
        /// </summary>
        /// <remarks>
        /// For example, for scenes we do <see cref="UnityEngine.SceneManagement.Scene.IsValid"/>.
        /// For prefabs we use the implicit boolean operator to check if the object is still alive and well.
        /// If your object can never get corrupted, you can simply shortcut to <c>true</c>.
        /// </remarks>
        protected abstract bool IsRuntimeLevelValid(TLevelRuntime runtimeLevel);
        
#if UNITY_EDITOR
        [ShowIf(nameof(LevelLoadMethodAfterCompletion), LevelLoadMethodAfterCompletion.Randomized)]
        [Button]
        private void ExportRandomIndexArray(int length = 1000, string assetPath = @"Submodules/EasyClapFramework/Scripts/LevelManagement/Random_Index_Array.txt")
        {
            try
            {
                var filePath = @$"{Application.dataPath}/{assetPath}";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (StreamWriter sw = File.CreateText(filePath))
                {
                    var last = int.MinValue;
                    for (int i = 0; i < length; i++)
                    {
                        var n = Random.Range(0, Int32.MaxValue);
                        var limit = 100;
                        while (n == last)
                        {
                            if (limit-- == 0)
                            {
                                throw new Exception("Possible infinite loop!");
                            }

                            n = Random.Range(0, Int32.MaxValue);
                        }

                        sw.WriteLine(n.ToString());
                        last = n;
                    }
                }

                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        [ShowIf(nameof(LevelLoadMethodAfterCompletion), LevelLoadMethodAfterCompletion.Randomized)]
        [Button]
        private void ImportRandomIndexArray(string assetPath = @"Submodules/EasyClapFramework/Scripts/LevelManagement/Random_Index_Array.txt")
        {
            try
            {
                var filePath = @$"{Application.dataPath}/{assetPath}";

                using (StreamReader sr = File.OpenText(filePath))
                {
                    var l = new List<int>();
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (String.IsNullOrEmpty(s))
                        {
                            continue;
                        }

                        if (Int32.TryParse(s, out int i))
                        {
                            l.Add(i);
                        }
                    }

                    RandomIndexArray = l.ToArray();
                    EditorUtility.SetDirty(this);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
#endif
    }
}
