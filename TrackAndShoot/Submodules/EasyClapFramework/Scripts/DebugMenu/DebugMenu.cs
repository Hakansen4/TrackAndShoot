using System;
using System.Collections.Generic;
using EasyClap.Seneca.Common.EventBus;
using EasyClap.Seneca.Common.LevelManagement;
using UnityEngine;
using UnityEngine.UI;

namespace EasyClap.Seneca.Common
{
    [RequireComponent(typeof(Canvas))]
    public class DebugMenu : MonoBehaviour
    {
        [SerializeField] protected Dropdown levelDropdown;
        [SerializeField] protected bool pauseGame;
        
        protected static bool uiEnabled = true;
        protected Canvas canvas;
        protected float oldTimeScale;

        protected virtual void Awake()
        {
            canvas = GetComponent<Canvas>();
            if (pauseGame)
            {
                oldTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            EventBus<DebugMenuOpenedEvent>.Emit(this, new DebugMenuOpenedEvent(this));
        }

        protected virtual void Start()
        {
            var levelCount = LevelServiceLocator.Instance.ActiveDatabase.LevelCount;
            var options = new List<Dropdown.OptionData>(levelCount);
            for (int i = 0; i < levelCount; i++)
            {
                options.Add(new Dropdown.OptionData((i + 1).ToString()));
            }

            levelDropdown.options = options;
        }

        public virtual void LoadNextLevel()
        {
            ProgressionManager.Instance.HandleLevelComplete(true);
            LevelServiceLocator.Instance.ActiveLoader.LoadNextLevel();
        }

        public virtual void ReloadCurrentLevel()
        {
            LevelServiceLocator.Instance.ActiveLoader.ReloadCurrentLevel();
        }
        
        public virtual void LoadLevelWithIndex()
        {
            if (levelDropdown.options.Count == 0)
            {
                return;
            }
            
            var index = Int32.Parse(levelDropdown.options[levelDropdown.value].text) - 1;
            LevelServiceLocator.Instance.ActiveLoader.LoadLevelWithIndex(index);
        }

        public virtual void ToggleUI()
        {
            uiEnabled = !uiEnabled;
            
            foreach (var canvas in FindObjectsOfType<Canvas>())
            {
                if (canvas == this.canvas)
                {
                    continue;
                }
                
                canvas.enabled = uiEnabled;
            }
        }

        public virtual void DeleteSaves()
        {
            PlayerPrefs.DeleteAll();
        }
        
        public virtual void CloseDebugMenu()
        {
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (pauseGame)
            {
                Time.timeScale = oldTimeScale;
            }
            EventBus<DebugMenuClosedEvent>.Emit(this, new DebugMenuClosedEvent(this));
        }
    }
}
