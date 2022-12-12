using System;
using UnityEngine;
using UnityEngine.Events;

namespace EasyClap.Seneca.Common.MenuManagement
{
    [RequireComponent(typeof(Canvas))]
    public abstract class Menu<T> : Menu where T : Menu<T>
    {
        public UnityEvent<T> OpenEvent;
        public UnityEvent<T> CloseEvent;

        #region Static

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<T>(true);
                }

                return _instance;
            }
        }
        
        public static void Open()
        {
            MenuManager.Instance.OpenMenu(Instance);
        }
        
        public static void Close()
        {
            if (MenuManager.Instance)
            {
                MenuManager.Instance.CloseMenu(Instance);
            }
        }

        #endregion

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this as T;
        }

        private void OnDestroy()
        {
            OpenEvent.RemoveAllListeners();
            CloseEvent.RemoveAllListeners();
        }

        public override void OnOpen()
        {
            OpenEvent?.Invoke(this as T);
        }

        public override void OnClose()
        {
            CloseEvent?.Invoke(this as T);
        }
    }
    
    public abstract class Menu : MonoBehaviour
    {
        public abstract void OnOpen();
        public abstract void OnClose();
    }
}
