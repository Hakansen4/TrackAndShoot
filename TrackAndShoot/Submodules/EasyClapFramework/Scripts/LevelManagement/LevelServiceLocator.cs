using System;
using EasyClap.Seneca.Common.Core;
using EasyClap.Seneca.Common.LevelManagement.Databases;
using EasyClap.Seneca.Common.LevelManagement.Loaders;
using EasyClap.Seneca.Common.ModuleManagement;
using EasyClap.Seneca.Common.PlayerPrefsUtils;
using EasyClap.Seneca.Common.Utility;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.LevelManagement
{
    /// <summary>
    /// A service locator that provides non-discriminated access to the active level management services.
    /// </summary>
    [PublicAPI]
    [CreateAssetMenu(menuName = MenuName, fileName = TypeName)]
    public class LevelServiceLocator : SingletonModule<LevelServiceLocator>
    {
        // ReSharper disable InconsistentNaming
        private const string __Inspector_GroupName_DevTools = "Level Management Development Tools";
        // ReSharper restore InconsistentNaming

        private const string TypeName = nameof(LevelServiceLocator);
        private const string MenuName = Core.Constants.EasyClap + "/" + TypeName;

        /// <summary>
        /// The active level loader.
        /// </summary>
        [field: NonSerialized]
        public ILevelLoader ActiveLoader { get; private set; }

        /// <summary>
        /// The active level database.
        /// </summary>
        [field: NonSerialized]
        public ILevelDatabase ActiveDatabase { get; private set; }

        /// <summary>
        /// The active progression manager.
        /// </summary>
        [field: NonSerialized]
        public ProgressionManager ActiveProgressionManager { get; private set; }

        /// <summary>
        /// Sets <see cref="ActiveLoader"/>.
        /// </summary>
        /// <exception cref="SenecaCommonException">Throws if <see cref="ActiveLoader"/> is already set to a <b>different</b> instance.</exception>
        internal void RegisterActiveLoader(ILevelLoader levelLoader)
        {
            if (ActiveLoader == levelLoader)
            {
                Debug.LogWarning($"{TypeName}: The same level loader is trying to register itself again. Ignoring.");
                return;
            }

            if (ActiveLoader != null)
            {
                throw new SenecaCommonException($"{TypeName}: Multiple level loaders detected! Check the module registry and make sure there is only one level loader registered.");
            }

            Debug.Log($"{TypeName}: Registering level loader ({levelLoader.GetType().GetFormattedName()})");
            ActiveLoader = levelLoader;
        }

        /// <summary>
        /// Sets <see cref="ActiveDatabase"/>.
        /// </summary>
        /// <exception cref="SenecaCommonException">Throws if <see cref="ActiveDatabase"/> is already set to a <b>different</b> instance.</exception>
        internal void RegisterActiveDatabase(ILevelDatabase levelDatabase)
        {
            if (ActiveDatabase == levelDatabase)
            {
                Debug.LogWarning($"{TypeName}: The same level database is trying to register itself again. Ignoring.");
                return;
            }

            if (ActiveDatabase != null)
            {
                throw new SenecaCommonException($"{TypeName}: Multiple level databases detected! Check the module registry and make sure there is only one level database registered.");
            }

            Debug.Log($"{TypeName}: Registering level database ({levelDatabase.GetType().GetFormattedName()})");
            ActiveDatabase = levelDatabase;
        }

        /// <summary>
        /// Sets <see cref="ActiveProgressionManager"/>.
        /// </summary>
        /// <exception cref="SenecaCommonException">Throws if <see cref="ActiveProgressionManager"/> is already set to a <b>different</b> instance.</exception>
        internal void RegisterActiveProgressionManager(ProgressionManager progressionManager)
        {
            if (ActiveProgressionManager == progressionManager)
            {
                Debug.LogWarning($"{TypeName}: The same progression manager is trying to register itself again. Ignoring.");
                return;
            }

            if (ActiveProgressionManager != null)
            {
                throw new SenecaCommonException($"{TypeName}: Multiple progression managers detected! Check the module registry and make sure there is only one progression manager registered.");
            }

            Debug.Log($"{TypeName}: Registering progression manager ({progressionManager.GetType().GetFormattedName()})");
            ActiveProgressionManager = progressionManager;
        }

#if UNITY_EDITOR
        // ReSharper disable InconsistentNaming
        /// !!! INSPECTOR UTILITY, DO NOT USE IN CODE !!!
        [InfoBox("These properties are for display only. Do NOT use the object pickers.", InfoMessageType.Warning)]
        [ShowInInspector, LabelText("Active Loader"), FoldoutGroup(__Inspector_GroupName_DevTools)]
        [HideReferenceObjectPicker, EnableGUI, ReadOnly]
        private ILevelLoader __Inspector_Utility_ActiveLoader => ActiveLoader;

        /// !!! INSPECTOR UTILITY, DO NOT USE IN CODE !!!
        [ShowInInspector, LabelText("Active Database"), FoldoutGroup(__Inspector_GroupName_DevTools)]
        [HideReferenceObjectPicker, EnableGUI, ReadOnly]
        private ILevelDatabase __Inspector_Utility_ActiveDatabase => ActiveDatabase;

        /// !!! INSPECTOR UTILITY, DO NOT USE IN CODE !!!
        [ShowInInspector, LabelText("Active Progression Manager"), FoldoutGroup(__Inspector_GroupName_DevTools)]
        [HideReferenceObjectPicker, EnableGUI, ReadOnly]
        private ProgressionManager __Inspector_Utility_ActiveProgressionManager => ActiveProgressionManager;

        /// !!! INSPECTOR UTILITY, DO NOT USE IN CODE !!!
        [ShowInInspector, LabelText("Did Complete At Least Once"), FoldoutGroup(__Inspector_GroupName_DevTools)]
        [HideReferenceObjectPicker, EnableGUI, ReadOnly]
        private PlayerPrefsEntryBool __Inspector_Utility_DidCompleteAtLeastOnce => PlayerPrefsGateway.LevelManagement.DidCompleteAtLeastOnce;

        /// !!! INSPECTOR UTILITY, DO NOT USE IN CODE !!!
        [ShowInInspector, LabelText("Last Successful Level Index"), FoldoutGroup(__Inspector_GroupName_DevTools)]
        [HideReferenceObjectPicker, EnableGUI, ReadOnly]
        private PlayerPrefsEntryInt __Inspector_Utility_LastSuccessfulLevelIndex => PlayerPrefsGateway.LevelManagement.LastSuccessfulLevelIndex;

        /// !!! INSPECTOR UTILITY, DO NOT USE IN CODE !!!
        [ShowInInspector, LabelText("Last Successful VIRTUAL Level Index"), FoldoutGroup(__Inspector_GroupName_DevTools)]
        [HideReferenceObjectPicker, EnableGUI, ReadOnly]
        private PlayerPrefsEntryInt __Inspector_Utility_LastSuccessfulVirtualLevelIndex => PlayerPrefsGateway.LevelManagement.LastSuccessfulVirtualLevelIndex;

        /// !!! INSPECTOR UTILITY, DO NOT USE IN CODE !!!
        [Button, LabelText("Win Level"), FoldoutGroup(__Inspector_GroupName_DevTools), PropertySpace(SpaceBefore = 16)]
        private void __Inspector_Utility_WinLevel() => ProgressionManager.Instance.HandleLevelComplete(true);

        /// !!! INSPECTOR UTILITY, DO NOT USE IN CODE !!!
        [Button, LabelText("Lose Level"), FoldoutGroup(__Inspector_GroupName_DevTools)]
        private void __Inspector_Utility_LoseLevel() => ProgressionManager.Instance.HandleLevelComplete(false);

        /// !!! INSPECTOR UTILITY, DO NOT USE IN CODE !!!
        [Button, LabelText("Load Next Level"), FoldoutGroup(__Inspector_GroupName_DevTools)]
        private void __Inspector_Utility_LoadNextLevel() => ActiveLoader.LoadNextLevel();

        /// !!! INSPECTOR UTILITY, DO NOT USE IN CODE !!!
        [Button, LabelText("Reload Current Level"), FoldoutGroup(__Inspector_GroupName_DevTools)]
        private void __Inspector_Utility_ReloadCurrentLevel() => ActiveLoader.ReloadCurrentLevel();
        // ReSharper restore InconsistentNaming
#endif // UNITY_EDITOR
    }
}
