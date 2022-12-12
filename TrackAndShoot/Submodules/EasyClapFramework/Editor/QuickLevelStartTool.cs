using EasyClap.Seneca.Common.Core;
using EasyClap.Seneca.Common.Editor;
using EasyClap.Seneca.Common.PlayerPrefsUtils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EasyClap.Seneca.Editor
{
    public class QuickLevelStartTool : EditorWindow
    {
        [MenuItem("Easy Clap/Utility/Quick Level Start...")]
        private static void CreateEditorWindow()
        {
            GetWindow<QuickLevelStartTool>("Quick Level Start", true);
        }

        // ReSharper disable InconsistentNaming
        private const string TypeName = nameof(QuickLevelStartTool);
        private const string EditorPrefsKey_StateAsJson = TypeName + "_JsonState";

        [SerializeField] private bool lastSuccessfulLevelIndex_HasValue_Memento = false;
        [SerializeField] private int lastSuccessfulLevelIndex_Value_Memento = -1;
        [SerializeField] private bool didCompleteAtLeastOnce_HasValue_Memento = false;
        [SerializeField] private bool didCompleteAtLeastOnce_Value_Memento = false;

        [SerializeField] private bool shouldRestorePlayerPrefs_Flag = false;
        // ReSharper restore InconsistentNaming

        [SerializeField] private SceneAsset levelLoaderScene;
        [SerializeField] private int levelIndex;
        [SerializeField] private bool restorePlayerPrefsOnPlaymodeExit;

        private static PlayerPrefsEntryInt LastSuccessfulLevelIndex => PlayerPrefsGateway.LevelManagement.LastSuccessfulLevelIndex;
        private static PlayerPrefsEntryBool DidCompleteAtLeastOnce => PlayerPrefsGateway.LevelManagement.DidCompleteAtLeastOnce;

        void OnEnable()
        {
            RecoverStateFromEditorPrefs();
            EditorApplication.playModeStateChanged += EditorApplication_OnPlayModeStateChanged;
        }

        void OnDisable()
        {
            SaveStateToEditorPrefs();
            EditorApplication.playModeStateChanged -= EditorApplication_OnPlayModeStateChanged;
        }

        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("This toll will enter playmode with the given scene, while also making sure we load the level with the given index.", MessageType.None);
            EditorGUILayout.HelpBox("IF YOU WISH TO RESTORE PLAYERPREFS AFTER PLAYMODE EXIT, KEEP THE WINDOW OPEN UNTIL AFTER PLAYMODE EXIT.", MessageType.Warning);
            SenecaEditorUtility.DrawLine();
            
            levelLoaderScene = EditorGUILayout.ObjectField(nameof(levelLoaderScene).Nicify(), levelLoaderScene, typeof(SceneAsset), false) as SceneAsset;
            levelIndex = EditorGUILayout.IntField(nameof(levelIndex).Nicify(), levelIndex);

            restorePlayerPrefsOnPlaymodeExit = EditorGUILayout.ToggleLeft(nameof(restorePlayerPrefsOnPlaymodeExit).Nicify(), restorePlayerPrefsOnPlaymodeExit);

            EditorGUILayout.Space();

            if (!levelLoaderScene)
            {
                EditorGUILayout.HelpBox($"No {nameof(levelLoaderScene).Nicify()} selected.", MessageType.Info);
                return;
            }
            
            if (levelIndex < 0)
            {
                EditorGUILayout.HelpBox("Level index cannot be negative.", MessageType.Info);
                return;
            }

            if (GUILayout.Button($"Load level with index {levelIndex} together with the level loader with name {levelLoaderScene.name}"))
            {
                var confirmation = SenecaEditorUtility.Confirm($"Do you want to load level with index {levelIndex} together with the level loader with name {levelLoaderScene.name}?");

                if (confirmation)
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

                    if (restorePlayerPrefsOnPlaymodeExit)
                    {
                        shouldRestorePlayerPrefs_Flag = true;
                        RememberPlayerPrefs();
                    }

                    LastSuccessfulLevelIndex.Value = levelIndex - 1;
                    DidCompleteAtLeastOnce.Value = false;

                    EditorSceneManager.playModeStartScene = levelLoaderScene;
                    EditorApplication.EnterPlaymode();
                }
            }
        }

        private void EditorApplication_OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.EnteredEditMode)
            {
                EditorSceneManager.playModeStartScene = null;
            }
            else if (change == PlayModeStateChange.ExitingPlayMode)
            {
                if (shouldRestorePlayerPrefs_Flag)
                {
                    shouldRestorePlayerPrefs_Flag = false;
                    RestorePlayerPrefs();
                }
            }
        }

        private void RememberPlayerPrefs()
        {
            lastSuccessfulLevelIndex_Value_Memento = LastSuccessfulLevelIndex.Value;
            lastSuccessfulLevelIndex_HasValue_Memento = LastSuccessfulLevelIndex.HasValue;

            didCompleteAtLeastOnce_Value_Memento = DidCompleteAtLeastOnce.Value;
            didCompleteAtLeastOnce_HasValue_Memento = DidCompleteAtLeastOnce.HasValue;
        }

        private void RestorePlayerPrefs()
        {
            if (lastSuccessfulLevelIndex_HasValue_Memento)
            {
                LastSuccessfulLevelIndex.Value = lastSuccessfulLevelIndex_Value_Memento;
            }
            else
            {
                LastSuccessfulLevelIndex.Delete();
            }

            if (didCompleteAtLeastOnce_HasValue_Memento)
            {
                DidCompleteAtLeastOnce.Value = didCompleteAtLeastOnce_Value_Memento;
            }
            else
            {
                DidCompleteAtLeastOnce.Delete();
            }
        }

        private void RecoverStateFromEditorPrefs()
        {
            var dataJson = EditorPrefs.GetString(EditorPrefsKey_StateAsJson, null);
            JsonUtility.FromJsonOverwrite(dataJson ?? JsonUtility.ToJson(this, false), this);
        }

        private void SaveStateToEditorPrefs()
        {
            var dataJson = JsonUtility.ToJson(this, false);
            EditorPrefs.SetString(EditorPrefsKey_StateAsJson, dataJson);
        }
    }
}