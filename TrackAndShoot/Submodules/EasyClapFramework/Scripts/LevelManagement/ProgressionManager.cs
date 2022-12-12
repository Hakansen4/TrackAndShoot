using System;
using EasyClap.Seneca.Common.Core;
using EasyClap.Seneca.Common.ModuleManagement;
using EasyClap.Seneca.Common.EventBus;
using EasyClap.Seneca.Common.LevelManagement.Events;
using EasyClap.Seneca.Common.PlayerPrefsUtils;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.LevelManagement
{
    /// <summary>
    /// Handles everything related to level-to-level progression.
    /// </summary>
    /// <remarks>
    /// Does NOT have anything to do with in-level progressions.
    /// </remarks>
    [PublicAPI]
    [CreateAssetMenu(menuName = MenuName, fileName = TypeName)]
    public class ProgressionManager : SingletonModule<ProgressionManager>
    {
        // ReSharper disable InconsistentNaming
        private const string __InspectorGroupName_DevTools = "Progression Manager Development Tools";
        // ReSharper restore InconsistentNaming

        private const string TypeName = nameof(ProgressionManager);
        private const string MenuName = Core.Constants.EasyClap + "/" + TypeName;

        /// <summary>
        /// Returns whether we have any progression data saved locally for this user.
        /// </summary>
        public static bool HasAnyProgressionData => LastSuccessfulLevelIndex.HasValue || DidCompleteAtLeastOnce.HasValue;

        private static PlayerPrefsEntryInt LastSuccessfulLevelIndex => PlayerPrefsGateway.LevelManagement.LastSuccessfulLevelIndex;
        private static PlayerPrefsEntryBool DidCompleteAtLeastOnce => PlayerPrefsGateway.LevelManagement.DidCompleteAtLeastOnce;
        private static PlayerPrefsEntryInt LastSuccessfulVirtualLevelIndex => PlayerPrefsGateway.LevelManagement.LastSuccessfulVirtualLevelIndex;

        private static int CurrentLevelIndex => LevelServiceLocator.Instance.ActiveLoader.CurrentLevelIndex;
        private static int LevelCount => LevelServiceLocator.Instance.ActiveDatabase.LevelCount;
        private static int VirtualCurrentLevelIndex => LevelServiceLocator.Instance.ActiveLoader.VirtualCurrentLevelIndex;

        /// <summary>
        /// Did we already handle a complete event for the current level?
        /// </summary>
        [field: NonSerialized]
        public bool DidHandleCompleteForCurrentLevel { get; private set; } = false;

        /// <summary>
        /// Whether the latest meaningful call to <see cref="HandleLevelComplete"/> was for a success.
        /// </summary>
        /// <remarks>
        /// If <see cref="DidHandleCompleteForCurrentLevel"/> is true, then this value is about the current level.
        /// Otherwise, it is about the previous level.
        /// </remarks>
        [field: NonSerialized]
        public bool WasLatestCompleteASuccess { get; private set; }

        protected override void Init()
        {
            LevelServiceLocator.Instance.RegisterActiveProgressionManager(this);
        }

        // TODO: remove listener -- need support from ModuleManagement for a cleanup/dispose mechanism
        protected override void InitSecondPass()
        {
            EventBus<LevelLoadCompletedEvent>.AddListener(OnLevelLoadComplete);
        }

        private void OnLevelLoadComplete(object sender, LevelLoadCompletedEvent e)
        {
            DidHandleCompleteForCurrentLevel = false;
        }

        /// <summary>
        /// Call this to notify the framework that a level has been completed.
        /// </summary>
        /// <param name="isSuccess">Did the player win (true) or lose (false)?</param>
        /// <param name="score">Optional. The final score for this level. Set this if the game has a level-based score mechanism.</param>
        /// <param name="levelIndexOverride">Optional. If set, runs the method for the given level index instead of current.</param>
        /// <param name="virtualLevelIndexOverride">Optional. If set, runs the method for the given VIRTUAL level index instead of current.</param>
        /// <param name="canPerformSafetyChecks">Optional. Controls if we can perform safety checks.</param>
        /// <param name="canSetDidHandleCompleteForCurrentLevel">Optional. Controls if we can set the safety flag that prevents duplicate calls to this method for the current level.</param>
        /// <param name="canSetLastSuccessfulLevelIndex">Optional. Controls if we can set <see cref="PlayerPrefsGateway.LevelManagement.LastSuccessfulLevelIndex"/>.</param>
        /// <param name="canSetLastSuccessfulVirtualLevelIndex">Optional. Controls if we can set <see cref="PlayerPrefsGateway.LevelManagement.LastSuccessfulVirtualLevelIndex"/>.</param>
        /// <param name="canSetDidCompleteAtLeastOnce">Optional. Controls if we can set <see cref="PlayerPrefsGateway.LevelManagement.DidCompleteAtLeastOnce"/>.</param>
        /// <param name="canNotify">Optional. Controls if we can emit a <see cref="LevelCompletedEvent"/>.</param>
        /// <param name="canSetWasLatestCompleteASuccess">Optional. Controls if we can set <see cref="WasLatestCompleteASuccess"/>.</param>
        /// <remarks>
        /// Emits a <see cref="LevelCompletedEvent"/> if <paramref name="canNotify"/> is <c>true</c>.
        /// </remarks>
        [Button, FoldoutGroup(__InspectorGroupName_DevTools)]
        public void HandleLevelComplete(bool isSuccess,
            int score = 0,
            int? levelIndexOverride = null,
            int? virtualLevelIndexOverride = null,
            bool? canPerformSafetyChecks = null,
            bool canSetDidHandleCompleteForCurrentLevel = true,
            bool canSetLastSuccessfulLevelIndex = true,
            bool canSetLastSuccessfulVirtualLevelIndex = true,
            bool canSetDidCompleteAtLeastOnce = true,
            bool canSetWasLatestCompleteASuccess = true)
        {
            canPerformSafetyChecks ??= !Application.isEditor;
            if (canPerformSafetyChecks.Value)
            {
                if (!LevelServiceLocator.Instance.ActiveLoader.HasFirstLevelOfTheSessionLoaded)
                {
                    Debug.LogError($"{TypeName}: Cannot handle level complete, no levels are loaded yet!");
                    return;
                }

                if (DidHandleCompleteForCurrentLevel)
                {
                    Debug.LogWarning($"{TypeName}: Complete of level {CurrentLevelIndex} already handled! Ignoring.");
                    return;
                }
            }

            if (canSetWasLatestCompleteASuccess)
            {
                WasLatestCompleteASuccess = isSuccess;
            }

            var levelIndex = levelIndexOverride ?? CurrentLevelIndex;
            var virtualLevelIndex = virtualLevelIndexOverride ?? VirtualCurrentLevelIndex;

            if (canSetDidHandleCompleteForCurrentLevel && levelIndex == CurrentLevelIndex)
            {
                DidHandleCompleteForCurrentLevel = true;
            }

            if (isSuccess)
            {
                if (canSetLastSuccessfulLevelIndex)
                {
                    LastSuccessfulLevelIndex.Value = levelIndex;
                }

                if (canSetLastSuccessfulVirtualLevelIndex)
                {
                    LastSuccessfulVirtualLevelIndex.Value = virtualLevelIndex;
                }

                if (canSetDidCompleteAtLeastOnce && levelIndex == LevelCount - 1)
                {
                    // Redundant sets do not matter as this value will never revert back to false.
                    DidCompleteAtLeastOnce.Value = true;
                }
            }

            EventBus<LevelCompletedEvent>.Emit(this, new LevelCompletedEvent(levelIndex, virtualLevelIndex, isSuccess, score));
        }

#if UNITY_EDITOR
        [Button("Win Current Level"), FoldoutGroup(__InspectorGroupName_DevTools)]
        private void __Win(int score = 0)
        {
            HandleLevelComplete(true, score);
        }

        [Button("Lose Current Level"), FoldoutGroup(__InspectorGroupName_DevTools)]
        private void __Lose(int score = 0)
        {
            HandleLevelComplete(false, score);
        }
#endif // UNITY_EDITOR
    }
}
